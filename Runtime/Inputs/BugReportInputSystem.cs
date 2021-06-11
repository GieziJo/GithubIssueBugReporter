// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Inputs/BugReportInputSystem.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Giezi.Tools
{
    public class @BugReportInputSystem : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @BugReportInputSystem()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""BugReportInputSystem"",
    ""maps"": [
        {
            ""name"": ""ReportBug"",
            ""id"": ""727ef52e-a87f-4880-a92e-30d8186622e1"",
            ""actions"": [
                {
                    ""name"": ""ReportBug"",
                    ""type"": ""Button"",
                    ""id"": ""eb5c2e55-e649-4233-953e-35b0a0669e29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cfe0d88e-0428-4a34-a9ed-295235c486a5"",
                    ""path"": ""<Keyboard>/f12"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ReportBug"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // ReportBug
            m_ReportBug = asset.FindActionMap("ReportBug", throwIfNotFound: true);
            m_ReportBug_ReportBug = m_ReportBug.FindAction("ReportBug", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // ReportBug
        private readonly InputActionMap m_ReportBug;
        private IReportBugActions m_ReportBugActionsCallbackInterface;
        private readonly InputAction m_ReportBug_ReportBug;
        public struct ReportBugActions
        {
            private @BugReportInputSystem m_Wrapper;
            public ReportBugActions(@BugReportInputSystem wrapper) { m_Wrapper = wrapper; }
            public InputAction @ReportBug => m_Wrapper.m_ReportBug_ReportBug;
            public InputActionMap Get() { return m_Wrapper.m_ReportBug; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ReportBugActions set) { return set.Get(); }
            public void SetCallbacks(IReportBugActions instance)
            {
                if (m_Wrapper.m_ReportBugActionsCallbackInterface != null)
                {
                    @ReportBug.started -= m_Wrapper.m_ReportBugActionsCallbackInterface.OnReportBug;
                    @ReportBug.performed -= m_Wrapper.m_ReportBugActionsCallbackInterface.OnReportBug;
                    @ReportBug.canceled -= m_Wrapper.m_ReportBugActionsCallbackInterface.OnReportBug;
                }
                m_Wrapper.m_ReportBugActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ReportBug.started += instance.OnReportBug;
                    @ReportBug.performed += instance.OnReportBug;
                    @ReportBug.canceled += instance.OnReportBug;
                }
            }
        }
        public ReportBugActions @ReportBug => new ReportBugActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface IReportBugActions
        {
            void OnReportBug(InputAction.CallbackContext context);
        }
    }
}
