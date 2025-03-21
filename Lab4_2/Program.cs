using System;
using System.Collections.Generic;
using System.Linq;

class STUDENT<T> : ICloneable, IComparable<STUDENT<T>>
{
    public string FullName { get; set; }
    public T GroupNumber { get; set; }
    public int[] Grades { get; set; }

    public STUDENT(string fullName, T groupNumber, int[] grades)
    {
        FullName = fullName;
        GroupNumber = groupNumber;
        Grades = grades;
    }

    public double GetAverageGrade()
    {
        return Grades.Average();
    }

    public object Clone()
    {
        return new STUDENT<T>(FullName, GroupNumber, (int[])Grades.Clone());
    }

    public int CompareTo(STUDENT<T> other)
    {
        return this.GetAverageGrade().CompareTo(other.GetAverageGrade());
    }

    public override string ToString()
    {
        return $"{FullName}, Группа: {GroupNumber}, Средний балл: {GetAverageGrade():F2}";
    }
}

class GroupNumberComparer<T> : IComparer<STUDENT<T>> where T : IComparable<T>
{
    public int Compare(STUDENT<T> x, STUDENT<T> y)
    {
        return x.GroupNumber.CompareTo(y.GroupNumber);
    }
}

class Program
{
    static void Main(string[] args)
    {
        STUDENT<int>[] students = new STUDENT<int>[3];

        for (int i = 0; i < students.Length; i++)
        {
            Console.WriteLine($"Введите данные для студента {i + 1}:");
            Console.Write("Фамилия и инициалы: ");
            string fullName = Console.ReadLine();

            Console.Write("Номер группы: ");
            int groupNumber = int.Parse(Console.ReadLine());

            int[] grades = new int[2];
            Console.WriteLine("Введите 2 оценок:");
            for (int j = 0; j < grades.Length; j++)
            {
                grades[j] = int.Parse(Console.ReadLine());
            }

            students[i] = new STUDENT<int>(fullName, groupNumber, grades);
        }

        Array.Sort(students);

        Console.WriteLine("\nСписок студентов, отсортированный по среднему баллу:");
        foreach (var student in students)
        {
            Console.WriteLine(student);
        }

        var excellentStudents = students.Where(s => s.Grades.All(g => g >= 4)).ToList();

        if (excellentStudents.Any())
        {
            Console.WriteLine("\nСтуденты с оценками 4 и 5:");
            foreach (var student in excellentStudents)
            {
                Console.WriteLine($"{student.FullName}, Группа: {student.GroupNumber}");
            }
        }
        else
        {
            Console.WriteLine("\nНет студентов с оценками 4 и 5.");
        }

        var clonedStudent = (STUDENT<int>)students[0].Clone();
        Console.WriteLine("\nКлон студент:");
        Console.WriteLine(clonedStudent);

        Array.Sort(students, new GroupNumberComparer<int>());
        Console.WriteLine("\nСписок студентов, отсортированный по номеру группы:");
        foreach (var student in students)
        {
            Console.WriteLine(student);
        }
    }
}