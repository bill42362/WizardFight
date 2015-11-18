using UnityEngine;

public class BlizzardCaster : MonoBehaviour {
	public int skillIndex = 1;
	public string skillName = "Blizzard";
	public GameObject owner;
	public GameObject Owner {
		get {
			if(!owner && transform.parent && transform.parent.parent) {
				owner = transform.parent.parent.gameObject;
			}
			return owner;
		}
	}

	private SkillProperties skillProperties;
	private bool isButtonPressed = false;
	private CoolDownTimer coolDownTimer;
	private GuideTimer guideTimer;
	private Blizzard blizzard;

	public void Awake () {
		skillProperties = GetComponent<SkillProperties>();
		coolDownTimer = GetComponent<CoolDownTimer>();
		guideTimer = GetComponent<GuideTimer>();
		EventManager eventManager = EventManager.Instance;
		eventManager.RegisterListener(eventManager, "skillButtonDown", gameObject, OnSkillButtonDown);
		eventManager.RegisterListener(eventManager, "skillButtonUp", gameObject, OnSkillButtonUp);
		eventManager.RegisterListener(eventManager, "leftButtonPressed", gameObject, OnPlayerMove);
		eventManager.RegisterListener(eventManager, "rightButtonPressed", gameObject, OnPlayerMove);
		eventManager.RegisterListener(eventManager, "startGuiding", gameObject, OnStartGuiding);
		eventManager.RegisterListener(eventManager, "stopGuiding", gameObject, OnStopGuiding);
	}
	public void Update () {
		if(GameManager.Instance.GetPlayerCharacter() != owner) { return; }
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
		if(GameManager.Instance.GetPlayerCharacter() != Owner) { return; }
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) {
			if(true == guideTimer.isGuiding) { StopGuidingRPC(); }
			return;
		}
		isButtonPressed = true;
	}
	public void OnSkillButtonUp(SbiEvent e) {
		if(GameManager.Instance.GetPlayerCharacter() != Owner) { return; }
		SkillButtonEventData data = e.data as SkillButtonEventData;
		if(skillIndex != data.index) return;
		isButtonPressed = false;
	}
	public void OnPlayerMove(SbiEvent e) {
		if(GameManager.Instance.GetPlayerCharacter() != Owner) { return; }
		isButtonPressed = false;
		StopGuidingRPC();
	}
	private void StartGuidingRPC() {
		if(null == gameObject.transform.parent) { return; }
		SkillHandler handler = gameObject.transform.parent.gameObject.GetComponent<SkillHandler>();
		handler.StartGuidingRPC(skillProperties.skillId);
	}
	private void StopGuidingRPC() {
		if(null == gameObject.transform.parent) { return; }
		guideTimer.StopGuiding();
		SkillHandler handler = gameObject.transform.parent.gameObject.GetComponent<SkillHandler>();
		handler.StopGuidingRPC(skillProperties.skillId);
	}
	private void OnStartGuiding(SbiEvent e) {
		GuidingEventData data = (GuidingEventData)e.data;
		if((Owner != data.role) || (skillProperties.skillId != data.skillId)) {
			return;
		}
		guideTimer.StartGuiding();

        GameObject target = Owner.GetComponent<LookAt>().target;
		Vector3 targetPosition = transform.position;
		if(null != target) { targetPosition = target.transform.position; }

		if(null == blizzard) {
			GameObject blizzardGameObject = Instantiate(
				Resources.Load("Prefab/Skill/Blizzard"), targetPosition, transform.rotation
			) as GameObject;
			blizzardGameObject.GetComponent<Faction>().SetFaction(Owner.GetComponent<Faction>());
			blizzard = blizzardGameObject.GetComponent<Blizzard>();
			blizzard.owner = Owner;
		} else {
			blizzard.transform.position = targetPosition;
		}
		blizzard.gameObject.SetActive(true);
	}
	private void OnStopGuiding(SbiEvent e) {
		GuidingEventData data = (GuidingEventData)e.data;
		if((Owner != data.role) || (skillProperties.skillId != data.skillId)) {
			return;
		}
		guideTimer.StopGuiding();
		blizzard.gameObject.SetActive(false);
	}
}
