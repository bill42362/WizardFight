using System;
using System.Collections;
using UnityEngine;
public class NetworkManager : Photon.PunBehaviour
{
    // ***** Singleton Related *****
    private static NetworkManager _instance = null;
    protected NetworkManager()
    {
        //PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.autoCleanUpPlayerObjects = true;
    }
    public static NetworkManager Instance
    {
        get
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

    // ***** Public Networking Interface *****
    public bool isOffline
    {
        get
        {
            return PhotonNetwork.offlineMode;
        }
    }
    public void Connect(SbiEvent e)
    {

        PhotonNetwork.offlineMode = false;
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(GameManager.gameVersion);

    }
    public void Match(SbiEvent e)
    {

        if (PhotonNetwork.connectedAndReady && !PhotonNetwork.inRoom)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public bool isMatched(int maxPlayer = 2)
    {
        return (isOffline || (PhotonNetwork.inRoom && PhotonNetwork.playerList.Length == maxPlayer));

    }
    public void Leave(SbiEvent e)
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
            PhotonNetwork.offlineMode = true;
        }
    }

    public GameObject Instantiate(
        string prefabString,
        Vector3 position, Quaternion rotation,
        int group, object[] instantiationData
    )
    {
        GameObject obj = PhotonNetwork.Instantiate(
            prefabString, position, rotation, group, instantiationData
        );

        return obj;
    }

    public void Ready()
    {
        UpdateRoomProperty(PlayerID.ToString(), "ready");
    }
    public int PlayerID
    {
        get
        {
            if (isOffline)
                return 1;
            return PhotonNetwork.player.ID;
        }
    }
    // ***** Start and Update Behaviours *****
    public void Start()
    {
        EventManager.Instance.RegisterListener(
            EventManager.Instance, "connectButtonClick", gameObject, Connect
        );
        EventManager.Instance.RegisterListener(
            EventManager.Instance, "matchButtonClick", gameObject, Match 
        );
        EventManager.Instance.RegisterListener(
            EventManager.Instance, "leaveButtonClick", gameObject, Leave
        );
    }
    public void Update()
    {
        UpdatePing();
    }

    // ***** Public Networking Monitor Interface *****
    public float RPCDelay = 0.1f;
    public void UpdateRPCDelay(float delay_offset)
    {
        RPCDelay = RPCDelay + DELAY_ESTIMATION_COEFF * delay_offset;
        if (RPCDelay > MAX_INPUT_DELAY)
            RPCDelay = MAX_INPUT_DELAY;
        if (RPCDelay < MIN_INPUT_DELAY)
            RPCDelay = MIN_INPUT_DELAY;
    }

    // ***** Private Network Monitor Utilities *****
    private void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 2 };
        if (PhotonNetwork.connectedAndReady && !PhotonNetwork.inRoom)
        {
            PhotonNetwork.CreateRoom(GameManager.Instance.GetPlayerName() + UnityEngine.Random.Range(1, 999),
                                     roomOptions, TypedLobby.Default);
        }
    }
    private float ping = 0.1f;
    private const float FLOW_CONTROL_BETA = 0.25f;
    private void UpdatePing()
    {
        ping = FLOW_CONTROL_BETA * ping +
                (1 - FLOW_CONTROL_BETA) * (float)PhotonNetwork.GetPing() / 1000;
        //Debug.Log("Ping: " + ping);
    }

    private float DELAY_ESTIMATION_COEFF = 0.75f;
    private const float MAX_INPUT_DELAY = 0.2f;
    private const float MIN_INPUT_DELAY = 0.05f;
    private void UpdateRoomProperty(string key, string value)
    {
        Debug.Log("UpdateRoomProperty: " + key + " to : " + value);
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props[key] = value;
        PhotonNetwork.room.SetCustomProperties(props);
    }

    // ***** Public Photon CallBacks *****
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("OnJoinedRoom");
        GameManager.Instance.OnPlayerJoinedRoom(PhotonNetwork.playerList.Length);
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
        CreateNewRoom(GameManager.Instance.maxPlayer);
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
        GameManager.Instance.OnOtherPlayerJoinedRoom(newPlayer.ID, PhotonNetwork.playerList.Length);
        if (PhotonNetwork.isMasterClient)
        {
            BattleManager.CreateInstance();
        }
    }
    /*
    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
        PhotonPlayer player = (PhotonPlayer)playerAndUpdatedProps[0];

        ExitGames.Client.Photon.Hashtable updatedProps
            = (ExitGames.Client.Photon.Hashtable)playerAndUpdatedProps[1];
        if (updatedProps.ContainsKey("skillIds"))
        {
            if (player.ID == PlayerID)
                return;
            Debug.Log("SetCharacterSkillIDs");
            int[] skillIds = (int[])updatedProps["skillIds"];
            GameManager.Instance.SetCharacterSkillIDs(player.ID, skillIds);
        }
    }
    */
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
        Leave(null);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //GameManager.Instance.OnLeftRoom();
    }


}