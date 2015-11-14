using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private static string gameVersion = "0.00001";
    private static GameManager _instance = null;
    private string playerName = null;
    private GameObject PlayerCharacter = null;
    private GameObject[] skillCasters = null;
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

    public void SetPlayerCharacter(GameObject me)
    {
        Instance.PlayerCharacter = me;
    }
    public GameObject GetPlayerCharacter()
    {
        return Instance.PlayerCharacter;
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
        InstantiateSkillCasters(skillIDs);
        NetworkManager.Instance.SetPlayerProperties(props);
    }
    public void Ready()
    {
        // TODO
        NetworkManager.Instance.Ready();
        EventManager.Instance.RegisterListener(NetworkManager.Instance, "playerAllReady", this.gameObject, OnAllReady) ;
    }
    public void OnAllReady(SbiEvent e)
    {
        //TODO
    }
    public void InstantiateSkillCasters( int[] skillIDs )
    {
        // TODO
        // EventManager.Instance.CastEvent(this, "playerSkillsReady", new PlayerSkillsReadyEventData();
        skillCasters = new GameObject[skillIDs.Length];
        foreach ( int i in skillIDs)
        {
            skillCasters[i] = DataManager.Instance.createSkillCasterByID(i);
        }
        EventManager.Instance.CastEvent(this, "playerSkillsReady", new PlayerSkillsReadyEventData(skillIDs, skillCasters));
    }

}
