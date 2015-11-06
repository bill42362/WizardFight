using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill {
    public enum SkillTag
    {
        Spell,
        Attack,
        Melee,
        Ranged,
        AOE,
        Projectile,
        Instant,
        Cast,
        Channel,
        Movement,
        Dummy
    } 
    public enum TargetTag
    {
        Enemy,
        Self,
        Area,
        Dummy
    }
    public TargetTag targetTag = TargetTag.Dummy;
    public HashSet<SkillTag> skillTags = new HashSet<SkillTag>();
    public Hashtable tagProperties = new Hashtable();
    public string name = null;
    public JSONObject ConvertToJSON()
    {
        
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        if (name == null)
            return null;
        j.AddField("name", name);
        JSONObject tags = new JSONObject(JSONObject.Type.ARRAY);
        foreach ( SkillTag tag in skillTags )
        {
            tags.Add((int)tag);
            if ( tagProperties.ContainsKey(tag) )
            {
                ;
            }
                
        }
        j.AddField("tags", tags);
        return j;
    }
}
