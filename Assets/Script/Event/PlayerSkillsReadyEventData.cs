using UnityEngine;
using System.Collections;

public class PlayerSkillsReadyEventData : SbiEventData {
    public GameObject player;
    public GameObject[] skillCasters;
    public PlayerSkillsReadyEventData(GameObject p, GameObject[] skillCasters) {
        player = p;
        this.skillCasters = skillCasters;
    }
}
