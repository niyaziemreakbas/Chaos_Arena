using System.Collections.Generic;
using UnityEngine;

public enum CharState { Idle, Chasing, Attacking }

public class CharMovementController : MonoBehaviour
{
    private Character character;
    private Owner teamOwner;
    private Owner enemyOwner;

    private GameObject currentTarget;
    private CharState currentState;

    private CharAttackController attackController;



    public void Initialize(Character character, Owner teamOwner, Owner enemyOwner)
    {
        this.character = character;
        this.teamOwner = teamOwner;
        this.enemyOwner = enemyOwner;

        attackController = GetComponent<CharAttackController>();
        if (attackController != null)
            attackController.Initialize(character, character.CharacterData.attackCooldown);
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameState.Fight)
            return;

        switch (currentState)
        {
            case CharState.Idle:
                HandleIdle();
                break;
            case CharState.Chasing:
                HandleChasing();
                break;
            case CharState.Attacking:
                HandleAttacking();
                break;
        }
    }

    public void EnterState(CharState newState)
    {
        currentState = newState;

        if (newState == CharState.Chasing)
            SetClosestTarget();
    }

    private void SetClosestTarget()
    {
        List<GameObject> targets = enemyOwner.UnitRegistry.SpawnedCharacters;
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector2 pos = transform.position;

        foreach (var t in targets)
        {
            if(!t.activeInHierarchy) continue;
            if (t == null) continue;
            float dist = Vector2.Distance(pos, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        currentTarget = closest;
    }

    private void HandleIdle()
    {
        //print($"Character {character.name} is idle");
        SetClosestTarget();
        if(currentTarget != null && currentTarget.activeInHierarchy)
        {
            //print($"Character {character.name} found target: {currentTarget.name}");
            currentState = CharState.Chasing;
            return;
        }
    }

    private void HandleChasing()
    {
        SetClosestTarget();
        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            currentState = CharState.Idle;
            return;
        }

        float distance = Vector2.Distance(transform.position, currentTarget.transform.position);

        if (distance > character.CharacterData.range)
        {
            MoveTowards(currentTarget.transform.position);
        }
        else
        {
            EnterState(CharState.Attacking);
        }
    }

    private void HandleAttacking()
    {
        SetClosestTarget();
        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            EnterState(CharState.Idle);
            return;
        }

        float distance = Vector2.Distance(transform.position, currentTarget.transform.position);

        if (distance > character.CharacterData.range)
        {
            EnterState(CharState.Chasing);
        }
        else
        {
            attackController.TryAttack(currentTarget);
        }
    }

    private void MoveTowards(Vector2 targetPos)
    {
        Vector2 currentPos = transform.position;
        Vector2 dir = (targetPos - currentPos).normalized;
        transform.position = currentPos + dir * character.CharacterData.movementSpeed * Time.deltaTime;
    }
}
