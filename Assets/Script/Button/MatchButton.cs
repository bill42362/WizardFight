using UnityEngine;
using UnityEngine.UI;

public class MatchButton : MonoBehaviour {
	public NetworkManager networkManager;
	public void Awake () {
		GetComponent<Button>().onClick.AddListener(OnClick);
		if(null == GetComponent<EventButton>()) {
			gameObject.AddComponent<EventButton>();
			GetComponent<EventButton>().eventName = "matchButtonClick";
		}
	}
	public void OnClick() {
		networkManager = NetworkManager.Instance;
		EventManager.Instance.RegisterListener(
			NetworkManager.Instance, "joinedRoom", gameObject, OnJoined
		);
	}
	public void OnJoined(SbiEvent e) {
		GetComponentInChildren<Text>().text = "Leave";
		GetComponentInChildren<EventButton>().eventName = "leaveButtonClick";
	}
}
