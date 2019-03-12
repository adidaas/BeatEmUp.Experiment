using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttack : MonoBehaviour
{
    private EnemyAttack enemyAttack;
    public GameObject terrySpecialEffect;
    private Vector3 spawnPosition;
	private bool isFacingRight = true;
	private Vector3 targetPosition;
	private float targetXPosition;
    private float targetYPosition;
    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private int currentStep = 0;

    private void Start() {
        enemyAttack = gameObject.GetComponent<EnemyAttack>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        myAnim = gameObject.GetComponent<Animator>();
    }

    public void InitializeSpecialAttack(int attackType, bool isFacingRightCheck) {
        currentStep = 0;
        isFacingRight = isFacingRightCheck;
		float animationDelay = 0f;
        if (attackType == (int)EnemyAttackEnums.TerryAttacks.PowerDunk) {
            // powerdunk
			targetXPosition = isFacingRight ? 15f : -15f;
            targetYPosition = 24.5f;
			animationDelay = 0.025f;
            float spawnPositionXModifier = isFacingRight ? 4.4f : -4.4f;
            spawnPosition = new Vector2(gameObject.transform.position.x + spawnPositionXModifier, gameObject.transform.position.y + 5f);
            
        }

        StartCoroutine(WaitToPlay(animationDelay, attackType));
    }

    private IEnumerator WaitToPlay(float duration, int attackType) {		
		yield return new WaitForSeconds(duration);	

		PlaySpecialAttackAnimation(attackType);
	}

    private void PlaySpecialAttackAnimation(int attackType) {
        if (attackType == (int)EnemyAttackEnums.TerryAttacks.PowerDunk) {
            if (currentStep == 0) {
                myRigidbody.velocity = new Vector2(targetXPosition, targetYPosition);
                currentStep++;
                StartCoroutine(WaitToPlay(0.27f, attackType));
            }
            else if (currentStep == 1) {
                myRigidbody.AddForce(targetPosition, ForceMode2D.Impulse);
				StartCoroutine(WaitToChangeBodyType());

                // power dunk effect prefab
                GameObject terrySpecialEffectInstanceBurst = Instantiate(terrySpecialEffect, spawnPosition, this.transform.rotation);
                terrySpecialEffectInstanceBurst.transform.localScale = isFacingRight ? new Vector2(-1f, 1f) : new Vector2(1f, 1f);
                terrySpecialEffectInstanceBurst.layer = GeneralEnums.GameObjectLayer.SpecialEffects;

                EffectsController effectsController = terrySpecialEffectInstanceBurst.GetComponent<EffectsController>();
                effectsController.isFacingRight = isFacingRight;
                effectsController.enemyUser = GeneralEnums.EnemyCharacters.Terry;
                effectsController.currentStep = 0;
                effectsController.PlaySpecialEffects((int)SpecialEffectsEnums.TerrySpecialEffectsType.PowerDunk);

                var timeToLive = SpecialEffectsEnums.GetEnemySpecialEffectsDestoryTime((int)SpecialEffectsEnums.TerrySpecialEffectsType.PowerDunk);
			
                float spawnPositionXModifier = isFacingRight ? 1.8f : -1.8f;

                spawnPosition = new Vector2(spawnPosition.x + spawnPositionXModifier, spawnPosition.y + 1f);
                GameObject terrySpecialEffectInstanceFlames = Instantiate(terrySpecialEffect, spawnPosition, this.transform.rotation, gameObject.transform);
                terrySpecialEffectInstanceBurst.layer = GeneralEnums.GameObjectLayer.SpecialEffects;

                EffectsController effectsControllerFlames = terrySpecialEffectInstanceFlames.GetComponent<EffectsController>();
                effectsControllerFlames.isFacingRight = isFacingRight;
                effectsControllerFlames.enemyUser = GeneralEnums.EnemyCharacters.Terry;
                effectsControllerFlames.currentStep = 1;
                effectsControllerFlames.PlaySpecialEffects((int)SpecialEffectsEnums.TerrySpecialEffectsType.PowerDunk);


			    Destroy (terrySpecialEffectInstanceBurst, timeToLive);
                Destroy (terrySpecialEffectInstanceFlames, timeToLive);

                myRigidbody.velocity = isFacingRight ? new Vector2(15f, -14f) : new Vector2(-15f, -14f);
                currentStep++;
                StartCoroutine(WaitToPlay(0.30f, attackType));
            }
            else if (currentStep == 2) {
                myAnim.SetTrigger(GeneralEnums.MovementTriggerNames.CrouchToStanding);
                currentStep++;
                enemyAttack.isAttacking = false;
                
            }
		}
    }
}