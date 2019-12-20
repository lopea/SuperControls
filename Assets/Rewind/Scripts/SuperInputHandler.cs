//SuperInputHandler.cs by Javier Sandoval (Lopea)
//https://github.com/lopea
//Description:
//Gets all input given and creates events for each type of input

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lopea.SuperControl.InputHandler
{
    //delegate for event output
    public delegate void InputEvent(InputArgs input);

    //a struct to hold all input values
    public struct InputArgs 
    {
        public string inputString;
        public InputType type;
        
        public InputArgs(string inputString, InputType type)
        {
            this.inputString = inputString;
            this.type = type;
        }
    }
    public class SuperInputHandler : MonoBehaviour 
    {
        //event to send out all inputs
        event InputEvent _ev;

        //if the handler is active or not
        static bool _active = false;

        //store all input type necessary
        InputType _type;
        
        //singleton value 
        static SuperInputHandler _single;

        public static void Initialize(InputType type)
        {
            //return if handler is already initialized
            if (_active && (_single._type & type) != type)
            {
                _single._type |= type;
                return;
            }
            else if(_active)
            {
                return;
            }
            //set the singleton value to a value in the scene
            _single = new GameObject("SuperInputHandler").AddComponent<SuperInputHandler>();

            //empty event if necessary
            if (_single._ev != null)
                _single._ev = null;



            //set the type to the singleton
            _single._type = type;

            //set handler to active
            _active = true;

        }

        //add event to the handler
        public static void AddEvent(InputEvent ie)
        {
            if (!_active)
                return;
            _single._ev += ie;
        }

        //remove event to the handler
        public static void RemoveEvent(InputEvent ie) 
        {
            if (!_active)
                return;
            _single._ev -= ie;

        }

        //shutdown the handler
        public static void Shutdown()
        {
            if()
            //destroy singleton in the scene
            Destroy(_single.gameObject);
            _single = null;

            _active = false;
        }
       
    }

    [System.Flags]
    public enum InputType
    {
        None = 0,
        Keyboard = 1,
        Mouse = 2,
        Midi = 4,
        All = 7
    }
}
