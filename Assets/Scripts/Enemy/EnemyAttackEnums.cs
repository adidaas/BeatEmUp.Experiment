using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackEnums : MonoBehaviour
{
	public static class AttackTriggerNames {
		// universal default attacks
        public static string AttackFarRoundhouse { get { return "attack_FarRoundhouse"; } }
		public static string AttackFierce { get { return "attack_Fierce"; } }
		public static string SpecialPowerDunk { get { return "special_PowerDunk"; } }
		public static string SpecialPowerDunkRecovery { get { return "special_PowerDunk_Recovery"; } }
	}
    public enum TerryAttacks { 
		// normals
		Jab = 1, Short = 2, Strong = 3, Forward = 4, Fierce = 5, FarRoundhouse = 6, Roundhouse = 7, CrouchForward = 8,
		PowerDunk = 9
	}

	// 0 = hurt high
	// 1 = hurt mid
	// 2 = launcher
	// 3 = air
	// 4 = launch back
	// 5 = hard knockdown
	public enum TerryAttacksHurtType { 
		// normals
		Jab = 0, Short = 1, Strong = 0, Forward = 0, Fierce = 0, FarRoundhouse = 1, Roundhouse = 0,
		PowerDunk_0 = 2, PowerDunk_1 = 5
	}

	public enum TerryAttacksScreenShake { 
		// normals
		FarRoundhouse = 1, Roundhouse = 1, Fierce = 1,
		PowerDunk = 2
	}

    public static int TerryAttacksActiveFrames (int attackType) {
		Dictionary<int, int> terryActiveFrames = new Dictionary<int, int> {
			{ (int)TerryAttacks.Fierce, 5},
			{ (int)TerryAttacks.FarRoundhouse, 18},			
			{ (int)TerryAttacks.PowerDunk, 26},	
		};
		int activeFrames = terryActiveFrames[attackType];

		return activeFrames;
	}

	public static int TerryAttacksRecoveryFrames (int attackType) {
		Dictionary<int, int> terryRecoveryFrames = new Dictionary<int, int> {
			{ (int)TerryAttacks.Fierce, 11},
			{ (int)TerryAttacks.FarRoundhouse, 25},	
			{ (int)TerryAttacks.PowerDunk, 36},	
		};

		int recoveryFrames = terryRecoveryFrames[attackType];

		return recoveryFrames;
	}

	public static int TerryAttacksEndFrames (int attackType) {
		Dictionary<int, int> terryEndFrames = new Dictionary<int, int> {
			{ (int)TerryAttacks.Fierce, 26},
			{ (int)TerryAttacks.FarRoundhouse, 35},	
			{ (int)TerryAttacks.PowerDunk, 42},	
		};

		int endFrames = terryEndFrames[attackType];

		return endFrames;
	}
}
