using UnityEngine;

namespace RaspberryGames.ObjectPooling
{
	public abstract class ComponentPoolInitializer<T> : MonoBehaviour where T : Component
	{
		[SerializeField] private ObjectPool objectPool;
		[SerializeField] private T prefab;
		[Min(2)]
		[SerializeField] private int size = 128;
		
		private void Awake()
		{
			objectPool.CreateComponentPool(prefab, size);
		}
	}
}