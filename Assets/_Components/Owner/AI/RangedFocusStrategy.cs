using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedFocusStrategy : IUpgradeDecisionStrategy
{
    public UpgradeCardData ChooseUpgrade(List<UpgradeCardData> options, AIOwner owner)
    {
        foreach (var option in options)
        {
            if (option.charName == "Blup")
                return option;
        }
        foreach (var option in options)
        {
            if (option.upgradeType == UpgradeType.Doubler || option.upgradeType == UpgradeType.Upgrader)
                return option;
        }

        return options[UnityEngine.Random.Range(0, options.Count)];
    }
}
