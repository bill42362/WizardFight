using UnityEngine;

public class TimerEventData : SbiEventData {
	public string type;
	public GameObject role;
	public Timer timer;
	public TimerEventData(string type, GameObject r, Timer t) {
		this.type = type;
		role = r;
		timer = t;
	}
}
