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

    void Start() {
        
    }

    void Update() {
        
    }

    public void PlayHitSound(int character, int attackType) {
        if (character == (int)GeneralEnums.PlayerCharacters.Ryu) {
            if (attackType == (int)PlayerAttackEnums.RyuAttacks.Jab) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitLight0, soundHitLight1);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Short) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitLight2, soundHitMedium0);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Strong) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard0, soundHitMedium2);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Fierce) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard0, soundHitMedium3);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.HighRoundhouse) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard1, soundHitMedium2);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Overhead) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard1, soundHitMedium1);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CrouchForward) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitLight1, soundHitLight2);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CrouchFierce) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard0, soundHitMedium2);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CloseForward) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard3, soundHitMedium3);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Shoryuken) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard2, soundHitHard3);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Tatsumaki) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitMedium0, soundHitMedium3);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.HardKnee) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard3, soundHitHard2);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.RunningKick) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitMedium1, soundHitMedium2);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.SolarPlexus) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitHard0, soundHitHard1);
            }
            else if (attackType == (int)PlayerAttackEnums.RyuAttacks.JumpShort) {
                SoundEffectsManager.instance.RandomizeSfx(0.4f, soundHitLight2, soundHitMedium0);
            }
        }
    }

    public void PlayAttackSound(int character, int attackType, bool isExAttack) {
        if (character == 1) {
            if (!isExAttack) {
                if (attackType == (int)PlayerAttackEnums.RyuAttacks.Jab) {
                SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing0, soundRyuSwing1);
                SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingLight0, soundSwingLight1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Short) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing4, soundRyuSwing6);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium0, soundSwingMedium1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Fierce) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing1, soundRyuSwing2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium2, soundSwingMedium0);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Strong) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing2, soundRyuSwing3);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.HighRoundhouse) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing3);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Forward) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing6, soundRyuSwing0);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Overhead) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing1, soundRyuSwing2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium2, soundSwingMedium0);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CrouchForward) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing3);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CrouchFierce) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing4, soundRyuSwing2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium2, soundSwingHard1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.CloseForward) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing4, soundRyuSwing2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard1);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Shoryuken) {
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard2, soundSwingHard1);
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuShoryuken);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Tatsumaki) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuTatsumaki);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium1, soundSwingMedium2);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.HardKnee) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard2);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Hadouken) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuHadouken);
                    SoundEffectsManager.instance.RandomizeSfx(0.3f, soundHadoukenSwing);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.JumpShort) {
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundRyuSwing0, soundRyuSwing2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingLight1, soundSwingMedium0);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.RunningKick) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing5, soundRyuSwing4);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingMedium1, soundSwingMedium0);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.SolarPlexus) {
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundRyuSwing3, soundRyuSwing2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard1, soundSwingMedium0);
                }

            }
            else {
                if (attackType == (int)PlayerAttackEnums.RyuAttacks.HardKnee) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuSwing3, soundRyuSwing5);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundSwingHard0, soundSwingHard2);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundExActivate);
                }
                else if (attackType == (int)PlayerAttackEnums.RyuAttacks.Hadouken) {
                    SoundEffectsManager.instance.RandomizeSfx(0.7f, soundRyuHadouken);
                    SoundEffectsManager.instance.RandomizeSfx(0.5f, soundExActivate);
                }

            }
        }
    }
}
