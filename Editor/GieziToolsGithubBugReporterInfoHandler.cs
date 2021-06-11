using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class GieziToolsGithubBugReporterInfoHandler : EditorWindow
{
    private TextField _imagesRepoTokenTextField;
    private TextField _issuesRepoTokenTextField;

    private TextField _usernameTextField;
    private TextField _poductHeaderTextField;
    private TextField _issuesRepoTextField;
    private TextField _issueLabelTextField;
    private TextField _imageRepoTextField;
    private TextField _screenshotsBranchTextField;
    private Button _generateJsonButton;

    private bool state = false;
    private VisualElement _visualElement;
    private const string BUG_REPORTER_DEFINE = "GIEZI_TOOLS_ENABLE_BUG_REPORTER";


    private const string JSON_FILES_PATH = "Assets/Resources/GieziTools.GithubIssueBugReporter/";

    [MenuItem("Tools/GieziTools/Github Bug Reporter")]
    public static void HandleInfos()
    {
        GieziToolsGithubBugReporterInfoHandler wnd = GetWindow<GieziToolsGithubBugReporterInfoHandler>();
        wnd.maxSize = new Vector2(600f, 270f);
        wnd.minSize = new Vector2(600f, 270f);
        wnd.titleContent = new GUIContent("Github Bug Reporter Info Setup");
    }

    public void CreateGUI()
    {
        try
        {
            _visualElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/ch.giezi.tools.githubissuebugreporter/Editor/GieziToolsGithubBugReporterInfoHandler.uxml").Instantiate();
        }
        catch
        {
            _visualElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/GieziToolsGithubBugReporterInfoHandler.uxml").Instantiate();            
        }
        rootVisualElement.Add(_visualElement);
        _issuesRepoTokenTextField = _visualElement.Q<TextField>("IssuesRepoToken");
        _imagesRepoTokenTextField = _visualElement.Q<TextField>("ImageRepoToken");
        
        _usernameTextField = _visualElement.Q<TextField>("Username");
        _poductHeaderTextField = _visualElement.Q<TextField>("ProductHeader");
        _issuesRepoTextField = _visualElement.Q<TextField>("IssuesRepo");
        _issueLabelTextField = _visualElement.Q<TextField>("IssueLabel");
        _imageRepoTextField = _visualElement.Q<TextField>("ImageRepo");
        _screenshotsBranchTextField = _visualElement.Q<TextField>("ScreenshotsBranch");

        if (File.Exists($"{JSON_FILES_PATH}GithubAccessTokens.json"))
        {
            Dictionary<string, string> tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>("GieziTools.GithubIssueBugReporter/GithubAccessTokens").ToString());
            _issuesRepoTokenTextField.value = tokens["github-repo-bug-tracker-token"];
            _imagesRepoTokenTextField.value = tokens["github-repo-bug-images-token"];
        }

        if (File.Exists($"{JSON_FILES_PATH}GithubInfos.json"))
        {
            Dictionary<string, string> tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>("GieziTools.GithubIssueBugReporter/GithubInfos").ToString());
            _usernameTextField.value = tokens["github-username"];
            _poductHeaderTextField.value = tokens["product-header"];
            _issuesRepoTextField.value = tokens["issues-repo"];
            _issueLabelTextField.value = tokens["custom-label"];
            _imageRepoTextField.value = tokens["screenshots-repo"];
            _screenshotsBranchTextField.value = tokens["screenshots-branch"];
            
        }

        _generateJsonButton = _visualElement.Q<Button>("GenerateJson");
        _generateJsonButton.clicked += () => GenerateJson(_visualElement.Q<Label>("ConfirmationText"));

        SetupChangeOfStatus();
    }

    private void SetupChangeOfStatus()
    {
        CheckState();
        _visualElement.Q<Button>("activeBugReporter").clicked += OnStateChangedClicked;
        UpdateState();
    }

    private void CheckState()
    {
        state = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';').ToList().Contains(BUG_REPORTER_DEFINE);
    }

    private void OnStateChangedClicked()
    {
        state = !state;
        UpdateState();
        UpdateDefines();
    }

    private void UpdateState()
    {
        _visualElement.Q<Label>("BurgReporterStatus").text = "Status: " + (state ? "Enabled" : "Disabled");
        _visualElement.Q<Button>("activeBugReporter").text = (state ? "Disable" : "Enable") + " Bug Reporter";
        _visualElement.Q<VisualElement>("InfoHolder").style.opacity = state ? 1f : .5f;

        _issuesRepoTokenTextField.isReadOnly = !state;
        _imagesRepoTokenTextField.isReadOnly = !state;
        
        _usernameTextField.isReadOnly = !state;
        _poductHeaderTextField.isReadOnly = !state;
        _issuesRepoTextField.isReadOnly = !state;
        _issueLabelTextField.isReadOnly = !state;
        _imageRepoTextField.isReadOnly = !state;
        _screenshotsBranchTextField.isReadOnly = !state;
        
        _generateJsonButton.SetEnabled(state);
    }

    private void UpdateDefines()
    {
        string scriptingDefineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        List<string> definesList = scriptingDefineSymbolsForGroup.Split(';').ToList();

        if (state && !definesList.Contains(BUG_REPORTER_DEFINE))
        {
            definesList.Add(BUG_REPORTER_DEFINE);
            string newDefines = string.Join(";",definesList);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, newDefines);
            
        }
        else if (!state && definesList.Contains(BUG_REPORTER_DEFINE))
        {
            definesList.Remove(BUG_REPORTER_DEFINE);
            string newDefines = string.Join(";",definesList);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone,
                newDefines);
        }
    }


    private void GenerateJson(Label label)
    {
        Directory.CreateDirectory(JSON_FILES_PATH);
        File.WriteAllText($"{JSON_FILES_PATH}GithubAccessTokens.json",JsonConvert.SerializeObject(new Dictionary<string, string>()
        {
            {"github-repo-bug-tracker-token", _issuesRepoTokenTextField.text},
            {"github-repo-bug-images-token", _imagesRepoTokenTextField.text }
        }, Formatting.Indented));
        
        File.WriteAllText($"{JSON_FILES_PATH}.gitignore", "GithubAccessTokens.json\nGithubAccessTokens.json.meta");
        
        
        Directory.CreateDirectory(JSON_FILES_PATH);
        File.WriteAllText($"{JSON_FILES_PATH}GithubInfos.json",JsonConvert.SerializeObject(new Dictionary<string, string>()
        {
            {"github-username",     _usernameTextField.text},
            {"product-header",      _poductHeaderTextField.text},
            {"issues-repo",         _issuesRepoTextField.text},
            {"screenshots-repo",    _imageRepoTextField.text},
            {"screenshots-branch",  _screenshotsBranchTextField.text},
            {"custom-label",        _issueLabelTextField.text},
        }, Formatting.Indented));
        
        
        label.style.display = DisplayStyle.Flex;
    }
}