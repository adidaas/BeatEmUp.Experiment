using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public Slider healthBar;
	[Range(0f, 100f)]
	public int currentHealth;
	[Range(0f, 1f)]
	public float currentHealthDisplay;
	[Range(0f, 100f)]
	public int healthTarget;
	[Range(0f, 1f)]
	public float healthTargetDisplay;
	public float maxHealth;

	void Start() {
		healthTargetDisplay = healthTarget * 0.01f;
		currentHealthDisplay = currentHealth * 0.01f;
	}


	void Update() {
		if (healthTarget != currentHealth) {
			currentHealth = healthTarget;
			healthTargetDisplay = healthTarget * 0.01f;
		}	

		if (healthTargetDisplay != currentHealthDisplay) {
			PositionHealthBar();
		}		
	}
	
	private void PositionHealthBar() {		
		currentHealthDisplay = (healthTargetDisplay < currentHealthDisplay) ?
					currentHealthDisplay - (0.8f * Time.deltaTime) : currentHealthDisplay + (0.8f * Time.deltaTime);
		if ( currentHealthDisplay <= healthTargetDisplay + 0.02f &&	currentHealthDisplay >= healthTargetDisplay - 0.02f) {
			currentHealthDisplay = healthTargetDisplay;
		}
		healthBar.value = currentHealthDisplay;
	}
}
