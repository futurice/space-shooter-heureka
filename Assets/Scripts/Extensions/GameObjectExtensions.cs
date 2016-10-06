using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Extension methods for the Unity's default GameObject.
/// </summary>
public static class GameObjectExtensions
{
	/// <summary>
	/// Gets or add a component. Usage example:
	/// 
	/// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
	/// </summary>
	static public T GetOrAddComponent<T> (this GameObject gameObject) where T : Component
	{
		T result = gameObject.GetComponent<T> ();

		if (result == null)
		{
			result = gameObject.AddComponent<T> ();
		}

		return result;
	}

	/// <summary>
	/// Gets the component or logs an error and returns null.
	/// </summary>
	/// <returns>
	/// The component or null.
	/// </returns>
	static public T GetComponentOrLogError<T> (this GameObject gameObject, string errorText) where T : Component
	{
		T result = gameObject.GetComponent <T> ();

		if (result == null)
		{
			Debug.LogError (errorText);
		}

		return result;
	}

	/// <summary>
	/// Gets the component in parents.
	/// </summary>
	/// <returns>The component in parents.</returns>
	public static T GetComponentInParents<T> (this GameObject gameObject) where T : Component
	{
		for (Transform t = gameObject.transform; t != null; t = t.parent)
		{
			T result = t.GetComponent<T> ();

			if (result != null)
			{
				return result;
			}
		}

		return null;
	}

	/// <summary>
	/// Gets the components in parents.
	/// </summary>
	/// <returns>
	/// The matching components in the GameObject's parents as an array.
	/// </returns>
	public static T[] GetComponentsInParents<T> (this GameObject gameObject) where T : Component
	{
		List<T> results = new List<T>();

		for (Transform t = gameObject.transform; t != null; t = t.parent)
		{
			T result = t.GetComponent<T> ();

			if (result != null)
			{
				results.Add(result);
			}
		}

		return results.ToArray();
	}

	/// <summary>
	/// Gets the first matching component in children.
	/// </summary>
	/// <returns>
	/// The first matching component in children or null.
	/// </returns>
	public static T GetComponentInChildren<T> (this GameObject gameObject, bool includeInactive) where T : Component
	{
		T[] objects = gameObject.GetComponentsInChildren<T> (includeInactive);

		if (objects != null)
		{
			return objects[0];
		}

		return null;
	}
}
