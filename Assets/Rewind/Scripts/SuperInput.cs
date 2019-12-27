#define SUPERCONTROL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lopea.SuperControl
{

    //stores current status of the virtual key
    public struct KeyStatus
    {
        public float length;
        public KeyCode key;

        public float timeStamp;

        public bool init;
        

        public int id;
        
        public float maxLength;

        public KeyStatus(float length, KeyCode key, float timeStamp, bool init, int id, float maxLength)
        {
            this.length = length;
            this.key = key;
            this.timeStamp = timeStamp;
            this.init = init;
            this.id = id;
            this.maxLength = maxLength;
        }

        public static KeyStatus Init(KeyCode key, float time, int id, float cliplength)
        {
            return new KeyStatus(0, key, time, true, id, cliplength);
        }
    }
public static class SuperInput 
{
    static List<KeyStatus>_kstates;

    //finds the KeyStatus in _kstates with a given KeyCode 
    //returns the index in _kstates with the index.
    static int FindKey(KeyCode key)
    {
        if(_kstates == null)
            return -1;
        for(int i = 0; i < _kstates.Count; i++)
        {
            if(_kstates[i].key == key)
            return i;
        }
        return -1;
    }
    public static void SetKey(KeyCode key, float time, int id, float clip)
    {
        //initialize the list if necessary
        if(_kstates == null)
            _kstates = new List<KeyStatus>();

        //if the list is empty, add a key state
        if(_kstates.Count == 0)
        {
            _kstates.Add(KeyStatus.Init(key, time, id, clip));
        }
        else
        {
            //get the index in the list
            int index = FindKey(key);
            if(index < 0)
                //add a new keystate
                _kstates.Add(KeyStatus.Init(key,time, id, clip));
            else
            {
                //override currrent key state
                _kstates.RemoveAt(index);
                _kstates.Add(KeyStatus.Init(key,time, id, clip));
            }
        }
    }

    public static void UnsetKey(KeyCode key)
    {
        //dont do anything if the list is null
        if(_kstates == null)
            return;
        
        //remove key from list
        int index = FindKey(key);
        if(index >= 0)
            _kstates.RemoveAt(index);
    }

    //returns true on the same frame that the key is pressed
    //or
    //returns true on the same frame the clip corresponding to the key given is initialized
    public static bool GetKeyDown(KeyCode key)
    {
        
        int index = FindKey(key);
        bool curr = Input.GetKeyDown(key);
        return (index >= 0) ? _kstates[index].init || curr : curr;
    }

    //returns true if key given is held
    //or
    //returns true if the clip corresponding to the key is being played.
    public static bool GetKey(KeyCode key) => (FindKey(key) >= 0) || Input.GetKey(key);

    public static bool GetKeyUp(KeyCode key)
    {
        int index = FindKey(key);
        bool curr = Input.GetKeyUp(key);
        return (index >= 0) ? _kstates[index].length == _kstates[index].maxLength || curr : curr;
    }
    public static bool CheckSet(KeyCode key, int id)
    {
        int index = FindKey(key);
        return index >= 0 && _kstates[index].id == id;
    }
    
 
    public static void UpdateKeys(float current)
    {
        if(_kstates == null)
            return;
        
        for(int i = 0; i < _kstates.Count; i++)
        {
            var temp = _kstates[i];
            
            if(temp.init && temp.length != 0)
                temp.init = false;
            if(temp.length == 0)
                temp.init = true;
            
            temp.length = current - temp.timeStamp;
            
            _kstates[i] = temp;
        }
        
    }
    
}
}