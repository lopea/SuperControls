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
    public float[] times;
    public float value;
    
    public DynamicKey(float time, float value)
    {
        AddTime(time);
        this.value = value;
    } 
    public void AddTime(float newTime)
    {
        if(times == null)
        {
            times = new float[1];
            times[0] = newTime;
        }
        else
        {
            var newArray = new float[times.Length + 1];
            for(int i = 0; i < times.Length; i++)
              newArray[i] = times[i];
            newArray[times.Length] = newTime;
            times = newArray;
        }
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
    

    int[] FindNearest(float time)
    {
        int[] index = new int[2];
        float distance = float.MaxValue;
        
        //go through every key
        for(int i = 0; i < keys.Length; i ++)
        {
            //store value of current key
            var currKey = keys[i];
            
            //loop through every time in key
            for(int j = 0; j < currKey.times.Length; j++)
            {
                //check if current distance in time is short enough
                if(Mathf.Abs(currKey.times[j] - time) < distance)
                {
                    index[0] = i;
                    index[1] = j;
                    distance = Mathf.Abs(currKey.times[j] - time);
                }
            }
        }
        return index;
        
    }
    
    public float Evaluate(float time, bool lerp = true)
    {
        int[] index = FindNearest(time);
        if(lerp && index[0] != keys.Length - 1)
            return Mathf.Lerp(keys[index[0]].value, keys[index[0] + 1].value, 
                Mathf.InverseLerp(keys[index[0]].times[index[1]], keys[index[0] + 1].times[index[1]],time));
        else
            return keys[index[0]].value;
    }

}
