using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTristan : playerUnit
{
    public projectiles attackEffect;
    public Transform frontSide, attackPoint;

    // will contain the stats for the character to be set on the UI
    public float base_ATKbase, base_ATKmax, base_ATKdelay, base_ATKRange; 

    private Rigidbody2D rbBody;
    private Vector2 moveInput, moveData;
    private bool canAttack = true;
    

    [SerializeField]
    protected GameObject body;
    protected Animator animBody;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rbBody = GetComponent<Rigidbody2D>();
        animBody = body.GetComponent<Animator>();
        initializeStats();

        // temporarily enable god mode;
        // isImmune = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isControlDisabled) {
            checkIfAlive();
            inputMovement();
            tapTesting();
        }
    }

    void FixedUpdate()
    {
        // move the rigidbody here
        rbBody.MovePosition(rbBody.position + moveData * Time.fixedDeltaTime);
    }

    // ================ Stats and Status sequences start here  ================ //
    private void checkIfAlive () {
        isAlive = checkHPifAlive() ? true : false;
    }

    private void initializeStats() {
        ATKbase = base_ATKbase;
        ATKmax = base_ATKmax;
        ATKdelay = base_ATKdelay;
        ATKRange = base_ATKRange;
    }

    // ================ Input action sequences start here  ================ //
    // Input for moving player
    private void inputMovement() {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveData = moveInput.normalized * moveSpeed;

        // add default keycode for up, down left and right arrows
        if (Input.GetKeyDown(controls.MoveUp) || Input.GetKeyDown(KeyCode.UpArrow)) setAnimationWalking(1, true);
        else if (Input.GetKeyDown(controls.MoveDown) || Input.GetKeyDown(KeyCode.DownArrow)) setAnimationWalking(2, true);
        else if (Input.GetKeyDown(controls.MoveLeft) || Input.GetKeyDown(KeyCode.LeftArrow)) setAnimationWalking(3, true);
        else if (Input.GetKeyDown(controls.MoveRight) || Input.GetKeyDown(KeyCode.RightArrow)) setAnimationWalking(4, true);
        
        // keep walking animation while any of the directions are pressed
        if (Input.GetKey(controls.MoveUp) || Input.GetKey(controls.MoveDown) || Input.GetKey(controls.MoveLeft) || Input.GetKey(controls.MoveRight)) setAnimationWalking(0, true); 
        else setAnimationWalking(0, false);

        // attack animation
        if (Input.GetKey(controls.Attack)) {
            if (canAttack) {
                canAttack = false;
                animateAttack();
                Invoke("updateAttackCD", ATKdelay);
            }
        }
    }
    
    private void tapTesting() {
        if (Input.GetKeyDown(controls.Skillsync)) {
          if (rhythmHandler.checkTap()) consoleUI.Log("Tap Successful");
          else consoleUI.Log("Tap Missed");
        }
    }

    private void updateAttackCD() {
        canAttack = true;
    }

    private void updateDamageShield() {
        isdamageShldActive = false;
    }

    //  ================ animation sequences start here  ================ //
    private void setAnimationWalking(int direction, bool isWalking) {
        animBody.SetBool("isWalking", isWalking);
        if (direction > 0) animBody.SetInteger("walkDirection", direction);
    }

    private void animateAttack() {
        projectiles tempSlash; 
        int frontSideRotation = 0;
        frontSideRotation = animBody.GetInteger("walkDirection");
        animBody.SetTrigger("normalAttack");
        tempSlash = Instantiate(attackEffect, attackPoint.position, attackPoint.rotation);
        tempSlash.DMG = Random.Range(base_ATKbase, base_ATKmax);
    }
    
    // do on takes damage
    protected override void doOnTakeDamage(float DMG) {
        Debug.Log("Getting damaged: " + isdamageShldActive);
        if (!isdamageShldActive && HP > 0) {
            isdamageShldActive = true;
            HP -= DMG;
            // Debug.Log(" Player takes damage: " + DMG);
            if (HP <= 0) {
                HP = 0;
                // isAlive = false; // disable if you don't want the player to die
            }
            
            // inform parent class that HP bar can now be updated;
            updateHPBar();
            Invoke("updateDamageShield", base_DMGdelay);
        }

    }



}
