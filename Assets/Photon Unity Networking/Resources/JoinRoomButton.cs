using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class JoinRoomButton : Photon.PunBehaviour {
    private GameObject me;
	// Use this for initialization
	public void OnClicked () {
		PhotonNetwork.JoinRoom("myroom");
	}
	
	// Update is called once per frame
	public override void OnJoinedRoom() {
  
		Debug.Log("Join Room Success!!!");
        if (PhotonNetwork.room.playerCount == 1)
        {
            GameObject myPlayer = PhotonNetwork.Instantiate("Player",
                                       new Vector3(0, 0, -5),
                                       Quaternion.identity,
                                       0);
            myPlayer.tag = "Player";
        }
        else {
            GameObject myPlayer = PhotonNetwork.Instantiate("Player",
                           new Vector3(0, 0, 5),
                           Quaternion.identity,
                           0);
            myPlayer.tag = "Player";
        }
	}
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if ( info.photonView.isMine)
        {
            me = info.photonView.gameObject;
        }
        else
        {
            me.GetComponent<LookAt>().target = info.photonView.gameObject;
        }

    }
}
