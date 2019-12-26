using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

[TrackColor(0,0,0)]
[TrackClipType(typeof(KeyboardPlayableAsset))]
public class KeyboardTrack : TrackAsset
{
    public string key;
    
    KeyCode ConvertKey()
    {
        foreach(KeyCode curr in Enum.GetValues(typeof(KeyCode)))
        {  
            if(key.ToLower().Replace(" ", "") == Enum.GetName(typeof(KeyCode),curr).ToLower())
                return curr;
        }
        return KeyCode.None;
    }
    protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
    {
        var playable = ScriptPlayable<KeyboardPlayableBehaviour>.Create(graph);
        playable.GetBehaviour().key = ConvertKey();
        playable.GetBehaviour().currentTime = (float)gameObject.GetComponent<PlayableDirector>().time;
        return playable;
    }
}
    