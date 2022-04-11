using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Giezi.Tools
{
    public class CanvasHandler : MonoBehaviour
    {
        public event Action OnSubmitBug = delegate {  };
        public event Action OnCancelBug = delegate {  };

        public string UserName => _userName.text;
        public string Description => _description.text;

        public bool GithubToggle => _githubToggle.isOn;
        public bool PlayerLogToggle => _playerLogToggle.isOn;

        public string GithubUsername => _githubUsername.text;
        
        public string Title => _title.text;

        [SerializeField] private TMP_InputField _userName;
        [SerializeField] private TMP_InputField _githubUsername;
        [SerializeField] private GameObject _githubUserObject;
        [SerializeField] private Toggle _githubToggle;
        [SerializeField] private Toggle _playerLogToggle;
        [SerializeField] private TMP_InputField _description;
        [SerializeField] private TMP_InputField _title;

        public void OnEnable()
        {
            _description.text = "";
            _title.text = "";
            
            if (PlayerPrefs.HasKey("Giezi.Tools.GithubBugReporter.Username"))
                _userName.text = PlayerPrefs.GetString("Giezi.Tools.GithubBugReporter.Username");
            if (PlayerPrefs.HasKey("Giezi.Tools.GithubBugReporter.GithubUsername"))
                _userName.text = PlayerPrefs.GetString("Giezi.Tools.GithubBugReporter.GithubUsername");
            if (PlayerPrefs.HasKey("Giezi.Tools.GithubBugReporter.githubToggle"))
                _githubToggle.isOn = PlayerPrefs.GetInt("Giezi.Tools.GithubBugReporter.githubToggle") == 1;
            if (PlayerPrefs.HasKey("Giezi.Tools.GithubBugReporter.playerLogToggle"))
                _playerLogToggle.isOn = PlayerPrefs.GetInt("Giezi.Tools.GithubBugReporter.playerLogToggle") == 1;
            
            ToggleState();
            _githubToggle.onValueChanged.AddListener(delegate { ToggleState(); });
            
        }

        public void OnDisable() => _githubToggle.onValueChanged.RemoveAllListeners();

        private void ToggleState() => _githubUserObject.SetActive(_githubToggle.isOn);

        public void SubmitBug()
        {
            SaveInfos();
            OnSubmitBug();
        }

        private void SaveInfos()
        {
            PlayerPrefs.SetString("Giezi.Tools.GithubBugReporter.Username", UserName);
            PlayerPrefs.SetString("Giezi.Tools.GithubBugReporter.GithubUsername", GithubUsername);
            PlayerPrefs.SetInt("Giezi.Tools.GithubBugReporter.githubToggle", GithubToggle ? 1 : 0);
            PlayerPrefs.SetInt("Giezi.Tools.GithubBugReporter.playerLogToggle", PlayerLogToggle ? 1 : 0);
        }

        public void CancelBug()
        {
            SaveInfos();
            OnCancelBug();
        }
    }
}