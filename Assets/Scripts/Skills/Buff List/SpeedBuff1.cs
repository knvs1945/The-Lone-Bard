using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Buff class specific for speed buff skill
public class SpeedBuff1 : Buff
{
    protected const float MIN_SPEED_BUFF = 5f, MIN_BUFF_DURATION = 5f;

    // speed to add when buff is used
    [SerializeField]
    protected SpeedGlowEffect1 glowEffect;

    protected float speedAmount = MIN_SPEED_BUFF;
    protected int factor = 1;
    
    // Start is called before the first frame update
    protected override void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        monitorBuffDuration();
    }

    // overrideable actions for buffs
    public override void doOnApplyBuff() {
        SpeedGlowEffect1 tempEffect;
        
        testStats(); // comment out this function if done with testing stats
        targetUnit.addBuff(speedAmount, statAffected, this, factor);
        
        // Instantiate glowing effect into target Unit
        tempEffect = Instantiate(glowEffect, castPoint, false);
        tempEffect.transform.SetParent(castPoint);
        tempEffect.target = castPoint.gameObject;
        tempEffect.startGlow();
        
        timeToExpire = Time.time + duration;
        hasExpired = false;
        isActive = true; // start the buff timer check
    }

    public override void doOnRemoveBuff() {
        Debug.Log("Removing this buff: " + buffName);
        targetUnit.addBuff(speedAmount, statAffected, this, -1, true);

        Effects tempEffect = castPoint.GetComponent<SpeedGlowEffect1>();
        if (tempEffect != null) Destroy(glowEffect.gameObject);

        Destroy(gameObject);
    }

    // add testing stats here
    protected override void testStats() {
        if (statAffected == "") statAffected = "movespeed";
        duration = 5f;  // test value
    }
}

