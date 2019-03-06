using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman 
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		Chessman c;
		int i, j;

		//Наверх
		i = CurrentX - 1;
		j = CurrentY + 1;
		if (CurrentY != 7) 
		{
			for (int k = 0; k < 3; k++) 
			{
				
				if (i >= 0 || i < 8) 
				{
					c = BoardControl.Instance.Chessmans [i, j];
					if (c == null)
						r [i, j] = true;
					else 
					{
						if(isWhite != c.isWhite)
							r [i, j] = true;
					}
				}

				i++;
					
			}
		}

		//Вниз
		i = CurrentX - 1;
		j = CurrentY - 1;
		if (CurrentY != 0) 
		{
			for (int k = 0; k < 3; k++) 
			{

				if (i >= 0 || i < 8) 
				{
					c = BoardControl.Instance.Chessmans [i, j];
					if (c == null)
						r [i, j] = true;
					else 
					{
						if(isWhite != c.isWhite)
							r [i, j] = true;
					}
				}

				i++;

			}
		}

		//Налево
		if (CurrentX != 0) 
		{
			c = BoardControl.Instance.Chessmans [CurrentX - 1, CurrentY];
			if (c == null)
				r [CurrentX - 1, CurrentY] = true;
			else 
			{
				if(isWhite != c.isWhite)
					r [CurrentX - 1, CurrentY] = true;
			}
		}

		//Направо
		if (CurrentX != 7) 
		{
			c = BoardControl.Instance.Chessmans [CurrentX + 1, CurrentY];
			if (c == null)
				r [CurrentX + 1, CurrentY] = true;
			else 
			{
				if(isWhite != c.isWhite)
					r [CurrentX + 1, CurrentY] = true;
			}
		}

		return r;

	}
}
