//SuperRecorder.cs by Javier Sandoval(lopea)
//https://github.com/lopea
//Description:
//Records all input from given devices and adds them to the timeline.
//NOTE: Component must be the in the same GameObject as the PlayableDirector.

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Lopea.SuperControl.InputHandler;
using System.Collections.Generic;
using System;

namespace Lopea.SuperControl
{
    [RequireComponent(typeof(PlayableDirector))]
    public class SuperRecorder : MonoBehaviour
    { 
        
        //stores if recorder should start recording
        bool _recording;

        //is the recorder recording?
        public bool Recording { get => _recording; }

        [SerializeField]
        bool recordOnAwake;
        

        SuperController _controller;

        SuperController Controller
        {
            get
            {
                
                if (_controller == null)
                    _controller = GetComponent<SuperController>();

                return _controller;
            }
        }

        //type of input that gets recorded
        [SerializeField]
        InputType Type;

        void Awake()
        {
            if(recordOnAwake)
                StartRecording();
        }
        //updated every frame
        void Update()
        {
           if(Input.GetKeyDown(KeyCode.Escape))
            StopRecording();
        }
        void OnDisable()
        {
            //stop recording if the recorder is not active in the scene
            if(_recording)
                StopRecording();
        }

        public void StartRecording()
        {
            //dont run if the recorder is already recording
            if(_recording)
                return;
            
            //set recorder flag 
            _recording = true;
            
            //set SuperEventHandler to get input
            SuperInputHandler.Initialize(Type);
            SuperInputHandler.AddEvent(OnInvoke);
            
            //Play the timeline
            Controller.PlayTimeline();
        }
       
        //stops all recording and shuts down the input handler
        public void StopRecording()
        {
            //dont run if the recorder is not currently recording
            if(!_recording)
                return;

            //unset recorder flag
            _recording = false;
            
            //shutdown SuperInputHandler
            SuperInputHandler.RemoveEvent(OnInvoke);
            SuperInputHandler.Shutdown(Type);
        }

        //handles event invoke that is given from SuperInputHandler
        public void OnInvoke(InputArgs a)
        {

            //Keyboard/Joystick handling
            if((a.type & InputType.KeyJoy) == InputType.KeyJoy)
            {
                //get all values that are pressed in the keyboard
                foreach(KeyCode key in a.keyPresses)
                {
                    //get track representing the keyboard
                    var track = Controller.GetKeyboardTrack(key);

                    //Make a new track if necessary
                    if (track == null)
                        track = Controller.CreateKeyboardTrack(key);
                    
                    Controller.AddKeyboardClip(track);
                    

                }
            }
        }

    }
}
