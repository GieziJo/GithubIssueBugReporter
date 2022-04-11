using Giezi.Tools;
using Octokit;

public static class GithubBugReporter
{

    public static void ReportBug(string title, string body, string version)
    {
        
        var client = new GitHubClient(new ProductHeaderValue(GithubInfos.ProductHeader));

        var basicAuth = new Credentials(AccessToken.BUG_REPO_TOKEN);
        client.Credentials = basicAuth;
        WaitForAnswer(client, title, body, version);
    }

    private static async void WaitForAnswer(GitHubClient client, string title, string body, string version)
    {
        var createIssue = new NewIssue(title){ Body = body, Labels = {GithubInfos.CustomLabel, "App Version " + version}};
        var issue = await client.Issue.Create(GithubInfos.GithubUsername, GithubInfos.IssuesRepo, createIssue);
    }
}