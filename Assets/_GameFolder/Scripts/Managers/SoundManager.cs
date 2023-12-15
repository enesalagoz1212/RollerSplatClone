using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.Managers
{
	public class SoundManager : MonoBehaviour
	{
		public static SoundManager Instance { get; private set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}
		void Start()
		{

		}

		void Update()
		{

		}
	}
}

