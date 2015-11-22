using UnityEngine;
public class Role : MonoBehaviour {
	public int playerId = -1;
	public double health = 100;
	public double maxHealth = 100;
	public double speed = 10;
	private LookAt lookAt;
	private PhotonView photonView;
	public void Awake () {
        lookAt = GetComponent<LookAt>();
        photonView = GetComponent<PhotonView>();
		EventManager.Instance.RegisterListener(EventManager.Instance, "playerChange", gameObject, OnPlayerChange);
	}
	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		lookAt.target = data.player;
	}

	public void TakeDamageRPC(double d) {
		if(!photonView.isMine) { return; }
		photonView.RPC("TakeDamage", PhotonTargets.AllBufferedViaServer, d);	
	}
	[PunRPC]
	public void TakeDamage(double d) {
		health -= d;
		if(0 >= health) {
			DeadEventData deadData = new DeadEventData(gameObject);
			EventManager.Instance.CastEvent(EventManager.Instance, "dead", deadData);
		}
	}
}
