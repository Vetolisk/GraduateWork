using Pocketsphinx;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SphinxManager : MonoBehaviour
{
    public enum language
    {
        en_US,
        ru_RU
    }
    public enum decoderMode
    {
        KWS,
        LM
    }

    private Decoder d; // Decoder that's actually interpreting our speech.
    public MicrophoneHandler mic; // Handler that manages the recording of our microphone.

    [Header("Configuration:")]
    public language lang = language.ru_RU; // The language model you wish to use.
    public decoderMode mode = decoderMode.KWS;
    public int maxRecordingTime = 2; // Maximum length of the AudioClip recorded for your microphone.
    public float minRecordingTime = 1; // Minimum length of the AudioClip recorded for your microphone.
    public float micLoudnessTreshold = -60; // Loudness of silence
    public double kwsThreshold = 1e-26;
    public bool debugMode = false;

    private string keywordsFile = "keywords.txt";
    private string customDictFile = "custom.dic";
    private float recordingStartTime;
    private SphinxListener[] listeners;
    [System.NonSerialized]
    public bool ready = false;

    public delegate void SpeechRecognizedHandler(string phrase);
    public event SpeechRecognizedHandler OnSpeechRecognized;

    ThreadedProcessRaw pr = null;
    private IEnumerator Start()
    {
        #if !UNITY_ANDROID || UNITY_EDITOR
        yield return DeleteCache();
        #endif
        //if (!streamingAssetsAlreadyExists())
            yield return loadStreamingAssets();

        yield return WaitForMicrophoneInput();

        if (mode == decoderMode.KWS)
        {
            CollectListenersData();
            SetupDecoderKWS();
        }
        else SetupDecoderLM();

        SetupMicrophone();
    }

    private void Update()
    {
        if (!ready) return;

        //это вторая часть костыля из ProcessAudio
        if (pr != null)
        {
            if (pr.Update())
            {
                if (d.Hyp() != null)
                {
                    if (OnSpeechRecognized != null)
                    {
                        OnSpeechRecognized.Invoke(d.Hyp().Hypstr);
                    }
                    
                    d.EndUtt();
                    d.StartUtt();
                }
                else
                {
                    Wolf.flagError = true;
                }
                pr = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!Microphone.IsRecording(mic.Name))
            {
                mic.BeginRecording();
                recordingStartTime = Time.time;
            }
            //else if (silenceOrTimeIsOut())
            //{
            //    mic.EndRecording();
            //}
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            mic.EndRecording();
        }
    }
    private bool streamingAssetsAlreadyExists()
    {
        return System.IO.Directory.Exists(
            Path.Combine(Application.persistentDataPath, lang.ToString("g"))
            );
    }
    private bool silenceOrTimeIsOut()
    {
        return (Time.time - recordingStartTime > minRecordingTime && mic.LevelMax() < micLoudnessTreshold) || (Time.time - recordingStartTime >= maxRecordingTime);
    }
    private static int CompareDictonaryLines(string x, string y)
    {
        return x.CompareTo(y);
    }
    private void CollectListenersData()
    {
        string customDicPath = Path.Combine(Application.temporaryCachePath, customDictFile);
        string keyphrasePath = Path.Combine(Application.temporaryCachePath, keywordsFile);

        List<string> dicList = new List<string>();
        List<string> kwsList = new List<string>();

        listeners = GameObject.FindObjectsOfType<SphinxListener>();

        foreach (SphinxListener lis in listeners)
        {
            if (lis.lang.ToString("g") != lang.ToString("g")) continue;
            foreach (SphinxListener.keyword word in lis.keywords)
            {
                if (word.Trancription != string.Empty)
                {
                    dicList.Add(word.Word + " " + word.Trancription);
                    kwsList.Add(word.Word);
                }
            }
        }

        dicList.Sort(CompareDictonaryLines);
        kwsList.Sort(CompareDictonaryLines);

        StreamWriter writer = new StreamWriter(customDicPath, true);
        foreach (string line in dicList) writer.WriteLine(line);
        writer.Close();

        writer = new StreamWriter(keyphrasePath, true);
        foreach (string line in kwsList) writer.WriteLine(line);
        writer.Close();

    }
    public void ReinitDecoderKWS()
    {
        string keyphrasePath = Path.Combine(Application.temporaryCachePath, keywordsFile);
        string customDicPath = Path.Combine(Application.temporaryCachePath, customDictFile);

        Config cfg = d.GetConfig();

        cfg.SetString("-kws", keyphrasePath);
        cfg.SetString("-dict", customDicPath);

        d.Reinit(cfg);
        d.StartUtt();

        if (debugMode) Debug.Log("<color=green><b>Decoder reinitialized!</b></color>");
    }
    private IEnumerator WaitForMicrophoneInput()
    {
        while (Microphone.devices.Length <= 0)
            yield return null;
    }
    private void SetupMicrophone()
    {
        mic = new MicrophoneHandler(Microphone.devices[0], MicrophoneHandler.SamplingRateEnum.SixteenK, maxRecordingTime);
        mic.RecordingFinished += ProcessAudio;
    }
    private void SetupDecoderKWS()
    {
        if (debugMode) Debug.Log("<color=yellow>Initializing decoder...</color>");

        string speechDataPath = Path.Combine(Application.persistentDataPath, lang.ToString("g"));
        string dictPath = Path.Combine(Application.temporaryCachePath, customDictFile);
        string keyphrasePath = Path.Combine(Application.temporaryCachePath, keywordsFile);

        Config c = Decoder.DefaultConfig();

        c.SetString("-hmm", speechDataPath);
        c.SetString("-dict", dictPath);
        c.SetString("-kws", keyphrasePath);
        c.SetFloat("-kws_threshold", kwsThreshold);

        d = new Decoder(c);
        d.StartUtt();
        ready = true;

        if (debugMode) Debug.Log("<color=green><b>Decoder initialized!</b></color>");
    }
    private void SetupDecoderLM()
    {
        if (debugMode) Debug.Log("<color=yellow>Initializing decoder...</color>");

        string speechDataPath = Path.Combine(Application.persistentDataPath, lang.ToString("g"));
        string dictPath = Path.Combine(speechDataPath, lang + ".dic");
        string lmPath = Path.Combine(speechDataPath, lang + ".lm.bin");

        Config c = Decoder.DefaultConfig();
        c.SetString("-hmm", speechDataPath);
        c.SetString("-dict", dictPath);
        c.SetFloat("-kws_threshold", kwsThreshold);

        d = new Decoder(c);
        d.SetLmFile("lm", lmPath);
        d.SetSearch("lm");
        d.StartUtt();

        if (debugMode) Debug.Log("<color=green><b>Decoder initialized!</b></color>");
    }
    private void ProcessAudio(AudioClip audio)
    {
        var newData = new float[audio.samples * audio.channels];
        audio.GetData(newData, 0);
        byte[] byteData = ConvertToBytes(newData, audio.channels);

        //это первая часть костыля, вторая в Update()
        if (pr == null)
        {
            pr = new ThreadedProcessRaw();
            pr.d = d;
            pr.byteData = byteData;
            pr.Start();
        }

        //это старый код поиска речи в записанном аудио
        //он синхронный, поэтому вызывал зависание игры на время выполнения
        //я так и не понял сделал ли я лучше асинхронным костылем,
        //так что оставил тут и оригинал
        /*d.ProcessRaw(byteData, byteData.Length, false, false);

        if (d.Hyp() != null)
        {
            if (OnSpeechRecognized != null)
                OnSpeechRecognized.Invoke(d.Hyp().Hypstr);
            d.EndUtt();
            d.StartUtt();
        }*/
    }
    public IEnumerator loadStreamingAssets()
    {
        if (debugMode) Debug.Log("<color=yellow><b>Loading Streaming Assets...</b></color>");

        string sourcePath = Path.Combine(Application.streamingAssetsPath, lang.ToString("g"));
        string targetPath = Path.Combine(Application.persistentDataPath, lang.ToString("g"));
        string assetsListPath = Path.Combine(sourcePath, "assets.lst");

        string[] assetsList;

        if (!System.IO.Directory.Exists(targetPath))
            System.IO.Directory.CreateDirectory(targetPath);
        if (sourcePath.Contains("://") || sourcePath.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(assetsListPath);
            yield return www.SendWebRequest();
            assetsList = www.downloadHandler.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            foreach (string assetName in assetsList)
            {
                string correctAssetName = string.Concat(assetName.Split(Path.GetInvalidFileNameChars()));
                string assetSourcePath = Path.Combine(sourcePath, correctAssetName);
                string assetTargetPath = Path.Combine(targetPath, correctAssetName);
                www = UnityEngine.Networking.UnityWebRequest.Get(assetSourcePath);
                yield return www.SendWebRequest();
                File.WriteAllBytes(assetTargetPath, www.downloadHandler.data);
            }
        }
        else
        {
            assetsList = System.IO.File.ReadAllText(assetsListPath).Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            foreach (string assetName in assetsList)
            {
                string assetSourcePath = Path.Combine(sourcePath, assetName.ToString());
                string assetTargetPath = Path.Combine(targetPath, assetName.ToString());
                File.Copy(assetSourcePath, assetTargetPath, true);
            }
        }

        if (debugMode) Debug.Log("<color=green><b>Streaming Assets loaded!</b></color>");
    }
    private static byte[] ConvertToBytes(float[] data, int channels)
    {
        float tot = 0;
        byte[] byteData = new byte[data.Length / channels * 2];
        for (int i = 0; i < data.Length / channels; i++)
        {
            float sum = 0;
            for (int j = 0; j < channels; j++)
            {
                sum += data[i * channels + j];
            }
            tot += sum * sum;
            short val = (short)(sum / channels * 20000); // volume
            byteData[2 * i] = (byte)(val & 0xff);
            byteData[2 * i + 1] = (byte)(val >> 8);
        }
        return byteData;
    }
    public IEnumerator DeleteCache()
    {
        string path = Application.temporaryCachePath;

        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);

        foreach (System.IO.FileInfo file in di.GetFiles())
        {
            try { file.Delete(); }
            catch { }
        }

        foreach (System.IO.DirectoryInfo dir in di.GetDirectories())
        {
            try { dir.Delete(true); }
            catch { }
        }

        yield return null;

        if (debugMode) Debug.Log("<color=green><b>Cache cleaned!</b></color>");
    }
}
public class ThreadedProcessRaw
{
    private bool m_IsDone = false;
    private object m_Handle = new object();
    private System.Threading.Thread m_Thread = null;

    public byte[] byteData;
    public Decoder d;
    public bool IsDone
    {
        get
        {
            bool tmp;
            lock (m_Handle)
            {
                tmp = m_IsDone;
            }
            return tmp;
        }
        set
        {
            lock (m_Handle)
            {
                m_IsDone = value;
            }
        }
    }

    public virtual void Start()
    {
        m_Thread = new System.Threading.Thread(Run);
        m_Thread.Start();
    }
    public virtual void Abort()
    {
        m_Thread.Abort();
    }

    protected virtual void ThreadFunction()
    {
        d.ProcessRaw(byteData, byteData.Length, false, false);
    }

    protected virtual void OnFinished() { }

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinished();
            return true;
        }
        return false;
    }
    public IEnumerator WaitFor()
    {
        while (!Update())
        {
            yield return null;
        }
    }
    private void Run()
    {
        ThreadFunction();
        IsDone = true;
    }
}