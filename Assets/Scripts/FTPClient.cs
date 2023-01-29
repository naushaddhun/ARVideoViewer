using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Net;

public class FTPClient : MonoBehaviour
{
	UnityEngine.UI.Slider sliderPrefab;
	GameObject slider;
    private string ftpUser = "arkubdev@arkub.co";
    private string ftpPassword = "Dubai2023#";
    private string server = "162.241.217.33";
    private string initialPath = "ARVideoViewer";
	Uri address;
	FtpWebRequest request;


	public FTPClient(GameObject sliderObj = null)
    {
		slider = sliderObj;		
		address = new Uri("ftp://" + server + "/" + Path.Combine(initialPath, "ARVideo.mp4"));
		request = FtpWebRequest.Create(address) as FtpWebRequest;
		request.Credentials = new NetworkCredential(ftpUser, ftpPassword);

		// Set control connection to closed after command execution
		request.KeepAlive = false;
	}


    public void uploadFile(string filename, string filePath)
    {
		FileInfo file = new FileInfo(filePath);
		// Specify command to be executed
		request.Method = WebRequestMethods.Ftp.UploadFile;

		// Specify data transfer type
		request.UseBinary = true;

		// Notify server about size of uploaded file
		request.ContentLength = file.Length;

		// Set buffer size to 2KB.
		var bufferLength = 2048;
		var buffer = new byte[bufferLength];
		var contentLength = 0;

		// Open file stream to read file
		var fs = file.OpenRead();

		try
		{
			var sliderObject = Instantiate(slider, new Vector3(0, 0, 0), Quaternion.identity);
			sliderPrefab = sliderObject.GetComponent<UnityEngine.UI.Slider>();
			// Stream to which file to be uploaded is written.
			var stream = request.GetRequestStream();

			// Read from file stream 2KB at a time.
			contentLength = fs.Read(buffer, 0, bufferLength);

			// Loop until stream content ends.
			while (contentLength != 0)
			{
				Debug.Log("Progress: " + ((fs.Position / fs.Length) * 100f));
				// Write content from file stream to FTP upload stream.
				stream.Write(buffer, 0, contentLength);
				contentLength = fs.Read(buffer, 0, bufferLength);
				sliderPrefab.value = ((fs.Position / fs.Length) * 100f);
				Debug.Log("slider value " + sliderPrefab.value);
			}

			// Close file and request streams
			stream.Close();
			fs.Close();
		}
		catch (Exception e)
		{
			Debug.LogError("Error uploading file: " + e.Message);
			return;
		}

		Debug.Log("Upload successful.");
		SceneManager.LoadScene("ARScene");
	}

    public void downloadFile(string filename)
    {
		WebClient request = new WebClient();
		request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
		byte[] fileData = request.DownloadData(address);
		if (fileData != null)
		{
			WriteByteArrayToFile(filename, fileData);
		}
	}

	public void WriteByteArrayToFile(string fileName, byte[] data)
	{
		FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
		fileStream.Write(data, 0, data.Length);
	}

	public void deleteFile(string filename)
    {
    }

    public void closeFtp()
    {
    }
}
