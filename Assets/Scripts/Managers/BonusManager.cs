using UnityEngine;
using YG;

namespace RaspberryGames.BlockPuzzle
{
	public class BonusManager : MonoBehaviour
	{
		[SerializeField] private int stepBackAdId = 1;
		
		private Configuration config;
		private UiManager uiManager;
		private BoardPresenter boardPresenter;
		private int currentStepBackAmount;
		
		private void Awake()
		{
			config = ServiceLocator.Get<Configuration>();
			uiManager = ServiceLocator.Get<UiManager>();
			boardPresenter = ServiceLocator.Get<BoardPresenter>();
		}
		
		private void Start()
		{
			OnRestart();
		}
		
		private void OnEnable()
		{
			YandexGame.RewardVideoEvent += StepsBackRewarded;
		}
		
		private void OnDisable()
		{
			YandexGame.RewardVideoEvent -= StepsBackRewarded;
		}
		
		public void OnRestart()
		{
			currentStepBackAmount = config.StepBackAmountOnRestart;
			uiManager.UpdateStepsBack(currentStepBackAmount);
		}
		
		public void TryUseStepBack()
		{
			if (currentStepBackAmount > 0)
			{
				if (boardPresenter.TryStepBack())
				{
					currentStepBackAmount--;
					uiManager.UpdateStepsBack(currentStepBackAmount);
				}
			}
			else
			{
				uiManager.ShowStepBackPanel();
			}
		}
		
		public void ShowAdForStepsBack()
		{
			YandexGame.RewVideoShow(stepBackAdId);
		}
		
		private void StepsBackRewarded(int id)
		{
			if (id != stepBackAdId)
				return;
				
			currentStepBackAmount += config.StepBackAmountAward;
		}
	}
}