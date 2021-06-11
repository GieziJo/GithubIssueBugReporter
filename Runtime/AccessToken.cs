using System.Collections.Generic;
using EncryptStringSample;
using Newtonsoft.Json;
using UnityEngine;

public static class AccessToken
{
    public static string BUG_REPO_TOKEN => bug_repo_token;
    public static string IMAGE_REPO_TOKEN => image_repo_token;

    private static string bug_repo_token;
    private static string image_repo_token;

    static AccessToken() => Initialize();

    private static void Initialize()
    {
        if (!(bug_repo_token == null || image_repo_token == null))
            return;
        Dictionary<string, string> tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>("GieziTools.GithubIssueBugReporter/GithubAccessTokens").ToString());
        bug_repo_token = StringCipher.Decrypt(tokens["github-repo-bug-tracker-token"]);
        image_repo_token = StringCipher.Decrypt(tokens["github-repo-bug-images-token"]);
    }
}