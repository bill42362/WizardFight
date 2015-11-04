using UnityEngine;
using UnityEngine.UI;

public class RoleGuidingBar : MonoBehaviour {
	public bool isGuiding = false;
	public GameObject player;
	public GuideTimer guideTimer;
	private EventCenter eventCenter;
	private Slider slider;
	private Image backgroundImage;
	private Image fillImage;

	public void Awake () {
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "startGuiding", gameObject, OnStartGuiding);
		eventCenter.RegisterListener(eventCenter, "stopGuiding", gameObject, OnStopGuiding);
		eventCenter.RegisterListener(eventCenter, "playerChange", gameObject, OnPlayerChange);
		slider = GetComponent<Slider>();
		Image[] images = GetComponentsInChildren<Image>();
		for(int i = 0; i < images.Length; ++i) {
			if("Background" == images[i].gameObject.name) { backgroundImage = images[i]; }
			else if("Fill" == images[i].gameObject.name) { fillImage = images[i]; }
		}
		gameObject.SetActive(false);
	}
	public void Update () {
		if(null != guideTimer) {
			slider.value = 1f - (float)guideTimer.GetGuidingProgress();
		}
	}

	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		player = data.player;
	}
	public void OnStartGuiding(SbiEvent e) {
		GuidingEventData data = e.data as GuidingEventData;
		if(player == data.role) {
			guideTimer = data.guideTimer;
			SkillProperties sp = guideTimer.gameObject.GetComponent<SkillProperties>();
			fillImage.color = sp.buttonColor;
			backgroundImage.color = sp.buttonColor - new Color(0.5f, 0.5f, 0.5f, 0f);
			gameObject.SetActive(true);
		}
	}
	public void OnStopGuiding(SbiEvent e) {
		GuidingEventData data = e.data as GuidingEventData;
		if(player == data.role) {
			guideTimer = null;
			gameObject.SetActive(false);
		}
	}
}
