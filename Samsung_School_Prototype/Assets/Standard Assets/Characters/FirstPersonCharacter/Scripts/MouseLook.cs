using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;
        public bool click = false;
        public bool inv_button = false;
        public int down_button = 0;
        List<float> startTouch = new List<float>();
        List<bool> rotateTouch = new List<bool>();
        List<Vector2> positionTouch = new List<Vector2>();
        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;
        public float k = 0.2f;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        public void setRotateTouchFalse()
        {
            for(int i = 0; i < rotateTouch.Count; i++)
            {
                rotateTouch[i] = false;
            }
        }


        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = 0;
            float xRot = 0;
            click = false;
            //Touch touch = Input.GetTouch(Input.touches.Length-1);
            foreach (Touch touch in Input.touches)
            {

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (Input.touches.Length > startTouch.Count)
                        {
                            startTouch.Add(0);
                            positionTouch.Add(new Vector2());
                            rotateTouch.Add(false);
                        }
                        startTouch[touch.fingerId] = Time.time;
                        positionTouch[touch.fingerId] = touch.position;
                        if ((touch.position.x > Screen.width * 0.3f) || (touch.position.y > Screen.height * 0.3f))
                        {
                            rotateTouch[touch.fingerId] = true;
                        }
                        break;
                    case TouchPhase.Moved:
                        if (rotateTouch[touch.fingerId])
                        {
                            yRot = touch.deltaPosition.x * k;
                            xRot = touch.deltaPosition.y * k;
                        }
                        break;
                    case TouchPhase.Ended:
                        if ((Time.time - startTouch[touch.fingerId] < 0.3f) && (Math.Abs(touch.position.x - positionTouch[touch.fingerId].x) < Screen.width / 50f) && (Math.Abs(touch.position.y - positionTouch[touch.fingerId].y) < Screen.height / 50f))
                        {
                            if (!inv_button && (down_button == 0))
                            {
                                Debug.Log("click");
                                click = true;
                            }
                        }

                        rotateTouch[touch.fingerId] = false;
                        break;
                }
            }
            if (down_button == 2) down_button = 0;
            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);
            
            //float yRot = CrossPlatformInputManager.GetAxis("Mouse sX") * XSensitivity;
            //float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

            

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

            //UpdateCursorLock();
        }

        //public void SetCursorLock(bool value)
        //{
        //    lockCursor = value;
        //    if(!lockCursor)
        //    {//we force unlock the cursor if the user disable the cursor locking helper
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //    }
        //}

        //public void UpdateCursorLock()
        //{
        //    //if the user set "lockCursor" we check & properly lock the cursos
        //    if (lockCursor)
        //        InternalLockUpdate();
        //}

        //private void InternalLockUpdate()
        //{
        //    if(Input.GetKeyUp(KeyCode.Escape))
        //    {
        //        m_cursorIsLocked = false;
        //    }
        //    else if(Input.GetMouseButtonUp(0))
        //    {
        //        m_cursorIsLocked = true;
        //    }

        //    //if (m_cursorIsLocked)
        //    //{
        //    //    Cursor.lockState = CursorLockMode.Locked;
        //    //    Cursor.visible = false;
        //    //}
        //    //else if (!m_cursorIsLocked)
        //    //{
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //    //}
        //}

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
