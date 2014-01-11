using com.coveo.blitz.thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlitzIndex
{
    public class SearchResult
    {
        public string Term { get; private set; }
        public List<int> Occurrences { get; private set; }
        public IDocument Document { get; private set; }

        public SearchResult(string term, IDocument document, List<int> occurrences)
        {
            Term = term;
            Document = document;
            Occurrences = occurrences;
        }

        public override int GetHashCode()
        {
            return Document.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            SearchResult that = obj as SearchResult;
            if (that == null) return false;
            return that.Document == this.Document;
        }
    }
}
