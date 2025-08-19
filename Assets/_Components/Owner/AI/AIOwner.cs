using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIOwner : Owner
{
    [SerializeField] private TextMeshProUGUI thinkingText;

    // Think Delay
    private float minDelay = 3.0f;
    private float maxDelay = 5.0f;

    IUpgradeDecisionStrategy upgradeDecisionStrategy;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        unitRegistry.SelectedCharacters = DataManager.Instance.RandomlySelectDeck();
        upgradeDecisionStrategy = OwnerManager.Instance.ReturnRandomStrategy();
        isUpward = false; // Assuming AI is always downward for now
    }

    protected override void HandleUpgradeState()
    {
        print("AI Owner handling upgrade state.");
        StartCoroutine(DelayedUpgradeRoutine());
    }

    private IEnumerator DelayedUpgradeRoutine()
    {
        float waitTime = UnityEngine.Random.Range(minDelay, maxDelay);

        Coroutine thinkingAnim = StartCoroutine(ShowThinkingText(waitTime));

        yield return new WaitForSeconds(waitTime);

        StopCoroutine(thinkingAnim);
        thinkingText.text = "";

        List<UpgradeCardData> availableUpgrades = CardUpgradeManager.Instance.ReturnRandomUpgradeList(this, 4);

        if (CardUpgradeManager.Instance.HandleCardUpgrades(upgradeDecisionStrategy.ChooseUpgrade(availableUpgrades, this), this))
        {
            OnUpgradePerformedFunction();
        }
    }

    private IEnumerator ShowThinkingText(float duration)
    {
        float timer = 0f;
        int dotCount = 0;

        while (timer < duration)
        {
            dotCount = (dotCount + 1) % 4; // 0,1,2,3
            thinkingText.text = "Choosing" + new string('.', dotCount);

            yield return new WaitForSeconds(0.3f);
            timer += 0.3f;
        }
    }
}
