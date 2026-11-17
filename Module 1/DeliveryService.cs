using System;
using System.Collections.Generic;

namespace Module_1
{
    public class DeliveryService
    {
        private List<Order> activeOrders = new List<Order>();
        private List<Order> completedOrders = new List<Order>();

        public void AddOrder(Order order)
        {
            activeOrders.Add(order);
            Console.WriteLine($"[LOG] Додано нове замовлення: {order.Description} ({order.Weight} кг)");
        }

        public void ShowAvailableTransportsForOrder(Order order, List<ITransport> transports)
        {
            Console.WriteLine($"\n--- Перевірка транспорту для замовлення #{order.Id} ({order.Description}) ---");
            foreach (var transport in transports)
            {
                if (transport.CanTake(order))
                {
                    double cost = transport.CalculateCost(order);
                    Console.WriteLine($" -> {transport.Name} МОЖЕ взяти замовлення. Вартість: {cost} грн");
                }
                else
                {
                    Console.WriteLine($" -> {transport.Name} НЕ МОЖЕ взяти замовлення (не пройде за обмеженнями).");
                }
            }
        }
        public void CompleteDelivery(int orderId, ITransport transport, IPayment paymentMethod)
        {
            var order = activeOrders.Find(o => o.Id == orderId);
            if (order != null)
            {
                if (!transport.IsAvailable)
                {
                    Console.WriteLine($"[WARNING] Транспорт '{transport.Name}' наразі зайнятий!");
                    return;
                }

                if (!transport.CanTake(order))
                {
                    Console.WriteLine($"[ERROR] Транспорт '{transport.Name}' не може взяти замовлення #{order.Id} через обмеження.");
                    return;
                }

                try
                {
                    transport.IsAvailable = false;

                    double calculatedPrice = transport.CalculateCost(order);
                    paymentMethod.Pay(calculatedPrice, order);

                    order.IsCompleted = true;
                    activeOrders.Remove(order);
                    completedOrders.Add(order);

                    transport.IsAvailable = true;

                    Console.WriteLine($"[SUCCESS] Замовлення #{order.Id} успішно доставлено транспортом '{transport.Name}'.\n");
                }
                catch (Exception ex)
                {
                    transport.IsAvailable = true;
                    Console.WriteLine($"[ERROR] Помилка доставки замовлення #{order.Id}: {ex.Message}\n");
                }
            }
            else
            {
                Console.WriteLine($"[WARNING] Замовлення з ID {orderId} не знайдено в активних.\n");
            }
        }

        public void ShowAllReports()
        {
            Console.WriteLine("\n--- ФІНАЛЬНИЙ ЗВІТ СЛУЖБИ ДОСТАВКИ ---");
            Console.WriteLine($"Залишилось активних замовлень: {activeOrders.Count}");

            foreach (var order in activeOrders)
            {
                Console.WriteLine($"   (Активне) -> {order}");
            }

            Console.WriteLine($"Успішно виконаних замовлень: {completedOrders.Count}");
            foreach (var order in completedOrders)
            {
                Console.WriteLine($"   (Архів)   -> {order}");
            }
            Console.WriteLine("----------------------------------------\n");
        }
    }
}