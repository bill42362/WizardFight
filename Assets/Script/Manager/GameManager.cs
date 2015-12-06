using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    //  ***** Singleton Related *****
    private static GameManager _instance = null;
    protected GameManager() {
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


    // ***** Overall Game Constant *****
    private static string gameVersion = "0.00001";
    public string GetGameVersion() { return gameVersion; }


    // ***** Player Information *****
    public string playerName = "username";
    public int playerID = 1;
    private GameObject mainCamera = null;

    // ***** Character Information *****
    private int max_player = 2; // FIXME , this should depend on map.
    private Dictionary<int, int> characterOrder;
    private Dictionary<int, GameObject> characters;
    private Dictionary<int, GameObject[]> characterSkillCasters;
    private Dictionary<int, int[]> characterSkillIDs;


    // ***** Character Public Methods *****
    public void InitializeGame()
    {
        characterOrder = new Dictionary<int, int>();
        characters = new Dictionary<int, GameObject>();
        characterSkillCasters = new Dictionary<int, GameObject[]>();
        characterSkillIDs = new Dictionary<int, int[]>();

        SetPlayerID(NetworkManager.Instance.GetPlayerID());
        SetCharacterSkillIDs(GetPlayerID(), new int[3] { 0, 1, 2 });
        
    }
    public void SetOrder( int ID , int order)
    {
        characterOrder[ID] = order;
    }
    public int GetOrder( int ID)
    {
        return characterOrder[ID];
    }

    public void SetPlayerID(int ID) { playerID = ID; }
    public int GetPlayerID() { return playerID; }

    public void SetCharacter(int ID, GameObject me) { characters[ID] = me; }
    public GameObject GetCharacter(int ID = -1)
    {
        if (ID == -1) ID = playerID;
        return characters[ID];
    }
    public GameObject GetPlayerCharacter() { return GetCharacter(); }
    public void SetCharacterSkillIDs(int ID, int[] skillIDs)
    {
        characterSkillIDs[ID] = skillIDs;
        GameObject[] skillCasters = new GameObject[skillIDs.Length];
        for (int i = 0; i < skillIDs.Length; ++i)
        {
            skillCasters[i] = DataManager.Instance.createSkillCasterByID(skillIDs[i]);
        }
        characterSkillCasters[ID] = skillCasters;
    }
    public int[] GetCharacterSkillIDs(int ID = -1) {
        if (ID == -1)
            return characterSkillIDs[playerID];
        return characterSkillIDs[ID];
    }
    public GameObject[] GetCharacterSkillCasters(int ID)
    {
        return (GameObject[])characterSkillCasters[ID];
    }

    // FIXME: It should be GetName(ID) 
    public string GetPlayerName() { return Instance.playerName; }


    // ***** NetworkManager Coupling methods *****
    private void SetGameProperties()
    {
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
        playerProps.Add("name", playerName);
        playerProps.Add("order", characterOrder[playerID]);
        playerProps.Add("skillIds", characterSkillIDs[playerID]);
        NetworkManager.Instance.SetPlayerProperties(playerProps);
    }

    public void OnLeftRoom() { mainCamera.SetActive(true); }
    public void OnPlayerJoinRoom(int playerOrder) {
        SetOrder(playerID, playerOrder);
        GameObject playerCharacter = InstantiateCharacter(playerID,playerOrder);
        Camera camera = playerCharacter.GetComponentsInChildren<Camera>(true)[0];
        camera.gameObject.SetActive(true);
        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCamera.SetActive(false);
       
        SetCharacter(playerID, playerCharacter);         // Player Character must be set at first side


        if (NetworkManager.Instance.isOffline) {
            GameObject neutralCharacter = InstantiateCharacter(0, 2);
            SetOrder(0, 2);
            //SetCharacterSkillIDs(0, new int[] { 0, 1, 2 });
           
        }
        SetGameProperties();
    }


    // ***** Game Logic Public Methods *****
    public GameObject InstantiateCharacter(int ID, int order) {
        float positionZ = (order == 1) ? -5 : 5;
        GameObject character = NetworkManager.Instance.Instantiate("unitychan",
                                                                   new Vector3(0, 0, positionZ),
                                                                   Quaternion.identity,
                                                                   0,
                                                                   CreateInstantiateData(ID));
        return character;
    }

    // ***** Game Logic Private Utility Methods *****
    public const int INSTANTIATE_DATA_INDEX_COUNT = 1;
    public const int INSTANTIATE_DATA_ID = 0;
    private object[] CreateInstantiateData(int ID)
    {
        object[] instantiationData = new object[INSTANTIATE_DATA_INDEX_COUNT];
        instantiationData[INSTANTIATE_DATA_ID] = (object)ID;
        return instantiationData;
    }
}
