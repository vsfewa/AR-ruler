using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class MeasureControler : MonoBehaviour
{
    [SerializeField]
    private GameObject measurementPointPrefab;  //测量点

    [SerializeField]
    private float measurementFactor = 39.37f; //单位

    [SerializeField]
    private Vector3 offsetMeasurement = Vector3.zero;

  

   

    [SerializeField]
    private TextMeshProUGUI distanceText; //显示距离文字

    [SerializeField]
    private Dropdown dropdown;

    [SerializeField]
    private ARCameraManager arCameraManager;

    private LineRenderer measureLine; //测量的两点之前的线

    private ARRaycastManager arRaycastManager;
     


    private GameObject startPoint;

    private GameObject endPoint;

    private Vector2 touchPosition = default;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake() //初始化
    {
        arRaycastManager = GetComponent<ARRaycastManager>(); 

        startPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);

        measureLine = GetComponent<LineRenderer>();

        startPoint.SetActive(false);
        endPoint.SetActive(false);

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
            Touch touch = Input.GetTouch(0); //检测触摸点
            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position; //触摸位置

                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    startPoint.SetActive(true);

                    Pose hitPose = hits[0].pose; //射线检测获取三维点
                    startPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }

            if (touch.phase == TouchPhase.Moved) //用户划动屏幕后
            {
                touchPosition = touch.position;

                if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                {
                    measureLine.gameObject.SetActive(true);
                    endPoint.SetActive(true);

                    Pose hitPose = hits[0].pose; //获取碰撞点
                    endPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                }
            }
        }

        if (startPoint.activeSelf && endPoint.activeSelf) //两个点都已经获取
        {
    
            measureLine.SetPosition(0, startPoint.transform.position);
            measureLine.SetPosition(1, endPoint.transform.position);
         if(dropdown.captionText.text=="cm") //单位转换
          {
            distanceText.text = $"Distance: {(Vector3.Distance(startPoint.transform.position, endPoint.transform.position) * measurementFactor*2.54).ToString("F2")} cm";
        }
        else
        {
             distanceText.text = $"Distance: {(Vector3.Distance(startPoint.transform.position, endPoint.transform.position) * measurementFactor).ToString("F2")} in";
        }
        }
    }
}
