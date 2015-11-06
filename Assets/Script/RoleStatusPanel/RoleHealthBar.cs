using UnityEngine;
using UnityEngine.UI;
public class RoleHealthBar : MonoBehaviour {
	public Role role;
	private Slider slider;

	public void Awake () {
		EventManager.Instance.RegisterListener(EventManager.Instance, "playerChange", gameObject, OnPlayerChange);
		slider = GetComponent<Slider>();
	}
	public void Update () {
		if(null != role) {
			slider.value = (float)(role.health/role.maxHealth);
		}
	}
	public void OnPlayerChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		role = data.player.GetComponent<Role>();
	}
}
