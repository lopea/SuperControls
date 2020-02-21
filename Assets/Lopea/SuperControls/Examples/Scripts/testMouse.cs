using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lopea.SuperControl;
[ExecuteInEditMode]
public class testMouse : MonoBehaviour
{
    
    void Update()
    {
        //print(SuperInput.mousePosition);
        var ray = Camera.main.ScreenPointToRay(SuperInput.mousePosition);
        transform.position = ray.GetPoint(10);
        

    }
}
