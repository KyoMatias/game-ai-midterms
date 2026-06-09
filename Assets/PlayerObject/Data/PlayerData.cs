[System.Serializable]
public struct PlayerData
{
    public string Player_Name;
    public float Player_HP;
    public PlayerState Player_State;

    public PlayerData(string player_name, float player_hp)
    {
        Player_Name = player_name;
        Player_HP = player_hp;
        Player_State = PlayerState.IDLE;
    }
}

public enum PlayerState
{
    IDLE,
    MOVING,
    DEAD,
}
