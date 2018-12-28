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

	public static class ScreenShakeTriggerNames {
        public static string SmallVerticalShake { get { return "smallVertShake"; } }
		public static string StrongVerticalShake { get { return "strongVertShake"; } }
    }

	void Start () {
		target = GetComponent<Transform>();
		initialPos = target.localPosition;
		// myAnim = GetComponent<Animator>();
		// myAnim.SetBool("cameraIdle", true);

		// myCamera = GetComponent<Transform>();
		startPosition = target.localPosition;
		initialDuration = duration;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.N)) {
			shouldShake = true;
			StartCoroutine(DoShake());
		}
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
		

	}

	public void ShakeScreenWithType(int shakeType) {
		if (shakeType == 1) {
			shouldShake = true;
			power = 0.15f;
			duration = 0.15f;
			StartCoroutine(DoShake());
		}
		if (shakeType == 2) {
			shouldShake = true;
			power = 0.3f;
			duration = 0.2f;
			StartCoroutine(DoShake());
		}
	}
}
