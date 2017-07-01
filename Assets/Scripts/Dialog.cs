using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

    Text textBox;

    public string[] lines;
    private int currLine = 0;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (currLine == lines.Length + 1)
            {
                currLine = 0;
                gameObject.SetActive(false);
            }
            else {
                ++currLine;
                showText(lines[currLine]);
            }
        }
    }

    public void showText(string _text) {
        textBox.text = _text;
    }
}
