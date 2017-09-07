using System;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// Determines if a GameObject has a component of the given type, and if that component satisfies some predicate
    /// </summary>
    /// <typeparam name="T">The component type to get</typeparam>
    /// <param name="go"></param>
    /// <param name="pred">The predicate to run on the component, if it exists.</param>
    /// <returns>True if the GameObject has a component of the given type and it satisfies the given predicate, false otherwise.</returns>
    public static bool HasComponentAnd<T>(this GameObject go, Predicate<T> pred)
    {
        var component = go.GetComponent<T>();
        if (go != null)
        {
            return pred(component);
        }
        else
        {
            return false;
        }
    }
}
