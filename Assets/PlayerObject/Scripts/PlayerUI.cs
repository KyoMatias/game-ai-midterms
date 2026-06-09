using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _playerNameLabel;

    void Awake()
    {
        UpdateName();
    }

    void UpdateName()
    {
        SetPlayerName("DEFAULTSTRING");
    }

    public void SetPlayerName(string name)
    {
        _playerNameLabel.text = name;
    }
}
