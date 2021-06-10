using Octokit;
using UnityEngine;

public class GithubBugReporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ReportBug();   
    }

    void ReportBug()
    {
        var client = new GitHubClient(new ProductHeaderValue("EarlyCoffee.Games.Hermit"));
        var basicAuth = new Credentials(AccessToken.TOKEN);
        client.Credentials = basicAuth;
        WaitForAnswer(client);
    }

    private async void WaitForAnswer(GitHubClient client)
    {
        var createIssue = new NewIssue("this thing doesn't work");
        var issue = await client.Issue.Create("EarlyCoffeeGames", "Hermit-Issues", createIssue);
    }
}