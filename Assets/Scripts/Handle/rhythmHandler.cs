using System.Collections;
using System.Collections.Generic;
using System.Linq; // required to allow queue structures to do averages
using UnityEngine;
using UnityEngine.UI;

// Also handles in-game soundtrack
public class RhythmHandler : Handler
{
    public static RhythmHandler instance; // holds the ONLY instance of the object
    
    private const float defaultBeatInterval = 1, defaultBPM = 60; // BPM in seconds
    private const float defaultTapPercent = 0.7f; // percentage of tap interval for each beat. Calculated as (1 - n) * 100, e.g. 0.4 = 60%
    public static bool isInBeat = false; // what the player will check when doing certain actions
    public static float BPM = 60;
    private static bool isPlayingBeat = false, pauseBeat = false;
    private static bool tapSuccess = false;
    private static float beatInterval = 1, tapGate = 0.5f, tapBarSpeed, tapBarLimit; // in seconds
    private static int beatCounter = 0;

    private Slider tapBar;
    private bool isLooping = true, isPlaying = false, isPaused = false, animateTapBarUI = false;

    // beat detection components:
    protected float[] samples = new float[1024];
    protected float[] spectrum = new float[1024];
    protected float beatThreshold = 5f;   // adjust this value lower to detect lower (and more) beat impacts
    protected float beatFrameData = 20;   // Number of audio frames to examine for beat detection. Higher value = lower sensitivity (and lower beat detection). Default at 30
    protected float currentBeatTime = 0, lastBeatTime = 0;
    Queue<float> beatQueue = new Queue<float>();
    

    [SerializeField]
    protected AudioClip metronome;
    
    [SerializeField]
    protected AudioSource beatTrack;
    [SerializeField]
    protected AudioSource currentTrack; 

    //BGM tracks in this array
    public AudioClip[] gameTracks;
    
    // Called on object initialization. Great for preventing multiple instances of an object
    void Awake()
    {
        Debug.Log("rhythmHandler instance awake");
        if (instance != null)
        {
            Debug.Log("Rhythmhandler instance is active: " + instance);
            Destroy(gameObject);
            // instance = this; // replace current instance with new one
        }
        else
        {
            Debug.Log("Rhythmhandler instance not found. Registering instance. ");
            instance = this;
            DontDestroyOnLoad(gameObject); // this object persists over different scenes
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // tapBarSpeed = (beatInterval * 100);
        // assign the metronome bar UI slider if it's empty
        if (tapBar == null) {
            tapBar = GameObject.Find("metronomeUI").GetComponent<Slider>();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // animate the tapBarUI if enabled
        if (animateTapBarUI) animateTapBar();
        if (isPlayingBeat) detectBeat();

    }

    public static bool checkTap() { return tapSuccess; } // this will be the checker for tap validity
    public static void setBPM(float bpm) {
        BPM = bpm;
        
        // Important: Instead of adjusting the speed at how the slider "falls",
        // It's easier to adjust the slider's max value instead 
        // and move the slider at a consistent rate using beatInterval
        // since Time.fixedUnscaledDeltaTime is defaulted to 0.02 seconds per frame
        beatInterval = 60 / BPM; // divide the BPM to get the beatInterval
        tapBarLimit = (beatInterval / Time.fixedUnscaledDeltaTime);
        tapBarSpeed = (100 * Time.fixedUnscaledDeltaTime) / beatInterval; 
        tapGate = (beatInterval * defaultTapPercent); // time to tap is based on defaultTapPercent percentage of each beat's length. Multiply by 4 to simulate 4/4
        Debug.Log("Interval: " + beatInterval + " - tapBarLimit: " + tapBarLimit + " - tapBarSpeed: " + tapBarSpeed + " - tapGate: " + tapGate);
        consoleUI.Log("new BPM: " + BPM + " - new TapGate: " + tapGate);
    }
    public static void resetBeat() {
        beatInterval = defaultBeatInterval;
    }

    // Metronome functions here
    public void playMetronome() {
        consoleUI.Log("Start Metronome");
        if (!isPlayingBeat) {

            // set up the beat current and last times:
            currentBeatTime = 0;
            lastBeatTime = Time.time;
            tapBarSpeed = 100 / (lastBeatTime);

            beatTrack.Play();
            startPlayBeats(true);
            isPlayingBeat = true;
        }
    }

    public void stopMetronome() {
        if (isPlayingBeat) {
            beatTrack.Stop();
            startPlayBeats(false);
            isPlayingBeat = false;
            animateTapBarUI = false;
        }
    }

    // function for playing tracks
    public void playTrack(int ID) {
        if (ID < gameTracks.Length) {
            consoleUI.Log("Start BGM: " + ID);
            if (isPlaying) currentTrack.Stop();
            currentTrack.clip = gameTracks[ID];
            currentTrack.Play();
            isPlaying = true;
        }
    }

    public void startPlayBeats(bool startBeats) {
        if (!isPlayingBeat) {
            isPlayingBeat = true;
            beatCounter = 0;
            animateTapBarUI = true;
            // StartCoroutine(generateBeats());
        }
    }

    // Beat detection algorithm starts here    
    private void detectBeat() {
        float averageBeat, beatDuration = 0, tapGateTime;
        float beat = 0;

        beatTrack.GetOutputData(samples, 0);
        
        for (int i = 0; i < samples.Length; i++)
        {
            beat += samples[i] * samples[i];
        }

        if (beatQueue.Count > beatFrameData) beatQueue.Dequeue(); 
        beatQueue.Enqueue(beat); // add current beat into beatQueue

        averageBeat = beatQueue.Average();
        if (beat > (averageBeat * beatThreshold))
        {
            // prevent tapSuccess from being called multiple times in a frame
            if (!tapSuccess) { 
                
                // set up the beat current and last times:
                currentBeatTime = Time.time;
                if (lastBeatTime > 0) {
                    beatDuration = currentBeatTime - lastBeatTime;
                    tapBarSpeed = 100 / beatDuration;
                    tapGate = beatDuration * defaultTapPercent;
                    Debug.Log("beatDuration: " + beatDuration + " - tapBarSpeed: " + tapBarSpeed + " - tapGate: " + tapGate);
                }
                lastBeatTime = currentBeatTime;
                
                if (tapGate > 0) {
                    beatCounter++;
                    consoleUI.Log("TAP NOW " + beatCounter);
                    tapSuccess = true;
                    if (tapBar != null) tapBar.value = 0;
                    Invoke("closeTapGate", tapGate); // reset tapSuccess back to false after a certain time;
                }
            }
            
            
        }
    }


    public static void stopPlayBeats() {

    }

    // IEnumerator call for the beat generator
    protected IEnumerator generateBeats()
    {
        while(isPlayingBeat)
        {
            beatCounter++;
            yield return new WaitForSeconds(beatInterval);
        }
    }

    protected void closeTapGate() {
        // consoleUI.Log("Tap Now!");
        tapSuccess = false;
    }

    protected void animateTapBar() {
        if (tapBar != null) tapBar.value += (tapBarSpeed * Time.deltaTime);
    }
}
