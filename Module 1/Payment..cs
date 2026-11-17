using System;

namespace Module_1
{
    public interface IPayment
    {
        void Pay(double cost, Order order);
    }

    public class PayCash : IPayment
    {
        public void Pay(double cost, Order order)
        {
            Console.WriteLine($"Ви заплатили {cost} грн готівкою.");
            order.pay = "cash";
        }
    }

    public class PayCard : IPayment
    {
        public void Pay(double cost, Order order)
        {
            Console.WriteLine($"Ви заплатили {cost} грн карткою.");
            order.pay = "card";
        }
    }
}