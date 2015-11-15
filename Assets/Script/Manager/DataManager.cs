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
    public GameObject createSkillCasterByID( int id)
    {
        // FIXME 
        Object prefab = null;
        switch ( id )
        {
            case 0: //fireball
                prefab = Resources.Load("Prefab/Skill/FireBallCaster"); break;
            case 1: //blizzard
                prefab = Resources.Load("Prefab/Skill/BlizzardCaster"); break;
            case 2: //blizzard
                prefab = Resources.Load("Prefab/Skill/ThunderNovaCaster"); break;
            default:
                break;
        }
        return (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

}
