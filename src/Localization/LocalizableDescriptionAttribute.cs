using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ru.EmlSoft.WMS.Localization
{
    [Serializable]
    public class LocalizableDescriptionAttribute : DescriptionAttribute
    {
        public LocalizableDescriptionAttribute(string inClassName)
            : base(Resources.SharedResource.ResourceManager.GetString(inClassName))
        { }

    }
}