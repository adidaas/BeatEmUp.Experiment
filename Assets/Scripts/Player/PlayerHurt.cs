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

    public bool isInInvincibleState = false;

    // Start is called before the first frame update
    void Start() {
        playerController = gameObject.GetComponent<PlayerController>();
        playerAudio = gameObject.GetComponent<PlayerAudio>();
        myAnim = gameObject.GetComponent<Animator>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void IsHit(int hurtType, float knockBackDistance, bool knockBackLeft, float hitStop) {
        float verticalKnockBack = myRigidbody.velocity.y;
		float direction = 1.0f;
		float hitshakeDuration = 0.2f;

        playerAudio.PlayHurtSound(playerController.playerCharacter, hurtType);

        if (hurtType == (int)GeneralEnums.AttacksHurtType.High) {
            myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtHigh);			
            hitStunCoroutine = PerformHitStun(0.4f + hitStop, GeneralEnums.AttacksHurtType.High);				
        }
        else if (hurtType == (int)GeneralEnums.AttacksHurtType.Mid) {
            myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtMid);
            hitStunCoroutine = PerformHitStun(0.4f + hitStop, GeneralEnums.AttacksHurtType.Mid);
        }
        else if (hurtType == (int)GeneralEnums.AttacksHurtType.Launch) {
            myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtMid);
            hitStunCoroutine = PerformHitStun(hitStop, GeneralEnums.AttacksHurtType.Launch);
        }
        StartCoroutine(hitStunCoroutine);		
        StartCoroutine(PerformHitShake(hitshakeDuration));	
    }
    public void PlaySoundEffect(int hurtType) {

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
		
		if (hurtType == GeneralEnums.AttacksHurtType.High) {
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtHighRecover);
			playerController.canMove = true;
		}	
		else if (hurtType == GeneralEnums.AttacksHurtType.Mid) {
			myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtMidRecover);
			playerController.canMove = true;
		}	
		// else if (hurtType == GeneralEnums.AttacksHurtType.Launch) {		
		// 	myAnim.SetTrigger(GeneralEnums.HurtTriggers.HurtLaunch);
		// 	HurtLaunch_MovePosition();
		// }	
		// else if (hurtType == GeneralEnums.AttacksHurtType.LaunchBack) {		
		// 	HurtLaunchBack_MovePosition();
		// }	
	}
}
