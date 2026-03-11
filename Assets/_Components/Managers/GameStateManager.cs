using FurtleGame.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    public static event Action OnCharUIUpdateHandle;

    [SerializeField] TextMeshProUGUI upgradeCount;

    List<Owner> owners = new();

    private GameState currentState;
    public GameState CurrentState => currentState;

    private int currentUpgradeTurn = 0;
    private int maxUpgradeTurn = 3;

    private bool playerReady;
    private bool enemyReady;

    private void OnEnable()
    {
        FightManager.OnFightStateEnd += HandleFightEnded;
    }

    private void OnDisable()
    {
        FightManager.OnFightStateEnd -= HandleFightEnded;
    }

    private IEnumerator Start()
    {
        owners = OwnerManager.Instance.Owners;

        yield return null;

        StartUpgradeRound();
    }

    void Update()
    {
       upgradeCount.text = $"Turn : {currentUpgradeTurn}/{maxUpgradeTurn}";
    }

    public void NotifyOwnerUpgradeSelected(Owner owner)
    {
        if (owner == OwnerManager.Instance.PlayerOwner)
            playerReady = true;
        else
            enemyReady = true;

        CheckUpgradePhaseCompletion();
    }

    private void CheckUpgradePhaseCompletion()
    {
        if (!playerReady || !enemyReady)
            return;

        playerReady = false;
        enemyReady = false;

        currentUpgradeTurn++;

        if (currentUpgradeTurn >= maxUpgradeTurn)
        {
            StartFightPhase();
            return;
        }

        StartCoroutine(StartNextUpgradeSelection());
    }

    private IEnumerator StartNextUpgradeSelection()
    {
        yield return new WaitForSeconds(0.5f);

        OnCharUIUpdateHandle?.Invoke();

        HandleOwnersState();
    }

    // ROUND FLOW

    void StartUpgradeRound()
    {
        currentUpgradeTurn = 0;

        playerReady = false;
        enemyReady = false;

        ResetOwners();

        StartUpgradePhase();
    }

    void FightRoundEnd()
    {
        currentState = GameState.RoundEnd;

        if (owners.Exists(o => o.IsLosedLastFight))
        {
            StartBonusPhase();
        }
        else
        {
            StartUpgradeRound();
        }
    }

    // PHASES

    void StartUpgradePhase()
    {
        currentState = GameState.Upgrade;

        HandleOwnersState();
    }

    void StartBonusPhase()
    {
        currentState = GameState.Bonus;

        StartCoroutine(BonusRoutine());
    }

    void StartFightPhase()
    {
        currentState = GameState.Fight;

        FightManager.Instance.ResetFightState();

        HandleOwnersState();
    }

    // BONUS

    IEnumerator BonusRoutine()
    {
        ResetOwners();

        foreach (var owner in owners)
        {
            if (!owner.IsLosedLastFight)
                continue;

            // Bonus oynanmasýný bekle
            yield return StartCoroutine(WaitForBonusPlayed(owner));

            // 2 saniye gecikme
            yield return new WaitForSeconds(2f);
        }

        StartUpgradeRound();
    }

    private IEnumerator WaitForBonusPlayed(Owner owner)
    {
        bool bonusFinished = false;

        void OnBonusPlayed()
        {
            bonusFinished = true;
        }

        owner.OnBonusPlayed += OnBonusPlayed;

        // Upgrade state baþlat
        owner.HandleState(GameState.Upgrade);

        // Bonus bitene kadar bekle
        yield return new WaitUntil(() => bonusFinished);

        owner.OnBonusPlayed -= OnBonusPlayed;
    }

    // FIGHT END

    void HandleFightEnded()
    {
        StartCoroutine(RoundEndRoutine());
    }

    IEnumerator RoundEndRoutine()
    {
        yield return new WaitForSeconds(3f);

        FightRoundEnd();
    }

    // UTIL

    void HandleOwnersState()
    {
        foreach (var owner in owners)
        {
            owner.HandleState(currentState);
        }
    }

    void ResetOwners()
    {
        foreach (var owner in owners)
        {
            owner.Reset();
        }
    }
}

public enum GameState
{
    RoundStart,
    Upgrade,
    Bonus,
    Fight,
    RoundEnd
}
