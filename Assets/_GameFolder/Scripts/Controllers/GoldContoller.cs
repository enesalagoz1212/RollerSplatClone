using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;
using DG.Tweening;

namespace RollerSplatClone.Controllers
{
	public class GoldContoller : MonoBehaviour
	{

		private Vector3 startScale;
		private Tween goldTween;
		void Start()
		{
			startScale = transform.localScale;

		}

		private void OnEnable()
		{
			GoldScale();
			Debug.Log("Gold Tween");
		}

		private void OnDisable()
		{
			StopGoldTween();
			Debug.Log("Ondisable Gold Tween");
		}


		private void GoldScale()
		{
			goldTween = transform.DOScale(startScale * 5f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
			{
				transform.DOScale(startScale*1.5f, 1f).SetEase(Ease.OutQuad);
			});

			goldTween.SetLoops(-1, LoopType.Restart);


		}

		private void StopGoldTween()
		{
			if (goldTween != null && goldTween.IsActive())
			{
				goldTween.Kill();
			}
		}
	}
}

