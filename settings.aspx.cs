using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class settings : System.Web.UI.Page
{
    // משתנה גלבולי לשמירת עץ ה-XML 
    XmlDocument LaCasaDeCookieXML = new XmlDocument();

    // שמירת קיצור לענף של המשחק הנבחר
    XmlNode gameNode;

    // שיטה שמתבצעת פעם אחת בטעינת העמוד 
    protected void Page_init(object sender, EventArgs e)
    {
        // בדיקה אם המשתמשת לא מחוברת - לשלוח אותה לעמוד הכניסה
        if (Session["user"] == null)
        {
            Response.Redirect("sessionTimeout.aspx");
        }
        // בדיקת הגנה נוספת אם לא נבחר משחק - להעביר לעמוד המשחקים שלי
        if (Session["gameIDSession"] == null)
        {
            Response.Redirect("myGames.aspx");
        }
        //קבלת המספר הסידורי של המשחק מתוך הסשן
        string gameID = Session["gameIDSession"].ToString();
        // טעינת העץ לתוך המשתנה הגלובלי
        LaCasaDeCookieXML.Load(Server.MapPath("XML/LaCasaDeCookie.xml"));
        // מציאת הענף שמכיל את המשחק ושמירה במשתנה הגלובלי
        gameNode = LaCasaDeCookieXML.SelectSingleNode("//game[@gameCode=" + gameID + "]");
        // הדפסת שם המשחק לתיבת הטקסט
        gameNameTxtBox.Text = Server.UrlDecode(gameNode.SelectSingleNode("gameName").InnerXml);

        // הצגת זמן לשאלה לפי המידע מהעץ
        switch (gameNode.Attributes["timePerQuest"].InnerText)
        {
            case "0":
                timePerQuestRB.SelectedIndex = 3;
                break;
            case "30":
                timePerQuestRB.SelectedIndex = 0;
                break;
            case "60":
                timePerQuestRB.SelectedIndex = 1;
                break;
            case "90":
                timePerQuestRB.SelectedIndex = 2;
                break;
            default:
                break;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    { }


    protected void SaveSettingsBtn_Click(object sender, EventArgs e)
    {
        UpdateSettingsInTree();
        // מעבר למשחקים שלי
        Response.Redirect("myGames.aspx");
    }

    protected void SaveSettingsAndEditBtn_Click(object sender, EventArgs e)
    {
        UpdateSettingsInTree();
        // מעבר לעריכת שאלות
        Response.Redirect("edit.aspx");
    }

    // עדכון הגדרות חדשות בעץ
    protected void UpdateSettingsInTree()
    {
        //עדכון של השם החדש בעץ
        gameNode.SelectSingleNode("gameName").InnerText = Server.UrlEncode(gameNameTxtBox.Text);// קידוד - בהוספת ערכים טקסטואלים לעץ

        // עדכון זמן חדש לשאלה
        // עדכון של המאפיין בעץ לפי מה שסומן בממשק
        gameNode.Attributes["timePerQuest"].InnerText = timePerQuestRB.SelectedValue;

        // שמירת השינויים בעץ
        LaCasaDeCookieXML.Save(Server.MapPath("XML/LaCasaDeCookie.xml"));
    }


    // שיטה שמתרחשת בלחיצה על הלינק בחזרה למשחקים שלי
    //  אם התבצעו שינויים - צריך לקפוץ פופאפ
    protected void ReturnMyGamesLinkInSettings_Click(object sender, EventArgs e)
    {
        // בדיקה האם קיים תוכן חדש ולא שמור בממשק                 
        if (WereChangesMade())
        {
            // פתיחת המודל באמצעות הג'אווה סקריפט - וידוא לפני חזרה למשחקים שלי
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "backMyGamesModal();", true);
            return;
        }
        // אם לא התבצעו שינויים - מעבירה בחזרה למשחקים שלי
        Response.Redirect("myGames.aspx");

    }


    // בדיקה האם קיים תוכן חדש ולא שמור בעריכת הגדרות משחק
    protected bool WereChangesMade()
    {
        // משיכת הטקסט של שם המשחק השמור בעץ 
        string gameNameXML = Server.UrlDecode(gameNode.SelectSingleNode("gameName").InnerXml);

        // אם שם המשחק ששמור בעץ שונה משם המשחק שנמצא בתיבת הטקסט בממשק
        if (gameNameXML != gameNameTxtBox.Text)
        {
            return true;
        }

        // משיכת הזמן לשאלה ששמור בעץ
        string timePerQuestXML = gameNode.Attributes["timePerQuest"].InnerText;

        // אם הזמן לשאלה ששמור בעץ שונה מהזמן לשאלה שנמצא בממשק
        if (timePerQuestXML != timePerQuestRB.SelectedValue)
        {
            return true;
        }

        // לא קיים תוכן חדש ולא שמור
        return false;
    }

}