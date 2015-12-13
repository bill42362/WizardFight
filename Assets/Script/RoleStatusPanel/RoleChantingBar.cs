using UnityEngine;
using UnityEngine.UI;

public class RoleChantingBar : MonoBehaviour {
	public bool isChanting = false;
	public GameObject role;
	public Timer chantTimer;
	private Slider slider;
	private Image backgroundImage;
	private Image fillImage;

	public void Awake () {
		EventManager.Instance.RegisterListener(EventManager.Instance, "startChanting", gameObject, OnStartChanting);
		EventManager.Instance.RegisterListener(EventManager.Instance, "stopChanting", gameObject, OnStopChanting);
		slider = GetComponent<Slider>();
		Image[] images = GetComponentsInChildren<Image>();
		for(int i = 0; i < images.Length; ++i) {
			if("Background" == images[i].gameObject.name) { backgroundImage = images[i]; }
			else if("Fill" == images[i].gameObject.name) { fillImage = images[i]; }
		}
	}
	public void Start () { gameObject.SetActive(false); }
	public void Update () {
		if(null != chantTimer) {
			slider.value = (float)chantTimer.GetProgress();
		}
	}

	public void OnStartChanting(SbiEvent e) {
		TimerEventData data = e.data as TimerEventData;
		if(role == data.role) {
			chantTimer = data.timer;
			fillImage.color = chantTimer.gameObject.GetComponent<SkillCasterBase>().buttonColor;
			backgroundImage.color = fillImage.color - new Color(0.5f, 0.5f, 0.5f, 0f);
			gameObject.SetActive(true);
		}
	}
	public void OnStopChanting(SbiEvent e) {
        TimerEventData data = e.data as TimerEventData;
		if(role == data.role) {
			chantTimer = null;
			gameObject.SetActive(false);
		}
	}
}
