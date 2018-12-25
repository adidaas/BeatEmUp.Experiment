using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectsEnums : MonoBehaviour {

	//public enum HitSparkTriggers { Small = "hitspark_Small", Mid = "hitspark_Mid", Big = "hitspark_Big" }
	public enum HitSparkType { Small = 0, Mid = 1, Big = 2 }
	public enum RyuSpecialEffectsType { Hadouken = 0 }

	public static class RyuSpecialEffectsTriggerNames
    {
        public static string Hadouken { get { return "special_Hadouken"; } }
		public static string HadoukenHit { get { return "hadouken_Hit"; } }
    }

	public static float GetHitSparkDestoryTime(int hitSparkType)
	{		
		if (hitSparkType == (int)HitSparkType.Small) 
		{
			return 0.45f;
		}
		else if (hitSparkType == (int)HitSparkType.Mid) 
		{
			return 0.3f;
		}
		else if (hitSparkType == (int)HitSparkType.Big) 
		{
			return 0.4f;
		}

		return 0.0f;
	}
}
