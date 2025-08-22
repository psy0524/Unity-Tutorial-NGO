using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    private Vector3 moveInput;

    private void Update()
    {
        transform.position += moveInput * 3f * Time.deltaTime;
    }

    private void OnMove(InputValue value)
    {
        var moveValue = value.Get<Vector2>();

        moveInput = new Vector3(moveInput.x, 0, moveValue.y);
    }
}
