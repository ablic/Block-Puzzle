using UnityEngine;

namespace RaspberryGames
{
    public static class ArrayExtension
	{
		public static T GetRandomElement<T>(this T[] array)
		{
			return array[Random.Range(0, array.Length)];
		}
	}
}

