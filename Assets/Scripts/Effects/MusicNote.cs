using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : Effects
{
    private Animator anim;

    [SerializeField]
    protected AudioSource SFX;

    // Called on script load
    void Awake()
    {
        anim = GetComponent<Animator>();   
    }

    // Start is called before the first frame update
    void Start()
    {
        // play the tone since it was launched
        noteStarted();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // changed the color of the note to animated
    public void comboPassed() {
        anim.SetTrigger("passed");
    }

    public void comboFailed() {
        anim.SetTrigger("failed");
    }

    public void noteStarted() {
        SFX.Play();
    }
}
