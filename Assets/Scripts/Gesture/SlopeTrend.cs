//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

public class SlopeTrend
{
	private int [] trend_array = new int[4] {-1, -1, -1, -1};

	public SlopeTrend ()
	{
	}

	public void SetTrend(int index, bool val)
	{
		if (index>=4 || index<0)
		{
			return;
		}
		if (val)
			trend_array[index] = 1;
		else
			trend_array[index] = 0;
	}

	/* 判断是否符合以下格式的变化规律
	 * 			+
	 * 		+		-
	 * 			-
	 * 可以从任意一点开始顺时针，或逆时间处理
	 * 若符合，则认为是单向循环的
	 * */
	public bool IsLoop()
	{
		int count=0;
		for(int i=0; i<4; ++i)
		{
			if (trend_array[i] != -1)
			{
				++count;
			}
			else
			{
				break;
			}
		}

		bool result = false;
		//只有2种变化以下，认为是单向的
		if (count <= 2)
		{
			result = true;
		}
		else if (count == 3)
		{
			//ConsoleOut("value:["+trend_array[0].ToString()+","+trend_array[1].ToString()+","+trend_array[2].ToString()+"]");
			if (trend_array[0] == trend_array[1])
			{
				if (trend_array[0] != trend_array[2])
				{
					// + + -
					result = true;
				}
			}
			else
			{
				if (trend_array[0] != trend_array[2])
				{
					// + - -
					result = true;
				}
			}
		}
		else if (count == 4)
		{
			//ConsoleOut("value:["+trend_array[0].ToString()+","+trend_array[1].ToString()+","+trend_array[2].ToString()+","+trend_array[3].ToString()+"]");
			if (trend_array[0] == trend_array[1])
			{
				if ((trend_array[0] != trend_array[2]) && (trend_array[0] != trend_array[3]) )
				{
					//+ + - -
					result = true;
				}
			}
			else
			{
				if ( (trend_array[0] != trend_array[2]) && (trend_array[0] == trend_array[3] ) )
				{
					// + - - +
					result = true;
				}
			}
		}

		return result;
	}
}
