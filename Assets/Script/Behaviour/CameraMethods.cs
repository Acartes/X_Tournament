using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMethods : MonoBehaviour
{

    [HideInInspector] public static CameraMethods Instance;
    [SerializeField]
    float maxZoomIn;
    [SerializeField]
    float maxZoomOut;

    [SerializeField]
    Vector3 globalPosition;

    public float currentZoom;

    Vector3 globalBottomLeftRect;
    Vector3 globalTopRightRect;

    Vector3 localBottomLeftRect;
    Vector3 localTopRightRect;

    // *************** //
    // ** Initialisation ** //
    // *************** //

    void Awake()
    {
        currentZoom = maxZoomOut;
        Instance = this;
        globalPosition = transform.position;

        globalBottomLeftRect = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        globalTopRightRect = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * 3;
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal"));
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(Vector3.up * Input.GetAxis("Vertical"));
        }

        localBottomLeftRect = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            localTopRightRect = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));

            Vector3 offset = Vector3.zero;
            if (localBottomLeftRect.x < globalBottomLeftRect.x)
            {
                offset.Set(globalBottomLeftRect.x - localBottomLeftRect.x, offset.y, offset.z);
            }

            if (localBottomLeftRect.y < globalBottomLeftRect.y)
            {
                offset.Set(offset.x, globalBottomLeftRect.y - localBottomLeftRect.y, offset.z);
            }
            if (localTopRightRect.x > globalTopRightRect.x)
            {
                offset.Set(globalTopRightRect.x - localTopRightRect.x, offset.y, offset.z);
            }
            if (localTopRightRect.y > globalTopRightRect.y)
            {
                offset.Set(offset.x, globalTopRightRect.y - localTopRightRect.y, offset.z);
            }

            transform.position += offset;

        if (currentZoom <= maxZoomIn)
            currentZoom = maxZoomIn;
        else if (currentZoom > maxZoomOut)
            currentZoom = maxZoomOut;
        Camera.main.orthographicSize = currentZoom;

    }
}
