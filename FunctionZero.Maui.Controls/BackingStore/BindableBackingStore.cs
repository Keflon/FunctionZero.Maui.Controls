using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.BackingStore
{
    public class BindableBackingStore : IBackingStore
    {
        private static char[] _dot = new[] { '.' };

        public BindableBackingStore(object host)
        {
            Host = host;
        }

        public object Host { get; }

        public (OperandType type, object value) GetValue(string qualifiedName)
        {
            var hostInfo = GetPropertyInfo(Host, qualifiedName);
            var value = hostInfo.info.GetValue(hostInfo.host);

            if (BackingStoreHelpers.OperandTypeLookup.TryGetValue(hostInfo.info.PropertyType, out var theOperandType))
                return (theOperandType, value);

            if (value == null)
                return (OperandType.Null, null);

            return (OperandType.Object, value);
        }

        public void SetValue(string qualifiedName, object value)
        {
            var hostInfo = GetPropertyInfo(Host, qualifiedName);

            //if(hostInfo.info.CanWrite)
            {
                hostInfo.info.SetValue(hostInfo.host, value);
            }
        }

        protected (object host, PropertyInfo info) GetPropertyInfo(object host, string qualifiedName)
        {
            var bits = qualifiedName.Split(_dot);

            PropertyInfo pi = null;
            object nextHost = host;
            foreach (var bit in bits)
            {
                host = nextHost;

                // Get info for the property
                pi = host.GetType().GetProperty(bit, BindingFlags.Public | BindingFlags.Instance);
                if (pi == null || pi.CanRead == false)
                    return (null, null);


                nextHost = pi.GetValue(host);
            }
            return (host, pi);
        }

    }
}