using UnityEngine;
public class NetworkManager : Photon.PunBehaviour {

    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
    private static NetworkManager _instance = null;
    protected NetworkManager() { }
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

    public void Connect()
    {
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(GameManager.Instance.GetGameVersion());

    }
    public void Match()
    {
        Debug.Log("Match");
        if (PhotonNetwork.connectedAndReady && !PhotonNetwork.inRoom)
        {
           
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public RoomInfo[] GetRoomList() {
        return PhotonNetwork.GetRoomList();
            }
    public void LeaveRoom()
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

    public void Instantiate(string v1, Vector3 vector3, Quaternion identity, int v2)
    {
        PhotonNetwork.Instantiate(v1, vector3, identity, v2);
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
        Debug.Log("OnJoinRoom");
        GameManager.Instance.onJoinRoom(PhotonNetwork.room.playerCount);
    }
    public override void OnConnectedToPhoton()
    {
        base.OnConnectedToPhoton();
        Debug.Log("OnConnectedToPhoton");

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("OnPhotonRandomJoinFailed: " + codeAndMsg.ToString());
        CreateNewRoom();
    }
    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonCreateRoomFailed(codeAndMsg);
        Debug.Log("OnPhotonCreateRoomFailed: " + codeAndMsg.ToString());
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        Debug.Log("OnPhotonPlayerConnected: " + newPlayer.name );
    }
}
