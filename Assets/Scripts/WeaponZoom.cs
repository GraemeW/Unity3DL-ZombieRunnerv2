using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{
    // Tunables
    [SerializeField][Range(10, 100)] int defaultZoom = 60;
    [SerializeField][Range(10, 100)] int zoomIn = 25;
    [SerializeField] float defaultSensitivity = 2f;
    [SerializeField] float zoomInSensitivity = 1f;

    // State
    bool zoomedIn = false;

    // Cached References
    RigidbodyFirstPersonController fpsController = null;

    private void Start()
    {
        Camera.main.fieldOfView = defaultZoom;
        fpsController = GetComponentInParent<RigidbodyFirstPersonController>();
    }

    private void OnDisable()
    {
        ZoomOut();
        UpdateSensitivity();
    }

    private void Update()
    {
        CheckForZoomPress();
    }

    private void CheckForZoomPress()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            ZoomView();
            UpdateSensitivity();
        }
    }

    private void ZoomView()
    {
        if (!zoomedIn) { Camera.main.fieldOfView = zoomIn; }
        else { Camera.main.fieldOfView = defaultZoom; }
        zoomedIn = !zoomedIn;
    }

    private void ZoomOut()
    {
        Camera.main.fieldOfView = defaultZoom;
        zoomedIn = false;
    }

    private void UpdateSensitivity()
    {
        if (fpsController == null) { return; }
        if (!zoomedIn) 
        { 
            fpsController.mouseLook.XSensitivity = defaultSensitivity;
            fpsController.mouseLook.YSensitivity = defaultSensitivity;
        }
        else
        {
            fpsController.mouseLook.XSensitivity = zoomInSensitivity;
            fpsController.mouseLook.YSensitivity = zoomInSensitivity;
        }
    }
}
