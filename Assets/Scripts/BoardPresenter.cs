using System;
using System.Collections;
using System.Collections.Generic;
using RaspberryGames.ObjectPooling;
using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class BoardPresenter : MonoBehaviour
	{
		[SerializeField] private GameObject targetPrefab;
		[SerializeField] private SpriteRenderer blockPrefab;
		[SerializeField] private GameObject blockParticlesPrefab;
		[SerializeField] private FigureSpawner figureSpawner;

		private Configuration config;
		private Board board;
		private ObjectPool objectPool;
		private AudioManager audioManager;
		private ScoreManager scoreManager;
		
		private WaitForSeconds waitForClearPeriod;
		private WaitForSeconds waitForLinesToClear;
		private const float halfSize = (Board.Size - 1f) / 2;
		
		private List<Coroutine> horizontalLineMarkAnims = new();
		private List<Coroutine> verticalLineMarkAnims = new();

		public SpriteRenderer[,] ViewContent { get; private set; }

		public event Action<int> Prepared;
		
		private void Awake()
		{
			config = ServiceLocator.Get<Configuration>();
			objectPool = ServiceLocator.Get<ObjectPool>();
			audioManager = ServiceLocator.Get<AudioManager>();
			scoreManager = ServiceLocator.Get<ScoreManager>();
		}
		
		private void Start()
		{
			Build();
		}
		
		public void Build()
		{
			board = new Board();
			ViewContent = new SpriteRenderer[Board.Size, Board.Size];

			waitForClearPeriod = new WaitForSeconds(config.BoardClearPeriod);
			waitForLinesToClear = new WaitForSeconds(config.BoardClearPeriod * Board.Size);
		}

		public Vector2Int GlobalToLocalPosition(Vector2 globalPosition)
		{
			return new Vector2Int(
				Mathf.RoundToInt(globalPosition.x + halfSize),
				Mathf.RoundToInt(globalPosition.y + halfSize));
		}

		public Vector2 LocalToGlobalPosition(int x, int y)
		{
			return new Vector2(
				x - halfSize,
				y - halfSize);
		}

		public void UpdateView()
		{
			for (int x = 0; x < Board.Size; x++)
			{
				for (int y = 0; y < Board.Size; y++)
				{
					if (ViewContent[x, y] == null)
						continue;
					
					ViewContent[x, y].gameObject.SetActive(false);
					ViewContent[x, y] = null;
				}	
			}
			
			for (int x = 0; x < Board.Size; x++)
			{
				for (int y = 0; y < Board.Size; y++)
				{
					if (board.Content[x, y] == Board.NullBlock)
						continue;
					
					SpriteRenderer block = objectPool.GetComponent(blockPrefab);
					block.transform.position = LocalToGlobalPosition(x, y);
					block.sprite = config.GetBlockSpriteById(board.Content[x, y]);
					ViewContent[x, y] = block;
				}
			}
		}
		
		public bool CanPlaceFigure(Figure figure)
		{
			return board.CanPlaceFigure(figure);
		}
		
		public void CheckPlaceForFigure(int x, int y, Figure figure)
		{
			ClearHorizontalLineMarkAnims();
			ClearVerticalLineMarkAnims();
			
			if (!board.CanPlaceFigureTo(x, y, figure))
			{
				ClearTargets();
				
				if (board.HasOnlyOneMatchingPosition(figure, out Vector2Int lastPosition))
					DrawTargetsForFigure(lastPosition.x, lastPosition.y, figure);
					
				return;
			}
				
			DrawTargetsForFigure(x, y, figure);
			MarkPotentiallyFilledLines(x, y, figure);
		}
		
		public bool TryPlaceFigureTo(int x, int y, Figure figure)
		{
			ClearTargets();
			
			if (!board.CanPlaceFigureTo(x, y, figure))
			{
				return false;
			}
			
			board.PlaceFigureTo(x, y, figure);
			scoreManager.Score += figure.NumberBlocks;
					
			return true;
		}
		
		public void ClearFilledLines()
		{			
			List<int> horizontalLinesToRemove;
			List<int> verticalLinesToRemove;
			
			board.ClearFilledLines(
				out horizontalLinesToRemove, 
				out verticalLinesToRemove);
			
			foreach (var y in horizontalLinesToRemove)
			{
				StartCoroutine( AnimateClearHorizontalLine(y) );
			}
			foreach (var x in verticalLinesToRemove)
			{
				StartCoroutine( AnimateClearVerticalLine(x) );
			}
				
			scoreManager.IncreaseScoreForLines(
				horizontalLinesToRemove.Count + verticalLinesToRemove.Count);
		}
		
		public bool TryStepBack()
		{
			if (figureSpawner.NumberEmptyPlaces == 0)
				return false;
				
			Board.Step lastStep = board.StepBack();
			
			if (lastStep.Figure == null)
				return false;
				
			figureSpawner.CreateFigure(lastStep.Figure);
			UpdateView();
			return true;
		}
		
		public void ClearTargets()
		{
			objectPool.Reload(targetPrefab);
		}

		public void Clear()
		{
			StartCoroutine(RemoveAll());
		}
		
		private void DrawTargetsForFigure(int x, int y, Figure figure)
		{
			ClearTargets();
			
			for (int x1 = 0; x1 < figure.Size; x1++)
			{
				for (int y1 = 0; y1 < figure.Size; y1++)
				{
					if (figure.Content[x1, y1] == 0)
						continue;
						
					GameObject target = objectPool.GetObject(targetPrefab);
					target.transform.position = LocalToGlobalPosition(x1 + x, y1 + y);
				}
			}
		}
		
		private void MarkPotentiallyFilledLines(int x, int y, Figure figure)
		{
			board.PlaceFigureTo(x, y, figure);
			
			List<int> hFilledLines;
			List<int> vFilledLines;
			
			board.ClearFilledLines(out hFilledLines, out vFilledLines);
			
			foreach (int y1 in hFilledLines)
			{
				MarkHorizontalLine(y1);
			}
			foreach (int x1 in vFilledLines)
			{
				MarkVerticalLine(x1);
			}
			
			board.StepBack();
		}
		
		private void ClearHorizontalLineMarkAnims()
		{
			foreach (var coroutine in horizontalLineMarkAnims)
				StopCoroutine(coroutine);
				
			horizontalLineMarkAnims.Clear();
		}
		
		private void ClearVerticalLineMarkAnims()
		{
			foreach (var coroutine in verticalLineMarkAnims)
				StopCoroutine(coroutine);
				
			verticalLineMarkAnims.Clear();
		}
		
		private void MarkHorizontalLine(int y)
		{
			Debug.Log($"h line marked {y}");
			//horizontalLineMarkAnims.Add( StartCoroutine( AnimateMarkHorizontalLine(y) ) );
		}
		
		private void MarkVerticalLine(int x)
		{
			Debug.Log($"v line marked {x}");
			//verticalLineMarkAnims.Add( StartCoroutine( AnimateMarkVerticalLine(x) ) );
		}
		
		private IEnumerator AnimateMarkHorizontalLine(int y)
		{
			while (true) 
			{
				// float angle = Mathf.PingPong(Time.time, 30f) - 15f;
				
				// for (int x = 0; x < Board.Size; x++)
				// {
				// 	if (ViewContent[x, y] != null)
				// 		ViewContent[x, y].transform.rotation = Quaternion.Euler(0f, 0f, angle);
				// }
				
				yield return null;
			}
		}
		
		private IEnumerator AnimateMarkVerticalLine(int x)
		{
			while (true) 
			{
				// float angle = Mathf.PingPong(Time.time, 30f) - 15f;
				
				// for (int x = 0; x < Board.Size; x++)
				// {
				// 	if (ViewContent[x, y] != null)
				// 		ViewContent[x, y].transform.rotation = Quaternion.Euler(0f, 0f, angle);
				// }
				
				yield return null;
			}
		}
		
		private IEnumerator AnimateClearHorizontalLine(int y)
		{
			for (int x = 0; x < Board.Size; x++)
			{
				if (ViewContent[x, y] != null)
				{
					RemoveBlock(ViewContent[x, y]);
					ViewContent[x, y] = null;
				}
				yield return waitForClearPeriod;
			}
		}
		
		private IEnumerator AnimateClearVerticalLine(int x)
		{
			for (int y = 0; y < Board.Size; y++)
			{
				if (ViewContent[x, y] != null)
				{
					RemoveBlock(ViewContent[x, y]);
					ViewContent[x, y] = null;
				}
				yield return waitForClearPeriod;
			}
		}

		private IEnumerator RemoveAll()
		{
			SpriteRenderer[,] matrixToRemove = new SpriteRenderer[Board.Size, Board.Size];

			audioManager.PlaySound(config.BoardClearSound);

			for (int x = 0; x < Board.Size; x++)
			{
				for (int y = 0; y < Board.Size; y++)
				{
					matrixToRemove[x, y] = ViewContent[x, y];
					ViewContent[x, y] = null;
				}
			}

			for (int x1 = -Board.Size + 1; x1 < Board.Size; x1++)
			{
				bool hasBlocks = false;

				for (int y = 0; y < Board.Size; y++)
				{
					int x = x1 + y;

					if (x < 0 || x >= Board.Size)
						continue;

					if (matrixToRemove[x, y] != null)
					{
						hasBlocks = true;
						RemoveBlock(matrixToRemove[x, y]);
						matrixToRemove[x, y] = null;
					}
				}

				yield return hasBlocks ? waitForClearPeriod : null;
			}
		}

		private void RemoveBlock(SpriteRenderer block)
		{
			block.gameObject.SetActive(false);
			objectPool.GetObject(blockParticlesPrefab).transform.position = block.transform.position;
		}
	}
}