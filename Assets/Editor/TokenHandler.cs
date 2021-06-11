using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class TokenHandler : EditorWindow
{
    private TextField _imagesRepoTextField;
    private TextField _issuesRepoTextField;

    [MenuItem("Tools/GieziTools/TokenHandler")]
    public static void HandleTokens()
    {
        TokenHandler wnd = GetWindow<TokenHandler>();
        wnd.maxSize = new Vector2(600f, 100f);
        wnd.titleContent = new GUIContent("TokenHandler");
    }

    public void CreateGUI()
    {
        VisualElement visualElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/TokenHandler.uxml").Instantiate();
        rootVisualElement.Add(visualElement);
        _issuesRepoTextField = visualElement.Q<TextField>("IssuesRepo");
        _imagesRepoTextField = visualElement.Q<TextField>("ImageRepo");

        if (File.Exists("Assets/Resources/GieziTools.Tokens/GithubAccessTokens.json"))
        {
            Dictionary<string, string> tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>("GieziTools.Tokens/GithubAccessTokens").ToString());
            _issuesRepoTextField.value = tokens["github-repo-bug-tracker-token"];
            _imagesRepoTextField.value = tokens["github-repo-bug-images-token"];
        }

        visualElement.Q<Button>("GenerateJson").clicked += () => GenerateJson(visualElement.Q<Label>("ConfirmationText"));
    }

    private void GenerateJson(Label label)
    {
        Directory.CreateDirectory("Assets/Resources/GieziTools.Tokens/");
        File.WriteAllText("Assets/Resources/GieziTools.Tokens/GithubAccessTokens.json",JsonConvert.SerializeObject(new Dictionary<string, string>()
        {
            {"github-repo-bug-tracker-token", _issuesRepoTextField.value},
            {"github-repo-bug-images-token", _imagesRepoTextField.value }
        }, Formatting.Indented));
        label.style.display = DisplayStyle.Flex;
    }
}