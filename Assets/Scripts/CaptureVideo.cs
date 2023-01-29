using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureVideo : MonoBehaviour
{
    public GameObject sliderPrefab;
    private FTPClient ftpClient;

    public void RecordVideo()
    {
        ftpClient = new FTPClient(sliderPrefab);
        NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
        {
            //textUIValue.text = "Video path: " + path;
            Debug.Log("Video path: " + path);

            Debug.Log("Video path: " + path);
            if (path != null)
            {
                // Play the recorded video
                // Handheld.PlayFullScreenMovie("file://" + path);
                NativeGallery.SaveVideoToGallery(path, "ARVideoViewer", "ARVideo.mp4");
                ftpClient.uploadFile("ARVideo.mp4", path);
            }
        }, NativeCamera.Quality.Default, 30);

        Debug.Log("Permission result: " + permission);
    }
}
