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

		public static string JumpShort { get { return "jump_Short"; } }		

		public static string SpecialShoryuken { get { return "special_Shoryuken"; } }
        public static string SpecialHadouken { get { return "special_Hadouken"; } }
		public static string SpecialTatsumaki { get { return "special_Tatsumaki"; } }
		public static string SpecialSolarPlexus { get { return "special_SolarPlexus"; } }
		public static string SpecialRunningKick { get { return "special_RunningKick"; } }
		public static string SpecialHardKnee { get { return "special_HardKnee"; } }

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
		// jump short
		JumpShort = 10,
		// special moves
		Shoryuken = 4, Hadouken = 5, Tatsumaki = 6, SolarPlexus = 7, RunningKick = 8, HardKnee = 9
	}

	public static float RyuAttacksKnockbackDistance (int attackType) {
		// normals
		if (attackType == (float)RyuAttacks.Jab) return 0.3f;
		else if (attackType == (float)RyuAttacks.Short) return 0.3f;
		else if (attackType == (float)RyuAttacks.Strong) return 0.6f;
		else if (attackType == (float)RyuAttacks.Forward) return 0.5f;					
		// jump attack
		else if (attackType == (float)RyuAttacks.JumpShort) return 0.5f;					
		// special moves
		else if (attackType == (float)RyuAttacks.Hadouken) return 0.3f;
		else if (attackType == (float)RyuAttacks.Shoryuken) return 2.5f;
		else if (attackType == (float)RyuAttacks.Tatsumaki) return 14.5f;
		else if (attackType == (float)RyuAttacks.SolarPlexus) return 5.5f;
		else if (attackType == (float)RyuAttacks.RunningKick) return 8.5f;	
		else if (attackType == (float)RyuAttacks.HardKnee) return 1.5f;	

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
	// 3 = air
	// 4 = launch back
	public enum RyuAttacksHurtType { 
		// normals
		Jab = 0, Short = 1, Strong = 0, Forward = 0,
		// jump short
		JumpShort = 0,
		// special moves
		Shoryuken = 2, Hadouken = 0, Tatsumaki = 0, SolarPlexus = 1, RunningKick = 0, HardKnee = 1, EXHardKnee = 4,
	}

	// 0 = no shake
	// 1 = light shake
	// 2 = strong shake
	public enum RyuAttacksScreenShake { 
		// normals
		Jab = 0, Short = 0, Strong = 1, Forward = 0,
		// jump attacks
		JumpShort = 0,
		// special moves
		Shoryuken = 2, Hadouken = 0, Tatsumaki = 0, SolarPlexus = 1,  RunningKick = 1, HardKnee = 2,
	}

	public static float RyuAttacksHitStop (RyuAttacks attackType, bool isEXAttack = false) { 
		if (!isEXAttack) {
			if (attackType == RyuAttacks.Jab) return 0f;
			else if (attackType == RyuAttacks.Short) return 0f;
			else if (attackType == RyuAttacks.Strong) return 0.1f;
			else if (attackType == RyuAttacks.Forward) return 0f;	
			// jump attacks	
			else if (attackType == RyuAttacks.JumpShort) return 0f;	
			// special moves
			else if (attackType == RyuAttacks.Hadouken) return 0f;
			else if (attackType == RyuAttacks.Shoryuken) return 0.30f;
			else if (attackType == RyuAttacks.Tatsumaki) return 0f;
			else if (attackType == RyuAttacks.SolarPlexus) return 0.2f;
			else if (attackType == RyuAttacks.RunningKick) return 0f;	
			else if (attackType == RyuAttacks.HardKnee) return 0.35f;	
		}
		else if (isEXAttack) {
			if (attackType == RyuAttacks.HardKnee) return 0.08f;	
		}
		

		return 1f;
	}
	#endregion

}
