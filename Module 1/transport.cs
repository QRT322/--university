using System;

namespace Module_1
{
    public interface ITransport
    {
        string Name { get; }
        bool IsAvailable { get; set; }
        double CalculateCost(Order order);
        bool CanTake(Order order);
    }

    public abstract class Transport : ITransport
    {
        public string Name { get; protected set; }
        public bool IsAvailable { get; set; } = true;

        protected Transport(string name)
        {
            Name = name;
        }
        public abstract double CalculateCost(Order order);
        public virtual bool CanTake(Order order)
        {
            return true;
        }
    }

    public class Bike : Transport
    {
        public Bike() : base("Велосипед (кур'єр)") { }

        public override double CalculateCost(Order order)
        {
            return 20.0 + (order.Distance * 5.0);
        }
        public override bool CanTake(Order order)
        {
            return order.Weight <= 10 && order.Distance <= 10;
        }
    }

    public class Car : Transport
    {
        public Car() : base("Автомобіль") { }

        public override double CalculateCost(Order order)
        {
            return 50.0 + (order.Distance * 8.0) + (order.Weight * 1.0);
        }
    }

    public class Truck : Transport
    {
        public Truck() : base("Вантажівка") { }

        public override double CalculateCost(Order order)
        {
            return 200.0 + (order.Distance * 15.0) + (order.Weight * 0.5);
        }
    }
}