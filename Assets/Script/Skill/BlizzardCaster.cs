using UnityEngine;

public class BlizzardCaster : MonoBehaviour {
	public int skillIndex = 1;
	public string skillName = "Blizzard";
	public double guidingTime = 10000;
	public bool isGuiding = false;
	public GameObject owner;
	public GameObject enemy;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private EventCenter eventCenter;
	private CoolDownTimer coolDownTimer;
	public bool isButtonPressed = false;
	private double timeStartGuiding = 0;
	private GameObject blizzard;

	public void Awake () {
		coolDownTimer = GetComponent<CoolDownTimer>();
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "skillButtonDown", gameObject, OnSkillButtonDown);
		eventCenter.RegisterListener(eventCenter, "skillButtonUp", gameObject, OnSkillButtonUp);
		eventCenter.RegisterListener(eventCenter, "leftButtonPressed", gameObject, OnPlayerMove);
		eventCenter.RegisterListener(eventCenter, "rightButtonPressed", gameObject, OnPlayerMove);
	}
	public void Update () {
		if(false == isButtonPressed) {	
			if(true == isGuiding) {
				StopGuiding();
				isGuiding = false;
			}
			return;
		}
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if(false == isGuiding) {
			if(true == coolDownTimer.GetIsCoolDownFinished()) {
				isGuiding = true;
				timeStartGuiding = timestamp;
				StartGuiding();
				coolDownTimer.StartCoolDown();
			}
		} else {
			if((timeStartGuiding + guidingTime) < timestamp) {
				isGuiding = false;
				StopGuiding();
			}
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
	public void OnPlayerMove(SbiEvent e) {
		isGuiding = false;
		isButtonPressed = false;
	}
	private void StartGuiding() {
		Vector3 targetPosition = transform.position;
		if(null != enemy) { targetPosition = enemy.transform.position; }
		if(null == blizzard) {
			blizzard = Instantiate(
				Resources.Load("Prefab/Skill/Blizzard"), targetPosition, transform.rotation
			) as GameObject;
		} else {
			blizzard.transform.position = targetPosition;
		}
		blizzard.SetActive(true);
	}
	private void StopGuiding() { blizzard.SetActive(false); }
}
