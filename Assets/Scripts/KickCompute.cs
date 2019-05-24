using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KickCompute : MonoBehaviour
{   
    private float MaxAngle;
    private DateTime StartTime;
    private DateTime EndTime;
    private GameObject VectorReference;
    [SerializeField]
    private int KickPower;

    [SerializeField]
    private Vector3 KickVector;
    private Vector3 KickAngleNormal;
    

    public bool BeginKick=false;
    public bool EndKick=false;

    void Start()
    {
        VectorReference = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        KickVector = VectorReference.GetComponent<BodySourceView>().R_JointToTrackPos;

        if(!BeginKick && KickVector.z<-1.7f)
        {
            BeginKick = true;
            StartTime = DateTime.UtcNow;

            Debug.Log("started");
        }

        if(BeginKick && !EndKick && KickVector.z>1.7f)
        {
            EndKick=true;
            EndTime = DateTime.UtcNow;

            PerformKick();

            Debug.Log("end");
        }

    }

    private void PerformKick()
    {
        int _timedel = (EndTime - StartTime).Seconds;

        KickAngleNormal = Vector3.Cross(KickVector, Vector3.up).normalized;

        Debug.Log(KickAngleNormal);

        KickAngleNormal = new Vector3(KickAngleNormal.x, KickAngleNormal.y, -KickAngleNormal.z);

        GameObject _futball = GameObject.FindGameObjectWithTag("football");
        _futball.GetComponent<Rigidbody>().AddForce(KickAngleNormal*KickPower);
    }



}