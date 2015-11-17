using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public string playerName = "username";
    public int playerID = -1;
    public int[] playerSkillIds = {0, 1, 2};
    public Hashtable characterSkillCasters = new Hashtable();

    private static string gameVersion = "0.00001";
    private static GameManager _instance = null;
    private Hashtable characters = new Hashtable();
    private Hashtable characterSkillIDs = new Hashtable();
    protected GameManager() { }
    public static GameManager Instance {
        get {
            if(_instance == null) {
                _instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;
                if(_instance == null) {
                    GameObject gm = new GameObject("GameManager");
                    DontDestroyOnLoad(gm);
                    _instance = gm.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    public void SetPlayerCharacter(int ID, GameObject me) { characters[ID] = me; }
    public void SetPlayerID(int ID) { playerID = ID; }
    public int GetPlayerID() { return playerID; }
    public GameObject GetPlayerCharacter(int ID = -1) {
        if (ID == -1) ID = playerID;
        return (GameObject)characters[ID];
    }
    public string GetPlayerName() { return Instance.playerName; }
    public string GetGameVersion() { return gameVersion; }
    public void OnLeftRoom() { GameObject.FindWithTag("MainCamera").transform.parent = null; }
    public void onJoinRoom(int playerId) {
		this.playerID = playerId;
        SetGameProperties();
        float positionZ = (PhotonNetwork.isMasterClient) ? -5 : 5;
        GameObject newPlayer = NetworkManager.Instance.Instantiate(
			"unitychan", new Vector3(0, 0, positionZ), Quaternion.identity, 0
		);
		newPlayer.name = "Player";
		newPlayer.GetComponent<Role>().playerId = playerId;
		newPlayer.AddComponent<RoleEventController>();
		newPlayer.AddComponent<LabelLookAtTarget>();
		EventManager.Instance.CastEvent(
			this, "playerChange", new PlayerChangeEventData(newPlayer)
		);

		GameObject camera = GameObject.FindWithTag("MainCamera");
		camera.transform.parent = newPlayer.transform;
		camera.transform.localPosition = new Vector3(0, 5, -5);
		camera.transform.eulerAngles = new Vector3(30, 0, 0);

        if(NetworkManager.Instance.isOffline) {
            GameObject neutral = NetworkManager.Instance.Instantiate(
				"unitychan", new Vector3(0, 0, 5), Quaternion.identity, 0, true
			);
			neutral.name = "NeutralRole";
			neutral.GetComponent<LookAt>().target = newPlayer;
			newPlayer.GetComponent<LookAt>().target = neutral;
            EventManager.Instance.CastEvent(
				this, "enemyChange", new PlayerChangeEventData(neutral)
			);
        }
    }
    public void SetGameProperties() {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps.Add("name", playerName);
        playerProps.Add("playerId", playerID);
        playerProps.Add("skillIds", playerSkillIds);
        NetworkManager.Instance.SetPlayerProperties(playerProps);
    }
    public void SetCharacterSkillIDs(int playerId, int[] skillIDs) {
        characterSkillIDs[playerId] = skillIDs;
		GameObject[] skillCasters = new GameObject[skillIDs.Length];
		for(int i = 0; i < skillIDs.Length; ++i) {
			skillCasters[i] = DataManager.Instance.createSkillCasterByID(skillIDs[i]);
		}
		characterSkillCasters[playerId] = skillCasters;
    }
    public int[] GetCharacterSkillIDs(int playerId = 1) { return (int[])characterSkillIDs[playerId]; }
    public GameObject[] GetCharacterSkillCastersById(int playerId) {
		return (GameObject[])characterSkillCasters[playerId];
	}
}
