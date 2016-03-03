using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManagerScript : MonoBehaviour
{
	public const int INFINI = (1 << 30);
	public int w = 8;
	public int h = 8;
	public CellManagerScript[,] Cells2D = new CellManagerScript[8,8];
	public Move[] killerMoveTab= new Move[16];


	[SerializeField] 
	bool isThereAHPlayer = true;

	[SerializeField] 
	bool isThereAVPlayer = true;
	

	public bool Player1 = false;

    CellManagerScript ClickedCell;
    CellManagerScript NearCellToClicked;

    [SerializeField]
	List<CellManagerScript> ListOfCell;

	Stack<Move> movesDone = new Stack <Move>();

	void AllocateKillerMoveTab(int size)
	{			
		killerMoveTab = new Move[size];

		for (int i = 0; i < killerMoveTab.Length; i++)
		{
			killerMoveTab[i] = new Move(false);
		}
	}

	public void DoMove(Move move)
	{
		Color color = Player1 ? Color.magenta : Color.green;
		Cells2D[move.x,move.y].isClicked = true;
		Cells2D [move.x, move.y].Mat.color = color;
		Cells2D [move.x + (move.h_or_v == 0 ? 1 : 0), move.y + (move.h_or_v == 1 ? 1 : 0)].isClicked = true;
		Cells2D [move.x + (move.h_or_v == 0 ? 1 : 0), move.y + (move.h_or_v == 1 ? 1 : 0)].Mat.color = color;
		movesDone.Push(move);
	}
	
	public void UndoMove()
	{
		Move move = movesDone.Pop();
		
		Cells2D [move.x,move.y].isClicked = false;
		Cells2D [move.x, move.y].Mat.color = Color.white;
		Cells2D [move.x + (move.h_or_v == 0 ? 1 : 0),move.y + (move.h_or_v == 1 ? 1 : 0)].isClicked = false;
		Cells2D [move.x + (move.h_or_v == 0 ? 1 : 0), move.y + (move.h_or_v == 1 ? 1 : 0)].Mat.color = Color.white;
	}

	List<Move> PossibleMoves(int p_h_or_v)
	{
		List<Move> moves = new List<Move>();
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				if (Move.CanMoveBeDone(i, j, p_h_or_v, this))
				{
					moves.Add(new Move(i, j, p_h_or_v));
				}
			}
		}
		
		return moves;
	}
	
	public bool canPlay(int p_h_or_v)
	{
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				if (Move.CanMoveBeDone(i, j, p_h_or_v, this))
				{
					return true;
				}
			}
		}
		
		return false;
	}

	
	int evaluate(int p_h_or_v)
	{
		int coups_possibles_h = 0, coups_possibles_v = 0;
		
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				if (Move.CanMoveBeDone(i, j, 0, this))
				{
					coups_possibles_h++;
				}
				
				if (Move.CanMoveBeDone(i, j, 1, this))
				{
					coups_possibles_v++;
				}
			}
		}
		
		if (p_h_or_v == 0)
		{
			if (coups_possibles_v == 0)
			{
				return INFINI;
			}
			
			if (coups_possibles_h == 0)
			{
				return -INFINI;
			}
			
			
			return coups_possibles_h - coups_possibles_v;
		}
		else 
		{
			if (coups_possibles_h == 0)
			{
				return INFINI;
			}
			
			if (coups_possibles_v == 0)
			{
				return -INFINI;
			}
			
			return coups_possibles_v - coups_possibles_h;
		}
		
	}
	
	Move GetBestMove(int p_h_or_v)
	{	
		List<Move> moves = PossibleMoves(p_h_or_v);

		int best_evaluation = -1;
		int best_evaluation_index = -1;
		for (int i = 0; i < moves.Count; i++)
		{
			this.DoMove(moves[i]);

			int eva = this.evaluate(p_h_or_v);
			if (best_evaluation_index == -1 || eva > best_evaluation)
			{
				best_evaluation = eva;
				best_evaluation_index = i;
			}
			
			this.UndoMove();
			
			if (eva == INFINI) // mouvement gagnant
			{
				return moves[i];
			}
		}
		
		return moves[best_evaluation_index];
	}
	
	int NegaMaxScore(int p_h_or_v, int depth, int rootdepth, int alpha, int beta, bool min)
	{
		int eva = evaluate(p_h_or_v);
		
		if (depth == 0)
		{
			return eva;
		}
		
		List<Move> moves = PossibleMoves(p_h_or_v);
		
		Move killerMove = killerMoveTab[rootdepth - depth];
		if (killerMove.legal && killerMove.CanBeDone(this))
		{
			moves.Insert(0, killerMove);
		}
		
		if (moves.Count == 0)
		{
			return eva;
		}
		
		for (int i = 0; i < moves.Count; i++)
		{
			this.DoMove(moves[i]);
			int moveScore = -this.NegaMaxScore(p_h_or_v, depth - 1, rootdepth, -beta, -alpha, !min);
			this.UndoMove();
			
			if (moveScore == INFINI)
			{
				return moveScore;
			}
			
			if ( (moveScore > alpha) )
			{
				alpha = moveScore;
				killerMoveTab[rootdepth - depth] = moves[i];
				
				if (alpha >= beta)
				{
					return beta;
				}
			}
		}
		
		return alpha;
	}
	
	public Move NegaMove(int p_h_or_v, int profondeur)
	{
		AllocateKillerMoveTab(profondeur);
		int bestMoveIndex = -1;
		int bestMoveScore = -1;
		int alpha = -INFINI;
		int beta = INFINI;
		
		List<Move> moves = PossibleMoves(p_h_or_v);
		
		for (int i = 0; i < moves.Count; i++) 
		{
			this.DoMove(moves[i]);	
			int moveScore = ((profondeur % 2 == 1) ? 1 :-1 ) * NegaMaxScore(p_h_or_v, profondeur - 1, profondeur, alpha, beta, false);
			this.UndoMove();
			
			if (moveScore == INFINI)
			{
				return moves[i];
			}
			
			if (bestMoveIndex == -1 || moveScore > bestMoveScore)
			{
				bestMoveIndex = i;
				bestMoveScore = moveScore;
				if (bestMoveScore > alpha)
				{
					alpha = bestMoveScore;
					
					if (alpha > beta)
					{
						return moves[i];
					}
				}
			}
		}
		
		return moves[bestMoveIndex];
	}

    // Use this for initialization
    void Start()
    {
		int x = 0;
		int y = 0;

		foreach (var cell in ListOfCell) 
		{
			Cells2D[x,y] = cell;
			x++;
			if (x >= 8)
			{
				x = 0;
				y++;
			}
		}

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ColorCells(int posX, int posY)
    {
		if (!isThereAHPlayer && !Player1 || !isThereAVPlayer && Player1) 
		{
			return;
		}
        Debug.Log("Position clicked, X:" + posX + " , Y: " + posY);

		if (!Move.CanMoveBeDone (posX, posY, Player1 ? 1 : 0, this)) 
		{
			return;
		}

        //Récupérer la cellule
        ClickedCell = getCellFromPos(posX , posY );
        

        NearCellToClicked = getCellFromPos(posX + (!Player1 ? 1 : 0), posY + (!Player1 ? 0 : 1));


        if (!ClickedCell.isClicked && !NearCellToClicked.isClicked)
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
        Debug.Log("Position pointed, X:" + posX + " , Y: " + posY);

		if (!Move.CanMoveBeDone (posX, posY, Player1 ? 1 : 0, this)) 
		{
			return;
		}

        //Récupérer la cellule
        ClickedCell = getCellFromPos(posX , posY );

<<<<<<< HEAD
        NearCellToClicked = getCellFromPos(posX + (!Player1 ? 1 : 0), posY + (!Player1 ? 0 : 1));
=======
        if (Player1)
        {
            if (posX != 7)
            {
                NearCellToClicked = getCellFromPos(posX + (Player1 ? 1 : 0), posY + (Player1 ? 0 : 1));
                if (!ClickedCell.isClicked && !NearCellToClicked.isClicked)
                {
                    ClickedCell.Mat.color = (Player1 ? Color.magenta : Color.green);
                    NearCellToClicked.Mat.color = (Player1 ? Color.magenta : Color.green);
                }
>>>>>>> origin/master

            }
        }
        else
        {
            if (posY != 7)
            {
                NearCellToClicked = getCellFromPos(posX + (Player1 ? 1 : 0), posY + (Player1 ? 0 : 1));
                if (!ClickedCell.isClicked && !NearCellToClicked.isClicked)
                {
                    ClickedCell.Mat.color = (Player1 ? Color.magenta : Color.green);
                    NearCellToClicked.Mat.color = (Player1 ? Color.magenta : Color.green);
                }


<<<<<<< HEAD
        if(NearCellToClicked != null && !ClickedCell.isClicked && !NearCellToClicked.isClicked)
        {
            ClickedCell.Mat.color = (Player1 ? Color.magenta : Color.green);
            NearCellToClicked.Mat.color = (Player1 ? Color.magenta : Color.green);
=======
            }
>>>>>>> origin/master
        }
        



        


    }

    public void VoidColorOnMouseExit(int posX, int posY)
    {
        Debug.Log("Position out, X:" + posX + " , Y: " + posY);

        //Récupérer la cellule
        ClickedCell = getCellFromPos(posX, posY);
<<<<<<< HEAD
        
        
        NearCellToClicked = getCellFromPos(posX + (!Player1 ? 1 : 0), posY + (!Player1 ? 0 : 1));
=======
>>>>>>> origin/master


        if (Player1)
        {
            if (posX != 7)
            {
                NearCellToClicked = getCellFromPos(posX + (Player1 ? 1 : 0), posY + (Player1 ? 0 : 1));

                if (!ClickedCell.isClicked && !NearCellToClicked.isClicked)
                {
                    ClickedCell.Mat.color = Color.white;
                    NearCellToClicked.Mat.color = Color.white;
                }

            }
        }
        else
        {
            if (posY != 7)
            {
                NearCellToClicked = getCellFromPos(posX + (Player1 ? 1 : 0), posY + (Player1 ? 0 : 1));

                if (!ClickedCell.isClicked && !NearCellToClicked.isClicked)
                {
                    ClickedCell.Mat.color = Color.white;
                    NearCellToClicked.Mat.color = Color.white;
                }
            }
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
