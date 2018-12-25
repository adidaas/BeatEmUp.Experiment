using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

	public Transform target;
	public Vector3 initialPos;
	[Range(0f, 2f)]
	public float power = 0.7f;
	public float duration = 1.0f;
	public Transform myCamera;
	public float slowDownAmount = 1.0f;
	public bool shouldShake = false;

	Vector3 startPosition;
	float initialDuration;

	public static class ScreenShakeTriggerNames
    {
        public static string SmallVerticalShake { get { return "smallVertShake"; } }
		public static string StrongVerticalShake { get { return "strongVertShake"; } }
    }


	// Use this for initialization
	void Start () {
		target = GetComponent<Transform>();
		initialPos = target.localPosition;
		// myAnim = GetComponent<Animator>();
		// myAnim.SetBool("cameraIdle", true);

		// myCamera = GetComponent<Transform>();
		startPosition = target.localPosition;
		initialDuration = duration;
	}

	// public void Shake(float duration) {
	// 	if (duration > 0) {
	// 		pendingShakeDuration += duration;
	// 	}
	// }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.N)) {
			shouldShake = true;
			StartCoroutine(DoShake());
		}
		// if (shouldShake) {		
		// 	if (duration > 0) {
		// 		var randomPoint = new Vector3(
		// 			initialPos.x, 
		// 			initialPos.y + (Random.insideUnitSphere.y * power),
		// 			initialPos.z);
		// 		target.localPosition = randomPoint;
		// 		duration -= Time.deltaTime * slowDownAmount;
		// 	}
		// 	else {
		// 		shouldShake = false;
		// 		duration = initialDuration;
		// 		target.localPosition = initialPos;
		// 	}
		// }
		// if (Input.GetKeyDown(KeyCode.N)) {
		// 	myAnim.SetTrigger(ScreenShakeTriggerNames.SmallVerticalShake);
		// 	// pendingShakeDuration += 0.5f;
		// }
		// if (Input.GetKeyDown(KeyCode.M)) {
		// 	myAnim.SetTrigger(ScreenShakeTriggerNames.StrongVerticalShake);
		// 	// pendingShakeDuration += 0.5f;
		// }

		// if (pendingShakeDuration > 0 && !isShaking) {
		// 	StartCoroutine(DoShake());
		// }
	}

	IEnumerator DoShake() {
		while (duration > 0) {
			var randomPoint = new Vector3(
				initialPos.x, 
				initialPos.y + (Random.insideUnitSphere.y * power),
				initialPos.z);
			target.localPosition = randomPoint;
			duration -= Time.deltaTime * slowDownAmount;
			yield return null;
		}
		
		shouldShake = false;
		duration = initialDuration;
		target.localPosition = initialPos;
		

		// isShaking = true;

		// var ctr = 0;
		// var startTime = Time.realtimeSinceStartup;
		// while (Time.realtimeSinceStartup < startTime + pendingShakeDuration) {
		// 	// Debug.Log(startTime);
		// 	if (ctr < 8) {
		// 		// var randomPoint = new Vector3(
		// 		// target.localPosition.x + Random.Range(-0.2f, 0.2f) * intensity, 
		// 		// target.localPosition.y + Random.Range(-0.2f, 0.2f) * intensity,
		// 		// initialPos.z);
		// 		var randomPoint = new Vector3();
		// 		if (ctr % 2 == 0) {
		// 			randomPoint = new Vector3(
		// 			initialPos.x, 
		// 			target.localPosition.y + 0.3f * intensity,
		// 			initialPos.z);
		// 		}
		// 		else {
		// 			randomPoint = new Vector3(
		// 			initialPos.x, 
		// 			target.localPosition.y - 0.3f * intensity,
		// 			initialPos.z);
		// 		}
				
		// 		target.localPosition = randomPoint;

		// 		ctr++;
		// 	}			
		// 	yield return null;
		// }

		// pendingShakeDuration = 0f;
		// target.localPosition = initialPos;
		// isShaking = false;
	}

	public void ShakeScreenWithType(int shakeType) {
		if (shakeType == 1) {
			shouldShake = true;
			power = 0.15f;
			duration = 0.15f;
			StartCoroutine(DoShake());
			//myAnim.SetTrigger(ScreenShakeTriggerNames.SmallVerticalShake);
			// pendingShakeDuration += 0.5f;
		}
		if (shakeType == 2) {
			shouldShake = true;
			power = 0.3f;
			duration = 0.2f;
			StartCoroutine(DoShake());
			//myAnim.SetTrigger(ScreenShakeTriggerNames.StrongVerticalShake);
			// pendingShakeDuration += 0.5f;
		}
	}
}
