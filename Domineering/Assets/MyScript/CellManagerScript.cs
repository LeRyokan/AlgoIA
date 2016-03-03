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

    CellManagerScript myScript;

    public Material Mat;

    public int posX;
    public int posY;

    public bool isClicked;

	// Use this for initialization
	void Start () {
        Mat = GetComponent<Renderer>().material;
        isClicked = false;
        posX = (int)(transform.position.x + 3.5);
        posY = (int)(transform.position.y + 3.5);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        if (!isClicked)
        {
            Debug.Log("On est au dessus du lot");
          //  Mat.color = Color.magenta;
            boardManagerScript.ShowCellsToColor(posX, posY);
        }
    }

    void OnMouseExit()
    {
        if (!isClicked)
        {
            Debug.Log("On SORT");
           // Mat.color = Color.white;
            boardManagerScript.VoidColorOnMouseExit(posX, posY);
        }
    }

    void OnMouseDown()
    {
        if (!isClicked)
        {
            isClicked = true;
           // Mat.color = Color.cyan;
            boardManagerScript.ColorCells(posX, posY);
        }
       // boardManagerScript.getCellFromPos(posX,posY);

    }

    
}
