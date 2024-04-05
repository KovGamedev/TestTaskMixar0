using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ImageSampleLoader : MonoBehaviour
{
    [SerializeField] string _textureUrl;
    [SerializeField] ImagesTracker _imagesTracker;

    private void Start()
    {
        StartCoroutine(LoadTexture());
    }

    private IEnumerator LoadTexture()
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(_textureUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Downloading success");
                _imagesTracker.AddImageToLibrary(DownloadHandlerTexture.GetContent(request));
            }
            else
            {
                Debug.LogError("Downloading failed");
            }
        }
    }
}