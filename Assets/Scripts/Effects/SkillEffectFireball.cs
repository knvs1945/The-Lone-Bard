using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectFireball : Projectiles
{
    // public values on editor
    public float fireballSpd, fireballRange;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        spd = fireballSpd;
        range = fireballRange;
    }

    // Update is called once per frame
    void Update()
    {
        moveUntilCollide();
    }

    // remove game object after reaching max range;
    protected override void doOnMaxRange()
    {
        Destroy(gameObject);
    }

}
