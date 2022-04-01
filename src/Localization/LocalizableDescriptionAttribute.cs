using System;
using System.ComponentModel;

namespace ru.emlsoft.WMS.Localization
{
    [Serializable]
    public class LocalizableDescriptionAttribute : DescriptionAttribute
    {
        public LocalizableDescriptionAttribute(string inClassName)
            : base(Resources.SharedResource.ResourceManager.GetString(inClassName))
        { }

    }
}