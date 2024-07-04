using System;
using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
	public class FigureController : MonoBehaviour
	{
		[SerializeField] private FigureSpawner spawner;
		[SerializeField] private BoardPresenter boardPresenter;

		private Configuration config;
		private Camera mainCamera;
		private LayerMask figuresLayer;
		private Vector3 draggingOffset = Vector3.zero;
		private AudioManager audioManager;
		private FigurePresenter draggingFigure;
		private Vector2Int prevLocalPosition = -Vector2Int.one;

		private Vector3 mousePosition => 
			mainCamera.ScreenToWorldPoint(Input.mousePosition);
		private Vector2Int localBoardFigurePosition => 
			boardPresenter.GlobalToLocalPosition((Vector2)draggingFigure.transform.position + draggingFigure.ViewOffset);

		private void Awake()
		{
			config = ServiceLocator.Get<Configuration>();
			audioManager = ServiceLocator.Get<AudioManager>();
			mainCamera = Camera.main;
			figuresLayer = LayerMask.GetMask("Figures");
			//boardPresenter.Prepared += earnedPoints => score.Value += earnedPoints;
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
				GrabFigure();

			else if (Input.GetMouseButton(0))
				MoveFigure();

			else if (Input.GetMouseButtonUp(0))
				ReleaseFigure();
		}

		private void GrabFigure()
		{
			Collider2D figureCollider = Physics2D.OverlapPoint(mousePosition, figuresLayer);

			if (figureCollider != null)
				draggingFigure = figureCollider.GetComponent<FigurePresenter>();

			if (draggingFigure == null)
				return;

			draggingOffset = draggingFigure.transform.position - mousePosition + Vector3.back;
			draggingFigure.ScaleTo(
				config.FigureScaleWhenDragging,
				config.FigurePointerFollowingSpeed);

			audioManager.PlaySound(config.FigureGrabSound);
		}

		private void MoveFigure()
		{
			if (draggingFigure == null)
				return;

			draggingFigure.MoveTo(
				mousePosition + draggingOffset, 
				config.FigurePointerFollowingSpeed);

			Vector2Int localPosition = localBoardFigurePosition;
			
			if (localPosition == prevLocalPosition)
				return;
			
			boardPresenter.CheckPlaceForFigure(
				localPosition.x,
				localPosition.y,
				draggingFigure.Model
			);
			
			prevLocalPosition = localPosition;
		}

		private void ReleaseFigure()
		{
			if (draggingFigure == null)
				return;

			Vector2Int localPosition = localBoardFigurePosition;

			if (boardPresenter.TryPlaceFigureTo(localPosition.x, localPosition.y, draggingFigure.Model))
			{
				spawner.MakeSpace(draggingFigure);
				spawner.TrySpawn();
			
				draggingFigure.Destroy();
				draggingFigure = null;
				
				boardPresenter.UpdateView();
				boardPresenter.ClearFilledLines();
				
				// важно здесь после удаления линий
				spawner.CheckForFreeSpace();

				audioManager.PlaySound(config.FigurePlaceSound);
			}
			else
			{
				draggingFigure.ScaleTo(1f, config.FigureReturnSpeed);
				draggingFigure.MoveTo(draggingFigure.InitialPosition, config.FigureReturnSpeed);
				draggingFigure.ScaleTo(0.5f, config.FigureReturnSpeed);
				draggingFigure = null;
				audioManager.PlaySound(config.FigureReturnSound);
			}
		}
	}
}