using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour {

    public string attackName;
	public int attackType;
	public AudioClip attackSound;
	public int hurtType;
	public float knockBackDistance;
	public float hitStop;
	public int screenShakeType;
    public int damageValue;
	public SpecialEffectsEnums.HitSparkType hitSparkType;
	public bool destroyOnHit = false;
	public GameObject parentObject;
	public BoxCollider2D myBoxCollider;

	public GameObject hitSpark;
	public HitSparksController hitSparkController;

	public ScreenShake cameraScreenShake;

	public GameObject comboCounterObject;
	public ComboCounter comboCounter;
	public GameObject styleManagerObject;
	public StyleManager styleManager;
	private PlayerController parentPlayerController;
	private PlayerAudio parentPlayerAudio;
	private PlayerAttack playerAttack;
	private Animator playerAnimator;
	private EffectsController effectsController;
	private EnemyController parentEnemyController;
	private EnemyAudio parentEnemyAudio;
	private Animator enemyAnimator;
	private bool isFacingRight;
	private bool knockbackLeft = true;
	private bool isPlayer = true;

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
			parentPlayerController = parentObject.GetComponent<PlayerController>();
			parentPlayerAudio = parentObject.GetComponent<PlayerAudio>();
			playerAttack = parentObject.GetComponent<PlayerAttack>();
			playerAnimator = parentObject.GetComponent<Animator>();
		}
		else if (parentObject.tag == GeneralEnums.GameObjectTags.BattleEffects) {
			effectsController = parentObject.GetComponent<EffectsController>();
		}
		else if (parentObject.tag == GeneralEnums.GameObjectTags.Enemy) {
			parentEnemyController = parentObject.GetComponent<EnemyController>();
			parentEnemyAudio = parentObject.GetComponent<EnemyAudio>();
			isPlayer = false;
		}
	}
	#endregion


    IEnumerator OnTriggerEnter2D(Collider2D other) {
		int layerMask = LayerMask.GetMask("Enemy");
		Vector2 checkLeftStartingPosition = new Vector2(parentObject.transform.position.x -0.4f, parentObject.transform.position.y + 1f);
		Vector2 checkUpLeftStartingPosition = new Vector2(parentObject.transform.position.x -0.5f, parentObject.transform.position.y);
		Vector2 checkRightStartingPosition = new Vector2(parentObject.transform.position.x + 0.4f, parentObject.transform.position.y + 1f);
		Vector2 checkUpRightStartingPosition = new Vector2(parentObject.transform.position.x + 0.5f, parentObject.transform.position.y);
		RaycastHit2D checkLeft = Physics2D.Raycast(checkLeftStartingPosition, Vector2.left, 3.3f, layerMask);
		RaycastHit2D checkUpLeft = Physics2D.Raycast(checkUpLeftStartingPosition, Vector2.up, 3.3f, layerMask);
		RaycastHit2D checkRight = Physics2D.Raycast(checkRightStartingPosition, Vector2.right, 3.3f, layerMask);
		RaycastHit2D checkUpRight = Physics2D.Raycast(checkUpRightStartingPosition, Vector2.up, 3.3f, layerMask);

		// player hit box
		if (gameObject.tag == GeneralEnums.GameObjectTags.PlayerHitbox && other.tag == GeneralEnums.GameObjectTags.EnemyHurtBox) {	
			// print(gameObject.tag);
			// print(other.tag);
			// print("player hitboxing");
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
				switchComboStyleSystem = parentPlayerController.switchComboStyleSystem;
				isFacingRight = parentPlayerController.isFacingRight;
				parentPlayerAudio.PlayHitSound((int)GeneralEnums.PlayerCharacters.Ryu, attackType);
				if (!enemyController.isAirJuggleable && hitStop > 0f) {
					StartCoroutine(playerAttack.PlayHitStop(hitStop));
				}
			}
			else if (parentObject.tag == GeneralEnums.GameObjectTags.BattleEffects) {
				isFacingRight = effectsController.isFacingRight;
				effectsController.PlaySoundEffect(attackType);
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
			if ((checkLeft.collider != null && checkLeft.transform.gameObject == other.transform.parent.gameObject)
				|| (checkUpLeft.collider != null && checkUpLeft.transform.gameObject == other.transform.parent.gameObject)) {
				knockbackLeft = true;
			}   
			else if (checkRight.collider != null && checkRight.transform.gameObject == other.transform.parent.gameObject
				|| (checkUpRight.collider != null && checkUpRight.transform.gameObject == other.transform.parent.gameObject) ) {
				 knockbackLeft = false;
			}   		
			enemyController.IsHit(hurtType, knockBackDistance, !isFacingRight, hitStop, damageValue);

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

		// enemy hit box
		if (gameObject.tag == GeneralEnums.GameObjectTags.EnemyHitbox && other.tag == GeneralEnums.GameObjectTags.PlayerHurtBox) {	
			var playerHurt = other.transform.parent.gameObject.GetComponent<PlayerHurt>();
			var playerController = other.transform.parent.gameObject.GetComponent<PlayerController>();
			playerController.canMove = false;
			playerController.canGroundAttack = false;
			//print("Player hit******");

			// if player is invincible state; ignore hit colliders
			if (playerController.isInInvincibleState) {
				yield break;
			}

			
			if (playerController.isBlocking) {
				// if player is blocking and still has guard meter; no damage and play block effect
				if (playerHurt.HitWhileBlocking(damageValue)) {
					// block spark
					// =================================
					Vector3 blockSparkSpawnPosition = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
					blockSparkSpawnPosition.z = -1;

					var direction = parentEnemyController.isFacingRight ? 1 : -1;

					GameObject blockSparkInstance = Instantiate(hitSpark, blockSparkSpawnPosition, this.transform.rotation);
					blockSparkInstance.layer = GeneralEnums.GameObjectLayer.SpecialEffects;
					HitSparksController blockSparkController = blockSparkInstance.GetComponent<HitSparksController>();
					blockSparkController.PlayBlockSpark(hitSparkType, parentEnemyController.isFacingRight);

					parentEnemyAudio.PlayBlockSound(hitSparkType);
					var blockSparkTimeToLive = SpecialEffectsEnums.GetHitSparkDestoryTime(hitSparkType);

					Destroy (blockSparkInstance, blockSparkTimeToLive);
					yield break;
				}
				else {
					print("GUARD CRUSH");
					// guard meter is below 0; guard crush
					Vector3 guardCrushSpawnPosition = other.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
					guardCrushSpawnPosition.z = -1;

					var direction = parentEnemyController.isFacingRight ? 1 : -1;

					GameObject guardCrushInstance = Instantiate(hitSpark, guardCrushSpawnPosition, this.transform.rotation);
					guardCrushInstance.layer = GeneralEnums.GameObjectLayer.SpecialEffects;
					HitSparksController guardCrushController = guardCrushInstance.GetComponent<HitSparksController>();
					guardCrushController.PlayBlockSpark(SpecialEffectsEnums.HitSparkType.GuardCrush, parentEnemyController.isFacingRight);

					parentEnemyAudio.PlayBlockSound(SpecialEffectsEnums.HitSparkType.GuardCrush);					
					var blockSparkTimeToLive = SpecialEffectsEnums.GetHitSparkDestoryTime(SpecialEffectsEnums.HitSparkType.GuardCrush);

					Destroy (guardCrushInstance, blockSparkTimeToLive);
					yield break;
				}
				
			}

			// check if this is a special effect that needs to disappear upon hit
			if (destroyOnHit) {				
				Destroy(parentObject, 0.01f);
			}

			if (parentObject.tag == GeneralEnums.GameObjectTags.Player) { 
				parentEnemyAudio.PlayHitSound(parentEnemyController.enemyCharacter, attackType);
				isFacingRight = parentEnemyController.isFacingRight;
			}
			else if (parentObject.tag == GeneralEnums.GameObjectTags.BattleEffects) {
				isFacingRight = effectsController.isFacingRight;
				effectsController.PlaySoundEffect(attackType);
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

			// Player Reaction
			// =================================
			layerMask = LayerMask.GetMask("Player");

			if ((checkLeft.collider != null && checkLeft.transform.gameObject == other.transform.parent.gameObject)
				|| (checkUpLeft.collider != null && checkUpLeft.transform.gameObject == other.transform.parent.gameObject)) {
				knockbackLeft = true;
			}   
			else if (checkRight.collider != null && checkRight.transform.gameObject == other.transform.parent.gameObject
				|| (checkUpRight.collider != null && checkUpRight.transform.gameObject == other.transform.parent.gameObject) ) {
				 knockbackLeft = false;
			}   		
			playerHurt.IsHit(hurtType, knockBackDistance, isFacingRight, hitStop, damageValue);
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
