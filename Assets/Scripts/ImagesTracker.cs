using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImagesTracker : MonoBehaviour
{
    private UnityEvent _imageRecognized = new();
    private ARTrackedImageManager _trackedImageManager;

    public UnityEvent GetImageRecognitionEvent()
    {
        return _imageRecognized;
    }

    public void AddImageToLibrary(Texture2D texture)
    {
        if (!(ARSession.state == ARSessionState.SessionInitializing || ARSession.state == ARSessionState.SessionTracking))
        {
            Debug.LogError("AR session state is invalid");
            return;
        }

        if (_trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            mutableLibrary.ScheduleAddImageWithValidationJob(texture, "Crystal", 0.3f);
            _trackedImageManager.enabled = true;
        }
    }

    public bool WasImageRecognized()
    {
        return _trackedImageManager.enabled;
    }

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {
        _trackedImageManager.referenceLibrary = _trackedImageManager.CreateRuntimeLibrary();
    }

    private void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += OnChanged;
    }

    private void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= OnChanged;
    }

    private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            _imageRecognized.Invoke();
        }
    }

    private void OnDestroy()
    {
        _imageRecognized.RemoveAllListeners();
    }
}