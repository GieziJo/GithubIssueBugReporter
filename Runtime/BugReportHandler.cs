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
        private string logMessage;
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
        
        
        void OnEnable() { Application.logMessageReceived += Log;  }

        void OnDisable() { Application.logMessageReceived -= Log; }

        private void Log(string logString, string stacktrace, LogType type)
        {
            logMessage += logString + "\n";
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

            logFileEntry += logMessage;
            
            logFileEntry += "\n</details>";
            return logFileEntry;
        }
    }
}