using UnityEngine;
public class Role : MonoBehaviour {
	public double health = 100;
	public double maxHealth = 100;
	public double speed = 10;
	private LookAt lookAt;
	private EventCenter eventCenter;
	public void Awake () {
        lookAt = GetComponent<LookAt>();
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "playerChange", gameObject, OnPlayerChange);
	}
	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		lookAt.target = data.player;
	}
}
