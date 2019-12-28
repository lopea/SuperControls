using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

namespace Lopea.SuperControl.Timeline
{
    //same as InputType but with no flags
    public enum TrackType
    {
        None,
        KeyJoy,
        Midi,
        Mouse
    }

    [TrackColor(0, 0, 0)]
    [TrackClipType(typeof(SuperInputPlayableAsset))]
    public class SuperInputTrack : TrackAsset
    {
        public string key;
        public TrackType type;

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
            var playable = ScriptPlayable<SuperInputPlayableBehaviour>.Create(graph);
            switch(type)
            {
                case TrackType.KeyJoy:
                    playable.GetBehaviour().key = ConvertKey();
                    playable.GetBehaviour().clip = clip;
                    break;
            }
            return playable;
        }
    }
}
