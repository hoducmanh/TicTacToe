using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DemoObserver;
using System;

public class RaycastFindTile : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask inactiveMask;
    [SerializeField] private GameObject circle;
    private Mark currentMark;
    public Mark[,] marks;
    public Mark winPlayer = Mark.none;
    public int rowNumber;
    public int colNumber;
    public int numberOfMarkToWin;
    public Stack<Box> boxStack = new Stack<Box>();
    private int cnt;
    private Action<object> EventOnRestart;
    private Action<object> EventOnTurnback;
    public static RaycastFindTile Instance;
    private void Awake()
    {
        //boxes = new List<Box>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        EventOnRestart = (param) => Restart();
        this.RegisterListener(EventID.OnRestart, EventOnRestart);
        EventOnTurnback = (param) => Turnback();
        this.RegisterListener(EventID.OnTurnback, EventOnTurnback);
    }
    void Start()
    {
        currentMark = Mark.x;
        marks = new Mark[3,3];
        
        for (int i = 0; i < rowNumber; i++)
        {
            for(int j = 0; j < colNumber; j++) marks[i, j] = Mark.none;
        }
    }
    void Update()
    {
        Playing();
    }

    private void SetCircle(Vector3 tilePos)
    {
        circle.SetActive(true);
        circle.transform.position = tilePos;
    }
    private void MarkBox(Box box)
    {
        if (box.isMarked != true) 
        {
            cnt++;
            marks[box.row, box.col] = currentMark;
            boxStack.Push(box);
            box.SetAsMarked();            
            box.mark = currentMark;
            WinCheck(currentMark);
            SwitchMark();
        }
    }
    
    public void SwitchMark()
    {
        currentMark = (currentMark == Mark.x) ? Mark.o : Mark.x;
    }
    private void WinCheck(Mark curMark)
    {
        for (int i = 0; i < rowNumber; i++) //check hang
        {
            for(int j = 0; j < colNumber - 2; j++)
            {
                if (marks[i, j] == curMark && marks[i, j + 1] == curMark && marks[i, j + 2] == curMark) 
                {
                    UIManager.Instance.OutputWinner(curMark);
                    UIManager.Instance.DrawLine(i * rowNumber + j, i * rowNumber + j + 2);
                    winPlayer = curMark;
                    GameManager.Instance.isPlaying = false;

                }
            }
        }
        
        for (int j = 0; j < colNumber ;j++) //check cot
        {
            for (int i = 0; i < rowNumber - 2; i++)
            {
                if (marks[i, j] == curMark && marks[i + 1, j] == curMark && marks[i + 2, j] == curMark)
                {
                    UIManager.Instance.OutputWinner(curMark);
                    UIManager.Instance.DrawLine(i * rowNumber + j, (i + 2) * rowNumber  + j);
                    winPlayer = curMark;
                    GameManager.Instance.isPlaying = false;
                }
            }
        }

        for (int j = 0; j < colNumber - 2 ; j++) //check cheo phai
        {
            for (int i = 0; i < rowNumber - 2; i++)
            {
                if (marks[i, j] == curMark && marks[i + 1, j + 1] == curMark && marks[i + 2, j + 2] == curMark)
                {
                    UIManager.Instance.OutputWinner(curMark);
                    UIManager.Instance.DrawLine(i * rowNumber + j, (i + 2) * rowNumber + j + 2);
                    winPlayer = curMark;
                    GameManager.Instance.isPlaying = false;
                }
            }
        }
        for (int i = 2; i - 2 >= 0; i--) //check hang
        {
            for (int j = 0; j < colNumber - 2; j++)
            {
                if (marks[i, j] == curMark && marks[i - 1 , j + 1] == curMark && marks[i - 2, j + 2] == curMark)
                {
                    UIManager.Instance.OutputWinner(curMark);
                    UIManager.Instance.DrawLine(i * rowNumber + j, (i - 2) * rowNumber + j + 2);
                    winPlayer = curMark;
                    GameManager.Instance.isPlaying = false;
                }
            }
        }
        Debug.Log(winPlayer);
        if (winPlayer != Mark.none) UIManager.Instance.OnWinScreen();
        if (cnt == 9 && winPlayer == Mark.none) UIManager.Instance.OutputDraw();
    }

    private void Turnback()
    {
        if(boxStack.Count == 1)
        {
            currentMark = Mark.x;
        }
        else
        {
            SwitchMark();
        }
        Debug.Log(currentMark);
        Box tmpBox = boxStack.Pop();
        tmpBox.isMarked = false;
        tmpBox.mark = Mark.none;
        marks[tmpBox.row, tmpBox.col] = Mark.none;
        cnt--;
        tmpBox.gameObject.layer = 6;
    }
    private void ResetAllBox()
    {
        GameManager.Instance.isPlaying = true;
        while(boxStack.Count > 0)
        {
            currentMark = Mark.x;
            Box tmpBox = boxStack.Pop();
            tmpBox.isMarked = false;
            tmpBox.mark = Mark.none;
            marks[tmpBox.row, tmpBox.col] = Mark.none;           
            tmpBox.gameObject.layer = 6;
        }
        winPlayer = Mark.none;
        cnt = 0;
    }
    private void Playing()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mousePos);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hit.collider != null && GameManager.Instance.isPlaying == true)
        {
            GameObject hitObject = hit.collider.gameObject;
            SetCircle(hit.transform.position);
            if (Input.GetMouseButtonDown(0))
            {
                MarkBox(hit.collider.gameObject.GetComponent<Box>());
                GameManager.Instance.PutXO(hit.transform.position);

                hit.collider.gameObject.layer = 2;

            }
        }
        else
        {
            circle.SetActive(false);
        }
    }
    private void Restart()
    {
        ResetAllBox();
    }
    private void OnDestroy()
    {
        DemoObserver.EventDispatcher.Instance.RemoveListener(EventID.OnRestart, EventOnRestart);
        DemoObserver.EventDispatcher.Instance.RemoveListener(EventID.OnTurnback, EventOnTurnback);
    }
}

