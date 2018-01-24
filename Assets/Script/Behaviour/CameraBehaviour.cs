using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    [HideInInspector] public static CameraBehaviour Instance;
    [SerializeField]
    float focusZoom;
    [SerializeField]
    float globalZoom;

    [SerializeField]
    Vector3 globalPosition;


    [SerializeField]
    Transform focusTarget;


    // *************** //
    // ** Initialisation ** //
    // *************** //

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Dezoom();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Zoom();
        }
    }

    public void Focus(Transform targetTransform)
    {
        SetFocusTarget(targetTransform);
        if (Camera.main.orthographicSize != globalZoom)
            Zoom();
    }

    public void LongFocus(Transform targetTransform)
    {
        StartCoroutine(LongFocusCoroutine(targetTransform));
    }

    IEnumerator LongFocusCoroutine(Transform targetTransform)
    {
        SetFocusTarget(targetTransform);
        while (true)
        {
            if (Camera.main.orthographicSize == focusZoom)
                Zoom();
            yield return Time.deltaTime;
        }
    }

    public void StopLongFocus()
    {
        StopCoroutine("LongFocus");
    }

    public void Dezoom()
    {
        Camera.main.orthographicSize = globalZoom;
        transform.position = new Vector3(globalPosition.x, globalPosition.y, transform.position.z);
    }

    public void Zoom()
    {
        Camera.main.orthographicSize = focusZoom;
        if (focusTarget == null)
            return;
        transform.position = new Vector3(focusTarget.position.x, focusTarget.position.y, transform.position.z);
    }

    private void SetFocusTarget(Transform targetTransform)
    {
        focusTarget = targetTransform;
    }

}

