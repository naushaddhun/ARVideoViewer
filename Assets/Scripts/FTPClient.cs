using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Net;
using System.Threading;

public class FTPClient : MonoBehaviour
{
	UnityEngine.UI.Slider sliderPrefab;
	GameObject slider;
    private string ftpUser = "arkubdev@arkub.co";
    private string ftpPassword = "Dubai2023#";
    private string server = "162.241.217.33";
    private string initialPath = "ARVideoViewer";
	Uri address;


	public FTPClient(GameObject sliderObj = null)
    {
		slider = sliderObj;		
		address = new Uri("ftp://" + server + "/" + Path.Combine(initialPath, "ARVideo.mp4"));		
	}


    public void uploadFile(string filename, string filePath)
    {
		FileInfo file = new FileInfo(filePath);
		FtpState state = new FtpState();
		FtpWebRequest request = FtpWebRequest.Create(address) as FtpWebRequest;
		request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
		state.Request = request;
		state.FileName = filePath;

		// Set control connection to closed after command execution
		request.KeepAlive = false;
		// Specify command to be executed
		request.Method = WebRequestMethods.Ftp.UploadFile;

		// Specify data transfer type
		request.UseBinary = true;

		// Notify server about size of uploaded file
		request.ContentLength = file.Length;
		request.BeginGetRequestStream(
		//new AsyncCallback (EndGetStreamCallback), 
		new AsyncCallback(EndGetStreamCallback),
		state
		);

		// Set buffer size to 2KB.
		/*var bufferLength = 2048;
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
				Debug.Log("Progress: " + ((float)((float)fs.Position / (float)fs.Length) * 100f));
				// Write content from file stream to FTP upload stream.
				stream.Write(buffer, 0, contentLength);
				contentLength = fs.Read(buffer, 0, bufferLength);
				sliderPrefab.value = ((float)((float)fs.Position / (float)fs.Length) * 100f);
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
		}*/

		Debug.Log("Upload successful.");
		SceneManager.LoadScene("ARScene");
	}

    public void downloadFile(string filename)
    {
		WebClient request = new WebClient();
		request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
		request.DownloadFile(address, Path.Combine(Application.persistentDataPath, "ARVideo.mp4") );
		/*request.DownloadFileAsync(address, Path.Combine(Application.persistentDataPath, "ARVideo.mp4") );*/
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

	public class FtpState
	{
		private ManualResetEvent wait;
		private FtpWebRequest request;
		private string fileName;
		private Exception operationException = null;
		string status;

		public FtpState()
		{
			wait = new ManualResetEvent(false);
		}

		public ManualResetEvent OperationComplete
		{
			get { return wait; }
		}

		public FtpWebRequest Request
		{
			get { return request; }
			set { request = value; }
		}

		public string FileName
		{
			get { return fileName; }
			set { fileName = value; }
		}
		public Exception OperationException
		{
			get { return operationException; }
			set { operationException = value; }
		}
		public string StatusDescription
		{
			get { return status; }
			set { status = value; }
		}
	}

	private static void EndGetStreamCallback(IAsyncResult ar)
	{
		FtpState state = (FtpState)ar.AsyncState;

		Stream requestStream = null;
		// End the asynchronous call to get the request stream.
		try
		{
			requestStream = state.Request.EndGetRequestStream(ar);
			// Copy the file contents to the request stream.
			const int bufferLength = 2048;
			byte[] buffer = new byte[bufferLength];
			int count = 0;
			int readBytes = 0;
			FileStream stream = File.OpenRead(state.FileName);
			do
			{
				readBytes = stream.Read(buffer, 0, bufferLength);
				requestStream.Write(buffer, 0, readBytes);
				count += readBytes;
			}
			while (readBytes != 0);
			Console.WriteLine("Writing {0} bytes to the stream.", count);
			// IMPORTANT: Close the request stream before sending the request.
			requestStream.Close();
			// Asynchronously get the response to the upload request.
			state.Request.BeginGetResponse(
				new AsyncCallback(EndGetResponseCallback),
				state
			);
		}
		// Return exceptions to the main application thread.
		catch (Exception e)
		{
			Console.WriteLine("Could not get the request stream.");
			state.OperationException = e;
			state.OperationComplete.Set();
			return;
		}

	}

	// The EndGetResponseCallback method  
	// completes a call to BeginGetResponse.
	private static void EndGetResponseCallback(IAsyncResult ar)
	{
		FtpState state = (FtpState)ar.AsyncState;
		FtpWebResponse response = null;
		try
		{
			response = (FtpWebResponse)state.Request.EndGetResponse(ar);
			response.Close();
			state.StatusDescription = response.StatusDescription;
			// Signal the main application thread that 
			// the operation is complete.
			state.OperationComplete.Set();
		}
		// Return exceptions to the main application thread.
		catch (Exception e)
		{
			Console.WriteLine("Error getting response.");
			state.OperationException = e;
			state.OperationComplete.Set();
		}
	}
}
