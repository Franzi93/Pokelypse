using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public void closeMenu(GameObject menu) {
        menu.SetActive(false);
    }
    public void openMenu(GameObject menu) {
        menu.SetActive(true);
    }

    public static void openDialog(string[] text, Dialog dialogBox) {
        dialogBox.lines = text;
        dialogBox.gameObject.SetActive(true);
    }

}
