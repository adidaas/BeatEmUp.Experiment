using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnums : MonoBehaviour {    

	public enum AttacksHurtType { 
		High = 0, Mid = 1, Launch = 2, Air = 3, LaunchBack = 4, WallBounce = 5, Defeated = 6
	}

	public enum ScreenShakeType { 
		None = 0, Light = 1, Strong = 2 
	}

	public enum MovementDirection { 
		Up = 0, Down = 1, Left = 2, Right = 3
	}

	public static class HurtTriggers
	{
		public static string HurtHigh { get { return "hurt_High"; } }
		public static string HurtMid { get { return "hurt_Mid"; } }
		public static string HurtMidRecover { get { return "hurt_MidRecover"; } }
		public static string HurtHighRecover { get { return "hurt_HighRecover"; } }		
		public static string HurtLaunch { get { return "hurt_Launch"; } }
		public static string HurtLaunchBack { get { return "hurt_LaunchBack"; } }
		public static string HurtWakeUp { get { return "hurt_WakeUp"; } }
		public static string HurtAir { get { return "hurt_Air"; } }
		public static string HurtGround { get { return "hurt_Ground"; } }
		public static string HurtFalling { get { return "hurt_Falling"; } }
		public static string HurtDefeated { get { return "hurt_Defeated"; } }		
	}

	public static class GameObjectTags {
		public static string MainCamera { get { return "MainCamera"; } }
		public static string Player { get { return "Player"; } }
		public static string Enemy { get { return "Enemy"; } }
		public static string EnemyHurtBox { get { return "EnemyHurtBox"; } }
		public static string Ground { get { return "Ground"; } }
		public static string Wall { get { return "Wall"; } }
		public static string ComboCounter { get { return "ComboCounter"; } }
		public static string BattleEffects { get { return "BattleEffects"; } }
		public static string CharacterEffects { get { return "CharacterEffects"; } }
		public static string WorldEffects { get { return "WorldEffects"; } }
		public static string PlayerHitbox { get { return "PlayerHitbox"; } }
		public static string StyleManager { get { return "StyleManager"; } }		
		public static string PlayerHurtBox { get { return "PlayerHurtBox"; } }
		public static string EnemyHitbox { get { return "EnemyHitbox"; } }
	}

	public static class GameObjectLayer {
		public static int Ground { get { return 8; } }
		public static int Characters { get { return 9; } }
		public static int Hurtbox { get { return 10; } }
		public static int Hitbox { get { return 11; } }
		public static int SpecialEffects { get { return 12; } }
		public static int Enemy { get { return 13; } }
		public static int Player { get { return 14; } }
		public static int ComboCounter { get { return 15; } }
		public static int Wall { get { return 16; } }		
	}

	public static class MovementTriggerNames {
		// universal movements
        public static string MoveDash { get { return "move_Dash"; } }
		public static string MoveJump { get { return "move_Jump"; } }
		public static string AirIdle { get { return "air_Idle"; } }
		public static string AirFalling { get { return "air_Falling"; } }
		public static string AirLanding { get { return "air_Landing"; } }
		public static string CrouchToStanding { get { return "crouch_ToStanding"; } }
	}

	public enum PlayerCharacters {
		None = 0, Ryu = 1
	}

	public enum EnemyCharacters {
		None = 0, Terry = 1
	}

}
