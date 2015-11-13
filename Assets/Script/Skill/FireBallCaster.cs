﻿using UnityEngine;

public class FireBallCaster : MonoBehaviour {
	public int skillIndex = 0;
	public string skillName = "Fire Ball";
	public GameObject owner;

	private CoolDownTimer coolDownTimer;
	private ChantTimer chantTimer;
	private bool isButtonPressed = false;

	void Awake () {
		coolDownTimer = GetComponent<CoolDownTimer>();
		chantTimer = GetComponent<ChantTimer>();
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonDown", gameObject, OnSkillButtonDown);
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonUp", gameObject, OnSkillButtonUp);
		EventManager.Instance.RegisterListener(EventManager.Instance, "leftButtonPressed", gameObject, OnPlayerMove);
		EventManager.Instance.RegisterListener(EventManager.Instance, "rightButtonPressed", gameObject, OnPlayerMove);
		EventManager.Instance.RegisterListener(EventManager.Instance, "playerChange", gameObject, OnPlayerChange);
	}
	
	void Update () {
		if(false == isButtonPressed) {	
			if(true == chantTimer.isChanting) { chantTimer.StopChanting(); }
			return;
		}
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
        //CastingEventData castingData = new CastingEventData(owner);
        //EventManager.Instance.CastEvent(EventManager.Instance, "casting", castingData);
        GameObject target = owner.GetComponent<LookAt>().target;
        Quaternion direction = Quaternion.LookRotation(target.transform.position - owner.transform.position);

		FireBallBullet fireBallBullet = NetworkManager.Instance.Instantiate(
			"Prefab/Skill/FireBallBullet", owner.transform.position, direction, 0
		).GetComponent<FireBallBullet>();

		fireBallBullet.owner = owner;
	}
}
