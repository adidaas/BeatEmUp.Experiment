using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHelper : MonoBehaviour {
    public static void SetHitCollider(ref HitCollider hitCollider, int attackType, bool isEXActive = false)
    {
        if ((int)PlayerAttackEnums.RyuAttacks.Jab == attackType) {
            hitCollider.attackName = "Jab";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Jab;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Jab;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Jab;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Small);
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.Short == attackType) {
            hitCollider.attackName = "Short";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Short;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Short;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Short;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Small);
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.Strong == attackType) {
            hitCollider.attackName = "Strong";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Strong;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Strong;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Strong;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Big;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.Forward == attackType) {
            hitCollider.attackName = "Forward";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Forward;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Forward;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Forward;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Mid;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.Fierce == attackType) {
            hitCollider.attackName = "Fierce";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Fierce;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Fierce;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Fierce;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Mid;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.Forward == attackType) {
            hitCollider.attackName = "HighRoundhouse";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.HighRoundhouse;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.HighRoundhouse;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.HighRoundhouse;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Big;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.Overhead == attackType) {
            hitCollider.attackName = "Overhead";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Overhead;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Overhead;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Overhead;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Mid;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.CrouchForward == attackType) {
            hitCollider.attackName = "CrouchForward";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.CrouchForward;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.CrouchForward;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.CrouchForward;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Small;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.CrouchFierce == attackType) {
            hitCollider.attackName = "CrouchFierce";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.CrouchFierce;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.CrouchFierce;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.CrouchFierce;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Big;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.CloseForward == attackType) {
            hitCollider.attackName = "CloseForward";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.CloseForward;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.CloseForward;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.CloseForward;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Big;
        }
        // jump attacks
        // ======================
        else if ((int)PlayerAttackEnums.RyuAttacks.JumpShort == attackType) {
            hitCollider.attackName = "Jump Short";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.JumpShort;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.JumpShort;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.JumpShort;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Mid;
        }
        // special attacks
        // ======================
        else if ((int)PlayerAttackEnums.RyuAttacks.Shoryuken == attackType) {
            hitCollider.attackName = "Shoryuken";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Shoryuken;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Shoryuken;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Shoryuken;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Big;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.Hadouken == attackType) {
            hitCollider.attackName = "Hadouken";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Hadouken;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Hadouken;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Hadouken;            

            var hitSparkType = isEXActive ? SpecialEffectsEnums.HitSparkType.RyuShakunetsu : SpecialEffectsEnums.HitSparkType.RyuHadouken;

            hitCollider.hitSparkType = (hitSparkType);
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.Tatsumaki == attackType) {
            hitCollider.attackName = "Tatsumaki";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.Tatsumaki;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Tatsumaki;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Tatsumaki;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Mid;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.SolarPlexus == attackType) {
            hitCollider.attackName = "Solar Plexus";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.SolarPlexus;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.SolarPlexus;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.SolarPlexus;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Mid;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.RunningKick == attackType) {
            hitCollider.attackName = "Running Kick";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.RunningKick;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.RunningKick;
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.RunningKick;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Mid;
        }
        else if ((int)PlayerAttackEnums.RyuAttacks.HardKnee == attackType) {
            hitCollider.attackName = "Hard Knee";
            hitCollider.attackType = (int)PlayerAttackEnums.RyuAttacks.HardKnee;
            hitCollider.hurtType = isEXActive ? (int)PlayerAttackEnums.RyuAttacksHurtType.EXHardKnee : (int)PlayerAttackEnums.RyuAttacksHurtType.HardKnee;            
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.HardKnee;

            hitCollider.hitSparkType = SpecialEffectsEnums.HitSparkType.Big;
        }
        hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance(attackType);
        hitCollider.hitStop = PlayerAttackEnums.RyuAttacksHitStop(attackType, isEXActive);
        
    }

    public static void SetEnemyHitCollider(ref HitCollider hitCollider, int attackType, int character)
    {
        if (character == 2) {
            if ((int)EnemyAttackEnums.TerryAttacks.FarRoundhouse == attackType) {
                hitCollider.attackName = "Far RoundHouse";
                hitCollider.attackType = (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse;
                hitCollider.hurtType = (int)EnemyAttackEnums.TerryAttacksHurtType.FarRoundhouse;
                hitCollider.screenShakeType = (int)EnemyAttackEnums.TerryAttacksScreenShake.FarRoundhouse;

                hitCollider.hitSparkType = (SpecialEffectsEnums.HitSparkType.Mid);
            }
            else if ((int)EnemyAttackEnums.TerryAttacks.Fierce == attackType) {
                hitCollider.attackName = "Fierce";
                hitCollider.attackType = (int)EnemyAttackEnums.TerryAttacks.Fierce;
                hitCollider.hurtType = (int)EnemyAttackEnums.TerryAttacksHurtType.Fierce;
                hitCollider.screenShakeType = (int)EnemyAttackEnums.TerryAttacksScreenShake.Fierce;

                hitCollider.hitSparkType = (SpecialEffectsEnums.HitSparkType.Mid);
            }
        }
        
    }    
}