using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchZoneLook : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	[System.Serializable]
	public class Event : UnityEvent<Vector2> { }

	[Header("Output")]
	public Event touchZoneOutputEvent;

	public RectTransform TouchLook;
	public RectTransform StickLook;

	[Header("Settings")]
	public bool clampToMagnitude;
	public float magnitudeMultiplier = 1f;
	public bool invertXOutputValue;
	public bool invertYOutputValue;

	//Stored Pointer Values
	private Vector3 pointerDownPosition;

	public void OnDrag(PointerEventData eventData)
	{
		//RectTransformUtility.ScreenPointToWorldPointInRectangle(TouchLook, eventData.position, eventData.pressEventCamera, out pointerDownPosition);
		//StickLook.position = eventData.position;

		//Vector2 positionDelta = eventData.delta;
		//Vector2 clampedPosition = ClampValuesToMagnitude(positionDelta);
		//Vector2 outputPosition = ApplyInversionFilter(clampedPosition);

		//OutputPointerEventValue(outputPosition);

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//RectTransformUtility.ScreenPointToWorldPointInRectangle(TouchLook, eventData.position, eventData.pressEventCamera, out pointerDownPosition);
		//UpdateHandleRectPosition(pointerDownPosition);
		StickLook.position = eventData.position;
	}
	void UpdateHandleRectPosition(Vector2 newPosition)
	{
		StickLook.position = newPosition;
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		//StickLook.gameObject.SetActive(false);
	}
	void OutputPointerEventValue(Vector2 pointerPosition)
	{
		touchZoneOutputEvent.Invoke(pointerPosition);
	}
	Vector2 ClampValuesToMagnitude(Vector2 position)
	{
		return Vector2.ClampMagnitude(position, 1);
	}

	Vector2 ApplyInversionFilter(Vector2 position)
	{
		if (invertXOutputValue)
		{
			position.x = InvertValue(position.x);
		}

		if (invertYOutputValue)
		{
			position.y = InvertValue(position.y);
		}

		return position;
	}



	float InvertValue(float value)
	{
		return -value;
	}
}
