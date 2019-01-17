using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSparksController : MonoBehaviour {
	public Animator myAnim;

	public void PlayHitSpark(SpecialEffectsEnums.HitSparkType sparkType)	{
		if(myAnim.isActiveAndEnabled) {
			if(sparkType == SpecialEffectsEnums.HitSparkType.Small) {
				transform.localScale = new Vector2(0.6f, 0.6f);
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
}
