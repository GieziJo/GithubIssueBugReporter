using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Giezi.Tools
{
    public class InputsListener : MonoBehaviour
    {
        public event Action ReportBugNow = delegate {  };
        private void Awake()
        {
            BugReportInputSystem bugReportInputSystem = new BugReportInputSystem();
            bugReportInputSystem.ReportBug.Enable();
            bugReportInputSystem.ReportBug.ReportBug.performed += ReportBug;
        }

        private void ReportBug(InputAction.CallbackContext obj)
        {
            ReportBugNow();
        }
    }
}