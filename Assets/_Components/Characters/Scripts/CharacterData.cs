using UnityEngine;

public class CharacterData
{
    public Color charColor;

    public Sprite charImage;

    public int charLevel;

    public string charName;

    public int priorityLevel;

    public int health;

    public int damage;

    public float range;

    public int movementSpeed;

    //public int attackSpeed;

    public int spawnCount;

    public float attackCooldown;

    public GameObject charPrefab;

    public CharacterData(CardData cardData)
    {
        spawnCount = cardData.spawnCount;
        charName = cardData.cardName;
        priorityLevel = cardData.priorityLevel;
        health = cardData.health;
        damage = cardData.damage;
        range = cardData.range;
        movementSpeed = cardData.movementSpeed;
        //attackSpeed = cardData.attackSpeed;
        charImage = cardData.cardImage;
        charColor = cardData.cardColor;
        charPrefab = cardData.charPrefab;
        attackCooldown = cardData.attackCooldown;
    }

    public CharacterData Clone()
    {
        return new CharacterData(this);
    }

    public CharacterData(CharacterData other)
    {
        charColor = other.charColor;
        charImage = other.charImage;
        charLevel = other.charLevel;
        charName = other.charName;
        priorityLevel = other.priorityLevel;
        health = other.health;
        damage = other.damage;
        range = other.range;
        movementSpeed = other.movementSpeed;
      //  attackSpeed = other.attackSpeed;
        spawnCount = other.spawnCount;
        attackCooldown = other.attackCooldown;
        charPrefab = other.charPrefab;
    }
}
