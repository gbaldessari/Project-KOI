using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    private bool isKeyboard = true;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed) isKeyboard = true;
        if (Gamepad.current.aButton.isPressed || Gamepad.current.bButton.isPressed || Gamepad.current.xButton.isPressed || Gamepad.current.yButton.isPressed ||
        Gamepad.current.startButton.isPressed || Gamepad.current.selectButton.isPressed || Gamepad.current.rightTrigger.isPressed || Gamepad.current.leftTrigger.isPressed ||
        Gamepad.current.rightShoulder.isPressed || Gamepad.current.leftShoulder.isPressed || Gamepad.current.dpad.left.isPressed || Gamepad.current.dpad.right.isPressed ||
        Gamepad.current.dpad.up.isPressed || Gamepad.current.dpad.down.isPressed) isKeyboard = false;

        if (isKeyboard) Cursor.visible = true;
        else Cursor.visible = false;
    }
}
