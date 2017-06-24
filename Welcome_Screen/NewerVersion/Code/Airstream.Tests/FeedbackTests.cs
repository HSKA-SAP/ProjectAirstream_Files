using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airstream.Tests
{

    [TestClass]
    public class FeedbackTests
    {
        Feedback.Database.DatabaseConnection db;
        [TestInitialize]
        public void StartTests()
        {
            db = new Feedback.Database.DatabaseConnection();

        }

        [TestMethod]
        public void SelectFromFeedbackTests()
        {
            string q = @"SELECT * FROM feedback";
            List<string>[] result = db.SelectFromFeedback(q);
            //Test column 1 data  - expect int
            for(int i = 0; i < result[0].ToArray().Length; i++)
            {
                int num;
                bool isNum  = Int32.TryParse(result[0][i], out num);
                Assert.AreEqual(isNum, true);
            }
            //Test column 2 data - expect int
            for (int i = 0; i < result[1].ToArray().Length; i++)
            {
                int num;
                bool isNum = Int32.TryParse(result[1][i], out num);
                Assert.AreEqual(isNum, true);
            }
            //Test column 3 data  - expect string > 0 
            for (int i = 0; i < result[2].ToArray().Length; i++)
            {
                Assert.AreEqual(result[2][i].GetType(), typeof(string));
                Assert.IsTrue(result[2][i].Length > 0);
            }

        }
        [TestMethod]
        public void SelectFromUsersTest()
        {
            string q = @"SELECT * FROM users";
            List<string>[] result = db.SelectFromUsers(q);
            //Test column 1 data  - expect int
            for (int i = 0; i < result[0].ToArray().Length; i++)
            {
                int num;
                bool isNum = Int32.TryParse(result[0][i], out num);
                //First column is a number
                Assert.AreEqual(isNum, true);
            }
            //Test column 2 data - expect string > 0 
            for (int i = 0; i < result[1].ToArray().Length; i++)
            {
                Assert.AreEqual(result[1][i].GetType(), typeof(string));
                Assert.IsTrue(result[1][i].Length > 0);
            }
            //Test column 3 data  - expect string > 0 
            for (int i = 0; i < result[2].ToArray().Length; i++)
            {
                Assert.AreEqual(result[2][i].GetType(), typeof(string));
                Assert.IsTrue(result[2][i].Length > 0);
            }

            //Test column 4 data  - expect string > 0 
            for (int i = 0; i < result[3].ToArray().Length; i++)
            {
                string[] companies = { "SAP", "Deutsche Bank", "Sparkasse", "Commerzbank" };

                Assert.AreEqual(result[3][i].GetType(), typeof(string));
                Assert.IsTrue(result[3][i].Length > 0);
                Assert.IsTrue(companies.Contains(result[3][i]));
            }

        }


        [TestCleanup]
        public void TestTearDown()
        {
            db.CloseConnection();
            db = null;
        }
      
    }
}
