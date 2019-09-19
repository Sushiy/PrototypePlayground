using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputListener
{
    void OnMoveInput(Vector2 moveInput);
    void OnJumpInput();
    void OnFireInput(bool inputDown);
    void OnRetractInput(bool inputDown);
    void OnCursorMove(Vector2 cursorPosition);
}
