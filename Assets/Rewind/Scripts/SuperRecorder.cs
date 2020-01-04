//SuperRecorder.cs by Javier Sandoval(lopea)
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
        Vector2 _lastMouse = Vector2.zero;

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
            // if the recorder is recording
            if (Recording)
            {
                //check every unfinished clip and stop them from extending
                //keyboard tracks
                for (int i = 0; i < newClips.Count; i++)
                {
                    var clip = newClips.ElementAt(i);
                    if (clip.Key is KeyCode)
                    {
                        if (!Input.GetKey((KeyCode)clip.Key))
                            newClips.Remove(clip.Key);
                        
                    }
                    else if(clip.Key is DynamicTrackType)
                        Controller.ExtendClip(clip.Value);
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
                    if (!newClips.ContainsKey(key))
                        newClips.Add(key, Controller.AddStaticClip(track));


                }

            }
            //Mouse Handling
            if ((a.type & InputType.Mouse) == InputType.Mouse)
            {
                //
                //mouseX
                //

                //get track if exists
                var track = Controller.FindDynamicTrack(DynamicTrackType.MouseX);

                //create track if it doesn't
                if (track == null)
                    track = Controller.CreateDynamicTrack(DynamicTrackType.MouseX);

                //checks if the mousex track already exists
                if(!newClips.ContainsKey(DynamicTrackType.MouseX))
                    newClips.Add(DynamicTrackType.MouseX, Controller.AddDynamicClip(track));
            
                //check if the current mouse pos is different than previous changed position
                if(_lastMouse.x != a.mousepos.x)
                {
                   
                    //get asset in current clip
                    var asset = newClips[DynamicTrackType.MouseX].asset as DynamicInputPlayableAsset;
                    
                    //add key to represent change
                    if(asset.curve == null)
                        asset.curve = new DynamicCurve();
                    
                   
                    asset.curve.AddKey((float)(Controller.Director.time - newClips[DynamicTrackType.MouseX].start),
                                       a.mousepos.x);
                    
                    //set asset in the clip 
                    newClips[DynamicTrackType.MouseX].asset = asset;

                    //set last position to the new one
                    _lastMouse.x = Input.mousePosition.x;
                }

                //
                //mouse y
                //

                 //get track if exists
                var tracky = Controller.FindDynamicTrack(DynamicTrackType.MouseY);

                //create track if it doesn't
                if (tracky == null)
                    tracky = Controller.CreateDynamicTrack(DynamicTrackType.MouseY);

                //checks if the mousex track already exists
                if(!newClips.ContainsKey(DynamicTrackType.MouseY))
                    newClips.Add(DynamicTrackType.MouseY, Controller.AddDynamicClip(tracky));
            
                //check if the current mouse pos is different than previous changed position
                if(_lastMouse.y != a.mousepos.y)
                {
                    
                    //get asset in current clip
                    var asset = newClips[DynamicTrackType.MouseY].asset as DynamicInputPlayableAsset;
                    
                    //add key to represent change
                    if(asset.curve == null)
                        asset.curve = new DynamicCurve();
                    
                   
                    asset.curve.AddKey((float)(Controller.Director.time - newClips[DynamicTrackType.MouseY].start),
                                       a.mousepos.y);
                    
                    //set asset in the clip 
                    newClips[DynamicTrackType.MouseY].asset = asset;

                    //set last position to the new one
                    _lastMouse.y = Input.mousePosition.y;
                }
                

            }
        }

    }
}
