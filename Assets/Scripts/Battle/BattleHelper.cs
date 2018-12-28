using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHelper : MonoBehaviour {
    public static void SetHitCollider(ref HitCollider hitCollider, PlayerAttackEnums.RyuAttacks attackType)
    {
        if (PlayerAttackEnums.RyuAttacks.Jab == attackType) {
            hitCollider.attackName = "Jab";
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Jab;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Jab;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)attackType);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Jab;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Small);
        }
        else if (PlayerAttackEnums.RyuAttacks.Short == attackType) {
            hitCollider.attackName = "Short";
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Short;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Short;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)attackType);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Short;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Small);
        }
        else if (PlayerAttackEnums.RyuAttacks.Strong == attackType) {
            hitCollider.attackName = "Strong";
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Strong;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Strong;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)attackType);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Strong;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Big);
        }
        else if (PlayerAttackEnums.RyuAttacks.Forward == attackType) {
            hitCollider.attackName = "Forward";
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Forward;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Forward;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)attackType);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Forward;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Mid);
        }
        else if (PlayerAttackEnums.RyuAttacks.Shoryuken == attackType) {
            hitCollider.attackName = "Shoryuken";
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Shoryuken;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Shoryuken;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)attackType);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Shoryuken;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Big);
        }
        else if (PlayerAttackEnums.RyuAttacks.Hadouken == attackType) {
            hitCollider.attackName = "Hadouken";
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Hadouken;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Hadouken;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)attackType);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Hadouken;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.RyuHadouken);
        }
        else if (PlayerAttackEnums.RyuAttacks.Tatsumaki == attackType) {
            hitCollider.attackName = "Tatsumaki";
            hitCollider.attackId = (int)PlayerAttackEnums.RyuAttacks.Tatsumaki;
            hitCollider.hurtType = (int)PlayerAttackEnums.RyuAttacksHurtType.Tatsumaki;
            hitCollider.knockBackDistance = PlayerAttackEnums.RyuAttacksKnockbackDistance((int)attackType);
            hitCollider.screenShakeType = (int)PlayerAttackEnums.RyuAttacksScreenShake.Tatsumaki;

            hitCollider.hitSparkType = ((int)SpecialEffectsEnums.HitSparkType.Mid);
        }
    }
    
}