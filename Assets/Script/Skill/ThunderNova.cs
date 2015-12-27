using UnityEngine;
public class ThunderNova : MonoBehaviour {
	private double damage = 30;
	private double lifeTime = 1.5;
	private double startTime;
	private bool doneDamage = false;
	private Faction faction;
    private ThunderNovaCaster caster;
    public static GameObject CreateInstance(double time, Vector3 pos, Faction faction, ThunderNovaCaster pCaster)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(
            Resources.Load("Prefab/Skill/ThunderNova"), pos, Quaternion.identity
        );
        obj.GetComponent<Faction>().SetFaction(faction);
        obj.GetComponent<ThunderNova>().caster = pCaster;
        obj.GetComponent<ThunderNova>().startTime = time;
        return obj;
    }
    public void Start()
    {
        faction = GetComponent<Faction>();
    }
    public void Update() {
        double timestamp = PhotonNetwork.time;
        if ((startTime + lifeTime) < timestamp) { Destroy(gameObject); }
	}
	public void OnTriggerStay(Collider other) {
        Role role = other.gameObject.GetComponent<Role>();
        Faction otherFaction = other.gameObject.GetComponent<Faction>();

        if (
            (null != role)
            && (true == otherFaction.IsRival(faction))
            && (false == doneDamage)
            && (role.playerId == GameManager.Instance.PlayerId)
        )
        {
            role.TakeDamageRPC(damage);
            doneDamage = true;
        }
    }
}
