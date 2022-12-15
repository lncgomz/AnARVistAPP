using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
 
public class ImageTracking : MonoBehaviour
{
    private ARTrackedImageManager m_trackedImageManager;
 
    [SerializeField]
    private TrackedPrefab[] prefabToInstantiate;
 
    private Dictionary<string, GameObject> instanciatePrefab;
 
    private void Awake()
    {
        m_trackedImageManager = GetComponent<ARTrackedImageManager>();
        instanciatePrefab = new Dictionary<string, GameObject>();
        Debug.Log("Awake");
    }
 
    private void OnEnable()
    {
        m_trackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
        Debug.Log("OnEnable");
    }
 
    private void OnDisable()
    {
        m_trackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
        Debug.Log("OnDisable");
    }
 
    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage addedImage in eventArgs.added)
        {
            InstantiateGameObject(addedImage);
        }
 
        foreach (ARTrackedImage updatedImage in eventArgs.updated)
        {
            if(updatedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                Debug.Log("State Tracking: " + updatedImage.referenceImage.name);
                UpdateTrackingGameObject(updatedImage);
            }
            else if(updatedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
            {
                
                Debug.Log("State Limited: " + updatedImage.referenceImage.name);
                UpdateLimitedGameObject(updatedImage);
            }
            else
            {
                Debug.Log("State None" + updatedImage.referenceImage.name);
                UpdateNoneGameObject(updatedImage);
                
            }
        }
 
        foreach (ARTrackedImage removedImage in eventArgs.removed)
        {
            // DestroyGameObject(removedImage);
        }
    }
 
    private void InstantiateGameObject(ARTrackedImage addedImage)
    {
        for (int i = 0; i < prefabToInstantiate.Length; i++)
        {
            if (addedImage.referenceImage.name == prefabToInstantiate[i].name)
            {
                if (!(instanciatePrefab.ContainsKey(addedImage.referenceImage.name))) {
                    GameObject prefab = Instantiate<GameObject>(prefabToInstantiate[i].prefab, transform.parent);
                    prefab.transform.position = addedImage.transform.position;
                    prefab.transform.rotation = addedImage.transform.rotation;
                    prefab.SetActive(true);
                    instanciatePrefab.Add(addedImage.referenceImage.name, prefab);
                }
                else 
                {
                    if (instanciatePrefab.TryGetValue(addedImage.referenceImage.name, out GameObject prefab)) {
                        prefab.SetActive(true);
                    }
                }

                
            }
            else 
            {
                if (instanciatePrefab.TryGetValue(addedImage.referenceImage.name, out GameObject prefab)) {
                        prefab.SetActive(false);
                }
            }
        }
        Debug.Log("Instanciate Prefabs: " + instanciatePrefab.Count);
    }
 
    private void UpdateTrackingGameObject(ARTrackedImage updatedImage)
    {
        foreach(KeyValuePair<string, GameObject> prefab in instanciatePrefab)
        {
            if (updatedImage.referenceImage.name == prefab.Key) {
                prefab.Value.transform.position = updatedImage.transform.position;
                prefab.Value.transform.rotation = updatedImage.transform.rotation;
                prefab.Value.SetActive(true);
            
             } 
            else 
            {
             prefab.Value.SetActive(false);
            }
        }
    }
 
    private void UpdateLimitedGameObject(ARTrackedImage updatedImage)
    {
        foreach(KeyValuePair<string, GameObject> prefab in instanciatePrefab)
        {
            if (updatedImage.referenceImage.name == prefab.Key) {
                prefab.Value.SetActive(false);
            }
        }
    }
 
    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {
        if (updateImage != null) {
            if(!(instanciatePrefab == null) && instanciatePrefab.TryGetValue(updateImage.referenceImage.name, out GameObject prefab))
            {
                prefab.SetActive(false);
            }
        }
    }
 
    private void DestroyGameObject(ARTrackedImage removedImage)
    {
        if (!(instanciatePrefab == null) && instanciatePrefab.TryGetValue(removedImage.referenceImage.name, out GameObject prefab))
        {
            instanciatePrefab.Remove(removedImage.referenceImage.name);
            Destroy(prefab);
        }
    }
 
    [System.Serializable]
    public struct TrackedPrefab
    {
        public string name;
        public GameObject prefab;
    }
}