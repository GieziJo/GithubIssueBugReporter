using System;
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
            string title = $"Automated bug report generate by {_canvasHandler.UserName} ({DateTime.UtcNow.AddHours(1).ToString("f")})";
            string body = GenerateBody(imagePath);
            GithubBugReporter.ReportBug(title, body);
            RestoreNormalGame();
        }

        private string GenerateBody(string imagePath)
        {
            string body = $"Bug submitted by: {_canvasHandler.UserName}\n\n"; 
            body += "Bug description:\n\n";
            body += _canvasHandler.Description;
            body += "\n\n";
            body += $"![BugShot]({imagePath})";
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
    }
}