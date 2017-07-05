using UnityEngine;
using System.Collections;

public static class TextureUtility {

	public static Texture2D DrawPane(int borderWidth,Color borderColor,int width,int height)
	{
		borderWidth = Mathf.Max (1,borderWidth);
		Texture2D tex = new Texture2D (width,height);
		for(int i = 0;i < width;i ++)
		{
			for(int j = 0;j < height;j++)
			{
				if(i < borderWidth || i >= width - borderWidth || j < borderWidth || j >= height - borderWidth)
				{
					tex.SetPixel(i,j,borderColor);
				}
			}
		}
		tex.Apply ();
		return tex;
	}

}
