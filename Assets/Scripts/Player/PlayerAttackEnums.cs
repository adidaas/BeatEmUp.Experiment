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
		public static string AttackFierce { get { return "attack_Fierce"; } }		
		public static string AttackHighRoundhouse { get { return "attack_HighRoundhouse"; } }
		public static string AttackOverheadIdle { get { return "attack_OverheadIdle"; } }
		public static string AttackOverhead { get { return "attack_Overhead"; } }
		public static string AttackCrouchForward { get { return "attack_CrouchForward"; } }
		public static string AttackCrouchFierce { get { return "attack_CrouchFierce"; } }
		public static string AttackCloseForward { get { return "attack_CloseForward"; } }
		public static string JumpShort { get { return "jump_Short"; } }		

		public static string SpecialShoryuken { get { return "special_Shoryuken"; } }
        public static string SpecialHadouken { get { return "special_Hadouken"; } }
		public static string SpecialTatsumaki { get { return "special_Tatsumaki"; } }
		public static string SpecialSolarPlexus { get { return "special_SolarPlexus"; } }
		public static string SpecialRunningKick { get { return "special_RunningKick"; } }
		public static string SpecialHardKnee { get { return "special_HardKnee"; } }

		// aurea attacks
		public static string AttackHighKick { get { return "attack_HighKick"; } }
        //public static string AttackFierce { get { return "attack_Fierce"; } }
        public static string AttackImpact { get { return "attack_Impact"; } }
    }

	public enum ComboRouteTypes { 
		// A, A, A
		None = -1, AAA = 0, AA_AA = 1, AAHoldAAA = 2
	}

	#region Ryu Attacks
	public enum RyuAttacks { 
		// normals
		Jab = 0, Short = 1, Strong = 2, Forward = 3, Fierce = 11, HighRoundhouse = 12, Overhead = 13, CrouchForward = 14
		, CrouchFierce = 15, CloseForward = 16
		// jump short
		, JumpShort = 10
		// special moves
		, Shoryuken = 4, Hadouken = 5, Tatsumaki = 6, SolarPlexus = 7, RunningKick = 8, HardKnee = 9
	}

	public static float RyuAttacksKnockbackDistance (int attackType) {
		// normals
		if (attackType == (int)RyuAttacks.Jab) return 0.3f;
		else if (attackType == (int)RyuAttacks.Short) return 0.3f;
		else if (attackType == (int)RyuAttacks.Strong) return 9.6f;
		else if (attackType == (int)RyuAttacks.Fierce) return 0.6f;
		else if (attackType == (int)RyuAttacks.HighRoundhouse) return 14.5f;
		else if (attackType == (int)RyuAttacks.Overhead) return 0.3f;
		else if (attackType == (int)RyuAttacks.CrouchForward) return 0.3f;
		else if (attackType == (int)RyuAttacks.CrouchFierce) return 0.6f;
		else if (attackType == (int)RyuAttacks.CloseForward) return 18.5f;
		// jump attack
		else if (attackType == (int)RyuAttacks.JumpShort) return 0.5f;
		// special moves
		else if (attackType == (int)RyuAttacks.Hadouken) return 0.3f;
		else if (attackType == (int)RyuAttacks.Shoryuken) return 2.5f;
		else if (attackType == (int)RyuAttacks.Tatsumaki) return 14.5f;
		else if (attackType == (int)RyuAttacks.SolarPlexus) return 5.5f;
		else if (attackType == (int)RyuAttacks.RunningKick) return 8.5f;	
		else if (attackType == (int)RyuAttacks.HardKnee) return 1.5f;	

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
		Jab = 0, Short = 1, Strong = 0, Forward = 0, Fierce = 0, HighRoundhouse = 0, Overhead = 0, CrouchForward = 1
		, CrouchFierce = 1, CloseForward = 0
		// jump short
		, JumpShort = 0
		// special moves
		, Shoryuken = 2, Hadouken = 0, Tatsumaki = 0, SolarPlexus = 1, RunningKick = 0, HardKnee = 1, EXHardKnee = 4,
	}

	// 0 = no shake
	// 1 = light shake
	// 2 = strong shake
	public enum RyuAttacksScreenShake { 
		// normals
		Jab = 0, Short = 0, Strong = 1, Forward = 0, Fierce = 0, HighRoundhouse = 1, Overhead = 1, CrouchForward = 0
		, CrouchFierce = 0, CloseForward = 1
		// jump attacks
		, JumpShort = 0
		// special moves
		, Shoryuken = 2, Hadouken = 0, Tatsumaki = 0, SolarPlexus = 1,  RunningKick = 1, HardKnee = 2,
	}

	public static float RyuAttacksHitStop (int attackType, bool isEXAttack = false) { 
		if (!isEXAttack) {
			if (attackType == (int)RyuAttacks.Jab) return 0f;
			else if (attackType == (int)RyuAttacks.Short) return 0f;
			else if (attackType == (int)RyuAttacks.Strong) return 0.1f;
			else if (attackType == (int)RyuAttacks.Forward) return 0f;	
			else if (attackType == (int)RyuAttacks.Fierce) return 0.02f;
			else if (attackType == (int)RyuAttacks.HighRoundhouse) return 0.05f;
			else if (attackType == (int)RyuAttacks.Overhead) return 0.02f;
			else if (attackType == (int)RyuAttacks.CrouchForward) return 0.01f;	
			else if (attackType == (int)RyuAttacks.CrouchFierce) return 0.02f;
			else if (attackType == (int)RyuAttacks.CloseForward) return 0.05f;
			// jump attacks	
			else if (attackType == (int)RyuAttacks.JumpShort) return 0f;	
			// special moves
			else if (attackType == (int)RyuAttacks.Hadouken) return 0f;
			else if (attackType == (int)RyuAttacks.Shoryuken) return 0.30f;
			else if (attackType == (int)RyuAttacks.Tatsumaki) return 0f;
			else if (attackType == (int)RyuAttacks.SolarPlexus) return 0.2f;
			else if (attackType == (int)RyuAttacks.RunningKick) return 0f;	
			else if (attackType == (int)RyuAttacks.HardKnee) return 0.35f;	
		}
		else if (isEXAttack) {
			if (attackType == (int)RyuAttacks.HardKnee) return 0.08f;	
		}
		

		return 1f;
	}

	public static int RyuAttacksStartUpFrames (int attackType, bool isEXAttack = false) {
		Dictionary<int, int> ryuStartUpFrames = new Dictionary<int, int> {
			{ (int)RyuAttacks.Jab, 4},
			{ (int)RyuAttacks.Short, 4},
			{ (int)RyuAttacks.Strong, 7},
			{ (int)RyuAttacks.Fierce, 6},
			{ (int)RyuAttacks.HighRoundhouse, 9},
			{ (int)RyuAttacks.Overhead, 7},
			{ (int)RyuAttacks.CrouchForward, 5},
			{ (int)RyuAttacks.CrouchFierce, 9},
			{ (int)RyuAttacks.CloseForward, 17},

			{ (int)RyuAttacks.JumpShort, 7},

			{ (int)RyuAttacks.SolarPlexus, 6},
			{ (int)RyuAttacks.RunningKick, 11},

			{ (int)RyuAttacks.Shoryuken, 9},
			{ (int)RyuAttacks.Tatsumaki, 10},
			{ (int)RyuAttacks.HardKnee, 11},
			
		};

		Dictionary<int, int> ryuExStartUpFrames = new Dictionary<int, int> {
			{ (int)RyuAttacks.HardKnee, 3},		
		};

		int startUpFrames;

		if (isEXAttack) {
			startUpFrames = ryuExStartUpFrames[attackType];
		}
		else {
			startUpFrames = ryuStartUpFrames[attackType];
		}

		return startUpFrames;
	}

	public static int RyuAttacksActiveFrames (int attackType, bool isEXAttack = false) {
		Dictionary<int, int> ryuActiveFrames = new Dictionary<int, int> {
			{ (int)RyuAttacks.Jab, 6},
			{ (int)RyuAttacks.Short, 6},
			{ (int)RyuAttacks.Strong, 8},
			{ (int)RyuAttacks.Fierce, 7},
			{ (int)RyuAttacks.HighRoundhouse, 10},
			{ (int)RyuAttacks.Overhead, 6},
			{ (int)RyuAttacks.CrouchForward, 5},			
			{ (int)RyuAttacks.CrouchFierce, 9},
			{ (int)RyuAttacks.CloseForward, 18},

			{ (int)RyuAttacks.JumpShort, 8},

			{ (int)RyuAttacks.SolarPlexus, 8},
			{ (int)RyuAttacks.RunningKick, 12},

			{ (int)RyuAttacks.Shoryuken, 9},
			{ (int)RyuAttacks.Tatsumaki, 11},
			{ (int)RyuAttacks.HardKnee, 23},
		};

		Dictionary<int, int> ryuExActiveFrames = new Dictionary<int, int> {
			{ (int)RyuAttacks.HardKnee, 4},		
		};

		int activeFrames;

		if (isEXAttack) {
			activeFrames = ryuExActiveFrames[attackType];
		}
		else {
			activeFrames = ryuActiveFrames[attackType];
		}

		return activeFrames;
	}

	public static int RyuAttacksRecoveryFrames (int attackType, bool isEXAttack = false) {
		Dictionary<int, int> ryuRecoveryFrames = new Dictionary<int, int> {
			{ (int)RyuAttacks.Jab, 9},
			{ (int)RyuAttacks.Short, 11},
			{ (int)RyuAttacks.Strong, 16},
			{ (int)RyuAttacks.Fierce, 11},
			{ (int)RyuAttacks.HighRoundhouse, 17},
			{ (int)RyuAttacks.Overhead, 15},
			{ (int)RyuAttacks.CrouchForward, 13},			
			{ (int)RyuAttacks.CrouchFierce, 15},
			{ (int)RyuAttacks.CloseForward, 24},

			{ (int)RyuAttacks.JumpShort, 16},

			{ (int)RyuAttacks.SolarPlexus, 16},
			{ (int)RyuAttacks.RunningKick, 21},

			{ (int)RyuAttacks.Shoryuken, 22},
			{ (int)RyuAttacks.Tatsumaki, 36},
			{ (int)RyuAttacks.HardKnee, 29},
		};

		Dictionary<int, int> ryuExRecoveryFrames = new Dictionary<int, int> {
			{ (int)RyuAttacks.HardKnee, 9},
		};

		int recoveryFrames;

		if (isEXAttack) {
			recoveryFrames = ryuExRecoveryFrames[attackType];
		}
		else {
			recoveryFrames = ryuRecoveryFrames[attackType];
		}

		return recoveryFrames;
	}

	public static int RyuAttacksEndFrames (int attackType, bool isEXAttack = false) {
		Dictionary<int, int> ryuEndFrames = new Dictionary<int, int> {
			{ (int)RyuAttacks.Jab, 13},
			{ (int)RyuAttacks.Short, 20},
			{ (int)RyuAttacks.Strong, 26},
			{ (int)RyuAttacks.Fierce, 25},
			{ (int)RyuAttacks.HighRoundhouse, 25},
			{ (int)RyuAttacks.Overhead, 32},
			{ (int)RyuAttacks.CrouchForward, 25},			
			{ (int)RyuAttacks.CrouchFierce, 41},
			{ (int)RyuAttacks.CloseForward, 36},

			{ (int)RyuAttacks.JumpShort, 28},

			{ (int)RyuAttacks.SolarPlexus, 31},
			{ (int)RyuAttacks.RunningKick, 30},

			{ (int)RyuAttacks.Shoryuken, 35},
			{ (int)RyuAttacks.Tatsumaki, 45},
			{ (int)RyuAttacks.HardKnee, 30},
		};

		Dictionary<int, int> ryuExEndFrames = new Dictionary<int, int> {
			{ (int)RyuAttacks.HardKnee, 18},
		};

		int endFrames;

		if (isEXAttack) {
			endFrames = ryuExEndFrames[attackType];
		}
		else {
			endFrames = ryuEndFrames[attackType];
		}

		return endFrames;
	}

	public static int RyuAttacksCancelWindow (int attackType, bool isEXAttack = false) {
		Dictionary<int, int> ryuCancelWindow = new Dictionary<int, int> {
			{ (int)RyuAttacks.Jab, 8},
			{ (int)RyuAttacks.Short, 9},
			{ (int)RyuAttacks.Strong, 26},
			{ (int)RyuAttacks.Fierce, 9},
			{ (int)RyuAttacks.HighRoundhouse, 25},
			{ (int)RyuAttacks.Overhead, 12},
			{ (int)RyuAttacks.CrouchForward, 10},			
			{ (int)RyuAttacks.CrouchFierce, 13},
			{ (int)RyuAttacks.CloseForward, 36},

			{ (int)RyuAttacks.JumpShort, 28},

			{ (int)RyuAttacks.SolarPlexus, 12},
			{ (int)RyuAttacks.RunningKick, 24},

			{ (int)RyuAttacks.Shoryuken, 35},
			{ (int)RyuAttacks.Tatsumaki, 45},
			{ (int)RyuAttacks.HardKnee, 35},
		};

		Dictionary<int, int> ryuExCancelWindow = new Dictionary<int, int> {
			{ (int)RyuAttacks.HardKnee, 15},
		};

		int cancelWindow;

		if (isEXAttack) {
			cancelWindow = ryuExCancelWindow[attackType];
		}
		else {
			cancelWindow = ryuCancelWindow[attackType];
		}

		return cancelWindow;
	}
	#endregion

}
