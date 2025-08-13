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

    public int movementSpeed;

    public int attackSpeed;

    public int priorityLevel;

    public int spawnCount;

    public float attackCooldown;

    public Sprite cardImage;

    public Color cardColor;

    public GameObject charPrefab;
}
