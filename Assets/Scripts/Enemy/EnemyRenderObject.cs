using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRenderObject : MonoBehaviour {

	public GameObject parentObject;
	public EnemyController enemyController;

	private Vector3 targetPosition;
	private Vector3 originalPosition;
	private int currentStep;
	private bool isAnimationDone;

	// Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// if (!isAnimationDone) 
		// {
		// 	// Distance moved = time * speed.
        // float distCovered = (Time.time - startTime) * speed;

        // // Fraction of journey completed = current distance divided by total distance.
        // float fracJourney = distCovered / journeyLength;

        // // Set our position as a fraction of the distance between the markers.
        // transform.position = Vector3.Lerp(startMarker.position, endMarker, fracJourney);
		// }
	}

	void HurtLaunch_MovePosition()
	{		
		currentStep = 0;
		isAnimationDone = false;
		//Debug.Log("Hurt launch move position ======================");

		originalPosition = parentObject.transform.position;
		var nextPosition = new Vector3 (parentObject.transform.position.x + 2, parentObject.transform.position.y + 5f, parentObject.transform.position.z);
		StartCoroutine (MoveOverSpeed (parentObject, nextPosition , 30f));

		enemyController = parentObject.GetComponent<EnemyController>();

		// startMarker = parentObject.transform;
		// endMarker = nextPosition;
		
		

		// Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        //journeyLength = Vector3.Distance(startMarker.position, endMarker);
	}

	void HurtLaunch_MovePosition_NextStep(Vector3 currentPosition)
	{
		// Debug.Log("^^^^^^^^^^^^^^ HurtLaunch_MovePosition_NextStep");
		// Debug.Log("CurrentStep: " + currentStep);
		if (currentStep == 0) {
			currentPosition = new Vector3 (currentPosition.x + 1.5f, currentPosition.y - 5f, currentPosition.z);
			StartCoroutine (MoveOverSpeed (parentObject, currentPosition, 10f));
		} else if (currentStep == 1) {
			currentPosition = new Vector3 (currentPosition.x + 0.5f, originalPosition.y - 0.2f, currentPosition.z);
			StartCoroutine (MoveOverSpeed (parentObject, currentPosition, 12f));
			isAnimationDone = true;
			currentStep = 0;
		} else if (currentStep == 2) {
			currentPosition = new Vector3 (currentPosition.x + 0.5f, currentPosition.y - 0.9f, currentPosition.z);
			StartCoroutine (MoveOverSpeed (parentObject, currentPosition, 23f));
			isAnimationDone = true;
			currentStep = 0;
		}

		currentStep++;

		
	}

	public IEnumerator MoveOverSpeed (GameObject objectToMove, Vector3 end, float speed)
	{		
		while (objectToMove.transform.position != end && !isAnimationDone) {
			objectToMove.transform.position = 
					//Vector3.Lerp(objectToMove.transform.position, end, speed * Time.deltaTime);
					Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
					//Vector3.SmoothDamp(objectToMove.transform.position, end, velocity * Time.deltaTime, 0.3f);
			yield return new WaitForEndOfFrame ();
		}

		if (isAnimationDone == true) {
			StartCoroutine(enemyController.ToggleHurtWakeUp((int)PlayerAttackEnums.AttacksHurtType.Launch));
			
		}
		else {
			HurtLaunch_MovePosition_NextStep(end);
		}	
		
	}

	public IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
	{
		float elapsedTime = 0;
		Vector3 startingPos = objectToMove.transform.position;
		while (elapsedTime < seconds)
		{
			objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		objectToMove.transform.position = end;
	}
}
