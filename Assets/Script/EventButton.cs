using UnityEngine;
using UnityEngine.UI;
public class EventButton : MonoBehaviour {
	public string eventName;
	private EventCenter eventCenter;

	public void Awake () {
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		GetComponent<Button>().onClick.AddListener(OnClick);
	}
	public void OnClick() {
		if(null != eventName) {
			eventCenter.CastEvent(this, eventName, null);
		}
	}
}
