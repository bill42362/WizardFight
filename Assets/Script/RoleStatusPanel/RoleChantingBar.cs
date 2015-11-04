using UnityEngine;
using UnityEngine.UI;

public class RoleChantingBar : MonoBehaviour {
	public bool isChanting = false;
	public GameObject player;
	public ChantTimer chantTimer;
	private EventCenter eventCenter;
	private Slider slider;
	private Image backgroundImage;
	private Image fillImage;

	public void Awake () {
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "startChanting", gameObject, OnStartChanting);
		eventCenter.RegisterListener(eventCenter, "stopChanting", gameObject, OnStopChanting);
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
		if(null != chantTimer) {
			slider.value = (float)chantTimer.GetChantingProgress();
		}
	}

	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		player = data.player;
	}
	public void OnStartChanting(SbiEvent e) {
		ChantingEventData data = e.data as ChantingEventData;
		if(player == data.role) {
			chantTimer = data.chantTimer;
			SkillProperties sp = chantTimer.gameObject.GetComponent<SkillProperties>();
			fillImage.color = sp.buttonColor;
			backgroundImage.color = sp.buttonColor - new Color(0.5f, 0.5f, 0.5f, 0f);
			gameObject.SetActive(true);
		}
	}
	public void OnStopChanting(SbiEvent e) {
		ChantingEventData data = e.data as ChantingEventData;
		if(player == data.role) {
			chantTimer = null;
			gameObject.SetActive(false);
		}
	}
}
