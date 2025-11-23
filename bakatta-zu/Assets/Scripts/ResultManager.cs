using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timeText;

    public void ShowResult(int score, float time)
    {
        resultPanel.SetActive(true);

        scoreText.text = "Score: " + score;
        timeText.text = "Time: " + time.ToString("F2") + "s";

        // ゲームを止めたい場合
        Time.timeScale = 0f;
    }

    public void OnRetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnTitleButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
}