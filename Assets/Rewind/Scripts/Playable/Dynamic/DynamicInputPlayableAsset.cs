using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Lopea.SuperControl.Timeline
{
    public class DynamicInputPlayableAsset : PlayableAsset
    {
        public int index;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            //store new playable
            var playable = ScriptPlayable<DynamicInputPlayableBehaviour>.Create(graph);
            return playable;
        }
    }
}