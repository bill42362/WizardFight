using UnityEngine;

public class FireBallCaster : MonoBehaviour {
	public int skillIndex = 0;
	public string skillName = "Fire Ball";
	public GameObject owner;
	public GameObject Owner {
		get {
			if((null == owner) && (null != transform.parent) && (null != transform.parent.parent)) {
				owner = transform.parent.parent.gameObject;
			}
			return owner;
		}
		set { owner = value; }
	}

	private SkillProperties skillProperties;
	private CoolDownTimer coolDownTimer;
	private ChantTimer chantTimer;
	private bool isButtonPressed = false;

	void Awake () {
		skillProperties = GetComponent<SkillProperties>();
		coolDownTimer = GetComponent<CoolDownTimer>();
		chantTimer = GetComponent<ChantTimer>();
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonDown", gameObject, OnSkillButtonDown);
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonUp", gameObject, OnSkillButtonUp);
		EventManager.Instance.RegisterListener(EventManager.Instance, "leftButtonPressed", gameObject, OnPlayerMove);
		EventManager.Instance.RegisterListener(EventManager.Instance, "rightButtonPressed", gameObject, OnPlayerMove);
		EventManager.Instance.RegisterListener(EventManager.Instance, "casting", gameObject, OnCasting);
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
				CastRPC();
			}
		}
	}
	private void OnSkillButtonDown(SbiEvent e) {
		if(GameManager.Instance.GetPlayerCharacter() != Owner) { return; }
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) {
			chantTimer.StopChanting();
			return;
		}
		isButtonPressed = true;
	}
	private void OnSkillButtonUp(SbiEvent e) {
		if(GameManager.Instance.GetPlayerCharacter() != Owner) { return; }
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) return;
		isButtonPressed = false;
	}
	private void OnPlayerMove(SbiEvent e) {
		if(GameManager.Instance.GetPlayerCharacter() != Owner) { return; }
		chantTimer.StopChanting();
		isButtonPressed = false;
	}
	private void CastRPC() {
		if(null == gameObject.transform.parent) { return; }
		SkillHandler handler = gameObject.transform.parent.gameObject.GetComponent<SkillHandler>();
		handler.CastRPC(skillProperties.skillId);
	}
	private void OnCasting(SbiEvent e) {
		CastingEventData data = (CastingEventData)e.data;
		if((Owner != data.role) || (skillProperties.skillId != data.skillId)) {
			return;
		}
        GameObject target = Owner.GetComponent<LookAt>().target;
        Quaternion direction = Quaternion.LookRotation(target.transform.position - Owner.transform.position);

		GameObject fireBallBulletGameObject = (GameObject)GameObject.Instantiate(
			Resources.Load("Prefab/Skill/FireBallBullet"), Owner.transform.position, direction
		);

		fireBallBulletGameObject.GetComponent<Faction>().SetFaction(
			Owner.GetComponent<Faction>().GetFaction()
		);
	}
}
