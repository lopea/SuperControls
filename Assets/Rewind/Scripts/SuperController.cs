//SuperController.cs by Javier Sandoval(lopea)
//https://github.com/lopea
//Description:
//Adds or removes tracks/clips in the timeline.
//Also handles input and sends it to SuperInput
//NOTE: Component must be the in the same GameObject as the PlayableDirector.

using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Lopea.SuperControl.InputHandler;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.Timeline;
using Lopea.SuperControl.Timeline;
using Lopea.SuperControl.Timeline.Internal;

namespace Lopea.SuperControl
{
    [RequireComponent(typeof(PlayableDirector))]
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

        //checks if timeline has any clips in the timeline.
        public bool isTimelineEmpty
        {
            get
            {
                //get tracks in the timeline
                var tracks = Timeline.GetRootTracks();

                //there are no tracks in the timeline
                if (tracks.Count() == 0)
                    return true;

                //find at least one clip in every tracks
                foreach (var track in tracks)
                {
                    // a clip was found
                    if (track.GetClips().Count() != 0)
                        return false;
                }

                //no clip was found in the timeline
                return true;
            }
        }


        [SerializeField]
        InputType type = InputType.None;

        public InputType Type { get => type; }

        TrackAsset _placeholder;
        
        internal TrackAsset mouseTrack;
        public void AddPlaceholder()
        {
            //Add a temp track with a clip
            _placeholder = Timeline.CreateTrack<PlaceHolderTrack>();
            var clip = _placeholder.CreateClip<PlaceHolderAsset>();

            //give the track and clip a name
            _placeholder.name = "Recording...";
            clip.displayName = "Recording Input.";

            //give the clip a LONG duration
            clip.duration = 1000;

            //Update GUI
            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
        }

       

        public void RemovePlaceholder()
        {
            //check if a place holder was made
            if (_placeholder != null)
            {

                //remove the placeholder
                Timeline.DeleteTrack(_placeholder);
                _placeholder = null;

                //update the GUI
                TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
            }

        }
        public void PlayTimeline(bool recording = false)
        {

            //The Timeline will NOT play if the timeline is empty
            //we have to add a placeholder track with clip for the timeline to play
            if (recording)
                AddPlaceholder();

            //start the timeline
            //note: shouldn't this be the same as Director.Play()?
            //Director.Play() has some unexpected results 
            Director.Play(Director.playableAsset);
        }


        public void StopTimeline()
        {
            //remove the placeholder if necessary 
            RemovePlaceholder();

            //stop the timeline
            Director.Stop();
        }
        public StaticInputTrack CreateStaticTrack(StaticTrackType type, params object[] args)
        {
            switch (type)
            {
                case StaticTrackType.KeyJoy:
                    //get name for new track
                    var name = Enum.GetName(typeof(KeyCode), (KeyCode)args[0]);

                    //create track
                    var ret = Timeline.CreateTrack<StaticInputTrack>(name);
                    ret.key = name;

                    //refresh GUI
                    TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);

                    return ret;

            }

            //? good luck getting here.
            return null;
        }

        public DynamicInputTrack CreateDynamicTrack(DynamicTrackType type)
        {
            switch(type)
            {
                case DynamicTrackType.MouseX:
                    var mouseX = Timeline.CreateTrack<DynamicInputTrack>("MouseX");
                    TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
                    return mouseX;
                case DynamicTrackType.MouseY:
                    var mouseY = Timeline.CreateTrack<DynamicInputTrack>("MouseY");
                    TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
                    return mouseY;
            }
            return null;
        }


        //TODO: change this for midi
        public StaticInputTrack GetTrack(KeyCode key)
        {
            //get each track and check it it is for the current keycode
            var tracks = Timeline.GetRootTracks().OfType<StaticInputTrack>();
            if (tracks.Count() == 0)
                return null;
            for (int i = 0; i < tracks.Count(); i++)
            {
                var keyTrack = tracks.ElementAt(i);
                if (keyTrack == null)
                    continue;
                if (keyTrack.key.ToLower().Replace(" ", "") == Enum.GetName(typeof(KeyCode), key).ToLower())
                    return keyTrack;

            }
            //track not found
            return null;
        }

        public TimelineClip AddStaticClip(TrackAsset track)
        {
            //create a new clip
            var clip = track.CreateClip<StaticInputPlayableAsset>();
            
            //set clip values
            clip.start = Director.time;
            clip.duration = Time.deltaTime;


            //refresh GUI
            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
            return clip;
        }

        public TimelineClip AddDynamicClip(TrackAsset track)
        {
            //create new clip
            var clip = track.CreateClip<DynamicInputPlayableAsset>();

            //set clip values
            clip.start = Director.time;
            clip.duration = 1/Timeline.editorSettings.fps;

            //refresh GUI
            TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);

            return clip;
        }
        
        public TrackAsset FindDynamicTrack(DynamicTrackType track)
        {
            var tracks = Timeline.GetRootTracks().OfType<DynamicInputTrack>();
        
            for(int i = 0; i < tracks.Count(); i++)
            {
                if(tracks.ElementAt(i).type == track)
                    return tracks.ElementAt(i);
            }
            return null;
        
        }

        public TimelineClip ExtendClip(TimelineClip clip)
        {
            clip.duration = Director.time - clip.start;
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
            return clip;
        }


    }
}