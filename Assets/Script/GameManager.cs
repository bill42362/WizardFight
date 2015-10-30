using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private static string GameVersion = "0.00001";
    private string PlayerName = null;
    private int PlayerOrderInRoom = 0;
    private static GameManager _instance = null;
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


    // Use this for initialization
    public void Start()
    {
        Debug.Log("GameManager.Start() should not be called.");
    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log("GameManager.Update() should not be called.");
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
        int position = (Instance.PlayerOrderInRoom == 1) ? -5 : 5;
        NetworkManager.Instance.Instantiate("Player",
                                       new Vector3(0, 0, position),
                                       Quaternion.identity,
                                       0);
    }
}
