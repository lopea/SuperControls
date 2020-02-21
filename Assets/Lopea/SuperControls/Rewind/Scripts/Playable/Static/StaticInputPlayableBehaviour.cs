﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

namespace Lopea.SuperControl.Timeline
{

    // A behaviour that is attached to a playable
    public class StaticInputPlayableBehaviour : PlayableBehaviour
    {
        public KeyCode key;

        [HideInInspector]
        public TimelineClip clip;

        [HideInInspector]
        public StaticTrackType type;

        

       
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
            
            SuperInput.SetKey(key, this);

        }

        // Called when the state of the playable is set to Paused
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            SuperInput.UnsetKey(key, this);

        }

        // Called each frame while the state is set to Play
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            //idk how i got this but its the timeline time so its ok i guess?
            var time = info.output.GetSourcePlayable().GetTime();
            switch (type)
            {
                //keyboard handling
                case StaticTrackType.KeyJoy:
                    //change states when necessary
                    if (Math.Abs(time - clip.start) < 0.019)  //around 60 fps time epsilon
                        SuperInput.ChangeKeyState(key, KeyState.Pressed, this);
                    else if (Math.Abs(time - clip.end) < 0.019)
                        SuperInput.ChangeKeyState(key, KeyState.Released, this);
                    else
                        SuperInput.ChangeKeyState(key, KeyState.Held, this);
                    break;  
            }
            
        }
    }
}
