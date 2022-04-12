using System;
using UnityEngine;

namespace GithubIssueBugReporter.Runtime
{
    public class DisableAfterTime : MonoBehaviour
    {
        private void OnEnable()
        {
            Invoke(nameof(DisableAfterMessageAppears), 3f);
        }

        void DisableAfterMessageAppears()
        {
            this.gameObject.SetActive(false);
        }
    }
}
