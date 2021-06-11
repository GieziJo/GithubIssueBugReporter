using Octokit;
using UnityEngine;

public class GithubBugReporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ReportBug();   
    }

    void ReportBug()
    {
        // new AccessToken("test1", "test2");
        
        // Debug.Log(AccessToken.BUG_REPO_TOKEN);
        
        var client = new GitHubClient(new ProductHeaderValue("EarlyCoffee.Games.Hermit"));

        var basicAuth = new Credentials(AccessToken.BUG_REPO_TOKEN);
        client.Credentials = basicAuth;
        WaitForAnswer(client);
    }

    private async void WaitForAnswer(GitHubClient client)
    {
        var createIssue = new NewIssue("this thing doesn't work2");
        var issue = await client.Issue.Create("EarlyCoffeeGames", "Hermit-Issues-Screenshots", createIssue);
    }
}