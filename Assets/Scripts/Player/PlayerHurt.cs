using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerAudio playerAudio;
    private Animator myAnim;
    private Rigidbody2D myRigidbody;
    public IEnumerator hitStunCoroutine;
	private PlayerInfoManager playerInfoManager;

	private IEnumerator waitToPlayRoutine;

    public float currentGravityScale = 5f;
    private bool isBeingKnockedBack = false;
    public bool pauseAirSlideTracker = false;
	public bool isAnimationDone = false;
	private int currentStep = 0;

	public int playerHurtType;

    void Start() {
        playerController = gameObject.GetComponent<PlayerController>();
        playerAudio = gameObject.GetComponent<PlayerAudio>();
		playerInfoManager = playerController.playerInfoManager;
        myAnim = gameObject.GetComponent<Animator>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

	public bool HitWhileBlocking(int damageValue) {
		playerInfoManager.AdjustBlockValue(damageValue);

		// if guardValue is below or equal to 0, than player has been guard crush. return false to play guard crush animation
		if (playerInfoManager.currentGuardValue <= 0) {
			return false;
		}
		
		return true;
	}

    public void IsHit(int hurtType, float knockBackDistance, bool isEnemyFacingRight, float hitStop, int damageValue) {
		playerHurtType = hurtType;
		playerInfoManager.AdjustHealthMeter(damageValue);
        float verticalKnockBack = myRigidbody.velocity.y;
		float direction = 1.0f;
		float hitshakeDuration = 0.2f;
		//myRigidbody.velocity = new Vector2(0, 0);

		if (isEnemyFacingRight) {			
			playerController.isFacingRight = false;			
			transform.localScale = new Vector3(-1, 1, 1);
		}
		else {
			direction = -1f;
			playerController.isFacingRight = true;
			transform.localScale = new Vector3(1, 1, 1);
		}

        playerAudio.PlayHurtSound(playerController.playerCharacter, hurtType);

        if (!playerController.isFalling && !playerController.isJumping) {
			if (hurtType == (int)GeneralEnums.AttacksHurtType.High) {
                myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtHigh);			
                hitStunCoroutine = PerformHitStun(0.4f + hitStop, GeneralEnums.AttacksHurtType.High);				
            }
            else if (hurtType == (int)GeneralEnums.AttacksHurtType.Mid) {
                myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtMid);
                hitStunCoroutine = PerformHitStun(0.4f + hitStop, GeneralEnums.AttacksHurtType.Mid);
            }
            else if (hurtType == (int)GeneralEnums.AttacksHurtType.Launch) {
                myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtLaunch);
                hitStunCoroutine = PerformHitStun(hitStop, GeneralEnums.AttacksHurtType.Launch);
            }
            StartCoroutine(hitStunCoroutine);		
            StartCoroutine(PerformHitShake(hitshakeDuration));			
        }	

        if (!isBeingKnockedBack) {
			if (!playerController.isFalling && !playerController.isJumping) {
				verticalKnockBack = 0f;
			}
			else {
				myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtAir);
				//groundCheckRadius = 0.3f;
				knockBackDistance = 18.5f;
				currentGravityScale += 3.3f;
				myRigidbody.gravityScale = currentGravityScale;
				//verticalKnockBack = 15f;
				playerController.isBeingAirJuggle = true;
			}

			if (playerController.isCornered) {
				knockBackDistance = myRigidbody.velocity.x;
			}
			isBeingKnockedBack = true;
			
			myRigidbody.velocity = new Vector2(knockBackDistance * direction, verticalKnockBack);
			StartCoroutine(KnockBackWait());			
		}
    }

	public void HurtLaunch_MovePosition() {
		print("hurt launch ++++++++++++++++++++++++++++++");
		if (waitToPlayRoutine != null) {
			StopCoroutine(waitToPlayRoutine);
		}	
		
		Vector2 nextPosition = new Vector2();
		float animationDelay = 0f;
		float direction = playerController.isFacingRight ? -1.0f: 1.0f;
		if (currentStep == 0) {
			if (playerController.isCornered) {
				nextPosition = new Vector2 (myRigidbody.velocity.x * direction, myRigidbody.velocity.y + 23);
			}
			else {
				nextPosition = new Vector2 (myRigidbody.velocity.x + 24 * direction, myRigidbody.velocity.y + 23);
			}
			
			myRigidbody.velocity = nextPosition;
			animationDelay = 0.9f;			
			waitToPlayRoutine = WaitToPlay(animationDelay, GeneralEnums.AttacksHurtType.Launch);
			StartCoroutine(waitToPlayRoutine);
		}
		else if (currentStep == 1) {
			playerController.pauseGroundCheck = false;
			animationDelay = 0.5f;
			isAnimationDone = true;
			myRigidbody.gravityScale = 4f;
			waitToPlayRoutine = WaitToPlay(animationDelay, GeneralEnums.AttacksHurtType.Launch, isAnimationDone);
			StartCoroutine(waitToPlayRoutine);
		}
	}

    public IEnumerator PerformHitShake(float duration = 0.2f) {
		var initialPos = gameObject.transform.position;
		
		while (duration > 0) {
			var randomPoint = new Vector3(
				initialPos.x + (Random.insideUnitSphere.y * 0.10f), 
				initialPos.y,
				initialPos.z);
			transform.localPosition = randomPoint;
			duration -= Time.deltaTime * 1;
			yield return null;
		}
		
		transform.localPosition = initialPos;
	}

    public IEnumerator PerformHitStun(float hitStunTime, GeneralEnums.AttacksHurtType hurtType) {
		yield return new WaitForSeconds(hitStunTime);	
		print("hitstrun");
		if (hurtType == GeneralEnums.AttacksHurtType.High) {
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtHighRecover);
			playerController.canMove = true;
		}	
		else if (hurtType == GeneralEnums.AttacksHurtType.Mid) {
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtMidRecover);
			playerController.canMove = true;
		}	
		else if (hurtType == GeneralEnums.AttacksHurtType.Launch) {		
			print("launch");
			isAnimationDone = false;			
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtLaunch);
			HurtLaunch_MovePosition();
		}	
		// else if (hurtType == GeneralEnums.AttacksHurtType.LaunchBack) {		
		// 	HurtLaunchBack_MovePosition();
		// }	
	}

    public IEnumerator KnockBackWait() {		
		yield return new WaitForSeconds(0.2f);
		isBeingKnockedBack = false;
		pauseAirSlideTracker = false;
	}

	public IEnumerator ToggleHurtWakeUp() {
		var seconds = GetRecoveryTime(playerHurtType);
		float elapsedTime = 0;
		while (elapsedTime < seconds)
		{
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}		
		myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtWakeUp);
		playerController.isInInvincibleState = false;
		playerInfoManager.isInInvincibleState = false;
	}

	private IEnumerator WaitToPlay(float duration, GeneralEnums.AttacksHurtType hurtType, bool isAnimationDone = false) {		
		yield return new WaitForSeconds(duration);	
		currentStep++;

		if (isAnimationDone) {
			currentStep = 0;
			isAnimationDone = false;
			// if (hurtType == GeneralEnums.AttacksHurtType.LaunchBack) {
			// 	enemyController.isInLaunchBack = false;
			// 	StartCoroutine(ToggleHurtWakeUp());
			// }
			// else if (hurtType == GeneralEnums.AttacksHurtType.WallBounce) {;
			// 	enemyController.isInWallBounce = false;
			// 	enemyController.isInInvincibleState = false;
			// 	myRigidBody.bodyType = RigidbodyType2D.Dynamic;				
			// }
			
		}
		else if (hurtType == GeneralEnums.AttacksHurtType.Launch){
			HurtLaunch_MovePosition();
		}		
		else if (hurtType == GeneralEnums.AttacksHurtType.LaunchBack){
			//HurtLaunchBack_MovePosition();
		}		
	}

	public float GetRecoveryTime(int hurtType) {
		if (hurtType == 0) {
			return 0.24f;
		}	
		if (hurtType == 1) {
			return 0.24f;
		}
		if (hurtType == 2) {
			return 0.4f;
		}	
		if (hurtType == 3) {
			return 0.24f;
		}
		if (hurtType == 4) {
			return 0.45f;
		}
		if (hurtType == 5) {
			return 0.24f;
		}	
		return 3f;
	}
}
