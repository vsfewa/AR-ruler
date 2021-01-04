using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class MeasureController : MonoBehaviour
{
    [SerializeField]
    private GameObject measurementPointPrefab;

    [SerializeField]
    private float measurementFactor = 39.37f;

    [SerializeField]
    private TextMeshProUGUI distanceText;

    private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

    [SerializeField]
    private ARCameraManager arCameraManager;

    private LineRenderer measureLine;

    private ARRaycastManager arRaycastManager;

    private List<Vector3> points = new List<Vector3>();

    private Vector2 touchPosition = default;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private int index;

    private bool isReady;

    Camera camera = new Camera();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        measureLine = GetComponent<LineRenderer>();

        index=1;

    }


    private void OnEnable()
    {
        if (measurementPointPrefab == null)
        {
            Debug.LogError("measurementPointPrefab must be set");
            enabled = false;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            measureLine.enabled = true;
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;

                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    if (index == 1)
                    {
                        Pose hitPose = hits[0].pose;
                        points.Add(hitPose.position);
                    }
                    index++;
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                touchPosition = touch.position;

                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    measureLine.gameObject.SetActive(true);
                    isReady = true;

                    Pose hitPose = hits[0].pose;

                    if(points.Count <= index)
                    {
                        points.Add(hitPose.position);
                    } else
                    {
                        points.RemoveAt(points.Count - 1);
                        points.Add(hitPose.position);
                    }
                }
            }
        }

        if (isReady)
        {
            measureLine.positionCount = points.Count;
            measureLine.SetPositions(points.ToArray());
            distanceText.text = $"Distance: {(Vector3.Distance(points[points.Count - 2], points[points.Count - 1]) * measurementFactor*2.54).ToString("F2")} cm";
        }
    }
}