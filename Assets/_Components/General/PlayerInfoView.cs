
using TMPro;
using UnityEngine;

public class PlayerInfoView : MonoBehaviour
{
    PlayerInfo playerInfo;

    [SerializeField] TextMeshProUGUI gold;

    private void OnEnable()
    {
        playerInfo = GetComponent<PlayerInfo>();

        if (playerInfo != null)
            playerInfo.OnDataChanged += UpdatePlayerInfoView;

        UpdatePlayerInfoView();
    }

    private void UpdatePlayerInfoView()
    {
        gold.text = PlayerInfo.Instance.gold.ToString();
    }

}
