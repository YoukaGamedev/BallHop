using UnityEngine;

public class LeaderboardButtonGINVO : MonoBehaviour
{
    public void ShowLeaderboard()
    {
        ScoreManagerGINVO.Instance.ShowLeaderboard();
    }
}