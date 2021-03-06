﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman 
{
	public override bool[,] PossibleMove ()
	{
		bool [,] r = new bool[8,8];
		int[] e = BoardControl.Instance.EnPassanMove;
		Chessman c, c2;

		//ход белых
		if (isWhite) {
			//вверх на левно
			if (CurrentX != 0 && CurrentY != 7) {
				if (e [0] == CurrentX - 1 && e [1] == CurrentY + 1) 
				{
					r [CurrentX - 1, CurrentY + 1] = true;
				}

				c = BoardControl.Instance.Chessmans [CurrentX - 1, CurrentY + 1];

                if (c != null && !c.isWhite)
                {
                    r[CurrentX - 1, CurrentY + 1] = true;
                }
          
            }
				
			//вверх на право
			if (CurrentX != 7 && CurrentY != 7) {
				if (e [0] == CurrentX + 1 && e [1] == CurrentY + 1) 
				{
					r [CurrentX + 1, CurrentY + 1] = true;
				}

				c = BoardControl.Instance.Chessmans [CurrentX + 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                {
                    r [CurrentX + 1, CurrentY + 1] = true;
                }
            }

			//прямо
			if (CurrentY != 7) {
				c = BoardControl.Instance.Chessmans [CurrentX, CurrentY + 1];
				if (c == null)
					r [CurrentX, CurrentY + 1] = true;
			}

			//прямо первый шаг
			if (CurrentY == 1) {
				c = BoardControl.Instance.Chessmans [CurrentX, CurrentY + 1];
				c2 = BoardControl.Instance.Chessmans [CurrentX, CurrentY + 2];
				if (c == null && c2 == null) {
					r [CurrentX, CurrentY + 2] = true;
				}
			}
		} 
		else
		{
			//вверх налевно
			if (CurrentX != 7 && CurrentY != 0) {
				if (e [0] == CurrentX + 1 && e [1] == CurrentY - 1) 
				{
					r [CurrentX + 1, CurrentY - 1] = true;
				}

				c = BoardControl.Instance.Chessmans [CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    r[CurrentX + 1, CurrentY - 1] = true;
                }
            }

			//вверх направо
			if (CurrentX != 0 && CurrentY != 0) {
				if (e [0] == CurrentX - 1 && e [1] == CurrentY - 1) 
				{
					r [CurrentX - 1, CurrentY - 1] = true;
				}

				c = BoardControl.Instance.Chessmans [CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                {
                    r [CurrentX - 1, CurrentY - 1] = true;
                }
            }

			//прямо
			if (CurrentY != 0) {
				c = BoardControl.Instance.Chessmans [CurrentX, CurrentY - 1];
				if (c == null)
					r [CurrentX, CurrentY - 1] = true;
			}

			//прямо первый шаг
			if (CurrentY == 6) {
				c = BoardControl.Instance.Chessmans [CurrentX, CurrentY - 1];
				c2 = BoardControl.Instance.Chessmans [CurrentX, CurrentY - 2];
				if (c == null && c2 == null) {
					r [CurrentX, CurrentY - 2] = true;
				}
			}
		}

		return r;
	}
}
