using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

namespace Lopea.SuperControl.Timeline
{
    //same as InputType but with no flags
    public enum StaticTrackType
    {
        KeyJoy,
        Midi,
    }

    [TrackColor(0, 0, 0)]
    [TrackClipType(typeof(StaticInputPlayableAsset))]
    public class StaticInputTrack : TrackAsset
    {
        public string key;
        public StaticTrackType type;

        public AnimationCurve mouseX;

        public AnimationCurve mouseY;

        KeyCode ConvertKey()
        {
            foreach (KeyCode curr in Enum.GetValues(typeof(KeyCode)))
            {
                if (key.ToLower().Replace(" ", "") == Enum.GetName(typeof(KeyCode), curr).ToLower())
                    return curr;

            }
            return KeyCode.None;
        }
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            var playable = ScriptPlayable<StaticInputPlayableBehaviour>.Create(graph);
            switch(type)
            {
                case StaticTrackType.KeyJoy:
                    playable.GetBehaviour().key = ConvertKey();
                    playable.GetBehaviour().clip = clip;
                    break;
            }
            return playable;
        }
    }
}
