using UnityEngine;

public class BlizzardCaster : MonoBehaviour {
	public int skillIndex = 1;
	public string skillName = "Blizzard";
	public GameObject owner;
	public GameObject enemy;

	private SkillProperties skillProperties;
	private bool isButtonPressed = false;
	private CoolDownTimer coolDownTimer;
	private GuideTimer guideTimer;
	private Blizzard blizzard;

	public void Awake () {
		skillProperties = GetComponent<SkillProperties>();
		coolDownTimer = GetComponent<CoolDownTimer>();
		guideTimer = GetComponent<GuideTimer>();
		owner = GameManager.Instance.GetPlayerCharacter();
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonDown", gameObject, OnSkillButtonDown);
		EventManager.Instance.RegisterListener(EventManager.Instance, "skillButtonUp", gameObject, OnSkillButtonUp);
		EventManager.Instance.RegisterListener(EventManager.Instance, "leftButtonPressed", gameObject, OnPlayerMove);
		EventManager.Instance.RegisterListener(EventManager.Instance, "rightButtonPressed", gameObject, OnPlayerMove);
		EventManager.Instance.RegisterListener(EventManager.Instance, "playerChange", gameObject, OnPlayerChange);
		EventManager.Instance.RegisterListener(EventManager.Instance, "enemyChange", gameObject, OnEnemyChange);
		EventManager.Instance.RegisterListener(EventManager.Instance, "startGuiding", gameObject, OnStartGuiding);
		EventManager.Instance.RegisterListener(EventManager.Instance, "stopGuiding", gameObject, OnStopGuiding);
	}
	public void Update () {
		if(false == isButtonPressed) {	
			if(true == guideTimer.isGuiding) {
				StopGuidingRPC();
			}
			return;
		}
		if(false == guideTimer.isGuiding) {
			if(true == coolDownTimer.GetIsCoolDownFinished()) {
				coolDownTimer.StartCoolDown();
				StartGuidingRPC();
			}
		} else {
			if(true == guideTimer.GetIsGuidingFinished()) {
				StopGuidingRPC();
			}
		}
	}
	public void OnSkillButtonDown(SbiEvent e) {
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) {
			if(true == guideTimer.isGuiding) { StopGuidingRPC(); }
			return;
		}
		isButtonPressed = true;
	}
	public void OnSkillButtonUp(SbiEvent e) {
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) return;
		isButtonPressed = false;
	}
	public void OnPlayerMove(SbiEvent e) {
		isButtonPressed = false;
		StopGuidingRPC();
	}
	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		owner = data.player;
		guideTimer.owner = data.player;
		if(null != blizzard) { blizzard.owner = data.player; }
	}
	public void OnEnemyChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		enemy = data.player;
	}
	private void StartGuidingRPC() {
		if(null == gameObject.transform.parent) { return; }
		SkillHandler handler = gameObject.transform.parent.gameObject.GetComponent<SkillHandler>();
		handler.StartGuiding(skillProperties.skillId);
	}
	private void StopGuidingRPC() {
		if(null == gameObject.transform.parent) { return; }
		SkillHandler handler = gameObject.transform.parent.gameObject.GetComponent<SkillHandler>();
		handler.StopGuidingRPC(skillProperties.skillId);
	}
	private void OnStartGuiding(SbiEvent e) {
		GuidingEventData data = (GuidingEventData)e.data;
		if((owner != data.role) || (skillProperties.skillId != data.skillId)) {
			return;
		}
		guideTimer.StartGuiding();

		Vector3 targetPosition = transform.position;
		if(null != owner) { targetPosition = owner.transform.position; }
		if(null != enemy) { targetPosition = enemy.transform.position; }

		if(null == blizzard) {
			GameObject blizzardGameObject = Instantiate(
				Resources.Load("Prefab/Skill/Blizzard"), targetPosition, transform.rotation
			) as GameObject;
			blizzard = blizzardGameObject.GetComponent<Blizzard>();
			blizzard.owner = owner;
		} else {
			blizzard.transform.position = targetPosition;
		}
		blizzard.gameObject.SetActive(true);
	}
	private void OnStopGuiding(SbiEvent e) {
		GuidingEventData data = (GuidingEventData)e.data;
		if((owner != data.role) || (skillProperties.skillId != data.skillId)) {
			return;
		}
		guideTimer.StopGuiding();
		blizzard.gameObject.SetActive(false);
	}
}
