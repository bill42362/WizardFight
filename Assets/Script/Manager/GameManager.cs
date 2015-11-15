using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private static string gameVersion = "0.00001";
    private static GameManager _instance = null;
    private string playerName = null;
    private Hashtable characters = new Hashtable();
    private int playerID = -1;
    private Hashtable skillIDs = new Hashtable();
    protected GameManager() {
        playerName = "username";
    }
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;
                if (_instance == null)
                {
                    GameObject gm = new GameObject("GameManager");
                    DontDestroyOnLoad(gm);
                    _instance = gm.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    public void SetPlayerCharacter(int ID, GameObject me)
    {
        characters[ID] = me;
    }
    public void SetPlayerID(int ID)
    {
        playerID = ID;
    }
    public int GetPlayerID()
    {
        return playerID;
    }
    public GameObject GetPlayerCharacter(int ID = -1)
    {
        if (ID == -1)
            ID = playerID;
        return (GameObject)characters[ID];
    }

    // Use this for initialization
    public void Start()
    {
		
    }

    // Update is called once per frame
    public void Update()
    {
    }

    public string GetPlayerName()
    {
        return Instance.playerName;
    }
    public string GetGameVersion()
    {
        return gameVersion;
    }
    public void onJoinRoom(int order)
    {

        float positionZ = (PhotonNetwork.isMasterClient) ? -5 : 5;
        GameObject newPlayer = NetworkManager.Instance.Instantiate("unitychan",
                                       new Vector3(0, 0, positionZ),
                                       Quaternion.identity,
                                       0);
		newPlayer.name = "Player";
		newPlayer.AddComponent<RoleEventController>();
		newPlayer.AddComponent<LabelLookAtTarget>();
		GameObject camera = GameObject.FindWithTag("MainCamera");
		camera.transform.parent = newPlayer.transform;
		camera.transform.localPosition = new Vector3(0, 5, -5);
		camera.transform.eulerAngles = new Vector3(30, 0, 0);
		EventManager.Instance.CastEvent(
			this, "playerChange", new PlayerChangeEventData(newPlayer)
		);
        if ( NetworkManager.Instance.isOffline)
        {
            GameObject neutral = NetworkManager.Instance.Instantiate("unitychan",
                               new Vector3(0, 0, 5),
                               Quaternion.identity,
                               0, true);
			neutral.name = "NeutralRole";
			neutral.GetComponent<LookAt>().target = newPlayer;
			newPlayer.GetComponent<LookAt>().target = neutral;
            EventManager.Instance.CastEvent(
				this, "enemyChange", new PlayerChangeEventData(neutral)
			);
        }
        SetGameProperties();
        Ready();

    }
    public void SetGameProperties()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props.Add("name", playerName);
        int[] skillIDs = { 0, 1, 2}; // debug: fireball blizzard and thunder nova;
        //skillCasters = DataManager.Instance.createSkillCastersByIDs( skillIDs );
        props.Add("skills", skillIDs);
        NetworkManager.Instance.SetPlayerProperties(props);
    }
    public void Ready()
    {
        // TODO
        EventManager.Instance.RegisterListener(NetworkManager.Instance, "playerAllReady", this.gameObject, OnAllReady) ;
        NetworkManager.Instance.Ready();
        InstantiatePlayerSkillCaster();
    }
    public void OnAllReady(SbiEvent e)
    {
        //TODO
        InstantiateOtherSkillCaster();
    }
    public void InstantiatePlayerSkillCaster( )
    {
        InstantiateSkillCasters(playerID);

    }
    public void InstantiateOtherSkillCaster()
    {
        foreach( int pID in skillIDs.Keys )
        {
            if (pID == playerID)
                continue;
            InstantiateSkillCasters(pID);
        }


    }
    public void InstantiateSkillCasters(int pID)
    {
        int[] characterSkills = (int[])skillIDs[pID];
        GameObject[] skillCaster = new GameObject[characterSkills.Length];
        foreach (int sID in characterSkills)
        {
            skillCaster[sID] = DataManager.Instance.createSkillCasterByID(sID);
        }
        PlayerSkillsReadyEventData data = new PlayerSkillsReadyEventData(GetPlayerCharacter(pID), characterSkills, skillCaster);
        EventManager.Instance.CastEvent(this, "playerSkillsReady", data);
    }
    public void SetSkillIDs(int ID, int[] skillIDs)
    {
        this.skillIDs[ID] = skillIDs;
    }
}
