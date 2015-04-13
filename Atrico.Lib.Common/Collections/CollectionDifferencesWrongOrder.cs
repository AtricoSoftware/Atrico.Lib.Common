using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atrico.Lib.Common.Collections
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
