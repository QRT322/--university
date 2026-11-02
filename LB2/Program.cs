using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Formatters;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

Random rand =  new Random();

int groupsCount = 4;
int[][] groups = new int[groupsCount][];
for (int i = 0; i < groupsCount; i++)
{
    int studentCount = rand.Next(10, 31);
    groups[i] = new int[studentCount];
    for (int j = 0; j < groups[i].Length; j++)
    {
        groups[i][j] = rand.Next(1, 101);
    }

}
Console.WriteLine("Масив створенно та заповненно оцінками");
Console.WriteLine("РЕЗУЛЬТАТ ПО ГРУПАМ");
for (int i = 0; i < groups.Length; i++)
{
    if (groups[i].Length == 0)  continue;
    int sum = 0;
    int min = groups[i][0];
    int max = groups[i][1];
    for (int j = 0; j < groups[i].Length; j++)
    {
        int CurrentGrade = groups[i][j];
        sum += CurrentGrade;
        if (CurrentGrade < min)
        {
          min = CurrentGrade;
        }
        if (CurrentGrade < max)
        {
            max = CurrentGrade;
        }
        
            
    }
    double average = (double)sum / groups[i].Length;
    Console.WriteLine($"Група №{i + 1} (студентів: {groups[i].Length}):");
    Console.WriteLine($"  - Середній бал: {average:F2}");
    Console.WriteLine($"  - Мінімальна оцінка: {min}");
    Console.WriteLine($"  - Максимальна оцінка: {max}");
    Console.WriteLine();
}
Console.WriteLine("РЕЗУЛЬТАТИ ПО ВСЬОМУ ПОТОКУ ");
int Totalsum  = 0;
int totalStudentsCount = 0;
int globalmin = groups[0][0];
int globalmax = groups[0][0];
for (int i = 0; i < groups.Length; i++)
{
    for (int j = 0; j < groups[i].Length; j++)
    {
        int CurrentGrade=groups[i][j];
        Totalsum += CurrentGrade;
        totalStudentsCount++;
        if (CurrentGrade < globalmin)
        {
            globalmin = CurrentGrade;
        }
        if (CurrentGrade > globalmax)
        {
            globalmax= CurrentGrade;
        }
    }
}
double globalAverage = (double)Totalsum / totalStudentsCount;
Console.WriteLine($"Усього студентів на потоці: {totalStudentsCount}");
Console.WriteLine($"Середній бал потоку: {globalAverage:F2}");
Console.WriteLine($"Мінімальна оцінка потоку: {globalmin} ");
Console.WriteLine($"Максимальна оцінка потоку: {globalmax}");
Console.WriteLine("\nРоботу завершено. Натисніть будь-яку клавішу...");
Console.ReadKey();
