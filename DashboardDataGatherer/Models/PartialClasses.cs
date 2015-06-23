using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Infrastructure;


namespace DashboardDataGatherer
{
    public partial class SytelineDbEntities
    {

        public void SetCommandTimeOut(int Timeout)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;


            objectContext.CommandTimeout = Timeout;
        }
    }

    public partial class FactTrakEntities
    {

        public void SetCommandTimeOut(int Timeout)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;


            objectContext.CommandTimeout = Timeout;
        }
    }
}

namespace DashboardDataGatherer.Models
{
    public partial class WO
    {

        public virtual int? DaysOpen
        { //the int is made nullable so that it can be excluded from the pivot table calculation when invalid data is encountered
            get
            {
                DateTime dtmRequestDate, dtmCompletionDate;
                int intDaysOpen;


                if (STATUS != null && STATUS == 'C') //all Work Orders that go through the 'Close' process get thier status updated to 'Completed' regardless of what thier original status was
                {
                    dtmRequestDate = REQUESTDATE ?? SharedVariables.MINDATE; //uses the null-coalescing operator to get the nullable REQUESTDATE to a DateTime object
                    if (dtmRequestDate == SharedVariables.MINDATE)
                        return (int?)null;

                    dtmCompletionDate = COMPLETIONDATE ?? SharedVariables.MINDATE; //uses the null-coalescing operator to get the nullable COMPLETIONDATE to a DateTime object
                    if (dtmCompletionDate == SharedVariables.MINDATE)
                        return (int?)null;

                    intDaysOpen = (dtmCompletionDate - dtmRequestDate).Days;
                    return intDaysOpen >= 0 ? intDaysOpen : (int?)null;   //if the CLOSEDATE occurred before the REQUESTDATE (resulting in a negative) then the data is invalid and so a null is returned
                }
                else if (STATUS != null && (STATUS == 'O' || STATUS == 'R'))
                {
                    dtmRequestDate = REQUESTDATE ?? SharedVariables.MINDATE; //uses the null-coalescing operator to get the nullable REQUESTDATE to a DateTime object
                    if (dtmRequestDate == SharedVariables.MINDATE)
                        return (int?)null;

                    dtmCompletionDate = DateTime.Now.Date;

                    intDaysOpen = (dtmCompletionDate - dtmRequestDate).Days;
                    return intDaysOpen > 0 ? intDaysOpen : (int?)null;   //if the CLOSEDATE occurred before the REQUESTDATE (resulting in a negative) then the data is invalid and so a null is returned
                }
                else
                    return (int?)null;
            }
        }


    }
}
