using System;
using System.Collections.Generic;
using com.coveo.blitz.thrift;
using System.Collections.Concurrent;

namespace BlitzIndex
{
	public class Database
    {
        private static readonly List<int> emptyIntList = new List<int>();
		private static readonly Func<string, HashSet<SearchResult>> searchResultFactory
			= str => new HashSet<SearchResult>();
        private ConcurrentDictionary<string, HashSet<SearchResult>> m_key_entries = new ConcurrentDictionary<string, HashSet<SearchResult>>(StringComparer.Ordinal);
        private Dictionary<string, SearchResult> m_entries = new Dictionary<string, SearchResult>(StringComparer.Ordinal);
		
		public Database()
		{
			
		}

		public int Count
		{
			get { return m_entries.Count; }
		}
		
		public void Insert(IDocument entry)
		{
            lock (m_entries)
            {
                m_entries.Add(entry.Id, new SearchResult(entry, emptyIntList));
            }

            Dictionary<string, List<int>> keywordOccurences = new Dictionary<string, List<int>>();
            foreach (var token in entry.Keywords)
            {
                List<int> positions;
				if (!keywordOccurences.TryGetValue(token.Value, out positions))
                {
                    positions = new List<int>();
					keywordOccurences.Add(token.Value, positions);
                }
				positions.Add(token.StartIndex);
            }

            foreach (var pair in keywordOccurences)
            {
				HashSet<SearchResult> keywordSet = m_key_entries.GetOrAdd(pair.Key, searchResultFactory);
                lock (keywordSet)
                {
                    keywordSet.Add(new SearchResult(entry, pair.Value));
                }
			}
		}
		
		public HashSet<SearchResult> Query(string keyword)
		{
			keyword = keyword.ToUpperInvariant().Trim();

			if(keyword == "*")
                return new HashSet<SearchResult>(m_entries.Values);

            HashSet<SearchResult> set;
			if (m_key_entries.TryGetValue(keyword, out set))
			{
				return set;
			}
            return new HashSet<SearchResult>();
		}	
		
		public void Reset() 
		{
			m_entries.Clear();
			m_key_entries.Clear();
		}
		
		public IDocument GetDocument(string id)
		{
			return m_entries[id].Document;
	    }
	}
}

