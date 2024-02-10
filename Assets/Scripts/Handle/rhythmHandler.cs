using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Also handles in-game soundtrack
public class RhythmHandler : Handler
{
    public static RhythmHandler instance; // holds the ONLY instance of the object
    
    private const float defaultBeatInterval = 1, defaultBPM = 60; // BPM in seconds
    private const float defaultTapPercent = 0.8f; // percentage of tap interval for each beat
    public static bool isInBeat = false; // what the player will check when doing certain actions
    public static float BPM = 60;
    private static bool isPlayingBeat = false, pauseBeat = false;
    private static bool tapSuccess = false;
    private static float beatInterval = 1, tapGate = 0.5f, tapBarSpeed, tapBarLimit; // in seconds
    private static int beatCounter = 0;

    private Slider tapBar;
    private bool isLooping = true, isPlaying = false, isPaused = false, animateTapBarUI = false;
    

    [SerializeField]
    protected AudioClip metronome;
    public AudioClip[] gameTracks;
    
    [SerializeField]
    protected AudioSource beatTrack;
    [SerializeField]
    protected AudioSource currentTrack; 

    //BGM tracks in this array
    
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
        // Debug.Log(tapBarSpeed);
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
        tapGate = (beatInterval * defaultTapPercent); // time to tap is based on defaultTapPercent percentage of each beat's length
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

    // static function for playing tracks
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
            StartCoroutine(generateBeats());
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
            tapSuccess = false;
            if (tapBar != null) tapBar.value = 0;
            // Debug.Log("Beat: " + beatCounter);
            Invoke("closeTapGate", tapGate); // reset tapSuccess back to false after a certain time;
            yield return new WaitForSeconds(beatInterval);
        }
    }

    protected void closeTapGate() {
        consoleUI.Log("Tap Now!");
        tapSuccess = true;
    }

    protected void animateTapBar() {
        if (tapBar != null) tapBar.value += tapBarSpeed;
    }
}
