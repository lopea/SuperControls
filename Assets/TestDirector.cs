using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TestDirector : MonoBehaviour
{
    PlayableDirector d;
    TimelineAsset t;
    void Start()
    {
        //how to add a track to the timeline
        d = GetComponent<PlayableDirector>();
        t = d.playableAsset as TimelineAsset;
        t.CreateTrack<KeyboardTrack>("SUPERHOT");
        
    }
    
}
