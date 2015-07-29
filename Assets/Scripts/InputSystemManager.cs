using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputSystemManager : MonoBehaviour
{
	EventSystem system;

	void Start()
	{
		system = EventSystem.current;// EventSystemManager.currentSystem;

	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (system.currentSelectedGameObject == null) return;

			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
//			if (next == null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
//			if (next == null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();
//			if (next == null) next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();

			if (next != null)
			{
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null)
					inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
			//else Debug.Log("next nagivation element not found");

		}
	}
}