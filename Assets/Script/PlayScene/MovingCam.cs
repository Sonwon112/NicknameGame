using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingCam : MonoBehaviour
{
    public float movingSpeed = 1f;
    public float fastMovingSpeed = 2f;
    public float rotateSpeed = 1f;

    private float currSpeed;
    private bool canMove = false;
    private CinemachineVirtualCamera virtualCamera;
    private float xRotate, yRotate;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        currSpeed = movingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float upAndDown = Input.GetAxis("UpAndDown");
            if (Input.GetKeyDown(KeyCode.LeftShift)) currSpeed = fastMovingSpeed;
            else if (Input.GetKeyUp(KeyCode.LeftShift)) currSpeed = movingSpeed;

            transform.Translate(Vector3.forward * vertical * currSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * horizontal * currSpeed * Time.deltaTime);
            transform.Translate(movingSpeed * Time.deltaTime * upAndDown * Vector3.up);

            float mouseRotateX = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed * -1;
            float mouseRotateY = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
            // 캐릭터 회전 및 카메라 회전
            yRotate = yRotate + mouseRotateY;
            xRotate = xRotate + mouseRotateX;
            xRotate = Mathf.Clamp(xRotate, -90, 40);
            //yRotate = Mathf.Clamp(yRotate, -90, 90);
            transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        }
    }

    public void ToggleMovement(bool move)
    {
        canMove = move;
        switch (canMove)
        {
            case true:
                virtualCamera.Priority = 11;
                break;
            case false:
                virtualCamera.Priority = 10;
                break;
        }
    }

}
