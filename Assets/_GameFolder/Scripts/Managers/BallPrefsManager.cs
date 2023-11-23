using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.Managers
{
    public class BallPrefsManager
    {
        private const string CurrentLevelKey = "CurrentLevel";
		private const string GoldScorePrefsString = "DiamondScore";

		public static int CurrentLevel
		{
			get
			{
				return PlayerPrefs.GetInt(CurrentLevelKey, 1);
			}
			set
			{
				PlayerPrefs.SetInt(CurrentLevelKey, value);
			}
		}

		public static int GoldScore
		{
			get
			{
				return PlayerPrefs.GetInt(GoldScorePrefsString);
			}
			set
			{
				PlayerPrefs.SetInt(GoldScorePrefsString, value);
			}
		}

	}
}

