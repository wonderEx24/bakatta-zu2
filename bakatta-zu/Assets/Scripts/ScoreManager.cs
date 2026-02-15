using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int TotalScore { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        TotalScore = 0;
    }

    public void AddScore(int score)
    {
        TotalScore += score;
        Debug.Log("現在の合計スコア: " + TotalScore);
    }

    public void ResetScore()
    {
        TotalScore = 0;
    }
}
