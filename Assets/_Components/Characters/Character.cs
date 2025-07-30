using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterData characterData;

    private int currentHealth;

    //int currentHealth = ;
    private void Start()
    {
        
    }

    public bool UpgradeChar()
    {
        if(characterData.charLevel >= 3)
        {
            Debug.LogWarning("Character is already at max level!");
            return false;
        }

        //Upgrade Logic
        print($"Upgrading character: {characterData.charName} to level {characterData.charLevel + 1}");
        characterData.charLevel++;
        return true;

    }

    public void SetCharData(CharacterData data)
    {
        characterData = data;
    }


}
public class CharacterData
{
    public int charLevel;

    public string charName;

    public int priorityLevel;

    public int health;

    public int damage;

    public float range;

    public int movementSpeed;

    public int attackSpeed;

    public int spawnCount;

    public CharacterData(CardData cardData)
    {
        spawnCount = cardData.spawnCount;
        charName = cardData.cardName;
        priorityLevel = cardData.priorityLevel;
        health = cardData.health;
        damage = cardData.damage;
        range = cardData.range;
        movementSpeed = cardData.movementSpeed;
        attackSpeed = cardData.attackSpeed;
    }
}
