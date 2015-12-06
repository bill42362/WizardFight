using UnityEngine;
using System.Collections;

public class PlayerSkillReadyEventData : SbiEventData {
    public GameObject player;
    public int skillIndex;
    public GameObject skillCaster;
    public PlayerSkillsReadyEventData(GameObject p, int index, GameObject caster) {
        player = p;
        skillIndex = index;
        skillCaster = caster;
    }
}
