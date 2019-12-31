﻿//SuperRecorder.cs by Javier Sandoval(lopea)
//https://github.com/lopea
//Description:
//Records all input from given devices and adds them to the timeline.
//NOTE: Component must be the in the same GameObject as the SuperController

using UnityEngine;
using Lopea.SuperControl.InputHandler;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;
using System.Linq;
using Lopea.SuperControl.Timeline;

namespace Lopea.SuperControl
{
    [RequireComponent(typeof(SuperController))]
    public class SuperRecorder : MonoBehaviour
    {

        //stores if recorder should start recording
        bool _recording;

        //is the recorder recording?
        public bool Recording { get => _recording; }

        [SerializeField]
        bool recordOnAwake = false;

        //store clips that are not fully complete
        Dictionary<object, TimelineClip> newClips = new Dictionary<object, TimelineClip>();

        //track for mouse positions (you can only record one track)

        //store last mouse position
        Vector2 _lastMouse;

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

        void Awake()
        {
            if (recordOnAwake)
                StartRecording();
        }
        //updated every frame
        void Update()
        {
            if (Recording)
            {

                for (int i = 0; i < newClips.Count; i++)
                {
                    var clip = newClips.ElementAt(i);
                    if (clip.Key is KeyCode)
                    {
                        if (!Input.GetKey((KeyCode)clip.Key))
                            newClips.Remove(clip.Key);
                    }
                }

            }
        }
        void OnDisable()
        {
            //stop recording if the recorder is not active in the scene
            if (_recording)
                StopRecording();
        }

        public void StartRecording()
        {
            //dont run if the recorder is already recording
            if (_recording)
                return;

            //set recorder flag 
            _recording = true;

            //set SuperEventHandler to get input
            SuperInputHandler.Initialize(Controller.Type);
            SuperInputHandler.AddEvent(OnInvoke);

            //Play the timeline
            Controller.PlayTimeline(true);

        
        }

        //stops all recording and shuts down the input handler
        public void StopRecording(bool StopTimeline = false)
        {
            //dont run if the recorder is not currently recording
            if (!_recording)
                return;

            //unset recorder flag
            _recording = false;

            //shutdown SuperInputHandler
            SuperInputHandler.RemoveEvent(OnInvoke);
            SuperInputHandler.Shutdown(Controller.Type);

            //stop the timeline if necessary
            if (StopTimeline)
                Controller.StopTimeline();
            else
                Controller.RemovePlaceholder();
        }


        //handles event invoke that is given from SuperInputHandler
        public void OnInvoke(InputArgs a)
        {

            //Keyboard/Joystick handling
            if ((a.type & InputType.KeyJoy) == InputType.KeyJoy)
            {

                //get all values that are pressed in the keyboard
                foreach (KeyCode key in a.keyPresses)
                {
                    //get track representing the keyboard
                    var track = Controller.GetTrack(key);

                    //Make a new track if necessary
                    if (track == null)
                        track = Controller.CreateStaticTrack(StaticTrackType.KeyJoy, key);

                    // add/change clips
                    if (newClips.ContainsKey(key))
                        newClips[key] = Controller.ExtendClip(newClips[key]);
                    else
                        newClips.Add(key, Controller.AddStaticClip(track));


                }

            }
            //Mouse Handling
            if ((a.type & InputType.Mouse) == InputType.Mouse)
            {
                //mouseX
                if(!newClips.ContainsKey(DynamicTrackType.MouseX))
                {
                    var track = Controller.FindDynamicTrack(DynamicTrackType.MouseX);
                    if(track == null)
                        track = Controller.CreateDynamicTrack(DynamicTrackType.MouseX);

                    newClips.Add(DynamicTrackType.MouseX, Controller.AddDynamicClip(track));
                }
                if(_lastMouse.x != a.mousepos.x)
                {
                    
                }

            }
        }

    }
}
