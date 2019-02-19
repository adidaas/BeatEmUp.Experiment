using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip soundTerrySwing0;
    public AudioClip soundTerrySwing1;
    public AudioClip soundTerrySwing2;
    public AudioClip soundTerrySwing3;
    public AudioClip soundTerrySwing4;
    public AudioClip soundTerrySwing5;
    public AudioClip soundTerrySwing6;
    public AudioClip soundTerrySwing7;

    public AudioClip soundTerryHurtLight0;
	public AudioClip soundTerryHurtLight1;
	public AudioClip soundTerryHurtMedium0;
	public AudioClip soundTerryHurtMedium1;
	public AudioClip soundTerryHurtHard0;
	public AudioClip soundTerryHurtHard1;
	public AudioClip soundBodySlam0;

    // 0: Light0, 1: Light1, 2: Light2, 3: Light3
    // 4: Medium0, 5: Medium1, 6: Medium2, 7: Medium3
    // 8: Hard0, 9: Hard1, 10: Hard2, 11: Hard3

    public void PlayHitSound(int character, int attackType) {
        print("*****************************playhitosund");
        if (character == (int)GeneralEnums.EnemyCharacters.Terry) {
            if (attackType == (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse) {
                print("farroundhouse");
                SoundEffectsManager.instance.PlayRandomHitSound(0.5f, 5, 6);
            }
            else if (attackType == (int)EnemyAttackEnums.TerryAttacks.Fierce) {
                print("fierce-------------------------");
                SoundEffectsManager.instance.PlayRandomHitSound(0.5f, 4, 7);
            }
     
        }
    }
    
    public void PlayAttackSound(int character, int attackType) {
        if (character == (int)GeneralEnums.EnemyCharacters.Terry) {
            if (attackType == (int)EnemyAttackEnums.TerryAttacks.FarRoundhouse) {
                SoundEffectsManager.instance.RandomizeSfx(0.6f, soundTerrySwing6, soundTerrySwing5);
                SoundEffectsManager.instance.PlayRandomSwingSound(0.5f, 6, 7);
            }
            if (attackType == (int)EnemyAttackEnums.TerryAttacks.Fierce) {
                SoundEffectsManager.instance.RandomizeSfx(0.6f, soundTerrySwing4, soundTerrySwing3);
                SoundEffectsManager.instance.PlayRandomSwingSound(0.5f, 4, 5);
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
        }
	}
}
