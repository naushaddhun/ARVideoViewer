using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FetchVideo : MonoBehaviour
{
    private FTPClient ftpClient;
    // Start is called before the first frame update
    void Start()
    {
        ftpClient = new FTPClient();
        ftpClient.downloadFile("ARVideo.mp4");

        GameObject quad = gameObject;
        var videoPlayer = quad.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = Path.Combine(Application.persistentDataPath, "ARVideo.mp4");
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

}
