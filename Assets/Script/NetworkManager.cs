using UnityEngine;
public class NetworkManager : Photon.PunBehaviour {

    public void Start()
    {
        //Debug.Log("NetworkManager.Start() should not be called.");
    }

    // Update is called once per frame
    public void Update()
    {
        //Debug.Log("NetworkManager.Update() should not be called.");
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
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToPhoton");
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

}
