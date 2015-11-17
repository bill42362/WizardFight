using System;
using System.Collections;
using UnityEngine;
public class NetworkManager : Photon.PunBehaviour {
    public bool isOffline
    {
        get
        {
            return PhotonNetwork.offlineMode;
        }
    }
    private int readyCount = 0; 
    private static NetworkManager _instance = null;
    protected NetworkManager() {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.autoCleanUpPlayerObjects = true;
    }
    private void clear()
    {
        readyCount = 0;
    }
	public void Start () {
		EventManager.Instance.RegisterListener(
			EventManager.Instance, "connectButtonClick", gameObject, Connect
		);
		EventManager.Instance.RegisterListener(
			EventManager.Instance, "matchButtonClick", gameObject, Match
		);
		EventManager.Instance.RegisterListener(
			EventManager.Instance, "leaveButtonClick", gameObject, LeaveRoom
		);
	}
    public static NetworkManager Instance 
    {   get
        {
            if (_instance == null)
            {
               
                if (_instance == null)
                {
                    GameObject nm = new GameObject("NetworkManager");
                    DontDestroyOnLoad(nm);
                    _instance = nm.AddComponent<NetworkManager>();
                }

            }
            return _instance;
        }
    }

    public void Connect(SbiEvent e)
    {
        PhotonNetwork.offlineMode = false;
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(GameManager.Instance.GetGameVersion());

    }
    public void Match(SbiEvent e)
    {
        if (PhotonNetwork.connectedAndReady && !PhotonNetwork.inRoom)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public bool isMatched( int maxPlayer = 2)
    {
        return (isOffline || (PhotonNetwork.inRoom && PhotonNetwork.playerList.Length == maxPlayer) );
             
    }
    public RoomInfo[] GetRoomList() {
        return PhotonNetwork.GetRoomList();
            }
    public void LeaveRoom(SbiEvent e)
    {
        if (PhotonNetwork.inRoom)
        {

            PhotonNetwork.LeaveRoom();
        }
    }
    public void Disconnect()
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public GameObject Instantiate(string v1, Vector3 vector3, Quaternion identity, int v2, bool isNeutral = false)
    {
        GameObject obj = PhotonNetwork.Instantiate(v1, vector3, identity, v2);
        if (isNeutral)
        {
            obj.GetComponent<Faction>().SetNeutral();
        }
        return obj;
    }

    public void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = true,  maxPlayers = 2 };
        if (PhotonNetwork.connectedAndReady && !PhotonNetwork.inRoom)
        {
            PhotonNetwork.CreateRoom(GameManager.Instance.GetPlayerName() + UnityEngine.Random.Range(1, 999), 
                                     roomOptions, TypedLobby.Default);
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        GameManager.Instance.onJoinRoom(PhotonNetwork.playerList.Length);
		EventManager.Instance.CastEvent(this, "joinedRoom", null);
        Debug.Log("OnJoinedRoom");
    }
    public override void OnConnectedToPhoton()
    {
        base.OnConnectedToPhoton();
		EventManager.Instance.CastEvent(this, "connectedToPhoton", null);
    }

    public override void OnJoinedLobby()
    {
        //Debug.Log("OnJoinedLobby");
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        //Debug.Log("OnPhotonRandomJoinFailed: " + codeAndMsg.ToString());
        CreateNewRoom();
    }

    public void SetPlayerProperties(ExitGames.Client.Photon.Hashtable props)
    {
        PhotonNetwork.player.SetCustomProperties(props);
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonCreateRoomFailed(codeAndMsg);
        //Debug.Log("OnPhotonCreateRoomFailed: " + codeAndMsg.ToString());
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        //Debug.Log("OnPhotonPlayerConnected: " + newPlayer.name );
    }
    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
        base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
		PhotonPlayer player = (PhotonPlayer)playerAndUpdatedProps[0];
		ExitGames.Client.Photon.Hashtable updatedProps
			= (ExitGames.Client.Photon.Hashtable)playerAndUpdatedProps[1];
		if(updatedProps.ContainsKey("skillIds")) {
			int[] skillIds = (int[])updatedProps["skillIds"];
			GameManager.Instance.SetCharacterSkillIDs(player.ID, skillIds);
		}
    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
        LeaveRoom(null);
    }
}
