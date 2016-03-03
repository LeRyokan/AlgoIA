using UnityEngine;
using System.Collections;

public struct Move
{

	public bool legal;
	public int x, y, h_or_v;
	
	
	public Move(bool p_legal) 
	{
		x = y = h_or_v = 0;
		legal = p_legal;
	}
	
	public Move(int p_x, int p_y, int p_h_or_v)
	{
		x = p_x;
		y = p_y;
		h_or_v = p_h_or_v;
		legal = true;
	}
	
	
	public static bool CanMoveBeDone(int p_x, int p_y, int p_h_or_v, BoardManagerScript board)
	{
		int x_plus = (p_h_or_v == 0 ? 1 : 0);
		int y_plus = (p_h_or_v == 1 ? 1 : 0);
		
		if (p_x < 0 || ((p_x + x_plus ) >= board.w) || p_y < 0 || ((p_y + y_plus) >= board.h))
		{
			return false;
		}
		
		if (board.Cells2D[p_x,p_y].isClicked)
		{
			return false;
		}
		
		if (board.Cells2D[p_x + x_plus,p_y + y_plus].isClicked)
		{
			return false;
		}
		
		return true;

	}
	
	public bool CanBeDone(BoardManagerScript b)
	{
		return CanMoveBeDone(x, y, h_or_v, b);
	}
}

