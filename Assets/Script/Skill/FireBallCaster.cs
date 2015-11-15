using UnityEngine;

public class FireBallCaster : MonoBehaviour {
	public int skillIndex = 0;
	public string skillName = "Fire Ball";
	public GameObject owner;

	private SkillProperties skillProperties;
	private CoolDownTimer coolDownTimer;
	private ChantTimer chantTimer;
	private bool isButtonPressed = false;

	void Awake () {
		skillProperties = GetComponent<SkillProperties>();
		coolDownTimer = GetComponent<CoolDownTimer>();
		chantTimer = GetComponent<ChantTimer>();
		owner = GameManager.Instance.GetPlayerCharacter();
		chantTimer.owner = owner;
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonDown", gameObject, OnSkillButtonDown);
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonUp", gameObject, OnSkillButtonUp);
		EventManager.Instance.RegisterListener(EventManager.Instance, "leftButtonPressed", gameObject, OnPlayerMove);
		EventManager.Instance.RegisterListener(EventManager.Instance, "rightButtonPressed", gameObject, OnPlayerMove);
		EventManager.Instance.RegisterListener(EventManager.Instance, "playerChange", gameObject, OnPlayerChange);
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
	private void CastRPC() {
		if(null == gameObject.transform.parent) { return; }
		SkillHandler handler = gameObject.transform.parent.gameObject.GetComponent<SkillHandler>();
		handler.CastRPC(skillProperties.skillId);
	}
	private void OnCasting(SbiEvent e) {
		CastingEventData data = (CastingEventData)e.data;
		if((owner != data.role) || (skillProperties.skillId != data.skillId)) {
			return;
		}
        GameObject target = owner.GetComponent<LookAt>().target;
        Quaternion direction = Quaternion.LookRotation(target.transform.position - owner.transform.position);

		GameObject fireBallBulletGameObject = (GameObject)GameObject.Instantiate(
			Resources.Load("Prefab/Skill/FireBallBullet"), owner.transform.position, direction
		);

		fireBallBulletGameObject.GetComponent<FireBallBullet>().owner = owner;
	}
}
