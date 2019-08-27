using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Handball4All2iCalLib {
  public class Handball4All2iCal {
    const string h4aUrl = @"https://m.h4a.mobi/php/spo-proxy_public.php?cmd=data&lvTypeNext=team&lvIDNext=";
    private static readonly HttpClient client = new HttpClient();
    private static async Task<string> getJson(string url) {
      string result = null;
      try {
        HttpResponseMessage response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode) {
          result = await response.Content.ReadAsStringAsync();
        }
      } catch (Exception e) {
        Console.WriteLine(e.Message);
      }
      return result;
    }
    public static async Task<bool> generate(string teamId) {
      try {
        var json = await getJson(h4aUrl + teamId);
        json = json.Substring(1, json.Length - 2);
        dynamic dyn = JsonConvert.DeserializeObject(json);
        string teamName = dyn.lvTypeLabelStr;
        StringBuilder sb = new StringBuilder();
        sb.Append("BEGIN:VCALENDAR" + Environment.NewLine);
        sb.Append("VERSION:2.0" + Environment.NewLine);
        sb.Append("PRODID: Handball4All2iCal" + Environment.NewLine);
        sb.Append("CALSCALE:GREGORIAN" + Environment.NewLine);
        foreach (dynamic game in dyn.dataList) {
          string sDate = game.gDate;
          string sTime = game.gTime;
          string sPlace = game.gGymnasiumName;
          string sPostal = game.gGymnasiumPostal;
          string sCity = game.gGymnasiumTown;
          string sStreet = game.gGymnasiumStreet;
          string sHomeTeam = game.gHomeTeam;
          string sGuestTeam = game.gGuestTeam;
          DateTime dtTime;
          if (DateTime.TryParse(sDate + " " + sTime, out dtTime)) {
            sb.Append("BEGIN:VEVENT" + Environment.NewLine);
            sb.Append("DTSTART:" + dtTime.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + Environment.NewLine);
            sb.Append("DTEND:" + dtTime.AddHours(2).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + Environment.NewLine);
            sb.Append("LOCATION:" + sPlace + ", " + sStreet + ", " + sPostal + " " + sCity + Environment.NewLine);
            sb.Append("DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ") + Environment.NewLine);
            sb.Append("SUMMARY:" + sHomeTeam + " - " + sGuestTeam + Environment.NewLine);
            sb.Append("UID:" + Guid.NewGuid().ToString() + Environment.NewLine);
            sb.Append("END:VEVENT" + Environment.NewLine);
          }
        }
        sb.Append("END:VCALENDAR" + Environment.NewLine);
        File.WriteAllText(teamId + ".ics", sb.ToString(), Encoding.UTF8);
        return true;
      } catch (Exception e) {
        Console.WriteLine(e.Message);
      }
      return false;
    }
  }
}
