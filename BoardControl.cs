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

	public int[] EnPassanMove{ set; get;}

	public bool isWhiteTurn = true;

	private void Start()
	{
		Instance = this;
		SpawnBoard ();
		SpawnAllChessmans ();
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

		bool hasAnotherMove = false;			
		allowedMoves = Chessmans [x, y].PossibleMove();
		for (int i = 0; i < 8; i++)
			for (int j = 0; j < 8; j++)
				if (allowedMoves [i, j])
					hasAnotherMove = true;
		if (!hasAnotherMove)
			return;

		selectedChessman = Chessmans [x, y];
		//Debug.Log("Selected figure "+selectedChessman.name);
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);
		BuffsControl.Instance.ActivateBuff (allowedMoves);
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
                //selectedChessman.Attack();
				if (c.GetType() == typeof(King)) 
				{
					EndGame();
					return;
				}

				activeChessman.Remove(c.gameObject);
				//Destroy(c.gameObject);
			}

            #region взятие в проходе
            if (x == EnPassanMove [0] && y == EnPassanMove [1]) 
			{
				if (isWhiteTurn)
					c = Chessmans[x,y-1];	
				else
					c = Chessmans[x,y+1];
				activeChessman.Remove(c.gameObject);
				Destroy(c.gameObject);
			}

			EnPassanMove [0] = -1;
			EnPassanMove [1] = -1;
			if (selectedChessman.GetType () == typeof(Pawn)) 
			{
				if (y == 7) 
				{
					activeChessman.Remove(selectedChessman.gameObject);
					Destroy(selectedChessman.gameObject);
					SpawnChessman (1, x, y);
					selectedChessman = Chessmans [x, y];
				}	
				else if (y == 0) 
				{
					activeChessman.Remove(selectedChessman.gameObject);
					Destroy(selectedChessman.gameObject);
					SpawnChessman (7, x, y);
					selectedChessman = Chessmans [x, y];
				}

				if (selectedChessman.CurrentY == 1 && y == 3) {
					EnPassanMove [0] = x;
					EnPassanMove [1] = y - 1;
				}
				else if(selectedChessman.CurrentY == 6 && y == 4)
				{
					EnPassanMove [0] = x;
					EnPassanMove [1] = y + 1;
				}
			}
            #endregion

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;

			selectedChessman.SetPosition (x, y, c);
            //отмечаем новую позицию фигуры
			Chessmans [x, y] = selectedChessman;

			isWhiteTurn = !isWhiteTurn;
		}

		BoardHighlights.Instance.HideHighlights();
		BuffsControl.Instance.HideBuffs();

        selectedChessman = null;
	}

	private void MarkSquare(int x, int y)
	{
		Vector3 pos = GetTileCenter (x, y);
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


		//Рисуем крест на выбранной клетке
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

	private void SpawnChessman(int index, int x, int y)
	{
		if (index < 6)
			orientation = Quaternion.Euler(0,0,0);
		else
			orientation = Quaternion.Euler (0,180,0);
		Vector3 pos = GetTileCenter (x, y);
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
		EnPassanMove = new int[2]{ -1, -1 };

		//Spawn the white team
		//King
		SpawnChessman(0, 4, 0);
		//Queen
		SpawnChessman(1, 3, 0);
		//Rooks
		SpawnChessman(2, 0, 0);
		SpawnChessman(2, 7, 0);
		//Bishops
		SpawnChessman(3, 2, 0);
		SpawnChessman(3, 5, 0);
		//Knights
		SpawnChessman(4, 1, 0);
		SpawnChessman(4, 6, 0);
		//Pawns
		for (int i = 0; i < 8; i++)
			SpawnChessman (5, i, 1);

		//Spawn the black team
		//King
		SpawnChessman(6, 4, 7);
		//Queen
		SpawnChessman(7, 3, 7);
		//Rooks
		SpawnChessman(8, 0, 7);
		SpawnChessman(8, 7, 7);
		//Bishops
		SpawnChessman(9, 2, 7);
		SpawnChessman(9, 5, 7);
		//Knights
		SpawnChessman(10, 1, 7);
		SpawnChessman(10, 6, 7);
		//Pawns
		for (int i = 0; i < 8; i++)
			SpawnChessman (11, i, 6);		
	}

	private void SpawnSquare(bool is_white, int x, int y)
	{
		Vector3 pos = GetTileCenter (x, y);
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

	public Vector3 GetTileCenter(int x, int y)
	{
		Vector3 origin = Vector3.zero;
		origin.x += (tile_size * x * multi) + tile_offset + multi/2;
		origin.z += (tile_size * y * multi) + tile_offset + multi/2;
		return origin;
	}
		
	public void EndGame()
	{
		if (isWhiteTurn) 
			Debug.Log ("Победила команда белых!");
		else
			Debug.Log ("Победила команда черных!");

		foreach (GameObject go in activeChessman)
			Destroy(go);

		isWhiteTurn = true;
		BoardHighlights.Instance.HideHighlights ();
		SpawnAllChessmans ();
		
	}
}
