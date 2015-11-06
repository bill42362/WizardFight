using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {
    private static DataManager _instance;
    protected DataManager()
    {
    }
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindObjectOfType(typeof(DataManager)) as DataManager;
                if (_instance == null)
                {
                    GameObject gm = new GameObject("DataManager");
                    DontDestroyOnLoad(gm);
                    _instance = gm.AddComponent<DataManager>();
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
