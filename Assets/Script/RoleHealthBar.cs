using UnityEngine;
using UnityEngine.UI;
public class RoleHealthBar : MonoBehaviour {
	public Role role;
	private EventCenter eventCenter;
	private Slider slider;

	public void Awake () {
		eventCenter = GameObject.FindWithTag("EventCenter").GetComponent<EventCenter>();
		eventCenter.RegisterListener(eventCenter, "playerChanged", gameObject, OnPlayerChanged);
		slider = GetComponent<Slider>();
	}
	public void Update () {
		if(null != role) {
			slider.value = (float)(role.health/role.maxHealth);
		}
	}
	public void OnPlayerChanged(SbiEvent e) {
		Debug.Log("playerChangeEvent");
	}
}
