using UnityEngine;
using System.Collections;

public class PlayerSkillsReadyEventData : SbiEventData {
    public GameObject[] skillCasters;
    public int[] skillIDs;
    public PlayerSkillsReadyEventData(int[] skillIDs, GameObject[] skillCasters)
    {
        this.skillIDs = skillIDs;
        this.skillCasters = skillCasters;
    }
}
