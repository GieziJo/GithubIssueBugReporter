using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Giezi.Tools
{
    public class BugReportHandler : MonoBehaviour
    {
        [SerializeField] private InputsListener _inputsListener;
        [SerializeField] private ScreenshotHandler _screenshotHandler;
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject bugReportedCanvas;
        private byte[] screenshot;
        private CanvasHandler _canvasHandler;

        private string logMessage;
        private bool onErrorPopup;
        private string onErrorPopupMsg;
        
        public static BugReportHandler Instance;
        private bool handleInBackground = false;

        private float _previousTimeScale = 1f;
        private CursorLockMode _previousMouseLock = CursorLockMode.None;
        private bool _previousMouseVisibility = true;
        [SerializeField] private EventSystem _thisEventSystem;
        private Dictionary<EventSystem, bool> _eventSystemDictionary;

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
            logMessage += logString + stacktrace + "\n";
            
#if !GIEZI_TOOLS_DISABLE_ON_ERROR_POPUP
            if(type == LogType.Error && PlayerPrefs.GetInt("Giezi.Tools.GithubBugReporter.PopupOnError", 1) == 1)
            {
                onErrorPopup = true;
                onErrorPopupMsg = logString;
                handleInBackground = PlayerPrefs.GetInt("Giezi.Tools.GithubBugReporter.ErrorPopupInBackground", 0) == 1;
                ReportBug();
            }
#endif
        }

        private void ReportBug()
        {
            _inputsListener.ReportBugNow -= ReportBug;
            SetupGameForBugReport();
            _screenshotHandler.OnScreenshotTakingDone += ScreenShotResult;
            _screenshotHandler.TakeScreenShot();
        }

        private void ScreenShotResult(byte[] screenshot)
        {
            _screenshotHandler.OnScreenshotTakingDone -= ScreenShotResult;
            this.screenshot = screenshot;
            
            if (onErrorPopup && handleInBackground)
            {
                OnSubmitBug();
                return;
            }

            canvas.SetActive(true);
            _canvasHandler ??= canvas.GetComponent<CanvasHandler>();
            _canvasHandler.OnCancelBug += OnCancelBug;
            _canvasHandler.OnSubmitBug += OnSubmitBug;
            if (onErrorPopup)
                _canvasHandler.EnablePopupInfo();
        }

        private void OnSubmitBug()
        {
            string imagePath = UploadImage.UploadImageToGithub(screenshot);
            string title = GetTitle();
            string body = GenerateBody(imagePath);
            GithubBugReporter.ReportBug(title, body, Application.version, Application.platform.ToString(), onErrorPopup);
            RestoreNormalGame();
        }

        private string GetTitle()
        {
            if (onErrorPopup && handleInBackground)
                return $"[Automated Bug Report in  Background] ({DateTime.UtcNow.AddHours(1).ToString("f")})";
            return $"[Automated Bug Report] {_canvasHandler.Title} ({_canvasHandler.UserName} - {DateTime.UtcNow.AddHours(1).ToString("f")})";
        }

        private string GenerateBody(string imagePath)
        {
            string body = "";
            if (onErrorPopup && handleInBackground)
            {
                body += "## Background bug report\n\n";
                body += "\n\n";
                body += $"App Version {Application.version}\n\n";
                body += "\n\n";
                body += "## Error Message:\n\n";
                body += $"{onErrorPopupMsg}\n\n";
                body += "\n\n";
                body += "\n\n";
                body += $"![BugShot]({imagePath})";
                body += "\n\n";
                body += AppendLogFile();
                
                return body;
            }
            
            body += $"Bug submitted by: ";
            body += _canvasHandler.GithubToggle ? $"@{_canvasHandler.GithubUsername}" : $"{_canvasHandler.UserName}";
            body += "\n\n";
            body += $"App Version {Application.version}\n\n";
            body += "\n\n";
            
            if (onErrorPopup)
            {
                body += "## Error Message:\n\n";
                body += $"{onErrorPopupMsg}\n\n";
                body += "\n\n";
            }

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
        
        
        private void SetupGameForBugReport()
        {
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            _previousMouseLock = Cursor.lockState;
            _previousMouseVisibility = Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            List<EventSystem> eventSystems = FindObjectsOfType<EventSystem>().ToList();
            eventSystems.Remove(_thisEventSystem);
            _eventSystemDictionary = eventSystems.ToDictionary(x => x, x => x.sendNavigationEvents);
            eventSystems.ForEach(system => system.sendNavigationEvents = false);
        }

        private void RestoreNormalGame()
        {
            if (onErrorPopup && handleInBackground)
                bugReportedCanvas.SetActive(true);
            else
            {
                _canvasHandler.OnCancelBug -= OnCancelBug;
                _canvasHandler.OnSubmitBug -= OnSubmitBug;
                canvas.SetActive(false);
            }

            _inputsListener.ReportBugNow += ReportBug;
            
            
            onErrorPopup = false;
            handleInBackground = false;
            
            foreach (KeyValuePair<EventSystem,bool> eventSystemPair in _eventSystemDictionary)
                eventSystemPair.Key.sendNavigationEvents = eventSystemPair.Value;
            
            Time.timeScale = _previousTimeScale;
            
            Cursor.lockState = _previousMouseLock;
            Cursor.visible = _previousMouseVisibility;
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