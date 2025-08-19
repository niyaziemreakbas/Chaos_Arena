using B83.ExpressionParser;
using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharUpgradeManager : SingletonMonoBehaviour<CharUpgradeManager>
{
    [SerializeField] List<CharLevelDataSO> charLevelDataList;

    private Expression healthExpr;
    private Expression damageExpr;
    private Expression cooldownExpr;

    public LevelStats ReturnLevelData(int level, CharacterData baseData)
    {
        foreach (var charLevelData in charLevelDataList)
        {
            if (charLevelData.characterName == baseData.charName)
            {
                return CalculateLevelStats(level, charLevelData, baseData);
            }
        }

        return null;
    }

    private LevelStats CalculateLevelStats(int level, CharLevelDataSO charLevelData, CharacterData baseData)
    {
        var stats = new LevelStats();

        healthExpr = Expression.Parse(charLevelData.healthFormula);
        damageExpr = Expression.Parse(charLevelData.damageFormula);
        cooldownExpr = Expression.Parse(charLevelData.cooldownFormula);

        var dmgDel = damageExpr.ToDelegate("level", "baseDamage");
        var hpDel = healthExpr.ToDelegate("level", "baseHealth");
        var cdDel = cooldownExpr.ToDelegate("level", "baseCooldown");

        stats.health = (int)hpDel(level, baseData.baseHealth);
        stats.damage = (int)dmgDel(level, baseData.baseDamage);
        stats.attackCooldown = (float)cdDel(level, baseData.baseCooldown);

        print($"Calculating Level Stats for {charLevelData.characterName} at level {level}: " +
              $"Health: {stats.health}, Damage: {stats.damage}, Attack Cooldown: {stats.attackCooldown}");


        //stats.levelSprite = charLevelData.levelSprites[level];

        return stats;
    }
}
public class LevelStats
{
    public int damage;
    public int health;
    public float attackCooldown;
    public Sprite levelSprite;
}
