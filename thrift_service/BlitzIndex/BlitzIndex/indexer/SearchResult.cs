using com.coveo.blitz.thrift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlitzIndex
{
    public class SearchResult
    {
        public List<int> Occurrences { get; private set; }
        public IDocument Document { get; private set; }

        public SearchResult(IDocument document, List<int> occurrences)
        {
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
