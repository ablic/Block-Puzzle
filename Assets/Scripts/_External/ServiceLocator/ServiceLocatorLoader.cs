using UnityEngine;

namespace RaspberryGames
{
	public abstract class ServiceLocatorLoader : MonoBehaviour
	{
		[SerializeField] 
		private ServiceLocatorLoadMode loadMode = ServiceLocatorLoadMode.Awake;
		
		private void Awake()
		{
			if (loadMode == ServiceLocatorLoadMode.Awake)
			{
				Load();
			}
		}
		
		private void Start()
		{
			if (loadMode == ServiceLocatorLoadMode.Start)
			{
				Load();
			}
		}
		
		public abstract void Load();
	}
}

