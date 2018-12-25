using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    #region Parameters
    public float moveSpeed;
    private float activeMoveSpeed;
    public Rigidbody2D myRigidbody;
	public HitCollider hitCollider;

    public bool canDash;
    public bool canMove;
	public bool canGroundAttack;
	public bool canAirAttack;
    private float buttonTapCooldown = 0f;
    private int buttonCount = 0;
    private int currentComboRouteCount = 0;
    private int currentComboRouteType;
    private float comboRouteCooldown;

    public float jumpSpeed;

    public bool isAttacking = false;

    // ground checks
    // ==================================
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundType;

    private KeyCode previousMovementKey;
    private KeyCode currentMovementKey;
    public bool isGrounded;
    public bool isDashing;
    public bool isDashingRight;
    public Animator myAnim;

	public GameObject hitBox;

    public AudioSource soundSwing1;
    public AudioSource soundSwing2;
    public AudioSource soundSwing3;

	public AudioClip soundHit1;
	public AudioClip soundHit2;
	public AudioClip soundHit3;
	public AudioClip soundHit4;

    public Slider healthBar;
    public float currentHealth = 1.0f;
    #endregion
    
	public static class MovementTriggerNames
	{
		// universal movements
        public static string MoveDash { get { return "move_Dash"; } }
		public static string MoveJump { get { return "move_Jump"; } }
	}

    #region Start
    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
		hitCollider = hitBox.GetComponent<HitCollider>();
        activeMoveSpeed = moveSpeed;

        canDash = true;
        canMove = true;
		canGroundAttack = true;
		canAirAttack = false;
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update () {
        // debugging check
        if (Input.GetKeyDown(KeyCode.Slash)) {
            Debug.Log("canMove:" + canMove);
            Debug.Log("canGroundAttack:" + canGroundAttack);
            Debug.Log("isAttacking:" + isAttacking);
            Debug.Log("comboRouteCooldown" + comboRouteCooldown);
            Debug.Log("currentComboRouteCount" + currentComboRouteCount);
        }
        // ==========================================================

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundType);
        
        if (comboRouteCooldown > 0f) {
            comboRouteCooldown -= 1 * Time.deltaTime ;
        }
        else {
            currentComboRouteCount = 0;
        }

        if (buttonTapCooldown > 0f) {
            buttonTapCooldown -= 1 * Time.deltaTime ;
        }
        else {
            buttonCount = 0;
        }

        if (buttonTapCooldown > 0 && buttonCount > 1 && !isDashing) {
            if (myAnim.GetBool("IsWalking") && !isDashing && canMove) {
                isDashing = true;
                canMove = false;
                canGroundAttack = false;
                buttonCount = 0;
                isDashingRight = (Input.GetAxisRaw("Horizontal") > 0f) ? true : false;
                myAnim.SetTrigger(MovementTriggerNames.MoveDash);

                StartCoroutine(MoveDash());
            }
        }


        #region Movement
        if (canMove && !isAttacking) {            

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) 
                || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)) {
                if (Input.GetKeyDown(KeyCode.D)) {
                    currentMovementKey = KeyCode.D;
                }
                else if (Input.GetKeyDown(KeyCode.W)) {
                    currentMovementKey = KeyCode.W;
                }
                else if (Input.GetKeyDown(KeyCode.S)) {
                    currentMovementKey = KeyCode.S;
                }
                else {
                    currentMovementKey = KeyCode.A;
                }

                if (previousMovementKey == currentMovementKey) {
                    buttonTapCooldown = 0.18f;
                    buttonCount++;
                }
                else if (previousMovementKey != currentMovementKey && buttonCount == 0) {
                    buttonTapCooldown = 0.18f;
                    buttonCount++;
                }
                else {
                    buttonCount--;
                }
                previousMovementKey = currentMovementKey;                
            }

            // movement left and right
            if (Input.GetAxisRaw("Horizontal") > 0f) {
                // input walk right
                myAnim.SetBool("IsWalking", true);
                myRigidbody.velocity = new Vector3(activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f) {
                // input walk left
                myAnim.SetBool("IsWalking", true);
                myRigidbody.velocity = new Vector3(-activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else {
                // idle
                myAnim.SetBool("IsWalking", false);                
                myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
            }
            
            // input jump            
            //if (Input.GetButtonDown("Jump") && isGrounded)
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //myAnim.SetBool("IsGrounded", false);
                canGroundAttack = false;
                myAnim.SetTrigger(MovementTriggerNames.MoveJump);

                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0f);
                //soundJump.Play();
            }
        }
        #endregion

        #region Attacks
        // attacks

        if (canGroundAttack && !isAttacking) {            
            // normal attacks
            // ==========================================
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // stop movement velocity when attacking
                if (!isDashing) {
                    myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                }
                isAttacking = true;
                canMove = false;
                myAnim.SetBool("IsAttacking", true);
                if (currentComboRouteCount == 0) {                
                    SetHitCollider(PlayerAttackEnums.RyuAttacks.Jab);
                    //soundSwing1.Play();
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackJab);
                    comboRouteCooldown = 0.62f;
                    currentComboRouteCount++;
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.AAA;
                }
                else if (currentComboRouteCount == 1 && comboRouteCooldown > 0f) {
                    SetHitCollider(PlayerAttackEnums.RyuAttacks.Short);
                    //soundSwing2.Play();
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackShort);
                    comboRouteCooldown = 0.5f;
                    currentComboRouteCount++;
                }
                else if (currentComboRouteCount == 2 && comboRouteCooldown > 0f) {
                    SetHitCollider(PlayerAttackEnums.RyuAttacks.Strong);
                    //soundSwing3.Play();
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackStrong);
                    currentComboRouteCount = 0;
                    comboRouteCooldown = 0f;
                }
                
            }
            // else if (Input.GetKeyDown(KeyCode.DownArrow))
            else if (5 == 6)
            {
                if (!isDashing) {
                    myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                }
                isAttacking = true;
                canMove = false;

                myAnim.SetBool("IsAttacking", true);
                SetHitCollider(PlayerAttackEnums.RyuAttacks.Short);

                //AttackStart(attackHitboxes[1]);
                //soundSwing2.Play();

                //myAnim.SetTrigger(AttackTriggerNames.AttackHighKick);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackShort);

            }
            //else if (Input.GetKeyDown(KeyCode.UpArrow))
            else if (5 == 6)
            {
                if (!isDashing) {
                    myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                }
                isAttacking = true;
                canMove = false;

                myAnim.SetBool("IsAttacking", true);
                SetHitCollider(PlayerAttackEnums.RyuAttacks.Strong);

                //AttackStart(attackHitboxes[2]);
                //soundSwing3.Play();

                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackStrong);
            }
            //else if (Input.GetKeyDown(KeyCode.RightArrow))
            else if (5 == 6)
            {
                if (!isDashing) {
                    myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                }
                
                isAttacking = true;
                canMove = false;

                myAnim.SetBool("IsAttacking", true);

                SetHitCollider(PlayerAttackEnums.RyuAttacks.Forward);

                //AttackStart(attackHitboxes[2]);
                //soundSwing3.Play();

                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackForward);
            }

            // special attacks
            // ==========================================
            if (buttonTapCooldown > 0 && currentMovementKey == KeyCode.W && Input.GetKeyDown(KeyCode.UpArrow)) {                    
                isAttacking = true;
                canMove = false;
                myAnim.SetBool("IsAttacking", true);
                SetHitCollider(PlayerAttackEnums.RyuAttacks.Shoryuken);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialShoryuken);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow)) {                    
                isAttacking = true;
                canMove = false;
                myAnim.SetBool("IsAttacking", true);
                SetHitCollider(PlayerAttackEnums.RyuAttacks.Hadouken);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHadouken);
            }
        }
        

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    myAnim.SetBool("IsAttacking", true);
        //    Debug.Log("special1");
        //    //AttackStart(attackHitboxes[2]);
        //    //soundSwing3.Play();

        //    myAnim.SetTrigger(AttackTriggerNames.AttackImpact);
        //}
        #endregion

        myAnim.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));
        myAnim.SetBool("IsGrounded", isGrounded);

        // check if falling
        if (myRigidbody.velocity.y < 0)
        {
            
        }
        else
        {
            
        }
    }
    #endregion

    #region Move Dash
    public IEnumerator MoveDash()
    {
        var direction = isDashingRight ? 4f : -4F; 

        var endPosition = new Vector3(transform.position.x + direction, transform.position.y, transform.position.z);        

        while (transform.position != endPosition) {
			transform.position = 			
					Vector3.MoveTowards(transform.position, endPosition, 20f * Time.deltaTime);
					
			yield return new WaitForEndOfFrame ();
		}

    }
    #endregion

    #region Set Hit Collider
    private void SetHitCollider(PlayerAttackEnums.RyuAttacks attackType)
    {
        if (PlayerAttackEnums.RyuAttacks.Jab == attackType) {
            hitCollider.attackName = "Jab";
            hitCollider.attackSound = soundHit4;
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Jab;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Jab;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)PlayerAttackEnums.RyuAttacksHurtType.Jab);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Jab;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Small);
        }
        else if (PlayerAttackEnums.RyuAttacks.Short == attackType) {
            hitCollider.attackName = "Short";
            hitCollider.attackSound = soundHit1;
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Short;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Short;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)PlayerAttackEnums.RyuAttacksHurtType.Short);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Short;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Small);
        }
        else if (PlayerAttackEnums.RyuAttacks.Strong == attackType) {
            hitCollider.attackName = "Strong";
            hitCollider.attackSound = soundHit2;
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Strong;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Strong;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)PlayerAttackEnums.RyuAttacksHurtType.Strong);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Strong;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Big);
        }
        else if (PlayerAttackEnums.RyuAttacks.Forward == attackType) {
            hitCollider.attackName = "Forward";
            hitCollider.attackSound = soundHit4;
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Forward;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Forward;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)PlayerAttackEnums.RyuAttacksHurtType.Forward);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Forward;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Mid);
        }
        else if (PlayerAttackEnums.RyuAttacks.Shoryuken == attackType) {
            hitCollider.attackName = "Shoryuken";
            hitCollider.attackSound = soundHit3;
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Shoryuken;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Shoryuken;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)PlayerAttackEnums.RyuAttacksHurtType.Shoryuken);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Shoryuken;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Big);
        }
        

    }
    #endregion

    #region Animator_ToggleCanMove
    void Animator_ToggleCanMove()
    {
        canMove = true;
    }
    #endregion

	#region Animator_ToggleIsAttacking
    void Animator_ToggleIsAttacking()
    {
        isAttacking = isAttacking == false ? true : false;
        myAnim.SetBool("IsAttacking", isAttacking);
    }
    #endregion

	#region Animator_ToggleIsGrounded
	void Animator_ToggleIsGrounded()
	{
		canGroundAttack = true;
		//Debug.Log("toggle is grounded");
		//myAnim.SetBool("IsGrounded", true);
		//isGrounded = true;
	}
	#endregion

    #region Animator_ToggleDashing
	void Animator_ToggleDashing()
	{        
        isDashing = false;		
	}
	#endregion

}
