using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Giezi.Tools
{
    public class BugReportHandler : MonoBehaviour
    {
        [SerializeField] private InputsListener _inputsListener;
        [SerializeField] private ScreenshotHandler _screenshotHandler;
        [SerializeField] private GameObject canvas;
        private byte[] screenshot;
        private CanvasHandler _canvasHandler;

        public static BugReportHandler Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this.gameObject);
                return;
            }
            
            DontDestroyOnLoad(this.gameObject);
            _inputsListener.ReportBugNow += ReportBug;
        }

        private void OnDestroy()
        {
            _inputsListener.ReportBugNow -= ReportBug;
        }

        private void ReportBug()
        {
            Time.timeScale = 0f;
            _screenshotHandler.OnScreenshotTakingDone += ScreenShotResult;
            _screenshotHandler.TakeScreenShot();
        }

        private void ScreenShotResult(byte[] screenshot)
        {
            _screenshotHandler.OnScreenshotTakingDone -= ScreenShotResult;
            this.screenshot = screenshot;
            canvas.SetActive(true);
            _canvasHandler ??= canvas.GetComponent<CanvasHandler>();
            _canvasHandler.OnCancelBug += OnCancelBug;
            _canvasHandler.OnSubmitBug += OnSubmitBug;
        }

        private void OnSubmitBug()
        {
            string imagePath = UploadImage.UploadImageToGithub(screenshot);
            string title = $"[Automated Bug Report] {_canvasHandler.Title} ({_canvasHandler.UserName} - {DateTime.UtcNow.AddHours(1).ToString("f")})";
            string body = GenerateBody(imagePath);
            GithubBugReporter.ReportBug(title, body);
            RestoreNormalGame();
        }

        private string GenerateBody(string imagePath)
        {
            string body = "";
            body += $"Bug submitted by: ";
            body += _canvasHandler.GithubToggle ? $"@{_canvasHandler.GithubUsername}" : $"{_canvasHandler.UserName}";
            body += "\n\n";
            body += "## Bug description\n\n";
            body += _canvasHandler.Description;
            body += "\n\n";
            body += $"![BugShot]({imagePath})";
            body += "\n\n";
            if(_canvasHandler.PlayerLogToggle)
                body += AppendLogFile();
            return body;
        }

        private void OnCancelBug()
        {
            RestoreNormalGame();
        }

        private void RestoreNormalGame()
        {
            _canvasHandler.OnCancelBug -= OnCancelBug;
            _canvasHandler.OnSubmitBug -= OnSubmitBug;
            canvas.SetActive(false);
            Time.timeScale = 1f;
        }
        
        

        private string AppendLogFile()
        {
            string logFileEntry = "";
            logFileEntry += "<details>";
            logFileEntry += "<summary>Log File</summary>\n\n";

            string log_path;
#if UNITY_STANDALONE_WIN
                // %USERPROFILE%\AppData\LocalLow\CompanyName\ProductName\Player.log
            log_path = CombinePaths(Environment.GetEnvironmentVariable("AppData"), "..", "LocalLow", Application.companyName, Application.productName, "Player.log");
#elif UNITY_STANDALONE_LINUX
            log_path = CombinePaths("~/.config/unity3d", Application.companyName, Application.productName, "Player.log");
#elif UNITY_STANDALONE_OSX
            log_path = "~/Library/Logs/Unity/Player.log";
#else
            return logFileEntry + "\n</details>";
#endif
            
            logFileEntry += "\n" + System.IO.File.ReadAllText(log_path);
            
            logFileEntry += "\n</details>";
            return logFileEntry;
        }
    
        string CombinePaths(params string[] paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }
            return paths.Aggregate(Path.Combine);
        }
    }
}