using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsEnums : MonoBehaviour {

	public enum MovementEffectsType { RunDust = 0, RunDust02 = 1, JumpDust01 = 2, JumpDust02 = 3, WallSplash = 4 }

	public static class MovementEffectsTriggerNames {
        public static string RunDust { get { return "runDust"; } }
        public static string RunDust02 { get { return "runDust02"; } }
		public static string JumpDust01 { get { return "jumpDust01"; } }
		public static string JumpDust02 { get { return "jumpDust02"; } }
		public static string WallSplash { get { return "wallSplash"; } }
    }

	public static float GetEffectDestoryTime(CharacterEffectsEnums.MovementEffectsType effectType)	{
		if (effectType == MovementEffectsType.RunDust) {
			return 0.20f;
		}
        else if (effectType == MovementEffectsType.RunDust02) {
			return 0.25f;
		}
		else if (effectType == MovementEffectsType.JumpDust01) {
			return 0.20f;
		}
		else if (effectType == MovementEffectsType.JumpDust02) {
			return 0.17f;
		}
		else if (effectType == MovementEffectsType.JumpDust02) {
			return 0.17f;
		}
		else if (effectType == MovementEffectsType.WallSplash) {
			return 0.21f;
		}

		return 1.0f;
	}
}
