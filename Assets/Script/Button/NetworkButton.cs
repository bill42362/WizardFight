using UnityEngine;
using UnityEngine.UI;
public class NetworkButton : MonoBehaviour {
	public NetworkManager networkManager;
	public void Awake () {
		GetComponent<Button>().onClick.AddListener(OnClick);
	}
	public void OnClick() {
		networkManager = NetworkManager.Instance;
	}
}
