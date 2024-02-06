using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime1 : enemy
{
    // Start is called before the first frame update
    public gameUnit tempTarget; // use for testing only

    private float distance, atkAnimSpd, burstMoveSpeed;

    // constructor
    public slime1 (int level = 1) {
        if (currentTarget == null) currentTarget = tempTarget;
        Level = level;
        atkAnimSpd = 3;
        ATKTimer = 0;
        ATKdelay = 1;
        ATKbase = 5;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        atkAnimSpd = 3;
        ATKTimer = 0;
        ATKdelay = 1;
        ATKbase = 5;
        // currentTarget = tempTarget;
        UpdateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        chaseCurrentTarget();
    }

    // called within the slime's run animation
    public void addBurstMoveSpeed()
    {
        burstMoveSpeed = moveSpeed;
    }

    // override the normal chase behavior and make it "jump" during its animation instead
    protected override void chaseCurrentTarget() {
        if (currentTarget) {
            if (currentTarget.IsAlive) {
                doOnChaseTarget();
                // keep moving until target is in engaging distance
                if (getTargetDistance() >= engageRange) {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, burstMoveSpeed * Time.deltaTime);
                    if (burstMoveSpeed > 0) burstMoveSpeed -= 0.5f;
                    else burstMoveSpeed = 0;
                     
                }
                else doOnReachTarget();
            }
        }
    }

    // bat 1 behavior
    protected override void doOnReachTarget() {
        if (currentTarget.IsAlive) {
            distance = getTargetDistance();
            if (distance <= engageRange) {
                // bite the player with an animation sequence
                if (Time.time >= ATKTimer) {
                    StartCoroutine(biteAttack());
                    ATKTimer = Time.time + ATKdelay; // delay the next attack using the ATKDelay
                    showDamage(ATKbase, target.position);
                    damageTarget(ATKbase);
                }
            }
        }
    }

    protected IEnumerator biteAttack ()
    {
        Vector2 startPos = transform.position, endPos = target.position;
        float percent = 0, animFormula;
        while (percent <= 1) {
            percent += (atkAnimSpd * Time.deltaTime);
            animFormula = (-Mathf.Pow(percent,3) + percent) * 4;
            transform.position = Vector2.Lerp(startPos, endPos, animFormula);
            yield return null;  // don't return any time to make the animation instant;
        }
    }

    // bat 1 gets damaged
    protected override void doOnTakeDamage(float DMG) {        
        if (HP >= 0) {
            HP -= DMG;
            if (HP <= 0) {
                this.isAlive = false;
                Debug.Log("Bat is killed: " + HP);
                Destroy(gameObject);
            }
        }
    }

}
