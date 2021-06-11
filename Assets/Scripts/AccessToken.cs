using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public static class AccessToken
{
    public static string BUG_REPO_TOKEN => ReturnBugRepoToken();
    public static string IMAGE_REPO_TOKEN => ReturnImageRepoToken();

    private static string bug_repo_token;
    private static string image_repo_token;

    static AccessToken()
    {
        Initialize();
    }

    private static string ReturnBugRepoToken() => bug_repo_token;

    private static string ReturnImageRepoToken() => image_repo_token;

    private static void Initialize()
    {
        if (!(bug_repo_token == null || image_repo_token == null))
            return;
        Dictionary<string, string> tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>("GieziTools.Tokens/GithubAccessTokens").ToString());
        bug_repo_token = tokens["github-repo-bug-tracker-token"];
        bug_repo_token = tokens["github-repo-bug-images-token"];
    }
}