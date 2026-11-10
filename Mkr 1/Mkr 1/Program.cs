using Mkr_1;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        DeliveryService service = new DeliveryService();

        Bicycle bike = new Bicycle();
        Car car = new Car();
        Truck truck = new Truck();

        service.AddTransport(bike);
        service.AddTransport(car);
        service.AddTransport(truck);

        Order order1 = new Order(1, "Піца", 2.0, 4);
        Order order2 = new Order(2, "Холодильник", 80.0, 15);
        Order order3 = new Order(3, "Шафа", 300.0, 25);

        service.AddOrder(order1);
        service.AddOrder(order2);
        service.AddOrder(order3);

        service.ShowCostsForOrder(order1);
        service.ShowCostsForOrder(order2);
        service.ShowCostsForOrder(order3);

        Console.WriteLine("\n--- ТЕСТ: Спроба порушити ліміти транспорту ---");
        service.AssignOrder(order2, bike);

        service.AssignOrder(order1, bike);
        service.ShowAllLists();
        service.CompleteDelivery(order1, bike, "Картка");

        Console.WriteLine("\n--- ТЕСТ: Спроба використати зайнятий транспорт ---");
        service.AssignOrder(order3, bike);

        service.ShowAllLists();

        service.AssignOrder(order2, car);
        service.CompleteDelivery(order2, car, "Готівка");

        service.AssignOrder(order3, truck);
        service.CompleteDelivery(order3, truck, "Картка");

        service.ShowAllLists();
    }
}