using UnityEngine;
using UnityEngine.UI;

public class RoleChantingBar : MonoBehaviour {
	public bool isChanting = false;
	public GameObject player;
	public GameObject caster;
	private EventCenter eventCenter;
	private Slider slider;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double timeStartChanting = 0.0;
	private double chantTime = 0.0;

	public void Awake () {
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "startChanting", gameObject, OnStartChanting);
		eventCenter.RegisterListener(eventCenter, "stopChanting", gameObject, OnStopChanting);
		eventCenter.RegisterListener(eventCenter, "playerChange", gameObject, OnPlayerChange);
		slider = GetComponent<Slider>();
	}
	public void Update () {
		if(true == isChanting) {
			double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
			slider.value = (float)((timestamp - timeStartChanting)/chantTime);
		}
	}

	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		player = data.player;
	}
	public void OnStartChanting(SbiEvent e) {
		ChantingEventData data = e.data as ChantingEventData;
		if(player == data.role) {
			caster = data.caster;
			chantTime = data.chantTime;
			timeStartChanting = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
			isChanting = true;
		}
	}
	public void OnStopChanting(SbiEvent e) {
		ChantingEventData data = e.data as ChantingEventData;
		if(player == data.role) {
			caster = null;
			slider.value = 0;
			isChanting = false;
		}
	}
}
