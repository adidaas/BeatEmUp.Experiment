using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerMovementControl;

public class PlayerController : MonoBehaviour {

    #region Parameters
    public float moveSpeed;
    private float activeMoveSpeed;
    private float runSpeed = 20f;
    public Rigidbody2D myRigidbody;
    
    public BoxCollider2D myBoxCollider;
	public GameObject hitBox;
	public HitCollider hitCollider;
    public PlayerSpecialAttacks playerSpecialAttacks;
    private CharacterEffectController characterEffectController;
    public SpecialMeter specialMeterObject;
    private SpecialMeter specialMeterController;
    public PlayerInfoManager playerInfoManager;
    public AttackFramesManager attackFramesManager;


    public bool canDash;
    public bool canMove;
    public bool canRun = false;
    public bool canJump = true;
    public bool isFacingRight = true;
	public bool canGroundAttack;
	public bool canAirAttack;
    public bool canDashAttack = false;
    public float buttonTapCooldown = 0f;
    public int buttonCount = 0;

    public bool isAttacking = false;

    // ground checks
    // ==================================
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundType;

    public bool isGrounded;
    public bool pauseGroundCheck = false;
    public bool isDashing;
    public bool isDashingRight;
    public bool isRunning;
    public bool isJumping;
    public bool isFalling;
    public Animator myAnim;
    public SpriteRenderer mySpriteRenderer;
    public GameObject ground;

    public  bool inputLeft = false, inputRight = false, inputUp = false, inputDown = false, inputNeutral = true, lastInput = false;
    public bool initialTap = false;
    public bool secondTap = false;
    public bool tapRelease = false;


    public float previousAxisValue = 0;
    public float currentAxisValue = 0;
    public float previousHorizontalAxis = 0;
    public float previousDPadAxis = 0;

    public bool switchComboStyleSystem = true;

    public Slider healthBar;
    public float currentHealth = 1.0f;
    #endregion    

    #region Start
    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
		hitCollider = hitBox.GetComponent<HitCollider>();
        playerSpecialAttacks = GetComponent<PlayerSpecialAttacks>();
        characterEffectController = GetComponent<CharacterEffectController>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        specialMeterController = specialMeterObject.GetComponent<SpecialMeter>();
        attackFramesManager = GetComponent<AttackFramesManager>();
        activeMoveSpeed = moveSpeed;

        canDash = true;
        canMove = true;
		canGroundAttack = true;
		canAirAttack = false;        
        ground = GameObject.FindWithTag("Ground");
    }
    #endregion

    #region Update
    void Update () {
        // debugging check
        if (Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("canMove:" + canMove);
            Debug.Log("canRun:" + canRun);            
            Debug.Log("isDashing:" + isDashing);
            Debug.Log("isJumping:" + isJumping);
            Debug.Log("isGrounded:" + isGrounded);
            Debug.Log("canGroundAttack:" + canGroundAttack);
            Debug.Log("isAttacking:" + isAttacking);
            Debug.Log("-------------");
            hitBox.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.F4)) {
			switchComboStyleSystem = switchComboStyleSystem ? false : true;
		}
        // ==========================================================

        

        if (!pauseGroundCheck) {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundType);
        }
        
        if (isGrounded) {            
            canDash = true;
            canAirAttack = false;            
            canGroundAttack = true;
            canJump = true;
        }

        // if (buttonTapCooldown > 0f) {
        //     buttonTapCooldown -= 1 * Time.deltaTime;            
        // }
        // else {
        //     buttonCount = 0;
        //     buttonTapCooldown = 0f;
        // }

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

    public void AdjustSpecialMeter(int amount) {
        playerInfoManager.AdjustSpecialMeter(amount);
    }

    public void ResetEverything() {
        canDash = true;
        canMove = true;
        canRun = false;
        canJump = true;
        canGroundAttack = true;
        canAirAttack = false;
        canDashAttack = false;
        isAttacking = false;
        pauseGroundCheck = false;
        isDashing = false;
        isDashingRight = false;
        isRunning = false;
        isJumping = false;
        isFalling = false;
    }    
    
    #region Animator_ToggleCanMove
    void Animator_ToggleCanMove()
    {
        canMove = true;
    }
    #endregion

	#region Animator_ToggleIsAttacking
    void Animator_ToggleIsAttacking()
    {
        isAttacking = false;
        myAnim.SetBool("IsAttacking", isAttacking);
    }
    #endregion

	#region Animator_ToggleIsGrounded
	void Animator_ToggleIsGrounded()
	{
		canGroundAttack = true;
	}
	#endregion

    #region Animator_ToggleIsRunning
	void Animator_ToggleIsRunning()
	{
		isRunning = false;
	}
	#endregion

    #region Animator_ToggleCanRun
	void Animator_ToggleIsJumping()
	{        
        isJumping = false;
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

    #region Animator_ToggleCanAirAttack
	void Animator_ToggleCanAirAttack() {        
        canAirAttack = true;
	}
	#endregion

    #region Animator_ToggleCanAirAttackFalse
	void Animator_ToggleCanAirAttackFalse() {        
        canAirAttack = true;
	}
	#endregion

    #region Animator_ToggleAirIdle
	void Animator_ToggleAirIdle()
	{        
        myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.AirIdle);
	}
	#endregion
    #region Animator_PauseGroundCheckFalse
	void Animator_PauseGroundCheckFalse()
	{        
        pauseGroundCheck = false;
	}
	#endregion

    #region Animator_ToggleCanDash
	void Animator_ToggleCanDash() {        
        canDash = true;
	}
	#endregion

    #region Animator_ToggleCanDashAttack
	void Animator_ToggleCanDashAttack() {        
        canDashAttack = false;
	}
	#endregion

}
