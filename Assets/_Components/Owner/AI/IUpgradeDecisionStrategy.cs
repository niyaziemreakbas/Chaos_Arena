using System.Collections.Generic;

public interface IUpgradeDecisionStrategy
{
    UpgradeCardData ChooseUpgrade(List<UpgradeCardData> options, AIOwner owner);
}
