using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRandomUpgrade : IUpgradeSelector
{
    public IEnumerator Select(Owner owner, List<UpgradeCardData> cards, Action<UpgradeCardData> onSelected)
    {
        if (cards == null || cards.Count == 0) { onSelected?.Invoke(null); yield break; }

        int idx = UnityEngine.Random.Range(0, cards.Count);
        onSelected?.Invoke(cards[idx]);
        yield return null;
    }
}
