using System;
using System.Collections.Generic;
using RaspberryGames.ObjectPooling;
using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class FigurePresenter : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer blockPrefab;
		
		private BoxCollider2D boxCollider;
		private Vector3 targetPosition;
		private float targetMovementSpeed;
		private float targetScale;
		private float targetScalingSpeed;
		private bool isPlacing;
		private bool isLocked;
		private ObjectPool objectPool;
		private Configuration config;

		public event Action PositionReached;

		public Figure Model { get; private set; }
		public SpriteRenderer[,] ViewContent { get; private set; }
		public List<SpriteRenderer> Blocks { get; private set; } = new List<SpriteRenderer>();

		public bool IsLocked
		{
			get { return isLocked; }
			set
			{
				isLocked = value;
				boxCollider.enabled = !isLocked;

				if (isLocked)
				{
					foreach (var block in Blocks)
						block.color = ColorUtils.SetAlpha(block.color, config.FigureLockedStateAlpha);
				}
				else
				{
					foreach (var block in Blocks)
						block.color = ColorUtils.SetAlpha(block.color, 1f);
				}
			}
		}

		public Vector2 ViewOffset { get; private set; }
		public Vector2 InitialPosition { get; set; }

		private void Awake()
		{
			boxCollider = GetComponent<BoxCollider2D>();
			objectPool = ServiceLocator.Get<ObjectPool>();
			config = ServiceLocator.Get<Configuration>();
		}

		private void Update()
		{
			DoMoving();
			DoScaling();
		}

		public void Build(Figure figure)
		{
			Model = figure;
			ViewOffset = -Vector2.one * (Model.Size - 1) / 2;
			ViewContent = new SpriteRenderer[Model.Size, Model.Size];
			Sprite blockSprite = config.GetBlockSpriteById(Model.BlockId);

			for (int x = 0; x < Model.Size; x++)
			{
				for (int y = 0; y < Model.Size; y++)
				{
					if (Model.Content[x, y] != 1)
						continue;
					
					SpriteRenderer block = objectPool.GetComponent(blockPrefab);
					block.sprite = blockSprite;
					block.transform.parent = transform;
					block.transform.localPosition = new Vector2(x, y) + ViewOffset;
					block.transform.localScale = Vector3.one;
					ViewContent[x, y] = block;
					Blocks.Add(block);
				}
			}
		}
		
		public void Destroy()
		{
			foreach (var block in Blocks)
			{
				block.transform.parent = null;
				block.gameObject.SetActive(false);
			}
			Blocks.Clear();
			gameObject.SetActive(false);
		}

		public void MoveTo(Vector3 position, float speed)
		{
			targetPosition = position;
			targetMovementSpeed = speed;
		}

		public void ScaleTo(float scale, float speed)
		{
			targetScale = scale;
			targetScalingSpeed = speed;
		}

		private void DoMoving()
		{
			if ((targetPosition - transform.position).sqrMagnitude > 0.0001f)
			{
				transform.position = Vector3.Lerp(
					transform.position,
					targetPosition,
					targetMovementSpeed * Time.deltaTime);

				if ((targetPosition - transform.position).sqrMagnitude <= 0.0001f)
					PositionReached?.Invoke();
			}
		}

		private void DoScaling()
		{
			if (Mathf.Abs(targetScale - transform.localScale.x) > 0.01f)
			{
				float scale = Mathf.Lerp(
					transform.localScale.x,
					targetScale,
					targetScalingSpeed * Time.deltaTime);

				transform.localScale = Vector3.one * scale;

				if (scale > 1f)
					foreach (var block in Blocks)
						block.transform.localScale = Vector3.one / scale;
			}
		}
	}
}