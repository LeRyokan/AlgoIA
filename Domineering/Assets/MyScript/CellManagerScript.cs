using UnityEngine;
using System.Collections;

public class CellManagerScript : MonoBehaviour {
/*
    [SerializeField]
    int posX;

    [SerializeField]
    int posY;
*/
    [SerializeField]
    BoardManagerScript boardManagerScript;

    Material Mat;

    bool isClicked;

	// Use this for initialization
	void Start () {
        Mat = GetComponent<Renderer>().material;
        isClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        if (!isClicked)
        {
            Debug.Log("On est au dessus du lot");
            Mat.color = Color.magenta;
        }
    }

    void OnMouseExit()
    {
        if (!isClicked)
        {
            Debug.Log("On SORT");
            Mat.color = Color.white;
        }
    }

    void OnMouseDown()
    {
        isClicked = true;
        Mat.color = Color.cyan;
        boardManagerScript.YOLO();
    }
}
