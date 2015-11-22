using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomSkillButton : MonoBehaviour {
	public int skillIndex = 0;
	public int playerId = 1;
	private SkillProperties skillProperties;
	void Awake() {
        EventManager.Instance.RegisterListener(EventManager.Instance, "playerSkillsReady", gameObject, OnPlayerSkillsReady);
	}
    public void OnPlayerSkillsReady(SbiEvent e) {
        PlayerSkillsReadyEventData data = (PlayerSkillsReadyEventData)e.data;
		Role role = (Role)data.player.GetComponent<Role>();
		if(playerId != role.playerId) { return; }
		GameObject[] playerSkillCasters = data.skillCasters;
		if(playerSkillCasters.Length > skillIndex) {
			skillProperties = playerSkillCasters[skillIndex].GetComponent<SkillProperties>();
		}
		if(null != skillProperties) {
			GetComponentInChildren<Text>().text = skillProperties.skillName;
			name = skillProperties.skillName + " Button";
			GetComponent<Image>().color = skillProperties.buttonColor;
		}
    }
}
