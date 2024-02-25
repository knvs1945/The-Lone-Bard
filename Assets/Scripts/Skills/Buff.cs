using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buff : MonoBehaviour
{
    protected const float MIN_TIME = 30f; // 30 second default buff time
    
    protected float duration = MIN_TIME;
    protected float timeToExpire;
    protected bool isActive = true, hasExpired = false; // by default a buff is not 

    // access functions
    public float Duration {
        get { return duration; }
        set { if (value > 0) duration = value; }
    }

    // check if a buff has expired to remove it
    public bool HasExpired {
        get { return hasExpired = true; }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        timeToExpire = Time.time + duration;
    }

    // monitor buff duration here
    protected void monitorBuffDuration() {
        if (isActive && !hasExpired) {
            if (Time.time > timeToExpire) hasExpired = true; // change the status for removal
        }
    }

    // overrideable actions for buffs
    protected virtual void doOnApplyBuff() {}
    protected virtual void doOnRemoveBuff() {}

}

// Buffmanager 
public class BuffManager
{
    protected List<Buff> buffList;

    // constructor 
    public BuffManager() {
        buffList = new List<Buff>();
    }

    // return true or false if buffs have expired
    public bool checkExpiredBuffs()
    {
        // return a list of buffs that have expired
        List<Buff> expiredBuffs = buffList.Where(buff => buff.HasExpired).ToList();

        return expiredBuffs.Count > 0;
    }

    // returns a list of buffs list that have expired
    public List<Buff> getExpiredBuffs(bool updateList = true)
    {
        // return a list of buffs that have expired
        List<Buff> expiredBuffs = buffList.Where(buff => buff.HasExpired).ToList();
        if (updateList) buffList.RemoveAll(buff => buff.HasExpired); // remove buffs that have expired from the list

        return expiredBuffs;
    }
}
