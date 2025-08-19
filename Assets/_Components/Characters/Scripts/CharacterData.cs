using B83.ExpressionParser;
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

    public int spawnCount;

    public float attackCooldown;

    public GameObject charPrefab;

    // Save for upcoming LevelDataSO
    public int baseHealth;
    public int baseDamage;
    public float baseCooldown;

    public void Upgrade()
    {

        charLevel++;

        LevelStats levelStats = CharUpgradeManager.Instance.ReturnLevelData(charLevel, this);

        health = levelStats.health;
        damage = levelStats.damage;
        attackCooldown = levelStats.attackCooldown;
        //charImage = levelStats.levelSprite;
        Debug.Log(charName + " upgraded to level " + charLevel + " with new stats: " +
                  health + " health, " + damage + " damage, " + attackCooldown + " attack cooldown.");
    }

    public CharacterData(CardData cardData)
    {
        spawnCount = cardData.spawnCount;
        charName = cardData.cardName;
        priorityLevel = cardData.priorityLevel;
        health = cardData.health;
        damage = cardData.damage;
        range = cardData.range;
        movementSpeed = cardData.movementSpeed;
        charImage = cardData.cardImage;
        charColor = cardData.cardColor;
        charPrefab = cardData.charPrefab;
        attackCooldown = cardData.attackCooldown;

        charLevel = 1;

        // baz deðerleri sakla
        baseHealth = health;
        baseDamage = damage;
        baseCooldown = attackCooldown;
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
        spawnCount = other.spawnCount;
        attackCooldown = other.attackCooldown;
        charPrefab = other.charPrefab;
    }


}
