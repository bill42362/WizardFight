using UnityEngine;
public class NetworkManager : Photon.PunBehaviour {
    public bool isOffline
    {
        get
        {
            return PhotonNetwork.offlineMode;
        }
    }
    private static NetworkManager _instance = null;
    protected NetworkManager() {
        PhotonNetwork.offlineMode = true;
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
        if (PhotonNetwork.connectedAndReady && !PhotonNetwork.inRoom)
        {
            PhotonNetwork.CreateRoom(GameManager.Instance.GetPlayerName() + UnityEngine.Random.Range(1, 999));
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        GameManager.Instance.onJoinRoom(PhotonNetwork.room.playerCount);
		EventManager.Instance.CastEvent(this, "joinedRoom", null);
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
}
