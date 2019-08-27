using Microsoft.VisualStudio.TestTools.UnitTesting;
using Handball4All2iCalLib;
using System.IO;

namespace Handball4All2iCalLibTests {
  [TestClass]
  public class Handball4All2iCalLibTests {
    [TestMethod]
    public void TestMethod1() {
      var t = Handball4All2iCal.generate("503666");
      t.Wait();
      Assert.IsTrue(t.Result && File.Exists("503666.ics"));
    }
  }
}
