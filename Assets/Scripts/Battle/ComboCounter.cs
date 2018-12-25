using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		comboCounterDisplay = comboCounter.GetComponent<UnityEngine.UI.Text>();
		comboBarSlider = comboBarContainer.GetComponentInChildren<UnityEngine.UI.Slider>();
		specialMeter = specialMeterContainer.GetComponent<SpecialMeter>();
	}
	
	// Update is called once per frame
	void Update () {
		if (comboTimer > 0f) {
            comboTimer -= 1 * Time.deltaTime ;
        }
        else {
			// convert combo count to special meter
			int specialMeterToAdd = currentComboCount * 10;
			specialMeter.ChangeSpecial(specialMeterToAdd);
			
			// reset all parameters
            currentComboCount = 0;
			comboCounterDisplay.text = "";
			isActive = false;
			backPanel.SetActive(false);
			comboBarContainer.SetActive(false);
        }

		if (isActive) {
			if (comboBarSlider.value > 0f) {				
				comboBarSlider.value -=  (1 * Time.deltaTime) / 0.8f;
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
