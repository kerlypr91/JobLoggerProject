using Microsoft.VisualStudio.TestTools.UnitTesting;
using JLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JLogger.Tests
{
    [TestClass()]
    public class JobLoggerTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid configuration")]
        public void LogMessageTest_InvalidConfiguration()
        {
            JobLogger jLog = new JobLogger(false, false, false, false, false, true);
            JobLogger.LogMessage("UNIT TESTING", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Error or Warning or Message must be specified")]
        public void LogMessageTest_NoTypeSpecified()
        {
            JobLogger jLog = new JobLogger(false, true, false, false, false, false);
            JobLogger.LogMessage("UNIT TESTING", 3);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Error or Warning or Message must be specified")]
        public void LogMessageTest_TypeNotExists()
        {
            JobLogger jLog = new JobLogger(false, true, false, false, true, true);
            JobLogger.LogMessage("UNIT TESTING", 4);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No message set")]
        public void LogMessageTest_MessageEmpty()
        {
            JobLogger jLog = new JobLogger(false, true, false, false, true, true);
            JobLogger.LogMessage("", 1);
        }

        [TestMethod]
        public void LogMessageTest_Console()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                JobLogger jLog = new JobLogger(false, true, false, true, true, true);
                JobLogger.LogMessage("TESTING", 1);

                Assert.AreEqual<string>("21/03/2016TESTING".Trim(), Convert.ToString(sw).Trim());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void LogMessageTest_Database()
        {
            JobLogger jLog = new JobLogger(false, false, true, true, true, true);
            JobLogger.LogMessage("TESTING", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.DirectoryNotFoundException))]
        public void LogMessageTest_File()
        {
            JobLogger jLog = new JobLogger(true, false, false, true, true, true);
            JobLogger.LogMessage("TESTING", 1);
        }

    }
}