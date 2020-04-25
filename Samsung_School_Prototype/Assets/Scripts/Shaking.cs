using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Shaking : MonoBehaviour
{
    float mouseX;
    float score;
    float got;
    bool finished;
    public string reward_name;
    public GameObject reward_mesh;
    // Start is called before the first frame update
    void Start()
    {
        mouseX = Input.GetAxis("Mouse X");
        score = 0;
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(score);
        if (score >= 100) got = 1;

        if ((Input.GetAxis("Mouse X") != mouseX) && (got == 0))
        {
            StopReactive(false);
            score += 0.7f;
            transform.localRotation = Quaternion.Euler((float)Math.Sin(score) * 10, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z), 5f * Time.deltaTime);
            if (transform.eulerAngles.x < 0.5f)
            {
                score = 0;
                if (got != 0)
                {
                    score = 0;
                    FindObjectOfType<RigidbodyFirstPersonController>().enabled = true;
                    GetComponent<Shaking>().enabled = false;
                    if (!finished)
                    {
                        FindObjectOfType<PlayerQuestStuff>().get_Reward(GetComponent<Shaking>());
                        finished = true;
                    }
                    StopReactive(true);
                }
            }
            if (score != 0)
            {
                score -= 0.02f;
            }
        }
    }

    void StopReactive(bool state)
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            ReactiveTarget ch =  child.gameObject.GetComponent<ReactiveTarget>();
            if (ch!= null) ch.enabled = state;
        }
    }
}