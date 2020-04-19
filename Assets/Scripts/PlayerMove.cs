using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour // писал Милованов Еремей
{
    public GameObject cam;  // ссылка на объект камеры
    public Transform[] camPositions; //массив координат положений камеры. Нужен, чтобы переключаться между двумя положениями во время приседания
    private bool getUp; // верно, когда над игроком нет объёктов, и значит он может встать
    private bool isCrouching; // верно, когда игрок крадётся. нужно для проверки приседаний
    private float vertical, horizontal;

    private CapsuleCollider collider; // ссылка на коллайдер этого объекта
    
    void Start()
    {
        getUp = true;
        isCrouching = false;
        collider = GetComponent<CapsuleCollider>();
    }

    
    void Update()
    {
    
        getUp = !Physics.Raycast(new Vector3(transform.position.x, collider.height, transform.position.z), Vector3.up, 2f); //выпускает луч на расстояние 2ф из положения верхней точки коллайдера вверх. Возвращет неверно, если нет столкновений
        if (Input.GetKeyDown(KeyCode.C)) //тип приседания, работают херово
        {
            if (!isCrouching) //если игрок не крадётся, то есть стоит
            {
                collider.height = 15f; 
                Vector3.Lerp(cam.transform.position, camPositions[1].position, 1f);
                collider.center = new Vector3(collider.center.x, -0.25f, collider.center.z);
                isCrouching = true;
                // изменяем высоту коллайдера и положение его центра
            }
            else if (isCrouching && getUp) //если крадётся и может встать
            {
                collider.height = 30F;
                collider.center = new Vector3(0f, 0f, 0f);
                isCrouching = false;
                Vector3.Lerp(cam.transform.position, camPositions[0].position, 1f);
            }
        }     
    }
}
