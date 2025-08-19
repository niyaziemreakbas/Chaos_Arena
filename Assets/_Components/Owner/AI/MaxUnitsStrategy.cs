using System.Collections.Generic;

public class MaxUnitsStrategy : IUpgradeDecisionStrategy
{
    public UpgradeCardData ChooseUpgrade(List<UpgradeCardData> options, AIOwner owner)
    {
        foreach (var option in options)
        {
            if (option.upgradeType == UpgradeType.Doubler)
                return option;
        }
        foreach (var option in options)
        {
            if (option.upgradeType == UpgradeType.Spawner)
                return option;
        }
        // Eðer Double yoksa rastgele bir þey seç
        return options[UnityEngine.Random.Range(0, options.Count)];
    }
}
