using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {
    // singleton
    private static PrefabManager m_Instance = null;
    public static PrefabManager Get()
    {
        if (m_Instance == null)
            m_Instance = (PrefabManager)FindObjectOfType(typeof(PrefabManager));
        return m_Instance;
    }
    // class 
    public GameObject Prefab_Crafting_Table;
	
	public GameObject Prefab_Monster_Spider;
	
	public GameObject Prefab_Item_RockSword;
}