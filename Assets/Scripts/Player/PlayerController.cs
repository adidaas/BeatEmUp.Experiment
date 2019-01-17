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
    
    public BoxCollider2D myBoxCollider;
	public GameObject hitBox;
	public HitCollider hitCollider;
    public PlayerSpecialAttacks playerSpecialAttacks;
    private CharacterEffectController characterEffectController;
    public SpecialMeter specialMeterObject;
    private SpecialMeter specialMeterController;
    public PlayerInfoManager playerInfoManager;


    public bool canDash;
    public bool canMove;
    public bool canRun = false;
    public bool canJump = true;
    public bool isFacingRight = true;
	public bool canGroundAttack;
	public bool canAirAttack;
    private float buttonTapCooldown = 0f;
    private int buttonCount = 0;
    private int currentComboRouteCount = 0;
    private int currentComboRouteType;
    private float comboRouteCooldown;
    private bool exKeyActive;
    private float exTapCooldown = 0f;
    

    public float jumpSpeed;

    public bool isAttacking = false;

    // ground checks
    // ==================================
    public GameObject groundCheckObject;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundType;

    private KeyCode previousMovementKey;
    private KeyCode currentMovementKey;
    public bool isGrounded;
    public bool pauseGroundCheck = false;
    public bool isDashing;
    public bool isDashingRight;
    public bool isRunning;
    public bool isJumping;
    public bool isFalling;
    private float previousYPosition;
    public Animator myAnim;
    public SpriteRenderer mySpriteRenderer;
    private bool isRunEffectPlaying = false;
    private GameObject ground;

    [Range(0f, 1f)]
    public float r = 1.0f;
    [Range(0f, 1f)]
    public float g = 1.0f;
    [Range(0f, 1f)]
    public float b = 1.0f;

    public static bool InputLeft = false, InputRight = false, InputUp = false, InputDown = false, InputNeutral = true;
    private float _LastX, _LastY;


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
            Debug.Log("comboRouteCooldown" + comboRouteCooldown);
            Debug.Log("currentComboRouteCount" + currentComboRouteCount);
        }
        // ==========================================================

        // ex key
        if (Input.GetButton("EXKey")) {
            print("exactive");
            exKeyActive = true;
            exTapCooldown = 0.3f;
            myAnim.SetBool("IsEXActive", true);
        }
        else {
            exKeyActive = false;
            myAnim.SetBool("IsEXActive", false);
        }

        if (exTapCooldown > 0f) {
            exTapCooldown -= 1 * Time.deltaTime ;
        }
        else {
            exTapCooldown = 0f;
            
        }

        if (!pauseGroundCheck) {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundType);
        }
        
        if (isGrounded) {            
            canDash = true;
            canAirAttack = false;            
            canGroundAttack = true;
            canJump = true;
        }
        
        if (comboRouteCooldown > 0f) {
            comboRouteCooldown -= 1 * Time.deltaTime ;
        }
        else {
            currentComboRouteCount = 0;
        }

        if (buttonTapCooldown > 0f) {
            buttonTapCooldown -= 1 * Time.deltaTime ;
            print(buttonCount);
        }
        else {
            buttonCount = 0;
            buttonTapCooldown = 0f;
        }

        if (buttonTapCooldown > 0 && buttonCount > 1) {
            if (myAnim.GetBool("IsWalking") && !isDashing && canMove && canDash && isGrounded) {                
                characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.RunDust02, isFacingRight);
                isDashing = true;
                canMove = false;                
                canGroundAttack = false;
                isAttacking = false;
                buttonCount = 0;
                isDashingRight = (Input.GetAxisRaw("Horizontal") > 0f) ? true : false;
                myAnim.SetBool("IsDashing", true);
                myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.MoveDash);

                MoveDash();
            }
        }

        if (isRunning) {
            if (!isRunEffectPlaying) {
                StartCoroutine(RunEffect());
                isRunEffectPlaying = true;
            }
        }

        if (Input.GetAxisRaw("Vertical") > 0f) {
            print("up");
        }
        if (Input.GetAxisRaw("Vertical") < 0f) {
            print("down");
        }

        if (Input.GetAxisRaw("DPadX") > 0) {
            print("right");
        }
        if (Input.GetAxisRaw("DPadX") < 0) {
            print("left");
        }

        if (canRun) {
            if (Input.GetAxisRaw("Horizontal") > 0f) {
                // input run right 
                myAnim.SetBool("IsRunning", true);
                isRunning = true;
                isDashing  = false;
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsDashing", false);
                myRigidbody.velocity = new Vector2(runSpeed, myRigidbody.velocity.y);
                transform.localScale = new Vector2(1f, 1f);
                isFacingRight = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f) {
                // input run left
                myAnim.SetBool("IsRunning", true);
                isRunning = true;
                isDashing  = false;
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsDashing", false);
                myRigidbody.velocity = new Vector2(-runSpeed, myRigidbody.velocity.y);
                transform.localScale = new Vector2(-1f, 1f);
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
                isRunEffectPlaying = false;
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            }
        }

        if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.W))) {
            if (Input.GetAxisRaw("Horizontal") > 0f) {
                currentMovementKey = KeyCode.D;
            }
            else if (Input.GetAxisRaw("Vertical") > 0f) {
                currentMovementKey = KeyCode.W;
            }
            else if (Input.GetAxisRaw("Vertical") < 0f) {
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
        

        #region Movement
        if (canJump && !isAttacking) {
            if (Input.GetButtonDown("Jump") && isGrounded) {
                //myAnim.SetBool("IsGrounded", false);
                pauseGroundCheck = true;
                isGrounded = false;
                canDash = false;
                canGroundAttack = false;
                canAirAttack = true;
                isJumping = true;                
                previousYPosition = gameObject.transform.position.y;
                myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.MoveJump);
                characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.JumpDust02, isFacingRight);                
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed + 15f);                
                myAnim.SetBool("IsJumping", isJumping);
                canJump = false;
                StartCoroutine(UnpauseGroundCheck());
                //soundJump.Play();
            }
        }

        if (isJumping) {
            if (!isFalling) {
                if (gameObject.transform.position.y >= previousYPosition) {
                previousYPosition = gameObject.transform.position.y;
                }
                else {
                    myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.AirFalling);
                    isFalling = true;
                    isJumping = false;
                    myAnim.SetBool("IsJumping", isJumping);
                }
            }
		}

        if (isFalling) {
            float distanceToGround = groundCheck.position.y - ground.transform.position.y;
			//print(distanceToGround);
			//print("");

			if (distanceToGround <= 2.65f) {
				canAirAttack = false;
			}

            if (isGrounded) {
                isJumping = false;			
                isFalling = false;
                myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.AirLanding);
            }
        }

        if (canMove && !isAttacking) {  
            // movement left and right
            if (Input.GetAxisRaw("Horizontal") > 0f) {
                // input walk right
                myAnim.SetBool("IsWalking", true);
                myAnim.SetBool("IsRunning", false);
                myRigidbody.velocity = new Vector2(activeMoveSpeed, myRigidbody.velocity.y);
                transform.localScale = new Vector2(1f, 1f);
                isFacingRight = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f) {
                // input walk left
                myAnim.SetBool("IsWalking", true);
                myAnim.SetBool("IsRunning", false);
                myRigidbody.velocity = new Vector2(-activeMoveSpeed, myRigidbody.velocity.y);
                transform.localScale = new Vector2(-1f, 1f);
                isFacingRight = false;
            }
            else {
                // idle
                myAnim.SetBool("IsWalking", false);                
                myAnim.SetBool("IsRunning", false);
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            }
            
        }
        #endregion

        #region Attacks
        // ground attacks
        if (canGroundAttack && !isAttacking && !isRunning && !isDashing) {            
            // normal attacks
            // ==========================================
            if (Input.GetButtonDown("AButton"))
            {
                // stop movement velocity when attacking
                if (!isDashing) {
                    myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                }
                isAttacking = true;
                canMove = false;
                canJump = false;
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
            if (!exKeyActive) {
                if (buttonTapCooldown > 0 && (Input.GetAxisRaw("Vertical") > 0f || InputUp) && Input.GetButtonDown("BButton")) {
                    // shoryuken
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Shoryuken);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialShoryuken);
                }
                else if (buttonTapCooldown > 0 && ( (Input.GetAxisRaw("Horizontal") < 0f) || (Input.GetAxisRaw("Horizontal") > 0f) ) && Input.GetButtonDown("BButton")) {       
                    // tatsumaki             
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Tatsumaki);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialTatsumaki);                

                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.Tatsumaki, isFacingRight);                
                }
                else if (buttonTapCooldown > 0 && (Input.GetAxisRaw("Vertical") < 0f) && Input.GetButtonDown("BButton")) {           
                    // hard knee         
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.HardKnee);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHardKnee);
                    // float direction = isFacingRight ? 15f : -15f;
                    // myRigidbody.velocity = new Vector2(direction, myRigidbody.velocity.y);

                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.HardKnee, isFacingRight);
                }
                else if (buttonTapCooldown <= 0 && Input.GetButtonDown("BButton")) {       
                    // hadouken             
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);                
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHadouken);

                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.Hadouken, isFacingRight);                
                }
            }
            // ex attacks
            // ==========================================
            else if (exKeyActive && playerInfoManager.currentSpecialLevel > 0) {
                if ((Input.GetAxisRaw("Vertical") < 0f) && Input.GetButtonDown("BButton")) {           
                    // hard knee         
                    
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.HardKnee, exKeyActive);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHardKnee);
                    myAnim.SetBool("IsEXActive", true);
                    StartCoroutine(PlayEXFlash(0.041f));
                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.HardKnee, isFacingRight, true);
                    playerInfoManager.ChangeSpecialLevel(-1);
                }
                else if (Input.GetButtonDown("BButton")) {     
                    
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Hadouken, exKeyActive);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHadouken);
                    myAnim.SetBool("IsEXActive", true);
                    StartCoroutine(PlayEXFlash(0.032f));
                    
                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.Hadouken, isFacingRight, true);                
                    playerInfoManager.ChangeSpecialLevel(-1);
                }
            }
              
        }

        if (canAirAttack && !isAttacking) {                
            // jump attacks
            // ==========================================
            if (Input.GetButtonDown("AButton"))
            {                
                isAttacking = true;
                myAnim.SetBool("IsAttacking", true);
                BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.JumpShort);
                //soundSwing3.Play();
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.JumpShort);                
                
            }
        }

        // dashing attacks
        if ((isDashing || isRunning) && !isJumping) {
            if (Input.GetButtonDown("AButton")) {                
                // myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                isAttacking = true;
                canRun = false;
                canMove = false;
                isRunning = false;
                isDashing = false;
                canJump = false;
                BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.RunningKick);
                characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.JumpDust01, isFacingRight);                
                myAnim.SetBool("IsAttacking", true);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialRunningKick);

                playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.RunningKick, isFacingRight);                
            }

            if (Input.GetButtonDown("BButton")) {
                // myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                isAttacking = true;
                canRun = false;
                canMove = false;
                isRunning = false;
                isDashing = false;
                canJump = false;
                BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.SolarPlexus);
                myAnim.SetBool("IsAttacking", true);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialSolarPlexus);

                playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.SolarPlexus, isFacingRight);                
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

    public void AdjustSpecialMeter(int amount) {
        playerInfoManager.AdjustSpecialMeter(amount);
    }

    #region RunEffect
    public IEnumerator RunEffect() {        
        while (isRunning && isGrounded) {
			characterEffectController.InitializeAnimationEffect((int)CharacterEffectsEnums.MovementEffectsType.RunDust, isFacingRight);
						
			yield return new WaitForSeconds (0.2f);
		}
    }
    #endregion

    private IEnumerator UnpauseGroundCheck() {
        yield return new WaitForSeconds(0.2f);
        pauseGroundCheck = false;
    }

    #region Hitstop
    public IEnumerator PlayHitStop(float hitStopDuration) {
        myAnim.enabled = false;
        yield return new WaitForSeconds(hitStopDuration);
        myAnim.enabled = true;
    }
    #endregion

    public IEnumerator PlayEXFlash(float duration)
    {
        var originalColor = mySpriteRenderer.color;
        while (duration >= 0f) {
            duration -= 1 * Time.deltaTime;
            var flashColor = new Color(r, g, b, 1);
            mySpriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.05f);
            mySpriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.05f);
        }
    }

    #region Move Dash
    public void MoveDash() {
        var direction = isDashingRight ? 4f : -4F; 

        var endPosition = new Vector3(transform.position.x + direction, transform.position.y, transform.position.z);        

        var speedDirection = isDashingRight ? 30f : -30F; 
        myRigidbody.velocity = new Vector2(speedDirection, myRigidbody.velocity.y);

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

}
