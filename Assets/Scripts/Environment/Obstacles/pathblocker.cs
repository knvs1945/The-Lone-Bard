using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathblocker : Env
{

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Awake()
    {
        isNPC = true;
        isAlive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // pathblocker is opened
    public void open() {
        Debug.Log("Opening THIS pathblocker: " + this);
        anim.SetTrigger("open");
    }

    // pathblocker is closed
    public void close() {
        anim.SetTrigger("close");
    }
    
    // remove pathblocker
    public void remove() {
        Debug.Log("removing pathblocker: " + this);
        Destroy(gameObject);
    }
}
