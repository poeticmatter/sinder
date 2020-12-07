﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System;
using UnityEngine.Events;

public class URLButton : MonoBehaviour, IPointerDownHandler
{
	[Serializable]
	public class ButtonPressEvent : UnityEvent { }

	public ButtonPressEvent OnPress = new ButtonPressEvent();

	public void OnPointerDown(PointerEventData eventData)
	{
		OnPress.Invoke();
	}

}
