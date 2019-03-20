using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHelper : MonoBehaviour
{
    public static string GetEnemyCharacterName(int enemyCharacter){
        if (enemyCharacter == (int)GeneralEnums.EnemyCharacters.Terry) {
            return "Terry Bogard";
        }

        return "";
    }
}
