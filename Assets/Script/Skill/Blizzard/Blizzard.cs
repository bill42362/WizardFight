using UnityEngine;
public class Blizzard : MonoBehaviour {
    public static GameObject CreateInstance( Vector3 pos , Faction fac )
    {
        GameObject blizzardGameObject = Instantiate(
            Resources.Load("Prefab/Skill/Blizzard"), pos, Quaternion.identity) as GameObject;
            blizzardGameObject.GetComponent<Faction>().SetFaction(fac);
            Blizzard blizzard = blizzardGameObject.GetComponent<Blizzard>();
        return blizzardGameObject;
    }
	public double damage = 1;
	public double damageCycle = 0.1;
	private double lastDamageTime = 0;
	private Faction faction;

	public void Awake() {
		faction = GetComponent<Faction>();
		lastDamageTime = PhotonNetwork.time;
	}

	public void OnTriggerStay(Collider other) {
		Role role = other.gameObject.GetComponent<Role>();
		Faction otherFaction = other.gameObject.GetComponent<Faction>();
		if(
			(null != role)
			&& (true == otherFaction.IsRival(faction))
            && ( role.playerId == GameManager.Instance.PlayerId )
		) {
            double timestamp = PhotonNetwork.time;
            if (damageCycle <= (timestamp - lastDamageTime)) {
				role.TakeDamageRPC(damage);
				lastDamageTime = timestamp;
			}
		}
	}
}
