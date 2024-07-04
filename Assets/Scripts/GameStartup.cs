using RaspberryGames.ObjectPooling;
using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
	public class GameStartup : ServiceLocatorLoader
	{
		[SerializeField] private Configuration config;
		[SerializeField] private BoardPresenter boardPresenter;
		[SerializeField] private ObjectPool objectPool;
		[SerializeField] private FigureSpawner spawner;
		[SerializeField] private FigureController controller;
		[SerializeField] private UiManager uiManager;
		[SerializeField] private ScoreManager scoreManager;
		[SerializeField] private AudioManager audioManager;
		[SerializeField] private BonusManager bonusManager;

		private void Start()
		{
		}
		
		public override void Load()
		{
			ServiceLocator.Register(config);
			ServiceLocator.Register(boardPresenter);
			ServiceLocator.Register(objectPool);
			ServiceLocator.Register(uiManager);
			ServiceLocator.Register(scoreManager);
			ServiceLocator.Register(audioManager);
			ServiceLocator.Register(bonusManager);
			ServiceLocator.Register(spawner);
		}
	}
}