using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessLib;
using System;

public class Rules : MonoBehaviour 
{
	DragAndDrop dad;
	Chess chess;
	public float scale_multipleer = 6.0f;
	public Rules()
	{
		dad = new DragAndDrop();
		chess = new Chess();
	}
	// Use this for initialization
	void Start () 
	{
		ShowFigures ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (dad.Action ())
		{
			string from = GetSquare1(dad.pickPosition);
			string to = GetSquare2(dad.dropPosition);
			int x = Convert.ToInt32(from.Split()[0]);
			int y = Convert.ToInt32(from.Split()[1]);
			Debug.Log (x + "  " + y);
			string figure = chess.GetFigureAt(x, y).ToString();
			from = GetSquare2(dad.pickPosition);
			//Debug.Log("from "+from + " to "+to);

			string move = figure + from + to;
			Debug.Log("MOVE   ->> " + move);
			chess = chess.Move(move);
			ShowFigures ();
		}
	}

	string GetSquare1(Vector3 position)
	{
		int x = Convert.ToInt32(position.x / 6.0 );
		int y = Convert.ToInt32(position.z / 6.0 );
		return x.ToString() + " " + y.ToString();
	}

	string GetSquare2(Vector3 position)
	{
		int x = Convert.ToInt32(position.x / 6.0);
		int y = Convert.ToInt32(position.z / 6.0);
		return ((char)('a' + x)).ToString () + (y + 1).ToString ();
	}

	void ShowFigures()
	{
		int nr = 0;
		for (int y = 0; y < 8; y++)
			for (int x = 0; x < 8; x++) {
				string figure = chess.GetFigureAt(x, y).ToString();
				if (figure == ".") continue;
				PlaceFigure("box" + nr, figure, x, y);
				nr++;
			}

		for (; nr < 32; nr++)
			PlaceFigure("box" + nr, "q", 9, 9);
		MarkSquare (0, 0, true);
	}

	void PlaceFigure (string box, string figure, int x, int y)
	{
		//Debug.Log(box + " " + figure + "  " + x + y);
		GameObject goBox = GameObject.Find(box);
		GameObject goFigure = GameObject.Find(figure); //R k p N
		GameObject goSquare = GameObject.Find("" + y + x);

		var figureModel = goFigure.GetComponentInChildren<MeshFilter>();
		var figureMaterial = goFigure.GetComponentInChildren<MeshRenderer>();
		var boxModel = goBox.GetComponentInChildren<MeshFilter>();
		var boxMaterial = goBox.GetComponentInChildren<MeshRenderer>();
		boxModel.mesh = figureModel.mesh;
		boxMaterial.sharedMaterial = figureMaterial.sharedMaterial;
		goBox.transform.position = goSquare.transform.position;

	}

	void MarkSquare(int x, int y, bool isMarked)
	{
		GameObject goSquare = GameObject.Find("" + y + x);
		GameObject mark;
		if (isMarked)
			mark = GameObject.Find ("magic_ring");
		else
			mark = GameObject.Find ("empty_mark");
		mark.transform.position = goSquare.transform.position;
	}
}


class DragAndDrop
{
	enum State 
	{
		none,
		drag
	}

	public Vector3 pickPosition { get; private set; }
	public Vector3 dropPosition { get; private set; }

	State state;
	GameObject item;

	public DragAndDrop()
	{
		state = State.none;
		item = null;
	}

	public bool Action()
	{
		switch(state)
		{
		case State.none:
			if (IsMouseButtonPressed ()) {
				PickUp();
			}
				break;
			case State.drag:
				if (IsMouseButtonPressed())
					Drag();
				else {
					Drop();
					return true;
				}
				break;
		}
		return false;
	}

	bool IsMouseButtonPressed()
	{
		return Input.GetMouseButton(0);
	}

	void PickUp ()
	{
		Transform clickedItem = GetItem();
		if (clickedItem == null) return;
		pickPosition = clickedItem.position;
		item = clickedItem.gameObject;

		//Debug.Log("picked up "+ item.name);
		state = State.drag;
	}

	Vector3 GetClickPosition()
	{
		return GetItem().position;
	}

	Transform GetItem()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) 
		{
			//Debug.Log (hit.transform);
			return hit.transform;
		}
		return null;
	}

	void Drag()
	{
		item.transform.position = GetClickPosition();
	}

	void Drop()
	{
		dropPosition = item.transform.position;
		state = State.none;
		item = null;
	}
}
