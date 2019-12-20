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

    //
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
    public class SuperInputHandler 
    {
        //event to send out all inputs
        static event InputEvent _ev;

        //if the handler is active or not
        static bool _active = false;

        //store all input type necessary
        static InputType _type;

        public static void Initialize(InputType type)
        {
            //return if handler is already initialized
            if (_active)
                return;
           
            //empty event if necessary
            if (_ev != null)
                _ev = null;
            

            //set handler to active
            _active = true;

        }

        //add event to the handler
        public static void AddEvent(InputEvent ie)
        {
            if (!_active)
                return;
            _ev += ie;
        }

        //remove event to the handler
        public static void RemoveEvent(InputEvent ie) 
        {
            if (!_active)
                return;
            _ev -= ie;

        }

        //this might not work but whatevs
        public static void GetUnityHandlers()
        {
            //return if not active
            if (!_active)
                return;

            //keyboard handling
            if (Input.inputString != "" && (_type & InputType.Keyboard) == InputType.Keyboard)
                _ev?.Invoke(new InputArgs(Input.inputString, InputType.Keyboard));
            
            //mouse handling
            //TODO:add mouse handling
            if)
        }
       
    }

    [System.Flags]
    public enum InputType
    {
        None = 0,
        Keyboard = 1,
        Mouse = 2,
        Midi = 4,
    }
}
