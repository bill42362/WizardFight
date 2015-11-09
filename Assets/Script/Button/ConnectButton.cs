using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour {
	public NetworkManager networkManager;
	public void Awake () {
		GetComponent<Button>().onClick.AddListener(OnClick);
	}
	public void OnClick() {
		networkManager = NetworkManager.Instance;
		EventManager.Instance.RegisterListener(
			NetworkManager.Instance, "connectedToPhoton", gameObject, OnConnected
		);
	}
	public void OnConnected(SbiEvent e) {
		GetComponentInChildren<Text>().text = "Disconnect";
		GetComponentInChildren<EventButton>().eventName = "disconnectButtonClick";
	}
}
