using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace flip_flop.WebService{

   public class WeatherAPI{
    public WeatherAPI(string city)
    {
        SetCurrentURL(city);
        xmlDocument = GetXML(CurrentURL);
    }

    public float GetTemp()
    {
        XmlNode temp_node = xmlDocument.SelectSingleNode("//temperature");
        XmlAttribute temp_value = temp_node.Attributes["value"];
        string temp_string = temp_value.Value;
        return float.Parse(temp_string);
    }

    private const string APIKEY = "API KEY HERE";
    private string CurrentURL;
    private XmlDocument xmlDocument;

    private void SetCurrentURL(string location)
    {
        CurrentURL = "http://api.openweathermap.org/data/2.5/weather?q="
            + location + "&mode=xml&units=metric&APPID=" + "0f155a1ad7f279a5ef117eff93695f5d";
    }

    private XmlDocument GetXML(string CurrentURL)
    {
        using (WebClient client = new WebClient())
        {
            string xmlContent = client.DownloadString(CurrentURL);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);
            return xmlDocument;
        }
    }
}





public class WeatherData
{
    public WeatherData(string City)
    {
        city = City;
    }
    private string city;
    private float temp;
    private float tempMax;
    private float tempMin;

    public void CheckWeather()
    {
        WeatherAPI DataAPI = new WeatherAPI(City);
        temp = DataAPI.GetTemp();
    }

    public string City
    {
        get { return city; }
        set { city = value; }
    }
    public int Temp
    {
        get { return (int)temp; }
        set { temp = value; }
    }
    public float TempMax
    {
        get { return tempMax; }
        set { tempMax = value; }
    }
    public float TempMin
    {
        get { return tempMin; }
        set { tempMin = value; }

    }

    public override string ToString()
    {
        return ("" + this.Temp +"c");
    }
}
}
