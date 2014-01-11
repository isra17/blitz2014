using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.coveo.blitz.thrift
{
	public struct TextToken
	{
		public int StartIndex;
		public string Value;

		public TextToken(int position, string value)
		{
			this.StartIndex = position;
			this.Value = value;
		}

		public int Length
		{
			get { return Value.Length; }
		}

		public int EndIndex
		{
			get { return StartIndex + Value.Length; }
		}

		public override string ToString()
		{
			return Value;
		}
	}
}
