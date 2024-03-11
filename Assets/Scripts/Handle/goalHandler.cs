using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Handler for objectives and goals
public class GoalHandler : Handler
{
    
    protected List<Goal> goalList = new List<Goal>();
    protected List<Goal> goalsCompleted = new List<Goal>(); // used for non-priority secondary goals
    protected List<Goal> goalsFailed = new List<Goal>(); // used for non-priority secondary goals
    protected bool isCheckingGoals = false;

    // Getters and Setters
    public List<Goal> GoalList {
        get { return goalList; }
    }

    public bool IsCheckingGoals {
        get { return isCheckingGoals; }
        set { isCheckingGoals = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int goalStatus = 0;
        if (isCheckingGoals) {
            // check if any goals have passed or failed and then report them to their handler
            for (int i = goalList.Count-1; i > goalList.Count; i++) {
                goalStatus = goalList[i].checkForCompletion();
                if (goalStatus != 0) { 
                    if (goalStatus == 1) goalList[i].Owner.goalCompleted(goalList[i]);
                    else if (goalStatus == -1) goalList[i].Owner.goalFailed(goalList[i]);
                    goalList[i].goalForRemoval = true;
                }
            }
            // then remove all goals that are either completed/failed;
            goalList.RemoveAll(goal => goal.goalForRemoval);
        }
    }

    // Create goals in this call
    public Goal CreateGoal(int type, string goalName, string goalDesc, GameUnit target, bool isPriority, float timer = 0, int count = 0, List<GameUnit> targets = null, Goal prevGoal = null) {
        Goal tempGoal = new Goal(goalList.Count, type, goalName, goalDesc, target, isPriority, timer, count, targets, prevGoal);
        if (tempGoal != null) goalList.Add(tempGoal);
        return tempGoal;
    }
}

// goal class to add to each map/stage
public class Goal
{
    public const int TYPE_KILL = 1, TYPE_PROTECT = 2;
    protected List<GameUnit> targets; // used for multiple targets;
    protected GameUnit target;
    protected Goal previousGoal, nextGoal;
    protected Handler owner;
    
    public bool goalForRemoval; // flags the goal for removal from GoalHandler's list only
    protected int goalStatus; // goalStatus has 3 types: incomplete (0), Success (1), or Fail (-1)
    protected int id, type = 1, count = 0;
    protected string name, description;
    protected float goalTimer = 0, goalTimeLimit, goalRemainingTime;
    protected bool isPriorityGoal, isTimedGoal;
    
    // public status checkers
    public bool isEnabled, isDone;

    // constructor. Exclude owner here since owner can be assigned after goal is created
    public Goal (int _ID, int _Type, string _Name, string _Desc, GameUnit _target, bool _isPriority, float _timer = 0, int _count = 0, List<GameUnit> _targets = null, Goal _previousGoal = null) {
        id = _ID;
        type = _Type;
        name = _Name;
        description = _Desc;
        target = _target;
        if (_timer > 0) goalTimer = _timer; // goal will not accept negative times
        targets = _targets;
        count = _count;
        isPriorityGoal = _isPriority;
        previousGoal = _previousGoal;

        if (goalTimer > 0) isTimedGoal = true;
        if (previousGoal != null) previousGoal.nextGoal = this; // set this goal as the next goal to its predecessor, if it's present. Will be used during goal finalizing
        goalStatus = 0; // default status is incomplete
        goalForRemoval = false;
    }

    public int ID {
        get { return id; }
        // set { if (value >= 0) id = value; }
    }

    public int Type {
        get { return type; }
        // set { if (value >= 0) type = value; }
    }

    public string Name {
        get { return name; }
        // set { name = value; }
    }

    public string Description {
        get { return description; }
        // set { description = value; }
    }

    public Handler Owner {
        get { return owner; }
        set { owner = value; }
    }

    public Goal PreviousGoal {
        get { return previousGoal; }
        set { previousGoal = value; }
    }

    public GameUnit Target {
        get { return target; }
        // set { nextGoal = value; }
    }

    public bool IsPriorityGoal {
        get { return isPriorityGoal; }
        set { isPriorityGoal = value; }
    }


    // used only for timed goals for the meantime, but can be used for regular goals too
    public bool initializeGoal() {
        if (isTimedGoal) {
            goalTimeLimit = Time.time + goalTimer;
        }
        return true;
    }

    // accessor function that will check if a goal has been completed.
    public int checkForCompletion() {
        if (isTimedGoal) {
            if (Time.time <= goalTimeLimit) {
                if (type == TYPE_KILL) {
                    if (checkKillObjective()) {
                        goalStatus = 1; // goal status is success 
                    }
                }
                if (type == TYPE_PROTECT) {
                    if (!checkProtectObjective()) {
                        goalStatus = -1;
                    }
                }
            }
            else {
                goalStatus = -1; // ran out of time. goal status is failure
            }
        }
        else {
            if (type == TYPE_KILL) {
                if (checkKillObjective()) {
                    goalStatus = 1; // goal status is success 
                }
            }
            if (type == TYPE_PROTECT) {
                if (!checkProtectObjective()) {
                    goalStatus = -1; // goal status is failure
                }
            }
        }
        return goalStatus;
    }

    // kill-type objectives. Will check if target is still alive and return true if so
    protected bool checkKillObjective() {
        bool success = false;
        if (!target.IsAlive) success = true; // unit has been killed;
        return success;
    }

    // protect-type objectives. Will check if target is still alive and return false is not anymore
    protected bool checkProtectObjective() {
        bool success = true;
        if (!target.IsAlive) success = false; // unit has been killed;
        return success;
    }

    // report completion to the owner. Only report if status is NOT 0 (incomplete)
    public void finishGoal() {
        if (goalStatus == 0) {
            Debug.Log("Invalid finish goal called - goal " + ID + " - " + name + " is still incomplete. ");
        }
        if (nextGoal != null) {
            nextGoal.initializeGoal();
        }
    }
}
