
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Utility;

public class ThirdPersonInput : MonoBehaviour
{
    public FixedJoystick LeftJoystick;
    public FixedButton Button;
    public FixedTouchField TouchField;
    protected ThirdPersonUserControl Control;

    public GameObject AimLookAt;

    protected float CameraAngle;
    protected float CameraAngleSpeed = 0.2f;

    [SerializeField]
    private float rotateSpeedY = 3f;

    public Transform Target, Player;

    Quaternion temp;

    // Use this for initialization
    void Start()
    {
        temp = Quaternion.Euler(0, 0, 0);
        Control = GetComponent<ThirdPersonUserControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Control.m_Jump = Button.Pressed;
        Control.Hinput = LeftJoystick.Horizontal;
        Control.Vinput = LeftJoystick.Vertical;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
#else
        if (LeftJoystick.Pressed)
#endif
        {
            temp = Player.transform.rotation;
            TouchField.Pressed = false;
        }
        else if (Control.m_Jump)
        {
            TouchField.Pressed = false;
        }
        else
        {
            if (TouchField.Pressed == false)
            {
#if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
                {
                    TouchField.Pressed = true;
                    Debug.Log("@@ echaekim : Editor Pressed false touch to true");
                }
#else
                if(Input.touches[0].phase == TouchPhase.Began)
                {
                    TouchField.Pressed = true;
                    Debug.Log("@@ echaekim : Pressed false touch to true");
                }
#endif
            }

            if (!TouchField.isDrag && TouchField.Pressed && Input.GetMouseButtonUp(0)) // 터치 한 번만 할 때만 나오도록 할것
            {
                LookMouseCursor();
                Debug.Log("@@ echaekim LookMouseCursor");
            }

            if (temp != Quaternion.Euler(0, 0, 0))
            {
                Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, temp, 10.0f * Time.deltaTime);
            }
        }


        Camera.main.transform.LookAt(Target);
        float y = Mathf.Clamp(TouchField.currentY * rotateSpeedY, -70, 60);
        Target.transform.rotation = Quaternion.Euler(y, TouchField.currentX * 3f, 0);

    }

    public void LookMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;
        if (Physics.Raycast(ray, out hitResult))
        {
            AimLookAt.transform.position = hitResult.point;
            Debug.DrawRay(Player.transform.position, hitResult.point, Color.black);
            
            Vector3 mouseDir = new Vector3(hitResult.point.x, Player.transform.position.y, hitResult.point.z) - Player.transform.position;
            Debug.DrawRay(Player.transform.position, mouseDir, Color.red);

            Quaternion OriginalRot = Player.transform.rotation;
            Player.transform.LookAt(mouseDir);
            Player.transform.forward = mouseDir;
            Quaternion NewRot = Player.transform.rotation;
            Player.transform.rotation = OriginalRot;
            temp = NewRot;
        }
    }
}