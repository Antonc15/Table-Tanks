using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Serialized Fields \\
    [Header("Components")]
    [SerializeField] private Transform dollyTransform;

    [Header("Settings - Zoom")]
    [SerializeField] private float zoomMagnitude = 1f;
    [SerializeField] private float zoomSmoothTime = 1f;
    [SerializeField] private float maxZoomDistance = 30f;
    [SerializeField] private float minZoomDistance = 8f;

    [Header("Settings - Move")]
    [SerializeField] private float minDragSpeed = 0.0075f;
    [SerializeField] private float maxDragSpeed = 0.05f;

    // Private Fields \\
    private Vector2 lastMousePos = Vector2.zero;

    private float currentZoomDistance = 0f;
    private Vector3 zoomSmoothDampVelocity = Vector3.zero;

    // Private Methods \\
    private void Start()
    {
        currentZoomDistance = Vector3.Distance(transform.position, dollyTransform.position);
    }

    private void LateUpdate()
    {
        int scrollInput = ScrollInput();
        Vector2 dragInput = DragInput();

        Zoom(scrollInput);
        Drag(dragInput);
    }

    private int ScrollInput()
    {
        int inputValue = 0;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            inputValue--;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            inputValue++;
        }

        return inputValue;
    }

    private Vector2 DragInput()
    {
        if (!Input.GetKey(KeyCode.Mouse0)) { return Vector2.zero; }

        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            lastMousePos = Input.mousePosition;
            return Vector2.zero; 
        }

        float xDisplacement = Input.mousePosition.x - lastMousePos.x;
        float yDisplacement = Input.mousePosition.y - lastMousePos.y;

        lastMousePos = Input.mousePosition;

        return new Vector2(-xDisplacement, -yDisplacement);
    }

    private float DragSpeed()
    {
        float zoomScale = Mathf.InverseLerp(minZoomDistance, maxZoomDistance, currentZoomDistance);
        
        return Mathf.Lerp(minDragSpeed, maxDragSpeed, zoomScale);
    }

    private void Zoom(int _scrollInput)
    {
        if(_scrollInput != 0) 
        {
            currentZoomDistance += _scrollInput * zoomMagnitude;

            if(currentZoomDistance > maxZoomDistance) 
            { 
                currentZoomDistance = maxZoomDistance; 
            }
            else if(currentZoomDistance < minZoomDistance)
            {
                currentZoomDistance = minZoomDistance;
            }
        }

        Vector3 zoomTarget = transform.up * currentZoomDistance;

        dollyTransform.localPosition = Vector3.SmoothDamp(dollyTransform.localPosition, zoomTarget, ref zoomSmoothDampVelocity, zoomSmoothTime);
    }

    private void Drag(Vector2 _dragInput)
    {
        if(_dragInput == Vector2.zero) { return; }

        Vector3 dragDisplacement = new Vector3(_dragInput.x, 0, _dragInput.y) * DragSpeed();

        transform.position += dragDisplacement;
    }
}