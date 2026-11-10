using System;
using System.Collections.Generic;
using System.Text;

namespace Mkr_1
{
    
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
        public Bicycle() : base("Велосипед") { }

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
}
