using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buff : MonoBehaviour
{
    protected const float MIN_TIME = 30f; // 30 second default buff time
    
    public string buffName, statAffected;

    protected GameUnit targetUnit;
    protected float duration = MIN_TIME;
    protected float timeToExpire;
    protected bool isActive = false, hasExpired = false; // by default a buff is not 

    // access functions
    public GameUnit TargetUnit {
        get { return targetUnit; }
        set { targetUnit = value; }
    }

    public float Duration {
        get { return duration; }
        set { if (value > 0) duration = value; }
    }

    // check if a buff has expired to remove it
    public bool HasExpired {
        get { return hasExpired; }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    protected virtual void applyBuff() {
        doOnApplyBuff();
    }

    // monitor buff duration here
    protected void monitorBuffDuration() {
        if (isActive && !hasExpired) {
            if (Time.time > timeToExpire) {
                Debug.Log("Buff Has Expired");
                hasExpired = true; // change the status for removal
            }
        }
    }

    // overrideable actions for buffs
    public virtual void doOnApplyBuff() {}
    public virtual void doOnRemoveBuff() {}
    
    protected virtual void testStats() {}

}

// Buffmanager 
public class BuffManager
{
    protected List<Buff> buffList;

    // constructor 
    public BuffManager() {
        buffList = new List<Buff>();
    }

    // add a buff to the managing list
    public void addBuffToList(Buff buffToAdd)
    {
        buffList.Add(buffToAdd);
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
