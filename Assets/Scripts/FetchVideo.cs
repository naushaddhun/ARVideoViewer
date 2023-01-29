using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchVideo : MonoBehaviour
{
    private FTPClient ftpClient;
    // Start is called before the first frame update
    void Start()
    {
        ftpClient = new FTPClient();
        ftpClient.downloadFile("ARVideo.mp4");

        GameObject camera = GameObject.Find("Main Camera");
        var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCameraAlpha = 0.5F;
        videoPlayer.url = "ARVideo.mp4";
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

}
