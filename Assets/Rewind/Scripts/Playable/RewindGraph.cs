using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class RewindGraph : MonoBehaviour
{
    //hold singleton variable 
    static RewindGraph _main;
    
    //create a singleton for the graph
    //there should be only one instance of the graph
    public static RewindGraph Main
    {
        get
        {
            if(!_main)
            {
                _main = new GameObject("Graph").AddComponent<RewindGraph>();    
            }
            return _main;
        }
    }
    void Start()
    {
        PlayableDirector g;
        g.playableAsset.
    }
}
