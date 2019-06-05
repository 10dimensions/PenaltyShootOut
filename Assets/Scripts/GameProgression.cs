using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameProgression : MonoBehaviour
{   
    [Header(" UI ")]
    public Button BeginGameButton;
    public Button ReInitializeBtton;
    public Image FadePanel;
    public GameObject SceneRoot;
    public bool isRenit= false;


    private DateTime StartTime;
    private DateTime EndTime;
    private GameObject VectorReference;


    
    [Header(" Parameters ")]
    [SerializeField]
    private int KickPower;

    private Vector3 L_KickVector;
    private Vector3 R_KickVector;
    private Vector3 KickAngleNormal;
    

    public bool BeginKick=false;
    public bool EndKick=false;

    void Start()
    {
        VectorReference = GameObject.FindGameObjectWithTag("Player");
        
        BeginGameButton.onClick.AddListener(BeginGame);
        ReInitializeBtton.onClick.AddListener(ReInitWrapper);
    }

    public void BeginGame()
    {
        BeginKick = true;
        StartTime = DateTime.UtcNow;

        Debug.Log("started");
    }



    private void ReInitWrapper()
    {   
       if(!isRenit) StartCoroutine(ReInitialize());
    }

    private IEnumerator ReInitialize()
    {   
        isRenit = true;

        FadePanel.gameObject.SetActive(true);
        
       for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1.0f)
        {
            Color newColor = new Color(0f, 0f, 0f, Mathf.Lerp(FadePanel.color.a, 1f, t));
            FadePanel.color = newColor;
            
            yield return null;
        }


        Destroy(GameObject.FindGameObjectWithTag("sceneroot"));

        //yield return new WaitForSeconds(0.5f);

        GameObject _sceneroot =  Instantiate(SceneRoot) as GameObject;
        _sceneroot.SetActive(true);
     
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1.0f)
        {
            Color newColor = new Color(0f, 0f, 0f, Mathf.Lerp(FadePanel.color.a, 0f, t));
            FadePanel.color = newColor;
            
            yield return null;
        }

        FadePanel.gameObject.SetActive(false);

        isRenit = false;    

        BeginKick = false;
        EndKick = false;    

        VectorReference.GetComponent<BodySourceView>().L_JointToTrackPos = Vector3.zero;
        VectorReference.GetComponent<BodySourceView>().R_JointToTrackPos = Vector3.zero;
    }


    void Update()
    {   
        L_KickVector = VectorReference.GetComponent<BodySourceView>().L_JointToTrackPos;
        R_KickVector = VectorReference.GetComponent<BodySourceView>().R_JointToTrackPos;

        if(!BeginKick && R_KickVector.z<0.8f && R_KickVector.z<0.8f)
        {
            BeginKick = true;
            StartTime = DateTime.UtcNow;

            Debug.Log("started");
        }

        if(!BeginKick && L_KickVector.z<0.8f && L_KickVector.z<0.8f)
        {
            BeginKick = true;
            StartTime = DateTime.UtcNow;

            Debug.Log("started");
        }


        if(BeginKick && !EndKick && R_KickVector.z > 1.7f)
        {   
            //StartTime =  DateTime.UtcNow;

            // if(R_KickVector.z > 1.7f)
            // {    }
                EndKick=true;
                EndTime = DateTime.UtcNow;

                PerformKick(R_KickVector);

                Debug.Log("end");
            
        }

        if(BeginKick && !EndKick && L_KickVector.z > 1.7f)
        {   
            
                EndKick=true;
                EndTime = DateTime.UtcNow;

                PerformKick(L_KickVector);

                Debug.Log("end");
            
        }

    }

    private void PerformKick(Vector3 _kickVector)
    {   
        float _range = UnityEngine.Random.Range(0.0f,1.0f);
        string _dir = "";

        if(_range<0.5f) _dir = "left";
        else    _dir="right";

        GameObject.FindGameObjectWithTag("ybot").GetComponent<Animator>().SetBool(_dir, true);

        TimeSpan duration = EndTime.Subtract(StartTime);
        float _timedel = duration.Milliseconds / 1000f;

        KickAngleNormal = Vector3.Cross(_kickVector, Vector3.up).normalized;

        Debug.Log(KickAngleNormal);

        KickAngleNormal = new Vector3(KickAngleNormal.x, (KickAngleNormal.y + 0.4f), -KickAngleNormal.z);

        GameObject _futball = GameObject.FindGameObjectWithTag("football").transform.GetChild(0).gameObject;
        _futball.GetComponent<Rigidbody>().AddForce(KickAngleNormal*KickPower*100f/_timedel);

        Debug.Log(_timedel + " + " + KickAngleNormal*KickPower*1000f/_timedel);
    }


}