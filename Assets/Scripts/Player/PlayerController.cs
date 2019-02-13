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
    private float buttonTapCooldown = 0f;
    private int buttonCount = 0;
    private int currentComboRouteCount = 0;
    private int currentComboRouteType;
    private bool getComboCoolDown = true;
    private float comboRouteCooldown;
    private float buttonHeldTime = 0f;
    private bool trackButtonHeldTime = false;
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

    private GeneralEnums.MovementDirection previousMovementKey;
    private GeneralEnums.MovementDirection currentMovementKey;
    public bool isGrounded;
    public bool pauseGroundCheck = false;
    public bool isDashing;
    public bool isDashingRight;
    public bool isRunning;
    public bool isJumping;
    public bool isFalling;
    public bool slidePlayerOverObject = false;
    private float previousYPosition;
    public Animator myAnim;
    public SpriteRenderer mySpriteRenderer;
    public bool isRunEffectPlaying = false;
    public IEnumerator runEffectCoroutine;
    private GameObject ground;

    [Range(0f, 1f)]
    public float r = 1.0f;
    [Range(0f, 1f)]
    public float g = 1.0f;
    [Range(0f, 1f)]
    public float b = 1.0f;

    public static bool inputLeft = false, inputRight = false, inputUp = false, inputDown = false, inputNeutral = true, lastInput = false;
    private float _LastX, _LastY;
    public bool currentAxisGreaterThanPrevious = false;
    public bool initialTap = false;
    public bool secondTap = false;
    public bool tapRelease = false;


    public float previousAxisValue = 0;
    public float currentAxisValue = 0;

    public float previousHorizontalAxis = 0;
    public float previousDPadAxis = 0;


    public AudioSource soundSwing1;
    public AudioSource soundSwing2;
    public AudioSource soundSwing3;

    public AudioClip soundRyuSwing0;
    public AudioClip soundRyuSwing1;
    public AudioClip soundRyuSwing2;
    public AudioClip soundRyuSwing3;
    public AudioClip soundRyuSwing4;
    public AudioClip soundRyuSwing5;
    public AudioClip soundRyuSwing6;
    public AudioClip soundRyuHadouken;
	public AudioClip soundRyuTatsumaki;
	public AudioClip soundRyuShoryuken;	

    public AudioClip soundSwingLight0;
	public AudioClip soundSwingLight1;
	public AudioClip soundSwingMedium0;
    public AudioClip soundSwingMedium1;
    public AudioClip soundSwingMedium2;
	public AudioClip soundSwingHard0;
    public AudioClip soundSwingHard1;
    public AudioClip soundSwingHard2;

	public AudioClip soundHitLight0;
    public AudioClip soundHitLight1;
    public AudioClip soundHitLight2;
    public AudioClip soundHitLight3;
    public AudioClip soundHitMedium0;
    public AudioClip soundHitMedium1;
    public AudioClip soundHitMedium2;
    public AudioClip soundHitMedium3;
    public AudioClip soundHitHard0;
    public AudioClip soundHitHard1;
    public AudioClip soundHitHard2;
    public AudioClip soundHitHard3;
    public AudioClip soundExActivate;

    public AudioClip soundHadoukenSwing;

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

        runEffectCoroutine = RunEffect();


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
            hitBox.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.F4)) {
			switchComboStyleSystem = switchComboStyleSystem ? false : true;
		}
        // ==========================================================

        // ex key
        if (Input.GetButton("EXKey")) {
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
            comboRouteCooldown -= 1 * Time.deltaTime;
        }
        else if (getComboCoolDown) {
            currentComboRouteCount = 0;
            currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.None;
        }

        // if (buttonTapCooldown > 0f) {
        //     buttonTapCooldown -= 1 * Time.deltaTime;            
        // }
        // else {
        //     buttonCount = 0;
        //     buttonTapCooldown = 0f;
        // }

        GetPlayerMovementControl();
        GetPlayerAttackControl();

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

    #region Movement Control
    public void GetPlayerMovementControl() {
        // detect axis input
        if (Input.GetAxisRaw("DPadX") > 0 || Input.GetAxisRaw("Horizontal") > 0f) {
            //print("right");
            inputUp = false;
            inputDown = false;
            inputLeft = false;
            inputRight = true;
            inputNeutral = false;
            currentMovementKey = GeneralEnums.MovementDirection.Right;            
            
        }
        else if (Input.GetAxisRaw("DPadX") < 0 || Input.GetAxisRaw("Horizontal") < 0f) {
            inputUp = false;
            inputDown = false;
            inputLeft = true;
            inputRight = false;
            inputNeutral = false;
            lastInput = true;
            currentMovementKey = GeneralEnums.MovementDirection.Left;
        }
        else if (Input.GetAxisRaw("DPadY") > 0 || Input.GetAxisRaw("Vertical") > 0f) {
            inputUp = true;
            inputDown = false;
            inputLeft = false;
            inputRight = false;
            inputNeutral = false;
            lastInput = true;
            currentMovementKey = GeneralEnums.MovementDirection.Up;
        }
        else if (Input.GetAxisRaw("DPadY") < 0 || Input.GetAxisRaw("Vertical") < 0f) {
            inputUp = false;
            inputDown = true;
            inputLeft = false;
            inputRight = false;
            inputNeutral = false;
            lastInput = true;
            currentMovementKey = GeneralEnums.MovementDirection.Down;
        }
        else {
            inputUp = false;
            inputDown = false;
            inputLeft = false;
            inputRight = false;
            inputNeutral = true;
            lastInput = false;
            isRunning = false;
            isDashing = false;
        }

        var dPadAxis = Input.GetAxis("DPadX");
        var horizontalAxis = Input.GetAxis("Horizontal");  
        
        if (!inputNeutral) {
            if (previousMovementKey != currentMovementKey || isAttacking) {
                initialTap = false;
                tapRelease = false;
                secondTap = false;
            }
            // dpad right
            if (dPadAxis > previousDPadAxis) {                
                if (initialTap && currentMovementKey == previousMovementKey) {
                    secondTap = true;
                    print("secondtap");
                }       
                initialTap = true;
                buttonTapCooldown = 0.18f;             
            }
            else if (dPadAxis < previousDPadAxis && dPadAxis == 0){
                tapRelease = true;
                print("release");
            }

            // dpad left
            if (dPadAxis < previousDPadAxis) {                
                if (initialTap && currentMovementKey == previousMovementKey) {
                    secondTap = true;
                }
                initialTap = true;
                buttonTapCooldown = 0.18f;               
            }
            else if (dPadAxis > previousDPadAxis && dPadAxis <= 0){              
            }

            // horizontal right
            if (horizontalAxis > 0.1f) {      
                if (tapRelease && currentMovementKey == previousMovementKey) {
                    secondTap = true;
                }        
                initialTap = true;
                buttonTapCooldown = 0.18f;            
            }
            else if (initialTap && horizontalAxis > 0 && horizontalAxis < 0.1f) {
                tapRelease = true;
            }
            // print("previous - " + previousHorizontalAxis);
            // print("horizontal - " + horizontalAxis);

            // horizontal left
            if (horizontalAxis < -0.1f) {      
                if (tapRelease && currentMovementKey == previousMovementKey) {
                    secondTap = true;
                }
                initialTap = true;
                buttonTapCooldown = 0.18f;            
            }
            else if (initialTap && horizontalAxis < 0 && horizontalAxis > -0.1f) {
                tapRelease = true;
            }

            previousMovementKey = currentMovementKey;
        }
        previousAxisValue = currentAxisValue;
        previousHorizontalAxis = horizontalAxis;
        previousDPadAxis = dPadAxis;

        if (!isDashing && !isRunning) {
            canDashAttack = false;
        }

        // dashmovement
        if (buttonTapCooldown > 0 && secondTap) {
            if (myAnim.GetBool("IsWalking") && !isDashing && canMove && canDash && isGrounded && !isRunning) {                      
                characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.RunDust02, isFacingRight);

                canMove = false;                
                canGroundAttack = false;
                canDashAttack = true;
                isAttacking = false;
                buttonCount = 0;
                isDashingRight = (currentMovementKey == GeneralEnums.MovementDirection.Right) ? true : false;
                myAnim.SetBool("IsDashing", true);
                myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.MoveDash);

                MoveDash();
                buttonTapCooldown = 0;          
            }
        }

        if (isRunning) {
            if (!isRunEffectPlaying) {
                StartCoroutine(runEffectCoroutine);
                isRunEffectPlaying = true;
            }
        }
        else {
            StopCoroutine(runEffectCoroutine);
            isRunEffectPlaying = false;
        }

        if (canRun) {
            if (inputRight) {
                // input run right 
                myAnim.SetBool("IsRunning", true);
                isRunning = true;
                isDashing  = false;
                canDash = false;
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsDashing", false);

                if (!isGrounded) {
                    float raycastPositionY = !isGrounded ? -0.6f : transform.position.y;
                    Vector2 raycastStartingPosition = new Vector2(transform.position.x + 1,  raycastPositionY);
                    RaycastHit2D hit = Physics2D.Raycast(raycastStartingPosition, Vector2.right, 0.5f);
                    if (hit.collider == null) {
                        myRigidbody.velocity = new Vector2(runSpeed, myRigidbody.velocity.y);
                        transform.localScale = new Vector2(1f, 1f);
                    }                
                }
                else {
                    myRigidbody.velocity = new Vector2(runSpeed, myRigidbody.velocity.y);
                    transform.localScale = new Vector2(1f, 1f);
                }
                
                isFacingRight = true;
            }
            else if (inputLeft) {
                // input run left
                myAnim.SetBool("IsRunning", true);
                isRunning = true;
                isDashing  = false;
                canDash = false;
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsDashing", false);
                
                if (!isGrounded) {
                    float raycastPositionY = !isGrounded ? -0.6f : transform.position.y;
                    Vector2 raycastStartingPosition = new Vector2(transform.position.x - 1,  raycastPositionY);
                    RaycastHit2D hit = Physics2D.Raycast(raycastStartingPosition, Vector2.left, 0.5f);
                    if (hit.collider == null) {
                        myRigidbody.velocity = new Vector2(-runSpeed, myRigidbody.velocity.y);
                        transform.localScale = new Vector2(-1f, 1f);
                    }                
                }
                else {
                    myRigidbody.velocity = new Vector2(-runSpeed, myRigidbody.velocity.y);
                    transform.localScale = new Vector2(-1f, 1f);
                }

                isFacingRight = false;
            }
            else {
                // idle
                myAnim.SetBool("IsWalking", false);                
                myAnim.SetBool("IsRunning", false);
                myAnim.SetBool("IsDashing", false);                
                canMove = true;
                canRun = false;                
                isRunEffectPlaying = false;
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            }
        }
        
        if (initialTap) {
            if (buttonTapCooldown > 0f) {
                buttonTapCooldown -= 1 * Time.deltaTime;            
            }
            else {
                initialTap = false;
                secondTap = false;
                tapRelease = false;
                buttonTapCooldown = 0f;
            }
        }        
        
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

        if (!isGrounded) {
            Vector2 leftRaycastStartingPosition = new Vector2(transform.position.x - 0.8f, transform.position.y - 0.2f );
            Vector2 leftMiddleRaycastStartingPosition = new Vector2(transform.position.x - 0.0f, transform.position.y - 0.2f );
            Vector2 middleLeftRaycastStartingPosition = new Vector2(transform.position.x - 0.2f, transform.position.y - 0.6f );
			Vector2 middleRightRaycastStartingPosition = new Vector2(transform.position.x + 0.2f, transform.position.y - 0.6f );
            Vector2 rightMiddleRaycastStartingPosition = new Vector2(transform.position.x + 0.0f, transform.position.y - 0.2f );
            Vector2 rightRaycastStartingPosition = new Vector2(transform.position.x + 0.8f, transform.position.y - 0.2f );
            
            int layerMask = LayerMask.GetMask("Enemy");

            RaycastHit2D leftRaycastHit = Physics2D.Raycast(leftRaycastStartingPosition, Vector2.down, 1.5f, layerMask);
            RaycastHit2D leftMiddleRaycastHit = Physics2D.Raycast(leftMiddleRaycastStartingPosition, Vector2.down, 1.5f, layerMask);
            RaycastHit2D middleLeftRaycastHit = Physics2D.Raycast(middleLeftRaycastStartingPosition, Vector2.down, 1.5f, layerMask);
			RaycastHit2D middleRightRaycastHit = Physics2D.Raycast(middleRightRaycastStartingPosition, Vector2.down, 1.5f, layerMask);
            RaycastHit2D rightRaycastHit = Physics2D.Raycast(rightRaycastStartingPosition, Vector2.down, 1.5f, layerMask);
            RaycastHit2D rightMiddleRaycastHit = Physics2D.Raycast(rightMiddleRaycastStartingPosition, Vector2.down, 1.5f, layerMask);
            if (leftRaycastHit.collider != null && leftMiddleRaycastHit.collider != null) {
                var enemyController = leftRaycastHit.transform.GetComponent<EnemyController>();
                if (enemyController.isCornered && !enemyController.isFacingRight) {
                    StartCoroutine(enemyController.SlideObjectOutOfCorner(true));
                }
                Debug.DrawRay(leftRaycastStartingPosition, leftRaycastHit.point, Color.blue, 1.2f);
                Debug.DrawRay(leftMiddleRaycastStartingPosition, leftMiddleRaycastHit.point, Color.blue, 1.2f);
                if (!slidePlayerOverObject) {
                    slidePlayerOverObject = true;
                    StartCoroutine(SlideOverObject(false, 9f));
                }
                
            }
            else if (rightMiddleRaycastHit.collider != null && rightRaycastHit.collider != null) {
                var enemyController = rightMiddleRaycastHit.transform.GetComponent<EnemyController>();
                if (enemyController.isCornered && enemyController.isFacingRight) {
                     StartCoroutine(enemyController.SlideObjectOutOfCorner(false));
                }
                else {
                    Debug.DrawRay(rightRaycastStartingPosition, rightRaycastHit.point, Color.green, 1.2f);
                    Debug.DrawRay(rightMiddleRaycastStartingPosition, rightMiddleRaycastHit.point, Color.green, 1.2f);
                    if (!slidePlayerOverObject) {
                        slidePlayerOverObject = true;
                        StartCoroutine(SlideOverObject(true, 9f));
                    }
                }
                
            }
            // else if (middleLeftRaycastHit.collider != null && middleRightRaycastHit.collider != null) {
			// 	Debug.DrawRay(middleLeftRaycastStartingPosition, middleLeftRaycastHit.point, Color.gray, 1.2f);
			// 	Debug.DrawRay(middleRightRaycastStartingPosition, middleRightRaycastHit.point, Color.grey, 1.2f);
			// 	if (!slidePlayerOverObject) {
			// 		slidePlayerOverObject = true;
			// 		StartCoroutine(SlideOverObject(isFacingRight, 4f));
			// 	}
			// }
            else {
                slidePlayerOverObject = false;
            }
        }
        else {
            slidePlayerOverObject = false;
            float startingXPosition = isFacingRight ? 1.3f : -1.3f;

            int layerMask = LayerMask.GetMask("Enemy", "Wall");
            Vector2 raycastPosition = new Vector2(transform.position.x + startingXPosition, transform.position.y);
            RaycastHit2D raycastHit = Physics2D.Raycast(raycastPosition, Vector2.left, 0.3f, layerMask);
            if (raycastHit.collider != null) {
                Debug.DrawRay(raycastPosition, raycastHit.point, Color.red, 1.2f);
                myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            }      
        }

        if (canMove && !isAttacking && !isDashing && !isRunning) {  
            // movement left and right
            if (inputRight) {
                // input walk right
                myAnim.SetBool("IsWalking", true);
                myAnim.SetBool("IsRunning", false);
                transform.localScale = new Vector2(1f, 1f);
                isFacingRight = true;

                int layerMask = LayerMask.GetMask("Enemy", "Wall");
                Vector2 raycastStartingPosition = new Vector2(transform.position.x + 0.7f, transform.position.y - 1.5f);
                RaycastHit2D hit = Physics2D.Raycast(raycastStartingPosition, Vector2.right, 0.3f, layerMask);
                if (hit.collider != null) {
                    Debug.DrawRay(raycastStartingPosition, hit.point, Color.red, 1.2f);                    
                    myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
                }                
                else {                    
                    myRigidbody.velocity = new Vector2(activeMoveSpeed, myRigidbody.velocity.y);
                }                
            }
            else if (inputLeft) {
                // input walk left
                myAnim.SetBool("IsWalking", true);
                myAnim.SetBool("IsRunning", false);
                transform.localScale = new Vector2(-1f, 1f);
                isFacingRight = false;

                int layerMask = LayerMask.GetMask("Enemy", "Wall");
                float raycastPositionY = transform.position.y;
                Vector2 raycastStartingPosition = new Vector2(transform.position.x - 0.7f, raycastPositionY - 1.5f);
                RaycastHit2D hit = Physics2D.Raycast(raycastStartingPosition, Vector2.left, 0.3f, layerMask);
                if (hit.collider != null) {
                    Debug.DrawRay(raycastStartingPosition, hit.point, Color.red, 1.2f);
                    myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
                }                
                else {                    
                    myRigidbody.velocity = new Vector2(-activeMoveSpeed, myRigidbody.velocity.y);                    
                }                
            }
            else {
                // idle
                myAnim.SetBool("IsWalking", false);                
                myAnim.SetBool("IsRunning", false);
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            }
            
        }

          
    }
    #endregion

    #region Attacks Control
    public void GetPlayerAttackControl() {
        if (trackButtonHeldTime) {
            buttonHeldTime += 1 * Time.deltaTime;
            // print(buttonHeldTime);
        }

        if (Input.GetButtonUp("AButton")) {
            if (currentComboRouteCount == 2 && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {                    
                BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Overhead);
                SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing1, soundRyuSwing2);
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium2, soundSwingMedium0);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackOverhead);
                comboRouteCooldown = 1.1f;
                currentComboRouteCount++;
                getComboCoolDown = true;

                attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Overhead, 1);

            }
            trackButtonHeldTime = false;                
            buttonHeldTime = 0;                
            //print("buttonhledtime: " + result);
        }

        // ground attacks
        if (canGroundAttack && !isAttacking && !isRunning && !isDashing) {   
            
            // normal attacks
            // ==========================================
            if (Input.GetButtonDown("AButton")) {
                
                // stop movement velocity when attacking
                if (!isDashing) {
                    myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
                }
                isAttacking = true;
                canMove = false;
                canJump = false;
                myAnim.SetBool("IsAttacking", true);
                if (currentComboRouteCount == 0 && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.None) {       
                    
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Jab);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing0, soundRyuSwing1);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingLight0, soundSwingLight1);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackJab);
                    comboRouteCooldown = 0.62f;
                    currentComboRouteCount++;
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.AAA;
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Jab, 1);
                }                
                else if (currentComboRouteCount == 1 && 0f < comboRouteCooldown && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Short);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing4, soundRyuSwing6);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium0, soundSwingMedium1);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackShort);
                    comboRouteCooldown = 0.8f;
                    currentComboRouteCount++;
                    
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Short, 1);
                    trackButtonHeldTime = true;                                     
                }
                else if (currentComboRouteCount == 2 && 0 < comboRouteCooldown && comboRouteCooldown < 0.4f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Fierce);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing1, soundRyuSwing2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium2, soundSwingMedium0);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackFierce);
                    comboRouteCooldown = 1.3f;
                    currentComboRouteCount++;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Fierce, 1);

                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.AA_AA;
                }
                else if (currentComboRouteCount == 2 && comboRouteCooldown > 0.4f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Strong);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing2, soundRyuSwing3);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackStrong);
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Strong, 1);
                    
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.None;
                    comboRouteCooldown = 0.3f;
                    currentComboRouteCount = 0;
                }

                
                else if (currentComboRouteCount == 3 && comboRouteCooldown > 0f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AA_AA) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.HighRoundhouse);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing3);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackHighRoundhouse);
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.None;
                    comboRouteCooldown = 0f;
                    currentComboRouteCount = 0;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.HighRoundhouse, 1);
                }  
                
                else if (currentComboRouteCount == 3 && comboRouteCooldown > 0f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.CrouchForward);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing6, soundRyuSwing0);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackCrouchForward);
                    comboRouteCooldown = 0.8f;
                    currentComboRouteCount++;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.CrouchForward, 1);
                }  
                else if (currentComboRouteCount == 4 && comboRouteCooldown > 0f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.CrouchFierce);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing3);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackCrouchFierce);
                    comboRouteCooldown = 0.95f;
                    currentComboRouteCount++;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.CrouchFierce, 1);
                }  
                else if (currentComboRouteCount == 5 && comboRouteCooldown > 0f && currentComboRouteType == (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.CloseForward);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing4, soundRyuSwing2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackCloseForward);
                    currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.None;
                    comboRouteCooldown = 0;
                    currentComboRouteCount = 0;

                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.CloseForward, 1);
                }      
                
            }

            if (currentComboRouteCount == 2 && buttonHeldTime > 0.6f && currentComboRouteType != (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA) {
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.AttackOverheadIdle);                
                currentComboRouteType = (int)PlayerAttackEnums.ComboRouteTypes.AAHoldAAA;
                getComboCoolDown = false;
            }

            
            

            // special attacks
            // ==========================================
            if (!exKeyActive) {
                if ((Input.GetAxisRaw("Vertical") > 0f || inputUp) && Input.GetButtonDown("BButton")) {
                    // shoryuken
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Shoryuken);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialShoryuken);
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Shoryuken, 1);

                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard2, soundSwingHard1);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuShoryuken);
                }
                else if (buttonTapCooldown > 0 && ( inputLeft || inputRight ) && Input.GetButtonDown("BButton")) {       
                    // tatsumaki             
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Tatsumaki);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialTatsumaki);
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.Tatsumaki, 1);

                    characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.JumpDust01, isFacingRight);
                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.Tatsumaki, isFacingRight);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuTatsumaki);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium1, soundSwingMedium2);
                }
                else if ( (inputDown) && Input.GetButtonDown("BButton")) {           
                    // hard knee         
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.HardKnee);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHardKnee);
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.HardKnee, 1);
                    // float direction = isFacingRight ? 15f : -15f;
                    // myRigidbody.velocity = new Vector2(direction, myRigidbody.velocity.y);

                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard2);
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
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuHadouken);
                    SoundEffectsManager.instance.RandomizeSfx(0.3f, soundHadoukenSwing);
                }
            }
            // ex attacks
            // ==========================================
            else if (exKeyActive && playerInfoManager.currentSpecialLevel > 0) {
                if ((inputDown) && Input.GetButtonDown("BButton")) {           
                    // hard knee         
                    
                    isAttacking = true;
                    canMove = false;
                    canJump = false;
                    myAnim.SetBool("IsAttacking", true);
                    BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.HardKnee, exKeyActive);
                    myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialHardKnee);
                    myAnim.SetBool("IsEXActive", true);
                    attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.HardKnee, 1, true);

                    StartCoroutine(PlayEXFlash(0.041f));
                    playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.HardKnee, isFacingRight, true);
                    playerInfoManager.ChangeSpecialLevel(-1);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing3, soundRyuSwing5);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundExActivate);
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
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuHadouken);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundExActivate);
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
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.JumpShort);
                attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.JumpShort, 1);

                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundRyuSwing0, soundRyuSwing2);
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingLight1, soundSwingMedium0);         
                
            }
        }

        // dashing attacks
        if ((canDashAttack) && !isJumping) {
            if (Input.GetButtonDown("AButton")) {
                isAttacking = true;
                canRun = false;
                canMove = false;
                isRunning = false;
                isDashing = false;
                canJump = false;
                canDash = false;
                canDashAttack = false;
                BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.RunningKick);
                characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.JumpDust01, isFacingRight);
                myAnim.SetBool("IsAttacking", true);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialRunningKick);
                attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.RunningKick, 1);

                SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing4);
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium1, soundSwingMedium0);
                playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.RunningKick, isFacingRight);
            }

            if (Input.GetButtonDown("BButton")) {
                isAttacking = true;
                canRun = false;
                canMove = false;
                isRunning = false;
                isDashing = false;
                canJump = false;
                canDash = false;
                canDashAttack = false;
                BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.SolarPlexus);
                myAnim.SetBool("IsAttacking", true);
                myAnim.SetTrigger(PlayerAttackEnums.AttackTriggerNames.SpecialSolarPlexus);
                attackFramesManager.SetAttackFrames((int)PlayerAttackEnums.RyuAttacks.SolarPlexus, 1);

                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundRyuSwing3, soundRyuSwing2);
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard1, soundSwingMedium0);
                playerSpecialAttacks.InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks.SolarPlexus, isFacingRight);                
            }
        }
    }
    #endregion

    public void AdjustSpecialMeter(int amount) {
        playerInfoManager.AdjustSpecialMeter(amount);
    }

    #region PlaySoundEffect
    public void PlaySoundEffect(int attackId) {
        if (attackId == (int)PlayerAttackEnums.RyuAttacks.Jab) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitLight0, soundHitLight1);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.Short) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitLight2, soundHitMedium0);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.Strong) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard0, soundHitMedium2);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.Fierce) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard0, soundHitMedium3);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.HighRoundhouse) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard1, soundHitMedium2);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.Overhead) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard1, soundHitMedium1);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.CrouchForward) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitLight1, soundHitLight2);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.CrouchFierce) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard0, soundHitMedium2);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.CloseForward) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard3, soundHitMedium3);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.Shoryuken) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard2, soundHitHard3);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.Tatsumaki) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitMedium0, soundHitMedium3);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.HardKnee) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard3, soundHitHard2);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.RunningKick) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitMedium1, soundHitMedium2);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.SolarPlexus) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard0, soundHitHard1);
        }
        else if (attackId == (int)PlayerAttackEnums.RyuAttacks.JumpShort) {
            SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitLight2, soundHitMedium0);
        }
    }
    #endregion
    
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
        isRunEffectPlaying = false;
    }

    public IEnumerator SlideOverObject(bool isSlidingLeft, float speed = 10f, bool pushEnemyOutOfCorner = false) {
        while (slidePlayerOverObject) {
            if (isSlidingLeft) {
                myRigidbody.velocity = new Vector2(-1 * speed, myRigidbody.velocity.y - 0.8f);
            }
            else {
                myRigidbody.velocity = new Vector2(speed, myRigidbody.velocity.y - 0.8f);
            }
						
			yield return new WaitForSeconds (0.01f);
		}
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
        print(hitStopDuration);
        yield return new WaitForSeconds(hitStopDuration);
        myAnim.enabled = true;
        print("myAnim.enabled = true;");
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

    #region StartAttackFrameTracking
    public void StartAttackFrameTracking() {
        //StartCoroutine(attackFramesManager.PlayAttackFramesRoutine());
        print("call strat framtracking");
        attackFramesManager.StartFrameTracking();

    }
    #endregion

    #region Move Dash
    public void MoveDash() {
        print("MOVEINTG DASH");
        if (!isDashing) {
            isDashing = true;
            var direction = isDashingRight ? 4f : -4F; 

            var speedDirection = isDashingRight ? 30f : -30F; 
            myRigidbody.velocity = new Vector2(speedDirection, myRigidbody.velocity.y);
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
