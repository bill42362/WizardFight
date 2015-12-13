using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillCasterDictionary = System.Collections.Generic.Dictionary<int , UnityEngine.GameObject>;
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
    public int[] playerSkillIds = {0};
    private GameObject mainCamera = null;

    // ***** Character Information *****
    public int maxPlayer = 2; // FIXME , this should depend on map.
    private Dictionary<int, int> characterOrder = new Dictionary<int, int>();
    private Dictionary<int, GameObject> characters = new Dictionary<int, GameObject>();
    private Dictionary<int, SkillCasterDictionary> characterSkillCasters
		= new Dictionary<int, SkillCasterDictionary>();

    // ***** Character Public Methods *****
    public void InitializeGame() { }
    public GameObject GetPlayer() { return characters[PlayerId]; }
    public void SetOrder(int charaterId, int order) { characterOrder[charaterId] = order; }
    public void SetCharacter(int charaterId, GameObject character) {
		characters[charaterId] = character;
		PlayerChangeEventData data = new PlayerChangeEventData(character);
		if(charaterId == PlayerId) {
			EventManager.Instance.CastEvent(this, "playerChange", data);
		} else {
			EventManager.Instance.CastEvent(this, "enemyChange", data);
		}
		Debug.Log("SetCharacter()");
		if(IsCharaterCastersReady(charaterId)) { OnCharatersCasterReady(charaterId); }
		if(IsCharactersAndCastersReady()) { OnCharactersAndCastersReady(); }
	}
    public void SetSkillCaster(int charaterId, int skillIndex, GameObject skillCaster) {
		if(!characterSkillCasters.ContainsKey(charaterId)) {
			characterSkillCasters[charaterId] = new Dictionary<int, GameObject>();
		}
		characterSkillCasters[charaterId][skillIndex] = skillCaster;
		Debug.Log("SetSkillCaster()");
		if(IsCharaterCastersReady(charaterId)) { OnCharatersCasterReady(charaterId); }
		if(IsCharactersAndCastersReady()) { OnCharactersAndCastersReady(); }
	}
	private bool IsCharaterCastersReady(int charaterId) {
		if(!characters.ContainsKey(charaterId)) { return false; }
		if(playerSkillIds.Length != characterSkillCasters[charaterId].Keys.Count) {
			return false;
		}
		return true;
	}
	private bool IsCharactersAndCastersReady() {
		if(maxPlayer != characters.Count) { return false; }
		if(maxPlayer != characterSkillCasters.Count) { return false; }
		foreach(KeyValuePair<int, SkillCasterDictionary> casters in characterSkillCasters) {
			if(playerSkillIds.Length != casters.Value.Count) { return false; }
		}
		return true;
	}
	private void OnCharatersCasterReady(int charaterId) {
		Debug.Log("OnCharatersCasterReady(): charaterId" + charaterId);
		GameObject skillHandler = new GameObject("SkillHandler");
		skillHandler.transform.parent = characters[charaterId].transform;
		foreach(int casterIndex in characterSkillCasters[charaterId].Keys) {
			Debug.Log("OnCharatersCasterReady():" + casterIndex);
			GameObject caster = characterSkillCasters[charaterId][casterIndex];
			caster.transform.parent = skillHandler.transform;

			Debug.Log("Cast Event: casterReady()");
			CasterReadyEventData data = new CasterReadyEventData(
				characters[charaterId], casterIndex, caster
			);
			EventManager.Instance.CastEvent(this, "casterReady", data);
		}
	}
	private void OnCharactersAndCastersReady() {
		NetworkManager.Instance.Ready();
	}

    // ***** NetworkManager Coupling methods *****
    public void OnLeftRoom() { mainCamera.SetActive(true); }
    public void OnPlayerJoinedRoom(int playerOrder) {

        SetOrder(PlayerId, playerOrder);
        GameObject playerCharacter = InstantiateCharacter(PlayerId, playerOrder);
        Camera camera = playerCharacter.GetComponentsInChildren<Camera>(true)[0];
        camera.gameObject.SetActive(true);
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCamera.SetActive(false);
       
        if(NetworkManager.Instance.isOffline) {
            GameObject neutralCharacter = InstantiateCharacter(0, 2);
            SetOrder(0, 2);
        }

		for(int i = 0; i < playerSkillIds.Length; ++i) {
            Debug.Log("Instantiate skillcaster with index : " + i);
			NetworkManager.Instance.Instantiate(
				DataManager.Instance.GetSkillCasterPrefabString(playerSkillIds[i]),
				Vector3.zero,
				Quaternion.identity,
				0,
				CreateCasterInsiantiateData(PlayerId, i, true)
			);
		}
    }
    public void OnOtherPlayerJoinedRoom(int id, int order) { SetOrder(id, order); }

    // ***** Game Logic Public Methods *****
    public GameObject InstantiateCharacter(int charaterId, int order) {
        float positionZ = (order == 1) ? -5 : 5;
        GameObject character = NetworkManager.Instance.Instantiate(
			"unitychan", new Vector3(0, 0, positionZ), Quaternion.identity, 0, CreatePlayerInsiantiateData(charaterId)
		);
        return character;
    }
    private object[] CreatePlayerInsiantiateData(int playerId) {
        return new object[] {(object)playerId};
    }
    private object[] CreateCasterInsiantiateData(int charaterId, int skillIndex, bool isPlayerControllable) {
        return new object[] {(object)charaterId, (object)skillIndex, (object)isPlayerControllable};
    }
}
