using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoleStatusPanel : MonoBehaviour {
	public string roleChangeEventName = "playerChange";
	public GameObject role;
	private RoleHealthBar roleHealthBar;
	private RoleChantingBar roleChantingBar;
	private RoleGuidingBar roleGuidingBar;
	public void Awake () {
		roleHealthBar = GetComponentInChildren<RoleHealthBar>();
		roleChantingBar = GetComponentInChildren<RoleChantingBar>();
		roleGuidingBar = GetComponentInChildren<RoleGuidingBar>();
		EventManager.Instance.RegisterListener(
			EventManager.Instance, roleChangeEventName, gameObject, OnRoleChange
		);
	}
	public void OnRoleChange(SbiEvent e) {
		PlayerChangeEventData data = e.data as PlayerChangeEventData;
		role = data.player;
		roleHealthBar.role = role.GetComponent<Role>();
		roleChantingBar.role = role;
		roleGuidingBar.role = role;
	}

}
