using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RollerSplatClone.Managers;

namespace RollerSplatClone.Controllers
{
    public class GoldController : MonoBehaviour
    {

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Ball"))
			{
				GameManager.Instance.IncreaseGoldScore(1);
				Destroy(gameObject);
			}
		}

	}

}
