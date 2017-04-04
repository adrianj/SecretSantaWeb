using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSantaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSantaWeb.Models.Tests
{
    [TestClass()]
    public class SecretSantaCalculatorTests
    {

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Calculate_BadInputsTest()
        {
            // Set up mappings with mappings to unknown IDs.
            Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
            dontBuyMappings[1] = 10;
            dontBuyMappings[2] = 1;
            dontBuyMappings[3] = 2;
            dontBuyMappings[4] = 3;

            SecretSantaCalculator cal = new SecretSantaCalculator(dontBuyMappings);
            cal.Calculate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Calculate_InputsTooConstrainedTest()
        {
            // Set up mappings where noone is buying for player1.
            Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
            dontBuyMappings[1] = 2;
            dontBuyMappings[2] = 1;
            dontBuyMappings[3] = 1;
            dontBuyMappings[4] = 1;

            SecretSantaCalculator cal = new SecretSantaCalculator(dontBuyMappings);
            cal.Calculate();
        }


        [TestMethod]
        public void Calculate_3PlayerTest()
        {
            // Set up mappings where everything works out fine
            Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
            dontBuyMappings[1] = 2;
            dontBuyMappings[2] = 3;
            dontBuyMappings[3] = 1;

            // Above setup only leads to one combination.
            SecretSantaCalculator cal = new SecretSantaCalculator(dontBuyMappings);
            cal.Calculate();
            Assert.AreEqual(cal.BuyForMappings[1],3);
            Assert.AreEqual(cal.BuyForMappings[2],1);
            Assert.AreEqual(cal.BuyForMappings[3],2);
            Assert.AreEqual(dontBuyMappings.Count, cal.BuyForMappings.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Calculate_1PlayerTest()
        {
            // Nobody wins when playing by themselves
            Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
            dontBuyMappings[1] = 0;

            // Above setup only leads to no assignments.
            SecretSantaCalculator cal = new SecretSantaCalculator(dontBuyMappings);
            cal.Calculate();
        }


        [TestMethod]
        public void Calculate_0PlayerTest()
        {
            // When you play the game of thrones, you win or you die.
            Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
          

            // Result should be nothing. No players to assign. Should be no exception either.
            SecretSantaCalculator cal = new SecretSantaCalculator(dontBuyMappings);
            cal.Calculate();
            Assert.AreEqual(dontBuyMappings.Count, cal.BuyForMappings.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Calculate_2PlayerExclusionTest()
        {
            // Set up mappings where both players exclude eachother
            Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
            dontBuyMappings[1] = 2;
            dontBuyMappings[2] = 1;

            // Above setup only leads to no assignments.
            SecretSantaCalculator cal = new SecretSantaCalculator(dontBuyMappings);
            cal.Calculate();
        }

        [TestMethod]
        public void Calculate_2PlayerTest()
        {
            // Set up mappings where there are no exclusions.
            Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
            dontBuyMappings[1] = 0;
            dontBuyMappings[2] = 0;

            // Above setup only leads to to one combination.
            SecretSantaCalculator cal = new SecretSantaCalculator(dontBuyMappings);
            cal.Calculate();
            Assert.AreEqual(cal.BuyForMappings[1], 2);
            Assert.AreEqual(cal.BuyForMappings[2], 1);
        }


        [TestMethod]
        public void Calculate_LargeGroupTest()
        {
            // Do this test a whole bunch of times
            for (int t = 0; t < 100; t++)
            {
                // Set up mappings with large number of players.
                // Possibility that this fails is basically 0
                Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
                Random rand = new Random();
                int nPlayers = 1000;
                for (int i = 1; i <= nPlayers; i++)
                {
                    dontBuyMappings[i] = rand.Next(nPlayers + 1);
                }

                // Above setup only leads to to one combination.
                SecretSantaCalculator cal = new SecretSantaCalculator(dontBuyMappings);
                cal.Calculate();
                Assert.AreEqual(dontBuyMappings.Count, cal.BuyForMappings.Count);
            }
        }
    }
}