using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectsEnums : MonoBehaviour {

	public enum HitSparkType { Small = 0, Mid = 1, Big = 2, RyuHadouken = 3, RyuShakunetsu = 4, GuardCrush = 5 }
	public enum RyuSpecialEffectsType { Hadouken = 0 }
	public enum TerrySpecialEffectsType { PowerDunk = 0 }
	public enum BattleEffectsUser { Ryu = 0 }	
	public enum MovementEffectsType { RunDust = 0 }

	public static class RyuSpecialEffectsTriggerNames {
        public static string Hadouken { get { return "special_Hadouken"; } }
		public static string Shakunetsu { get { return "special_Shakunetsu"; } }
    }

	public static class TerrySpecialEffectsTriggerNames {
        public static string SpecialPowerDunk { get { return "special_PowerDunk"; } }
		public static string PowerDunkLoop { get { return "powerDunk_Loop"; } }
    }

	public static float GetHitSparkDestoryTime(SpecialEffectsEnums.HitSparkType hitSparkType) {		
		if (hitSparkType == HitSparkType.Small) {
			return 0.25f;
		}
		else if (hitSparkType == HitSparkType.Mid) {
			return 0.21f;
		}
		else if (hitSparkType == HitSparkType.Big) {
			return 0.27f;
		}
		else if (hitSparkType == HitSparkType.RyuHadouken) {
			return 0.25f;
		}
		else if (hitSparkType == HitSparkType.RyuShakunetsu) {
			return 0.15f;
		}

		return 1.0f;
	}

	public static float GetSpecialEffectsDestoryTime(RyuSpecialEffectsType specialEffectType)	{		
		if (specialEffectType == RyuSpecialEffectsType.Hadouken) {
			return 0.85f;
		}

		return 1.0f;
	}

	public static float GetEnemySpecialEffectsDestoryTime(int specialEffectType)	{
		if (specialEffectType == (int)TerrySpecialEffectsType.PowerDunk) {
			return 0.42f;
		}

		return 1.0f;
	}
}
