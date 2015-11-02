using UnityEngine;

public class PlayerChangeEventData : SbiEventData {
	public GameObject player;
	public PlayerChangeEventData(GameObject p) { player = p; }
}
