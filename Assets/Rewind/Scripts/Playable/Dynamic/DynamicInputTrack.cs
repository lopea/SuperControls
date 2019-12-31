using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Lopea.SuperControl.Timeline
{
    public enum DynamicTrackType
    {
        MouseX,
        MouseY,
        MidiCC
    }
    [TrackColor(0, 0, 1)]
    [TrackClipType(typeof(DynamicInputPlayableAsset))]
    public class DynamicInputTrack : TrackAsset
    {
        public DynamicTrackType type;
        
        public AnimationCurve[] Curves;

        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            var playable = ScriptPlayable<DynamicInputPlayableBehaviour>.Create(graph);
            var castclip = clip.asset as DynamicInputPlayableAsset;

            playable.GetBehaviour().type = type;
            playable.GetBehaviour().CurrentCurve = Curves[castclip.index];

            return playable;
        }
    }
}
