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
        public string PropertyName { get; set; }
        public FilterOption Operation { get; set; }
        public object Value { get; set; }

    }
}