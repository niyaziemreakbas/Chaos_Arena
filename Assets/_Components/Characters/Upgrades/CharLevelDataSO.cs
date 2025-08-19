using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Chars/LevelData")]
public class CharLevelDataSO : ScriptableObject
{
    public string characterName = "Blup";

    [Header("Formulas")]
    public string healthFormula = "baseHealth + level * 20";

    public string damageFormula = "baseDamage + level * 5";

    public string cooldownFormula = "baseCooldown - level * 0.05";

    //[Header("Sprites")]
    //public List<Sprite> levelSprites;
}
