using UnityEngine;

public class FireBallCaster : MonoBehaviour {
	public int skillIndex = 0;
	public string skillName = "Fire Ball";
	public GameObject owner;

	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private EventCenter eventCenter;
	private CoolDownTimer coolDownTimer;
	private ChantTimer chantTimer;
	private bool isButtonPressed = false;

	void Awake () {
		coolDownTimer = GetComponent<CoolDownTimer>();
		chantTimer = GetComponent<ChantTimer>();
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "skillButtonDown", gameObject, OnSkillButtonDown);
		eventCenter.RegisterListener(eventCenter, "skillButtonUp", gameObject, OnSkillButtonUp);
		eventCenter.RegisterListener(eventCenter, "leftButtonPressed", gameObject, OnPlayerMove);
		eventCenter.RegisterListener(eventCenter, "rightButtonPressed", gameObject, OnPlayerMove);
		eventCenter.RegisterListener(eventCenter, "playerChange", gameObject, OnPlayerChange);
	}
	
	void Update () {
		if(false == isButtonPressed) {	
			if(true == chantTimer.isChanting) { chantTimer.StopChanting(); }
			return;
		}
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if(false == chantTimer.isChanting) {
			if(true == coolDownTimer.GetIsCoolDownFinished()) {
				chantTimer.StartChanting();
			}
		} else {
			if(true == chantTimer.GetIsChantingFinished()) {
				coolDownTimer.StartCoolDown();
				chantTimer.StopChanting();
				Cast();
			}
		}
	}
	void OnSkillButtonDown(SbiEvent e) {
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) {
			chantTimer.StopChanting();
			return;
		}
		isButtonPressed = true;
	}
	void OnSkillButtonUp(SbiEvent e) {
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) return;
		isButtonPressed = false;
	}
	void OnPlayerMove(SbiEvent e) {
		chantTimer.StopChanting();
		isButtonPressed = false;
	}
	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		owner = data.player;
		chantTimer.owner = data.player;
	}
	private void Cast() {
		NetworkManager.Instance.Instantiate(
			"Prefab/Skill/FireBallBullet", owner.transform.position, owner.transform.rotation , 0
		);
	}
}
