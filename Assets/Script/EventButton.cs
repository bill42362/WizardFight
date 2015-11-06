using UnityEngine;
using UnityEngine.UI;
public class EventButton : MonoBehaviour {
	public string eventName;

	public void Awake () {
		GetComponent<Button>().onClick.AddListener(OnClick);
	}
	public void OnClick() {
		if(null != eventName) {
			EventManager.Instance.CastEvent(this, eventName, null);
		}
	}
}
