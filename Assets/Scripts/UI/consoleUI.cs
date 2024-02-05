using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class consoleUI : MonoBehaviour
{
    public consoleUI instance;
    public static bool addDebug = false;

    protected static Text consoleText;
    protected static bool isDisplayed = true;
    protected static CanvasGroup UIinstance;

    // called on object initialization
    void Awake()
    {
        // we only need one console UI, so keep it static and have a record of it on any scene
        Debug.Log("ConsoleUI instance awake");
        if (instance != null)
        {
            Debug.Log("ConsoleUI instance is active: " + instance);
            Destroy(gameObject);
            instance = this; // replace current instance with new one
        }
        else
        {
            Debug.Log("ConsoleUI instance not found. Registering instance. ");
            instance = this;
            DontDestroyOnLoad(gameObject); // this object persists over different scenes
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        consoleText = GameObject.Find("consoleText").GetComponent<Text>();
        UIinstance = GetComponent<CanvasGroup>();
    }

    // Log behavior starts here
    public static void Log (string text) {
        consoleText.text += Time.fixedTime + ": " + text + "\n";
        if (addDebug) Debug.Log(text);
    }

    // Update is called once per frame
    void Update()
    {
        // show console
        if(Input.GetKeyDown("`")) toggleConsole();
        if(Input.GetKey("escape")) Application.Quit();
    }

    protected void toggleConsole() {
        if (isDisplayed) {
            isDisplayed = false;
            UIinstance.alpha = 0;            
        }
        else {
            isDisplayed = true;
            UIinstance.alpha = 1;
        }
    }
}
