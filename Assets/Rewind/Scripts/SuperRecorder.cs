//SuperRecorder.cs by Javier Sandoval(lopea)
//https://github.com/lopea
//Description:
//Records all input from given devices and adds them to the timeline.
//NOTE: Component must be the in the same GameObject as the PlayableDirector.

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Lopea.SuperControl.InputHandler;
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
            
            StopRecording();
        }

        public void StartRecording()
        {
            if(_recording)
                return;
            _recording = true;
            SuperInputHandler.Initialize(Type);
            SuperInputHandler.AddEvent(OnInvoke);
            print("Recording has started.");
        }

        //stops all recording and shuts down the input handler
        public void StopRecording()
        {
            if(!_recording)
                return;
            _recording = false;
            SuperInputHandler.RemoveEvent(OnInvoke);
            SuperInputHandler.Shutdown(Type);
        }
        KeyboardTrack GetKeyboardTrack(KeyCode key)
        {
            if(Controller.Timeline == null)
                print("why");
            foreach (var tracks in Controller.Timeline.GetRootTracks())
            {
                var keyTrack = tracks as KeyboardTrack;
                if (keyTrack.key.ToLower().Replace(" ", "") == Enum.GetName(typeof(KeyCode), key).ToLower())
                    return keyTrack;
               
            }
            return null;
        }

        KeyboardTrack CreateKeyboardTrack(KeyCode key)
        {
            var name = Enum.GetName(typeof(KeyCode), key);
            var ret = Controller.Timeline.CreateTrack<KeyboardTrack>(name);
            ret.key = name;
            return ret;
        }

        //handles event invoke that is given fromt SuperInputHandler
        public void OnInvoke(InputArgs a)
        {

            //Keyboard/Joystick handling
            if((a.type & InputType.KeyJoy) == InputType.KeyJoy)
            {
                foreach(KeyCode key in a.keyPresses)
                {
                    var track = GetKeyboardTrack(key);
                    if (track == null)
                        track = CreateKeyboardTrack(key);
                    print("Key Pressed. Key: " + key);

                }
            }
        }

    }
}
