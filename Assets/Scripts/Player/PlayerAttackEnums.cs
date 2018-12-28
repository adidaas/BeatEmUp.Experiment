using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEnums : MonoBehaviour {
	
	public static class AttackTriggerNames {
		// universal default attacks
        public static string AttackJab { get { return "attack_Jab"; } }
		public static string AttackStrong { get { return "attack_Strong"; } }
		public static string AttackShort { get { return "attack_Short"; } }
		public static string AttackForward { get { return "attack_Forward"; } }

		public static string SpecialShoryuken { get { return "special_Shoryuken"; } }
        public static string SpecialHadouken { get { return "special_Hadouken"; } }
		public static string SpecialTatsumaki { get { return "special_Tatsumaki"; } }

		// aurea attacks
		public static string AttackHighKick { get { return "attack_HighKick"; } }
        public static string AttackFierce { get { return "attack_Fierce"; } }
        public static string AttackImpact { get { return "attack_Impact"; } }
    }

	public enum ComboRouteTypes { 
		// A, A, A
		AAA = 0
	}

	#region Ryu Attacks
	public enum RyuAttacks { 
		// normals
		Jab = 0, Short = 1, Strong = 2, Forward = 3, 
		// special moves
		Shoryuken = 4, Hadouken = 5, Tatsumaki = 6
	}

	public static float RyuAttacksKnockbackDistance (int attackType) {
		// normals
		if (attackType == (float)RyuAttacks.Jab) return 0.3f;
		else if (attackType == (float)RyuAttacks.Short) return 0.3f;
		else if (attackType == (float)RyuAttacks.Strong) return 0.6f;
		else if (attackType == (float)RyuAttacks.Forward) return 0.5f;		
		// special moves
		else if (attackType == (float)RyuAttacks.Hadouken) return 0.3f;
		else if (attackType == (float)RyuAttacks.Shoryuken) return 2.5f;
		else if (attackType == (float)RyuAttacks.Tatsumaki) return 5.5f;

		return 0f;
	} 

	public static float RyuAttacksKnockbackSpeed (int attackType) {
		// normals
		if (attackType == (int)RyuAttacks.Jab) return 10f;
		else if (attackType == (int)RyuAttacks.Short) return 10f;
		else if (attackType == (int)RyuAttacks.Strong) return 10f;
		else if (attackType == (int)RyuAttacks.Forward) return 10f;		
		// special moves
		else if (attackType == (int)RyuAttacks.Hadouken) return 10f;
		else if (attackType == (int)RyuAttacks.Shoryuken) return 10f;
		else if (attackType == (int)RyuAttacks.Tatsumaki) return 15f;

		return 0f;
	} 

	// 0 = hurt high
	// 1 = hurt mid
	// 2 = launcher
	public enum RyuAttacksHurtType { 
		// normals
		Jab = 0, Short = 1, Strong = 0, Forward = 0, 
		// special moves
		Shoryuken = 2, Hadouken = 0, Tatsumaki = 0
	}

	// 0 = no shake
	// 1 = light shake
	// 2 = strong shake
	public enum RyuAttacksScreenShake { 
		// normals
		Jab = 0, Short = 0, Strong = 1, Forward = 0, 
		// special moves
		Shoryuken = 2, Hadouken = 0, Tatsumaki = 0
	}
	#endregion

}
