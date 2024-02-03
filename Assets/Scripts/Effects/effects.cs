using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effects : MonoBehaviour
{
    public float showDuration;
    public bool flashEffect = true;
    
    protected string type;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        // destroy this effect if it's a flash effect type
        if (flashEffect) Destroy(gameObject, showDuration);
    }

    // function for removing effect via animation
    protected virtual void animRemoveEffect()
    {
        Destroy(gameObject);
    }
}
