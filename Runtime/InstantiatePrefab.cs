using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Giezi.Tools
{
    [CreateAssetMenu]
    public class InstantiatePrefab : ScriptableObject
    {
        [SerializeField] private GameObject bugReportAsset;

        private void OnEnable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if(BugReportHandler.Instance == null)
                Instantiate(bugReportAsset);
        }
    }
}