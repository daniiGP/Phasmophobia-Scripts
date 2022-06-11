using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x02000145 RID: 325
public class RewardManager : MonoBehaviour
{
	// Token: 0x06000928 RID: 2344 RVA: 0x000383FA File Offset: 0x000365FA
	private void Start()
	{
		this.levelDifficulty = (Contract.LevelDifficulty)PlayerPrefs.GetInt("LevelDifficulty");
		this.SetupExperienceReward();
		this.SetupMoneyReward();
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00038418 File Offset: 0x00036618
	private void SetupMoneyReward()
	{
		int num;
		if (PlayerPrefs.GetInt("MissionType") == 0)
		{
			num = 10;
		}
		else if (PlayerPrefs.GetInt("MissionType") == 1)
		{
			num = 30;
		}
		else
		{
			num = 45;
		}
		this.mainMissionRewardText.text = ((PlayerPrefs.GetInt("MainMission") == 1) ? ("$" + num) : "$0");
		this.sideMission1RewardText.text = ((PlayerPrefs.GetInt("SideMission1") == 1) ? "$10" : "$0");
		this.sideMission2RewardText.text = ((PlayerPrefs.GetInt("SideMission2") == 1) ? "$10" : "$0");
		this.hiddenMissionRewardText.text = ((PlayerPrefs.GetInt("SideMission3") == 1) ? "$10" : "$0");
		this.totalPhotosRewardText.text = "$" + this.GetPhotosRewardAmount().ToString();
		this.totalDNARewardText.text = ((PlayerPrefs.GetInt("DNAMission") == 1) ? "$10" : "$0");
		this.ghostTypeText.text = LocalisationSystem.GetLocalisedValue("Reward_Ghost") + " " + PlayerPrefs.GetString("GhostType");
		int num2 = 0;
		num2 += ((PlayerPrefs.GetInt("MainMission") == 1) ? num : 0);
		num2 += ((PlayerPrefs.GetInt("SideMission1") == 1) ? 10 : 0);
		num2 += ((PlayerPrefs.GetInt("SideMission2") == 1) ? 10 : 0);
		num2 += ((PlayerPrefs.GetInt("SideMission3") == 1) ? 10 : 0);
		num2 += this.GetPhotosRewardAmount();
		num2 += ((PlayerPrefs.GetInt("DNAMission") == 1) ? 10 : 0);
		if (this.levelDifficulty == Contract.LevelDifficulty.Intermediate)
		{
			num2 *= 2;
		}
		else if (this.levelDifficulty == Contract.LevelDifficulty.Professional)
		{
			num2 *= 3;
		}
		if (PlayerPrefs.GetInt("PlayerDied") == 1)
		{
			this.insuranceAmountText.text = "-$" + (num2 / 2).ToString();
			num2 /= 2;
		}
		else if (PhotonNetwork.PlayerList.Length == 1)
		{
			if (PlayerPrefs.GetInt("MainMission") == 1)
			{
				this.insuranceAmountText.text = "$10";
				num2 += 10;
			}
		}
		else
		{
			this.insuranceAmountText.text = "$0";
		}
		PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayersMoney") + num2);
		this.totalMissionRewardText.text = "$" + num2.ToString();
		InventoryManager.ResetTemporaryInventory();
		PlayerPrefs.SetInt("PhotosMission", 0);
		PlayerPrefs.SetInt("MainMission", 0);
		PlayerPrefs.SetInt("MissionType", 0);
		PlayerPrefs.SetInt("SideMission1", 0);
		PlayerPrefs.SetInt("SideMission2", 0);
		PlayerPrefs.SetInt("SideMission3", 0);
		PlayerPrefs.SetInt("DNAMission", 0);
		PlayerPrefs.SetInt("PhotosMission", 0);
		this.storeManager.UpdatePlayerMoneyText();
		this.playerStatsManager.UpdateMoney();
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00038700 File Offset: 0x00036900
	private void SetupExperienceReward()
	{
		if (PlayerPrefs.GetInt("PlayerDied") == 1)
		{
			this.mainExperience.SetActive(false);
			this.deadExperience.SetActive(true);
			return;
		}
		int @int = PlayerPrefs.GetInt("myTotalExp");
		int int2 = PlayerPrefs.GetInt("totalExp");
		int num = Mathf.FloorToInt((float)(@int / 100));
		int num2 = num + 1;
		this.currentLevelText.text = num.ToString();
		this.nextLevelText.text = num2.ToString();
		this.experienceGainedText.text = LocalisationSystem.GetLocalisedValue("Experience_Gained") + int2.ToString();
		if (Mathf.FloorToInt((float)((@int - int2) / 100)) < num && int2 > 0)
		{
			this.levelUpText.enabled = true;
			this.levelUpText.text = LocalisationSystem.GetLocalisedValue("Experience_Congrats") + num.ToString();
			this.CheckUnlocks(num);
		}
		else
		{
			this.levelUpText.enabled = false;
		}
		this.expSlider.value = (float)(100 - (num2 * 100 - @int));
		this.expSliderValueText.text = 100 - (num2 * 100 - @int) + "/100 XP".ToString();
		this.playerStatsManager.UpdateLevel();
		this.playerStatsManager.UpdateExperience();
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00038848 File Offset: 0x00036A48
	private int GetPhotosRewardAmount()
	{
		if (PlayerPrefs.GetInt("PhotosMission") == 0)
		{
			return 0;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 50)
		{
			return 10;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 100)
		{
			return 15;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 200)
		{
			return 20;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 300)
		{
			return 25;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 400)
		{
			return 30;
		}
		if (PlayerPrefs.GetInt("PhotosMission") < 500)
		{
			return 35;
		}
		return 40;
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x000388D8 File Offset: 0x00036AD8
	private void CheckUnlocks(int level)
	{
		if (level == 3)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Experience_Medium");
			this.unlock2Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_StrongFlashlight");
			return;
		}
		if (level == 4)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_Thermometer");
			return;
		}
		if (level == 5)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Experience_Large");
			this.unlock2Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_SanityPills");
			return;
		}
		if (level == 6)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_MotionSensor");
			this.unlock2Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_IRLightSensor");
			return;
		}
		if (level == 7)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_SoundSensor");
			this.unlock2Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_ParabolicMicrophone");
			return;
		}
		if (level == 8)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Equipment_HeadMountedCamera");
			return;
		}
		if (level == 10)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Experience_Intermediate");
			return;
		}
		if (level == 15)
		{
			this.unlock1Text.text = LocalisationSystem.GetLocalisedValue("Experience_Unlocked") + LocalisationSystem.GetLocalisedValue("Experience_Professional");
		}
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00038AC0 File Offset: 0x00036CC0
	public void ResumeButton()
	{
		if (PhotonNetwork.InRoom)
		{
			MainManager.instance.serverManager.EnableMasks(true);
			base.gameObject.SetActive(false);
			this.serverSelector.SetSelection();
			return;
		}
		this.mainObject.SetActive(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000943 RID: 2371
	[Header("Main")]
	[SerializeField]
	private GameObject mainObject;

	// Token: 0x04000944 RID: 2372
	[SerializeField]
	private PlayerStatsManager playerStatsManager;

	// Token: 0x04000945 RID: 2373
	[SerializeField]
	private GamepadUISelector serverSelector;

	// Token: 0x04000946 RID: 2374
	[Header("Money Reward")]
	[SerializeField]
	private Text mainMissionRewardText;

	// Token: 0x04000947 RID: 2375
	[SerializeField]
	private Text sideMission1RewardText;

	// Token: 0x04000948 RID: 2376
	[SerializeField]
	private Text sideMission2RewardText;

	// Token: 0x04000949 RID: 2377
	[SerializeField]
	private Text hiddenMissionRewardText;

	// Token: 0x0400094A RID: 2378
	[SerializeField]
	private Text totalPhotosRewardText;

	// Token: 0x0400094B RID: 2379
	[SerializeField]
	private Text totalDNARewardText;

	// Token: 0x0400094C RID: 2380
	[SerializeField]
	private Text totalMissionRewardText;

	// Token: 0x0400094D RID: 2381
	[SerializeField]
	private Text ghostTypeText;

	// Token: 0x0400094E RID: 2382
	[SerializeField]
	private StoreManager storeManager;

	// Token: 0x0400094F RID: 2383
	[SerializeField]
	private Text insuranceAmountText;

	// Token: 0x04000950 RID: 2384
	[Header("Experience Reward")]
	[SerializeField]
	private Text levelUpText;

	// Token: 0x04000951 RID: 2385
	[SerializeField]
	private Text experienceGainedText;

	// Token: 0x04000952 RID: 2386
	[SerializeField]
	private Slider expSlider;

	// Token: 0x04000953 RID: 2387
	[SerializeField]
	private Text expSliderValueText;

	// Token: 0x04000954 RID: 2388
	[SerializeField]
	private Text currentLevelText;

	// Token: 0x04000955 RID: 2389
	[SerializeField]
	private Text nextLevelText;

	// Token: 0x04000956 RID: 2390
	[SerializeField]
	private Text unlock1Text;

	// Token: 0x04000957 RID: 2391
	[SerializeField]
	private Text unlock2Text;

	// Token: 0x04000958 RID: 2392
	[SerializeField]
	private Text unlock3Text;

	// Token: 0x04000959 RID: 2393
	[SerializeField]
	private GameObject mainExperience;

	// Token: 0x0400095A RID: 2394
	[SerializeField]
	private GameObject deadExperience;

	// Token: 0x0400095B RID: 2395
	private Contract.LevelDifficulty levelDifficulty;
}
