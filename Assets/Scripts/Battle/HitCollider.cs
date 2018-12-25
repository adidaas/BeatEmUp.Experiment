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

	public AnimationClip hitSparkSmall;
	public AnimationClip hitSparkMid;
	public AnimationClip hitSparkBig;

	public GameObject hitSpark;
	public HitSparksController hitSparkController;

	public AudioSource audioSource;
    public GameObject attackUser;
	public ScreenShake cameraScreenShake;

	public GameObject comboCounterObject;
	public ComboCounter comboCounter;

	#region Start
	// Use this for initialization
	void Start () {
		var mainCamera = GameObject.FindWithTag("MainCamera");
		cameraScreenShake = mainCamera.GetComponent<ScreenShake>();
		hitSparkController = hitSpark.GetComponent<HitSparksController>();
		comboCounter = comboCounterObject.GetComponent<ComboCounter>();

	}
	#endregion


    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
		//Debug.Log(other.tag);
		if (other.tag != attackUser.tag && other.tag == "Enemy")
        {
            //Debug.Log("Hit " + other.name + " with " + attackName + " for " + damageValue + ".");
			var enemyController = other.transform.parent.GetComponent<EnemyController>();

			//audioSource.PlayOneShot(attackSound);


			// Hit spark
			// =================================
			GameObject hitSparkInstance = Instantiate(hitSpark, this.transform.position, this.transform.rotation);
			hitSparkInstance.layer = 12;
			HitSparksController hitSparkController = hitSparkInstance.GetComponent<HitSparksController>();
			hitSparkController.PlayHitSpark(hitSparkType);

			var timeToLive = SpecialEffectsEnums.GetHitSparkDestoryTime (hitSparkType);

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

            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);

			screenShakeType = 0;
        }
    }
}
