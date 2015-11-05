using UnityEngine;

public class BlizzardCaster : MonoBehaviour {
	public int skillIndex = 1;
	public string skillName = "Blizzard";
	public GameObject owner;
	public GameObject enemy;

	private bool isButtonPressed = false;
	private EventCenter eventCenter;
	private CoolDownTimer coolDownTimer;
	private GuideTimer guideTimer;
	private Blizzard blizzard;

	public void Awake () {
		coolDownTimer = GetComponent<CoolDownTimer>();
		guideTimer = GetComponent<GuideTimer>();
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "skillButtonDown", gameObject, OnSkillButtonDown);
		eventCenter.RegisterListener(eventCenter, "skillButtonUp", gameObject, OnSkillButtonUp);
		eventCenter.RegisterListener(eventCenter, "leftButtonPressed", gameObject, OnPlayerMove);
		eventCenter.RegisterListener(eventCenter, "rightButtonPressed", gameObject, OnPlayerMove);
		eventCenter.RegisterListener(eventCenter, "playerChange", gameObject, OnPlayerChange);
	}
	public void Update () {
		if(false == isButtonPressed) {	
			if(true == guideTimer.isGuiding) {
				StopGuiding();
			}
			return;
		}
		if(false == guideTimer.isGuiding) {
			if(true == coolDownTimer.GetIsCoolDownFinished()) {
				coolDownTimer.StartCoolDown();
				StartGuiding();
			}
		} else {
			if(true == guideTimer.GetIsGuidingFinished()) {
				StopGuiding();
			}
		}
	}
	public void OnSkillButtonDown(SbiEvent e) {
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) {
			if(true == guideTimer.isGuiding) { StopGuiding(); }
			return;
		}
		isButtonPressed = true;
	}
	public void OnSkillButtonUp(SbiEvent e) {
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) return;
		isButtonPressed = false;
	}
	public void OnPlayerMove(SbiEvent e) {
		isButtonPressed = false;
		StopGuiding();
	}
	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		owner = data.player;
		guideTimer.owner = data.player;
		if(null != blizzard) { blizzard.owner = data.player; }
	}
	private void StartGuiding() {
		guideTimer.StartGuiding();

		Vector3 targetPosition = transform.position;
		if(null != owner) { targetPosition = owner.transform.position; }
		else if(null != enemy) { targetPosition = enemy.transform.position; }

		if(null == blizzard) {
			GameObject blizzardGameObject = Instantiate(
				Resources.Load("Prefab/Skill/Blizzard"), targetPosition, transform.rotation
			) as GameObject;
			blizzard = blizzardGameObject.GetComponent<Blizzard>();
			blizzard.owner = owner;
		} else {
			blizzard.transform.position = targetPosition;
		}
		blizzard.gameObject.SetActive(true);
	}
	private void StopGuiding() {
		guideTimer.StopGuiding();
		blizzard.gameObject.SetActive(false);
	}
}
