using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lopea.SuperControl;

[ExecuteInEditMode]
public class redboi : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(SuperInput.GetKey(KeyCode.R))
            GetComponent<Renderer>().sharedMaterial.color = Color.red;
        else
            GetComponent<Renderer>().sharedMaterial.color = Color.white;
    }
}
