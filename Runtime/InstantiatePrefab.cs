using UnityEngine;

namespace Giezi.Tools
{
    public class InstantiatePrefab
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void InitialisePrefab()
        {
            if (BugReportHandler.Instance == null)
            {
                GameObject bugReportAsset = Resources.Load<GameObject>("BugReporter");
                Object.Instantiate(bugReportAsset);
            }
        }
    }
}