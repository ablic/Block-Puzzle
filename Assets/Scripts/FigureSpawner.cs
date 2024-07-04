using System;
using System.Collections.Generic;
using RaspberryGames.ObjectPooling;
using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
	public class FigureSpawner : MonoBehaviour
	{
		[SerializeField] private FigurePresenter figurePresenterPrefab;
		[SerializeField] private SpriteRenderer blockPrefab;
		[SerializeField] private Transform[] spawnPoints;

		private Configuration config;
		private BoardPresenter boardPresenter;
		private ObjectPool objectPool;
		private UiManager uiManager;
		private FigurePresenter[] figurePresenters;

		public int NumberEmptyPlaces
		{
			get
			{
				int number = 0;
				
				for (int i = 0; i < figurePresenters.Length; i++)
				{
					if (figurePresenters[i] == null)
					{
						number++;
					}
				}
				return number;
			}
		}

		public event Action AllFiguresLocked;

		private void Awake()
		{
			config = ServiceLocator.Get<Configuration>();
			boardPresenter = ServiceLocator.Get<BoardPresenter>();
			objectPool = ServiceLocator.Get<ObjectPool>();
			uiManager = ServiceLocator.Get<UiManager>();
		}
		
		private void Start()
		{
			figurePresenters = new FigurePresenter[spawnPoints.Length];
			SpawnAll();
		}

		public void SpawnAll()
		{
			for (int i = 0; i < spawnPoints.Length; i++)
			{
				figurePresenters[i] = CreateFigure(new Figure(
					FigureMatrices.GetRandom(), 
					config.GetRandomBlockId()));
			}
		}
		
		public void TrySpawn()
		{
			for (int i = 0; i < figurePresenters.Length; i++)
				if (figurePresenters[i] != null)
					return;
			
			SpawnAll();
		}
		
		public void MakeSpace(FigurePresenter figurePresenter)
		{
			for (int i = 0; i < figurePresenters.Length; i++)
			{
				if (figurePresenters[i] == figurePresenter)
				{
					figurePresenters[i] = null;
				}
			}
		}
		
		public FigurePresenter CreateFigure(Figure figure)
		{
			FigurePresenter figurePresenter = objectPool.GetComponent(figurePresenterPrefab);
			
			Vector2 position = Vector2.zero;
			
			for (int i = 0; i < figurePresenters.Length; i++)
			{
				if (figurePresenters[i] == null)
				{
					figurePresenters[i] = figurePresenter;
					position = spawnPoints[i].position;
					break;
				}
			}
			
			figurePresenter.transform.position = position;
			figurePresenter.InitialPosition = position;
			figurePresenter.IsLocked = false;
			figurePresenter.Build(figure);
			figurePresenter.MoveTo(position, config.FigureReturnSpeed);
			figurePresenter.ScaleTo(0.5f, config.FigureReturnSpeed);

			return figurePresenter;
		}

		public void CheckForFreeSpace()
		{
			foreach (var figurePresenter in figurePresenters)
			{
				if (figurePresenter == null)
					continue;
					
				if (boardPresenter.CanPlaceFigure(figurePresenter.Model))
				{
					figurePresenter.IsLocked = false;
					return;
				}
				
				figurePresenter.IsLocked = true;
			}
			
			uiManager.ShowGameOverPanel();
		}
		
		public void Clear()
		{
			objectPool.Reload(figurePresenterPrefab);
		}
	}
}