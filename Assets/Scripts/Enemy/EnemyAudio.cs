using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip soundSwingLight0;
	public AudioClip soundSwingLight1;
	public AudioClip soundSwingMedium0;
    public AudioClip soundSwingMedium1;
    public AudioClip soundSwingMedium2;
	public AudioClip soundSwingHard0;
    public AudioClip soundSwingHard1;
    public AudioClip soundSwingHard2;


    public AudioClip soundTerryPowerDunk;
    public AudioClip soundTerrySwing0;
    public AudioClip soundTerrySwing1;
    public AudioClip soundTerrySwing2;
    public AudioClip soundTerrySwing3;
    public AudioClip soundTerrySwing4;
    public AudioClip soundTerrySwing5;
    public AudioClip soundTerrySwing6;
    public AudioClip soundTerrySwing7;

    public AudioClip soundTerrySpecialSwing0;
    public AudioClip soundTerrySpecialSwing1;
    public AudioClip soundTerrySpecialSwing2;
    public AudioClip soundTerrySpecialSwing3;

    public AudioClip soundTerryHurtLight0;
	public AudioClip soundTerryHurtLight1;
	public AudioClip soundTerryHurtMedium0;
	public AudioClip soundTerryHurtMedium1;
	public AudioClip soundTerryHurtHard0;
	public AudioClip soundTerryHurtHard1;
    public AudioClip soundTerryDefeated;
	public AudioClip soundBodySlam0;

    

    // 0: Light0, 1: Light1, 2: Light2, 3: Light3
    // 4: Medium0, 5: Medium1, 6: Medium2, 7: Medium3
    // 8: Hard0, 9: Hard1, 10: Hard2, 11: Hard3

    public void PlayHitSound(int character, int attackType) {
        if (character == (int)GeneralEnums.EnemyCharacters.Terry) {
            if (attackType == (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.5f, 5, 6);
            }
            else if (attackType == (int)EnemyAttackEnums.TerryAttacks.Fierce) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.5f, 4, 7);
            }     
        }
    }

    public void PlayBlockSound(SpecialEffectsEnums.HitSparkType hitsparkType) {        
        if (hitsparkType == SpecialEffectsEnums.HitSparkType.Small) {
            SoundEffectsManager.instance.PlayRandomBlockSound(0.5f, 0, 1);
        }
        else if (hitsparkType == SpecialEffectsEnums.HitSparkType.Mid) {
            SoundEffectsManager.instance.PlayRandomBlockSound(0.5f, 2, 3);
        }
        else if (hitsparkType == SpecialEffectsEnums.HitSparkType.Big) {
            SoundEffectsManager.instance.PlayRandomBlockSound(0.5f, 4);
        }
        else if (hitsparkType == SpecialEffectsEnums.HitSparkType.GuardCrush) {
            SoundEffectsManager.instance.PlayRandomBlockSound(0.4f, 6);
        }
    
    }
    
    public void PlayAttackSound(int character, int attackType, int step = 0) {
        if (character == (int)GeneralEnums.EnemyCharacters.Terry) {
            if (attackType == (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse) {
                SoundEffectsManager.instance.RandomizeSfx(0.6f, soundTerrySwing6, soundTerrySwing5);
                SoundEffectsManager.instance.PlayRandomSwingSound(0.5f, 6, 7);
            }
            if (attackType == (int)EnemyAttackEnums.TerryAttacks.Fierce) {
                SoundEffectsManager.instance.RandomizeSfx(0.6f, soundTerrySwing4, soundTerrySwing3);
                SoundEffectsManager.instance.PlayRandomSwingSound(0.5f, 4, 5);
            }
            if (attackType == (int)EnemyAttackEnums.TerryAttacks.PowerDunk && step == 0) {
                SoundEffectsManager.instance.PlaySingle(0.4f, soundSwingMedium1);
                SoundEffectsManager.instance.PlaySingle(0.5f, soundTerryPowerDunk);
            }
            else if (attackType == (int)EnemyAttackEnums.TerryAttacks.PowerDunk && step == 1) {
                print("second sound");
                SoundEffectsManager.instance.PlaySingle(0.2f, soundTerrySpecialSwing1);
                SoundEffectsManager.instance.PlaySingle(0.3f, soundTerrySpecialSwing2);
            }
        }
	}

    public void PlayHurtSound(int character, int hurtType) {
        if (character == (int)GeneralEnums.EnemyCharacters.Terry) {
            if (hurtType == (int)GeneralEnums.AttacksHurtType.Mid || hurtType == (int)GeneralEnums.AttacksHurtType.High) {
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundTerryHurtLight0, soundTerryHurtLight1, soundTerryHurtMedium0);
            }
            else if (hurtType == (int)GeneralEnums.AttacksHurtType.Launch) {
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundTerryHurtMedium1, soundTerryHurtHard0);
            }
            else if (hurtType == (int)GeneralEnums.AttacksHurtType.LaunchBack) {
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundTerryHurtMedium0, soundTerryHurtHard1);
            }
            else if (hurtType == (int)GeneralEnums.AttacksHurtType.WallBounce) {
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundTerryHurtLight0, soundTerryHurtLight1);
            }
            else if (hurtType == (int)GeneralEnums.AttacksHurtType.Defeated) {
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundTerryDefeated);
            }
        }
	}
}
