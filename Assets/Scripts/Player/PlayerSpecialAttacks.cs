using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttacks : MonoBehaviour {
	public GameObject ryuFireball;
	private Vector3 spawnPosition;
	private bool isFacingRight = true;
	private Vector3 targetPosition;

    public void InitializeRyuSpecialAttack(int attackType, bool isFacingRightCheck) {
		isFacingRight = isFacingRightCheck;
		float animationDelay = 0f;
        if (attackType == (int)PlayerAttackEnums.RyuAttacks.Hadouken) {
            // hadouken fireball
			spawnPosition = isFacingRight ? new Vector3(this.transform.position.x + 1.9f, this.transform.position.y + 1.3f, this.transform.position.z)
					: new Vector3(this.transform.position.x - 1.9f, this.transform.position.y + 1.3f, this.transform.position.z);

			// wait for frame 4 of Ryu's Hadouken animation before instantiating fireball
			animationDelay = 0.045f;			
        }
		else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Tatsumaki) {
            // tatsumaki
			targetPosition = isFacingRight ? new Vector3(this.transform.position.x + 9f, this.transform.position.y, this.transform.position.z)
					: new Vector3(this.transform.position.x - 9f, this.transform.position.y, this.transform.position.z);

			// wait for frame 5 of Ryu's Tatsumaki animation before moving parent object
			animationDelay = 0.045f;			
        }

		StartCoroutine(WaitToPlay(animationDelay, attackType));
    }

	private IEnumerator WaitToPlay(float duration, int attackType) {		
		yield return new WaitForSeconds(duration);	

		PlaySpecialAttackAnimation(attackType);
	}

	private void PlaySpecialAttackAnimation(int attackType) {
		if (attackType == (int)PlayerAttackEnums.RyuAttacks.Hadouken) {
            // hadouken fireball
			GameObject ryuFireballInstance = Instantiate(ryuFireball, spawnPosition, this.transform.rotation);			
			ryuFireballInstance.layer = 12;
			
			SpecialEffectsController specialEffectsController = ryuFireballInstance.GetComponent<SpecialEffectsController>();
			specialEffectsController.isFacingRight = isFacingRight;
			specialEffectsController.attackType = attackType;
			specialEffectsController.PlaySpecialEffects((int)SpecialEffectsEnums.RyuSpecialEffectsType.Hadouken);

			var timeToLive = SpecialEffectsEnums.GetSpecialEffectsDestoryTime((int)SpecialEffectsEnums.RyuSpecialEffectsType.Hadouken);
			
			Destroy (ryuFireballInstance, timeToLive);			
        }
		else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Tatsumaki) {
			StartCoroutine(MoveOverSpeed(gameObject, targetPosition, 8f));
		}
	}

	public IEnumerator MoveOverSpeed (GameObject objectToMove, Vector3 end, float speed) {		
		while (objectToMove.transform.position != end) {
			objectToMove.transform.position = 
					Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
	}

}
