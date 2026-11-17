using System;
using System.Collections.Generic;
using System.Text;

namespace Module_1
{
    public class Order
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double Distance { get; set; }
        public decimal Cost { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string pay { get; set; } = "не оплачено";

        public Order(int id, string description, double weight, double distance, decimal cost)
        {
            Id = id;
            Description = description;
            Weight = weight;
            Distance = distance;
            Cost = cost;
        }

        public override string ToString()
        {
            return $"Замовлення #{Id}: {Description} (Вага: {Weight}кг) — Статус: {(IsCompleted ? "Виконано" : "Активне")}, Оплата: {pay}";
        }
    }
}