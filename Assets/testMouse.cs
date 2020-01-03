using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lopea.SuperControl;
public class testMouse : MonoBehaviour
{
    [ExecuteInEditMode]
    void Update()
    {
        print(SuperInput.mousePosition);
        var ray = Camera.main.ScreenPointToRay(SuperInput.mousePosition);
        transform.position = ray.GetPoint(10);
        

    }
}
