using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour 
{
    public static BoardHighlights Instance;

	public GameObject greenHighlighte;
	public GameObject redHighlighte;

    private List<GameObject> highlights;

	private void Start()
	{
		Instance = this;
		highlights = new List<GameObject> ();
	}

	private GameObject GetHighlightObject()
	{
		GameObject go = highlights.Find (g => !g.activeSelf);

		if (go == null)
		{
			go = Instantiate (greenHighlighte);
			highlights.Add (go);
		}

		return go;
	}

	public void HighlightAllowedMoves(bool[,] moves)
	{
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (moves [i, j]) 
				{
					GameObject go = GetHighlightObject ();
					go.SetActive (true);
					go.transform.position = new Vector3 (5 + i * 10, 1, 5 + j * 10);
				}
			}
		}
	}

	public void HideHighlights()
	{
		foreach (GameObject go in highlights) 
		{
			go.SetActive (false);
		}
	}
}
