using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollerSplatClone.Managers
{
    public class BallPrefsManager
    {
        private const string CurrentLevelKey = "CurrentLevel";
		private const string GoldScorePrefsString = "DiamondScore";
		private const string SoundKey = "IsSoundOn";
		private const string VibrationKey = "IsVibrationOn";
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

		public static bool IsSoundOn
		{
			get
			{
				if (PlayerPrefs.HasKey(SoundKey))
				{
					return bool.Parse(PlayerPrefs.GetString(SoundKey));
				}
				return true;
			}
			set => PlayerPrefs.SetString(SoundKey, value.ToString());
		}

		public static bool IsVibrationOn
		{
			get
			{
				if (PlayerPrefs.HasKey(VibrationKey))
				{
					return bool.Parse(PlayerPrefs.GetString(VibrationKey));
				}
				return true;
			}
			set => PlayerPrefs.SetString(VibrationKey, value.ToString());
		}
	}
}

