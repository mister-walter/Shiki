using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wacki;
using Shiki.EventSystem;
using Shiki.EventSystem.InternalEvents;

public class MainMenu : MonoBehaviour {
    public SteamVR_TrackedController left;
    public SteamVR_TrackedController right;
    public GameObject menu;

    private void OnEnable () {
        left.MenuButtonClicked += this.HandleMenuButtonClicked;
        right.MenuButtonClicked += this.HandleMenuButtonClicked;
        EventManager.AttachDelegate<MenuEnableEvent>(this.OnMenuEnableEvent);
        EventManager.AttachDelegate<MenuDisableEvent>(this.OnMenuDisableEvent);
    }

    private void OnDisable() {
        left.MenuButtonClicked -= this.HandleMenuButtonClicked;
        right.MenuButtonClicked -= this.HandleMenuButtonClicked;
        EventManager.RemoveDelegate<MenuEnableEvent>(this.OnMenuEnableEvent);
        EventManager.RemoveDelegate<MenuDisableEvent>(this.OnMenuDisableEvent);
    }

    void OnMenuEnableEvent(MenuEnableEvent evt) {
        menu.SetActive(true);
        SetLaserPointerState(true);
    }

    void OnMenuDisableEvent(MenuDisableEvent evt) {
        menu.SetActive(false);
        SetLaserPointerState(false);
    }

    void HandleMenuButtonClicked(object sender, ClickedEventArgs e) {
        if(menu.activeSelf) {
            menu.SetActive(false);
            SetLaserPointerState(false);
        } else {
            menu.SetActive(true);
            SetLaserPointerState(true);
        }
    }

    void SetLaserPointerState(bool state) {
        ViveUILaserPointer leftPointer = left.GetComponent<ViveUILaserPointer>();
        leftPointer.SetVisibility(state);
        ViveUILaserPointer rightPointer = right.GetComponent<ViveUILaserPointer>();
        rightPointer.SetVisibility(state);
    }
}
