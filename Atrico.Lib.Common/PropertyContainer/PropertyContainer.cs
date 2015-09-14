namespace Atrico.Lib.Common.PropertyContainer
{
    public class PropertyContainer : PropertyContainerBase<object>
    {
        public PropertyContainer(object owner)
            : base(owner)
        {
        }

        protected override T GetValue<T>(string name)
        {
            return (T)Properties[name];
        }

        protected override void SetValue<T>(string name, T value)
        {
            Properties[name] = value;
        }
    }
}