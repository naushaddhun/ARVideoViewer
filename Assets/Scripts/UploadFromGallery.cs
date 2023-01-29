using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadFromGallery : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PickImageOrVideo()
	{
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
				}
			}, NativeGallery.MediaType.Image | NativeGallery.MediaType.Video, "Select an image or video");

			Debug.Log("Permission result: " + permission);
		}
	}
}
