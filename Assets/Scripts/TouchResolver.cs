using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class TouchResolver : MonoBehaviour
{
    [SerializeField] private LayerMask _SceneObjectsLayer;

    private ARRaycastManager _raycastManager;
    private List<ARRaycastHit> _hitList = new();
    private UnityEvent<RaycastHit> _objectDetected = new();
    private UnityEvent<ARRaycastHit> _planeDetected = new();
    private UnityEvent _touchEnded = new();

    public UnityEvent<RaycastHit> GetObjectDetectionEvent()
    {
        return _objectDetected;
    }

    public UnityEvent<ARRaycastHit> GetPlaneDetectionEvent()
    {
        return _planeDetected;
    }

    public UnityEvent GetTouchEventEvent()
    {
        return _touchEnded;
    }

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += OnFingerDown;
        EnhancedTouch.Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
    }

    private void OnDestroy()
    {
        _objectDetected.RemoveAllListeners();
        _planeDetected.RemoveAllListeners();
        _touchEnded.RemoveAllListeners();
    }

    private void OnFingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0)
            return;

        var touchPosition = finger.currentTouch.screenPosition;
        var physicsRay = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y));
        if (_hitList.Count > 0 && Physics.Raycast(physicsRay, out var hitInfo, Camera.main.farClipPlane, _SceneObjectsLayer.value))
        {
            _objectDetected.Invoke(hitInfo);
        }
        else if (_raycastManager.Raycast(touchPosition, _hitList, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            _planeDetected.Invoke(_hitList[0]);
        }
    }

    private void OnFingerUp(EnhancedTouch.Finger finger)
    {
        _touchEnded.Invoke();
    }
}
