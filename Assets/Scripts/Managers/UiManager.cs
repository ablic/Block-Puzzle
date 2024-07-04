using TMPro;
using UnityEngine;
using YG;

namespace RaspberryGames.BlockPuzzle
{
	public class UiManager : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI currentScore;
		[SerializeField] private TextMeshProUGUI currentBestScore;
		[SerializeField] private TextMeshProUGUI currentStepsBack;
		[SerializeField] private LangYGAdditionalText finalScore;
		[SerializeField] private LangYGAdditionalText finalBestScore;
		[Header("Buttons")]
		[SerializeField] private GameObject[] gameplayButtons;
		[Header("Panels")]
		[SerializeField] private Animator stepBackPanelAnimator;
		[SerializeField] private Animator gameOverPanelAnimator;
		
		private void Awake()
		{
		}
		
		public void UpdateStepsBack(int value)
		{
			currentStepsBack.text = value.ToString();
		}
		
		public void UpdateScore(int value)
		{
			string strValue = value.ToString();
			currentScore.text = strValue;
			finalScore.additionalText = strValue;
		}
		
		public void UpdateBestScore(int value)
		{
			string strValue = value.ToString();
			currentBestScore.text = strValue;
			finalBestScore.additionalText = strValue;
		}
		
		public void ShowStepBackPanel()
		{
			SetGameplayButtonsActive(false);
			stepBackPanelAnimator.gameObject.SetActive(true);
			stepBackPanelAnimator.SetTrigger("pop in");
		}
		
		public void HideStepBackPanel()
		{
			SetGameplayButtonsActive(true);
            stepBackPanelAnimator.SetTrigger("pop out");
        }
		
		public void ShowGameOverPanel()
		{
			SetGameplayButtonsActive(false);
			gameOverPanelAnimator.gameObject.SetActive(true);
            stepBackPanelAnimator.SetTrigger("pop in");
        }
		
		public void HideGameOverPanel()
		{
			SetGameplayButtonsActive(true);
            gameOverPanelAnimator.SetTrigger("pop out");
        }
		
		private void SetGameplayButtonsActive(bool active)
		{
			foreach (var button in gameplayButtons)
				button.SetActive(active);
		}
	}
}