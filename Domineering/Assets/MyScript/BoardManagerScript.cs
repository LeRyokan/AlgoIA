using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManagerScript : MonoBehaviour
{

    bool Player1 = true;

    CellManagerScript ClickedCell;
    CellManagerScript NearCellToClicked;

    [SerializeField]
    List<CellManagerScript> ListOfCell;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ColorCells(int posX, int posY)
    {
        Debug.Log("Position clicked, X:" + posX + " , Y: " + posY);

        //Récupérer la cellule
        ClickedCell = getCellFromPos(posX , posY );
        

        NearCellToClicked = getCellFromPos(posX + (Player1 ? 1 : 0), posY + (Player1 ? 0 : 1));


        if (!ClickedCell.isClicked || !NearCellToClicked.isClicked)
        {
            ClickedCell.Mat.color = (Player1 ? Color.cyan : Color.yellow);
            ClickedCell.isClicked = true;

            NearCellToClicked.Mat.color = (Player1 ? Color.cyan : Color.yellow);
            NearCellToClicked.isClicked = true;
            Player1 = !Player1;
        }



    }





    public void ShowCellsToColor(int posX, int posY)
    {
        Debug.Log("Position clicked, X:" + posX + " , Y: " + posY);

        
        //Récupérer la cellule
        ClickedCell = getCellFromPos(posX , posY );
       

        NearCellToClicked = getCellFromPos(posX + (Player1 ? 1 : 0), posY + (Player1 ? 0 : 1));



        if(!ClickedCell.isClicked && !NearCellToClicked.isClicked)
        {
            ClickedCell.Mat.color = (Player1 ? Color.magenta : Color.green);
            NearCellToClicked.Mat.color = (Player1 ? Color.magenta : Color.green);
        }


    }

    public void VoidColorOnMouseExit(int posX, int posY)
    {
        Debug.Log("Position clicked, X:" + posX + " , Y: " + posY);

        //Récupérer la cellule
        ClickedCell = getCellFromPos(posX, posY);
        
        
        NearCellToClicked = getCellFromPos(posX + (Player1 ? 1 : 0), posY + (Player1 ? 0 : 1));

        if (!ClickedCell.isClicked && !NearCellToClicked.isClicked)
        {
            ClickedCell.Mat.color = Color.white;
            NearCellToClicked.Mat.color = Color.white;
        }

    }




    public CellManagerScript getCellFromPos(int X, int Y)
    {
        foreach (CellManagerScript cellScript in ListOfCell)
        {
            if (X == cellScript.posX && Y == cellScript.posY)
            {
                return cellScript;
            }
        }
        return null;
    }

}
