using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {
	private static DataManager _instance;
	protected DataManager() { }
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
	public string GetSkillCasterPrefabString( int id)
	{
        // FIXME 
        string prefab = null;
		switch ( id )
		{
			case 0: //fireball
				prefab = "Prefab/Skill/FireBallCaster"; break;
			case 1: //blizzard
				prefab = "Prefab/Skill/BlizzardCaster"; break;
			case 2: //blizzard
				prefab = "Prefab/Skill/ThunderNovaCaster"; break;
			default:
				break;
		}
        return prefab;
	}
}
