using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
	[CreateAssetMenu(fileName = "Configuration")]
	public class Configuration : ScriptableObject
	{
		[SerializeField] private Sprite[] blockSprites;
		
		[Header("Figure")]
		[Min(0f)]
		[SerializeField] private  float figurePointerFollowingSpeed;
		[Min(0f)]
		[SerializeField] private  float figurePlacingSpeed;
		[Min(0f)]
		[SerializeField] private  float figureReturnSpeed;
		[Range(0f, 1f)]
		[SerializeField] private  float figureLockedStateAlpha;
		[Min(1f)]
		[SerializeField] private  float figureScaleWhenDragging;
		[SerializeField] private  AudioClip figureGrabSound;
		[SerializeField] private  AudioClip figureReturnSound;
		[SerializeField] private  AudioClip figurePlaceSound;
		[Header("Board")]
		[Min(0.01f)]
		[SerializeField] private  float boardClearPeriod;
		[SerializeField] private  AudioClip boardClearSound;
		[Header("Score")]
		[Min(0.01f)]
		[SerializeField] private  float scoreUpdatePeriod;
		[Header("Ads")]
		[Range(0, 5)]
		[SerializeField] private int stepBackAmountOnRestart = 1;
		[Range(1, 5)]
		[SerializeField] private int stepBackAmountAward = 1;
		
		public Sprite[] BlockSprites => blockSprites;
		public int MaxBlockId => blockSprites.Length;
		
		public float FigurePointerFollowingSpeed => figurePointerFollowingSpeed;
		public float FigurePlacingSpeed => figurePlacingSpeed;
		public float FigureReturnSpeed => figureReturnSpeed;
		public float FigureLockedStateAlpha => figureLockedStateAlpha;
		public float FigureScaleWhenDragging => figureScaleWhenDragging;
		public AudioClip FigureGrabSound => figureGrabSound;
		public AudioClip FigureReturnSound => figureReturnSound;
		public AudioClip FigurePlaceSound => figurePlaceSound;
		public float BoardClearPeriod => boardClearPeriod;
		public AudioClip BoardClearSound => boardClearSound;
		public float ScoreUpdatePeriod => scoreUpdatePeriod;
		public int StepBackAmountOnRestart => stepBackAmountOnRestart;
		public int StepBackAmountAward => stepBackAmountAward;
		
		public Sprite GetBlockSpriteById(int id)
		{
			return blockSprites[id - 1];
		}
		
		public int GetRandomBlockId()
		{
			return Random.Range(1, blockSprites.Length + 1);
		}
	}
}