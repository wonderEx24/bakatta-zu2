using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    private CursorLockMode prevLockMode;
    private bool prevVisible;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// UI表示用にカーソルを解放
    /// </summary>
    public void LockForUI()
    {
        prevLockMode = Cursor.lockState;
        prevVisible = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// UI表示前の状態に戻す
    /// </summary>
    public void Restore()
    {
        Cursor.lockState = prevLockMode;
        Cursor.visible = prevVisible;
    }
}
