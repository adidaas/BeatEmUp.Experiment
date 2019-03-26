using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    #region Parameters
    public float moveSpeed;
    private float activeMoveSpeed;
    private float runSpeed = 20f;
    public int playerCharacter = (int)GeneralEnums.PlayerCharacters.Ryu;     
    // scripts and objects
    // ================================
    public Rigidbody2D myRigidbody;
    private Animator myAnim;
    private SpriteRenderer mySpriteRenderer;    
    public BoxCollider2D myBoxCollider;
	private GameObject hitBox;
    private PlayerSpecialAttacks playerSpecialAttacks;
    private CharacterEffectController characterEffectController;
    public SpecialMeter specialMeterObject;
    private SpecialMeter specialMeterController;
    public PlayerInfoManager playerInfoManager;
    private AttackFramesManager attackFramesManager;
    private PlayerHurt playerHurt;    
    public GameObject airJuggleBoxObject;

    // ground checks
    // ==================================
    public Transform groundCheck;
    private float groundCheckRadius = 0.9f;
    public LayerMask groundType = GeneralEnums.GameObjectLayer.Ground;
    public GameObject groundCheckObject;
    public bool isGrounded;
    public bool pauseGroundCheck = false;
    public GameObject ground;

    // bool states
    // ==================================
    public bool canDash;
    public bool canMove;
    public bool canRun = false;
    public bool canJump = true;
    public bool isFacingRight = true;
	public bool canGroundAttack;
	public bool canAirAttack;
    public bool canDashAttack = false;
    public bool canBlock = true;
    public bool isCornered = false;    
    public bool isAttacking = false;
    public bool isDashing;
    public bool isDashingRight;
    public bool isRunning;
    public bool isRolling = false;
    public bool isJumping;
    public bool isFalling;
    public bool isBeingAirJuggle;
    public bool isAirJuggleable;
    public bool isInInvincibleState = false;
    public bool isBlocking = false;
    public bool isGuardCrush = false;
    
    // button inputs
    // ==================================
    public float buttonTapCooldown = 0f;
    public int buttonCount = 0;

    public bool inputLeft = false, inputRight = false, inputUp = false, inputDown = false, inputNeutral = true, lastInput = false;
    public bool initialTap = false;
    public bool secondTap = false;
    public bool tapRelease = false;
    public float previousAxisValue = 0;
    public float currentAxisValue = 0;
    public float previousHorizontalAxis = 0;
    public float previousDPadAxis = 0;

    public bool switchComboStyleSystem = true;

    #endregion    

    #region Start
    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
		//hitCollider = hitBox.GetComponent<HitCollider>();
        playerSpecialAttacks = GetComponent<PlayerSpecialAttacks>();
        playerHurt = GetComponent<PlayerHurt>();
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

        if (Input.GetKeyDown(KeyCode.L)) {
            print("here");
            pauseGroundCheck = true;
            isBeingAirJuggle = true;
            isGrounded = false;
			StartCoroutine(playerHurt.PerformHitStun(0.1f, GeneralEnums.AttacksHurtType.Launch));
		}
        // ==========================================================

        if (!pauseGroundCheck) {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.6f, groundType);
            //print(groundCheck.position);
            //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundType);
        }

        if (isBeingAirJuggle) {
			float distanceToGround = groundCheck.position.y - ground.transform.position.y;
            //print(distanceToGround);
			if (distanceToGround <= 1.55f) {
				isInInvincibleState = true;
                playerInfoManager.isInInvincibleState = true;
				//groundCheckCollider.enabled = true;
				myRigidbody.gravityScale = 3f;
			}
		}

        if (isGrounded && isBeingAirJuggle) {
			isBeingAirJuggle = false;			
            isFalling = false;
			//ToggleGroundCheckActive(true);
            myRigidbody.gravityScale = 5;
            myAnim.ResetTrigger(GeneralEnums.MovementTriggerNames.AirLanding);
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtGround);
			StartCoroutine(playerHurt.ToggleHurtWakeUp());
		}
        
        if (isGrounded) {            
            canDash = true;
            canAirAttack = false;            
            canGroundAttack = true;
            canJump = true;
        }

        // check if close to ground while in air juggle state
		if (isAirJuggleable) {
			float distanceToGround = groundCheck.position.y - ground.transform.position.y;

			if (distanceToGround <= 1.65f) {
				isInInvincibleState = true;
				
				myRigidbody.gravityScale = 3f;
				//currentGravityScale = 3f;
			}
		}
		isCornered = Physics2D.OverlapCircle(gameObject.transform.position, 2f, (int)GeneralEnums.GameObjectLayer.Wall);		

		// detect if object has touched the ground after being air juggled
		if (isGrounded && isAirJuggleable) {
			isAirJuggleable = false;			
			ToggleGroundCheckActive(true);
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtGround);			
			StartCoroutine(playerHurt.ToggleHurtWakeUp());
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

    public void ToggleGroundCheckActive(bool isActive) {
		groundCheckObject.SetActive(isActive);
		airJuggleBoxObject.SetActive(!isActive);
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
