using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extension methods for the Unity's default Transform.
/// </summary>
public static class TransformExtensions
{
	static public Transform GetChildWithTag (this Transform transform, string tag)
	{
		foreach (Transform child in transform)
		{
			if (child.CompareTag (tag))
			{
				return child;
			}
		}

		return null;
	}

	public static void DestroyChildren (this Transform transform)
	{	
		List<GameObject> children = new List<GameObject>();

		foreach (Transform ct in transform)
		{      
			children.Add (ct.gameObject); 
		}

		children.ForEach(child => GameObject.Destroy (child));  
	}

	public static void DestroyChildrenImmediate (this Transform transform)
	{
		List<GameObject> children = new List<GameObject>();

		foreach (Transform ct in transform)
		{      
			children.Add (ct.gameObject); 
		}

		children.ForEach(child => GameObject.DestroyImmediate (child)); 
	}
}
