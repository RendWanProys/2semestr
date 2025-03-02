public interface IPair
{
    IPair Add(IPair other);
    IPair Sub(IPair other);
    IPair Mul(IPair other);
    IPair Div(IPair other);
    string ToString();
    bool Equals(object obj);
    int GetHashCode();
}
public class Complex : IPair
{
    public double Real { get; set; }
    public double UnReal { get; set; }

    public Complex(double real, double unreal)
    {
        Real = real;
        UnReal = unreal;
    }
    public IPair Add(IPair other)
    {
        if (other is Complex complexOther)
            return new Complex(Real + complexOther.Real, UnReal + complexOther.UnReal);
        throw new ArgumentException("Неверный тип объекта");
    }
    public IPair Sub(IPair other)
    {
        if (other is Complex complexOther)
            return new Complex(Real - complexOther.Real, UnReal - complexOther.UnReal);
        throw new ArgumentException("Неверный тип объекта");
    }
    public IPair Mul(IPair other)
    {
        if (other is Complex complexOther)
        {
            double newReal = Real * complexOther.Real - UnReal * complexOther.UnReal;
            double newUnReal = Real * complexOther.UnReal + UnReal * complexOther.Real;
            return new Complex(newReal, newUnReal);
        }
        throw new ArgumentException("Неверный тип объекта");
    }
    public IPair Div(IPair other)
    {
        if (other is Complex complexOther)
        {
            double denominator = complexOther.Real * complexOther.Real + complexOther.UnReal * complexOther.UnReal;
            if (denominator == 0)
            {
                throw new DivideByZeroException("Деление на ноль!");
            }
            double newReal = (Real * complexOther.Real + UnReal * complexOther.UnReal) / denominator;
            double newUnReal = (UnReal * complexOther.Real - Real * complexOther.UnReal) / denominator;
            return new Complex(newReal, newUnReal);
        }
        throw new ArgumentException("Неверный тип объекта");
    }
    public bool Equ(Complex other)
    {
        const double epsilon = 1e-6;
        return Math.Abs(Real - other.Real) < epsilon && Math.Abs(UnReal - other.UnReal) < epsilon;
    }
    public Complex Conj()
    {
        return new Complex(Real, -UnReal);
    }
    public override string ToString()
    {
        return $"{Real}+{UnReal}i";
    }
    public override bool Equals(object obj)
    {
        if (obj is Complex other)
            return Equ(other);
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Real, UnReal);
    }
}
public class Money : IPair
{
    public long Rubles { get; set; }
    public byte Kopeyka { get; set; }

    public Money(long rubles, byte kopeyka)
    {
        Rubles = rubles;
        Kopeyka = kopeyka;
        Normalize();
    }
    public Money(double amount)
    {
        long rubles = (long)amount;
        byte kopeyka = (byte)Math.Round((amount - rubles) * 100);
        Rubles = rubles;
        Kopeyka = kopeyka;
        Normalize();
    }
    private void Normalize()
    {
        if (Kopeyka >= 100)
        {
            Rubles += Kopeyka / 100;
            Kopeyka = (byte)(Kopeyka % 100);
        }
        else if (Kopeyka < 0)
        {
            Rubles += (Kopeyka / 100) - 1;
            Kopeyka = (byte)(Kopeyka + 100);
        }
        if (Rubles < 0 && Kopeyka > 0)
        {
            Rubles++;
            Kopeyka = (byte)(100 - Kopeyka);
        }
    }
    public IPair Add(IPair other)
    {
        if (other is Money moneyOther)
            return new Money(Rubles + moneyOther.Rubles, (byte)(Kopeyka + moneyOther.Kopeyka));
        throw new ArgumentException("Неверный тип объекта");
    }
    public IPair Sub(IPair other)
    {
        if (other is Money moneyOther)
            return new Money(Rubles - moneyOther.Rubles, (byte)(Kopeyka - moneyOther.Kopeyka));
        throw new ArgumentException("Неверный тип объекта");
    }
    public IPair Div(IPair other)
    {
        if (other is Money moneyOther)
        {
            double totalAmount = ToDouble() / moneyOther.ToDouble();
            return new Money(totalAmount);
        }
        if (other is Complex)
        {
            throw new ArgumentException("Неверный тип объекта");
        }
        if (other is double)
        {
            double number = (double)Convert.ToDouble(other);
            if (number == 0)
            {
                throw new DivideByZeroException("Деление на 0!");
            }
            double totalAmount = ToDouble() / number;
            return new Money(totalAmount);
        }
        throw new ArgumentException("Неверный тип объекта");
    }
    public IPair Mul(IPair other)
    {
        if (other is Money moneyOther)
        {
            double totalAmount = ToDouble() * moneyOther.ToDouble();
            return new Money(totalAmount);
        }
        if (other is Complex)
        {
            throw new ArgumentException("Неверный тип объекта");
        }
        if (other is double)
        {
            double number = (double)Convert.ToDouble(other);
            double totalAmount = ToDouble() * number;
            return new Money(totalAmount);
        }
        throw new ArgumentException("Неверный тип объекта");
    }
    public double ToDouble()
    {
        return Rubles + Kopeyka / 100.0;
    }
    public override bool Equals(object obj)
    {
        if (obj is Money other)
        {
            return Rubles == other.Rubles && Kopeyka == other.Kopeyka;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Rubles, Kopeyka);
    }
    public override string ToString()
    {
        return $"{Rubles},{Kopeyka:D2}";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Тестирование Complex:");
        Complex c1 = new Complex(3, 4);
        Complex c2 = new Complex(1, 2);

        Console.WriteLine($"c1 = {c1}");
        Console.WriteLine($"c2 = {c2}");

        IPair resultAdd = c1.Add(c2);
        Console.WriteLine($"c1 + c2 = {resultAdd}");

        IPair resultSub = c1.Sub(c2);
        Console.WriteLine($"c1 - c2 = {resultSub}");

        IPair resultMul = c1.Mul(c2);
        Console.WriteLine($"c1 * c2 = {resultMul}");

        IPair resultDiv = c1.Div(c2);
        Console.WriteLine($"c1 / c2 = {resultDiv}");

        Console.WriteLine();

        Console.WriteLine("Тестирование Money:");
        Money m1 = new Money(10, 50);
        Money m2 = new Money(5, 75);

        Console.WriteLine($"m1 = {m1}");
        Console.WriteLine($"m2 = {m2}");

        IPair moneyAdd = m1.Add(m2);
        Console.WriteLine($"m1 + m2 = {moneyAdd}");

        IPair moneySub = m1.Sub(m2);
        Console.WriteLine($"m1 - m2 = {moneySub}");

        IPair moneyMul = m1.Mul(m2);
        Console.WriteLine($"m1 * m2 = {moneyMul}");

        IPair moneyDiv = m1.Div(m2);
        Console.WriteLine($"m1 / m2 = {moneyDiv}");
    }
}