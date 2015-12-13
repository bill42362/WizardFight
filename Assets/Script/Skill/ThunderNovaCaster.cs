using UnityEngine;

public class ThunderNovaCaster : MonoBehaviour {
    /*
	public int skillIndex = 2;
	public string skillName = "Thunder Nova";
	public GameObject owner;
	private CoolDownTimer coolDownTimer;
	private bool isButtonPressed = false;

	public void Awake () {
		coolDownTimer = GetComponent<CoolDownTimer>();
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonDown", gameObject, OnSkillButtonDown);
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonUp", gameObject, OnSkillButtonUp);
		EventManager.Instance.RegisterListener(EventManager.Instance, "playerChange", gameObject, OnPlayerChange);
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
		GameObject novaGameObject = Instantiate(
			Resources.Load("Prefab/Skill/ThunderNova"), position, Quaternion.identity
		) as GameObject;
		ThunderNova nova = novaGameObject.GetComponent<ThunderNova>();
		nova.owner = owner;
	}
    */
}
