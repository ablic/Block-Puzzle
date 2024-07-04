using System;
using UnityEngine;

namespace RaspberryGames
{
    public class ScreenSizeChangeListener : MonoBehaviour
	{
		private float prevScreenRatio = 0f;
		public event Action Changed;
		
		private void Update()
		{
			float screenRatio = (float)Screen.width / Screen.height;
			
			if (screenRatio != prevScreenRatio)
				Changed?.Invoke();
			
			prevScreenRatio = screenRatio;
		}
	}
}