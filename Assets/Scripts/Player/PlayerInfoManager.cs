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

	void Start () {
		
	}

	void Update () {
		if (currentSpecial >= 100) {
			currentSpecial -= 100;
			currentSpecialLevel++;
			specialMeter.ChangeSpecial(-100);
			specialMeter.ChangeSpecialLevel(1);
		}
	}

	public void AdjustSpecialMeter(int amount) {
        currentSpecial += amount;
		specialMeter.ChangeSpecial(amount);
    }

	public void ChangeSpecialLevel(int specialAmountToChange) {
		currentSpecialLevel += specialAmountToChange;		
		specialMeter.ChangeSpecialLevel(specialAmountToChange);
	}
}
