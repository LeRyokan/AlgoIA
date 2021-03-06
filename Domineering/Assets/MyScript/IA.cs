﻿using UnityEngine;
using System.Collections;

public class IA : MonoBehaviour 
{

	[SerializeField]
	BoardManagerScript board;

	[SerializeField]
	int profondeur = 1;

	[SerializeField]
	public bool h_or_v = true;

	[SerializeField]
	public bool History = false;

	[SerializeField]
	public bool MinMaxAlphaBeta = false;

	// Use this for initialization
	void Start () 
	{
	
	}

	public static int frame_wait = 0;
	
	// Update is called once per frame
	void Update () 
	{
		// attends 10 frames entre deux actions d'ia
		if (frame_wait < 10) 
		{
			if (board.Player1 == h_or_v) 
			{
				frame_wait++;
			}

			return;
		}

		if (board == null || !board.canPlay(h_or_v ? 1 : 0) ) 
		{
			return;
		}

		int h_or_v_int = h_or_v ? 1 : 0;

		if (board.Player1 == h_or_v) 
		{
			Move move;
			if (MinMaxAlphaBeta)
			{
				if (History)
				{
					move = board.MinMaxAlphaBetaHistory(h_or_v_int, profondeur);
				}
				else 
				{
					move = board.MinMaxAlphaBeta(h_or_v_int, profondeur);
				}
			}
			else 
			{
				if (History)
				{
					move = board.NegaMoveHistory(h_or_v_int, profondeur);
				}
				else 
				{
					move = board.NegaMove(h_or_v_int, profondeur);
				}
			}

			board.DoMove(move);
			board.Player1 = !board.Player1;
			frame_wait = 0;
		}

	}
}
