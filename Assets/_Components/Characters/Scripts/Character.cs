using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    public static Action<Owner> OnCharDie;

    private CharacterData characterData;
    public CharacterData CharacterData => characterData;

    private Owner teamOwner;
    private Owner enemyOwner;

    public Owner TeamOwner => teamOwner;
    public Owner EnemyOwner => enemyOwner;

    private GameObject currentTarget;

    private float health;
    float currentHealth;

    [SerializeField] public Transform CharModel;
    [SerializeField] public MeleeWeapon meleeWeapon;

    private CharMovementController movementController;

    public void Initialize(Owner enemyOwner, Owner teamOwner, CharacterData data)
    {
        this.enemyOwner = enemyOwner;
        this.teamOwner = teamOwner;
        this.characterData = data;
        health = characterData.health;
        currentHealth = health;
        //meleeWeapon.SetWeapon(teamOwner, enemyOwner, characterData.damage);

        movementController = GetComponent<CharMovementController>();
        movementController.Initialize(this, teamOwner, enemyOwner);
    }

    private void Start()
    {
        if (characterData == null)
        {
            Debug.LogError($"CharacterData is not assigned on {name}");
            return;
        }

        health = characterData.health;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (gameObject == null || !gameObject.activeInHierarchy)
            return;

        //print($"Character {name} took {damageInfo.amount} damage");

        currentHealth -= damageInfo.amount;

        if (currentHealth <= 0)
        {
            // Flash bittikten sonra öl
            StartCoroutine(DamageFlashAndDie());
        }
        else
        {
            // Sadece flash
            StartCoroutine(DamageFlash());
        }
    }

    private IEnumerator DamageFlash()
    {
        SpriteRenderer sr = CharModel.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.2f);
            yield return new WaitForSeconds(0.3f);
            sr.color = originalColor;
        }
    }

    private IEnumerator DamageFlashAndDie()
    {
        yield return DamageFlash(); // Önce efekt
        Die(); // Sonra ölüm
    }

    private void Die()
    {
        SpriteRenderer sr = CharModel.GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f); // Ölüm sonrası renk değişimi
        gameObject.SetActive(false);

        OnCharDie?.Invoke(teamOwner);
    }

    public void OnFightStateStarted()
    {
        movementController.EnterState(CharState.Chasing);
    }

    public bool UpgradeChar()
    {
        if (characterData.charLevel >= 3)
        {
            Debug.LogWarning("Character is already at max level!");
            return false;
        }

        //Upgrade Logic
        //print($"Upgrading character: {characterData.charName} to level {characterData.charLevel + 1}");
        characterData.charLevel++;
        return true;

    }
}
