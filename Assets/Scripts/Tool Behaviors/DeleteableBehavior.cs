using UnityEngine;
using Shiki.EventSystem;
using Shiki.EventSystem.Events;

/// <summary>
/// Enables an object to delete itself when requested via a DeleteObjectEvent.
/// </summary>
public class DeleteableBehavior : MonoBehaviour {
	void Start () {
        EventManager.AttachDelegate<DeleteObjectEvent>(this.OnDeleteObjectEvent);
	}

    void OnDestroy() {
        EventManager.RemoveDelegate<DeleteObjectEvent>(this.OnDeleteObjectEvent);
    }

    void OnDeleteObjectEvent(DeleteObjectEvent evt) {
        if(evt.toDelete == this.gameObject.name) {
            Destroy(this.gameObject);
        }
    }
}
