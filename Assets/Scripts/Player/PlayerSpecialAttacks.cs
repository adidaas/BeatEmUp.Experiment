using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttacks : MonoBehaviour {
	private PlayerController playerController;
	public GameObject ryuFireball;
	private Vector3 spawnPosition;
	private bool isFacingRight = true;
	private Vector3 targetPosition;
	private float targetXPosition;
	private Rigidbody2D myRigidbody;
	private bool isEXAttack = false;
	private int currentStep = 0;

	void Start() {
		playerController = gameObject.GetComponent<PlayerController>();
		myRigidbody = gameObject.GetComponent<Rigidbody2D>();
	}

    public void InitializeRyuSpecialAttack(PlayerAttackEnums.RyuAttacks attackType, bool isFacingRightCheck, bool isEXKeyActive = false) {
		currentStep = 0;
		isFacingRight = isFacingRightCheck;
		float animationDelay = 0f;
		isEXAttack = isEXKeyActive ? true : false;
        if (attackType == PlayerAttackEnums.RyuAttacks.Hadouken) {
            // hadouken fireball
			spawnPosition = isFacingRight ? new Vector3(this.transform.position.x + 1.9f, this.transform.position.y + 1.3f, this.transform.position.z)
					: new Vector3(this.transform.position.x - 1.9f, this.transform.position.y + 1.3f, this.transform.position.z);

			// wait for frame 4 of Ryu's Hadouken animation before instantiating fireball
			animationDelay = isEXKeyActive ? 0.035f : 0.045f;
        }
		else if (attackType == PlayerAttackEnums.RyuAttacks.Tatsumaki) {
            // tatsumaki
			targetXPosition = isFacingRight ? 26.5f : -26.5f;
			// wait for frame 5 of Ryu's Tatsumaki animation before moving parent object
			animationDelay = 0.045f;			
        }
		else if (attackType == PlayerAttackEnums.RyuAttacks.SolarPlexus) {
            // solar plexus
			targetXPosition = isFacingRight ? 15f : -15f;

			// begin immeditely
			animationDelay = 0.001f;			
        }
		else if (attackType == PlayerAttackEnums.RyuAttacks.RunningKick) {
            // RunningKick
			targetXPosition = isFacingRight ? 25f : -25f;

			// begin  immeditely
			animationDelay = 0.001f;			
        }
		else if (attackType == PlayerAttackEnums.RyuAttacks.HardKnee) {
            // HardKnee
			var distance = isEXKeyActive ? 10f : 5f;
			targetXPosition = isFacingRight ? distance : distance * -1;

			// begin  immeditely
			animationDelay = isEXKeyActive ? 0.04f : 0.15f;			
        }

		StartCoroutine(WaitToPlay(animationDelay, attackType));
    }

	private IEnumerator WaitToPlay(float duration, PlayerAttackEnums.RyuAttacks attackType) {		
		yield return new WaitForSeconds(duration);	

		PlaySpecialAttackAnimation(attackType);
	}

	private void PlaySpecialAttackAnimation(PlayerAttackEnums.RyuAttacks attackType) {
		if (attackType == PlayerAttackEnums.RyuAttacks.Hadouken) {
            // hadouken fireball
			GameObject ryuFireballInstance = Instantiate(ryuFireball, spawnPosition, this.transform.rotation);			
			ryuFireballInstance.layer = 12;
			
			EffectsController effectsController = ryuFireballInstance.GetComponent<EffectsController>();
			effectsController.isFacingRight = isFacingRight;
			effectsController.playerUser = GeneralEnums.PlayerCharacters.Ryu;
			effectsController.PlaySpecialEffects((int)SpecialEffectsEnums.RyuSpecialEffectsType.Hadouken, isEXAttack);

			var timeToLive = SpecialEffectsEnums.GetSpecialEffectsDestoryTime(SpecialEffectsEnums.RyuSpecialEffectsType.Hadouken);
			
			Destroy (ryuFireballInstance, timeToLive);			
        }
		else if (attackType == PlayerAttackEnums.RyuAttacks.Tatsumaki) {
			myRigidbody.velocity = new Vector2(targetXPosition, myRigidbody.velocity.y);
			//StartCoroutine(MoveOverSpeed(gameObject, targetPosition, 8f));
		}
		else if (attackType == PlayerAttackEnums.RyuAttacks.SolarPlexus) {
			myRigidbody.velocity = new Vector2(targetXPosition, myRigidbody.velocity.y);
			
			//StartCoroutine(MoveOverSpeed(gameObject, targetPosition, 8f));
		}
		else if (attackType == PlayerAttackEnums.RyuAttacks.RunningKick) {
			myRigidbody.velocity = new Vector2(targetXPosition, myRigidbody.velocity.y);
			
			//StartCoroutine(MoveOverSpeed(gameObject, targetPosition, 8f));
		}
		else if (attackType == PlayerAttackEnums.RyuAttacks.HardKnee) {
			if (!isEXAttack) {
				myRigidbody.velocity = new Vector2(targetXPosition, myRigidbody.velocity.y);
			}
			else {
				float direction = isFacingRight ? 1 : -1;
				targetPosition = new Vector2((11f) * direction, 0);
				//myRigidbody.bodyType = RigidbodyType2D.Kinematic;
				playerController.myBoxCollider.enabled = false;
				myRigidbody.AddForce(targetPosition, ForceMode2D.Impulse);
				StartCoroutine(WaitToChangeBodyType());
				//myRigidbody.velocity = new Vector2(targetXPosition, myRigidbody.velocity.y);
				
				targetPosition = new Vector2(gameObject.transform.position.x + (4f * direction), gameObject.transform.position.y);
				//StartCoroutine(MoveOverSpeed(gameObject, targetPosition, 38f));
			}
			
		}
	}

	public IEnumerator MoveOverSpeed (GameObject objectToMove, Vector3 end, float speed) {		
		while (objectToMove.transform.position != end) {
			objectToMove.transform.position = 
					Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
			myRigidbody.bodyType = RigidbodyType2D.Dynamic;
			playerController.myBoxCollider.enabled = true;
		}
	}

	public IEnumerator WaitToChangeBodyType() {
		yield return new WaitForSeconds(0.2f);
		myRigidbody.bodyType = RigidbodyType2D.Dynamic;
		playerController.myBoxCollider.enabled = true;
	}

}
