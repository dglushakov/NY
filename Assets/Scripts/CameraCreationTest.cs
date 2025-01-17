using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class CameraCreationTest : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        for (int i = 2; i <5; i++)
        {

            var cameraGameObject = new GameObject("Camera"+i);
            var camera = cameraGameObject.AddComponent<Camera>();
            camera.AddComponent <WebRTCPublisher_1> ();

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
