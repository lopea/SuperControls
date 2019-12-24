using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Lopea.SuperControl.InputHandler;
using System;

namespace Lopea.SuperControl
{
    [RequireComponent(typeof(SuperRecorder))]
    public class SuperController : MonoBehaviour
    {
        //store playabledirector 
        PlayableDirector _director;
        
        //get playabledirector if necessary
        public PlayableDirector Director
        {
            get
            {
                //get PlayableDirector from gameobject if necessary
                if (_director == null)
                    _director = GetComponent<PlayableDirector>();
                
                return _director;
            }
            set { _director = value; }
        }

        //store timeline
        TimelineAsset _timeline;

        //get timeline if necessary
        public TimelineAsset Timeline
        {
            get
            {
                if (_timeline == null)
                    _timeline = Director.playableAsset as TimelineAsset;

                return _timeline;
            }
            set { _timeline = value; }
        }

        

        [SerializeField]
        InputType type;


        [SerializeField]
        bool recordOnAwake;

        public void PlayTimeline()
        {
            //Add a temp track with a clip
            var temp = CreateKeyboardTrack(KeyCode.None);
            var clip = temp.CreateClip<KeyboardPlayableAsset>();
            clip.duration = 1000;
            Director.Play();
        }

        public KeyboardTrack CreateKeyboardTrack(KeyCode key)
        {
            var name = Enum.GetName(typeof(KeyCode), key);
            var ret = Timeline.CreateTrack<KeyboardTrack>(name);
            ret.key = name;
            return ret;
        }

        public KeyboardTrack GetKeyboardTrack(KeyCode key)
        {
            
            foreach (var tracks in Timeline.GetRootTracks())
            {
                var keyTrack = tracks as KeyboardTrack;
                if (keyTrack.key.ToLower().Replace(" ", "") == Enum.GetName(typeof(KeyCode), key).ToLower())
                    return keyTrack;
                
            }
            return null;
        }

    }
}