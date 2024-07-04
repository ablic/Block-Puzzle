using UnityEngine;
using UnityEngine.UI;

namespace RaspberryGames
{
	[RequireComponent(typeof(Button))]
	public class ButtonStateChanger : MonoBehaviour
	{
		[SerializeField] private Image image;
		[SerializeField] private Sprite[] stateSprites;
		
		private int state = 0;
		
		public int State
		{
			get => state;
			set
			{
				state = value;
				image.sprite = stateSprites[state % stateSprites.Length];
			}
		}
		
		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() => 
			{
				image.sprite = stateSprites[++state % stateSprites.Length];
			});
		}
	}
}