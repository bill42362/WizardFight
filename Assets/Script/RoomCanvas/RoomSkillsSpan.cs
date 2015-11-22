using UnityEngine;
using System.Collections;

public class RoomSkillsSpan : MonoBehaviour {
	public int playerId = 1;
	void Awake() {
		RoomSkillButton[] buttons = GetComponentsInChildren<RoomSkillButton>();
		foreach(RoomSkillButton button in buttons) {
			button.playerId = playerId;
		}
	}
}
