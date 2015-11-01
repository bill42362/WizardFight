using UnityEngine;
using UnityEngine.UI;

public class RoleChantingBar : MonoBehaviour {
	GameObject caster;
	private EventCenter eventCenter;
	private Slider slider;

	public void Awake () {
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "startChanting", gameObject, OnStartChanting);
		eventCenter.RegisterListener(eventCenter, "stopChanting", gameObject, OnStopChanting);
		slider = GetComponent<Slider>();
	}

	public void OnStartChanting(SbiEvent e) {
		print(e.data);
	}
	public void OnStopChanting(SbiEvent e) {
		print(e.data);
	}
}
