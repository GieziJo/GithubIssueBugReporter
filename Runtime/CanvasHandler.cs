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

        public string GithubUsername => _githubUsername.text;
        
        public string Title => _title.text;

        [SerializeField] private TMP_InputField _userName;
        [SerializeField] private TMP_InputField _githubUsername;
        [SerializeField] private Toggle _githubToggle;
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
        }

        public void SubmitBug()
        {
            PlayerPrefs.SetString("Giezi.Tools.GithubBugReporter.Username", UserName);
            PlayerPrefs.SetString("Giezi.Tools.GithubBugReporter.GithubUsername", GithubUsername);
            PlayerPrefs.SetInt("Giezi.Tools.GithubBugReporter.githubToggle", GithubToggle ? 1 : 0);
            OnSubmitBug();
        }
    }
}