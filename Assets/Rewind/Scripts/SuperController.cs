using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Lopea.SuperControl.InputHandler;

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
                    _timeline = _director?.playableAsset as TimelineAsset;

                return _timeline;
            }
            set { _timeline = value; }
        }

        

        [SerializeField]
        InputType type;


        [SerializeField]
        bool recordOnAwake;

    }
}