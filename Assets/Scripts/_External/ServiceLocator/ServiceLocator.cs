using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaspberryGames
{
	public static class ServiceLocator
	{
		private static Dictionary<Type, object> services = new ();
		
		public static void Register<T>(T service) where T : class
		{
			Type type = typeof(T);
			
			if (services.ContainsKey(type))
			{
				Debug.LogError($"Failed to register service. Service of type {type.Name} has already been registered");
				return;
			}
			
			services.Add(typeof(T), service);
		}
		
		public static void Unregister<T>() where T : class
		{
			Type type = typeof(T);
			
			if (!services.ContainsKey(type))
			{
				Debug.LogError($"Failed to unregister service. Service of type {type.Name} is not registered");
				return;
			}
			
			services.Remove(type);
		}
		
		public static void UnregisterAll()
		{
			services.Clear();
		}
		
		public static T Get<T>() where T : class
		{
			Type type = typeof(T);
			
			if (!services.ContainsKey(type))
			{
				Debug.LogError($"Failed to get service. Service of type {type.Name} is not registered");
				return null;
			}
			
			return services[typeof(T)] as T;
		}
	}
}

