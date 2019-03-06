using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman 
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		//вверх налево
		KnightMove(CurrentX - 1, CurrentY + 2, ref r);

		//вверх направо
		KnightMove(CurrentX + 1, CurrentY + 2, ref r);

		//налево вверх
		KnightMove(CurrentX + 2, CurrentY + 1, ref r);

		//направо вверх
		KnightMove(CurrentX + 2, CurrentY - 1, ref r);

		//вниз налево
		KnightMove(CurrentX - 1, CurrentY - 2, ref r);

		//вниз направо
		KnightMove(CurrentX + 1, CurrentY - 2, ref r);

		//налево вниз 
		KnightMove(CurrentX - 2, CurrentY + 1, ref r);

		//направо вниз 
		KnightMove(CurrentX - 2, CurrentY - 1, ref r);

		return r;
	}

	public void KnightMove(int x, int y, ref bool[,] r)
	{
		Chessman c;
		if (x >= 0 && x < 8 && y >= 0 && y < 8) 
		{
			c = BoardControl.Instance.Chessmans[x, y];
			if (c == null)
				r [x, y] = true;
			else if(isWhite != c.isWhite)
				r [x, y] = true;
		}
	}
}
