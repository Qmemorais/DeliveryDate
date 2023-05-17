using System;

namespace DeliveryDate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //to input dd.mm.yyyy hh:mm instead of mm.dd.yyyy hh:mm
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-gb");

            Console.WriteLine("Please input date and time of order in format dd.MM.yyyy HH:mm");

            var isDateParsed = DateTime.TryParse(Console.ReadLine(), out var inputedDateTime);

            if (isDateParsed)
            {
                DeliveryService deliver = new();

                var expectedDeliveryDate = deliver.FindExpectedDeliveryDate(inputedDateTime);

                Console.WriteLine($"Expected Delivery Date: {expectedDeliveryDate}");
            }

            Console.WriteLine("Enter any button");
            Console.ReadKey();
        }
    }
}
