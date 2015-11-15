using UnityEngine;
using UnityEngine.UI;

public class RoleGuidingBar : MonoBehaviour {
	public bool isGuiding = false;
	public GameObject role;
	public GuideTimer guideTimer;
	private Slider slider;
	private Image backgroundImage;
	private Image fillImage;

	public void Awake () {
		EventManager.Instance.RegisterListener(EventManager.Instance, "startGuiding", gameObject, OnStartGuiding);
		EventManager.Instance.RegisterListener(EventManager.Instance, "stopGuiding", gameObject, OnStopGuiding);
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
			slider.value = 1f - (float)guideTimer.GetGuidingProgress();
		}
	}

	public void OnStartGuiding(SbiEvent e) {
		GuidingEventData data = e.data as GuidingEventData;
		if((role == data.role) && (null != data.guideTimer)) {
			guideTimer = data.guideTimer;
			SkillProperties sp = guideTimer.gameObject.GetComponent<SkillProperties>();
			fillImage.color = sp.buttonColor;
			backgroundImage.color = sp.buttonColor - new Color(0.5f, 0.5f, 0.5f, 0f);
			gameObject.SetActive(true);
		}
	}
	public void OnStopGuiding(SbiEvent e) {
		GuidingEventData data = e.data as GuidingEventData;
		if((role == data.role) && (null != data.guideTimer)) {
			guideTimer = null;
			gameObject.SetActive(false);
		}
	}
}
