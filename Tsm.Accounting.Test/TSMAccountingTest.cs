using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tsm.Accounting;

namespace Tsm.Accounting.Test
{
    [TestClass]
    public class ConvertTest
    {
        [TestMethod]
        public void Base64ToDateTime_ValidDate_ReturnsDate()
        {
            // "BPfPo9" = 1333590589 = {05/Apr/2012 1:49:49 AM}
            string testDate = "BPfPo9";
            DateTime expected = new DateTime(2012, 4, 5, 1, 49, 49);

            DateTime testTime = Convert.Base64ToDateTime(testDate);

            Assert.AreEqual(expected, testTime);
        }

        [TestMethod]
        public void Base64ToLong_ValidNumber_ReturnsLong()
        {
            // "BPfPo9" = 1333590589 = {05/Apr/2012 1:49:49 AM}
            string input = "BPfPo9";
            long expected = 1333590589;

            long testTime = Convert.Base64ToLong(input);

            Assert.AreEqual(expected, testTime);
        }

        [TestMethod]
        public void UNIXTimeStampToDateTime_ValidNumber_ReturnsDate()
        {
            // "BPfPo9" = 1333590589 = {05/Apr/2012 1:49:49 AM}
            double testDate = 1333590589;
            DateTime expected = new DateTime(2012, 4, 5, 1, 49, 49);

            DateTime testTime = Convert.UNIXTimeStampToDateTime(testDate);

            Assert.AreEqual(expected, testTime);
        }
    }

    [TestClass]
    public class ParseTest
    {
        [TestMethod]
        public void GetItemID_ValidItemLink_ReturnsID()
        {
            string itemLink = "item:52190:0:0:0:0:0:0";
            long expected = 52190;

            long itemID = Parse.GetItemID(itemLink);

            Assert.AreEqual(expected, itemID);
        }

        [TestMethod]
        public void GetItemID_BlankItemLink_ReturnsZero()
        {
            string itemLink = string.Empty;
            long expected = 0;

            long itemID = Parse.GetItemID(itemLink);

            Assert.AreEqual(expected, itemID);
        }

        [TestMethod]
        public void GetItemID_InvalidItemLink_ReturnsZero()
        {
            string itemLink = "item:shouldbeanumber:0:0:0:0:0:0";
            long expected = 0;

            long itemID = Parse.GetItemID(itemLink);

            Assert.AreEqual(expected, itemID);
        }

        [TestMethod]
        public void ParseTransaction_ValidTransaction_ReturnsTransaction()
        {
            // Data format: itemString,itemName,stackSize,quantity,price,buyer,player,time,source
            // Example:     item:52190:0:0:0:0:0:0,Inferno Ruby,1,62,790000,Bixby,Legolas,1370112807,Auction

            string transaction = "item:52190:0:0:0:0:0:0,Inferno Ruby,1,62,790000,Bixby,Legolas,1370112807,Auction";

            Transaction txn = Parse.ParseTransaction("Stormrage - Horde", "buy", transaction);

            Assert.AreEqual(1, txn.StackSize, "StackSize");
            Assert.AreEqual(62, txn.Quantity, "Quantity");
            Assert.AreEqual(Convert.UNIXTimeStampToDateTime(1370112807), txn.TimeStamp, "TimeStamp");
            Assert.AreEqual(790000, txn.Amount, "Amount");
            Assert.AreEqual("Bixby", txn.BuyerName, "BuyerName");
            Assert.AreEqual("Legolas", txn.CharacterName, "CharacterName");
        }

        [TestMethod]
        public void ParseTransaction_InvalidTransaction_ReturnsNull()
        {
            string transaction = "itemString,itemName,stackSize,quantity,price,buyer,player,time,source";

            Transaction txn = Parse.ParseTransaction("Stormrage - Horde", "buy", transaction);

            Assert.IsNull(txn);
        }
    }

    [TestClass]
    public class TSMAccountingTest
    {
        [TestMethod]
        public void Load_SmallValidFile_WithoutAnError()
        {
            SavedVariables currentDB = new SavedVariables();

            string inputFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\TradeSkillMaster_Accounting_Single.lua";

            try
            {
                currentDB.Load(inputFile);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void Load_LargeValidFile_WithoutAnError()
        {
            SavedVariables currentDB = new SavedVariables();

            string inputFile = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\TradeSkillMaster_Accounting_Large.lua";

            try
            {
                currentDB.Load(inputFile);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }

    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void ToString_ValidTransaction_ReturnsString()
        {
            string transaction = "item:52190:0:0:0:0:0:0,Inferno Ruby,1,62,790000,Bixby,Legolas,1370112807,Auction";
            string expected = "Stormrage - Horde,Legolas,Bixby,buy,52190,Inferno Ruby,1,62,01/Jun/2013,18:53:27,790000";

            Transaction txn = Parse.ParseTransaction("Stormrage - Horde", "buy", transaction);            

            Assert.AreEqual(expected, txn.ToString());
        }   
    }
}
