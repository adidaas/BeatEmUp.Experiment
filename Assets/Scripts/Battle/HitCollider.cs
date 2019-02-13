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
	public GameObject styleManagerObject;
	public StyleManager styleManager;
	private PlayerController playerController;
	private Animator playerAnimator;
	private EffectsController effectsController;
	private Animator enemyAnimator;
	private bool isFacingRight;
	private bool knockbackLeft = true;

	private int previousSeed = 10;
	private int randomCount = 1;

	private bool switchComboStyleSystem = true;

	#region Start
	void Start () {
		var mainCamera = GameObject.FindWithTag(GeneralEnums.GameObjectTags.MainCamera);
		cameraScreenShake = mainCamera.GetComponent<ScreenShake>();
		hitSparkController = hitSpark.GetComponent<HitSparksController>();
		comboCounterObject = GameObject.FindWithTag(GeneralEnums.GameObjectTags.ComboCounter);
		comboCounter = comboCounterObject.GetComponent<ComboCounter>();
		styleManagerObject = GameObject.FindWithTag(GeneralEnums.GameObjectTags.StyleManager);
		styleManager = styleManagerObject.GetComponent<StyleManager>();
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
				switchComboStyleSystem = playerController.switchComboStyleSystem;
				isFacingRight = playerController.isFacingRight;
				playerController.PlaySoundEffect(attackId);
				if (!enemyController.isAirJuggleable && hitStop > 0f) {
					StartCoroutine(playerController.PlayHitStop(hitStop));
				}
			}
			else if (parentObject.tag == GeneralEnums.GameObjectTags.BattleEffects) {
				isFacingRight = effectsController.isFacingRight;
				effectsController.PlaySoundEffect(attackId);
			}

			// Hit spark
			// =================================
			Vector3 hitSparkSpawnPosition = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
			hitSparkSpawnPosition.z = -1;
			Quaternion randomZRotation = this.transform.rotation; // new Vector3(0, 0, Random.Range(0.0f, 360.0f));
			var randomNumber = GetRandomNumber();
			

			GameObject hitSparkInstance = Instantiate(hitSpark, hitSparkSpawnPosition, Quaternion.Euler(new Vector3(0, 0, randomNumber)));
			hitSparkInstance.transform.Rotate(new Vector3(0, 0, GetRandomNumber()));
			hitSparkInstance.layer = GeneralEnums.GameObjectLayer.SpecialEffects;
			HitSparksController hitSparkController = hitSparkInstance.GetComponent<HitSparksController>();
			hitSparkController.PlayHitSpark(hitSparkType, isFacingRight);

			var timeToLive = SpecialEffectsEnums.GetHitSparkDestoryTime(hitSparkType);

			Destroy (hitSparkInstance, timeToLive);

			// Enemy Reaction
			// =================================
			int layerMask = LayerMask.GetMask("Enemy");
			Vector2 checkLeftStartingPosition = new Vector2(parentObject.transform.position.x -0.4f, parentObject.transform.position.y + 1f);
			Vector2 checkUpLeftStartingPosition = new Vector2(parentObject.transform.position.x -0.5f, parentObject.transform.position.y);
			Vector2 checkRightStartingPosition = new Vector2(parentObject.transform.position.x + 0.4f, parentObject.transform.position.y + 1f);
			Vector2 checkUpRightStartingPosition = new Vector2(parentObject.transform.position.x + 0.5f, parentObject.transform.position.y);
			RaycastHit2D checkLeft = Physics2D.Raycast(checkLeftStartingPosition, Vector2.left, 3.3f, layerMask);
			RaycastHit2D checkUpLeft = Physics2D.Raycast(checkUpLeftStartingPosition, Vector2.up, 3.3f, layerMask);
			RaycastHit2D checkRight = Physics2D.Raycast(checkRightStartingPosition, Vector2.right, 3.3f, layerMask);
			RaycastHit2D checkUpRight = Physics2D.Raycast(checkUpRightStartingPosition, Vector2.up, 3.3f, layerMask);

			if ((checkLeft.collider != null && checkLeft.transform.gameObject == other.transform.parent.gameObject)
				|| (checkUpLeft.collider != null && checkUpLeft.transform.gameObject == other.transform.parent.gameObject)) {
				knockbackLeft = true;
			}   
			else if (checkRight.collider != null && checkRight.transform.gameObject == other.transform.parent.gameObject
				|| (checkUpRight.collider != null && checkUpRight.transform.gameObject == other.transform.parent.gameObject) ) {
				 knockbackLeft = false;
			}   		
			enemyController.IsHit(hurtType, knockBackDistance, !isFacingRight, hitStop);
			enemyController.PlaySoundEffect(hurtType);

			// Combo Counter
			// =================================			
			if (switchComboStyleSystem) {
				comboCounter.IncrementComboCounter();
			}
			else {
				styleManager.IncrementStyleMeter();
			}

			// Screen Shake
			// =================================
			cameraScreenShake.ShakeScreenWithType(screenShakeType);

            yield return new WaitForSeconds(2.0f);

			screenShakeType = 0;
        }
    }

	float GetRandomNumber() {
		if (previousSeed > 0) {
			previousSeed = previousSeed * randomCount * randomCount;
			randomCount++;
		}
		Random.InitState(previousSeed);
		float randomZRotation = Random.Range(0.0f, 360.0f);

		if (randomCount > 8) {
			randomCount = 1;
			previousSeed = 45;
		}
		return randomZRotation;
	}
}
