#define SUPERCONTROL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    static Dictionary<KeyCode,KeyState> _kstates;

    
    public static void SetKey(KeyCode key, object context)
    {
        if(context is KeyboardPlayableBehaviour)
        {
            //initialize the list if necessary
            if(_kstates == null)
                _kstates = new Dictionary<KeyCode, KeyState>();

            //add key to the dictionary
            if(_kstates.ContainsKey(key))
                _kstates[key] = KeyState.Pressed;
            else
                _kstates.Add(key, KeyState.Pressed);   
        }     
    }

    public static void UnsetKey(KeyCode key, object context)
    {
        if(context is KeyboardPlayableBehaviour)
        {
            //dont do anything if the list is null
            if(_kstates == null)
                return;

            //set key to released.
            if(_kstates.ContainsKey(key))
                _kstates[key] = KeyState.Released;
        }
    }

    //returns true on the same frame that the key is pressed
    //or
    //returns true on the same frame the clip corresponding to the key given is initialized
    public static bool GetKeyDown(KeyCode key)
    {
        if(_kstates == null)
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
        if(_kstates == null)
            return Input.GetKey(key);

        return (_kstates.ContainsKey(key)) ?
         _kstates[key] == KeyState.Held || Input.GetKey(key) 
         : Input.GetKey(key);
    }

    public static bool GetKeyUp(KeyCode key)
    {
        if(_kstates == null)
            return Input.GetKeyUp(key);

        return (_kstates.ContainsKey(key)) ?
        _kstates[key] == KeyState.Released || Input.GetKeyUp(key) 
        : Input.GetKeyUp(key);
    }
    
    
 
    public static void UpdateKeys(object context)
    {
        if(context is SuperController)
        {
            if(_kstates == null)
                return;
            
            for(int i = 0; i < _kstates.Count; i++)
            {
                switch(_kstates.ElementAt(i).Value)
                {
                    case KeyState.Pressed:
                        _kstates[_kstates.ElementAt(i).Key] = KeyState.Held;
                        break;
                    case KeyState.Released:
                        _kstates.Remove(_kstates.ElementAt(i).Key);
                        break;
                }
            }
        }
        
    }
    
}
}