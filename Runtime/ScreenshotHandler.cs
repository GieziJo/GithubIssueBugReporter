using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            List<Camera> cameras = FindObjectsOfType<Camera>().ToList();
            var camerasDictionary = cameras.ToDictionary(x => x, x => x.GetUniversalAdditionalCameraData().renderPostProcessing);
            cameras.ForEach(cameraElement => cameraElement.GetUniversalAdditionalCameraData().renderPostProcessing = false);
            
            yield return new WaitForEndOfFrame();
            
            Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            texture.ReadPixels(rect,0,0);
            byte[] bytearray = texture.EncodeToPNG();
            OnScreenshotTakingDone(bytearray);
            DestroyImmediate(texture);

            foreach ((Camera cameraElement, bool renderPostProcessing) in camerasDictionary)
            {
                cameraElement.GetUniversalAdditionalCameraData().renderPostProcessing = renderPostProcessing;
            }
        }

        public void TakeScreenShot() => StartCoroutine(WaitForScreenshot());
    }
}