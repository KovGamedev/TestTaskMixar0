using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class ObjectInstaller : MonoBehaviour
{
    [SerializeField] private GameObject _installablePrefab;

    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;
    private List<ARRaycastHit> _hitList = new();

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += OnFingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
    }

    private void OnFingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0)
            return;

        if (_raycastManager.Raycast(finger.currentTouch.screenPosition, _hitList, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            foreach (var hit in _hitList)
            {
                Instantiate(_installablePrefab, hit.pose.position, hit.pose.rotation);
            }
        }
    }
}
