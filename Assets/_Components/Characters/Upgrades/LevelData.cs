using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelInfo", menuName = "Chars/LevelInfo")]
public class LevelData : ScriptableObject
{
    CharacterData characterData;

    int damage;

    int health;
}
