using System;
using System.Collections.Generic;
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

        service.AddOrder(order1);
        service.AddOrder(order2);

       
        service.ShowCostsForOrder(order1);
        service.ShowCostsForOrder(order2);

        
        service.AssignOrder(order1, bike);

        
        service.ShowAllLists();

      
        service.CompleteDelivery(order1, bike, "Картка");

        
        service.ShowAllLists();
        
        service.AssignOrder(order2, car);

       
        service.CompleteDelivery(order2, car, "Готівка");

        
        service.ShowAllLists();
    }
}


public class Order
{
    public int Number  { get; set; }
    public string Description { get; set; }
    public double Weight { get; set; }
    public double Distance { get; set; }

    public Order(int number, string description, double weight, double distance)
    {
        Number = number;
        Description =description;
        Weight = weight;
        Distance = distance;
    }
    public override string ToString()
    {
        return $"Замовлення #{Number} ({Description}): вага {Weight} кг,відстань {Distance} км";
        
    }
}
public abstract class Transport
{
    public string Name { get; set; }
    public bool IsBusy { get; set; } = false;
    protected Transport(string name)
    {
        Name = name;
    }
    public abstract decimal CalculateCost(Order order);
    public virtual bool CanTake(Order order)
    {
        return !IsBusy;
    }
}
public class Bicycle : Transport
{
    public Bicycle(): base ("Велосипед") { }
    public override decimal CalculateCost(Order order)
    {
        return 20m + 5m * (decimal)order.Distance;
    }
    public override bool CanTake(Order order)
    {
        return base.CanTake(order) && order.Distance <= 10 && order.Weight <= 15;
    }
}
public class Car : Transport
{
    public Car() : base("Автомобіль") { }
    public override decimal CalculateCost(Order order)
    {
        return 50m + 8m * (decimal)order.Distance + 1m * (decimal)order.Weight;
    }
    public override bool CanTake(Order order)
    {
        return base.CanTake(order) && order.Weight <= 500;
    }
}
public class Truck : Transport
{
    public Truck() : base("Вантажівка") { }
    public override decimal CalculateCost(Order order)
    {
        return 200m + 15m * (decimal)order.Distance + 0.5m * (decimal)order.Weight;
    }
    public override bool CanTake(Order order)
    {
        return base.CanTake(order) && order.Weight <= 5000;
    }
}

public class DeliveryService
{
    private List<Transport> transports = new List<Transport>();
    private List<Order> activeOrders = new List<Order>();
    private List<Order> historyOrders = new List<Order>();
    public void AddTransport(Transport transport)
    {
        transports.Add(transport);
    }
    public void AddOrder(Order order)
    {
        activeOrders.Add(order);
        Console.WriteLine($"[СЛУЖБА] Додано нове замовлення :{order} ");
    }
    public void ShowCostsForOrder(Order order)
    {
        Console.WriteLine($"\n-- Розрахунок вартості для:{order.Description}---");
        foreach (var t in transports)
        {
            if (t.CanTake(order))
            {
                decimal cost = t.CalculateCost(order);
                Console.WriteLine($"[+] {t.Name}: {cost} грн (Доступно)");
            }
            else
            {
                Console.WriteLine($"[-] {t.Name}: Не може виконати замовлення (зайнятий або перевищено ліміти)");

            }
        }
    }
    public void AssignOrder(Order order, Transport transport)
    {
        if (transport.CanTake(order) && activeOrders.Contains(order))
        {
            transport.IsBusy = true; // Транспорт стає зайнятим
            Console.WriteLine($"\n[УСПІХ] Замовлення #{order.Number} закріплено за {transport.Name}. Транспорт тепер зайнятий.");
        }
        else
        {
            Console.WriteLine($"\n[ПОМИЛКА] Неймовірно призначити замовлення #{order.Number} на {transport.Name}.");
        }
    }

    public void CompleteDelivery(Order order, Transport transport, string paymentMethod)
    {
        if (activeOrders.Contains(order) && transport.IsBusy)
        {
            decimal cost = transport.CalculateCost(order);
            activeOrders.Remove(order);
            historyOrders.Add(order);         
            transport.IsBusy = false;

            Console.WriteLine($"[ДОСТАВКА] Замовлення #{order.Number} успішно доставлено! Оплата: {paymentMethod}. Сума: {cost} грн.");
        }
    }

    public void ShowAllLists()
    {
        Console.WriteLine("\n СТАН СИСТЕМИ ДОСТАВКИ ");
        Console.WriteLine("--- Активні замовлення ---");
        if (activeOrders.Count == 0) Console.WriteLine("Немає активних замовлень.");
        foreach (var o in activeOrders) Console.WriteLine($"  * {o}");

        Console.WriteLine("\n--- Історія доставок ---");
        if (historyOrders.Count == 0) Console.WriteLine("Історія порожня.");
        foreach (var o in historyOrders) Console.WriteLine($"  * {o} (Виконано)");
        Console.WriteLine("=======================================================\n");
    }
}
