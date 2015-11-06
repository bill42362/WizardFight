using UnityEngine;
public class Role : MonoBehaviour {
	public double health = 100;
	public double maxHealth = 100;
	public double speed = 10;
	private LookAt lookAt;
	public void Awake () {
        lookAt = GetComponent<LookAt>();
		EventManager.Instance.RegisterListener(EventManager.Instance, "playerChange", gameObject, OnPlayerChange);
	}
	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		lookAt.target = data.player;
	}
	public void TakeDamage(double d) {
		health -= d;
	}
}
