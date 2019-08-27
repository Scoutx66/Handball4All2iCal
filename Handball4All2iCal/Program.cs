using Newtonsoft.Json;
using System;

namespace Handball4All2iCal {
  class Program {
    static void Main(string[] args) {
      if (args.Length != 1) {
        Console.WriteLine("Handball4All2iCal generiert Spielpläne im iCal Format von der Handball4All Plattform.");
        Console.WriteLine("Verwendung: Handball4All2iCal.exe TeamId");
        Console.WriteLine("Beispiel: Handball4All2iCal.exe 123456");
      }
      var teamId = args[0];
      var t = Handball4All2iCalLib.Handball4All2iCal.generate(teamId);
      t.Wait();
      if (t.Result)
        Console.WriteLine("Kalender " + teamId + ".ics erfolgreich generiert.");
    }
  }
}
