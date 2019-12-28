using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Lopea.SuperControl.Timeline
{
    [System.Serializable]
    public class SuperInputPlayableAsset : PlayableAsset
    {
        // Factory method that generates a playable based on this asset
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            var playable = ScriptPlayable<SuperInputPlayableBehaviour>.Create(graph);
            return playable;
        }
    }
}
