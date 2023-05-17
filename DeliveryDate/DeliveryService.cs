using System;

namespace DeliveryDate
{
    internal class DeliveryService
    {
        readonly TimeSpan openHour = new(8, 0, 0);
        readonly TimeSpan closeHour = new(17, 0, 0);
        readonly TimeSpan midnight = new(24, 0, 0);
        readonly int deliveryTimeBetweenPoints = 2;
        readonly int checkDocumantationTime = 1;

        public DateTime FindExpectedDeliveryDate(DateTime orderedDate)
        {
            if (!IsWorkingHours(orderedDate))
                orderedDate = WaitForWorkingHours(orderedDate);
            
            var expectedDeliveryTime = DeliverPackageOrWaitDocumentation(orderedDate, deliveryTimeBetweenPoints);

            expectedDeliveryTime = DeliverPackageOrWaitDocumentation(expectedDeliveryTime, checkDocumantationTime);

            expectedDeliveryTime = DeliverPackageOrWaitDocumentation(expectedDeliveryTime, deliveryTimeBetweenPoints);

            return expectedDeliveryTime;
        }

        private DateTime DeliverPackageOrWaitDocumentation(DateTime date, int timeToWait)
        {
            var expectedDateToDeliver = date.AddHours(timeToWait);

            while (!IsWorkingHours(expectedDateToDeliver))
            {
                expectedDateToDeliver = WaitForWorkingHours(expectedDateToDeliver);
                expectedDateToDeliver = expectedDateToDeliver.AddHours(timeToWait);
            }
            return expectedDateToDeliver;
        }

        private DateTime WaitForWorkingHours(DateTime date)
        {
            var orderedTimeBeforOpenHour = openHour - date.TimeOfDay;
            var orderedTimeAfterCloseHour = date.TimeOfDay - closeHour;

            if (orderedTimeBeforOpenHour.TotalMinutes > 0)
                date = date.Add(orderedTimeBeforOpenHour);

            if (orderedTimeAfterCloseHour.TotalMinutes > 0 
                || date.DayOfWeek == DayOfWeek.Sunday 
                || date.DayOfWeek == DayOfWeek.Saturday)
                date = date.Add(midnight - date.TimeOfDay + openHour);

            if (!IsWorkingHours(date))
                date = WaitForWorkingHours(date);

            return date;
        }

        private bool IsWorkingHours(DateTime orderedDate)
            => orderedDate.DayOfWeek != DayOfWeek.Sunday && orderedDate.DayOfWeek != DayOfWeek.Saturday
                && orderedDate.TimeOfDay >= openHour && orderedDate.TimeOfDay <= closeHour;
    }
}