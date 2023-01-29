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
        Debug.Log("Inside Start");
        var videoPlayer = quad.AddComponent<UnityEngine.Video.VideoPlayer>();
        /*videoPlayer.transform.eulerAngles = new Vector3(0, 0, 90);*/
        /*videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;*/
        videoPlayer.url = Path.Combine(Application.persistentDataPath, "ARVideo.mp4");
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

}
