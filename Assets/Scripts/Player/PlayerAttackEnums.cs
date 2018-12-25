using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEnums : MonoBehaviour {

	public enum RyuAttacks { 
		// normals
		Jab = 0, Short = 1, Strong = 2, Forward = 3, 
		// special moves
		Shoryuken = 4, Hadouken = 5, 
	}

	public static class AttackTriggerNames {
		// universal default attacks
        public static string AttackJab { get { return "attack_Jab"; } }
		public static string AttackStrong { get { return "attack_Strong"; } }
		public static string AttackShort { get { return "attack_Short"; } }
		public static string AttackForward { get { return "attack_Forward"; } }

		public static string SpecialShoryuken { get { return "special_Shoryuken"; } }
        public static string SpecialHadouken { get { return "special_Hadouken"; } }

		// aurea attacks
		public static string AttackHighKick { get { return "attack_HighKick"; } }
        public static string AttackFierce { get { return "attack_Fierce"; } }
        public static string AttackImpact { get { return "attack_Impact"; } }
    }

	public enum ComboRouteTypes { 
		// A, A, A
		AAA = 0
	}

	public static float RyuAttacksKnockbackDistance (int attackType) {
		// normals
		if (attackType == (int)RyuAttacks.Jab) return 0.3f;
		else if (attackType == (int)RyuAttacks.Short) return 0.3f;
		else if (attackType == (int)RyuAttacks.Strong) return 0.6f;
		else if (attackType == (int)RyuAttacks.Forward) return 0.5f;		
		// special moves
		else if (attackType == (int)RyuAttacks.Shoryuken) return 2.5f;

		return 0f;
	} 

	// 0 = hurt high
	// 1 = hurt mid
	// 2 = launcher
	public enum RyuAttacksHurtType { 
		// normals
		Jab = 0, Short = 1, Strong = 0, Forward = 0, 
		// special moves
		Shoryuken = 2 
	}

	public enum RyuAttacksScreenShake { 
		// normals
		Jab = 0, Short = 0, Strong = 1, Forward = 0, 
		// special moves
		Shoryuken = 2 
	}

	public enum AttacksHurtType { 
		High = 0, Mid = 1, Launch = 2 
	}

}
