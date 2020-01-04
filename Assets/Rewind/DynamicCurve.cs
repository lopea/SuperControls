//DynamicCurve.cs by Javier Sandoval(lopea)
//https://github.com/lopea
//Description:
//stores values for Dynamic inputs(A replacement to AnimationCurve)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class DynamicKey
{
    public float time;
    public float value;
    
    public DynamicKey(float time, float value)
    {
        this.time = time;
        this.value = value;
    } 

}

[System.Serializable]
public class DynamicCurve 
{
    public DynamicKey[] keys;


    public void AddKey(float time, float value)
    {
        if(keys == null)
        {
            keys = new DynamicKey[1];
            keys[0] = new DynamicKey(time,value);
        }
        else
        {
        var newArray = new DynamicKey[keys.Length + 1];

        for(int i = 0; i < keys.Length; i ++)
            newArray[i] = keys[i];
        newArray[keys.Length] = new DynamicKey(time, value);
        keys = newArray;
        }
    }

    int FindNearest(float time)
    {
        int index = 0;
        float distance = float.MaxValue;

        for(int i = 0; i < keys.Length; i ++)
        {
            if(keys[i].time < keys.Length && Mathf.Abs(keys[i].time - time) < distance)
            {
                index = i; 
                distance = Mathf.Abs(keys[i].time - time);
            }
        }
        return index;
        
    }
    
    public float Evaluate(float time, bool lerp = true)
    {
        int index = FindNearest(time);
        if(lerp && index != keys.Length - 1)
            return Mathf.Lerp(keys[index].value, keys[index + 1].value, 
                Mathf.InverseLerp(keys[index].time, keys[index + 1].time,time));
        else
            return keys[index].value;
    }

}
