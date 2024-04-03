using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(RaycastResolver))]
public class CubesBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;

    private RaycastResolver _raycastResolver;

    private void Awake()
    {
        _raycastResolver = GetComponent<RaycastResolver>();
    }

    private void Start()
    {
        _raycastResolver.GetPlaneDetectionEvent().AddListener(InstantiateCube);
        _raycastResolver.GetObjectDetectionEvent().AddListener(PaintCube);
    }

    private void InstantiateCube(ARRaycastHit hitInfo)
    {
        Instantiate(_cubePrefab, hitInfo.pose.position, hitInfo.pose.rotation);
    }

    private void PaintCube(RaycastHit hitInfo)
    {
        hitInfo.collider.GetComponent<InstallableCube>()?.ChangeColor();
    }

    private void OnDestroy()
    {
        _raycastResolver.GetPlaneDetectionEvent().RemoveListener(InstantiateCube);
        _raycastResolver.GetObjectDetectionEvent().RemoveListener(PaintCube);
    }
}
