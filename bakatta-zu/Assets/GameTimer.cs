using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [Header("制限時間（秒）")]
    [SerializeField] private float _timeLimit = 60f;

    [Header("遷移先シーン名")]
    [SerializeField] private string _resultSceneName = "ResultScene";

    private float _currentTime;
    private bool _isFinished = false;

    void Start()
    {
        _currentTime = _timeLimit;
    }

    void Update()
    {
        if (_isFinished) return;

        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0f)
        {
            _currentTime = 0f;
            FinishGame();
        }
    }

    private void FinishGame()
    {
        _isFinished = true;

        // 必要ならここでスコア最終確定処理を書ける

        SceneManager.LoadScene(_resultSceneName);
    }

    public float GetRemainingTime()
    {
        return _currentTime;
    }
}
