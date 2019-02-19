using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip soundRyuSwing0;
    public AudioClip soundRyuSwing1;
    public AudioClip soundRyuSwing2;
    public AudioClip soundRyuSwing3;
    public AudioClip soundRyuSwing4;
    public AudioClip soundRyuSwing5;
    public AudioClip soundRyuSwing6;
    public AudioClip soundRyuHadouken;
	public AudioClip soundRyuTatsumaki;
	public AudioClip soundRyuShoryuken;	

    public AudioClip soundRyuHurtLight0;
	public AudioClip soundRyuHurtLight1;
	public AudioClip soundRyuHurtMedium0;
	public AudioClip soundRyuHurtMedium1;
	public AudioClip soundRyuHurtHard0;
	public AudioClip soundRyuHurtHard1;

    public AudioClip soundSwingLight0;
	public AudioClip soundSwingLight1;
	public AudioClip soundSwingMedium0;
    public AudioClip soundSwingMedium1;
    public AudioClip soundSwingMedium2;
	public AudioClip soundSwingHard0;
    public AudioClip soundSwingHard1;
    public AudioClip soundSwingHard2;

	public AudioClip soundHitLight0;
    public AudioClip soundHitLight1;
    public AudioClip soundHitLight2;
    public AudioClip soundHitLight3;
    public AudioClip soundHitMedium0;
    public AudioClip soundHitMedium1;
    public AudioClip soundHitMedium2;
    public AudioClip soundHitMedium3;
    public AudioClip soundHitHard0;
    public AudioClip soundHitHard1;
    public AudioClip soundHitHard2;
    public AudioClip soundHitHard3;
    public AudioClip soundExActivate;

    public AudioClip soundHadoukenSwing;

    // 0: Light0, 1: Light1, 2: Light2, 3: Light3
    // 4: Medium0, 5: Medium1, 6: Medium2, 7: Medium3
    // 8: Hard0, 9: Hard1, 10: Hard2, 11: Hard3

    public void PlayHitSound(int character, int attackType) {
        if (character == (int)GeneralEnums.PlayerCharacters.Ryu) {
            if (attackType == (int)PlayerAttackEnums.RyuAttacks.Jab) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 0, 1);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Short) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 2, 3);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Strong) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 8, 6);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Fierce) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 7, 5);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.HighRoundhouse) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 9, 6);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Overhead) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 10, 4);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CrouchForward) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 3, 1);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CrouchFierce) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 7, 9);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CloseForward) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 11, 10);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Shoryuken) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 10, 11);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Tatsumaki) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 7, 6);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.HardKnee) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 8, 9);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.RunningKick) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 5, 6);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.SolarPlexus) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 8, 9);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.JumpShort) {
                SoundEffectsManager.instance.PlayRandomHitSound(0.4f, 3, 5);
            }
        }
    }

    // 0: Light0, 1: Light1, 2: Light2, 3: Light3
    // 4: Medium0, 5: Medium1, 6: Medium2, 7: Medium3
    // 8: Hard0, 9: Hard1, 10: Hard2, 11: Hard3
    public void PlayAttackSound(int character, int attackType, bool isExAttack) {
        if (character == 1) {
            if (!isExAttack) {
                if (attackType == (int)PlayerAttackEnums.RyuAttacks.Jab) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing0, soundRyuSwing1);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 0, 1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Short) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing4, soundRyuSwing6);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 3, 4);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Fierce) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing1, soundRyuSwing2);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 5, 6);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Strong) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing2, soundRyuSwing3);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 8, 7);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.HighRoundhouse) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing3);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 9, 11);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Forward) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing6, soundRyuSwing0);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 4, 5);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Overhead) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing1, soundRyuSwing2);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 4, 6);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CrouchForward) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing3);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 9, 7);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CrouchFierce) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing4, soundRyuSwing2);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 6, 8);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CloseForward) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing4, soundRyuSwing2);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 7, 9);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Shoryuken) {
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 10, 8);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuShoryuken);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Tatsumaki) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuTatsumaki);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 5, 6);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.HardKnee) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 8, 11);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Hadouken) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuHadouken);
                    SoundEffectsManager.instance.RandomizeSfx(0.3f, soundHadoukenSwing);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.JumpShort) {
                    SoundEffectsManager.instance.RandomizeSfx(0.4f, soundRyuSwing0, soundRyuSwing2);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 3, 5);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.RunningKick) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing4);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 6, 7);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.SolarPlexus) {
                    SoundEffectsManager.instance.RandomizeSfx(0.4f, soundRyuSwing3, soundRyuSwing2);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 8, 5);
                }

            }
            else {
                if (attackType == (int)PlayerAttackEnums.RyuAttacks.HardKnee) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing3, soundRyuSwing5);
                    SoundEffectsManager.instance.PlayRandomSwingSound(0.4f, 8, 11);
                    SoundEffectsManager.instance.RandomizeSfx(0.4f, soundExActivate);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Hadouken) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuHadouken);
                    SoundEffectsManager.instance.RandomizeSfx(0.4f, soundExActivate);
                }

            }
        }
    }

    public void PlayHurtSound(int character, int hurtType) {
         if (character == (int)GeneralEnums.PlayerCharacters.Ryu) {
            if (hurtType == (int)GeneralEnums.AttacksHurtType.Mid || hurtType == (int)GeneralEnums.AttacksHurtType.High) {
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundRyuHurtLight0, soundRyuHurtMedium1, soundRyuHurtHard0);
            }
        }
    }
}
