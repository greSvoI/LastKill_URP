using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("Assets")]
    [Tooltip("The Camera to use for raycasting.")]
    [SerializeField] Camera mainCamera;

    [Header("Settings")]
    [Tooltip("Range of the Crosshair")]
    [SerializeField] int range = 100;
    [Tooltip("Minimum Crosshair Size")]
    [SerializeField] Vector2 minSize = new Vector2(87, 87);
    [Tooltip("Maximum Crosshair Size")]
    [SerializeField] Vector2 maxSize = new Vector2(300, 300);

    [Header("Debug")]
    [Tooltip("Current Targeted Object")]
    [SerializeField] GameObject targetedObject;

    // GET AND SET COMPLEX METHODS ------------------------------------------------- *

    // GetSize returns the current crosshair size.
    public Vector2 GetSize()
    {
        return this.GetComponent<RectTransform>().sizeDelta;
    }

    // SetSize sets the current crosshair size.
    public void SetSize(Vector2 size, float time = 0.1f)
    {
        Vector2 initialSize = GetComponent<RectTransform>().sizeDelta;
        if (size.x > maxSize.x || size.y > maxSize.y)
        {
            size = new Vector2(maxSize.x, maxSize.y);
        }
        else if (size.x < minSize.x || size.y < minSize.y)
        {
            size = new Vector2(minSize.x, minSize.y);
        }
        Vector2 smoothedSize = Vector2.Lerp(initialSize, size, time);
        GetComponent<RectTransform>().sizeDelta = smoothedSize;
    }

    // MultiplySize multiplies the current crosshair size.
    public void MultiplySize(float multiplier, float time = 0.1f)
    {
        Vector2 initialSize = GetComponent<RectTransform>().sizeDelta;
        Vector2 newSize = new Vector2(initialSize.x * multiplier, initialSize.y * multiplier);
        if (newSize.x > maxSize.x || newSize.y > maxSize.y)
        {
            newSize = new Vector2(maxSize.x, maxSize.y);
        }
        else if (newSize.x < minSize.x || newSize.y < minSize.y)
        {
            newSize = new Vector2(minSize.x, minSize.y);
        }
        Vector2 smoothedSize = Vector2.Lerp(initialSize, newSize, time);
        GetComponent<RectTransform>().sizeDelta = smoothedSize;
    }

    // SetColor sets the current crosshair color.
    public void SetColor(Color color, float time = 0.1f)
    {
        Image[] myImages = GetComponentsInChildren<Image>();
        foreach (Image image in myImages)
        {
            Color initialColor = image.color;
            Color newColor = color;
            Color smoothedColor = Color.Lerp(initialColor, newColor, time);
            image.color = smoothedColor;
        }
    }

    // GetTarget returns the GameObject that is currently targeted.
    public GameObject GetTarget()
    {
        // Shoot a Raycast from the center of the screen.
        RaycastHit hit;
        Vector3 rayDirection = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(rayDirection);
        // If the rayCast hits within the range, return the target.
        if (Physics.Raycast(ray, out hit, range))
        {
            targetedObject = hit.transform.gameObject;
            return hit.transform.gameObject;
        }
        else
        {
            targetedObject = null;
            return null;
        }
    }

    // GetHitPoint returns the point where the raycast hit.
    public Vector3 GetHitPoint()
    {
        // Shoot a Raycast from the center of the screen.
        RaycastHit hit;
        Vector3 rayDirection = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(rayDirection);
        // If the rayCast hits within the range, return the target.
        if (Physics.Raycast(ray, out hit, range))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    // GET AND SET SIMPLE METHODS ------------------------------------------------- *

    public void SetRange(int range)
    {
        this.range = range;
    }

    public int GetRange()
    {
        return range;
    }
}