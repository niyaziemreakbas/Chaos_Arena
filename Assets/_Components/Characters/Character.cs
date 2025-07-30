using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private void Start()
    {
        
    }
}
public class CharacterData
{
    public string charName;

    public int priorityLevel;

    public int health;

    public int damage;

    public float range;

    public int movementSpeed;

    public int attackSpeed;

    public CharacterData(CardData cardData)
    {
        charName = cardData.cardName;
        priorityLevel = cardData.priorityLevel;
        health = cardData.health;
        damage = cardData.damage;
        range = cardData.range;
        movementSpeed = cardData.movementSpeed;
        attackSpeed = cardData.attackSpeed;
    }
}
