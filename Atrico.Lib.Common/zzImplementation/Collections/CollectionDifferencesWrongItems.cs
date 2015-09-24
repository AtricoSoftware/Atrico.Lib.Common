using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atrico.Lib.Common.Collections;

namespace Atrico.Lib.Common.zzImplementation.Collections
{
    internal class CollectionDifferencesWrongItems : CollectionDifferences
    {
        public int CountDelta { get; private set; }
        public IEnumerable<object> Missing { get; private set; }
        public IEnumerable<object> Extra { get; private set; }

        public CollectionDifferencesWrongItems(int countDelta, IEnumerable<object> missing, IEnumerable<object> extra)
        {
            CountDelta = countDelta;
            Extra = extra;
            Missing = missing;
        }

        public override string ToString()
        {
            var message = new StringBuilder();
            if (CountDelta != 0)
            {
                message.AppendFormat("Count<{0:+#;-#;}>", CountDelta);
            }
            if (Missing.Any())
            {
                if (message.Length > 0)
                {
                    message.Append(", ");
                }
                message.AppendFormat("Missing<{0}>", Missing.ToCollectionString());
            }
            if (Extra.Any())
            {
                if (message.Length > 0)
                {
                    message.Append(", ");
                }
                message.AppendFormat("Extra<{0}>", Extra.ToCollectionString());
            }
            return message.ToString();
        }
    }
}
