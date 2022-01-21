using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    Camera characterCamera;

    public GameObject bulletPrefab;

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 firePos = transform.position + animator.transform.forward + new Vector3(0f, 0.5f, 0f);
            var bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity).GetComponent<Bullet>();
            bullet.Fire(animator.transform.forward);
        }
    }

    private void Awake()
    {
        characterCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //float moveX = 0f;
        //float moveZ = 0f;

        //if(Input.GetKey(KeyCode.W))
        //{
        //    moveZ += 1f;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    moveZ -= 1f;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    moveX -= 1f;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    moveZ += 1f;
        //}

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(moveX, 0f, moveZ);
        // 무브 벡터의 길이가 0이 아니면 키 입력이 들어오는 것으로 판정
        bool isMove = moveVector.magnitude > 0;
        // 애니메이터의 isMove의 값을 무브 벡터의 길이에 따라서 바뀌도록 함
        animator.SetBool("isMove", isMove);
        LookMouseCursor();
        Fire();
        // 유니티 엔진 1 단위는 1미터
        transform.Translate(new Vector3(moveX, 0f, moveZ).normalized * Time.deltaTime * 5f);
    }

    public void LookMouseCursor()
    {
        Ray ray = characterCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;
        if(Physics.Raycast(ray, out hitResult))
        {
            Vector3 mouseDir = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
            animator.transform.forward = mouseDir;
        }
    }
}
