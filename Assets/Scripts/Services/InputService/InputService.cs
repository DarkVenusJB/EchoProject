using UnityEngine;

namespace Services.InputService
{
    public class InputService
    {
        public float MoveInput => Input.GetAxisRaw("Horizontal");

        public bool JumpInput => Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump");
        
        public bool DashInput => Input.GetKeyDown(KeyCode.Space);

        public bool InteractInput => Input.GetKeyDown(KeyCode.E);
    }
}