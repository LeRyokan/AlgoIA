﻿using UnityEngine;
using System.Collections;

public class CellManagerScript : MonoBehaviour
{
    [SerializeField]
    public int posX;

    [SerializeField]
    public int posY;

    [SerializeField]
    BoardManagerScript boardManagerScript;

    CellManagerScript myScript;

    public Material Mat;

    public bool isClicked;

    // Use this for initialization
    void Start()
    {
        Mat = GetComponent<Renderer>().material;
        isClicked = false;
        posX = (int)(transform.position.x + 3.5);
        posY = (int)(3.5 - transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        boardManagerScript.ShowCellsToColor(posX, posY);
    }

    void OnMouseExit()
    {
        boardManagerScript.VoidColorOnMouseExit(posX, posY);
    }

    void OnMouseDown()
    {
        boardManagerScript.ColorCells(posX, posY);
    }


}
