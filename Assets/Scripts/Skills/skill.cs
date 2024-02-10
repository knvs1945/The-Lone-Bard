using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for skills 
public class Skill : MonoBehaviour
{
    protected int id, type, level;
    protected string skillName, description;
    protected GameUnit owner, target;
    protected bool isActive, isPassive;

    public int ID {
        get { return id; }
        set { id = value; }
    }

    public int Type {
        get { return type; }
        set { type = value; }
    }

    public int Level {
        get { return level; }
        set { level = value; }
    }

    public string Skillname {
        get { return skillName; }
    }
    
    public string Description {
        get { return description; }
    }

    public GameUnit Owner {
        get { return owner; }
        set { owner = value; }
    }

    public GameUnit Target {
        get { return target; }
        set { target = Target; }
    }

    // skill status checkers
    public bool IsActive {
        get { return isActive; }
    }

    public bool IsPassive {
        get { return isPassive; }
    }

}

// class used to handle skills a unit can have
public class SkillManager
{
    protected List<Skill> skillSet;
    protected GameUnit owner;

    // deploying skill Manager should have at least one skill
    public SkillManager (GameUnit owner)
    {
        skillSet = new List<Skill>();
    }

    // get the specific skill via its index in the skill array - return null
    public Skill getSkill (int index) 
    {
        if (skillSet.Count > 0 && index >= 0 && index < skillSet.Count) {
            if (index >= 0 && index < skillSet.Count) return skillSet[index];
        }
        else {
            if (skillSet.Count <= 0) Debug.Log("Skill Set is empty: " + skillSet.Count);
            if (index < 0) Debug.Log("index is invalid: " + index);
            if (index > skillSet.Count) Debug.Log("index is more than skillSet length: " + index + " - " + skillSet.Count);
        }

        return null;
    }

    // let the manager add a skill to the owner unit
    public void addSkill (Skill newSkill) 
    {
        if (newSkill != null) {
            skillSet.Add(newSkill);
        }
    }

    // let the manager add a skill to the owner unit
    public void removeSkill (Skill newSkill) 
    {
        if (newSkill != null) {
            skillSet.Remove(newSkill);
        }
    }
}
