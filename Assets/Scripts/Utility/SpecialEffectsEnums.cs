using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectsEnums : MonoBehaviour {

	public enum HitSparkType { Small = 0, Mid = 1, Big = 2, RyuHadouken = 3 }
	public enum RyuSpecialEffectsType { Hadouken = 0 }

	public static class RyuSpecialEffectsTriggerNames {
        public static string Hadouken { get { return "special_Hadouken"; } }
    }

	public static float GetHitSparkDestoryTime(int hitSparkType) {		
		if (hitSparkType == (int)HitSparkType.Small) {
			return 0.25f;
		}
		else if (hitSparkType == (int)HitSparkType.Mid) {
			return 0.21f;
		}
		else if (hitSparkType == (int)HitSparkType.Big) {
			return 0.27f;
		}
		else if (hitSparkType == (int)HitSparkType.RyuHadouken) {
			return 0.25f;
		}

		return 0.0f;
	}

	public static float GetSpecialEffectsDestoryTime(int specialEffectType)	{		
		if (specialEffectType == (int)RyuSpecialEffectsType.Hadouken) {
			return 0.85f;
		}

		return 0.0f;
	}
}
