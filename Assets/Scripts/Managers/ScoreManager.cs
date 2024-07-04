using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace RaspberryGames.BlockPuzzle
{
	public class ScoreManager : MonoBehaviour
	{
		private UiManager uiManager;
		private int score;
		
		public int Score
		{
			get { return score; }
			set 
			{ 
				score = value;
				uiManager.UpdateScore(score);
				
				if (score > YandexGame.savesData.bestScore)
				{
					YandexGame.savesData.bestScore = score;
					uiManager.UpdateBestScore(score);
				}
			}
		}
		
		private void Awake()
		{
			uiManager = ServiceLocator.Get<UiManager>();
		}
		
		private void OnEnable()
		{
			YandexGame.GetDataEvent += Init;
		}

		private void OnDisable()
		{
			YandexGame.GetDataEvent -= Init;
		}
		
		private void Init()
		{
			uiManager.UpdateBestScore(YandexGame.savesData.bestScore);
		}
		
		public void IncreaseScoreForLines(int linesCount)
		{
			Score += linesCount * Board.Size;
		}
	}
}
