using UnityEngine;

namespace RaspberryGames
{
	public class CameraScaler : MonoBehaviour
	{
		private enum ScaleType
		{
			LessThan,
			GreaterThan
		}
		
		[SerializeField] private ScreenSizeChangeListener screenSizeChangeListener;
		[SerializeField] private float aspectRatioThreshold = 960f / 640f;
		[SerializeField] private ScaleType scaleType = ScaleType.LessThan;
		[SerializeField] private float normalOrthographicSize;
		
		private void OnEnable()
		{
			screenSizeChangeListener.Changed += ChangeSize;
		}
		
		private void OnDisable()
		{
			screenSizeChangeListener.Changed -= ChangeSize;
		}
		
		[ContextMenu("Change size")]
		private void ChangeSize()
		{
			float aspectRatio = (float)Screen.width / Screen.height;
			
			if (scaleType == ScaleType.LessThan)
			{
				if (aspectRatio < aspectRatioThreshold)
				{
					Camera.main.orthographicSize = normalOrthographicSize * aspectRatioThreshold / aspectRatio;
				}
				else
				{
					Camera.main.orthographicSize = normalOrthographicSize;
				}
			}
			else
			{
				if (aspectRatio > aspectRatioThreshold)
				{
					Camera.main.orthographicSize = normalOrthographicSize / aspectRatioThreshold * aspectRatio;
				}
				else
				{
					Camera.main.orthographicSize = normalOrthographicSize;
				}
			}
		}
	}
}