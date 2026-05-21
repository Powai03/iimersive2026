using UnityEngine;

public class JumpClavier : MonoBehaviour
{
    private CharacterController cc;
    private Vector3 velocity;
    public float jumpHeight = 2f;
    private float gravity = -9.81f;

    void Start() { cc = GetComponent<CharacterController>(); }

    void Update()
    {
        if (cc.isGrounded && velocity.y < 0) velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}