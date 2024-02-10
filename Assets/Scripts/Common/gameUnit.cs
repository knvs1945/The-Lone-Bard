using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    public float HP, moveSpeed;

    protected int Level;
    protected float HPMax, HPTemp;
    protected float ATKbase, ATKmax, ATKTemp, ATKdelay, ATKTimer, ATKRange;
    protected bool isAlive, isActive, isPaused, isImmune, isNPC, isIllusion;
    
    [SerializeField]
    protected string unitName;

    // constructor
    public GameUnit() {
        isAlive = isImmune = true;
        isActive = isPaused = isNPC = false;
        Level = 1;
    }

    // getters
    public string Name {
        get { return unitName; }
    }
    public bool IsAlive {
        get { return isAlive; }
    }

    public bool IsActive {
        get { return isActive; }
    }

    public bool IsPaused {
        get { return isPaused; }
    }

    public bool IsImmune {
        get { return isImmune; }
    }

    public bool IsNPC {
        get { return isNPC; }
    }

    public bool IsIllusion {
        get { return isIllusion; }
    }

    // common unit behavior
    public bool takesDamage(float DMG) {
        if (!isImmune) {
            doOnTakeDamage(DMG);
            return true;
        }
        return false;
    }

    // force kill a unit
    public bool killUnit() {
        if (isAlive) {
            doOnDeath();
            return true;
        }
        return false;
    }

    // Overrideables
    protected virtual void doOnTakeDamage(float DMG) {}
    protected virtual void doOnDeath() {}

}
