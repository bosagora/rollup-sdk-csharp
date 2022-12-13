using Nethereum.Web3.Accounts;
using Nethereum.Signer;
using System;
using System.Numerics;
using System.Text;


namespace BOSagora.Rollup.BlockChain
{
	/// <summary>
	/// The class that defines the transaction of a block.
	/// </summary>
    public class Transaction
    {
	    /// <summary>
	    /// Of all transactions, it starts at zero and increases by one in a unique sequence
	    /// </summary>
	    public long sequence;
	    
	    /// <summary>
	    /// ID of the trade
	    /// </summary>
		public string trade_id;
	    
	    /// <summary>
	    /// The ID of User
	    /// </summary>
		public string user_id;
	    
	    /// <summary>
	    /// The type of transaction, "0" : charge, "1" : discharge
	    /// </summary>
		public string state;
	    
	    /// <summary>
	    /// The amount of sending
	    /// </summary>
		public BigInteger amount;
	    
	    /// <summary>
	    /// The time stamp
	    /// </summary>
		public long timestamp;
	    
	    /// <summary>
	    /// The exchange user id
	    /// </summary>
		public string exchange_user_id;
	    
	    /// <summary>
	    /// The exchange id
	    /// </summary>
		public string exchange_id;
	    
	    /// <summary>
	    /// The signer
	    /// </summary>
        public string signer;
	    
	    /// <summary>
	    /// The signature
	    /// </summary>
        public string signature;

        public Transaction(long _sequence, string _trade_id, string _user_id, string _state, BigInteger _amount, long _timestamp, string _exchange_user_id, string _exchange_id, string _signer = "", string _signature = "")
		{
			sequence = _sequence;
			trade_id = _trade_id;
			user_id = _user_id;
			state = _state;
			amount = _amount;
			timestamp = _timestamp;
			exchange_user_id = _exchange_user_id;
			exchange_id = _exchange_id;
			signer = _signer;
			signature = _signature;
        }

		public string ToJSON()
		{
            var json = new StringBuilder();
            json.Append('{');
            json.Append($"\"sequence\":{sequence.ToString()},");
            json.Append($"\"trade_id\":\"{trade_id}\",");
            json.Append($"\"user_id\":\"{user_id}\",");
            json.Append($"\"state\":\"{state}\",");
            json.Append($"\"amount\":\"{amount.ToString()}\",");
            json.Append($"\"timestamp\":{timestamp.ToString()},");
            json.Append($"\"exchange_user_id\":\"{exchange_user_id}\",");
            json.Append($"\"exchange_id\":\"{exchange_id}\",");
            json.Append($"\"signer\":\"{signer}\",");
            json.Append($"\"signature\":\"{signature}\"");
            json.Append('}');
            return json.ToString();
        }

        public Hash ToHash()
        {
            var builder = new HashBuilder();

            builder.Update(sequence);
            builder.Update(trade_id);
            builder.Update(user_id);
            builder.Update(state);
            builder.Update(amount);
            builder.Update(timestamp);
            builder.Update(exchange_user_id);
            builder.Update(exchange_id);
            builder.Update(signer);

            return builder.Digest();
        }

        public void Sign(Account account)
        {
            signer = account.Address;
            var msg = ToHash().ToString();
            var messageSigner = new EthereumMessageSigner();
            signature = messageSigner.EncodeUTF8AndSign(msg, new EthECKey(account.PrivateKey));
        }

        public void Sign(string privateKey)
        {
	        var account = new Account(privateKey);
	        signer = account.Address;
	        var msg = ToHash().ToString();
	        var messageSigner = new EthereumMessageSigner();
	        signature = messageSigner.EncodeUTF8AndSign(msg, new EthECKey(account.PrivateKey));
        }

        public bool Verify()
        {
            var msg = ToHash().ToString();
            var messageSigner = new EthereumMessageSigner();
            var res = messageSigner.EncodeUTF8AndEcRecover(msg, signature);
            return res.ToLower() == signer.ToLower();
        }
    }
}
