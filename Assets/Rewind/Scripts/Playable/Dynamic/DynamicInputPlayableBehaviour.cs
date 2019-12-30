using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Lopea.SuperControl.Timeline
{
    // A behaviour that is attached to a playable
    public class DynamicInputPlayableBehaviour : PlayableBehaviour
    {

        public float data1;
        public float data2;

        public DynamicTrackType type;

        bool set;

        // Called when the owning graph starts playing
        public override void OnGraphStart(Playable playable)
        {

        }

        // Called when the owning graph stops playing
        public override void OnGraphStop(Playable playable)
        {

        }

        // Called when the state of the playable is set to Play
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            
        }

        // Called when the state of the playable is set to Paused
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            switch (type)
            {
                case DynamicTrackType.Mouse:
                    if (!set)
                    {
                        SuperInput.SetMouse(new Vector2(data1, data2), this);
                        set = true;
                    }
                break;
            }
        }

        // Called each frame while the state is set to Play
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            switch (type)
            {
                case DynamicTrackType.Mouse:
                    if (!set)
                    {
                        SuperInput.SetMouse(new Vector2(data1, data2), this);
                        set = true;
                    }
                break;
            }
        }
    }
}
