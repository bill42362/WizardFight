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
	private void Cast() {
		GameObject nova = Instantiate(
			Resources.Load("Prefab/Skill/ThunderNova"), transform.position, transform.rotation
		) as GameObject;
	}
}
