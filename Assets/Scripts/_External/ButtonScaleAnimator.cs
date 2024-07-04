using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RaspberryGames
{
	public class ButtonScaleAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		[Range(0.5f, 1.5f)]
		[SerializeField] private float hoverScale = 1.1f;
		
		[Range(0.5f, 1.5f)]
		[SerializeField] private float pressedScale = 0.9f;
		
		[Range(0.1f, 2f)]
		[SerializeField] private float duration = 0.3f;

		public void OnPointerEnter(PointerEventData eventData) => SetTargetScale(hoverScale);
		public void OnPointerExit(PointerEventData eventData) => SetTargetScale(1f);
		public void OnPointerDown(PointerEventData eventData) => SetTargetScale(pressedScale);
		public void OnPointerUp(PointerEventData eventData) => SetTargetScale(1f);

		private void SetTargetScale(float scale)
		{
			StopAllCoroutines();
			StartCoroutine(DoScale(scale));
		}
		
		private IEnumerator DoScale(float scale)
		{
			while (transform.localScale.x != scale)
			{
				transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * scale, Time.deltaTime / duration);
				yield return null;
			}
		}
	}
}