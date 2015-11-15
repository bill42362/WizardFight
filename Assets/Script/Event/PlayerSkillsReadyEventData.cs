using UnityEngine;
using System.Collections;

public class PlayerSkillsReadyEventData : SbiEventData {
    public GameObject player;
    public GameObject[] skillCasters;
    public int[] skillIDs;
    public PlayerSkillsReadyEventData(GameObject p, int[] skillIDs, GameObject[] skillCasters)
    {
        player = p;
        this.skillIDs = skillIDs;
        this.skillCasters = skillCasters;
    }
}
