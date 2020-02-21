using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Lopea.SuperControl.Timeline.Internal
{
    public class PlaceHolderAsset : PlayableAsset
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<PlaceHolderBehaviour>.Create(graph);
        }
    }
}