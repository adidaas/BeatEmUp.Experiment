using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour {

    public string attackName;
	public int attackId;
	public AudioClip attackSound;
	public int hurtType;
	public float knockBackDistance;
	public float hitStop;
	public int screenShakeType;
    public float damageValue;
	public SpecialEffectsEnums.HitSparkType hitSparkType;
	public bool destroyOnHit = false;
	public GameObject parentObject;
	public BoxCollider2D myBoxCollider;

	public GameObject hitSpark;
	public HitSparksController hitSparkController;

	public AudioSource audioSource;
	public ScreenShake cameraScreenShake;

	public GameObject comboCounterObject;
	public ComboCounter comboCounter;
	private PlayerController playerController;
	private Animator playerAnimator;
	private EffectsController effectsController;
	private Animator enemyAnimator;
	private bool isFacingRight;

	#region Start
	void Start () {
		var mainCamera = GameObject.FindWithTag(GeneralEnums.GameObjectTags.MainCamera);
		cameraScreenShake = mainCamera.GetComponent<ScreenShake>();
		hitSparkController = hitSpark.GetComponent<HitSparksController>();
		comboCounterObject = GameObject.FindWithTag(GeneralEnums.GameObjectTags.ComboCounter);
		comboCounter = comboCounterObject.GetComponent<ComboCounter>();		
		myBoxCollider = gameObject.GetComponent<BoxCollider2D>();
		parentObject = gameObject.transform.parent.gameObject;
		if (parentObject.tag == GeneralEnums.GameObjectTags.Player) {
			playerController = parentObject.GetComponent<PlayerController>();
			playerAnimator = parentObject.GetComponent<Animator>();
		}
		else if (parentObject.tag == GeneralEnums.GameObjectTags.BattleEffects) {
			effectsController = parentObject.GetComponent<EffectsController>();
		}
	}
	#endregion

    IEnumerator OnTriggerEnter2D(Collider2D other) {		
		if (other.tag != gameObject.tag && other.tag == GeneralEnums.GameObjectTags.EnemyHurtBox) {	
			var enemyController = other.transform.parent.gameObject.GetComponent<EnemyController>();

			// if enemy is invincible state, ignore hit colliders
			if (enemyController.isInInvincibleState) {
				yield break;
			}			

			// check if this is a special effect that needs to disappear upon hit
			if (destroyOnHit) {				
				Destroy(parentObject, 0.01f);
			}
			//audioSource.PlayOneShot(attackSound);

			if (parentObject.tag == GeneralEnums.GameObjectTags.Player) {
				isFacingRight = playerController.isFacingRight;
				if (!enemyController.isAirJuggleable && hitStop > 0f) {
					StartCoroutine(playerController.PlayHitStop(hitStop));
				}
			}
			else if (parentObject.tag == GeneralEnums.GameObjectTags.BattleEffects) {
				isFacingRight = effectsController.isFacingRight;
			}

			// Hit spark
			// =================================
			Vector3 hitSparkSpawnPosition = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
			hitSparkSpawnPosition.z = -1;
			GameObject hitSparkInstance = Instantiate(hitSpark, hitSparkSpawnPosition, this.transform.rotation);
			hitSparkInstance.layer = GeneralEnums.GameObjectLayer.SpecialEffects;
			HitSparksController hitSparkController = hitSparkInstance.GetComponent<HitSparksController>();
			hitSparkController.PlayHitSpark(hitSparkType);

			var timeToLive = SpecialEffectsEnums.GetHitSparkDestoryTime(hitSparkType);

			Destroy (hitSparkInstance, timeToLive);

			// Enemy Reaction
			// =================================
			enemyController.IsHit(hurtType, knockBackDistance, isFacingRight, hitStop);

			// Combo Counter
			// =================================			
			comboCounter.IncrementComboCounter();			

			// Screen Shake
			// =================================
			cameraScreenShake.ShakeScreenWithType(screenShakeType);

            yield return new WaitForSeconds(2.0f);
            gameObject.SetActive(false);			

			screenShakeType = 0;
        }
    }
}
