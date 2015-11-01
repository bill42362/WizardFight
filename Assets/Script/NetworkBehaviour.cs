using UnityEngine;
using System.Collections;

public class NetworkBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Connect() {
        NetworkManager.Instance.Connect();
    }
    public void Match()
    {
        NetworkManager.Instance.Match();
    }
    public void Leave()
    {
        NetworkManager.Instance.LeaveRoom();
    }
}
