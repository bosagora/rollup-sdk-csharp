namespace BOSagora.Rollup.BlockChain.Test;


public class Tests2
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetSequence()
    {
        var sequence = Utils.GetSequence();
        Console.WriteLine(sequence);
    }
    
    [Test]
    public void SendTransaction()
    {
        var sequence = Utils.GetSequence();
        Console.WriteLine(sequence);
        var txs = Utils.MakeTransaction(sequence + 1, 1);
        var res = Utils.SendTransaction(txs[0]);
        Assert.That(res, Is.True);
    }
}
