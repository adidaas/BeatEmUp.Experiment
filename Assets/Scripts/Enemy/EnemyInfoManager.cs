using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoManager : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public GameObject enemyHealthObject;
	public SpecialMeter specialMeter;
	[Range(0f, 100f)]
	public int currentHealth;
	[Range(0f, 100f)]
	public int maxHealth;
	[Range(0f, 100f)]
	public int currentSpecial;
	[Range(0f, 100f)]
	public int currentSpecialLevel;
    public float singleHealthUnit;
    public Text currentEnemyName;
    public int currentEnemyCharacter;
    public int nextEnemyCharacter;

    public bool isHealthBarDisplaying = false;
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

        
	}

	public void AdjustHealthMeter(int amount, int current, int max) {
        if (!isHealthBarDisplaying) {
           enemyHealthObject.SetActive(true);
		   isHealthBarDisplaying = true;
        }

		singleHealthUnit = 100 / max;
		currentHealth = (int)(current * singleHealthUnit);

        if (currentEnemyCharacter != nextEnemyCharacter) {            
            currentEnemyName.text = EnemyHelper.GetEnemyCharacterName(nextEnemyCharacter);
            enemyHealth.InstantHealthChange(currentHealth, currentHealth * 0.01f);
			currentEnemyCharacter = nextEnemyCharacter;
        }

		enemyHealth.healthTarget = currentHealth;
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
