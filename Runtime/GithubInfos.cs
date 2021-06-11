using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Giezi.Tools
{
    public static class GithubInfos
    {
        public static string GithubUsername => github_username;

        public static string ProductHeader => product_header;

        public static string IssuesRepo => issues_repo;

        public static string ScreenshotsRepo => screenshots_repo;

        public static string ScreenshotsBranch => screenshots_branch;

        public static string CustomLabel => custom_label;

        private static string github_username;
        private static string product_header;
        private static string issues_repo;
        private static string screenshots_repo;
        private static string screenshots_branch;
        private static string custom_label;
        

        static GithubInfos() => Initialize();
        
        private static void Initialize()
        {
            Dictionary<string, string> tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>("GieziTools.GithubIssueBugReporter/GithubInfos").ToString());
            
            github_username = tokens["github-username"];
            product_header = tokens["product-header"];
            issues_repo = tokens["issues-repo"];
            screenshots_repo = tokens["screenshots-repo"];
            screenshots_branch = tokens["screenshots-branch"];
            custom_label = tokens["custom-label"];
        }
    }
}