using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private static string GameVersion = "0.00001";
    private static GameManager _instance = null;
    private string PlayerName = null;
    private int PlayerOrderInRoom = 0;
    private GameObject PlayerCharacter = null;
    protected GameManager() {
        PlayerName = "username";
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
        return Instance.PlayerName;
    }
    public string GetGameVersion()
    {
        return GameVersion;
    }
    public void onJoinRoom(int order)
    {
        Instance.PlayerOrderInRoom = order;
		bool isPlayerCharater = (order == 1);
        float positionZ = (isPlayerCharater) ? -5 : 5;
        NetworkManager.Instance.Instantiate("unitychan",
                                       new Vector3(0, 0, positionZ),
                                       Quaternion.identity,
                                       0);
    }
}
