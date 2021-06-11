using System;
using System.Collections;
using System.Collections.Generic;
using Octokit;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
using Random = UnityEngine.Random;

public class UploadImage : MonoBehaviour
{
    private RenderTexture _currentTexture;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToTakeScreenshot());
    }

    private IEnumerator WaitToTakeScreenshot()
    {
        yield return new WaitForSeconds(1f);
        TakeScreenShot();
    }

    private void UploadImageToGithub(byte[] byteArray)
    {
        string image = Convert.ToBase64String(byteArray);
        
        var client = new GitHubClient(new ProductHeaderValue("EarlyCoffee.Games.Hermit"));
        var basicAuth = new Credentials(AccessToken.IMAGE_REPO_TOKEN);
        client.Credentials = basicAuth;

        CreateFileRequest newFile = new CreateFileRequest(DateTime.UtcNow.AddHours(1).ToString("yyyy_MM_dd_HH_mm_ss") + "_" + Random.Range(0,100000), image, "Screenshots", false);
        

        WaitForAnswer(client, newFile);
    }

    private async void WaitForAnswer(GitHubClient client, CreateFileRequest newFile)
    {
        var response = await client.Repository.Content.CreateFile("EarlyCoffeeGames", "Hermit-Issues-Screenshots", $"Screenshots/{newFile.Message}.png", newFile);
    }

    private void MyPostRenderer(Camera cam)
    {
        if (cam != Camera.main)
            return;
        
        RenderTexture renderTexture = Camera.main.targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);

        byte[] byteArray = renderResult.EncodeToPNG();
        
        // System.IO.File.WriteAllBytes(Application.dataPath + "test.png", byteArray);
        
        Debug.Log("Screenshot taken");
        
        RenderTexture.ReleaseTemporary(renderTexture);
        Camera.main.targetTexture = _currentTexture;

        Camera.onPostRender -= MyPostRenderer;
        
        UploadImageToGithub(byteArray);
    }

    private void TakeScreenShot()
    {
        _currentTexture = Camera.main.targetTexture;
        Debug.Log(_currentTexture);
        Camera.main.targetTexture = RenderTexture.GetTemporary(Camera.main.pixelWidth, Camera.main.pixelHeight, 16);
        Camera.onPostRender += MyPostRenderer;
    }
}
