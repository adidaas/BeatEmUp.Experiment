using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectsController : MonoBehaviour {
	public Animator myAnim;
	
	public void PlaySpecialEffects(int sparkType) {
		if(myAnim.isActiveAndEnabled)
		{
			if(sparkType == ((int)SpecialEffectsEnums.RyuSpecialEffectsType.Hadouken))
			{
				myAnim.SetTrigger(SpecialEffectsEnums.RyuSpecialEffectsTriggerNames.Hadouken);
			}
		}
	}
}
