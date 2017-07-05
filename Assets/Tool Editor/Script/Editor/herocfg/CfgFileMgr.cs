using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class CfgFileMgr
{
	private string m_strFilePath = "";

	public bool havePath()
	{
		if(m_strFilePath==null || m_strFilePath.Trim().Length==0)
			return false;
		return true;
	}

	public string LoadFileStr() 
	{
		if(!havePath())
			return null;
		FileInfo file = new FileInfo(m_strFilePath);
		if(!file.Exists)
			return null;
		StreamReader sr = File.OpenText(m_strFilePath);
		string strContent = null; 
		while(true)
		{
			string temp = sr.ReadLine();
			if(temp==null)
				break;
			strContent += temp;
		}
		sr.Close();
		sr.Dispose();
		return strContent;
	}

	public StreamReader LoadFileStream() 
	{
		if(!havePath())
			return null;
		FileInfo file = new FileInfo(m_strFilePath);
		if(!file.Exists)
			return null;
		return File.OpenText(m_strFilePath);
	}

	public bool WriteFile(string content)
	{
		if(!havePath())
			return false;

		//先删除
		File.Delete(m_strFilePath);
		StreamWriter sw = null;
		FileInfo file = new FileInfo(m_strFilePath);
		if(!file.Exists)
			sw = file.CreateText();
		else
			sw = file.AppendText();
		sw.WriteLine(content);
		sw.Close();
		sw.Dispose();
		return true;
	}

	public bool openBrower(string title, string directory, string extension)
	{ 
		m_strFilePath = EditorUtility.OpenFilePanel(title,directory,extension);
		return havePath();
	}

	public bool saveBrower(string title, string directory, string defaultName, string extension)
	{
		m_strFilePath = EditorUtility.SaveFilePanel(title,directory,defaultName,extension);
		return havePath();
	}
}