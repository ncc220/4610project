using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // private Rigidbody _body;
    // public float speed = 10;
    // public float acceleration = 25;
    // public float jumpVelocity = 4;

    //code from https://www.youtube.com/watch?v=nfX8cnRxrHg
    public Rigidbody _body;

    public float moveSpeed = 6f;
    public float jumpForce = 1f;

    public LayerMask layerMask;

    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        // _body = GetComponent<Rigidbody>();
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     var velocity = _body.velocity;
        //     velocity.y = jumpVelocity;
        //     _body.velocity = velocity;
        // }

        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f, layerMask);


        float x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float y = Input.GetAxisRaw("Vertical") * moveSpeed;


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _body.velocity = new Vector3(_body.velocity.x, jumpForce, _body.velocity.z);
        }

        Vector3 move = transform.right * x + transform.forward * y;


        _body.velocity = new Vector3(move.x, _body.velocity.y,move.z);

    }

    // private void FixedUpdate()
    // {
    //     var targetVelocity = Vector3.zero;
    //     var direction = _body.rotation;
    //     //targetVelocity.w = direction.w;

    //     if(Input.GetKey(KeyCode.W))
    //     {
    //         targetVelocity.z += 1;
    //     }
    //     if(Input.GetKey(KeyCode.S))
    //     {
    //         targetVelocity.z -= 1;
    //     }
    //     if(Input.GetKey(KeyCode.D))
    //     {
    //         targetVelocity.x += 1;
    //     }
    //     if(Input.GetKey(KeyCode.A))
    //     {
    //         targetVelocity.x -= 1;
    //     }

    //     targetVelocity = targetVelocity.normalized;
    //     targetVelocity *= speed;

    //     targetVelocity = new Vector3(targetVelocity.x, _body.velocity.y, targetVelocity.z);

    //     _body.velocity = Vector3.MoveTowards(_body.velocity, targetVelocity, acceleration * Time.deltaTime);
    // }
}
