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

    [SerializeField] public MeleeWeapon meleeWeapon;

    private CharMovementController movementController;
    private CharacterView characterView;

    [Header("Flash System")]
    [SerializeField] public Transform CharModel;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float flashTimer;
    private const float flashDuration = 0.3f;
    private bool isFlashing;

    public void Initialize(Owner enemyOwner, Owner teamOwner, CharacterData data)
    {
        this.enemyOwner = enemyOwner;
        this.teamOwner = teamOwner;
        this.characterData = data.Clone();
        health = characterData.health;
        
        currentHealth = health;

        characterView = GetComponent<CharacterView>();
        movementController = GetComponent<CharMovementController>();

        characterView.Initialize(this);
        movementController.Initialize(this, teamOwner, enemyOwner);
    }

    private void Start()
    {
        spriteRenderer = CharModel.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (characterData == null)
        {
            Debug.LogError($"CharacterData is not assigned on {name}");
            return;
        }

        health = characterData.health;
    }

    private void Update()
    {
        if (isFlashing)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                spriteRenderer.color = originalColor;
                isFlashing = false;
            }
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (gameObject == null || !gameObject.activeInHierarchy)
            return;

        //print($"Character {name} took {damageInfo.amount} damage");

        currentHealth -= damageInfo.amount;
       
        StartFlash();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void StartFlash()
    {
        if (spriteRenderer == null) return;

        flashTimer = flashDuration;
        isFlashing = true;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
    }

    private void Die()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

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
        characterData.health += 10; // Example upgrade
        characterData.damage += 10; // Example upgrade
        characterData.attackCooldown -= 0.1f; // Example upgrade
        characterData.movementSpeed += 1; // Example upgrade

        characterData.charLevel++;

        characterView.UpdateView();

        return true;

    }

    public void ResetChar()
    {
        currentHealth = health;
    }
}
