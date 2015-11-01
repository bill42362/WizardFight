using UnityEngine;
using System.Collections;

public class FireBallCaster : MonoBehaviour {
	public int skillIndex = 0;
	public string skillName = "Fire Ball";
	public double chantTime = 1000;
	public bool isChanting = false;
	public GameObject owner;

	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private EventCenter eventCenter;
	private CoolDownTimer coolDownTimer;
	public bool isButtonPressed = false;
	private double timeStartChanting = 0;

	void Awake () {
		coolDownTimer = GetComponent<CoolDownTimer>();
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "skillButtonDown", gameObject, OnSkillButtonDown);
		eventCenter.RegisterListener(eventCenter, "skillButtonUp", gameObject, OnSkillButtonUp);
		eventCenter.RegisterListener(eventCenter, "leftButtonPressed", gameObject, OnPlayerMove);
		eventCenter.RegisterListener(eventCenter, "rightButtonPressed", gameObject, OnPlayerMove);
	}
	
	void Update () {
		// FIXME: this assignment should be removed.
		owner = GameObject.FindWithTag("Player");

		if(false == isButtonPressed) {	
			isChanting = false;
			return;
		}
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if(false == isChanting) {
			if(true == coolDownTimer.GetIsCoolDownFinished()) {
				isChanting = true;
				ChantingEventData startData = new ChantingEventData("start", owner, gameObject);
				eventCenter.CastEvent(eventCenter, "startChanting", startData);
				timeStartChanting = timestamp;
			}
		} else {
			if((timeStartChanting + chantTime) < timestamp) {
				coolDownTimer.StartCoolDown();
				isChanting = false;
				ChantingEventData stopData = new ChantingEventData("stop", owner, gameObject);
				eventCenter.CastEvent(eventCenter, "stopChanting", stopData);
				Cast();
			}
		}
	}
	void OnSkillButtonDown(SbiEvent e) {
		int index = System.Convert.ToInt32(e.data);
		if(skillIndex != index) return;
		isButtonPressed = true;
	}
	void OnSkillButtonUp(SbiEvent e) {
		int index = System.Convert.ToInt32(e.data);
		if(skillIndex != index) return;
		isButtonPressed = false;
	}
	void OnPlayerMove(SbiEvent e) {
		isChanting = false;
		isButtonPressed = false;
	}
	private void Cast() {
		NetworkManager.Instance.Instantiate(
			"Skill/FireBallBullet", owner.transform.position, owner.transform.rotation , 0
		);
	}
}
