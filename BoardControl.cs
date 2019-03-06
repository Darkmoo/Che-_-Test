using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControl : MonoBehaviour 
{
	public static BoardControl Instance{ set; get;}
	private bool[,] allowedMoves{ set; get;}

	public Chessman[,] Chessmans{ set; get;}
	private Chessman selectedChessman;

	private const float tile_size = 1.0f;
	private const float tile_offset = 0.0f;

	private int selectionX = -1;
	private int selectionY = -1;
	public int multi = 10;
	public GameObject marker;

	private Quaternion orientation = Quaternion.Euler(0, 180, 0);

	public List<GameObject> chessmanPerfabs;
	public List<GameObject> squarePerfabs;
	private List<GameObject> activeChessman;

	public bool isWhiteTurn = true;

	private void Start()
	{
		Instance = this;
		SpawnBoard ();
		SpawnAllChessmans ();

		Debug.Log (Chessmans[0,0]);
		Debug.Log (Chessmans[7,7]);

		/*SpawnSquare (true, 0, 0);

		SpawnSquare (false, 0, 1);
		SpawnSquare (true, 0, 2);

		SpawnSquare (false, 1, 0);
		SpawnSquare (true, 2, 0);*/

		/*activeChessman = new List<GameObject> ();

		SpawnCheesman (0, 0, 0);
		SpawnCheesman (6, 0, 1);*/
	}

	private void Update()
	{
		UpdateSelection ();
		if (Input.GetMouseButtonDown (0)) 
		{
			if (selectionX >= 0 && selectionY >= 0) {
				if (selectedChessman == null) {
					//select the chessman
					SelectChessman (selectionX/multi, selectionY/multi);
				} else {
					//move the chessman
					MoveChessman (selectionX/multi, selectionY/multi);
				}
			}
		}
	}
		
	private int RoundToDec(int num)
	{
		return  (int)Mathf.Round(num / 10) * 10;
	}

	private void SelectChessman(int x, int y)
	{	
		if (Chessmans [x, y] == null) {			
			return;
		}
		if (Chessmans [x, y].isWhite != isWhiteTurn) {
			return;
		}
			
		allowedMoves = Chessmans [x, y].PossibleMove();
		selectedChessman = Chessmans [x, y];
		Debug.Log("Selected figure "+selectedChessman.name);
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);
		//MarkSquare (x, y);
	}

	private void MoveChessman(int x, int y)
	{
		//bool canMove = CheckMove();
		if (allowedMoves[x,y]) 
		{
			Chessman c = Chessmans[x,y];
			if (c != null && c.isWhite != isWhiteTurn) 
			{
				if (c.GetType() == typeof(King)) 
				{
					return;
				}

				activeChessman.Remove(c.gameObject);
				Destroy(c.gameObject);
			}

			Chessmans [selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
			Vector3 pos = getTileCenter (x, y);
			//поднял фигуры по выше из-за кривызны)
			pos.y = 1.2f;
			selectedChessman.transform.position = pos;
			selectedChessman.SetPosition (x, y);
			Chessmans [x, y] = selectedChessman;
			isWhiteTurn = !isWhiteTurn;
		}

		BoardHighlights.Instance.HideHighlights();

		selectedChessman = null;
	}

	private void MarkSquare(int x, int y)
	{
		Vector3 pos = getTileCenter (x, y);
		Quaternion quot = Quaternion.Euler (-90,0,0);
		GameObject go = Instantiate (marker, pos, quot) as GameObject;
		go.transform.SetParent(transform);		
	}

	private void UpdateSelection()
	{
		if (!Camera.main)
			return;
		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Squares")))
		{
			//Debug.Log ( hit.point.x + "  " + hit.point.z);
			selectionX = RoundToDec( (int)hit.point.x );
			selectionY = RoundToDec( (int)hit.point.z );
		} else {
			selectionX = -1;
			selectionY = -1;
		}


		//Рисуем выбранну клетку
		if (selectionX >= 0 && selectionY >= 0) 
		{

			Debug.DrawLine (
				Vector3.forward * selectionY + Vector3.right * selectionX,
				Vector3.forward * (selectionY + 1 * multi) + Vector3.right * (selectionX + 1 * multi)
			);

			Debug.DrawLine (
				Vector3.forward * (selectionY + 1 * multi) + Vector3.right * selectionX,
				Vector3.forward * selectionY + Vector3.right * (selectionX + 1 * multi)
			);
		}
	}

	private void SpawnCheesman(int index, int x, int y)
	{
		if (index < 6)
			orientation = Quaternion.Euler(0,0,0);
		else
			orientation = Quaternion.Euler (0,180,0);
		Vector3 pos = getTileCenter (x, y);
		pos.y = 1.2f;
		GameObject go = Instantiate (chessmanPerfabs [index], pos, orientation) as GameObject;
		go.transform.SetParent(transform);
		Chessmans [x, y] = go.GetComponent<Chessman> ();
		Chessmans [x, y].SetPosition (x, y);

		activeChessman.Add(go);
	}

	private void SpawnAllChessmans()
	{
		activeChessman = new List<GameObject> ();
		Chessmans = new Chessman[8,8];

		//Spawn the white team
		//King
		SpawnCheesman(0, 4, 0);
		//Queen
		SpawnCheesman(1, 3, 0);
		//Rooks
		SpawnCheesman(2, 0, 0);
		SpawnCheesman(2, 7, 0);
		//Bishops
		SpawnCheesman(3, 2, 0);
		SpawnCheesman(3, 5, 0);
		//Knights
		SpawnCheesman(4, 1, 0);
		SpawnCheesman(4, 6, 0);
		//Pawns
		for (int i = 0; i < 8; i++)
			SpawnCheesman (5, i, 1);

		//Spawn the black team
		//King
		SpawnCheesman(6, 4, 7);
		//Queen
		SpawnCheesman(7, 3, 7);
		//Rooks
		SpawnCheesman(8, 0, 7);
		SpawnCheesman(8, 7, 7);
		//Bishops
		SpawnCheesman(9, 2, 7);
		SpawnCheesman(9, 5, 7);
		//Knights
		SpawnCheesman(10, 1, 7);
		SpawnCheesman(10, 6, 7);
		//Pawns
		for (int i = 0; i < 8; i++)
			SpawnCheesman (11, i, 6);		
	}

	private void SpawnSquare(bool is_white, int x, int y)
	{
		Vector3 pos = getTileCenter (x, y);
		GameObject go = Instantiate (squarePerfabs [is_white ? 1: 0], pos, orientation) as GameObject;
		go.transform.SetParent(transform);
	}

	private void SpawnBoard()
	{	
		bool clr = true;
		for (int y = 0; y < 8; y++) 
		{
			for (int x = 0; x < 4; x++) 
			{
					//spawn white squares
				SpawnSquare (clr, x*2, y);
					//spawn black squares
				SpawnSquare (!clr, x*2+1, y);
			}
			clr = !clr;
		}
	}

	private Vector3 getTileCenter(int x, int y)
	{
		Vector3 origin = Vector3.zero;
		origin.x += (tile_size * x * multi) + tile_offset + multi/2;
		origin.z += (tile_size * y * multi) + tile_offset + multi/2;
		return origin;
	}
		
}
