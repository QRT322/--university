using System;
using System.Collections.Generic;
using System.Text;

namespace Mkr_1
{
    
    public class Order
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public double Distance { get; set; }

        public Order(int number, string description, double weight, double distance)
        {
            Number = number;
            Description = description;
            Weight = weight;
            Distance = distance;
        }

        public override string ToString()
        {
            return $"Замовлення #{Number} ({Description}): вага {Weight} кг, відстань {Distance} км";
        }
    }
}
