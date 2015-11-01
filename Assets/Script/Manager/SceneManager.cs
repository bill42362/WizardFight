using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
    private static SceneManager _instance = null;
    protected SceneManager()
    {
    }

    public void ChangeScene( string name )
    {
        if ( Application.loadedLevelName == name )
        {
            Debug.Log("ChangeScene with the same scene: " + name);
            return;
        }
        Debug.Log("ChangeScene: " + name);
        PhotonNetwork.LoadLevel(name);
        
    }
    public static SceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindObjectOfType(typeof(SceneManager)) as SceneManager;
                if (_instance == null)
                {
                    GameObject sm = new GameObject("SceneManager");
                    DontDestroyOnLoad(sm);
                    _instance = sm.AddComponent<SceneManager>();
                }
            }
            return _instance;
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
