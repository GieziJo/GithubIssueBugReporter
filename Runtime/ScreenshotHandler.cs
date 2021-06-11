using System;
using UnityEngine;

namespace Giezi.Tools
{
    public class ScreenshotHandler : MonoBehaviour
    {
        public event Action<byte[]> OnScreenshotTakingDone = delegate(byte[] bytes) {  }; 
        private RenderTexture _currentTexture;
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
        
            OnScreenshotTakingDone(byteArray);
        }

        public void TakeScreenShot()
        {
            _currentTexture = Camera.main.targetTexture;
            Debug.Log(_currentTexture);
            Camera.main.targetTexture = RenderTexture.GetTemporary(Camera.main.pixelWidth, Camera.main.pixelHeight, 16);
            Camera.onPostRender += MyPostRenderer;
        }
    }
}