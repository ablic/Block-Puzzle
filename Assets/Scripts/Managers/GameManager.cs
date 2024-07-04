using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
	public class GameManager : MonoBehaviour
	{
		private BoardPresenter boardPresenter;
		private FigureSpawner figureSpawner;
		
		private void Awake()
		{
			boardPresenter = ServiceLocator.Get<BoardPresenter>();
			figureSpawner = ServiceLocator.Get<FigureSpawner>();
		}
		
		public void Restart()
		{
			boardPresenter.Clear();
			figureSpawner.Clear();
			figureSpawner.SpawnAll();
		}
		
		public void Revival()
		{
			
		}
	}
}