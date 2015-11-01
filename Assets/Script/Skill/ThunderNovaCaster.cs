using UnityEngine;
using System.Collections;

public class ThunderNovaCaster : MonoBehaviour {
	public int skillIndex = 2;
	public string skillName = "Thunder Nova";
	public GameObject owner;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private EventCenter eventCenter;
	private CoolDownTimer coolDownTimer;
	private bool isButtonPressed = false;
	private double timeStartCooling = 0.0;

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
		int index = System.Convert.ToInt32(e.data);
		if(skillIndex != index) return;
		isButtonPressed = true;
	}
	public void OnSkillButtonUp(SbiEvent e) {
		int index = System.Convert.ToInt32(e.data);
		if(skillIndex != index) return;
		isButtonPressed = false;
	}
	private void Cast() {
		GameObject nova = Instantiate(
			Resources.Load("Skill/ThunderNova"), transform.position, transform.rotation
		) as GameObject;
	}
}