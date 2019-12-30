﻿#define SUPERCONTROL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Lopea.SuperControl.Timeline;

namespace Lopea.SuperControl
{
    public enum KeyState
    {
        Pressed,
        Held,
        Released
    }
    public static class SuperInput
    {
        static Dictionary<KeyCode, KeyState> _kstates;

        public static Vector2 _mouse;
        //get mouse position
        public static Vector2 mousePosition
        {
            get
            {
                return _mouse;
            }
            private set => _mouse = value;
        }

        public static void SetKey(KeyCode key, object context)
        {
            if (context is StaticInputPlayableBehaviour)
            {
                //initialize the list if necessary
                if (_kstates == null)
                    _kstates = new Dictionary<KeyCode, KeyState>();

                //add key to the dictionary
                if (_kstates.ContainsKey(key))
                    _kstates[key] = KeyState.Pressed;
                else
                    _kstates.Add(key, KeyState.Pressed);
            }
        }

        public static void UnsetKey(KeyCode key, object context)
        {
            if (context is StaticInputPlayableBehaviour)
            {
                //dont do anything if the list is null
                if (_kstates == null)
                    return;

                //set key to released.
                if (_kstates.ContainsKey(key))
                    _kstates.Remove(key);
            }
        }

        public static void ChangeKeyState(KeyCode key, KeyState state, object context)
        {
            if (context is StaticInputPlayableBehaviour)
                if (_kstates.ContainsKey(key))
                    _kstates[key] = state;
        }
        //returns true on the same frame that the key is pressed
        //or
        //returns true on the same frame the clip corresponding to the key given is initialized
        public static bool GetKeyDown(KeyCode key)
        {
            if (_kstates == null)
                return Input.GetKeyDown(key);

            return (_kstates.ContainsKey(key)) ?
             _kstates[key] == KeyState.Pressed || Input.GetKeyDown(key)
             : Input.GetKeyDown(key);
        }

        //returns true if key given is held
        //or
        //returns true if the clip corresponding to the key is being played.
        public static bool GetKey(KeyCode key)
        {
            if (_kstates == null)
                return Input.GetKey(key);

            return (_kstates.ContainsKey(key)) ?
             _kstates[key] == KeyState.Held || _kstates[key] == KeyState.Pressed || Input.GetKey(key)
             : Input.GetKey(key);
        }

        public static bool GetKeyUp(KeyCode key)
        {
            if (_kstates == null)
                return Input.GetKeyUp(key);

            return (_kstates.ContainsKey(key)) ?
            _kstates[key] == KeyState.Released || Input.GetKeyUp(key)
            : Input.GetKeyUp(key);
        }

        public static void SetMouse(Vector2 relpos, object context)
        {
            if(context is DynamicInputPlayableBehaviour)
                mousePosition = Vector2.Scale(new Vector2(Screen.width,Screen.height), relpos);
        }
        public static void UnsetMouse(object context)
        {
            if(context is DynamicInputPlayableBehaviour)
                mousePosition = Vector2.zero;
        }
        


    }
}