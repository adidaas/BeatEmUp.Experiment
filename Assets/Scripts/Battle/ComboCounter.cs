using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour {
	public PlayerInfoManager playerInfoManager;
	public GameObject comboCounter;
	private UnityEngine.UI.Text comboCounterDisplay;
	public GameObject backPanel;
	public GameObject comboBarContainer;
	public UnityEngine.UI.Slider comboBarSlider;
	public UnityEngine.UI.Image comboBarColor;
	private int currentComboCount = 0;
	private float comboTimer = 0f; 
	private float comboBarValue = 0f;
	private bool isActive = false;
	public GameObject specialMeterContainer;
	private SpecialMeter specialMeter;

	void Start () {
		comboCounterDisplay = comboCounter.GetComponent<UnityEngine.UI.Text>();
		comboBarSlider = comboBarContainer.GetComponentInChildren<UnityEngine.UI.Slider>();
		specialMeter = specialMeterContainer.GetComponent<SpecialMeter>();
	}
	
	void Update () {
		if (comboTimer > 0f) {
            comboTimer -= 0.5f * Time.deltaTime ;
        }
        else if (comboTimer <= 0f && currentComboCount > 0) {
			// convert combo count to special meter
			int specialMeterToAdd = currentComboCount * 10;
			playerInfoManager.AdjustSpecialMeter(specialMeterToAdd);
			
			// reset all parameters
            currentComboCount = 0;
			comboCounterDisplay.text = "";
			isActive = false;
			backPanel.SetActive(false);
			comboBarContainer.SetActive(false);
        }

		if (isActive) {
			if (comboBarSlider.value > 0f) {				
				comboBarSlider.value -=  ( 0.5f * Time.deltaTime) / 0.8f;
			}

			if (comboBarSlider.value < 0.3f) {				
				comboBarColor.color = Color.red;
			}
		}		
	}

	public void IncrementComboCounter() {
		currentComboCount++;		
		comboBarColor.color = new Color(255, 182, 51);
		if (currentComboCount > 1) {
			comboCounterDisplay.text = currentComboCount.ToString();

			if (!isActive) {				
				comboBarContainer.SetActive(true);
				isActive = true;
				backPanel.SetActive(true);
			}
		}

		comboTimer = 0.8f;
		comboBarValue = 1f;
		comboBarSlider.value = comboBarValue;
	}
}
