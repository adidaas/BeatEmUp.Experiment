using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

	private GameObject parentObject;
	private Rigidbody2D parentRigidBody;
	private PlayerController playerController;
	private EnemyController enemyController;
	private bool slideParentObject = false;

	void Start () {
		parentObject = gameObject.transform.parent.gameObject;
		parentRigidBody = parentObject.GetComponent<Rigidbody2D>();
		if (gameObject.transform.parent.tag == "Player") {
			playerController = parentObject.GetComponent<PlayerController>();
		}
		else if (gameObject.transform.parent.tag == "Enemy") {
			enemyController = parentObject.GetComponent<EnemyController>();
		}
		
	}
	
	
	void Update () {
		if (slideParentObject) {
			int direction = 0;
			if (playerController != null) {
				 direction = playerController.isFacingRight ? 100 : -100;
			}
			else if (enemyController != null) {
				 direction = enemyController.isFacingRight ? 100 : -100;
			}			 

			parentRigidBody.AddForce(transform.right * direction);
		}		
	}

	

	// void OnTriggerEnter2D(Collider2D other)	{		 
	// 	if (other.tag != gameObject.tag) {			 
	// 		slideParentObject = true;			 
	// 	}
	// 	if (other.tag == "Ground") {			 
	// 		slideParentObject = false;			 
	// 	}
			
	// }
}
