using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lunity
{

	[RequireComponent(typeof(Collider))]
	public class MouseEventActions : MonoBehaviour
	{
		public UnityEvent OnDownEvent;
		public UnityEvent OnDragEvent;
		public UnityEvent OnEnterEvent;
		public UnityEvent OnExitEvent;
		public UnityEvent OnOverEvent;
		public UnityEvent OnUpEvent;
		public Action OnGlobalUpEvent;
		public UnityEvent OnUpAsButtonEvent;

		public Action OnDown;
		public Action OnDrag;
		public Action OnEnter;
		public Action OnExit;
		public Action OnOver;
		public Action OnUp;
		public Action OnGlobalUp;
		public Action OnUpAsButton;

		private Collider _collider;
		public Collider Collider
		{
			get
			{
				if (_collider == null) _collider = GetComponent<Collider>();
				return _collider;
			}
		}

		public void OnMouseDown()
		{
			OnDown?.Invoke();
			OnDownEvent?.Invoke();
		}

		public void OnMouseDrag()
		{
			OnDrag?.Invoke();
			OnDragEvent?.Invoke();
		}

		public void OnMouseEnter()
		{
			OnEnter?.Invoke();
			OnEnterEvent?.Invoke();
		}

		public void OnMouseExit()
		{
			OnExit?.Invoke();
			OnExitEvent?.Invoke();
		}

		public void OnMouseOver()
		{
			OnOver?.Invoke();
			OnOverEvent?.Invoke();
		}

		public void OnMouseUp()
		{
			OnUp?.Invoke();
			OnUpEvent?.Invoke();
		}

		public void OnMouseUpAsButton()
		{
			OnUpAsButton?.Invoke();
			OnUpAsButtonEvent?.Invoke();
		}

		public void Update()
		{
			if (Input.GetMouseButtonUp(0)) {
				OnGlobalUp?.Invoke();
				OnGlobalUpEvent?.Invoke();
			}
		}

	}
}