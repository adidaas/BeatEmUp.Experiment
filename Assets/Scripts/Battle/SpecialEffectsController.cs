using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectsController : MonoBehaviour {
	public Animator myAnim;
	public GameObject hitBox;
	public HitCollider hitCollider;
	public bool isFacingRight = true;
	public int attackType;

	void Start() {
		hitCollider = hitBox.GetComponent<HitCollider>();
	}

	public void PlaySpecialEffects(int specialAttackType) {
		hitCollider = hitBox.GetComponent<HitCollider>();
		BattleHelper.SetHitCollider(ref hitCollider, PlayerAttackEnums.RyuAttacks.Hadouken);	
		if(myAnim.isActiveAndEnabled) {
			if(specialAttackType == ((int)SpecialEffectsEnums.RyuSpecialEffectsType.Hadouken)) {
				myAnim.SetTrigger(SpecialEffectsEnums.RyuSpecialEffectsTriggerNames.Hadouken);
				Vector3 targetPosition = new Vector3();
				if (isFacingRight) {
					transform.localScale = new Vector3(1f, 1f, 1f);
					targetPosition = new Vector3(transform.position.x + 25, transform.position.y, transform.position.z);
				}
				else {
					transform.localScale = new Vector3(-1f, 1f, 1f);
					targetPosition =  new Vector3(transform.position.x - 25, transform.position.y, transform.position.z);
				}

				StartCoroutine(MoveSpecialEffectPosition(targetPosition, 25f));
			}
		}
	}

	public IEnumerator MoveSpecialEffectPosition(Vector3 targetPosition, float speed) {
		while (transform.position != targetPosition) {
			transform.position = 
					Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
						
			yield return new WaitForEndOfFrame ();
		}
	}

}
