using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace ScreenshotUtility
{
    // =====================
    // ENUM 定義
    // =====================
    public enum TIME_STAMP
    {
        MMDDHHMMSS,
        YYYYMMDDHHMMSS,
    }

    public enum BACK_GROUND_COLOR
    {
        Alpha,
        CustomColor,
        Skybox,
    }

    public enum SCREEN_SIZE_PIXEL
    {
        p256x256,
        p512x512,
        p1024x1024,
        p2048x2048,
        p4096x4096,
        p1280x720,
        p1920x1080,
        p2560x1440,
        p3840x2160,
        CustomSize
    }

    // =====================
    // SCREENSHOT CLASS
    // =====================
    public class ScreenShot : MonoBehaviour
    {
        [SerializeField] Camera _UseCamera;
        [SerializeField] SCREEN_SIZE_PIXEL _screenSizePixel = SCREEN_SIZE_PIXEL.p1024x1024;
        [SerializeField] Vector2Int _customSize = new Vector2Int(1024, 1024);
        [SerializeField] BACK_GROUND_COLOR _buckGroundColorType = BACK_GROUND_COLOR.Alpha;
        [SerializeField] Color _customColor = Color.green;
        [SerializeField] string _screenShotsTitle = "img";
        [SerializeField] string _screenShotFolderName = "ScreenShots";
        public KeyCode _screenShotsKeybinding = KeyCode.F1;
        [SerializeField] bool _consoleLogIsActive = true;
        [SerializeField] PhotoScoreCalculator _photoScoreCalculator;


        void Update()
        {
            if (Input.GetKeyDown(_screenShotsKeybinding))
            {
                GetScreenShots();
            }
        }

        public void GetScreenShots()
        {
            if (_UseCamera == null)
            {
                Debug.LogWarning("Camera が設定されていません");
                return;
            }

            string path = Path.Combine(
                Application.persistentDataPath,
                _screenShotFolderName
            );

            StartCoroutine(ImageShooting(path, _screenShotsTitle));
        }
        
        [ContextMenu("スクリーンショットを撮影する")]
public void getScreenShots()
{
    if (NullCheck()) { return; }

    // 📸 スコア計算（シャッターと同時）
    int score = _photoScoreCalculator.CalculateScore();
    Debug.Log("写真スコア: " + score);

    string path = Application.dataPath + "/Resources/" + _screenShotFolderName + "/";
    StartCoroutine(imageShooting(path, _screenShotsTitle));
}

        private IEnumerator ImageShooting(string path, string title)
        {
            yield return new WaitForEndOfFrame();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = GetNextFileName(path, title, ".png");

            Color cacheColor = _UseCamera.backgroundColor;

            _UseCamera.clearFlags =
                _buckGroundColorType == BACK_GROUND_COLOR.Skybox
                ? CameraClearFlags.Skybox
                : CameraClearFlags.SolidColor;

            if (_buckGroundColorType == BACK_GROUND_COLOR.Alpha)
                _UseCamera.backgroundColor = new Color(0, 0, 0, 0);
            else if (_buckGroundColorType == BACK_GROUND_COLOR.CustomColor)
                _UseCamera.backgroundColor = _customColor;

            Vector2Int size = GetScreenSizePixel2Int(_screenSizePixel);

            RenderTexture rt = new RenderTexture(size.x, size.y, 32);
            Texture2D tex = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

            _UseCamera.targetTexture = rt;
            _UseCamera.Render();

            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
            tex.Apply();

            _UseCamera.targetTexture = null;
            RenderTexture.active = null;

            File.WriteAllBytes(
                Path.Combine(path, fileName),
                tex.EncodeToPNG()
            );

            Destroy(rt);
            Destroy(tex);

            _UseCamera.backgroundColor = cacheColor;

            if (_consoleLogIsActive)
            {
                Debug.Log($"Saved: {fileName}");
                Debug.Log($"Path: {path}");
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        private string GetNextFileName(string directory, string baseName, string extension)
        {
            int max = 0;
            foreach (var f in Directory.GetFiles(directory, baseName + "*" + extension))
            {
                string num = Path.GetFileNameWithoutExtension(f)
                    .Replace(baseName, "")
                    .Replace("_", "");
                if (int.TryParse(num, out int n))
                    max = Mathf.Max(max, n);
            }
            return $"{baseName}_{(max + 1):D3}{extension}";
        }

        private Vector2Int GetScreenSizePixel2Int(SCREEN_SIZE_PIXEL type)
        {
            return type switch
            {
                SCREEN_SIZE_PIXEL.p256x256 => new Vector2Int(256, 256),
                SCREEN_SIZE_PIXEL.p512x512 => new Vector2Int(512, 512),
                SCREEN_SIZE_PIXEL.p1024x1024 => new Vector2Int(1024, 1024),
                SCREEN_SIZE_PIXEL.p2048x2048 => new Vector2Int(2048, 2048),
                SCREEN_SIZE_PIXEL.p4096x4096 => new Vector2Int(4096, 4096),
                SCREEN_SIZE_PIXEL.p1280x720 => new Vector2Int(1280, 720),
                SCREEN_SIZE_PIXEL.p1920x1080 => new Vector2Int(1920, 1080),
                SCREEN_SIZE_PIXEL.p2560x1440 => new Vector2Int(2560, 1440),
                SCREEN_SIZE_PIXEL.p3840x2160 => new Vector2Int(3840, 2160),
                SCREEN_SIZE_PIXEL.CustomSize => _customSize,
                _ => new Vector2Int(1024, 1024),
            };
        }
    }
}
