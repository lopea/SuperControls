using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Lopea.SuperControl.Timeline
{
    public class DynamicInputPlayableAsset : PlayableAsset
    {
        //store curve for every clip
        public DynamicCurve curve;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            //store new playable
            var playable = ScriptPlayable<DynamicInputPlayableBehaviour>.Create(graph);
            return playable;
        }
    }
}