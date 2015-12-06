using UnityEngine;
using System.Collections;

abstract public class SkillCasterBase : Photon.PunBehaviour {

    public SkillCasterBase()
    {
        SetSkillID();
        SetSkillName();
        SetSkillColor();
    }
    public int index { set; get; }
    public int skillID { get; }
    public string skillName { get; }
    public string skillColor { get; }
    private GameObject _owner = null;
    public GameObject owner
    {
        get {
            if ( _owner == null)
                _owner = this.gameObject.transform.parent.gameObject;
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

    protected abstract void SetSkillID();
    protected abstract void SetSkillName();
    protected abstract void SetSkillColor();
    protected abstract void Init();
    protected abstract void OnSkillButtonDown(SbiEvent e);
    protected abstract void OnSkillButtonUp(SbiEvent e);
    protected abstract void OnPlayerMove(SbiEvent e);
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        object[] instantiationData = GetComponent<PhotonView>().instantiationData;
        int ownerID = (int)instantiationData[0];
        index = (int)instantiationData[1];
        GameManager.Instance.SetSkillCaster(ownerID, index, gameObject);
    }
    private void RegisterEssentialListeners()
    {
        EventManager eventManager = EventManager.Instance;
        eventManager.RegisterListener(eventManager, "skillButtonDown", gameObject, OnSkillButtonDown);
        eventManager.RegisterListener(eventManager, "skillButtonUp", gameObject, OnSkillButtonUp);
        eventManager.RegisterListener(eventManager, "leftButtonPressed", gameObject, OnPlayerMove);
        eventManager.RegisterListener(eventManager, "rightButtonPressed", gameObject, OnPlayerMove);
    }
    void Awake()
    {
        RegisterEssentialListeners();
        Init();
    }
    // Use this for initialization
    abstract void Start();

    // Update is called once per frame
    abstract void Update();
}
