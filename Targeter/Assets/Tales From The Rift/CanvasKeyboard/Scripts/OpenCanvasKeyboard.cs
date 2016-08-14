using UnityEngine;
using System.Collections;
using System.Reflection;

namespace TalesFromTheRift
{
	public class OpenCanvasKeyboard : MonoBehaviour 
	{
		// Canvas to open keyboard under
		public Canvas CanvasKeyboardObject;

		// Optional: Input Object to receive text 
		public GameObject inputObject;

		public void OpenKeyboard() 
		{		
			CanvasKeyboard.Open(CanvasKeyboardObject, inputObject != null ? inputObject : gameObject);

			if (inputObject != null) 
			{
				Component[] components = inputObject.GetComponents(typeof(Component));
				foreach (Component component in components)
				{
					PropertyInfo prop = component.GetType().GetProperty("text", BindingFlags.Instance | BindingFlags.Public);
					if (prop != null)
					{
						prop.SetValue(component, null, null);
						return;
					}
				}
			}
		}

		public void CloseKeyboard() 
		{		
			CanvasKeyboard.Close ();
		}
	}
}