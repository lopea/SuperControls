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
    public delegate void InputEvent(InputArgs arg);

    //a struct to hold all keyboard input values
    public struct InputArgs 
    {
        public KeyCode[] keyPresses;
        public InputType type;
        public Vector2 mousepos;

        public InputArgs(KeyCode[] keyPresses, InputType type, Vector2 mousepos)
        {
            this.keyPresses = keyPresses;
            this.type = type;
            this.mousepos = mousepos;
        }

        //shorthand for keyboard
        public static InputArgs KeyBoard(KeyCode[] keypresses) => new InputArgs(keypresses, InputType.KeyJoy, Vector3.zero);
        public static InputArgs Mouse() => new InputArgs(null, InputType.Mouse, Input.mousePosition);
    }

    //this Component will track all incoming input and sends it through an event
    //the Component will add itself to the scene when called and shut itself down when no recorder is using it
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

        //Starts the handler if necessary and add the type to the handler
        //if the handler is already initialized, the function adds to the type if necessary
        public static void Initialize(InputType type)
        {
            //if the handler has been initialized but the type is not in the scene
            if (_active && (_single._type & type) != type)
            {
                //add the new value and return
                _single._type |= type;
                return;
            }
            else if(_active)
            {
                //already initialized
                return;
            }

            //set the singleton value to a value in the scene
            _single = new GameObject("SuperInputHandler").AddComponent<SuperInputHandler>();

            //empty event if necessary
            if (_single._ev != null)
                _single._ev = null;

          

            //set the type to the singleton
            _single._type |= type;

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

        //shuts down the handler
        //the InputType is given to continue the input handler if another recorder is using it
        public static void Shutdown(InputType tRemove)
        {
            //SuperInputHandler has not been initialized or the type given is not in the current type.
            if (!_active || _single == null || (_single._type & tRemove) == InputType.None)
                return;

            //remove value from the type
            _single._type &= ~tRemove;


            //check if type has nothing in order to shutdown
            if (_single._type == InputType.None)
            {
                //destroy singleton in the scene
                Destroy(_single.gameObject);
                print("Destroying SuperEventHandler");
                //set empty values
                _active = false;
            }
        }

        //update every frame
        void Update()
        {
            //check if handler is active
            if (_active)
            {
                //keyboard handling
                if ((_type & InputType.KeyJoy) == InputType.KeyJoy)
                {
                    //store all keys pressed in the current frame
                    List<KeyCode> keycodes = new List<KeyCode>();
                    
                    //get all keys pressed in the current frame
                    foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                        if (Input.GetKey(key))
                            //store key in keycodes
                            keycodes.Add(key);
                    
                    //send out pressed keys if any
                    if (keycodes.Count != 0)
                        _ev?.Invoke(InputArgs.KeyBoard(keycodes.ToArray()));
                }

                //mouse handling
                if((_type & InputType.Mouse) == InputType.Mouse)
                {
                    //send out current mouse position
                    _ev?.Invoke(InputArgs.Mouse());
                }
                
            }
        }

    }

    //used to send the type of input given 
    [System.Flags]
    public enum InputType
    {
        None = 0,
        KeyJoy = 1,
        Mouse = 2,
        Midi = 4,
        All = 7
    }
}
