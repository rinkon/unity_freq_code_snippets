using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCPlayerController : MonoBehaviour
{
    private CharacterController controller;
    private enum SIDE {Left, Mid, Right}
    private SIDE m_Side = SIDE.Mid;
    private bool SwipeLeft, SwipeRight, SwipeUp;
    private float newXPosition;
    private float xLimit = 1.25f;
    private float x;
    private float zSpeed = 7;
    private float xSpeed = 10;
    private float jumpSpeed = 7;
    private float gravity = 20;
    private float yPosition;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        SwipeLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        SwipeRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        SwipeUp = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        

        if(SwipeLeft){
            if(m_Side == SIDE.Mid){
                newXPosition = -xLimit;
                m_Side = SIDE.Left;
            }
            if(m_Side == SIDE.Right){
                newXPosition = 0;
                m_Side = SIDE.Mid;
            }
        }
        else if(SwipeRight){
            if(m_Side == SIDE.Mid){
                newXPosition = xLimit;
                m_Side = SIDE.Right;
            }
            if(m_Side == SIDE.Left){
                newXPosition = 0;
                m_Side = SIDE.Mid;
            }
        }
        if (SwipeUp && controller.isGrounded){
            yPosition = jumpSpeed;
        }
        else if(!controller.isGrounded){
            yPosition -= gravity * Time.deltaTime;
        }

        x = Mathf.Lerp(x, newXPosition, xSpeed * Time.deltaTime);
        controller.Move((x - transform.position.x) * Vector3.right + Vector3.forward * zSpeed * Time.deltaTime + (yPosition- transform.position.y) * Time.deltaTime * Vector3.up);
    }
}
