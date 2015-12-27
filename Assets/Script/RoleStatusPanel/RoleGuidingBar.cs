using UnityEngine;
using UnityEngine.UI;

public class RoleGuidingBar : MonoBehaviour {
	public bool isGuiding = false;
	public GameObject role;
	public Timer guideTimer;
	private Slider slider;
	private Image backgroundImage;
	private Image fillImage;

	public void Awake () {
		EventManager.Instance.RegisterListener(EventManager.Instance, "startGuide", gameObject, OnStartGuiding);
		EventManager.Instance.RegisterListener(EventManager.Instance, "stopGuide", gameObject, OnStopGuiding);
		slider = GetComponent<Slider>();
		Image[] images = GetComponentsInChildren<Image>();
		for(int i = 0; i < images.Length; ++i) {
			if("Background" == images[i].gameObject.name) { backgroundImage = images[i]; }
			else if("Fill" == images[i].gameObject.name) { fillImage = images[i]; }
		}
	}
	public void Start () { gameObject.SetActive(false); }
	public void Update () {
		if(null != guideTimer) {
			slider.value = 1f - (float)guideTimer.GetProgress();
		}
	}

	public void OnStartGuiding(SbiEvent e) {
        TimerEventData data = e.data as TimerEventData;
		if((role == data.role) && (null != data.timer)) {
			guideTimer = data.timer;
			fillImage.color = guideTimer.gameObject.GetComponent<SkillCasterBase>().buttonColor;
			backgroundImage.color = fillImage.color - new Color(0.5f, 0.5f, 0.5f, 0f);
			gameObject.SetActive(true);
		}
	}
	public void OnStopGuiding(SbiEvent e) {
        TimerEventData data = e.data as TimerEventData;
		if((role == data.role) && (null != data.timer)) {
			guideTimer = null;
			gameObject.SetActive(false);
		}
	}
}
