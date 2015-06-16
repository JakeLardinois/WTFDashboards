using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public enum Status
    {
        Open = 'O',
        Ready = 'R',
        Complete = 'C',
        Hold = 'H'
    }

    public enum WOMetricType
    {
        AnnualWOAvgDaysOpenByCategory = 'A', //Annual Average Duration Open per Work Order over the past year
        CurrentWOAvgDaysOpenByCategory = 'C' //Current Average Duration Open per Work Order in the current bank of Work Orders
    }
}
