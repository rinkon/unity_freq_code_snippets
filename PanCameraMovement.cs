using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanCameraMovement : MonoBehaviour
{
    private Vector2 lastPosition;


    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary){
                lastPosition = touch.position;
            }
            else if(touch.phase == TouchPhase.Moved){
                Vector2 direction = (touch.position - lastPosition).normalized;
                
                Vector3 realDirection = new Vector3(-direction.y, 0, direction.x);
                float cameraYRotation = transform.eulerAngles.y;
                realDirection = Quaternion.Euler(0, (360 - cameraYRotation), 0) * realDirection;
                
                transform.position += realDirection * 5 * Time.deltaTime;
                // clamp camera movement area
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, -26.6f, 14f),
                    transform.position.y,
                    Mathf.Clamp(transform.position.z, -46f, 14f)
                );
            }
        }
    }
}
