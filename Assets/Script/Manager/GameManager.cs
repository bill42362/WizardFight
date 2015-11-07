using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private static string gameVersion = "0.00001";
    private static GameManager _instance = null;
    private string playerName = null;
    private int PlayerOrderInRoom = 0;
    private GameObject PlayerCharacter = null;
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
        Instance.PlayerOrderInRoom = order;
        //Debug.Log("On JoinRoom order = " + order);
		bool isPlayerCharater = (order == 1);
        float positionZ = (isPlayerCharater) ? -5 : 5;
        NetworkManager.Instance.Instantiate("unitychan",
                                       new Vector3(0, 0, positionZ),
                                       Quaternion.identity,
                                       0);
        if ( NetworkManager.Instance.isOffline)
        {
            NetworkManager.Instance.Instantiate("unitychan",
                               new Vector3(0, 0, 5),
                               Quaternion.identity,
                               0, true);
        }
		
    }
}
