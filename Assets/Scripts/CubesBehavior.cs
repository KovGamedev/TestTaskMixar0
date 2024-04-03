using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(TouchResolver))]
public class CubesBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _deleteTouchDuration;

    private TouchResolver _raycastResolver;
    private Coroutine _deletingCoroutine;

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
        Instantiate(_cubePrefab, hitInfo.pose.position, hitInfo.pose.rotation);
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
