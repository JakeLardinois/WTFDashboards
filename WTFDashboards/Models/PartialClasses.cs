using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WTFDashboards
{
    public partial class WorkOrderMetric
    {
        public string WOCategoryNonNullable { get { return string.IsNullOrEmpty(WOCategory) ? "Empty" : WOCategory; } }
    }
}