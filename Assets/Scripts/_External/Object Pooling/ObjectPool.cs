using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaspberryGames.ObjectPooling
{
	public class ObjectPool : MonoBehaviour
	{
		[Serializable]
		public class PoolSetup
		{
			public GameObject gameObjectPrefab;
			[Min(2)]
			public int poolSize = 32;
		}
		
		[SerializeField] private PoolSetup[] setup;
		
		private Dictionary<GameObject, List<GameObject>> goPools = new();
		private Dictionary<Component, List<Component>> componentPools = new();
		
		private void Awake()
		{
			foreach (var poolSetup in setup)
			{
				List<GameObject> pool = new(poolSetup.poolSize);
				
				for (int i = 0; i < poolSetup.poolSize; i++)
				{
					var go = Instantiate(poolSetup.gameObjectPrefab, transform);
					go.SetActive(false);
					pool.Add(go);
				}
				
				goPools.Add(poolSetup.gameObjectPrefab, pool);
			}
		}
		
		public void CreateComponentPool<T>(T prefab, int poolSize) where T : Component
		{
			List<Component> pool = new (poolSize);
			
			for (int i = 0; i < poolSize; i++)
			{
				var go = Instantiate(prefab.gameObject, transform);
				go.SetActive(false);
				pool.Add(go.GetComponent<T>());
			}
			
			componentPools.Add(prefab, pool);
		}
		
		public List<GameObject> GetObjectPool(GameObject prefab)
		{
			return goPools[prefab];
		}
		
		public List<Component> GetComponentPool<T>(T prefab) where T : Component
		{
			return componentPools[prefab];
		}
		
		public GameObject GetObject(GameObject prefab)
		{
			List<GameObject> pool = goPools[prefab];
			int count = goPools[prefab].Count;
			
			for (int i = 0; i < count; i++)
				if (!pool[i].activeInHierarchy)
				{
					pool[i].SetActive(true);
					return pool[i];
				}
			
			return null;
		}
		
		public T GetComponent<T>(T prefab) where T : Component
		{
			List<Component> pool = componentPools[prefab];
			int count = componentPools[prefab].Count;
			
			for (int i = 0; i < count; i++)
				if (!pool[i].gameObject.activeInHierarchy)
				{
					pool[i].gameObject.SetActive(true);
					return pool[i] as T;
				}
			
			return null;
		}
		
		public void Reload(GameObject prefab)
		{
			goPools[prefab].ForEach(go => go.SetActive(false));
		}
		
		public void Reload<T>(T prefab) where T : Component
		{
			componentPools[prefab].ForEach(component => component.gameObject.SetActive(false));
		}
	}
}