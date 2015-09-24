using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections;

namespace Atrico.Lib.Common.zzImplementation.Collections
{
	internal class CollectionDifferencesWrongOrder : CollectionDifferences
	{
		public IEnumerable<object> OutOfOrder { get; private set; }

		public CollectionDifferencesWrongOrder(IEnumerable<object> outOfOrder)
		{
			OutOfOrder = outOfOrder;
		}

		public override string ToString()
		{
			var message = new StringBuilder();
			if (OutOfOrder.Any())
			{
				message.AppendFormat("OutOfOrder<{0}>", OutOfOrder.ToCollectionString());
			}
			return message.ToString();
		}
	}
}
