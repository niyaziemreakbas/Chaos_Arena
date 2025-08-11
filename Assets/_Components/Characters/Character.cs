using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    private CharacterData characterData;

    [SerializeField] List<Transform> patrolPoints; // Devriye noktalarý
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float arriveThreshold = 0.1f;

    [SerializeField] private Transform currentTarget;

    private int currentTargetIndex = 0;

    private DOTMovement dotMovement;

    private Vector2 currentTargetPos;
    private bool isMoving = false;

    private void Awake()
    {
        dotMovement = GetComponent<DOTMovement>();
    }

    private void Start()
    {
        //MoveToTarget(currentTarget.position); // Baþlangýçta hedefe doðru hareket et
    }

    //void Update()
    //{
    //    if (currentTargetPos != null)
    //    {
    //        MoveTowardCurrentTarget();
    //    }
    //    if(isMoving)
    //    {
    //        dotMovement.SetState(DOTMovement.CharacterState.Move);
    //    }
    //    else
    //    {
    //        dotMovement.SetState(DOTMovement.CharacterState.Idle);
    //    }
    //    print
    //    // PatrolMovement();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false); // Karakter bir hedefe ulaþtýðýnda onu devre dýþý býrak
    }

    public GameObject GetClosestTarget(Owner targetOwner)
    {
        List<GameObject> targets = targetOwner.UnitRegistry.SpawnedCharacters;

        if (targets == null || targets.Count == 0)
            return null;

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            float distance = Vector2.Distance(currentPosition, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    public void MoveToTarget(Vector2 targetPosition)
    {
        currentTargetPos = targetPosition;
        isMoving = true;
    }

    private void MoveTowardCurrentTarget()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = (currentTargetPos - currentPosition).normalized;
        Vector2 newPosition = currentPosition + direction * moveSpeed * Time.deltaTime;

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        float distance = Vector2.Distance(currentPosition, currentTargetPos);
        if (distance <= arriveThreshold)
        {
            isMoving = false; // Hedefe ulaþýldý
            print($"Character {characterData.charName} reached target at {currentTargetPos}.");
        }
    }

    private void PatrolMovement()
    {
        if (patrolPoints == null || patrolPoints.Count == 0) return;

        // 2D pozisyonlar: sadece x ve y eksenleri
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(patrolPoints[currentTargetIndex].position.x, patrolPoints[currentTargetIndex].position.y);

        Vector2 direction = (targetPosition - currentPosition).normalized;
        Vector2 newPosition = currentPosition + direction * moveSpeed * Time.deltaTime;

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z); // z sabit kalmalý

        float distance = Vector2.Distance(currentPosition, targetPosition);
        if (distance <= arriveThreshold)
        {
            currentTargetIndex = (currentTargetIndex + 1) % patrolPoints.Count; // loop yapmak için
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
    public Color charColor; 

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
        charColor = cardData.cardColor;
    }
}
