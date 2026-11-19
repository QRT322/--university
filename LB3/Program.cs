using System;
using System.Text;

namespace HunterOOP
{
    public abstract class Character
    {
        private int _health;
        public string Name { get; }
        public int MaxHealth { get; }

        public int Health
        {
            get => _health;
            protected set
            {
                if (value < 0) _health = 0;
                else if (value > MaxHealth) _health = MaxHealth;
                else _health = value;
            }
        }

        public bool IsAlive => Health > 0;

        protected Character(string name, int maxHealth)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Безіменний" : name;
            MaxHealth = maxHealth > 0 ? maxHealth : 100;
            Health = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;
            Health -= damage;
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || !IsAlive) return;
            Health += amount;
        }

        public abstract void ShowInfo();
    }

    public class Hunter : Character
    {
        protected static readonly Random Rng = new Random();

        public int Gold { get; protected set; }
        public int Arrows { get; protected set; }
        public int PreyCount { get; protected set; }

        public const int ArrowPrice = 5;

        public Hunter(string name, int health, int gold, int arrows)
            : base(name, health)
        {
            Gold = gold >= 0 ? gold : 0;
            Arrows = arrows >= 0 ? arrows : 0;
            PreyCount = 0;
        }

        protected void RecordSuccessfulHunt(int earnedGold, int damageTaken)
        {
            if (earnedGold > 0) Gold += earnedGold;
            PreyCount++;
            TakeDamage(damageTaken);
        }

        public virtual void Hunt()
        {
            Console.WriteLine($"\n--- {Name} вирушає на полювання ---");

            if (!IsAlive)
            {
                Console.WriteLine($"[Помилка] {Name} занадто виснажений (0 HP) і не може полювати!");
                return;
            }

            if (Arrows <= 0)
            {
                Console.WriteLine($"[Помилка] У {Name} закінчилися стріли! Спочатку купіть їх.");
                return;
            }

            Arrows--;
            int roll = Rng.Next(1, 101);

            if (roll <= 70)
            {
                int earnedGold = Rng.Next(10, 26);
                int damageTaken = Rng.Next(0, 15);

                RecordSuccessfulHunt(earnedGold, damageTaken);

                Console.WriteLine($"[Успіх!] {Name} вполював здобич! (+1 здобич, +{earnedGold} золота, -1 стріла).");
                if (damageTaken > 0)
                {
                    Console.WriteLine($"Під час вистежування отримано подряпини: -{damageTaken} здоров'я.");
                }
            }
            else
            {
                int damageTaken = Rng.Next(10, 25);
                TakeDamage(damageTaken);

                Console.WriteLine($"[Невдача] Звір втік! Витрачено 1 стрілу, а хижак поранив мисливця (-{damageTaken} здоров'я).");
            }
        }

        public void BuyArrows(int count)
        {
            Console.WriteLine($"\n--- {Name} намагається купити стріли ({count} шт.) ---");

            if (!IsAlive)
            {
                Console.WriteLine($"[Помилка] {Name} не може торгувати, бо не має здоров'я.");
                return;
            }

            if (count <= 0)
            {
                Console.WriteLine("[Помилка] Кількість стріл для купівлі має бути більшою за 0.");
                return;
            }

            int totalCost = count * ArrowPrice;

            if (Gold < totalCost)
            {
                Console.WriteLine($"[Відмова] Недостатньо золота! Потрібно {totalCost}, а у {Name} є лише {Gold}.");
                return;
            }

            Gold -= totalCost;
            Arrows += count;
            Console.WriteLine($"[Куплено] {Name} придбав {count} стріл за {totalCost} золота. Залишок золота: {Gold}.");
        }

        public void RestAtCamp()
        {
            const int restCost = 15;
            const int healAmount = 30;

            Console.WriteLine($"\n--- {Name} відпочиває у таборі ---");
            if (!IsAlive)
            {
                Console.WriteLine($"[Помилка] {Name} вже не може відновитися.");
                return;
            }
            if (Health == MaxHealth)
            {
                Console.WriteLine($"{Name} повністю здоровий, відпочинок не потрібен.");
                return;
            }
            if (Gold < restCost)
            {
                Console.WriteLine($"[Відмова] Для відпочинку потрібно {restCost} золота (наявно: {Gold}).");
                return;
            }

            Gold -= restCost;
            Heal(healAmount);
            Console.WriteLine($"[Відновлення] {Name} відновив здоров'я до {Health}/{MaxHealth} за {restCost} золота.");
        }

        public override void ShowInfo()
        {
            string status = IsAlive ? "Активний" : "Вибув (0 HP)";
            Console.WriteLine($"Мисливець: {Name,-15} | Статус: {status,-12} | HP: {Health,3}/{MaxHealth,-3} | Золото: {Gold,3} | Стріли: {Arrows,2} | Здобич: {PreyCount}");
        }
    }

    public class VeteranHunter : Hunter
    {
        public VeteranHunter(string name, int health, int gold, int arrows)
            : base(name, health, gold, arrows) { }

        public override void Hunt()
        {
            Console.WriteLine($"\n--- [Ветеран] {Name} виходить на стежку полювання ---");

            if (!IsAlive || Arrows <= 0)
            {
                base.Hunt();
                return;
            }

            Arrows--;
            int roll = Rng.Next(1, 101);

            if (roll <= 85)
            {
                int earnedGold = Rng.Next(15, 35);
                int damageTaken = Rng.Next(0, 8);

                RecordSuccessfulHunt(earnedGold, damageTaken);
                Console.WriteLine($"[Майстерний постріл!] {Name} вполював трофей! (+1 здобич, +{earnedGold} золота, -{damageTaken} HP).");
            }
            else
            {
                int damageTaken = Rng.Next(5, 15);
                TakeDamage(damageTaken);
                Console.WriteLine($"[Промах] Навіть ветерани помиляються. Звір втік (-1 стріла, -{damageTaken} HP).");
            }
        }
    }

    public class HuntCompetition
    {
        private readonly Hunter _hunter1;
        private readonly Hunter _hunter2;

        public HuntCompetition(Hunter hunter1, Hunter hunter2)
        {
            _hunter1 = hunter1 ?? throw new ArgumentNullException(nameof(hunter1));
            _hunter2 = hunter2 ?? throw new ArgumentNullException(nameof(hunter2));
        }

        public void ShowParticipantsStatus()
        {
            Console.WriteLine("\n================ СТАН УЧАСНИКІВ ================");
            _hunter1.ShowInfo();
            _hunter2.ShowInfo();
            Console.WriteLine("=================================================");
        }

        public void RunInteractiveCompetition()
        {
            Console.WriteLine("=== ПОЧАТОК ЗМАГАННЯ МИСЛИВЦІВ ===");
            ShowParticipantsStatus();

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\nОберіть дію:");
                Console.WriteLine($"1. {_hunter1.Name}: Полювати");
                Console.WriteLine($"2. {_hunter1.Name}: Купити стріли ({Hunter.ArrowPrice} зол./шт.)");
                Console.WriteLine($"3. {_hunter1.Name}: Відпочити у таборі (15 зол.)");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"4. {_hunter2.Name}: Полювати");
                Console.WriteLine($"5. {_hunter2.Name}: Купити стріли ({Hunter.ArrowPrice} зол./шт.)");
                Console.WriteLine($"6. {_hunter2.Name}: Відпочити у таборі (15 зол.)");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("7. Переглянути статистику обох мисливців");
                Console.WriteLine("0. Завершити змагання та підбити підсумки");
                Console.Write("\nВаш вибір: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _hunter1.Hunt();
                        break;
                    case "2":
                        HandleBuyArrows(_hunter1);
                        break;
                    case "3":
                        _hunter1.RestAtCamp();
                        break;
                    case "4":
                        _hunter2.Hunt();
                        break;
                    case "5":
                        HandleBuyArrows(_hunter2);
                        break;
                    case "6":
                        _hunter2.RestAtCamp();
                        break;
                    case "7":
                        ShowParticipantsStatus();
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("[Помилка] Невідома команда. Спробуйте ще раз.");
                        break;
                }

                if (!_hunter1.IsAlive && !_hunter2.IsAlive)
                {
                    Console.WriteLine("\n[Увага] Обидва мисливці втратили все здоров'я! Змагання примусово завершено.");
                    isRunning = false;
                }
            }

            FinishCompetition();
        }

        private void HandleBuyArrows(Hunter hunter)
        {
            Console.Write($"Скільки стріл купити для {hunter.Name}? Введіть кількість: ");
            if (int.TryParse(Console.ReadLine(), out int amount))
            {
                hunter.BuyArrows(amount);
            }
            else
            {
                Console.WriteLine("[Помилка] Введено некоректне число.");
            }
        }

        private void FinishCompetition()
        {
            Console.WriteLine("\n\n*************************************************");
            Console.WriteLine("           ПІДСУМКИ ЗМАГАННЯ МИСЛИВЦІВ          ");
            Console.WriteLine("*************************************************");

            _hunter1.ShowInfo();
            _hunter2.ShowInfo();
            Console.WriteLine("-------------------------------------------------");

            if (_hunter1.PreyCount > _hunter2.PreyCount)
            {
                Console.WriteLine($"ПЕРЕМОЖЕЦЬ: {_hunter1.Name}! (Здобич: {_hunter1.PreyCount} проти {_hunter2.PreyCount})");
            }
            else if (_hunter2.PreyCount > _hunter1.PreyCount)
            {
                Console.WriteLine($"ПЕРЕМОЖЕЦЬ: {_hunter2.Name}! (Здобич: {_hunter2.PreyCount} проти {_hunter1.PreyCount})");
            }
            else
            {
                Console.WriteLine($"НІЧИЯ! Обидва мисливці вполювали однакову кількість здобичі ({_hunter1.PreyCount}).");
            }
            Console.WriteLine("*************************************************");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Hunter hunter1 = new Hunter("Артем", health: 100, gold: 25, arrows: 5);
            Hunter hunter2 = new VeteranHunter("Богдан", health: 100, gold: 20, arrows: 4);

            HuntCompetition competition = new HuntCompetition(hunter1, hunter2);
            competition.RunInteractiveCompetition();

            Console.WriteLine("\nНатисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}