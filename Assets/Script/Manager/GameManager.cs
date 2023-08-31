using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using DemoObserver;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject X;
    [SerializeField] private GameObject O;
    [SerializeField] private Transform markParent;
    [SerializeField] private bool isXturn;
    [SerializeField] private bool isOturn; 
    [SerializeField] private GameObject ingameX;
    [SerializeField] private GameObject ingameXblur;
    [SerializeField] private GameObject ingameO;
    [SerializeField] private GameObject ingameOblur;
    private Action<object> EventOnRestart;
    private Action<object> EventOnTurnback;
    public static GameManager Instance;
    public Stack<GameObject> objStack = new Stack<GameObject>();
    public bool isPlaying;
    private void Awake()
    {
        isPlaying = true;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        EventOnRestart = (param) => Restart();
        this.RegisterListener(EventID.OnRestart, EventOnRestart);
        EventOnTurnback = (param) => Turnback();
        this.RegisterListener(EventID.OnTurnback, EventOnTurnback);
    }
    private void Update()
    {
        if(isXturn)
        {
            ingameX.SetActive(true);
            ingameOblur.SetActive(true);
            ingameO.SetActive(false);
            ingameXblur.SetActive(false);
        }
        if(isOturn)
        {
            ingameX.SetActive(false);
            ingameOblur.SetActive(false);
            ingameO.SetActive(true);
            ingameXblur.SetActive(true);
        }
    }
    private void PutX(Vector3 pos)
    {
        GameObject tmp = Instantiate(X, pos, Quaternion.identity, markParent);
        objStack.Push(tmp);
        isXturn = false;
        isOturn = true;
    }
    private void PutO(Vector3 pos)
    {
        GameObject tmp = Instantiate(O, pos, Quaternion.identity, markParent);
        objStack.Push(tmp);
        isOturn = false;
        isXturn = true;
    }
    public void PutXO(Vector3 pos)
    {
        if(isXturn)
        {
            PutX(pos);
        }
        else
        {
            PutO(pos);
        }
    }
    private void DestroyAllMark()
    {
        while(objStack.Count > 0)
        {
            GameObject tmp = objStack.Pop();
            Destroy(tmp);
            Debug.Log(objStack.Count);
        };
    }
    private void ResetTurn()
    {
        isXturn = true;
        isOturn = false;
    }
    private void Turnback()
    {
        GameObject tmp = objStack.Pop();
        Destroy(tmp);
        if(isOturn)
        {
            isXturn = true;
            isOturn= false;
        }
        else
        {
            isXturn = false;
            isOturn = true;
        }
    }
    private void Restart()
    {
        DestroyAllMark();
        ResetTurn();
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnRestart, EventOnRestart);
        EventDispatcher.Instance.RemoveListener(EventID.OnTurnback, EventOnTurnback);
    }
}
