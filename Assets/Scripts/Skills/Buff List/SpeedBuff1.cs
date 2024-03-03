using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff1 : Buff
{
    protected const float MIN_SPEED_BUFF = 5f, MIN_BUFF_DURATION = 5f;

    // speed to add when buff is used
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
        testStats(); // comment out this function if done with testing stats
        targetUnit.addBuff(speedAmount, statAffected, this, factor);
        timeToExpire = Time.time + duration;
        hasExpired = false;
        isActive = true; // start the buff timer check
    }

    public override void doOnRemoveBuff() {
        Debug.Log("Removing this buff: " + buffName);
        targetUnit.addBuff(speedAmount, statAffected, this, -1, true);
        Destroy(gameObject);
    }

    // add testing stats here
    protected override void testStats() {
        if (statAffected == "") statAffected = "movespeed";
        duration = 5f;  // test value
    }
}
