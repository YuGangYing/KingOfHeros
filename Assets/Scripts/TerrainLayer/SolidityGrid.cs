#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System;
using System.Collections.Generic;
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
namespace SLG
{
	/// <summary>
	///A grid that has solid and non solid cells (or blocked and unblocked)
	/// </summary>
    [System.Serializable]
    public class SolidityGrid : Grid
	{
	    #region Fields
        private short[,] m_solidList;
	    #endregion

		public SolidityGrid()
		{

        }
		
		public override void Awake (Vector3 origin, int numRows, int numCols, float cellSize, bool show)
		{
			base.Awake(origin, numRows, numCols, cellSize, show);
            m_solidList = new short[numCols, numRows];
			
			// Initialize all columns to false.
			for (int colIndex = 0; colIndex < numCols; colIndex++)
			{
				for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
				{
					m_solidList[colIndex, rowIndex] = 0;
				}
			}
		}

        public void SetSolidity(short[,] solidityList)
	    {
            m_solidList = (short[,])solidityList.Clone();
	    }

        public void SetSolidity(int cellIndex, short solid)
		{
			if ( !IsInBounds(cellIndex) )
			{
				return;
			}
			
			int col = GetColumn(cellIndex);
			int row = GetRow(cellIndex);
            m_solidList[col, row] = solid;
		}

        public void SetSolidity(Vector3 cellPos, short solid)
		{
			int cellIndex = GetCellIndex(cellPos);
            SetSolidity(cellIndex, solid);
		}

	    // Determine if the position is blocked by collision
        public short GetGridValue(Vector3 pos)
	    {
	        int cellIndex = GetCellIndex(pos);
			bool bInBounds = IsInBounds(cellIndex);
	        if (!bInBounds)
	        {
	            return -1;
	        }
			
			int col = GetColumn(cellIndex);
			int row = GetRow(cellIndex);
	        return m_solidList[col, row];
	    }

        public short GetGridValue(int index)
	    {
	        int row = GetRow(index);
	        int col = GetColumn(index);
			if ( !IsInBounds(col, row) )
			{
				return -1;
			}
			
	        return m_solidList[col, row];
	    }

        public short GetGridValue(int col, int row)
        {
            if (!IsInBounds(col, row))
            {
                return -1;
            }

            return m_solidList[col, row];
        }

        public int GetIndex(int col, int row)
        {
            return row * Columns + col;
        }
	}
	
}
