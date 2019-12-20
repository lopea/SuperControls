//SuperRecorder.cs by Javier Sandoval(lopea)
//https://github.com/lopea
//Description:
//Records all input from given devices and adds them to the timeline.
//NOTE: Component must be the in the same GameObject as the PlayableDirector.

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Lopea.SuperControl
{
    [RequireComponent(typeof(PlayableDirector))]
    public class SuperRecorder : MonoBehaviour
    {
        //stores and manipulates the timeline playable asset.
        PlayableDirector _director;

        //stores the Timeline asset 
        TimelineAsset _asset;

        //stores if recorder should start recording
        bool _recording;

        //starts recording on awake
        [SerializeField]
        bool recordOnAwake;

        void Update()
        {
            Debug.Log();
        }

    }
}
