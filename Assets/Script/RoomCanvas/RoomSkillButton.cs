using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomSkillButton : MonoBehaviour {
	public int skillIndex = 0;
	public int playerId = 1;
	void Awake() {
        EventManager.Instance.RegisterListener(EventManager.Instance, "playerSkillsReady", gameObject, OnPlayerSkillsReady);
	}
    public void OnPlayerSkillsReady(SbiEvent e) {
    }
}
