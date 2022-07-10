<%@ WebHandler Language="C#" Class="Handler" %>
using System;
using System.Web;
using System.Xml;
using Newtonsoft.Json;

public class Handler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string gameCode = context.Request["gameCode"]; //קוד המשחק שנשלח מאנימייט

        XmlDocument myDoc = new XmlDocument();
        myDoc.Load(context.Server.MapPath("XML/LaCasaDeCookie.xml")); //טעינת העץ שלכם

        XmlNode gameNode = myDoc.SelectSingleNode("//game[@gameCode='" + gameCode + "']"); //שליפת הענף של המשחק המתאים

        if (gameNode != null) //אם קיים משחק שתואם לקוד
        {
            if (bool.Parse(gameNode.Attributes["isPublish"].Value)) // אם המשחק מפורסם
            {
                //ההמרה לג'ייסון תתבצע רק אם המשחק קיים ומפורסם
                string jsonText = JsonConvert.SerializeXmlNode(gameNode); //המרת הענף מהעץ לטקסט במבנה של ג'ייסון
                context.Response.Write(jsonText); //שליחת המחרוזת אל אנימייט
            }
            else // // אם המשחק לא מפורסם
            {
                context.Response.Write("notPublished"); //המשחק לא מפורסם
            }
        }
        else //אם המשחק לא קיים
        {
            context.Response.Write("noGameFound"); //שליחת תשובה שלא נמצא משחק
        }
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
















