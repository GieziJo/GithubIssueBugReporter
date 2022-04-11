using UnityEngine;

namespace Giezi.Tools
{
    public class InstantiatePrefab
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnSceneLoaded()
        {
            if (BugReportHandler.Instance == null)
                MonoBehaviour.Instantiate(Resources.Load("BugReporter"));
        }
    }
}