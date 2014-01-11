using System;
using System.Collections.Generic;
using com.coveo.blitz.thrift;

namespace BlitzIndex
{
	public class Database
	{
		Dictionary<string, HashSet<IDocument>> m_key_entries = new Dictionary<string, HashSet<IDocument>>();
		Dictionary<string, IDocument> m_entries = new Dictionary<string, IDocument>();
		
		public Database ()
		{
			
		}
		
		public void Insert(IDocument entry)
		{
			m_entries.Add(entry.Id, entry);

			foreach(string keyword in entry.Keywords)
			{
				HashSet<IDocument> documents;
				if(!m_key_entries.TryGetValue(keyword, out documents))
				{
					documents = new HashSet<IDocument>();
					m_key_entries.Add(keyword, documents);
				}
				documents.Add(entry);
			}
		}
		
		public HashSet<IDocument> Query(string keyword)
		{
			if(keyword == "*")
				return new HashSet<IDocument>(m_entries.Values);
			
			HashSet<IDocument> set;
			if (m_key_entries.TryGetValue(keyword, out set))
			{
				return set;
			}
			return new HashSet<IDocument>();
		}	
		
		public void Reset() 
		{
			m_entries.Clear();
			m_key_entries.Clear();
		}
	}
}

