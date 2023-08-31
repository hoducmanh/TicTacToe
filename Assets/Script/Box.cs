using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isMarked;
    public int index;
    public Mark mark;
    public int row;
    public int col; 
    void Start()
    {
        index = transform.GetSiblingIndex();
        mark = Mark.none;
        //Debug.Log(this.name + " "+ index);
    }

    // Update is called once per frame
    public void SetAsMarked()
    {
        isMarked = true;
    }
    public void UnsetMark()
    {
        isMarked = false;
    }
}
