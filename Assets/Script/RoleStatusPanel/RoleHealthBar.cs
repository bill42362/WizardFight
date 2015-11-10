using UnityEngine;
using UnityEngine.UI;
public class RoleHealthBar : MonoBehaviour {
	public Role role;
	private Slider slider;
	public void Awake () {
		slider = GetComponent<Slider>();
	}
	public void Update () {
		if(null != role) {
			slider.value = (float)(role.health/role.maxHealth);
		}
	}
}
