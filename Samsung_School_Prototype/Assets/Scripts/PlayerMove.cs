using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerMove : MonoBehaviour
{ 
    float[] positions = new float[2];
    bool getUp; 
    bool isCrouching = false; 
    float newHeight;
    float oldHeight;
    CapsuleCollider collider; 
    float deltaPosition = 15f;
    MouseLook controlScript;

    void Start()
    {
        getUp = true;
        //isCrouching = false;
        collider = GetComponent<CapsuleCollider>();
        positions[1] = collider.height;
        positions[0] = positions[1] - deltaPosition;
        newHeight = positions[1];
        oldHeight = newHeight;
        controlScript = FindObjectOfType<RigidbodyFirstPersonController>().mouseLook;
    }


    void Update()
    {
        getUp = !Physics.Raycast(new Vector3(transform.position.x, collider.height, transform.position.z), Vector3.up, 2f); //выпускает луч на расстояние 2ф из положения верхней точки коллайдера вверх. Возвращет неверно, если нет столкновений
        collider.height = Mathf.Lerp(collider.height, newHeight, 0.05f);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (collider.height - oldHeight) / 2, transform.localPosition.z);
        oldHeight = collider.height;
        getUp = true;
        if (controlScript.down_button) 
        {
            controlScript.down_button = false;
            Debug.Log(isCrouching);
            if (!isCrouching) //если игрок не крадётся, то есть стоит
            {
                newHeight = positions[0];
                isCrouching = true;
            }
            else if (getUp) //если крадётся и может встать
            {
                newHeight = positions[1];
                isCrouching = false;
            }
        }
    }
}
