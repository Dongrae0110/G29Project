using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Wheel_Front : MonoBehaviour
{
    public float ro = 0; // �ڵ� ȸ���� ���� ���� �������� �����ϴ� ����
    public int speed = 0;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        LogitechGSDK.DIJOYSTATE2ENGINES rec;
        rec = LogitechGSDK.LogiGetStateUnity(0);
        ro = rec.lX / 819;
        transform.rotation = Quaternion.Euler(0, ro, 0);
        Debug.Log(speed);
    }
}
