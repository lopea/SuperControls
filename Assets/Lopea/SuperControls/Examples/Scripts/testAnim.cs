using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class testAnim : MonoBehaviour
{
    public AnimationCurve curve;
    
    [SerializeField]
    public List<float> f;   

    [SerializeField]
    DynamicCurve f2;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {       
        curve.AddKey(Time.time, Mathf.Sin(Time.time));
    }

    void AddKey()
    {
        
    }
    
}
