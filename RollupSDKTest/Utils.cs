using System.Net;
using BOSagora.Rollup.BlockChain;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BOSagora.Rollup.BlockChain.Test;


public class Utils
{
    public static List<Transaction> MakeTransaction(long sequence, int count)
    {
        var rand = new Random();
        var txs = new List<Transaction>();

        var users = new[]
        {
            "2022010100001",
            "2022010100002",
            "2022010100003",
            "2022010100004",
            "2022010100005",
            "2022010100006",
            "2022010100007",
            "2022010100008",
            "2022010100009",
            "2022010100010",
            "2022010200001",
            "2022010300002",
            "2022010400003",
            "2022010500004",
            "2022010600005",
            "2022010700006",
            "2022010800007",
            "2022010900008",
            "2022011000009",
            "2022011100010",
        };
        
        for (var idx = 0; idx < count; idx++) {
            var trade_id = "91313" + DateTime.Now.ToString("yyyyMMddHH");
            var status = rand.Next(1, 10) <= 8 ? "0" : "1";
            var amount = Amount.Make((rand.Next(1, 10000) / 10000.0).ToString(), 18).Value;
            var user_id = users[rand.Next(1, users.Length)];
            var exchange_user_id = user_id;
            var exchange_id = "a5c19fed89739383";
            var now = DateTimeOffset.UtcNow;
            var unixTimeSeconds = now.ToUnixTimeSeconds();
            
            const string privateKey = "0xf6dda8e03f9dce37c081e5d178c1fda2ebdb90b5b099de1a555a658270d8c47d";
            var tx = new Transaction(
                sequence+idx,
                trade_id,
                user_id,
                status,
                amount,
                unixTimeSeconds,
                exchange_user_id,
                exchange_id);
            tx.Sign(privateKey);

            txs.Add(tx);
        }

        return txs;
    }


    public static long GetSequence()
    {
        long sequence = -1;
        var url = "https://test.rollup.bosagora.com/tx/sequence";
        var responseText = string.Empty;

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = 30 * 1000;
        request.Headers.Add("Authorization", "e5b61ba4130cbc8bebceee4fd1011c8ab427e4c9ad2b0025f39bcd6b37d5558f");

        using (var resp = request.GetResponse() as HttpWebResponse)
        {
            var status = resp.StatusCode;
            if (status == HttpStatusCode.OK)
            {
                var respStream = resp.GetResponseStream();
                using (var sr = new StreamReader(respStream))
                {
                    responseText = sr.ReadToEnd();
                    var jsonDocumentOptions = new JsonDocumentOptions
                    {
                        AllowTrailingCommas = true // 데이터 후행의 쉼표 허용 여부
                    };
                    using (var jsonDocument = JsonDocument.Parse(responseText, jsonDocumentOptions))
                    {
                        var dataElement = jsonDocument.RootElement.GetProperty("data");
                        var sequenceElement = dataElement.GetProperty("sequence");
                        sequence = Int64.Parse(sequenceElement.ToString());
                    }
                }
            }
        }

        return sequence;
    }
    
    public static bool SendTransaction(Transaction tx)
    {
        var url = "https://test.rollup.bosagora.com/tx/record";
        var request = (HttpWebRequest)WebRequest.Create(url);

        var postData = "sequence=" + Uri.EscapeDataString(tx.sequence.ToString());
        postData += "&trade_id=" + Uri.EscapeDataString(tx.trade_id);
        postData += "&user_id=" + Uri.EscapeDataString(tx.user_id);
        postData += "&state=" + Uri.EscapeDataString(tx.state);
        postData += "&amount=" + Uri.EscapeDataString(tx.amount.ToString());
        postData += "&timestamp=" + Uri.EscapeDataString(tx.timestamp.ToString());
        postData += "&exchange_user_id=" + Uri.EscapeDataString(tx.exchange_user_id);
        postData += "&exchange_id=" + Uri.EscapeDataString(tx.exchange_id);
        postData += "&signer=" + Uri.EscapeDataString(tx.signer);
        postData += "&signature=" + Uri.EscapeDataString(tx.signature);
        var data = Encoding.ASCII.GetBytes(postData);
        Console.WriteLine(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;
        request.Timeout = 30 * 1000;
        request.Headers.Add("Authorization", "e5b61ba4130cbc8bebceee4fd1011c8ab427e4c9ad2b0025f39bcd6b37d5558f");

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        try
        {
            using (var resp = request.GetResponse() as HttpWebResponse)
            {
                var status = resp.StatusCode;
                if (status == HttpStatusCode.OK) return true;
            }
        }
        catch (WebException ex)
        {
            var response = string.Empty;
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                var statusCode = 0;
                statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                Console.WriteLine(statusCode);

            }
            using (var r = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream()))
            {
                response = r.ReadToEnd();
                Console.WriteLine(response);
            }
        }
        return false;
    }
}
