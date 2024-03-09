using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gatekeeper : Enemy
{
    public List<Enemy> defenders;

    protected bool hasProtectors = true;


    // Start is called before the first frame update
    void Start()
    {
        defenders = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // obelisk gets damaged
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
