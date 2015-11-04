using UnityEngine;
using UnityEngine.UI;
public class RoleHealthBar : MonoBehaviour {
	public Role role;
	private EventCenter eventCenter;
	private Slider slider;

	public void Awake () {
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "playerChange", gameObject, OnPlayerChange);
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
