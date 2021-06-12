using System;
using System.Collections;
using System.IO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Giezi.Tools
{
    public class ScreenshotHandler : MonoBehaviour
    {
        public event Action<byte[]> OnScreenshotTakingDone = delegate(byte[] bytes) {  };
        
        IEnumerator WaitForScreenshot()
        {
            yield return new WaitForEndOfFrame();
            
            Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            texture.ReadPixels(rect,0,0);
            byte[] bytearray = texture.EncodeToPNG();
            OnScreenshotTakingDone(bytearray);
            DestroyImmediate(texture);
        }

        public void TakeScreenShot() => StartCoroutine(WaitForScreenshot());
    }
}