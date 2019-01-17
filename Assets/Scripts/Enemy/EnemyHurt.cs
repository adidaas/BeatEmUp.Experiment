using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour {

	public GameObject parentObject;
	public EnemyController enemyController;
	private Rigidbody2D myRigidBody;
	private Animator myAnim;

	private Vector3 targetPosition;
	private Vector3 originalPosition;
	public int currentStep = 0;
	public bool isAnimationDone;
	public int hurtType;
	private IEnumerator waitToPlayRoutine;


	// Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    // Movement speed in units/sec.
    public float speed = 1.0F;

	void Start() {
		enemyController = parentObject.GetComponent<EnemyController>();
		myRigidBody = parentObject.GetComponent<Rigidbody2D>();
		myAnim = parentObject.GetComponent<Animator>();
	}

	public void HurtLaunch_MovePosition() {
		if (waitToPlayRoutine != null) {
			StopCoroutine(waitToPlayRoutine);
		}	
		isAnimationDone = false;
		Vector2 nextPosition = new Vector2();
		float animationDelay = 0f;
		float direction = enemyController.isFacingRight ? -1.0f: 1.0f;
		if (currentStep == 0) {
			if (enemyController.isCornered) {
				nextPosition = new Vector2 (myRigidBody.velocity.x * direction, myRigidBody.velocity.y + 22);
			}
			else {
				nextPosition = new Vector2 (myRigidBody.velocity.x + 13 * direction, myRigidBody.velocity.y + 22);
			}
			
			myRigidBody.velocity = nextPosition;
			animationDelay = 0.7f;			
			waitToPlayRoutine = WaitToPlay(animationDelay, GeneralEnums.AttacksHurtType.Launch);
			StartCoroutine(waitToPlayRoutine);
		}
		else if (currentStep == 1) {
			//nextPosition = new Vector2(myRigidBody.velocity.x + 0.5f, myRigidBody.velocity.y + 5);
			//myRigidBody.velocity = nextPosition;
			animationDelay = 0.5f;
			isAnimationDone = true;
			myRigidBody.gravityScale = 4f;
			waitToPlayRoutine = WaitToPlay(animationDelay, GeneralEnums.AttacksHurtType.Launch, isAnimationDone);
			StartCoroutine(waitToPlayRoutine);
		}
		// else if (currentStep == 1) {
		// 	nextPosition = new Vector2 (myRigidBody.velocity.x + 0.5f, myRigidBody.velocity.y - 1.5f);
		// 	myRigidBody.velocity = nextPosition;
		// 	myRigidBody.gravityScale = 5.5f;
		// 	animationDelay = 0.50f;
		// 	StartCoroutine(WaitToPlay(animationDelay, 0));
		// } else if (currentStep == 2) {
		// 	nextPosition = new Vector2(myRigidBody.velocity.x + 0.5f, myRigidBody.velocity.y + 5);
		// 	myRigidBody.velocity = nextPosition;
		// 	animationDelay = 0.5f;
		// 	isAnimationDone = true;
		// 	myRigidBody.gravityScale = 3f;
		// 	StartCoroutine(WaitToPlay(animationDelay, 0, isAnimationDone));
		// }
	}

	public void HurtLaunchBack_MovePosition() {
		if (waitToPlayRoutine != null) {
			StopCoroutine(waitToPlayRoutine);
		}	
		isAnimationDone = false;
		Vector2 nextPosition = new Vector2();
		float animationDelay = 0f;
		float direction = enemyController.isFacingRight ? -1.0f: 1.0f;
		if (currentStep == 0) {
			if (enemyController.isCornered) {
				nextPosition = new Vector2 (myRigidBody.velocity.x + 10f * direction * -1, myRigidBody.velocity.y + 24f);
			}
			else {
				nextPosition = new Vector2 (myRigidBody.velocity.x + 37f * direction, myRigidBody.velocity.y + 18f);
			}
			
			myRigidBody.velocity = nextPosition;
			animationDelay = 0.45f;			
			isAnimationDone = true;
			waitToPlayRoutine = WaitToPlay(animationDelay, GeneralEnums.AttacksHurtType.LaunchBack, isAnimationDone);
			StartCoroutine(waitToPlayRoutine);
		}
	}

	public void HurtWallBounce_MovePosition() {
		if (waitToPlayRoutine != null) {
			StopCoroutine(waitToPlayRoutine);
		}	
		Vector2 nextPosition = new Vector2();
		float animationDelay = 0f;
		float direction = enemyController.isFacingRight ? 1.0f: -1.0f;
		
		if (currentStep == 0) {
			nextPosition = new Vector2(transform.position.x + (3f * direction), transform.position.y + 6f);
			currentStep++;
			StartCoroutine(MovePosition(nextPosition, 28f));
			
		}
		else if (currentStep == 1) {
			enemyController.airJuggleBoxObject.SetActive(true);
			nextPosition = new Vector2(transform.position.x + (2f * direction), transform.position.y + 1f);
			currentStep++;
			StartCoroutine(MovePosition(nextPosition, 12f));
		}
		else if (currentStep == 2) {
			nextPosition = new Vector2 (myRigidBody.velocity.x + 10f * direction, myRigidBody.velocity.y - 4f);			
			myRigidBody.velocity = nextPosition;
			isAnimationDone = true;
			animationDelay = 0.1f;
		 	waitToPlayRoutine = WaitToPlay(animationDelay, GeneralEnums.AttacksHurtType.WallBounce, isAnimationDone);
			StartCoroutine(waitToPlayRoutine);
		}
		
		
		// if (currentStep == 0) {
		// 	if (enemyController.isCornered) {
		// 		nextPosition = new Vector2 (myRigidBody.velocity.x + 20f * direction * -1, myRigidBody.velocity.y + 15f);
		// 	}
		// 	else {
		// 		nextPosition = new Vector2 (myRigidBody.velocity.x + 45f * direction, myRigidBody.velocity.y + 20f);
		// 	}			
		// 	print("nexposition");
		// 	print(nextPosition);
		// 	myRigidBody.velocity = nextPosition;
		// 	animationDelay = 0.3f;			
		// 	isAnimationDone = true;	
		// 	waitToPlayRoutine = WaitToPlay(animationDelay, GeneralEnums.AttacksHurtType.WallBounce, isAnimationDone);
		// 	StartCoroutine(waitToPlayRoutine);
		// }
	}

	public IEnumerator MovePosition(Vector2 targetPosition, float speed) {
		while ((Vector2)transform.position != targetPosition) {
			transform.position = 
					Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
						
			yield return new WaitForEndOfFrame ();
		}

		if (!isAnimationDone) {
			if (hurtType == (int)GeneralEnums.AttacksHurtType.WallBounce) {
				HurtWallBounce_MovePosition();
			}
		}
	}

	private IEnumerator WaitToPlay(float duration, GeneralEnums.AttacksHurtType hurtType, bool isAnimationDone = false) {		
		yield return new WaitForSeconds(duration);	
		currentStep++;

		if (isAnimationDone) {
			currentStep = 0;
			isAnimationDone = false;
			if (hurtType == GeneralEnums.AttacksHurtType.LaunchBack) {
				enemyController.isInLaunchBack = false;
				StartCoroutine(enemyController.ToggleHurtWakeUp((int)hurtType));
			}
			else if (hurtType == GeneralEnums.AttacksHurtType.WallBounce) {;
				enemyController.isInWallBounce = false;
				enemyController.isInInvincibleState = false;
				myRigidBody.bodyType = RigidbodyType2D.Dynamic;				
			}
			
			//StartCoroutine(enemyController.ToggleHurtWakeUp((int)hurtType));
		}
		else if (hurtType == GeneralEnums.AttacksHurtType.Launch){
			HurtLaunch_MovePosition();
		}		
		else if (hurtType == GeneralEnums.AttacksHurtType.LaunchBack){
			HurtLaunchBack_MovePosition();
		}		
	}

	public IEnumerator PerformHitStun(float hitStunTime, GeneralEnums.AttacksHurtType hurtType) {
		yield return new WaitForSeconds(hitStunTime);	
		
		if (hurtType == GeneralEnums.AttacksHurtType.High) {
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtHighRecover);
		}	
		else if (hurtType == GeneralEnums.AttacksHurtType.Mid) {
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtMidRecover);
		}	
		else if (hurtType == GeneralEnums.AttacksHurtType.Launch) {		
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtLaunch);
			HurtLaunch_MovePosition();
		}	
		else if (hurtType == GeneralEnums.AttacksHurtType.LaunchBack) {		
			HurtLaunchBack_MovePosition();
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
}
