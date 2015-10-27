using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateRoomButton : Photon.PunBehaviour {

	// Use this for initialization
	public void OnClicked () {
		PhotonNetwork.CreateRoom("myroom");
	}
	
	// Update is called once per frame
	public override void OnCreatedRoom() {
		Debug.Log("Create Room Success!!!");
		PhotonNetwork.Instantiate( "Player" ,
								   new Vector3(0,0,-5), 
								   Quaternion.identity, 
								   0 );
	}
}