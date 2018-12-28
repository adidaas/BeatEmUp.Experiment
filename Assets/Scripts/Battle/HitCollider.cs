using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour {

    public string attackName;
	public int attackId;
	public AudioClip attackSound;
	public int hurtType;
	public float knockBackDistance;
	public int screenShakeType;
    public float damageValue;
	public int hitSparkType;
	public bool destroyOnHit = false;

	public GameObject hitSpark;
	public HitSparksController hitSparkController;

	public AudioSource audioSource;
	public ScreenShake cameraScreenShake;

	public GameObject comboCounterObject;
	public ComboCounter comboCounter;

	#region Start
	void Start () {
		var mainCamera = GameObject.FindWithTag("MainCamera");
		cameraScreenShake = mainCamera.GetComponent<ScreenShake>();
		hitSparkController = hitSpark.GetComponent<HitSparksController>();
		comboCounterObject = GameObject.FindWithTag("ComboCounter");
		comboCounter = comboCounterObject.GetComponent<ComboCounter>();		
	}
	#endregion

    IEnumerator OnTriggerEnter2D(Collider2D other) {		
		if (other.tag != gameObject.tag && other.tag == "Enemy") {			
			// check if this is a special effect that needs to disappear upon hit
			if (destroyOnHit) {
				GameObject parentObject = gameObject.transform.parent.gameObject;
				Destroy(parentObject, 0.01f);
			}

			var enemyController = other.transform.parent.GetComponent<EnemyController>();
			//audioSource.PlayOneShot(attackSound);

			// Hit spark
			// =================================
			GameObject hitSparkInstance = Instantiate(hitSpark, this.transform.position, this.transform.rotation);
			hitSparkInstance.layer = 12;
			HitSparksController hitSparkController = hitSparkInstance.GetComponent<HitSparksController>();
			hitSparkController.PlayHitSpark(hitSparkType);

			var timeToLive = SpecialEffectsEnums.GetHitSparkDestoryTime(hitSparkType);

			Destroy (hitSparkInstance, timeToLive);

			// Enemy Reaction
			// =================================
			enemyController.IsHit(hurtType, knockBackDistance);

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
