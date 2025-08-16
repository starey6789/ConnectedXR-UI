using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageStatusTracker : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager m_TrackedImageManager;
    // [SerializeField] GameObject canvasPrefab;

    private float startTime, endTime, totalTime;
    private bool onRepeat = false;

    void OnEnable() => m_TrackedImageManager.trackablesChanged.AddListener(WhenActive); 

    public void WhenActive(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // Handle added event
            startTime = Time.time;
            totalTime = 0;
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
            if (updatedImage.trackingState == TrackingState.Tracking) //image is in view
            {
                // updatedImage.gameObject.SetActive(true);
                // ShowContent(updatedImage.gameObject, true);
                if (onRepeat)
                {
                    startTime = Time.time;
                    onRepeat = false;
                }
            }
            else //image is not in view
            {
                String trackedName = updatedImage.referenceImage.name; //method to get the name

                // ShowContent(updatedImage.gameObject, false);
                endTime = Time.time;
                totalTime = endTime - startTime;
                MenuMetrics.LogMetrics(trackedName, startTime, endTime, totalTime);
                totalTime = 0;

                // updatedImage.gameObject.SetActive(false);

                // print(updatedImage.referenceImage.name + ": " + totalTime);

                onRepeat = true;
            }
        }

        foreach (var removed in eventArgs.removed)
        {
            // Handle removed event
            // remove event is almost never called, if it is, the behavior in updated event can be replicated here with only a few changes
        }

    }

    public void ShowContent(GameObject gameObject, bool state)
    {
        gameObject.SetActive(state);
    }

    // void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

}
