using System;
using UnityEngine;

public class UpgradeCardController
{
    public static event Action<UpgradeCardData, Owner> OnCardSelected;

    private UpgradeCardData model;
    private UpgradeCardView view;
    private Owner owner;

    public UpgradeCardController(UpgradeCardData model, UpgradeCardView view, Owner owner)
    {
        this.model = model;
        this.view = view;
        this.owner = owner;

        this.view.OnViewClicked += HandleViewClicked;
    }

    public void InitializeCard()
    {
        if (model == null || view == null) return;

        view.SetCharacterInfo(model.charName, model.charImage);

        ProcessUpgradeType();
        ProcessCardColor();
    }

    private void ProcessUpgradeType()
    {
        switch (model.upgradeType)
        {
            case UpgradeType.Doubler:
                view.SetUpgradeIcons(true, false);
                view.SetUpgradeText($"x2 {model.charData.charName}");
                break;
            case UpgradeType.Upgrader:
                view.SetUpgradeIcons(false, true);
                view.SetUpgradeText("Upgrade");
                break;
            case UpgradeType.Spawner:
                view.SetUpgradeIcons(false, false);
                view.SetUpgradeText($"+{model.charData.spawnCount} {model.charData.charName}");
                break;
            default:
                Debug.LogWarning("Unknown upgrade type.");
                break;
        }
    }

    private void ProcessCardColor()
    {
        Color cardColor = Color.white;

        switch (model.charData.charName)
        {
            case "Blup":
                cardColor = new Color(0.5647f, 0.6392f, 0.7294f);
                break;
            case "Dino":
                cardColor = new Color(1.0f, 0.9608f, 0.1333f);
                break;
            case "Demon":
                cardColor = new Color(0.0235f, 0.6745f, 0.9961f);
                break;
        }

        view.SetBackgroundColor(cardColor);
    }

    private void HandleViewClicked()
    {
        // Trigger global static events
        OnCardSelected?.Invoke(model, owner);
    }
}