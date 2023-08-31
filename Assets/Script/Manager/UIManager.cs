using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DemoObserver;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public List<Box> boxes;
    [SerializeField] private TMP_Text winner;
    [SerializeField] private GameObject winText;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject MainMenuScreen;
    [SerializeField] private GameObject Tiles;
    private int Xscore = 0;
    private int Oscore = 0;
    [SerializeField] private TMP_Text XscoreText;
    [SerializeField] private TMP_Text OscoreText;
    public static UIManager Instance;
    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void OutputWinner(Mark mark)
    {
        if (mark == Mark.x)
        {
            winner.text = "X player won";
            Xscore++;
            XscoreText.text = Xscore.ToString();
        }

        else
        {
            winner.text = "O player won";
            Oscore++;
            OscoreText.text = Oscore.ToString();
        }
            
    }
    public void OutputDraw()
    {
        winner.text = "Draw";
        WinScreen.SetActive(true);
    }

    public void OnClickRestartButton()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].gameObject.layer = 6;
            boxes[i].isMarked = false;
        }
        winner.text = "";
        lineRenderer.enabled = false;
        this.PostEvent(EventID.OnRestart);
        WinScreen.SetActive(false);
    }
    public void OnclickPlayButton()
    {
        MainMenuScreen.SetActive(false);
        Tiles.SetActive(true);
    }
    public void OnClickTurnBackButton()
    {
        Debug.Log("here");
        if(RaycastFindTile.Instance.boxStack.Count > 0 && GameManager.Instance.isPlaying == true)
        {
            this.PostEvent(EventID.OnTurnback);
        }
        
    }
    public void DrawLine(int firstBox, int lastBox)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, boxes[firstBox].transform.position);
        lineRenderer.SetPosition(1, boxes[lastBox].transform.position);
    }
    public void OnWinScreen()
    {
        WinScreen.SetActive(true);
        
    }
}
