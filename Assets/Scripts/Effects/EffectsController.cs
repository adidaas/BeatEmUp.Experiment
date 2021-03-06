using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour {
	public PlayerInfoManager playerInfoManager;
	public Animator myAnim;
	public GameObject hitBox;
	public HitCollider hitCollider;
	public bool isFacingRight = true;
	private bool isEXAttack = false;
	public int currentStep = 0;
	
	public GeneralEnums.PlayerCharacters playerUser;
	public GeneralEnums.EnemyCharacters enemyUser;

	public AudioClip soundHadoukenSwing;
	public AudioClip soundHadoukenHit0;
    public AudioClip soundHadoukenHit1;	

	void Awake() {
		myAnim = gameObject.GetComponent<Animator>();
		if (gameObject.tag == GeneralEnums.GameObjectTags.BattleEffects) {
			hitCollider = hitBox.GetComponent<HitCollider>();			
		}
	}

	public void PlaySpecialEffects(int effectType, bool isEXActive = false) {	
		
		isEXAttack = isEXActive;
		#region Character Effects
		if (gameObject.tag == GeneralEnums.GameObjectTags.CharacterEffects) {
			if (effectType == ((int)CharacterEffectsEnums.MovementEffectsType.RunDust)) {
				myAnim.SetTrigger(CharacterEffectsEnums.MovementEffectsTriggerNames.RunDust);
				Vector2 targetPosition = new Vector2();
				if (isFacingRight) {
					transform.localScale = new Vector2(1f, 1f);
					targetPosition = new Vector2(transform.position.x + 0.5f, transform.position.y);
				}
				else {
					transform.localScale = new Vector2(-1f, 1f);
					targetPosition =  new Vector2(transform.position.x - 0.5f, transform.position.y);
				}

				StartCoroutine(MoveSpecialEffectPosition(targetPosition, 1f));
			}
			else if(effectType == ((int)CharacterEffectsEnums.MovementEffectsType.RunDust02)) {
				myAnim.SetTrigger(CharacterEffectsEnums.MovementEffectsTriggerNames.RunDust02);
				Vector2 targetPosition = new Vector2();
				if (isFacingRight) {
					transform.localScale = new Vector2(1f, 1f);
					targetPosition = new Vector2(transform.position.x + 0.5f, transform.position.y);
				}
				else {
					transform.localScale = new Vector2(-1f, 1f);
					targetPosition =  new Vector2(transform.position.x - 0.5f, transform.position.y);
				}	

				StartCoroutine(MoveSpecialEffectPosition(targetPosition, 1f));
			}
			else if(effectType == ((int)CharacterEffectsEnums.MovementEffectsType.JumpDust01)) {
				myAnim.SetTrigger(CharacterEffectsEnums.MovementEffectsTriggerNames.JumpDust01);
				Vector2 targetPosition = new Vector2();
				if (isFacingRight) {
					transform.localScale = new Vector2(1f, 1f);
					targetPosition = new Vector2(transform.position.x + 0.5f, transform.position.y);
				}
				else {
					transform.localScale = new Vector2(-1f, 1f);
					targetPosition =  new Vector2(transform.position.x - 0.5f, transform.position.y);
				}	

				StartCoroutine(MoveSpecialEffectPosition(targetPosition, 1f));
			}
			else if(effectType == ((int)CharacterEffectsEnums.MovementEffectsType.JumpDust02)) {
				myAnim.SetTrigger(CharacterEffectsEnums.MovementEffectsTriggerNames.JumpDust02);
				transform.localScale = new Vector2(0.6f, 0.6f);
			}
			else if(effectType == ((int)CharacterEffectsEnums.MovementEffectsType.WallSplash)) {
				myAnim.SetTrigger(CharacterEffectsEnums.MovementEffectsTriggerNames.WallSplash);
				Vector2 targetPosition = new Vector2();
				if (isFacingRight) {
					transform.localScale = new Vector2(0.8f, 0.8f);
				}
				else {
					transform.localScale = new Vector2(-0.8f, 0.8f);					
				}	
			}
		}
		#endregion

		#region Ryu Attack Effects
		if (gameObject.tag == GeneralEnums.GameObjectTags.BattleEffects && GeneralEnums.PlayerCharacters.Ryu == playerUser) {
			if(effectType == ((int)SpecialEffectsEnums.RyuSpecialEffectsType.Hadouken)) {
				BattleHelper.SetHitCollider(ref hitCollider, (int)PlayerAttackEnums.RyuAttacks.Hadouken, isEXAttack);	
				var animTrigger = isEXActive ? SpecialEffectsEnums.RyuSpecialEffectsTriggerNames.Shakunetsu : SpecialEffectsEnums.RyuSpecialEffectsTriggerNames.Hadouken;
				myAnim.SetTrigger(animTrigger);
				Vector2 targetPosition = new Vector2();
				if (isFacingRight) {
					transform.localScale = new Vector2(1f, 1f);
					targetPosition = new Vector2(transform.position.x + 25, transform.position.y);
				}
				else {
					transform.localScale = new Vector2(-1f, 1f);
					targetPosition =  new Vector2(transform.position.x - 25, transform.position.y);
				}

				var speed = isEXActive ? 38f : 25f;

				StartCoroutine(MoveSpecialEffectPosition(targetPosition, speed));
			}
		}		
		#endregion

		#region Terry Attack Effects
		if (gameObject.tag == GeneralEnums.GameObjectTags.BattleEffects && GeneralEnums.EnemyCharacters.Terry == enemyUser) {
			if(effectType == ((int)SpecialEffectsEnums.TerrySpecialEffectsType.PowerDunk)) {
				if (currentStep == 0) {
					myAnim.SetTrigger(SpecialEffectsEnums.TerrySpecialEffectsTriggerNames.SpecialPowerDunk);
				}
				else if (currentStep == 1) {
					print("looping step-====-==-=-=--");
					BattleHelper.SetEnemyHitCollider(ref hitCollider, (int)EnemyAttackEnums.TerryAttacks.PowerDunk, (int)GeneralEnums.EnemyCharacters.Terry);	
					myAnim.SetTrigger(SpecialEffectsEnums.TerrySpecialEffectsTriggerNames.PowerDunkLoop);
					//myAnim.SetTrigger("powerDunk_Loop");
				}
				
			}
		}		
		#endregion
	}

	#region PlaySoundEffect
    public void PlaySoundEffect(int attackId) {
        if (attackId == (int)PlayerAttackEnums.RyuAttacks.Hadouken) {
            SoundEffectsManager.instance.RandomizeSfx(0.3f, soundHadoukenHit0);
			SoundEffectsManager.instance.RandomizeSfx(0.3f, soundHadoukenHit1);
        }

    }
    #endregion

	private IEnumerator WaitToPlay(float duration, int attackType) {		
		yield return new WaitForSeconds(duration);	

		PlaySpecialEffects(attackType);
	}

	public IEnumerator MoveSpecialEffectPosition(Vector2 targetPosition, float speed) {
		while ((Vector2)transform.position != targetPosition) {
			transform.position = 
					Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
						
			yield return new WaitForEndOfFrame ();
		}
	}

	public void AdjustSpecialMeter(int amount) {
        playerInfoManager.AdjustSpecialMeter(amount);
    }

}