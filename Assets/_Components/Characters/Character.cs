using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterData characterData;

    private int currentHealth;

    [SerializeField] List<Transform> patrolPoints; // Devriye noktalarý
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float arriveThreshold = 0.1f;

    private int currentTargetIndex = 0;

    void Update()
    {
        if (patrolPoints == null || patrolPoints.Count == 0) return;

        Transform target = patrolPoints[currentTargetIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= arriveThreshold)
        {
            currentTargetIndex = (currentTargetIndex + 1) % patrolPoints.Count;
        }
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
    public Sprite charImage;

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
        charImage = cardData.cardImage;
    }
}
