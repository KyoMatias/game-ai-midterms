using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance Settings
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("No GameManager Found!");

            return _instance;
        }
    }
    #endregion


    [Header("Player")]
    [SerializeField]
    private Player _player;

    [SerializeField]
    private string _playerName;

    [SerializeField]
    private float _playerHP;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        SetupPlayer();
    }

    void SetupPlayer()
    {
        _playerName = _player.P_Data.Player_Name;
    }
}
