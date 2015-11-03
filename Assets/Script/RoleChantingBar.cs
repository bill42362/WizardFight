using UnityEngine;
using UnityEngine.UI;

public class RoleChantingBar : MonoBehaviour {
	public bool isChanting = false;
	public GameObject player;
	public ChantTimer chantTimer;
	private EventCenter eventCenter;
	private Slider slider;

	public void Awake () {
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "startChanting", gameObject, OnStartChanting);
		eventCenter.RegisterListener(eventCenter, "stopChanting", gameObject, OnStopChanting);
		eventCenter.RegisterListener(eventCenter, "playerChange", gameObject, OnPlayerChange);
		slider = GetComponent<Slider>();
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
		}
	}
	public void OnStopChanting(SbiEvent e) {
		ChantingEventData data = e.data as ChantingEventData;
		if(player == data.role) {
			chantTimer = null;
			slider.value = 0;
		}
	}
}
