using UnityEngine;
using System.Collections;

public class CasterReadyEventData : SbiEventData {
    public GameObject player;
    public int skillIndex;
    public GameObject skillCaster;
    public CasterReadyEventData(GameObject p, int index, GameObject caster) {
        player = p;
        skillIndex = index;
        skillCaster = caster;
    }
}
