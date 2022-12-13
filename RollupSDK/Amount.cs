namespace BOSagora.Rollup.BlockChain;
using System.Numerics;

/// <summary>
/// The class that defines the Amount
/// </summary>
public class Amount
{
    public BigInteger Value { get; }
    public uint Decimal { get; }
    
    public Amount(BigInteger PValue, uint PDecimal = 18)
    {
        Value = PValue;
        Decimal = PDecimal;
    }

    public static Amount Make(int PAmount, uint PDecimal = 18)
    {
        return Amount.Make(PAmount.ToString(), PDecimal);
    }
    
    public static Amount Make(string PAmount, uint PDecimal = 18)
    {
        if (PAmount.Equals("")) return new Amount(new BigInteger(0), PDecimal);
        var ZeroString = "";
        for (var idx = 0; idx < PDecimal; idx++)
            ZeroString += "0";
        var numbers = PAmount.Split('.');
        if (numbers.Length == 1) return new Amount(BigInteger.Parse(numbers[0] + ZeroString), PDecimal);
        var PointString = numbers[1];
        if (PointString.Length > PDecimal) PointString = PointString[..(int)PDecimal];
        else if (PointString.Length < PDecimal) PointString = PointString.PadRight((int)PDecimal, '0');
        var integral = BigInteger.Parse(numbers[0] + ZeroString);
        return new Amount(BigInteger.Add(integral, BigInteger.Parse(PointString)), PDecimal);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
