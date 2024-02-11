using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFireball : Skill
{
    protected float dmg, spd, range;
    
    [SerializeField]
    protected Projectiles fireball;

    public float DMG {
        get { return dmg; }
        set {
            if (value >= 0) dmg = value; 
        }
    }

    public float SPEED {
        get { return spd; }
        set { spd = value; }
    }

    public float RANGE {
        get { return range; }
        set {
            if (value >= 0) range = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // fire a fireball
    protected override bool doOnTrigger()
    { 
        Debug.Log("Casting firebal skill 2...");
        Projectiles temp;
        temp = Instantiate(fireball, castpoint.position, castpoint.rotation);
        temp.DMG = dmg;
        temp.SPEED = spd;
        temp.RANGE = range;
        return true;
    }
}
