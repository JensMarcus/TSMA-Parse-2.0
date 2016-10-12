using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LuaInterface;

namespace Tsm.Accounting
{
    public class Convert
    {
        /// <summary>
        /// Convert a UNIX timestamp encoded in base 64 to a C# DateTime.
        /// </summary>
        /// <param name="base64Value"></param>
        /// <returns></returns>
        public static DateTime Base64ToDateTime(string base64Value)
        {
            long epochTime = 0;

            epochTime = Convert.Base64ToLong(base64Value);

            var dotNETDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dotNETDateTime.AddSeconds(epochTime);
        }

        /// <summary>
        /// Convert a base 64 encoded number to a long.
        /// </summary>
        /// <param name="base64Value"></param>
        /// <returns></returns>
        public static long Base64ToLong(string base64Value)
        {
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_=";
            long value = 0;

            base64Value = base64Value.Trim();

            for (int i = 0; i < base64Value.Length; i++)
            {
                if (alpha.IndexOf(base64Value[i]) != 0)
                {
                    value += alpha.IndexOf(base64Value[i]) * (long)Math.Pow(64, base64Value.Length - i - 1);
                }
            }

            return value;
        }

        /// <summary>
        /// Convert a UNIX timestamp to a C# DateTime.
        /// </summary>
        /// <param name="base64Value"></param>
        /// <returns></returns>
        public static DateTime UNIXTimeStampToDateTime(double UNIXTimeStamp)
        {
            var dotNETDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dotNETDateTime.AddSeconds(UNIXTimeStamp);
        }
    }

    public class Parse
    {
        /// <summary>
        /// Get the item ID from the item link. The item link contains extra info for items with random enchantments (i.e. of the Monkey),
        /// but we only really care about the base item ID.
        /// </summary>
        /// <param name="itemLink">string that contains the itemLink (i.e. item:52190:0:0:0:0:0:0)</param>
        /// <returns></returns>
        public static long GetItemID(string itemLink)
        {
            string[] itemInfo = itemLink.Split(':');
            long itemID;

            try
            {
                itemID = long.Parse(itemInfo[1]);
            }
            catch
            {
                itemID = 0;
            }

            return itemID;
        }

        public static Transaction ParseTransaction(string factionRealm, string buySell, string transaction)
        {
            // Split the details of each transaction
            // Data format: itemString,itemName,stackSize,quantity,price,buyer,player,time,source
            // Example:     item:52190:0:0:0:0:0:0,Inferno Ruby,1,62,790000,Bixby,Legolas,1370112807,Auction

            Transaction txn;

            try
            {
                string[] details = transaction.Split(',');

                if (details[8] != "Auction")
                {
                    return null;
                }

                // Add the parsed transaction the stored transaction list
                txn = new Transaction();

                txn.FactionRealm = factionRealm;
                txn.CharacterName = details[6];
                txn.BuyerName = details[5];
                txn.BuySell = buySell;
                txn.ItemLink = details[0];
                txn.ItemName = details[1];
                txn.ItemID = Parse.GetItemID(details[0]);
                txn.StackSize = long.Parse(details[2]);
                txn.Quantity = long.Parse(details[3]);
                txn.TimeStamp = Convert.UNIXTimeStampToDateTime(double.Parse(details[7]));
                txn.Amount = long.Parse(details[4]);
            }
            catch
            {
                txn = null;
            }

            return txn;
        }
    }

    public class SavedVariables
    {
        private static Lua lua = new Lua(); // Embedded Lua interpreter
        private Dictionary<string, string> itemList = new Dictionary<string, string>(); // Item list extracted from saved variables
        private List<Transaction> transactionList = new List<Transaction>();  // List of parsed transactions

        /// <summary>
        /// Produces a list of transactions. Details of each transaction are separated by commas.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ListTransactions(string.Empty, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Produces a list of transactions. Details of each transaction are separated by commas. Leave a 
        /// parameter blank if you don't want to filter by that type.
        /// </summary>
        /// <param name="factionRealm"></param>
        /// <param name="characterName"></param>
        /// <param name="buySell"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string ListTransactions(string factionRealm, string characterName, string buySell, string itemName)
        {
            StringBuilder strBuilder = new StringBuilder();

            // Output the header line
            strBuilder.AppendLine("FactionRealm,CharacterName,BuyerName,BuySell,ItemID,ItemName,StackSize,Quantity,Date,Time,Amount");

            var query = from Transaction txn in this.transactionList
                        where (factionRealm == string.Empty || txn.FactionRealm == factionRealm)
                        && (characterName == string.Empty || txn.CharacterName == characterName)
                        && (buySell == string.Empty || txn.BuySell == buySell)
                        && (itemName == string.Empty || txn.ItemName == itemName)
                        select txn;

            foreach (Transaction txn in query)
            {
                strBuilder.AppendLine(txn.ToString());
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Write the parsed transaction list to the specified file. Details of each transaction
        /// are written in a comma delimited format.
        /// </summary>
        /// <param name="fileName"></param>
        public void WriteToFile(string fileName)
        {
            using (System.IO.StreamWriter outFile = new System.IO.StreamWriter(fileName))
            {
                outFile.Write(this.ToString());
            }
        }

        /// <summary>
        /// Read the specified TSM_Accounting saved variables file and parse the transaction information.
        /// </summary>
        /// <param name="fileName"></param>
        public void Load(string fileName)
        {
            // Use the embedded Lua interpreter to load the saved variables file
            lua.DoFile(fileName);

            try
            {
                // Process sales data from each faction/realm combination
                foreach (DictionaryEntry factionRealm in lua.GetTable("TradeSkillMaster_AccountingDB.factionrealm"))
                {
                    // Proccess Auction Sales
                    this.ProcessItemSalesData(factionRealm.Key.ToString(), lua.GetString("TradeSkillMaster_AccountingDB.factionrealm." + factionRealm.Key.ToString() + ".csvSales"), "sell");

                    // Proccess Auction Purchases
                    this.ProcessItemSalesData(factionRealm.Key.ToString(), lua.GetString("TradeSkillMaster_AccountingDB.factionrealm." + factionRealm.Key.ToString() + ".csvBuys"), "buy");
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to parse file: {0}", e.Message), e);
            }
        }

        private void ProcessItemSalesData(string factionRealm, string salesData, string buySell)
        {
            // ["csvSales"] = "itemString,itemName,stackSize,quantity,price,buyer,player,time,source\nitem:74837:0:0:0:0:0:0,Raw Turtle Meat,1,4,45030,Bixby,Legolas,1370111737,Auction\nitem:74837:0:0:0:0:0:0,Raw Turtle Meat,12,12,45030,Tinhat,Legolas,1370112807,Auction\n
            //
            // itemString,itemName,stackSize,quantity,price,buyer,player,time,source
            // item:74837:0:0:0:0:0:0,Raw Turtle Meat,1,4,45030,Bixby,Legolas,1370111737,Auction
            // item:74837:0:0:0:0:0:0,Raw Turtle Meat,12,12,45030,Tinhat,Legolas,1370112807,Auction

            // If there's nothing to process, we're done
            if (salesData == null || salesData == string.Empty)
            {
                return;
            }

            // Transactions are all listed in one long string, separated by \n
            string[] transactionList = Regex.Split(salesData, "\n");

            foreach (string transaction in transactionList)
            {
                Transaction txn = Parse.ParseTransaction(factionRealm, buySell, transaction);

                if (txn != null)
                {
                    this.transactionList.Add(txn);
                }
            }
        }
    }

    public class Transaction
    {
        public string FactionRealm { get; set; }
        public string CharacterName { get; set; }
        public string BuyerName { get; set; }
        public string BuySell { get; set; }
        public string ItemLink { get; set; }
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public long StackSize { get; set; }
        public long Quantity { get; set; }
        public DateTime TimeStamp { get; set; }
        public long Amount { get; set; }

        public override string ToString()
        {
            return string.Format(
                        "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                        this.FactionRealm,
                        this.CharacterName,
                        this.BuyerName,
                        this.BuySell,
                        this.ItemID,
                        this.ItemName,
                        this.StackSize,
                        this.Quantity,
                        this.TimeStamp.ToShortDateString(),
                        this.TimeStamp.TimeOfDay,
                        this.Amount);
        }
    }
}
