﻿using UnityEngine;
using System.Collections;

abstract public class SkillCasterBase : Photon.PunBehaviour {

    public SkillCasterBase()
    {
        SetSkillID();
        SetSkillName();
        SetSkillColor();
    }
    public int index { set; get; }
    public bool isControllable = false;
    public int skillID;
    public string skillName;
    public Color buttonColor;
    private GameObject _owner = null;
    public GameObject owner {
        get {
            if ( _owner == null) {
                _owner = this.gameObject.transform.parent.parent.gameObject;
			}
            return _owner;
        }
    }
    public Faction faction
    {
        get { return this.owner.GetComponent<Faction>(); }
    }
    public GameObject target
    {
        get { return this.owner.GetComponent<LookAt>().target;  }
    }
    public Vector3 position
    {
        get { return this.owner.transform.position; }
    }
    public Timer GetTimerByType(string name )
    {
        Timer[] timers = GetComponents<Timer>();
        foreach( Timer timer in timers)
        {
            if (timer.type == name)
                return timer;
        }
        return null;
    }
    protected abstract void SetSkillID();
    protected abstract void SetSkillName();
    protected abstract void SetSkillColor();
    protected abstract void Init();
    protected abstract void OnSkillButtonDown(SbiEvent e);
    protected abstract void OnSkillButtonUp(SbiEvent e);
    protected abstract void OnPlayerMove(SbiEvent e);
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate: skillCasterBase with " + skillName);
        base.OnPhotonInstantiate(info);
        object[] instantiationData = GetComponent<PhotonView>().instantiationData;
        int ownerID = (int)instantiationData[0];
        index = (int)instantiationData[1];
        isControllable = ( ownerID == GameManager.Instance.PlayerId );
        GameManager.Instance.SetSkillCaster(ownerID, index, gameObject);
        if (isControllable) 
            RegisterEssentialListeners();
        Init();
    }
    private void RegisterEssentialListeners()
    {
        EventManager eventManager = EventManager.Instance;
        eventManager.RegisterListener(eventManager, "skillButtonDown", gameObject, OnSkillButtonDown);
        eventManager.RegisterListener(eventManager, "skillButtonUp", gameObject, OnSkillButtonUp);
        eventManager.RegisterListener(eventManager, "leftButtonPressed", gameObject, OnPlayerMove);
        eventManager.RegisterListener(eventManager, "rightButtonPressed", gameObject, OnPlayerMove);
    }

}
