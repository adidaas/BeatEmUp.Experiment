using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityEnums : MonoBehaviour {

    public static class ScreenShakeTriggerNames
    {
        public static string SmallVerticalShake { get { return "smallVertShake"; } }
		public static string StrongVerticalShake { get { return "strongVertShake"; } }
    }
    
	public enum RyuAttacks { 
		// normals
		Jab = 0, Short = 1, Strong = 2, Forward = 3, 
		// special moves
		Shoryuken = 4 
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

	public enum AttacksHurtType { 
		High = 0, Mid = 1, Launch = 2 
	}

}
