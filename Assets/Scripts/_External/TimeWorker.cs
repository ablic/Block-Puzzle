using System.Collections;
using UnityEngine;

namespace RaspberryGames
{
    public class TimeWorker
	{
		private MonoBehaviour monoBehaviour;
		
		public TimeWorker(MonoBehaviour monoBehaviour)
		{
			this.monoBehaviour = monoBehaviour;
		}
		
		public void ExecuteAfterSeconds(float seconds, System.Action action)
		{
			monoBehaviour.StartCoroutine(ActionAfterSeconds(seconds, action));
		}
		
		private IEnumerator ActionAfterSeconds(float seconds, System.Action action)
		{
			yield return new WaitForSeconds(seconds);
			action();
		}
	}
}