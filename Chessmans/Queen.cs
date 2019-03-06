using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman 
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		Chessman c;
		int i, j;

		//направо
		i = CurrentX;
		while (true) 
		{
			i++;
			if (i >= 8)
				break;
			c = BoardControl.Instance.Chessmans [i, CurrentY];
			if (c == null)
				r [i, CurrentY] = true;
			else 
			{
				if (c.isWhite != isWhite)
					r [i, CurrentY] = true;

				break;
			}
		}

		//налево
		i = CurrentX;
		while (true) 
		{
			i--;
			if (i < 0)
				break;
			c = BoardControl.Instance.Chessmans [i, CurrentY];
			if (c == null)
				r [i, CurrentY] = true;
			else 
			{
				if (c.isWhite != isWhite)
					r [i, CurrentY] = true;

				break;
			}
		}

		//наверх
		i = CurrentY;
		while (true) 
		{
			i++;
			if (i >= 8)
				break;
			c = BoardControl.Instance.Chessmans [CurrentX, i];
			if (c == null)
				r [CurrentX, i] = true;
			else 
			{
				if (c.isWhite != isWhite)
					r [CurrentX, i] = true;

				break;
			}
		}

		//вниз
		i = CurrentY;
		while (true) 
		{
			i--;
			if (i < 0)
				break;
			c = BoardControl.Instance.Chessmans [CurrentX, i];
			if (c == null)
				r [CurrentX, i] = true;
			else 
			{
				if (c.isWhite != isWhite)
					r [CurrentX, i] = true;

				break;
			}
		}

		//Наверх налево
		i = CurrentX;
		j = CurrentY;

		while (true) 
		{
			i--;
			j++;
			if (i < 0 || j >= 8)
				break;
			c = BoardControl.Instance.Chessmans[i, j];
			if (c == null)
				r [i, j] = true;
			else
			{
				if(isWhite != c.isWhite)
					r [i, j] = true;

				break;

			}
		}

		//Наверх направо
		i = CurrentX;
		j = CurrentY;

		while (true) 
		{
			i++;
			j++;
			if (i >= 8 || j >= 8)
				break;
			c = BoardControl.Instance.Chessmans[i, j];
			if (c == null)
				r [i, j] = true;
			else
			{
				if(isWhite != c.isWhite)
					r [i, j] = true;

				break;

			}
		}

		//Вниз налево
		i = CurrentX;
		j = CurrentY;

		while (true) 
		{
			i--;
			j--;
			if (i < 0 || j < 0)
				break;
			c = BoardControl.Instance.Chessmans[i, j];
			if (c == null)
				r [i, j] = true;
			else
			{
				if(isWhite != c.isWhite)
					r [i, j] = true;

				break;

			}
		}

		//Наверх направо
		i = CurrentX;
		j = CurrentY;

		while (true) 
		{
			i++;
			j--;
			if (i >= 8 || j < 0)
				break;
			c = BoardControl.Instance.Chessmans[i, j];
			if (c == null)
				r [i, j] = true;
			else
			{
				if(isWhite != c.isWhite)
					r [i, j] = true;

				break;

			}
		}

		return r;
	}
}
