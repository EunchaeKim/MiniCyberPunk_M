using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public bool Pressed;
    [HideInInspector]
    public bool PressedUp;

    public Transform Target, Player;

    public float RotationSpeed = 1;
    public float currentX = 0.0f;
    public float currentY = 0.0f;

    public bool isDrag = false;

    private float touchTime = 0.0f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // TODO : touch랑 OnPointerInterface 둘 중 어떤걸 쓸지 정할 것. touch는 모든 범위의 터치를 입력받고, OnPointer는 FixedTouchField 범위의 이미지만 입력받음.
#if !UNITY_EDITOR

        Touch touch = Input.touches[0];

        if(touch.phase == TouchPhase.Began)
        {
            touchTime = Time.time;
        }

        if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            if(Time.time - touchTime <= 0.5f)
            {
                Pressed = true;
                Debug.Log("@@ echaekim : Pressed true");
            }
            else
            {
                Pressed = false;
                Debug.Log("@@ echaekim : Pressed false");
            }
        }

#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_EDITOR
        touchTime = Time.time;
#endif
    }


    public void OnPointerUp(PointerEventData eventData)
    {
#if UNITY_EDITOR
        if(Time.time - touchTime <= 0.5f)
        {
            Pressed = true;
            Debug.Log("@@ echaekim : Editor Pressed true");
        }
        else
        {
            Pressed = false;
            Debug.Log("@@ echaekim : Pressed false");
        }
#endif
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("@@ echaekim : Begin Drag");
        isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("@@ echaekim : on Drag");
        isDrag = true;
        currentX += Input.GetAxis("Mouse X") * RotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        currentY = Mathf.Clamp(currentY, -35, 50);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("@@ echaekim : end Drag");
        isDrag = false;
        Pressed = false;
    }
}