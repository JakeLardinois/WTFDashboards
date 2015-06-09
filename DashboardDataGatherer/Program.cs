using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DashboardDataGatherer.Models;


namespace DashboardDataGatherer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var objAnnualMP2 = new MP2(DateTime.Now.AddYears(-1).Date, DateTime.Now.Date);
            //var objOpenReadyHoldMP2 = new MP2(new List<Status> { Status.Open, Status.Ready, Status.Hold});
            List<DurationCategory> DurationCategories;
            var objAnnualMP2ByCategory = new MP2(out DurationCategories, DateTime.Now.AddYears(-1).Date, DateTime.Now.Date);

           // Console.WriteLine(objAnnualMP2.AverageWODuration);
            //Console.WriteLine(objOpenReadyHoldMP2.AverageWODuration);
            foreach (var objDurationCategory in DurationCategories)
                Console.WriteLine("Category: {0, 10} AvgWODuration: {1, 4} WO Count: {2, 3}", 
                    objDurationCategory.Category, objDurationCategory.AverageWODuration, objDurationCategory.WOCount);

            Console.WriteLine();
            Console.WriteLine();

            var objOpenReadyHoldMP2ByCategory = new MP2(out DurationCategories, new List<Status> { Status.Open, Status.Ready, Status.Hold });
            foreach (var objDurationCategory in DurationCategories)
                Console.WriteLine("Category: {0, 10} AvgWODuration: {1, 4} WO Count: {2, 3}",
                    objDurationCategory.Category, objDurationCategory.AverageWODuration, objDurationCategory.WOCount);

            Console.ReadLine();
        }
    }
}
