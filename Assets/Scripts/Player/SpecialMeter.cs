using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMeter : MonoBehaviour {
	public PlayerInfoManager playerInfoManager;
	public UnityEngine.UI.Slider specialBar;
	public UnityEngine.UI.Text specialCounterDisplay;
	[Range(0f, 100f)]
	public int currentSpecial;
	[Range(0f, 1f)]
	public float currentSpecialDisplay;
	[Range(0f, 100f)]
	public int specialTarget;
	[Range(0f, 1f)]
	public float specialTargetDisplay;
	public int currentSpecialLevelDisplay = 0;

	void Start() {
		currentSpecialLevelDisplay = playerInfoManager.currentSpecialLevel;
		specialTarget = playerInfoManager.currentSpecial;
		specialTargetDisplay = specialTarget * 0.01f;
		currentSpecialDisplay = currentSpecial * 0.01f;
	}

	void Update() {
		if (specialTarget != currentSpecial) {
			specialTargetDisplay = specialTarget * 0.01f;
		}	

		if (specialTargetDisplay != currentSpecialDisplay) {
			PositionSpecialBar();
		}		

		/* if (currentSpecial >= 100) {
			currentSpecialLevel++;
			specialTarget -= 100;
			currentSpecial -= 100;
			currentSpecialDisplay = currentSpecialDisplay * 0.01f;
			specialTargetDisplay = currentSpecialDisplay;

			specialCounterDisplay.text = currentSpecialLevel.ToString();
		} */
		specialCounterDisplay.text = currentSpecialLevelDisplay.ToString();
	}

	public void ChangeSpecial(int specialToAdd) {
		specialTarget += specialToAdd;
	}
	
	public void ChangeSpecialLevel(int specialAmountToChange) {
		currentSpecialLevelDisplay += specialAmountToChange;		
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
