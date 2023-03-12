using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChenge : MonoBehaviour
{

    [SerializeField] private GameObject[] cameras;
    [SerializeField] private int activCamera;
    [SerializeField] private GameObject BackCameras;
   
    void Start()
    {
        activCamera = 0;
        foreach(GameObject cam in cameras)
        {
            cam.SetActive(false);
        }
        cameras[activCamera].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            ChangeCamera();
        }

        if(Input.GetKey(KeyCode.C))
        {
            BackCameras.SetActive(true);
        }
        else
        {
            BackCameras.SetActive(false);
        }
    }

    void ChangeCamera()
    {
        cameras[activCamera].SetActive(false);
        if(activCamera + 1 >= cameras.Length)
        {
            activCamera = 0;
        }
        else
        {
            activCamera += 1;
        }
        cameras[activCamera].SetActive(true);
    }
}
