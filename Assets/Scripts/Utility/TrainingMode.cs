using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingMode : MonoBehaviour {
	public GameObject playerCharacter;
	public GameObject enemyCharacter;

	public Vector2 savedPlayerPosition;
	public Vector2 savedEnemyPosition;

	public GameObject notificationObject;
	private Text notificationMessage;

	private bool moveNotificationToScreen;

	// Use this for initialization
	void Start () {
		notificationMessage = notificationObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {		
		if (Input.GetKeyDown(KeyCode.Period)) {			
			notificationMessage.text = "Positions Saved";
			savedPlayerPosition = playerCharacter.transform.position;
			savedEnemyPosition = enemyCharacter.transform.position;
			StartCoroutine(WaitToDeleteNotification());
		}

		if (Input.GetKeyDown(KeyCode.Slash)) {
			notificationMessage.text = "Positions Reseted";
			playerCharacter.transform.position = savedPlayerPosition;
			enemyCharacter.transform.position =savedEnemyPosition;
			StartCoroutine(WaitToDeleteNotification());
		}
	}

	public IEnumerator WaitToDeleteNotification(){
		yield return new WaitForSeconds(3f);
		notificationMessage.text = "";
	}
}
