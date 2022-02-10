namespace ru.EmlSoft.WMS.Data.Abstract.Database
{
    public enum FilterOption
    {
        Equals,
        NotEquals,
        IsNull,
        IsNotNull,
        In,
        NotIn,
        GreaterThanOrEqual,
        LessThanOrEqual
    }

    public class FilterObject
    {
        private readonly string _propertyName;
        private readonly FilterOption _option;
        private readonly object _value;

        public FilterObject(string propertyName, FilterOption option, object value)
        {
            _propertyName = propertyName;
            _option = option;
            _value = value;
        }
        public string PropertyName => _propertyName;
        public FilterOption Operation => _option;
        public object Value => _value;
    }
}