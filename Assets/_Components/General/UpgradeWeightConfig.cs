using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeWeightConfig", menuName = "Configs/Upgrade Weight Config")]
public class UpgradeWeightConfig : ScriptableObject
{
    [System.Serializable]
    public class UpgradeWeight
    {
        public UpgradeType type;
        public float weight = 1f;
    }

    public UpgradeWeight[] upgradeWeights;

    public float GetWeight(UpgradeType type)
    {
        foreach (var uw in upgradeWeights)
        {
            if (uw.type == type)
                return uw.weight;
        }
        return 0f;
    }
}
