using System;
using System.IO;
using UnityEngine;

public class Screenshooter : MonoBehaviour
{
	#if UNITY_EDITOR
	
	[SerializeField] private string folderPath;
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !string.IsNullOrEmpty(folderPath))
		{
			if (!string.IsNullOrEmpty(folderPath)) 
			{
				string path = Path.Combine(folderPath, $"{Application.productName}-{Screen.width}x{Screen.height}-{Guid.NewGuid()}.jpg");
				ScreenCapture.CaptureScreenshot(path);
				Debug.Log("Screenshot captured as " + path);
			}
			else
				Debug.LogError("Screenshot folder path is undefined!");
		}
	}
	
	#endif
}
