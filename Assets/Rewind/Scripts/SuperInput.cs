using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lopea.SuperControl
{
    public struct KeyStatus
    {
        public float length;
        public KeyCode key;

        public float timeStamp;

        public bool init;

        public KeyStatus(float length, KeyCode key, float timeStamp, bool init)
        {
            this.length = length;
            this.key = key;
            this.timeStamp = timeStamp;
            this.init = init;
        }

        public static KeyStatus Init(KeyCode key, float time)
        {
            return new KeyStatus(0, key, time, true);
        }
    }
public static class SuperInput 
{
    static List<KeyStatus>_kstates;

    static void initKeyStates()
    {
        if(_kstates == null)
            _kstates = new List<KeyStatus>();
    }
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
    public static void SetKey(KeyCode key, float time)
    {
        initKeyStates();
        if(_kstates.Count == 0)
        {
            _kstates.Add(KeyStatus.Init(key, time));
        }
        else
        {
            int index = FindKey(key);
            if(index < 0)
                _kstates.Add(KeyStatus.Init(key,time));
        }
    }

    public static void UnsetKey(KeyCode key)
    {
        if(_kstates == null)
            return;
        int index = FindKey(key);
        if(index >= 0)
            _kstates.RemoveAt(index);
    }

    public static bool GetKeyDown(KeyCode key)
    {
        int index = FindKey(key);
        bool curr = Input.GetKeyDown(key);
        return (index >= 0) ? _kstates[index].init || curr : curr;
    }
    public static bool GetKey(KeyCode key)
    {
        return (FindKey(key) >= 0) || Input.GetKey(key);
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
            temp.length = current - temp.timeStamp;
            
            _kstates[i] = temp;
        }
    }
    
}
}