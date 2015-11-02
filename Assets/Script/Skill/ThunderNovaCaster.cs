using UnityEngine;

public class ThunderNovaCaster : MonoBehaviour {
	public int skillIndex = 2;
	public string skillName = "Thunder Nova";
	public GameObject owner;
	private EventCenter eventCenter;
	private CoolDownTimer coolDownTimer;
	private bool isButtonPressed = false;

	public void Awake () {
		coolDownTimer = GetComponent<CoolDownTimer>();
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "skillButtonDown", gameObject, OnSkillButtonDown);
		eventCenter.RegisterListener(eventCenter, "skillButtonUp", gameObject, OnSkillButtonUp);
		eventCenter.RegisterListener(eventCenter, "playerChange", gameObject, OnPlayerChange);
	}
	public void Update () {
		if(
			(true == isButtonPressed)
			&& (true == coolDownTimer.GetIsCoolDownFinished())
		) {
			coolDownTimer.StartCoolDown();
			Cast();
		}
	}
	public void OnSkillButtonDown(SbiEvent e) {
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) return;
		isButtonPressed = true;
	}
	public void OnSkillButtonUp(SbiEvent e) {
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) return;
		isButtonPressed = false;
	}
	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		owner = data.player;
	}
	private void Cast() {
		Vector3 position = transform.position;
		if(null != owner) { position = owner.transform.position; }
		GameObject nova = Instantiate(
			Resources.Load("Prefab/Skill/ThunderNova"), position, Quaternion.identity
		) as GameObject;
	}
}
