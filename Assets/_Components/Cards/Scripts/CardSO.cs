using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card")]
public class CardSO : ScriptableObject
{
    [Header("Card Details")]
    public string cardName;

    public int health;

    public int damage;

    public float range;

    public float movementSpeed;

    public int attackSpeed;

    public int priorityLevel;

    public int spawnCount;

    public float attackCooldown;

    public Sprite cardImage;

    public Color cardColor;

    public GameObject charPrefab;

    public int maxUnitsPerRow = 6;

    [Header("Upgrade Formulas")]
    public string healthFormula = "baseHealth + level * 20";

    public string damageFormula = "baseDamage + level * 5";

    public string goldCostFormula = "100 * level ^ 1.2";
}
