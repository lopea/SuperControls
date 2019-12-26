using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lopea.SuperControl;

[ExecuteInEditMode]
public class redboi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SuperInput.GetKeyDown(KeyCode.R))
            GetComponent<Renderer>().material.color = Color.red;
        else
            GetComponent<Renderer>().material.color = Color.white;
    }
}
