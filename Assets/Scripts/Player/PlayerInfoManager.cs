using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour {
	public PlayerHealth playerHealth;
	public SpecialMeter specialMeter;
	[Range(0f, 100f)]
	public int currentHealth;
	[Range(0f, 100f)]
	public int maxHealth;
	[Range(0f, 100f)]
	public int currentSpecial;
	[Range(0f, 100f)]
	public int currentSpecialLevel;
	[Range(0f, 80f)]
	public float currentGuardValue = 40;

	public bool isInInvincibleState = false;

	void Start () {
		
	}

	void Update () {
		if (currentSpecial >= 100) {
			currentSpecial -= 100;
			currentSpecialLevel++;
			specialMeter.ChangeSpecial(-100);
			specialMeter.ChangeSpecialLevel(1);
		}

		if (currentGuardValue < 80) {
			currentGuardValue += (1.5f * Time.deltaTime);
		}
	}

	public void AdjustHealthMeter(int amount) {
        currentHealth += amount;
		playerHealth.healthTarget = currentHealth;
    }

	public void AdjustSpecialMeter(int amount) {
        currentSpecial += amount;
		specialMeter.ChangeSpecial(amount);
    }

	public void ChangeSpecialLevel(int specialAmountToChange) {
		currentSpecialLevel += specialAmountToChange;		
		specialMeter.ChangeSpecialLevel(specialAmountToChange);
	}

	public void AdjustBlockValue(int amount) {
		currentGuardValue -= amount;
	}
}
