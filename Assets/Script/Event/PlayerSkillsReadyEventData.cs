using UnityEngine;
using System.Collections;

public class PlayerSkillsReadyEventData : SbiEventData {
    private GameObject[] skillCasters;

    public PlayerSkillsReadyEventData(GameObject[] skillCasters)
    {
        this.skillCasters = skillCasters;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
