using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Lopea.SuperControl;
[TrackClipType(typeof(PlaceHolderAsset))]
[TrackColor(1,0,0)]
public class PlaceHolderTrack : TrackAsset
{
    protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
    {
        SuperController d = gameObject.GetComponent<SuperController>();
        if(d == null)
        {
           d = gameObject.AddComponent<SuperController>();
        }   
        var playable = ScriptPlayable<PlaceHolderBehaviour>.Create(graph);
        playable.GetBehaviour().sendDelta = d.getDeltaTime;
        return playable;
    }
}
