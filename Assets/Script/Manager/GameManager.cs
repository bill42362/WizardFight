using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillCasterDictionary = Dictionary< int , GameObject >;
public class GameManager : MonoBehaviour {
    //  ***** Singleton Related *****
    private static GameManager _instance = null;
    protected GameManager() { }
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

    // ***** Overall Game Constant *****
    public static string gameVersion = "0.00001";

    // ***** Player Information *****
    public string playerName = "username";
    public int PlayerId { get { return NetworkManager.Instance.PlayerID; } }
    public int[] playerSkillIds = {0, 1, 2};
    private GameObject mainCamera = null;

    // ***** Character Information *****
    public int maxPlayer = 2; // FIXME , this should depend on map.
    private Dictionary<int, int> characterOrder = new Dictionary<int, int>();
    private Dictionary<int, GameObject> characters = new Dictionary<int, GameObject>();
    private Dictionary<int, SkillCasterDictionary> characterSkillCasters
		= new Dictionary<int, SkillCasterDictionary>();

    // ***** Character Public Methods *****
    public void InitializeGame() { }
    public void SetOrder(int ID , int order) { characterOrder[ID] = order; }
    public void SetCharacter(int playerId, GameObject character) {
		characters[playerId] = character;
		PlayerChangeEventData data = new PlayerChangeEventData(character);
		EventManager.Instance.CastEvent(this, "characterReady", data);
		if(IsCharactersAndCastersReady()) { OnCharactersAndCastersReady(); }
	}
    public void SetSkillCaster(int playerId, int skillIndex, GameObject skillCaster) {
		if(null == characterSkillCasters[playerId]) {
			characterSkillCasters[playerId] = new Dictionary<int, GameObject>();
		}
		characterSkillCasters[playerId][skillIndex] = skillCaster;
		PlayerSkillReadyEventData data = new PlayerSkillReadyEventData(
			characters[playerId], skillIndex, skillCaster
		);
		EventManager.Instance.CastEvent(this, "casterReady", data);
		if(IsCharactersAndCastersReady()) { OnCharactersAndCastersReady(); }
	}
	private bool IsCharactersAndCastersReady() {
		if(2 != characters.Keys.Count) { return false; }
		if(2 != characterSkillCasters.Keys.Count) { return false; }
		foreach(KeyValuePair<int, SkillCasterDictionary> casters in characterSkillCasters) {
			if(5 != casters.Keys.Count) { return false; }
		}
		return true;
	}
	private void OnCharactersAndCastersReady() {
		NetworkManager.Instance.Ready();
	}

    // ***** NetworkManager Coupling methods *****
    public void OnLeftRoom() { mainCamera.SetActive(true); }
    public void OnPlayerJoinRoom(int playerOrder) {
        SetOrder(PlayerId, playerOrder);
        GameObject playerCharacter = InstantiateCharacter(playerID, playerOrder);
        Camera camera = playerCharacter.GetComponentsInChildren<Camera>(true)[0];
        camera.gameObject.SetActive(true);
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCamera.SetActive(false);
       
        if(NetworkManager.Instance.isOffline) {
            GameObject neutralCharacter = InstantiateCharacter(0, 2);
            SetOrder(0, 2);
        }

		for(int i = 0; i < playerSkillIds.Length; ++i) {
			NetworkManager.Instance.Instantiate(
				DataManager.Instance.GetSkillCasterPrefabString(playerSkillIds[i]),
				new Vector3(0, 0, positionZ),
				Quaternion.identity,
				0,
				CreateCasterInsiantiateData(ID, i)
			);
		}
    }
    public void OnOtherPlayerJoinRoom(int id, int order) { SetOrder(id, order); }

    // ***** Game Logic Public Methods *****
    public GameObject InstantiateCharacter(int ID, int order) {
        float positionZ = (order == 1) ? -5 : 5;
        GameObject character = NetworkManager.Instance.Instantiate(
			"unitychan", new Vector3(0, 0, positionZ), Quaternion.identity, 0, CreatePlayerInsiantiateData(ID)
		);
        return character;
    }
    private object[] CreatePlayerInsiantiateData(int playerId) {
        return new object[] {(object)playerId};
    }
    private object[] CreateCasterInsiantiateData(int playerId, int skillIndex) {
        return new object[] {(object)playerId, (object)skillIndex};
    }
}
