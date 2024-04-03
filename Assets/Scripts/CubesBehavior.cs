using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(TouchResolver))]
public class CubesBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _deleteTouchDuration;

    private TouchResolver _raycastResolver;
    private Coroutine _deletingCoroutine;
    private List<InstallableCube> _cubes = new();

    public void StartMoveCubesRight()
    {
        foreach (var cube in _cubes)
        {
            cube.StartMoveRight();
        }
    }

    public void StartMoveCubesLeft()
    {
        foreach (var cube in _cubes)
        {
            cube.StartMoveLeft();
        }
    }

    public void StopMoveCubes()
    {
        foreach (var cube in _cubes)
        {
            cube.StopMove();
        }
    }

    private void Awake()
    {
        _raycastResolver = GetComponent<TouchResolver>();
    }

    private void Start()
    {
        _raycastResolver.GetPlaneDetectionEvent().AddListener(InstantiateCube);
        _raycastResolver.GetObjectDetectionEvent().AddListener(OnCubeClick);
        _raycastResolver.GetTouchEventEvent().AddListener(StopDeletingCoroutine);
    }

    private void InstantiateCube(ARRaycastHit hitInfo)
    {
        _cubes.Add(
            Instantiate(_cubePrefab, hitInfo.pose.position, Quaternion.identity).GetComponent<InstallableCube>()
        );
    }

    private void OnCubeClick(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent<InstallableCube>(out var cube))
        {
            cube.ChangeColor();
            _deletingCoroutine = StartCoroutine(ScheduleCubeDestroying(cube.gameObject));
        }
    }

    private IEnumerator ScheduleCubeDestroying(GameObject cube)
    {
        yield return new WaitForSeconds(_deleteTouchDuration);
        _cubes.Remove(cube.GetComponent<InstallableCube>());
        Destroy(cube);
    }

    private void StopDeletingCoroutine()
    {
        if(_deletingCoroutine != null)
            StopCoroutine(_deletingCoroutine);
    }

    private void OnDestroy()
    {
        _raycastResolver.GetPlaneDetectionEvent().RemoveListener(InstantiateCube);
        _raycastResolver.GetObjectDetectionEvent().RemoveListener(OnCubeClick);
        _raycastResolver.GetTouchEventEvent().RemoveListener(StopDeletingCoroutine);
    }
}
