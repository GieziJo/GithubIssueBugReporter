using System;
using TMPro;
using UnityEngine;

namespace Giezi.Tools
{
    public class CanvasHandler : MonoBehaviour
    {
        public event Action OnSubmitBug = delegate {  };
        public event Action OnCancelBug = delegate {  };

        public string UserName => _userName.text;
        public string Description => _description.text;

        [SerializeField] private TMP_InputField _userName;
        [SerializeField] private TMP_InputField _description;

        public void OnEnable()
        {
            _description.text = "";
            if (PlayerPrefs.HasKey("Giezi.Tools.GithubBugReporter.Username"))
                _userName.text = PlayerPrefs.GetString("Giezi.Tools.GithubBugReporter.Username");
        }

        public void SubmitBug()
        {
            PlayerPrefs.SetString("Giezi.Tools.GithubBugReporter.Username", UserName);
            OnSubmitBug();
        }
    }
}