using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectController : MonoBehaviour {
	public GameObject runDust;
	private Vector2 spawnPosition;
	private bool isFacingRight = true;
	private Vector2 targetPosition;
	private float targetXPosition;

    public void InitializeAnimationEffect(CharacterEffectsEnums.MovementEffectsType effectType, bool isFacingRightCheck) {
		isFacingRight = isFacingRightCheck;
		float animationDelay = 0f;
        if (effectType == CharacterEffectsEnums.MovementEffectsType.RunDust) {
            // run dust
			spawnPosition = isFacingRight ? new Vector2(this.transform.position.x - 1.8f, this.transform.position.y - 2.25f)
					: new Vector2(this.transform.position.x + 1.8f, this.transform.position.y - 2.25f);

			animationDelay = 0.015f;			
        }
        else if (effectType == CharacterEffectsEnums.MovementEffectsType.RunDust02) {
            // run dust 02
			spawnPosition = isFacingRight ? new Vector2(this.transform.position.x - 1.8f, this.transform.position.y - 2.15f)
					: new Vector2(this.transform.position.x + 1.8f, this.transform.position.y - 2.15f);

			animationDelay = 0.015f;			
        }
		else if (effectType == CharacterEffectsEnums.MovementEffectsType.JumpDust01) {
            // jump dust 01
			spawnPosition = isFacingRight ? new Vector2(this.transform.position.x - 1.5f, this.transform.position.y - 1.65f)
					: new Vector2(this.transform.position.x + 1.5f, this.transform.position.y - 1.65f);

			animationDelay = 0.015f;			
        }
		else if (effectType == CharacterEffectsEnums.MovementEffectsType.JumpDust02) {
            // jump dust 02
			spawnPosition = isFacingRight ? new Vector2(this.transform.position.x, this.transform.position.y - 2.9f)
					: new Vector2(this.transform.position.x, this.transform.position.y - 2.9f);

			animationDelay = 0.015f;			
        }
		else if (effectType == CharacterEffectsEnums.MovementEffectsType.WallSplash) {
            // wallsplash
			spawnPosition = isFacingRight ? new Vector2(this.transform.position.x - 0.5f, this.transform.position.y + 1.4f)
					: new Vector2(this.transform.position.x + 0.5f, this.transform.position.y + 1.4f);

			animationDelay = 0.015f;			
        }

		StartCoroutine(WaitToPlay(animationDelay, effectType));
    }

	private IEnumerator WaitToPlay(float duration, CharacterEffectsEnums.MovementEffectsType effectType) {		
		yield return new WaitForSeconds(duration);	

		PlayAnimationEffect(effectType);
	}

	private void PlayAnimationEffect(CharacterEffectsEnums.MovementEffectsType effectType) {
		if (effectType == CharacterEffectsEnums.MovementEffectsType.RunDust) {
            // runDust
			GameObject runDustInstance = Instantiate(runDust, spawnPosition, this.transform.rotation);			
			runDustInstance.layer = 12;
			
			EffectsController effectsController = runDustInstance.GetComponent<EffectsController>();
			effectsController.isFacingRight = isFacingRight;
			effectsController.PlaySpecialEffects((int)CharacterEffectsEnums.MovementEffectsType.RunDust);

			var timeToLive = CharacterEffectsEnums.GetEffectDestoryTime(CharacterEffectsEnums.MovementEffectsType.RunDust);
			
			Destroy (runDustInstance, timeToLive);			
        }
        else if (effectType == CharacterEffectsEnums.MovementEffectsType.RunDust02) {
            // runDust
			GameObject runDustInstance = Instantiate(runDust, spawnPosition, this.transform.rotation);			
			runDustInstance.layer = 12;
			
			EffectsController effectsController = runDustInstance.GetComponent<EffectsController>();
			effectsController.isFacingRight = isFacingRight;
			effectsController.PlaySpecialEffects((int)CharacterEffectsEnums.MovementEffectsType.RunDust02);

			var timeToLive = CharacterEffectsEnums.GetEffectDestoryTime(CharacterEffectsEnums.MovementEffectsType.RunDust02);
			
			Destroy (runDustInstance, timeToLive);			
        }
		else if (effectType == CharacterEffectsEnums.MovementEffectsType.JumpDust01) {
            // jumpdust
			GameObject runDustInstance = Instantiate(runDust, spawnPosition, this.transform.rotation);			
			runDustInstance.layer = 12;
			
			EffectsController effectsController = runDustInstance.GetComponent<EffectsController>();
			effectsController.isFacingRight = isFacingRight;
			effectsController.PlaySpecialEffects((int)CharacterEffectsEnums.MovementEffectsType.JumpDust01);

			var timeToLive = CharacterEffectsEnums.GetEffectDestoryTime(CharacterEffectsEnums.MovementEffectsType.JumpDust01);
			
			Destroy (runDustInstance, timeToLive);			
        }
		else if (effectType == CharacterEffectsEnums.MovementEffectsType.JumpDust02) {
            // jumpdust
			GameObject runDustInstance = Instantiate(runDust, spawnPosition, this.transform.rotation);			
			runDustInstance.transform.localScale = new Vector2(10, 10);
			runDustInstance.layer = 12;
			
			EffectsController effectsController = runDustInstance.GetComponent<EffectsController>();
			effectsController.isFacingRight = isFacingRight;
			effectsController.PlaySpecialEffects((int)effectType);

			var timeToLive = CharacterEffectsEnums.GetEffectDestoryTime(effectType);
			
			Destroy (runDustInstance, timeToLive);			
        }
		else if (effectType == CharacterEffectsEnums.MovementEffectsType.WallSplash) {
            // wallsplash
			GameObject runDustInstance = Instantiate(runDust, spawnPosition, this.transform.rotation);	
			runDustInstance.transform.localScale = new Vector2(10, 10);
			runDustInstance.layer = 12;
			
			EffectsController effectsController = runDustInstance.GetComponent<EffectsController>();
			effectsController.isFacingRight = isFacingRight;
			effectsController.PlaySpecialEffects((int)effectType);

			var timeToLive = CharacterEffectsEnums.GetEffectDestoryTime(effectType);
			
			Destroy (runDustInstance, timeToLive);			
        }
	}


}
