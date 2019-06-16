using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsControl : MonoBehaviour 
{
    public static BuffsControl Instance;

    void Start()
    {
		Instance = this;
    }

	public void ActivateBuff(bool[,] moves)
	{
        BoardControl Board = BoardControl.Instance;

        Chessman curEnemy = null;
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (moves [i, j]) 
				{
                    curEnemy = Board.Chessmans[i, j];
                    if (curEnemy != null)
                    {
                        if (!Board.isWhiteTurn && curEnemy.isWhite)                        
                            curEnemy.deathMark = true;                        
                        else                        
                            curEnemy.deathMark = true;                        
                    }
                    else curEnemy = null;                    
                }
			}
		}
	}

    public void HideBuffs()
    {
        BoardControl Board = BoardControl.Instance;

        foreach (Chessman c in Board.Chessmans)
        {
            if(c != null)
                c.deathMark = false;
        }
    }


}
