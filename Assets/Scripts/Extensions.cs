using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameObjectExtensions {
    /// <summary>
    /// Determines if a GameObject has a component of the given type, and if that component satisfies some predicate
    /// </summary>
    /// <typeparam name="T">The component type to get</typeparam>
    /// <param name="go"></param>
    /// <param name="pred">The predicate to run on the component, if it exists.</param>
    /// <returns>True if the GameObject has a component of the given type and it satisfies the given predicate, false otherwise.</returns>
    public static bool HasComponentAnd<T>(this GameObject go, Predicate<T> pred) {
        var component = go.GetComponent<T>();
        if(component != null) {
            return pred(component);
        } else {
            return false;
        }
    }

    public static T GetComponentInImmediateParent<T>(this GameObject go) where T: class {
        if(go.transform.parent == null) return null;
        var parent = go.transform.parent.gameObject;
        if(parent == null) {
            return null;
        }
        return parent.GetComponent<T>();
    }

    public static T GetComponentInSelfOrImmediateParent<T>(this GameObject go) where T : class {
        T component = go.GetComponent<T>();
        if(component == null) {
            return go.GetComponentInImmediateParent<T>();
        } else {
            return component;
        }
    }
}

public static class AsyncOperationExtensions {
    // from https://gamedev.stackexchange.com/q/120643
    /// <summary>
    /// Await the completion of an AsyncOperation
    /// </summary>
    /// <param name="operation">The operation to await the completion of</param>
    /// <returns>An enumerator existing only to make this work.</returns>
    public static IEnumerator Await(this AsyncOperation operation) {
        while(!operation.isDone)
            yield return operation;
    }
}

public static class SceneExtensions {
    public static T FindInSceneShallow<T>(this Scene scene) where T: class {
        foreach(var go in scene.GetRootGameObjects()) {
            var component = go.GetComponent<T>();
            if(component != null) {
                return component;
            }
        }
        return null;
    }

    public static T FindInSceneDeep<T>(this Scene scene) where T : class {
        foreach(var go in scene.GetRootGameObjects()) {
            var component = go.GetComponentInChildren<T>();
            if(component != null) {
                return component;
            }
        }
        return null;
    }

    public static IEnumerator AwaitLoad(this Scene scene) {
        yield return new WaitUntil(() => scene.isLoaded);
    }
}

