using UnityEngine;
using System.Collections;
 
public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance> {
    public static Instance instance;
    public bool isPersistant;
 
    public virtual void Awake() {
       if(isPersistant) 
		{
         if(!instance) {
          instance = this as Instance;
         }
         else {
          DestroyObject(gameObject);
         }
         DontDestroyOnLoad(gameObject);
       }
       else {
         instance = this as Instance;
       }
    }
}