using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace ScreenshotUtility
{
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

    public class ScreenShot : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] Camera _UseCamera;
        [SerializeField] SCREEN_SIZE_PIXEL _screenSizePixel = SCREEN_SIZE_PIXEL.p1024x1024;
        [SerializeField] Vector2Int _customSize = new Vector2Int(1024, 1024);
        [SerializeField] BACK_GROUND_COLOR _buckGroundColorType = BACK_GROUND_COLOR.Alpha;
        [SerializeField] Color _customColor = Color.green;
        [SerializeField] TIME_STAMP _timeStampStyle = TIME_STAMP.MMDDHHMMSS;
        [SerializeField] string _screenShotsTitle = "img";
        [SerializeField] string _screenShotFolderName = "ScreenShots";
        public KeyCode _screenShotsKeybinding = KeyCode.F1;
        [SerializeField] bool _consoleLogIsActive = true;

        void Update()
        {
            if (Input.GetKeyDown(_screenShotsKeybinding))
            {
                getScreenShots();
            }
        }

        private bool NullCheck()
        {
            bool isNull = false;
            if (_UseCamera == null)
            {
                Debug.LogWarning("画像の書き出しに使用するカメラを ScreenSchotCamera に割り当ててください");
                isNull = true;
            }
            if (string.IsNullOrEmpty(_screenShotsTitle))
            {
                Debug.LogWarning("ScreenShotsTitle に画像ファイルに付ける末尾の文字を入力してください");
                isNull = true;
            }
            if (string.IsNullOrEmpty(_screenShotFolderName))
            {
                Debug.LogWarning("ScreenShotFolderName にScreenShotの保存先のフォルダ名を入力してください");
                isNull = true;
            }
            return isNull;
        }

        [ContextMenu("スクリーンショットを撮影する")]
        public void getScreenShots()
        {
            if (NullCheck()) { return; }

            string path = Application.dataPath + "/Resources/" + _screenShotFolderName + "/";
            StartCoroutine(imageShooting(path, _screenShotsTitle));
        }

        private IEnumerator imageShooting(string path, string title)
        {
            yield return new WaitForEndOfFrame();

            imagePathCheck(path);

            // ここで連番ファイル名を決定する
            string fileName = GetNextFileName(path, title, ".png");

            // 元の背景色を保持
            Color32 CacheColor = _UseCamera.backgroundColor;

            if (_buckGroundColorType == BACK_GROUND_COLOR.Alpha)
            {
                _UseCamera.backgroundColor = new Color32(0, 0, 0, 0);
                _UseCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            }
            else if (_buckGroundColorType == BACK_GROUND_COLOR.CustomColor)
            {
                _UseCamera.backgroundColor = _customColor;
                _UseCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            }
            else if (_buckGroundColorType == BACK_GROUND_COLOR.Skybox)
            {
                _UseCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            }

            Vector2Int size = getScreenSizePixel2Int(_screenSizePixel);
            Texture2D screenShot = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);
            RenderTexture rt = new RenderTexture(screenShot.width, screenShot.height, 32);
            RenderTexture prev = _UseCamera.targetTexture;
            _UseCamera.targetTexture = rt;
            _UseCamera.Render();
            _UseCamera.targetTexture = prev;
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
            screenShot.Apply();
            byte[] bytes = screenShot.EncodeToPNG();

            File.WriteAllBytes(path + fileName, bytes);

            if (_consoleLogIsActive)
            {
                Debug.Log("Title: " + fileName);
                Debug.Log("Directory: " + path);
            }

            _UseCamera.backgroundColor = CacheColor;

            UnityEditor.AssetDatabase.Refresh();
        }

        private void imagePathCheck(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log("CreateFolder: " + path);
            }
        }

        private string GetNextFileName(string directory, string baseName, string extension)
        {
            int maxNumber = 0;
            string[] files = Directory.GetFiles(directory, baseName + "*" + extension);
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                // 例: img_001 → 001 部分だけ抽出
                string numberPart = fileName.Replace(baseName, "").Replace("_", "");
                if (int.TryParse(numberPart, out int number))
                {
                    if (number > maxNumber) maxNumber = number;
                }
            }
            // 次の番号を作る（3桁ゼロパディング）
            int nextNumber = maxNumber + 1;
            return baseName + "_" + nextNumber.ToString("D3") + extension;
        }

        private Vector2Int getScreenSizePixel2Int(SCREEN_SIZE_PIXEL ScreenSize)
        {
            Vector2Int size = new Vector2Int(1024, 1024);
            switch (ScreenSize)
            {
                case SCREEN_SIZE_PIXEL.p256x256:
                    size = new Vector2Int(256, 256);
                    break;
                case SCREEN_SIZE_PIXEL.p512x512:
                    size = new Vector2Int(512, 512);
                    break;
                case SCREEN_SIZE_PIXEL.p1024x1024:
                    size = new Vector2Int(1024, 1024);
                    break;
                case SCREEN_SIZE_PIXEL.p2048x2048:
                    size = new Vector2Int(2048, 2048);
                    break;
                case SCREEN_SIZE_PIXEL.p4096x4096:
                    size = new Vector2Int(4096, 4096);
                    break;
                case SCREEN_SIZE_PIXEL.p1280x720:
                    size = new Vector2Int(1280, 720);
                    break;
                case SCREEN_SIZE_PIXEL.p1920x1080:
                    size = new Vector2Int(1920, 1080);
                    break;
                case SCREEN_SIZE_PIXEL.p2560x1440:
                    size = new Vector2Int(2560, 1440);
                    break;
                case SCREEN_SIZE_PIXEL.p3840x2160:
                    size = new Vector2Int(3840, 2160);
                    break;
                case SCREEN_SIZE_PIXEL.CustomSize:
                    if (_customSize.x > 4096) _customSize.x = 4096;
                    if (_customSize.y > 4096) _customSize.y = 4096;
                    if (_customSize.x < 32) _customSize.x = 32;
                    if (_customSize.y < 32) _customSize.y = 32;
                    size = _customSize;
                    break;
            }
            return size;
        }

        private string getTimeStamp(TIME_STAMP type)
        {
            switch (type)
            {
                case TIME_STAMP.MMDDHHMMSS:
                    return DateTime.Now.ToString("MMddHHmmss");
                case TIME_STAMP.YYYYMMDDHHMMSS:
                    return DateTime.Now.ToString("yyyyMMddHHmmss");
                default:
                    return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }
#endif
    }
}
