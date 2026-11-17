using System;
using System.Collections.Generic;
using System.Text;

namespace Module_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== СИМУЛЯЦІЯ СЛУЖБИ ДОСТАВКИ ТА ОПЛАТИ ===");

            DeliveryService service = new DeliveryService();

            List<ITransport> allTransports = new List<ITransport>
            {
                new Bike(),
                new Car(),
                new Truck()
            };

            IPayment cash = new PayCash();
            IPayment card = new PayCard();

            Order order1 = new Order(1, "Документи", 2.0, 5.0, 100m);
            Order order2 = new Order(2, "Коробка з одягом", 15.0, 20.0, 350m);
            Order order3 = new Order(3, "Двокамерний холодильник", 75.0, 30.0, 1500m);

            service.AddOrder(order1);
            service.AddOrder(order2);
            service.AddOrder(order3);

            
            foreach (var order in new List<Order> { order1, order2, order3 })
            {
                service.ShowAvailableTransportsForOrder(order, allTransports);
            }

            Console.WriteLine("\n--- ВИКОНАННЯ СЦЕНАРІЇВ ДОСТАВКИ ---");

            service.CompleteDelivery(1, allTransports[0], card); // Велосипед
            service.CompleteDelivery(3, allTransports[2], cash); // Вантажівка для холодильника
            service.CompleteDelivery(2, allTransports[1], card); // Автомобіль

            service.ShowAllReports();

            Console.WriteLine("Симуляцію завершено.");
        }
    }
}