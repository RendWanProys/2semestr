public abstract class Pair
{
    public abstract Pair Add(Pair other);
    public abstract Pair Sub(Pair other);
    public abstract Pair Mul(Pair other);
    public abstract Pair Div(Pair other);
    public abstract override string ToString();
    public abstract override bool Equals(object obj);
    public abstract override int GetHashCode();
}
public class Complex : Pair
{
    public double Real { get; set; }
    public double UnReal { get; set; }
    public Complex(double real, double unreal)
    {
        Real = real;
        UnReal = unreal;
    }
    public override Pair Add(Pair other)
    {
        if (other is Complex complexOther)
            return new Complex(Real + complexOther.Real, UnReal + complexOther.UnReal);
        throw new ArgumentException("Неверный тип объекта");
    }
    public override Pair Sub(Pair other)
    {
        if (other is Complex complexOther)
            return new Complex(Real - complexOther.Real, UnReal - complexOther.UnReal);
        throw new ArgumentException("Неверный тип объекта");
    }
    public override Pair Mul(Pair other)
    {
        if (other is Complex complexOther)
        {
            double newReal = Real * complexOther.Real - UnReal * complexOther.UnReal;
            double newUnReal = Real * complexOther.UnReal + UnReal * complexOther.Real;
            return new Complex(newReal, newUnReal);
        }
        throw new ArgumentException("Неверный тип объекта");
    }
    public override Pair Div(Pair other)
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
public class Money : Pair
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
    public override Pair Add(Pair other)
    {
        if (other is Money moneyOther)
            return new Money(Rubles + moneyOther.Rubles, (byte)(Kopeyka + moneyOther.Kopeyka));
        throw new ArgumentException("Неверный тип объекта");
    }
    public override Pair Sub(Pair other)
    {
        if (other is Money moneyOther)
            return new Money(Rubles - moneyOther.Rubles, (byte)(Kopeyka - moneyOther.Kopeyka));
        throw new ArgumentException("Неверный тип объекта");
    }
    public override Pair Div(Pair other)
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
    public override Pair Mul(Pair other)
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
