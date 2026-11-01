using System;
using System.Text;

Console.OutputEncoding=Encoding.UTF8;
Console.InputEncoding=Encoding.UTF8;

double sideA = ReadPositivedouble("A");
double sideB = ReadPositivedouble("B");
double sideC = ReadPositivedouble("C");

if (TestTrikutnika(sideA, sideB, sideC))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Трикутник існує.");
    Console.ResetColor();

    double perimeter = Perimeter(sideA, sideB, sideC);
    double area = Area(sideA, sideB, sideC);
    string type = TypeTrikutnika(sideA, sideB, sideC);

    Console.WriteLine($"Периметр трикутника {perimeter}");
    Console.WriteLine($"Площа трикутника {area}");
    Console.WriteLine($"Тип трикутника {type}");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Помилка! трикутник не існує");
    Console.ResetColor();
}
Console.WriteLine("Роботу завершено");
Console.ReadKey();

 





static double ReadPositivedouble(string Username)
    {
        double value;
        while (true)
        {
            Console.WriteLine($"Введіть значення сторони {Username}:");
            string input = Console.ReadLine();
            if (double.TryParse(input, out value) && value > 0)
            {
                return value;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Помилка! Сторона повинна будути додатньою");
            Console.ResetColor();

        }

    }


static bool TestTrikutnika(double a, double b, double c)
{
    return (a + b > c) && (b + c > a) && (c + a > b);
}

static double Perimeter(double a, double b, double c)
{
    return a + b + c;
}
static double Area(double a, double b, double c)
{
    double p = Perimeter(a, b, c) / 2;
    return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
}
static string TypeTrikutnika(double a, double b, double c)
{
    if (a == b && b == c)
    {
        return "Рівносторонній";
    }
    if (a == b || b == c || c == a)
    {
        return "Рівнобедренний";
    }
    double esp = 0.000001;
    if (Math.Abs(a * a + b * b - c * c) < esp || Math.Abs(a * a + c * c - b * b) < esp || Math.Abs(b * b + c * c - a * a) < esp)
    {
        return "Прямокутний";
    }
    return "Довільний";
}
