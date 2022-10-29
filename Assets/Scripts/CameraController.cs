using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera myCamera;
    private CinemachineTransposer myTransposer;
    private Vector3 myTempOffset;

    private void Awake()
    {
        myTransposer= myCamera.GetCinemachineComponent<CinemachineTransposer>();
        myTempOffset=myTransposer.m_FollowOffset;
    }
    void Update()
    {
        MoveLook();
        RotationLook();
        ZoomInOut();
    }

    private void MoveLook()
    {
        var inputMovedir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMovedir.z = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMovedir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMovedir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMovedir.x = +1f;
        }

        //var moveDir=transform.forward * inputMovedir.z + transform.right * inputMovedir.x;
        transform.Translate(inputMovedir * 10 * Time.deltaTime);
    }

    private void RotationLook()
    {
        var rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y -= 1f;
        }

        transform.eulerAngles += rotationVector * 40 * Time.deltaTime;
    }

    private void ZoomInOut()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            myTempOffset.y -= 1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            myTempOffset.y += 1f;
        }
        myTempOffset.y = Mathf.Clamp(myTempOffset.y, 2f, 10f);
        myTransposer.m_FollowOffset = Vector3.Lerp(myTransposer.m_FollowOffset, myTempOffset, Time.deltaTime);
    }
}
