using UnityEngine;
using System.Collections;

public class IA : MonoBehaviour 
{

	[SerializeField]
	BoardManagerScript board;

	[SerializeField]
	int profondeur = 1;

	[SerializeField]
	public bool h_or_v = true;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (board == null || !board.canPlay(h_or_v ? 1 : 0) ) 
		{
			return;
		}

		if (board.Player1 == h_or_v) 
		{
			Move move = board.NegaMove(h_or_v ? 1 : 0, profondeur);
			board.DoMove(move);
			board.Player1 = !board.Player1;
		}

	}
}
