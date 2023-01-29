using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadFromGallery : MonoBehaviour
{
	public GameObject sliderPrefab;
	private FTPClient ftpClient;

	public void PickImageOrVideo()
	{
		ftpClient = new FTPClient(sliderPrefab);
		if (NativeGallery.CanSelectMultipleMediaTypesFromGallery())
		{
			NativeGallery.Permission permission = NativeGallery.GetMixedMediaFromGallery((path) =>
			{
				Debug.Log("Media path: " + path);
				if (path != null)
				{
					// Determine if user has picked an image, video or neither of these
					switch (NativeGallery.GetMediaTypeOfFile(path))
					{
						case NativeGallery.MediaType.Image: Debug.Log("Picked image"); break;
						case NativeGallery.MediaType.Video: Debug.Log("Picked video"); break;
						default: Debug.Log("Probably picked something else"); break;
					}
					ftpClient.uploadFile("ARVideo.mp4", path);
				}
			}, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select an image or video");

			Debug.Log("Permission result: " + permission);
		}
	}
}
