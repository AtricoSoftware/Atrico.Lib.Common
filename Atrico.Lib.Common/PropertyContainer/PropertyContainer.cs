namespace Atrico.Lib.Common.PropertyContainer
{
    public class PropertyContainer : PropertyContainerBase<object>
    {
        public PropertyContainer(object owner)
            : base(owner)
        {
        }

        protected override T GetValue<T>(object prop)
        {
            return (T)prop;
        }

        protected override object AmendValue<T>(object prop, T value)
        {
            return value;
        }

        protected override object CreateValue<T>(string name, T value)
        {
            return value;
        }
    }
}