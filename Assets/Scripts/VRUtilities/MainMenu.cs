using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wacki;

public class MainMenu : MonoBehaviour {
    public SteamVR_TrackedController left;
    public SteamVR_TrackedController right;
    public GameObject menu;

    private void OnEnable () {
        left.MenuButtonClicked += this.HandleMenuButtonClicked;
        right.MenuButtonClicked += this.HandleMenuButtonClicked;
    }

    private void OnDisable() {
        left.MenuButtonClicked -= this.HandleMenuButtonClicked;
        right.MenuButtonClicked -= this.HandleMenuButtonClicked;
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
