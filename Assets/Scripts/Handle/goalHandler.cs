using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// goal class to add to each map/stage
public class Goal
{
    protected int id, type;
    protected string name, description;
    protected bool isMajorGoal;
    protected Goal nextGoal;
    
    // public status checkers
    public bool isEnabled, isDone;

    // constructor
    public Goal (int paramID, int paramType, string paramName, string paramDesc) {
        id = paramID;
        type = paramType;
        name = paramName;
        description = paramDesc;
    }

    public int ID {
        get { return id; }
        set { if (value >= 0) id = value; }
    }

    public int Type {
        get { return type; }
        set { if (value >= 0) type = value; }
    }

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public string Description {
        get { return description; }
        set { description = value; }
    }

    public Goal NextGoal {
        get { return nextGoal; }
        set { nextGoal = value; }
    }

    public bool IsMajorGOal {
        get { return isMajorGoal; }
        set { isMajorGoal = value; }
    }
}

// Handler for objectives and goals
public class GoalHandler : Handler
{
    
    public Goal[] goalSet;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
