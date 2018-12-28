using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    #region Parameters
    public float moveSpeed;
    private float activeMoveSpeed;
    private float runSpeed = 20f;
    public Rigidbody2D myRigidbody;
    
	public GameObject hitBox;
	public HitCollider hitCollider;
    public PlayerSpecialAttacks playerSpecialAttacks;


    public bool canDash;
    public bool canMove;
    public bool canRun = false;
    public bool isFacingRight = true;
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
    public bool isRunning;
    public Animator myAnim;


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
    
	public static class MovementTriggerNames {
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
        playerSpecialAttacks = GetComponent<PlayerSpecialAttacks>();
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
            Debug.Log("canRun:" + canRun);            
            Debug.Log("isDashing:" + isDashing);
            Debug.Log("canGroundAttack:" + canGroundAttack);
            Debug.Log("isAttacking:" + isAttacking);
            Debug.Log("-------------");
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
                myAnim.SetBool("IsDashing", true);
                myAnim.SetTrigger(MovementTriggerNames.MoveDash);

                StartCoroutine(MoveDash());
            }
        }

        if (canRun) {
            if (Input.GetAxisRaw("Horizontal") > 0f) {
                // input run right
                myAnim.SetBool("IsRunning", true);
                isRunning = true;
                isDashing  = false;
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsDashing", false);
                myRigidbody.velocity = new Vector3(runSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
                isFacingRight = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f) {
                // input run left
                myAnim.SetBool("IsRunning", true);
                isRunning = true;
                isDashing  = false;
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsDashing", false);
                myRigidbody.velocity = new Vector3(-runSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
                isFacingRight = false;
            }
            else {
                // idle
                myAnim.SetBool("IsWalking", false);                
                myAnim.SetBool("IsRunning", false);
                myAnim.SetBool("IsDashing", false);
                isRunning = false;
                canMove = true;
                canRun = false;
                myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
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
                myAnim.SetBool("IsRunning", false);
                myRigidbody.velocity = new Vector3(activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
                isFacingRight = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f) {
                // input walk left
                myAnim.SetBool("IsWalking", true);
                myRigidbody.velocity = new Vector3(-activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
                isFacingRight = false;
            }
            else {
                // idle
                myAnim.SetBool("IsWalking", false);                
                myAnim.SetBool("IsRunning", false);
                myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
            }
            
            // input jump            
            if (Input.GetKeyDown(KeyCode.DownArrow)&& isGrounded)
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
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Jab);
                    //soundSwing1.Play();
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackJab);
                    comboRouteCooldown = 0.62f;
                    currentComboRouteCount++;
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.AAA;
                }
                else if (currentComboRouteCount == 1 && comboRouteCooldown > 0f) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Short);
                    //soundSwing2.Play();
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackShort);
                    comboRouteCooldown = 0.5f;
                    currentComboRouteCount++;
                }
                else if (currentComboRouteCount == 2 && comboRouteCooldown > 0f) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Strong);
                    //soundSwing3.Play();
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackStrong);
                    currentComboRouteCount = 0;
                    comboRouteCooldown = 0f;
                }
                
            }

            // special attacks
            // ==========================================
            if (buttonTapCooldown > 0 && currentMovementKey == KeyCode.W && Input.GetKeyDown(KeyCode.UpArrow)) {                    
                isAttacking = true;
                canMove = false;
                myAnim.SetBool("IsAttacking", true);
                BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Shoryuken);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialShoryuken);
            }
            else if (buttonTapCooldown > 0 && ( currentMovementKey == KeyCode.D || currentMovementKey == KeyCode.A ) && Input.GetKeyDown(KeyCode.UpArrow)) {                    
                isAttacking = true;
                canMove = false;
                myAnim.SetBool("IsAttacking", true);
                BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Tatsumaki);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialTatsumaki);

                playerSpecialAttacks.InitializeRyuSpecialAttack((int)PlayerAttackEnums.RyuAttacks.Tatsumaki, isFacingRight);                
            }
            else if (buttonTapCooldown <= 0 && Input.GetKeyDown(KeyCode.UpArrow) && currentMovementKey != KeyCode.D && currentMovementKey != KeyCode.A && currentMovementKey != KeyCode.W) {                    
                isAttacking = true;
                canMove = false;
                myAnim.SetBool("IsAttacking", true);                
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHadouken);

                playerSpecialAttacks.InitializeRyuSpecialAttack((int)PlayerAttackEnums.RyuAttacks.Hadouken, isFacingRight);                
            }
        }
        
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
    public IEnumerator MoveDash() {
        var direction = isDashingRight ? 4f : -4F; 

        var endPosition = new Vector3(transform.position.x + direction, transform.position.y, transform.position.z);        

        while (transform.position != endPosition) {
			transform.position = 			
					Vector3.MoveTowards(transform.position, endPosition, 20f * Time.deltaTime);
					
			yield return new WaitForEndOfFrame ();
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
        myAnim.SetBool("IsDashing", isDashing);
	}
	#endregion

    #region Animator_ToggleCanRun
	void Animator_ToggleCanRun()
	{        
        canRun = canRun ? false : true;
	}
	#endregion

}
