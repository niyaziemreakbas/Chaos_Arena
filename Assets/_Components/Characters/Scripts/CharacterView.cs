using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    private Character character;

    public void Initialize(Character character)
    {
        this.character = character;

        UpdateView();
    }

    [SerializeField] GameObject teamColor;

    [SerializeField] GameObject levelStars;

    public void UpdateView()
    {
        teamColor.GetComponent<SpriteRenderer>().color = character.TeamOwner.teamColor;
        ShowLevelStars(character.CharacterData.charLevel);
    }

    private void ShowLevelStars(int i)
    {
        if (levelStars == null)
        {
            Debug.LogError("Level stars GameObject is not assigned.");
            return;
        }
        for (int j = 0; j < levelStars.transform.childCount; j++)
        {
            levelStars.transform.GetChild(j).gameObject.SetActive(j < i);
        }
    }
}
