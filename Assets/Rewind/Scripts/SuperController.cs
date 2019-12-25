using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Lopea.SuperControl.InputHandler;
using System;
using System.Linq;

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

        //checks if timeline has any clips in the timeline.
        public bool isTimelineEmpty
        {
            get
            {
                //get tracks in the timeline
                var tracks = Timeline.GetRootTracks();
                
                //there are no tracks in the timeline
                if(tracks.Count() == 0)
                    return true;
                
                //find at least one clip in every tracks
                foreach(var track in tracks)
                {
                    // a clip was found
                    if(track.GetClips().Count() != 0)
                        return false;
                }

                //no clip was found in the timeline
                return true;
            }
        }
        

        [SerializeField]
        InputType type;


        
        public void PlayTimeline()
        {

            //The Timeline will NOT play if the timeline is empty
            //we have to add a placeholder track with clip for it to move
            if(isTimelineEmpty)
            {

            //Add a temp track with a clip
            var temp = CreateKeyboardTrack(KeyCode.None);
            var clip = temp.CreateClip<KeyboardPlayableAsset>();
            
            //give the track and clip a name
            temp.name = "PlaceHolder";
            clip.displayName = "Placeholder Clip";
            
            //give the clip a LONG duration
            clip.duration = 1000;
            }

            //start the timeline
            //note: shouldnt this be the same as Director.Play()?
            //Director.Play() has some unexpected results 
            Director.Play(Director.playableAsset); 
        }

        public void StopTimeline()
        {
            
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

        public TimelineClip AddKeyboardClip(KeyboardTrack track)
        {
            var clip = track.CreateClip<KeyboardPlayableAsset>();
            clip.start = Director.time;
            clip.duration = 1;
            return clip;
        }

    }
}