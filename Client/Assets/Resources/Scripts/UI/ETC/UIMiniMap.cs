using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMiniMap : MonoBehaviour
{
    [SerializeField]
    private Camera minimapCamera;

    [SerializeField]
    private float zoomIn = 1;

    [SerializeField]
    private float zoomMax = 30;

    [SerializeField]
    private float zoomOneStep = 1;

    public void ZoomIn() 
    {
        minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize - zoomOneStep, zoomIn);
    }

    public void ZoomOut()
    {
        minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomOneStep, zoomMax);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad8))
        {
            ZoomIn();
        }
        else if (Input.GetKey(KeyCode.Keypad2)) 
        {
            ZoomOut();
        }
    }
}
