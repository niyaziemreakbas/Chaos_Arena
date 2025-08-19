using System;
using B83.ExpressionParser;
using UnityEngine;

public class CardData
{
    public string cardName;

    public int priorityLevel;

    public int health;

    public int damage;

    public float range;

    public int movementSpeed;

    //public int attackSpeed;

    public int spawnCount;

    public float attackCooldown;

    public Sprite cardImage;

    public event Action OnStatsChanged;

    public Color cardColor;

    public GameObject charPrefab;

    private Expression healthExpr;
    private Expression damageExpr;
    private Expression goldExpr;

    private int level = 1;

    public int Level => level;

    public CardData(CardSO cardSO)
    {
        spawnCount = cardSO.spawnCount;
        priorityLevel = cardSO.priorityLevel;
        cardName = cardSO.cardName;
        health = cardSO.health;
        damage = cardSO.damage;
        range = cardSO.range;
        movementSpeed = cardSO.movementSpeed;
        cardImage = cardSO.cardImage;
        cardColor = cardSO.cardColor;
        charPrefab = cardSO.charPrefab;
        attackCooldown = cardSO.attackCooldown;

        healthExpr = Expression.Parse(cardSO.healthFormula);
        damageExpr = Expression.Parse(cardSO.damageFormula);
        goldExpr = Expression.Parse(cardSO.goldCostFormula);
    }

    public int GetUpgradeCost()
    {
        var del = goldExpr.ToDelegate("level");
        Debug.Log("getu pgrade Cost func: " + del(level + 1));
        return Mathf.RoundToInt((float)del(level + 1));
    }

    // Upgrade Logic for CardData
    public void UpgradeStats()
    {
        int cost = GetUpgradeCost();
        if (PlayerInfo.Instance.gold < cost)
        {
            Debug.Log("Yeterli alt�n yok! : " + cost);
            Debug.Log("level : " + level);
            return;
        }

        PlayerInfo.Instance.SetGold(-cost);;
        level++;

        // Yeni de�erleri hesapla
        var dmgDel = damageExpr.ToDelegate("level", "baseDamage");
        var hpDel = healthExpr.ToDelegate("level", "baseHealth");

        damage = Mathf.RoundToInt((float)dmgDel(level, damage));
        health = Mathf.RoundToInt((float)hpDel(level, health));
        // Gold cost or other requirements can be added here

        //health += 10;
        //damage += 5;

        OnStatsChanged?.Invoke();
    }
}

