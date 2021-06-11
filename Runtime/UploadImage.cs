using System;
using Giezi.Tools;
using Octokit;
using UnityEngine;
using Random = UnityEngine.Random;

public static class UploadImage
{

    public static string UploadImageToGithub(byte[] byteArray)
    {
        string image = Convert.ToBase64String(byteArray);
        
        var client = new GitHubClient(new ProductHeaderValue(GithubInfos.ProductHeader));
        var basicAuth = new Credentials(AccessToken.IMAGE_REPO_TOKEN);
        client.Credentials = basicAuth;

        CreateFileRequest newFile = new CreateFileRequest(DateTime.UtcNow.AddHours(1).ToString("yyyy_MM_dd_HH_mm_ss") + "_" + Random.Range(0,100000), image, GithubInfos.ScreenshotsBranch, false);
        

        WaitForAnswer(client, newFile);

        return
            $"https://raw.githubusercontent.com/{GithubInfos.GithubUsername}/{GithubInfos.ScreenshotsRepo}/{GithubInfos.ScreenshotsBranch}/Screenshots/{newFile.Message}.png";
    }

    private static async void WaitForAnswer(GitHubClient client, CreateFileRequest newFile)
    {
        var response = await client.Repository.Content.CreateFile(GithubInfos.GithubUsername, GithubInfos.ScreenshotsRepo, $"Screenshots/{newFile.Message}.png", newFile);
    }
}
