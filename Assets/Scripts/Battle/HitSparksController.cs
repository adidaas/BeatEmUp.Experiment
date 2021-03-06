using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSparksController : MonoBehaviour {
	public Animator myAnim;

	public void PlayHitSpark(SpecialEffectsEnums.HitSparkType sparkType, bool isFacingRight)	{
		if(myAnim.isActiveAndEnabled) {
			if (!isFacingRight) {
				transform.localScale = new Vector2(-1f, 1f);
			}
			if(sparkType == SpecialEffectsEnums.HitSparkType.Small) {
				transform.localScale = new Vector2(0.5f, 0.5f);
				myAnim.SetTrigger("hitspark_Small");
			}
			else if(sparkType == SpecialEffectsEnums.HitSparkType.Mid) {
				transform.localScale = new Vector2(0.7f, 0.7f);
				myAnim.SetTrigger("hitspark_Mid");
			}
			else if(sparkType == SpecialEffectsEnums.HitSparkType.Big) {
				myAnim.SetTrigger("hitspark_Big");
			}
			else if(sparkType == SpecialEffectsEnums.HitSparkType.RyuHadouken) {
				myAnim.SetTrigger("hitspark_RyuHadouken");
			}
			else if(sparkType == SpecialEffectsEnums.HitSparkType.RyuShakunetsu) {
				myAnim.SetTrigger("hitspark_RyuShakunetsu");
			}
		}
	}

	public void PlayBlockSpark(SpecialEffectsEnums.HitSparkType sparkType, bool isFacingRight)	{
		if(myAnim.isActiveAndEnabled) {
			if (isFacingRight) {
				transform.localScale = new Vector2(-1f, 1f);
			}
			if(sparkType == SpecialEffectsEnums.HitSparkType.Small) {
				myAnim.SetTrigger("block_Small");
			}
			else if(sparkType == SpecialEffectsEnums.HitSparkType.Mid) {
				myAnim.SetTrigger("block_Mid");
			}
			else if(sparkType == SpecialEffectsEnums.HitSparkType.Big) {
				myAnim.SetTrigger("block_Big");
			}
			else if(sparkType == SpecialEffectsEnums.HitSparkType.GuardCrush) {
				transform.localScale = new Vector2(0.85f, 0.85f);
				myAnim.SetTrigger("guardCrush");
			}
		}
	}
}
