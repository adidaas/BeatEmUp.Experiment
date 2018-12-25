using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    // Set these in the editor
    public BoxCollider2D[] hitFrames;
    public PolygonCollider2D frame2;
    public PolygonCollider2D frame3;

    // Used for organization
    private BoxCollider2D[] colliders;

    // Collider on this game object
    private PolygonCollider2D localCollider;

    private KeyCombo falconPunch = new KeyCombo(new string[] { "d", "s", "a" });
    private KeyCombo falconKick = new KeyCombo(new string[] { "s", "d", "d" });

    // We say box, but we're still using polygons.
    public enum hitBoxes
    {
        hitFramesBox,
        frame2Box,
        frame3Box,
        clear // special case to remove all boxes
    }

    void Start()
    {

    }

    //void Update()
    //{
    //    if (falconPunch.Check())
    //    {
    //        // do the falcon punch
    //        Debug.Log("PUNCH");
    //    }
    //    if (falconKick.Check())
    //    {
    //        // do the falcon punch
    //        Debug.Log("KICK");
    //    }
    //}

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    Debug.Log("Collider hit something!");
    //}

    public class KeyCombo
    {
        public string[] buttons;
        private int currentIndex = 0; //moves along the array as buttons are pressed

        public float allowedTimeBetweenButtons = 1.3f; //tweak as needed
        private float timeLastButtonPressed;

        public KeyCombo(string[] b)
        {
            buttons = b;
        }

        //usage: call this once a frame. when the combo has been completed, it will return true
        public bool Check()
        {
            if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) currentIndex = 0;
            {
                if (currentIndex < buttons.Length)
                {
                    //if ((buttons[currentIndex] == "down" && Input.GetAxisRaw("Vertical") == -1) ||
                    //(buttons[currentIndex] == "up" && Input.GetAxisRaw("Vertical") == 1) ||
                    //(buttons[currentIndex] == "left" && Input.GetAxisRaw("Vertical") == -1) ||
                    //(buttons[currentIndex] == "right" && Input.GetAxisRaw("Horizontal") == 1) ||
                    //(buttons[currentIndex] != "down" && buttons[currentIndex] != "up" && buttons[currentIndex] != "left" && buttons[currentIndex] != "right" && Input.GetButtonDown(buttons[currentIndex])))
                    if ((buttons[currentIndex] == "s" && Input.GetAxisRaw("Vertical") == -1) ||
                    (buttons[currentIndex] == "w" && Input.GetAxisRaw("Vertical") == 1) ||
                    (buttons[currentIndex] == "a" && Input.GetAxisRaw("Vertical") == -1) ||
                    (buttons[currentIndex] == "d" && Input.GetAxisRaw("Horizontal") == 1) ||
                    (buttons[currentIndex] != "s" && buttons[currentIndex] != "w" && buttons[currentIndex] != "a" && buttons[currentIndex] != "d" && Input.GetButtonDown(buttons[currentIndex])))
                    {
                        timeLastButtonPressed = Time.time;
                        currentIndex++;
                    }

                    if (currentIndex >= buttons.Length)
                    {
                        currentIndex = 0;
                        return true;
                    }
                    else return false;
                }
            }

            return false;
        }
    }
}
