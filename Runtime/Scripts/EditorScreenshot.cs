using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity
{
    public class EditorScreenshot : MonoBehaviour
    {
        public bool StartFromDataPath = true;
        public string Directory = "/Screenshots";
        public KeyCode ScreenshotKey = KeyCode.F12;
        [Range(1, 4)] public int Supersize = 1;

        public void Update()
        {
            if (Input.GetKeyDown(ScreenshotKey)) {
                var basePath = (StartFromDataPath ? Application.dataPath : "") + Directory;
                if (!System.IO.Directory.Exists(basePath)) {
                    System.IO.Directory.CreateDirectory(basePath);
                }
                ScreenCapture.CaptureScreenshot($"{basePath}/{LTime.Now}.png", Supersize);
            }
        }
    }
}