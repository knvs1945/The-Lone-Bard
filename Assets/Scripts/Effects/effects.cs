using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effects : MonoBehaviour
{
    public float showDuration, splashDMG = 0, splashRadius = 1f;
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

    // add damage to the effect
    public void addSplashDMG(float dmg)
    {
        if (dmg >= 0) splashDMG = dmg;
    }

    // check if an object is close to the explosion
    public bool checkTargetsAroundSplash(Transform target)
    {
        if (getTargetDistance(target.position) <= splashRadius) return true;
        return false;
    }

    // check if an object is close to the explosion
    protected virtual float getTargetDistance(Vector2 target) {
        return Vector2.Distance(transform.position, target);
    }
}
