using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMeter : MonoBehaviour {

	public UnityEngine.UI.Slider specialBar;
	public UnityEngine.UI.Text specialCounterDisplay;
	
	public int currentSpecial;
	
	public float currentSpecialDisplay;
	
	public int specialTarget;
	
	public float specialTargetDisplay;
	public int currentSpecialLevel = 0;

	void Start() {
		specialTargetDisplay = specialTarget * 0.01f;
		currentSpecialDisplay = currentSpecial * 0.01f;
	}

	void Update() {
		if (specialTarget != currentSpecial) {
			currentSpecial = specialTarget;
			specialTargetDisplay = specialTarget * 0.01f;
		}	

		if (specialTargetDisplay != currentSpecialDisplay) {
			PositionSpecialBar();
		}		

		if (currentSpecial >= 100) {
			currentSpecialLevel++;
			specialTarget -= 100;
			currentSpecial -= 100;
			currentSpecialDisplay = currentSpecialDisplay * 0.01f;
			specialTargetDisplay = currentSpecialDisplay;

			specialCounterDisplay.text = currentSpecialLevel.ToString();
		}
	}

	public void ChangeSpecial(int specialToAdd) {
		specialTarget += specialToAdd;
	}
	
	private void PositionSpecialBar() {		
		currentSpecialDisplay = (specialTargetDisplay < currentSpecialDisplay) ?
					currentSpecialDisplay - (0.8f * Time.deltaTime) : currentSpecialDisplay + (0.8f * Time.deltaTime);
		if ( currentSpecialDisplay <= specialTargetDisplay + 0.02f &&	currentSpecialDisplay >= specialTargetDisplay - 0.02f) {
			currentSpecialDisplay = specialTargetDisplay;
		}
		specialBar.value = currentSpecialDisplay;
	}
}
