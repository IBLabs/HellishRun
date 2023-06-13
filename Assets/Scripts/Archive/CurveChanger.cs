using System.Collections;
using System.Collections.Generic;
using AmazingAssets.CurvedWorld;
using Unity.VisualScripting;
using UnityEngine;

public class CurveChanger : MonoBehaviour
{
    public Transform transformController;
    [InspectorRange(0, 1)]
    public float factor = 1f;
    
    private CurvedWorldController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CurvedWorldController>();
    }

    // Update is called once per frame
    void Update()
    {
        var position = transformController.position;
        _controller.bendVerticalSize = position.y * factor;
        _controller.bendHorizontalSize = -position.z * factor;
    }
}
