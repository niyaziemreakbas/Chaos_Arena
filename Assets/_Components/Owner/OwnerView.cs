using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OwnerView : MonoBehaviour
{
    Owner owner;

    [SerializeField] TextMeshProUGUI UpgradeCount;
    [SerializeField] TextMeshProUGUI HealthCount;
    [SerializeField] TextMeshProUGUI OwnerName;

    [SerializeField] GameObject BonusRound;

    private void OnEnable()
    {
        owner = GetComponent<Owner>();
        if (owner != null)
            owner.OnDataChanged += UpdateOwnerView;

        UpdateOwnerView();
    }

    private void OnDisable()
    {
        if (owner != null)
            owner.OnDataChanged -= UpdateOwnerView;
    }


    private void Start()
    {
       owner = GetComponent<Owner>();
    }

    public void UpdateOwnerView()
    {
        if (owner == null)
        {
            Debug.LogWarning("Owner is not assigned in OwnerView!");
            return;
        }

        HealthCount.text = owner.GameHealth.ToString();
        UpgradeCount.text = owner.UpgradeCount.ToString();
        OwnerName.text = owner.OwnerName;
 
        OwnerName.color = owner.teamColor;
        UpgradeCount.color = owner.teamColor;

        print("owner view called " + OwnerName.text);
        if (owner.IsLosedLastFight)
        {
            print("activating");
            BonusRound.SetActive(true);
        }
        else
        {
            print("deactivating");
            BonusRound.SetActive(false);
        }
    }
}
