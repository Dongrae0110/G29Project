using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private WheelCollider[] wheelColliders;
    private GameObject[] wheels;
    private Rigidbody rb;

    public float radius = 1f;
    public float ro = 0; // �ڵ� ȸ���� ���� ���� �������� �����ϴ� ����
    public int speed = 0;
    public float power;
    public float rot;
    public bool isBreak;
    private float wheelBase; //�� �� ���� ������ �Ÿ�(m����)
    private float rearTrack; //�� �� ��Ƣ ������ �Ÿ�(m����)
    public float turnRadius; //ȸ�� ������(m����)
    // Start is called before the first frame update
    void Start()
    {
        wheelColliders = new WheelCollider[4];
        wheels = new GameObject[4];
        rb = GetComponentInParent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1, 0); //�����߽��� y�� �������� ����
        power = 100f;
        rot = 45f;

        wheels[0] = GameObject.FindGameObjectWithTag("FRWheel");
        wheels[1] = GameObject.FindGameObjectWithTag("FLWheel");
        wheels[2] = GameObject.FindGameObjectWithTag("RRWheel");
        wheels[3] = GameObject.FindGameObjectWithTag("RLWheel");
        wheelColliders[0] = GameObject.Find("WheelHubFrontRight").GetComponent<WheelCollider>();
        wheelColliders[1] = GameObject.Find("WheelHubFrontLeft").GetComponent<WheelCollider>();
        wheelColliders[2] = GameObject.Find("WheelHubRearRight").GetComponent<WheelCollider>();
        wheelColliders[3] = GameObject.Find("WheelHubRearLeft").GetComponent<WheelCollider>();

        

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].transform.position = wheels[i].transform.position;
        }
        wheelBase = Mathf.Abs(wheelColliders[1].transform.position.z - wheelColliders[3].transform.position.z);
        rearTrack = Mathf.Abs(wheelColliders[2].transform.position.x - wheelColliders[3].transform.position.x);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            // for���� ���ؼ� ���ݶ��̴� ��ü�� Vertical �Է¿� ���� power��ŭ�� ������ �����̰��Ѵ�.
            wheelColliders[i].motorTorque = Input.GetAxis("Vertical") * power;
        }

        SteerVehicle(); //��Ŀ�� ����
        WheelPosWithCollider();  //������ ��ġ�� �����̼� ���� �׻� ���� �������

       
    }
    void Update()
    {
        //LogitechGSDK.DIJOYSTATE2ENGINES rec;
        //rec = LogitechGSDK.LogiGetStateUnity(0);
        //ro = rec.lX / 819;
        //transform.rotation = Quaternion.Euler(0, ro, 0);
        //Debug.Log(speed);
    }

    void WheelPosWithCollider()
    {
        //���� �޾ƿ��� �뵵�� ����
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetWorldPose(out wheelPosition, out wheelRotation); //wheelCollider��  ���� �����ǰ� ���� �����̼� ���� �޾ƿ�
            wheels[i].transform.position = wheelPosition;
            wheels[i].transform.rotation = wheelRotation;
        }
    }
    

    void SteerVehicle()
    {
        //steerAngle = ������ ���� ����
        // ��Ŀ�� ����
        //���� Rad2Deg * Atan(wheelBase in meter / (turnRadius in meters + (rearTrack in meters / 2to get center)) * steerInput  left
        // Rad2Deg* Atan(wheelBase in meter / (turnRadius in meters - (rearTrack in meters / 2to get center)) *steerInput  right
        
        //���� ���������� ȸ��
        if (Input.GetAxis("Horizontal") > 0)
        {   // rear tracks size is set to 1.5f          wheel base has been set to 2.55f
            //Rad2Deg -> ������ ���ȿ��� degree(��)�� ��ȯ
            wheelColliders[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * Input.GetAxis("Horizontal");
            wheelColliders[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * Input.GetAxis("Horizontal");
            Debug.Log(wheelColliders[0].steerAngle);
        }
        //���� �������� ȸ��
        else if (Input.GetAxis("Horizontal") < 0)
        {
            wheelColliders[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * Input.GetAxis("Horizontal");
            wheelColliders[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * Input.GetAxis("Horizontal");
        }
        else
        {
            wheelColliders[0].steerAngle = 0;
            wheelColliders[1].steerAngle = 0;
        }
    }
}
