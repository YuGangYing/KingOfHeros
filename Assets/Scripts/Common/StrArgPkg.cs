using System;
using System.Collections;
using System.Collections.Generic;

public abstract class IStrArgSeg
{
	public abstract bool empty();
	public abstract int size();
	public abstract IStrArgSeg getStrByPos(int pos);
	public abstract string getStr();
};

// example:
// 	pattern:“`;,”
// 	args:“1,2;3,4`5,6;7,8`”
public class StrArgPkg : IStrArgSeg
{
	public static string GetArg(IStrArgSeg seg, int index, string defaultValue)
	{
	    if (index >= seg.size())
	        return defaultValue;

	    return seg.getStrByPos(index).getStr();
	}

	public StrArgPkg(string pattern, string args)
	{
		m_pattern = pattern;
		m_args = args;
		m_rootSeg = new StrArgSeg(m_args);

		parse();
	}

	~StrArgPkg()
	{

	}

	public void setPattern(string pattern)
	{
		m_pattern= pattern;
		parse();
	}

	public string getPattern()
	{
		return m_pattern;
	}

	public void setArgs(string args)
	{
		m_args = args;
		parse();
	}

	public string getArgs()
	{
		return m_args;
	}

	public override bool empty()
	{
		return m_rootSeg.empty();
	}

	public override int size()
	{
		return m_rootSeg.size();
	}

	public override IStrArgSeg getStrByPos(int pos)
	{
		return m_rootSeg.getStrByPos(pos);
	}

    public IStrArgSeg this[int n]
    {
        get { return m_rootSeg.getStrByPos(n); }
    }

	public override string getStr()
	{
		return m_args;
	}

	private class StrArgSeg : IStrArgSeg
	{
		//typedef std::vector< StrArgSeg >	StrArgSegs;
		public StrArgSeg(string args)
		{
			m_args = args;
			clear();
		}

		public override bool empty()
		{
			return m_segs.Count == 0;
		}

		public override int size()
		{
			return m_segs.Count;
		}

		public override IStrArgSeg getStrByPos(int pos)
		{
			return m_segs[pos];
		}

		public override string getStr()
		{
			return m_args.Substring((int)(m_begin), (int)(m_len - 1));
		}

		public void clear()
		{
			m_begin= 0;
			m_len= 0;
			m_segs.Clear();
		}

		public void pushSeg(uint totalLevel, uint newLevel, uint endPos)
		{
			StrArgSeg parent = this;
			for(uint parentLevel = 0; parentLevel < newLevel; ++parentLevel)
			{
				parent = parent.m_segs[parent.m_segs.Count-1];
				parent.m_len = 1 + endPos - parent.m_begin;
			}

			uint newBegin= 0;
			if( ! (parent.m_segs.Count == 0) )
			{
				StrArgSeg prevSeg= parent.m_segs[parent.m_segs.Count-1];
				newBegin= prevSeg.m_begin + prevSeg.m_len;
			}

			uint newLen= 1 + endPos - newBegin;
			for( uint i = newLevel; i < totalLevel; ++i )
			{
				parent.m_segs.Add(new StrArgSeg(m_args));
				StrArgSeg newSeg = parent.m_segs[parent.m_segs.Count-1];
				newSeg.m_begin= newBegin;
				newSeg.m_len= newLen;
				parent = newSeg;
			}
		}

		private string	     m_args;
		private uint         m_begin;
		private uint         m_len;
		private List<StrArgSeg> m_segs = new List<StrArgSeg>();
	};

	private void parse()
	{
		m_rootSeg.clear();
		if (m_pattern.Length == 0)
		{
			return;
		}

		if (m_args.Length == 0)
		{
			return;
		}

		if (m_pattern.LastIndexOf(m_args[m_args.Length-1]) == -1)
		{
			m_args += m_pattern[0];
		}
		
		uint finishedLevel= 0;
		for(uint i = 0; i < m_args.Length; ++i)
		{
			char now  = m_args[(int)(i)];
			int level = m_pattern.LastIndexOf(now);
			if(i == m_args.Length - 1 )
			{

			}

			if( level == -1)
			{
				continue;
			}

			m_rootSeg.pushSeg((uint)(m_pattern.Length), finishedLevel, i);
			finishedLevel = (uint)(level);
		}
	}

	private string	m_pattern;
	private string	m_args;
	private StrArgSeg m_rootSeg;
};
