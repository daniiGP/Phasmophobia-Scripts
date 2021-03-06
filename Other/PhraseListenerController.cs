using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PhraseListenerController : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		GameController.instance.OnGhostSpawned.AddListener(new UnityAction(this.SetupKeywords));
	}

	public void SetupKeywords()
	{
		Question question = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Phrase_1"),
				LocalisationSystem.GetLocalisedValue("Phrase_2"),
				LocalisationSystem.GetLocalisedValue("Phrase_3"),
				LocalisationSystem.GetLocalisedValue("Phrase_4"),
				LocalisationSystem.GetLocalisedValue("Phrase_5"),
				LocalisationSystem.GetLocalisedValue("Phrase_6"),
				LocalisationSystem.GetLocalisedValue("Phrase_7"),
				LocalisationSystem.GetLocalisedValue("Phrase_8"),
				LocalisationSystem.GetLocalisedValue("Phrase_9"),
				LocalisationSystem.GetLocalisedValue("Phrase_10"),
				LocalisationSystem.GetLocalisedValue("Phrase_11"),
				LocalisationSystem.GetLocalisedValue("Phrase_12"),
				LocalisationSystem.GetLocalisedValue("Phrase_13"),
				LocalisationSystem.GetLocalisedValue("Phrase_14"),
				LocalisationSystem.GetLocalisedValue("Phrase_15"),
				LocalisationSystem.GetLocalisedValue("Phrase_16"),
				LocalisationSystem.GetLocalisedValue("Phrase_17"),
				LocalisationSystem.GetLocalisedValue("Phrase_18"),
				LocalisationSystem.GetLocalisedValue("Phrase_19"),
				LocalisationSystem.GetLocalisedValue("Phrase_20"),
				LocalisationSystem.GetLocalisedValue("Phrase_21"),
				LocalisationSystem.GetLocalisedValue("Phrase_22"),
				LocalisationSystem.GetLocalisedValue("Phrase_23"),
				LocalisationSystem.GetLocalisedValue("Phrase_24"),
				LocalisationSystem.GetLocalisedValue("Phrase_25"),
				LocalisationSystem.GetLocalisedValue("Phrase_26"),
				LocalisationSystem.GetLocalisedValue("Phrase_27"),
				LocalisationSystem.GetLocalisedValue("Phrase_28"),
				LocalisationSystem.GetLocalisedValue("Phrase_29"),
				LocalisationSystem.GetLocalisedValue("Phrase_30"),
				LocalisationSystem.GetLocalisedValue("Phrase_31"),
				LocalisationSystem.GetLocalisedValue("Phrase_32"),
				LocalisationSystem.GetLocalisedValue("Phrase_33"),
				LocalisationSystem.GetLocalisedValue("Phrase_34"),
				LocalisationSystem.GetLocalisedValue("Phrase_35"),
				LocalisationSystem.GetLocalisedValue("Phrase_36"),
				LocalisationSystem.GetLocalisedValue("Phrase_37"),
				LocalisationSystem.GetLocalisedValue("Phrase_38"),
				LocalisationSystem.GetLocalisedValue("Phrase_39"),
				LocalisationSystem.GetLocalisedValue("Phrase_40"),
				LocalisationSystem.GetLocalisedValue("Phrase_41"),
				LocalisationSystem.GetLocalisedValue("Phrase_42"),
				LocalisationSystem.GetLocalisedValue("Phrase_43"),
				LocalisationSystem.GetLocalisedValue("Phrase_44"),
				LocalisationSystem.GetLocalisedValue("Phrase_45"),
				LocalisationSystem.GetLocalisedValue("Phrase_46"),
				LocalisationSystem.GetLocalisedValue("Phrase_47"),
				LocalisationSystem.GetLocalisedValue("Phrase_48"),
				LocalisationSystem.GetLocalisedValue("Phrase_49"),
				LocalisationSystem.GetLocalisedValue("Phrase_50"),
				LocalisationSystem.GetLocalisedValue("Phrase_51"),
				LocalisationSystem.GetLocalisedValue("Phrase_52"),
				LocalisationSystem.GetLocalisedValue("Phrase_53"),
				LocalisationSystem.GetLocalisedValue("Phrase_54"),
				LocalisationSystem.GetLocalisedValue("Phrase_55"),
				LocalisationSystem.GetLocalisedValue("Phrase_56"),
				LocalisationSystem.GetLocalisedValue("Phrase_57")
			},
			questionType = Question.QuestionType.none
		};
		question.questions.Add(LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostName.Split(new char[]
		{
			' '
		})[0]);
		this.questions.Add(question);
		for (int i = 0; i < this.questions.Count; i++)
		{
			for (int j = 0; j < this.questions[i].questions.Count; j++)
			{
				SpeechRecognitionController.instance.AddKeyword(this.questions[i].questions[j]);
			}
		}
	}

	public void OnPhraseRecognized(string args)
	{
		if (LevelController.instance.currentPlayerRoom != LevelController.instance.currentGhostRoom)
		{
			return;
		}
		if (LevelController.instance.currentGhostRoom.currentPlayerInRoomTimer < 2f)
		{
			return;
		}
		if (LevelController.instance.currentGhostRoom.playersInRoom.Count > 1 && LevelController.instance.currentGhost.ghostInfo.ghostTraits.isShy)
		{
			return;
		}
		if (args == LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostName.Split(new char[]
		{
			' '
		})[0])
		{
			SpeechRecognitionController.instance.hasSaidGhostsName = true;
		}
		for (int i = 0; i < this.questions.Count; i++)
		{
			for (int j = 0; j < this.questions[i].questions.Count; j++)
			{
				if (args == this.questions[i].questions[j])
				{
					this.Answer();
				}
			}
		}
		for (int k = 0; k < LevelController.instance.peopleInHouse.Count; k++)
		{
			if (args == LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType.ToString())
			{
				this.Answer();
			}
		}
	}

	private void Update()
	{
		if (SpeechRecognitionController.instance.hasSaidGhostsName)
		{
			this.ghostsNameReactionTimer -= Time.deltaTime;
			if (this.ghostsNameReactionTimer < 0f)
			{
				SpeechRecognitionController.instance.hasSaidGhostsName = false;
				this.ghostsNameReactionTimer = 20f;
			}
		}
	}

	private void Answer()
	{
		this.view.RPC("NetworkedReaction", PhotonNetwork.MasterClient, Array.Empty<object>());
	}

	[PunRPC]
	private void NetworkedReaction()
	{
		LevelController.instance.currentGhost.ghostInfo.activityMultiplier += (float)UnityEngine.Random.Range(10, 25);
		if (UnityEngine.Random.Range(0, 200) == 1)
		{
			LevelController.instance.currentGhost.ChangeState(GhostAI.States.fusebox, null, null);
		}
		int num = UnityEngine.Random.Range(0, 8);
		if (num == 0)
		{
			LevelController.instance.currentGhost.ghostActivity.InteractWithARandomProp();
			return;
		}
		if (num == 1)
		{
			LevelController.instance.currentGhost.ghostActivity.InteractWithARandomDoor();
		}
	}

	[SerializeField]
	private List<Question> questions = new List<Question>();

	private PhotonView view;

	private float ghostsNameReactionTimer = 20f;
}

