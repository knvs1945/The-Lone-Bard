using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeedUp1 : Skill
{
    protected float minSpeedBonus = 3f;

    [SerializeField]
    protected Buff speedBuff;

    // Start is called before the first frame update
    void Awake()
    {
        isActive = true;
        skillName = "Speed Up";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // instantiate a buff then apply it to a player
    protected override bool doOnTrigger()
    {
        bool success = false;
        Debug.Log("Casting Speed Buff 1 on: " + target);
        consoleUI.Log("Cast Skill success: " + skillName);
        Buff tempBuff;
        tempBuff = Instantiate(speedBuff);
        System.Diagnostics.Debugger.Break();
        if (target) {
            Debug.Log("Speed Buff target present: " + target);
            // set the target and apply buff
            tempBuff.TargetUnit = target;
            tempBuff.doOnApplyBuff();
            success = true;
        }
        return success;
    }
}
