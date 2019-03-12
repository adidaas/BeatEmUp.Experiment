using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    public PlayerInfoManager playerInfoManager;
    public GameObject hitBoxGameObject;
    public Rigidbody2D myRigidbody;
    private Animator myAnim;
    private CharacterEffectController characterEffectController;

    public GeneralEnums.MovementDirection previousMovementKey;
    public GeneralEnums.MovementDirection currentMovementKey;

    private float activeMoveSpeed = 10f;
    private float runSpeed = 20f;
    public float jumpSpeed = 10f;
    private float previousYPosition;

    public bool slidePlayerOverObject = false;
    public bool isRunEffectPlaying = false;

    public IEnumerator runEffectCoroutine;

    // Start is called before the first frame update
    void Start() {
        playerController = GetComponent<PlayerController>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        characterEffectController = GetComponent<CharacterEffectController>();
        
        runEffectCoroutine = RunEffect();
    }

    // Update is called once per frame
    void Update() {
        if (playerController.isJumping) {
            if (!playerController.isFalling) {
                if (gameObject.transform.position.y >= previousYPosition) {
                    previousYPosition = gameObject.transform.position.y;
                }
                else {
                    myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.AirFalling);
                    playerController.isFalling = true;
                    playerController.isJumping = false;
                    myAnim.SetBool("IsJumping", playerController.isJumping);
                }
            }
		}

        if (playerController.isFalling) {
            float distanceToGround = playerController.groundCheck.position.y - playerController.ground.transform.position.y;

			if (distanceToGround <= 2.65f) {
				playerController.canAirAttack = false;
			}

            if (playerController.isGrounded && !playerController.isBeingAirJuggle) {
                playerController.isJumping = false;			
                playerController.isFalling = false;
                myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.AirLanding);
            }
        }

        if (!playerController.isGrounded) {
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
			// 		StartCoroutine(SlideOverObject(playerController.isFacingRight, 4f));
			// 	}
			// }
            else {
                slidePlayerOverObject = false;
            }
        }
        else {
            slidePlayerOverObject = false;
            float startingXPosition = playerController.isFacingRight ? 1.3f : -1.3f;

            int layerMask = LayerMask.GetMask("Enemy", "Wall");
            Vector2 raycastPosition = new Vector2(transform.position.x + startingXPosition, transform.position.y);
            RaycastHit2D raycastHit = Physics2D.Raycast(raycastPosition, Vector2.left, 0.3f, layerMask);
            if (raycastHit.collider != null) {
                Debug.DrawRay(raycastPosition, raycastHit.point, Color.red, 1.2f);
                myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            }      
        }

        if (playerController.canMove) {
            GetPlayerMovementControl();
        }
    }
    
    #region Movement Control
    public void GetPlayerMovementControl() {
        // detect axis input
        if (Input.GetAxisRaw("DPadX") > 0 || Input.GetAxisRaw("Horizontal") > 0f) {
            //print("right");
            playerController.inputUp = false;
            playerController.inputDown = false;
            playerController.inputLeft = false;
            playerController.inputRight = true;
            playerController.inputNeutral = false;
            currentMovementKey = GeneralEnums.MovementDirection.Right;            
            
        }
        else if (Input.GetAxisRaw("DPadX") < 0 || Input.GetAxisRaw("Horizontal") < 0f) {
            playerController.inputUp = false;
            playerController.inputDown = false;
            playerController.inputLeft = true;
            playerController.inputRight = false;
            playerController.inputNeutral = false;
            playerController.lastInput = true;
            currentMovementKey = GeneralEnums.MovementDirection.Left;
        }
        else if (Input.GetAxisRaw("DPadY") > 0 || Input.GetAxisRaw("Vertical") > 0f) {
            playerController.inputUp = true;
            playerController.inputDown = false;
            playerController.inputLeft = false;
            playerController.inputRight = false;
            playerController.inputNeutral = false;
            playerController.lastInput = true;
            currentMovementKey = GeneralEnums.MovementDirection.Up;
        }
        else if (Input.GetAxisRaw("DPadY") < 0 || Input.GetAxisRaw("Vertical") < 0f) {
            playerController.inputUp = false;
            playerController.inputDown = true;
            playerController.inputLeft = false;
            playerController.inputRight = false;
            playerController.inputNeutral = false;
            playerController.lastInput = true;
            currentMovementKey = GeneralEnums.MovementDirection.Down;
        }
        else {
            playerController.inputUp = false;
            playerController.inputDown = false;
            playerController.inputLeft = false;
            playerController.inputRight = false;
            playerController.inputNeutral = true;
            playerController.lastInput = false;
            playerController.isRunning = false;
            playerController.isDashing = false;
        }

        var dPadAxis = Input.GetAxis("DPadX");
        var horizontalAxis = Input.GetAxis("Horizontal");  
        
        if (!playerController.inputNeutral) {
            if (previousMovementKey != currentMovementKey || playerController.isAttacking) {
                playerController.initialTap = false;
                playerController.tapRelease = false;
                playerController.secondTap = false;
            }
            // dpad right
            if (dPadAxis > playerController.previousDPadAxis) {                
                if (playerController.initialTap && currentMovementKey == previousMovementKey) {
                    playerController.secondTap = true;
                }       
                playerController.initialTap = true;
                playerController.buttonTapCooldown = 0.18f;             
            }
            else if (dPadAxis < playerController.previousDPadAxis && dPadAxis == 0){
                playerController.tapRelease = true;
            }

            // dpad left
            if (dPadAxis < playerController.previousDPadAxis) {                
                if (playerController.initialTap && currentMovementKey == previousMovementKey) {
                    playerController.secondTap = true;
                }
                playerController.initialTap = true;
                playerController.buttonTapCooldown = 0.18f;               
            }
            else if (dPadAxis > playerController.previousDPadAxis && dPadAxis <= 0){              
            }

            // horizontal right
            if (horizontalAxis > 0.1f) {      
                if (playerController.tapRelease && currentMovementKey == previousMovementKey) {
                    playerController.secondTap = true;
                }        
                playerController.initialTap = true;
                playerController.buttonTapCooldown = 0.18f;            
            }
            else if (playerController.initialTap && horizontalAxis > 0 && horizontalAxis < 0.1f) {
                playerController.tapRelease = true;
            }
            // print("previous - " + playerController.previousHorizontalAxis);
            // print("horizontal - " + horizontalAxis);

            // horizontal left
            if (horizontalAxis < -0.1f) {      
                if (playerController.tapRelease && currentMovementKey == previousMovementKey) {
                    playerController.secondTap = true;
                }
                playerController.initialTap = true;
                playerController.buttonTapCooldown = 0.18f;            
            }
            else if (playerController.initialTap && horizontalAxis < 0 && horizontalAxis > -0.1f) {
                playerController.tapRelease = true;
            }

            previousMovementKey = currentMovementKey;
        }
        playerController.previousAxisValue = playerController.currentAxisValue;
        playerController.previousHorizontalAxis = horizontalAxis;
        playerController.previousDPadAxis = dPadAxis;

        if (!playerController.isDashing && !playerController.isRunning) {
            playerController.canDashAttack = false;
        }

        // dashmovement
        if (playerController.buttonTapCooldown > 0 && playerController.secondTap) {
            if (myAnim.GetBool("IsWalking") && !playerController.isDashing && playerController.canMove && playerController.canDash && playerController.isGrounded && !playerController.isRunning) {                      
                characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.RunDust02, playerController.isFacingRight);

                playerController.canMove = false;                
                playerController.canGroundAttack = false;
                playerController.canDashAttack = true;
                playerController.isAttacking = false;
                playerController.buttonCount = 0;
                playerController.isDashingRight = (currentMovementKey == GeneralEnums.MovementDirection.Right) ? true : false;
                myAnim.SetBool("IsDashing", true);
                myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.MoveDash);

                MoveDash();
                playerController.buttonTapCooldown = 0;          
            }
        }

        if (playerController.isRunning) {
            if (!isRunEffectPlaying) {
                StartCoroutine(runEffectCoroutine);
                isRunEffectPlaying = true;
            }
        }
        else {
            StopCoroutine(runEffectCoroutine);
            isRunEffectPlaying = false;
        }

        if (playerController.canRun) {
            if (playerController.inputRight) {
                // input run right 
                myAnim.SetBool("IsRunning", true);
                playerController.isRunning = true;
                playerController.isDashing  = false;
                playerController.canDash = false;
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsDashing", false);

                if (!playerController.isGrounded) {
                    float raycastPositionY = !playerController.isGrounded ? -0.6f : transform.position.y;
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
                
                playerController.isFacingRight = true;
            }
            else if (playerController.inputLeft) {
                // input run left
                myAnim.SetBool("IsRunning", true);
                playerController.isRunning = true;
                playerController.isDashing  = false;
                playerController.canDash = false;
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsDashing", false);
                
                if (!playerController.isGrounded) {
                    float raycastPositionY = !playerController.isGrounded ? -0.6f : transform.position.y;
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

                playerController.isFacingRight = false;
            }
            else {
                // idle
                myAnim.SetBool("IsWalking", false);                
                myAnim.SetBool("IsRunning", false);
                myAnim.SetBool("IsDashing", false);                
                playerController.canMove = true;
                playerController.canRun = false;                
                isRunEffectPlaying = false;
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            }
        }
        
        if (playerController.initialTap) {
            if (playerController.buttonTapCooldown > 0f) {
                playerController.buttonTapCooldown -= 1 * Time.deltaTime;            
            }
            else {
                playerController.initialTap = false;
                playerController.secondTap = false;
                playerController.tapRelease = false;
                playerController.buttonTapCooldown = 0f;
            }
        }        
        
        // jumping
        if (playerController.canJump && !playerController.isAttacking) {
            if (Input.GetButtonDown("Jump") && playerController.isGrounded) {
                //myAnim.SetBool("playerController.isGrounded", false);
                playerController.pauseGroundCheck = true;
                playerController.isGrounded = false;
                playerController.canDash = false;
                playerController.canGroundAttack = false;
                playerController.canAirAttack = true;
                playerController.canJump = false;

                playerController.isJumping = true;                
                previousYPosition = gameObject.transform.position.y;
                myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.MoveJump);
                characterEffectController.InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType.JumpDust02, playerController.isFacingRight);                
                myRigidbody.velocity = new Vector2(0, jumpSpeed + 15f);
                myAnim.SetBool("IsJumping", playerController.isJumping);
                
                StartCoroutine(UnpauseGroundCheck());
                //soundJump.Play();
            }
        }

        if (playerController.canMove && !playerController.isAttacking && !playerController.isDashing && !playerController.isRunning) {  

            if (playerController.inputRight) {
                // input walk right

                myAnim.SetBool("IsWalking", true);
                myAnim.SetBool("IsRunning", false);
                transform.localScale = new Vector2(1f, 1f);
                playerController.isFacingRight = true;

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
            else if (playerController.inputLeft) {
                // input walk left
                myAnim.SetBool("IsWalking", true);
                myAnim.SetBool("IsRunning", false);
                transform.localScale = new Vector2(-1f, 1f);
                playerController.isFacingRight = false;

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

    private IEnumerator UnpauseGroundCheck() {
        yield return new WaitForSeconds(0.2f);
        playerController.pauseGroundCheck = false;
    }

    #region RunEffect
    public IEnumerator RunEffect() {        
        while (playerController.isRunning && playerController.isGrounded) {
			characterEffectController.InitializeAnimationEffect((int)CharacterEffectsEnums.MovementEffectsType.RunDust, playerController.isFacingRight);
						
			yield return new WaitForSeconds (0.2f);
		}
    }
    #endregion

    #region Move Dash
    public void MoveDash() {
        if (!playerController.isDashing) {
            playerController.isDashing = true;
            var direction = playerController.isDashingRight ? 4f : -4F; 

            var speedDirection = playerController.isDashingRight ? 30f : -30F; 
            myRigidbody.velocity = new Vector2(speedDirection, myRigidbody.velocity.y);
        }

    }
    #endregion
}
