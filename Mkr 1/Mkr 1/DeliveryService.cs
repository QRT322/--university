using System;
using System.Collections.Generic;
using System.Text;

namespace Mkr_1
{
    using System;
    using System.Collections.Generic;

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
            Console.WriteLine($"[СЛУЖБА] Додано нове замовлення: {order}");
        }

        public void ShowCostsForOrder(Order order)
        {
            Console.WriteLine($"\n-- Розрахунок вартості для: {order.Description} ---");
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
                transport.IsBusy = true;
                Console.WriteLine($"\n[УСПІХ] Замовлення #{order.Number} закріплено за {transport.Name}. Транспорт тепер зайнятий.");
            }
            else
            {
                Console.WriteLine($"\n[ПОМИЛКА] Неможливо призначити замовлення #{order.Number} на {transport.Name}.");
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
}
