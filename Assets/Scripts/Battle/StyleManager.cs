using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleManager : MonoBehaviour
{
    public PlayerInfoManager playerInfoManager;
	public GameObject styleLevel;
	private UnityEngine.UI.Text styleLevelDisplay;
	public GameObject backPanel;
	public GameObject styleBarContainer;
	public UnityEngine.UI.Slider styleBarSlider;
	public UnityEngine.UI.Image styleBarColor;
    public int currentStyleLevel = 0;
    public float currentStyleMeter = 0;
    private bool isActive = false;
    void Start() {
        styleLevelDisplay = styleLevel.GetComponent<UnityEngine.UI.Text>();
        styleBarSlider = styleBarContainer.GetComponent<UnityEngine.UI.Slider>();
    }

    
    void Update() {
        if (currentStyleMeter > 0f) {
            currentStyleMeter -= 0.17f * Time.deltaTime;
        }
        else if (currentStyleMeter <= 0f) {
            currentStyleLevel = 0;

            // reset all parameters
            currentStyleLevel = 0;
			styleLevelDisplay.text = "";
			isActive = false;
			styleBarContainer.SetActive(false);
        }

        if (currentStyleMeter >= 1) {
            if (currentStyleLevel < 7)  {
                if (!isActive && currentStyleLevel > 0) {				
                    styleBarContainer.SetActive(true);
                    isActive = true;
                    currentStyleMeter += 0.25f;
                }
                currentStyleMeter = 0.42f;
                styleBarSlider.value = currentStyleMeter;
                currentStyleLevel++;
                AdjustStyleLevel();
            }
            else if (currentStyleLevel >= 6)  {
                currentStyleMeter = 1f;
                styleBarSlider.value = currentStyleMeter;
            }
            
        }

        if (isActive) {
			if (styleBarSlider.value > 0f) {				
				styleBarSlider.value -=  (0.25f * Time.deltaTime);
			}

			if (styleBarSlider.value < 0.3f) {				
				styleBarColor.color = Color.red;
			}
            else {
                styleBarColor.color = Color.white;
            }
		}	
        
    }

    public void IncrementStyleMeter() {
        if (!isActive && currentStyleLevel > 0) {				
            styleBarContainer.SetActive(true);
            isActive = true;
            currentStyleMeter += 0.25f;
        }
        currentStyleMeter += 0.18f;
        styleBarSlider.value = currentStyleMeter;        
    }

    void AdjustStyleLevel() {
        Color textColor;
        if (currentStyleLevel == 0)  {
            styleLevelDisplay.text = "";
        }
        else if (currentStyleLevel == 1)  {
            // Dim some, Don't want trouble
            //styleLevelDisplay.text = "<size=37>D</size><size=20>on't Want No Trouble</size>";
            styleLevelDisplay.text = "D<size=26>ynomite</size>";
            textColor = new Color(0.1f, 0.7f, 1);
            styleLevelDisplay.color = textColor;
        }
        else if (currentStyleLevel == 2)  {
            // Chai harder, Cam on
            styleLevelDisplay.text = "C<size=26>hillaxin</size>";
            //styleLevelDisplay.text = "C<size=20>hai Harder</size>";
            textColor = new Color(1, 0.5f, 0);
            styleLevelDisplay.color = textColor;
        }
        else if (currentStyleLevel == 3)  {
            // Bobalicious, Bobacious,
            styleLevelDisplay.text = "B<size=26>odacious</size>";
            //styleLevelDisplay.text = "B<size=20>obalicious</size>";
            textColor = new Color(0.5f, 1, 0.5f);
            styleLevelDisplay.color = textColor;
        }
        else if (currentStyleLevel == 4)  {
            // Asianomical, Asiasperating
            styleLevelDisplay.text = "A<size=26>aayyy!</size>";
            textColor = new Color(1, 0.5f, 0.5f);
            styleLevelDisplay.color = textColor;
        }
        else if (currentStyleLevel == 5)  {
            // Phonomenal
            styleLevelDisplay.text = "P<size=26>sychedelic!!</size>";
            //styleLevelDisplay.text = "P<size=20>honomenal!</size>";
            textColor = new Color(0.8f, 0.8f, 0);
            styleLevelDisplay.color = textColor;
        }
        else if (currentStyleLevel == 6)  {
            // Vietdiculous
            styleLevelDisplay.text = "T<size=26>rippendicular!!!</size>";
            //styleLevelDisplay.text = "V<size=20>ietdiculous!!!</size>";
            textColor = new Color(1, 1, 0.3f);
            styleLevelDisplay.color = textColor;
        }
        
    }
}
