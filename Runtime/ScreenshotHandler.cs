using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Giezi.Tools
{
    public class ScreenshotHandler : MonoBehaviour
    {
        private bool useRenderPipelinePostProcessing = false;
        public event Action<byte[]> OnScreenshotTakingDone = delegate(byte[] bytes) {  };
        
        IEnumerator WaitForScreenshot()
        {
            if (Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing)
            {
                useRenderPipelinePostProcessing = true;
                Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = false;
            }
            yield return new WaitForEndOfFrame();
            
            Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            texture.ReadPixels(rect,0,0);
            byte[] bytearray = texture.EncodeToPNG();
            OnScreenshotTakingDone(bytearray);
            DestroyImmediate(texture);

            if (useRenderPipelinePostProcessing)
                Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = true;
        }

        public void TakeScreenShot() => StartCoroutine(WaitForScreenshot());
    }
}