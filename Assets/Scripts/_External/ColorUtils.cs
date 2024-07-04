using UnityEngine;

namespace RaspberryGames
{
	public static class ColorUtils
	{
		public static Color SetAlpha(Color color, float alpha) 
		{
			return new Color(color.r, color.g, color.b, alpha);
		}
		
		public static Color AddRgbOffset(Color color, float rOffset, float gOffset, float bOffset)
		{
			color.r = Mathf.Clamp01(color.r + rOffset);
			color.g = Mathf.Clamp01(color.g + gOffset);
			color.b = Mathf.Clamp01(color.b + bOffset);
			
			return color;
		}
	}
}

