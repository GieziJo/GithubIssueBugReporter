using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Giezi.Tools
{
    public class ScreenshotHandler : MonoBehaviour
    {
        public event Action<byte[]> OnScreenshotTakingDone = delegate(byte[] bytes) {  }; 
        private RenderTexture _currentTexture;
        
        IEnumerator WaitForScreenshot()
        {
            yield return new WaitForEndOfFrame();

            _currentTexture = new RenderTexture(Screen.width, Screen.height, 0);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(_currentTexture);
            AsyncGPUReadback.Request(_currentTexture, 0, TextureFormat.RGBA32, ReadbackCompleted);
        }

        void ReadbackCompleted(AsyncGPUReadbackRequest request)
        {
            DestroyImmediate(_currentTexture);

            using (NativeArray<byte> imageBytes = request.GetData<byte>())
            {
                OnScreenshotTakingDone(imageBytes.ToArray());
            }
        }

        public void TakeScreenShot()
        {
            StartCoroutine(WaitForScreenshot());
        }
    }
}