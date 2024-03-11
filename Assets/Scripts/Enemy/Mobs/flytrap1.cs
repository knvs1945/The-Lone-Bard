using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flytrap1 : Enemy
{
    public GameUnit tempTarget;

    protected Animator anim;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        // get animator component
        anim = GetComponent<Animator>();
    
    }

    protected void Awake()
    {
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemyEnabled) return;
        checkTargetInRange();
    }

    // melee bite when player is in range
    protected void checkTargetInRange()
    {
        if (currentTarget.IsAlive) {
            distance = getTargetDistance();
            if (distance <= engageRange) {
                anim.SetTrigger("bite");
            }
        }
    }

    // animator behavior;
    public void doBiteAttack() {
        if (currentTarget.IsAlive) {
            showDamage(ATKbase, target.position);
            damageTarget(ATKbase);
        }
    }

    // bite enemy when it is close
    protected override void doOnTakeDamage(float DMG) {
        if (HP >= 0) {
            HP -= DMG;
            if (HP <= 0) {
                this.isAlive = false;
                Destroy(gameObject);
            }
        }
    }
}
