using UnityEngine;

namespace Services.InputService
{
    public class InputService
    {
        public Vector2 Movement => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        public bool JumpPressed => Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump");

        public bool InteractPressed => Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire3");
    }
}