using System.Collections.Generic;

public class RandomStrategy : IUpgradeDecisionStrategy
{
    public UpgradeCardData ChooseUpgrade(List<UpgradeCardData> options, AIOwner owner)
    {
        return options[UnityEngine.Random.Range(0, options.Count)];
    }
}