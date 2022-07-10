using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class myGames : System.Web.UI.Page
{
    // שמירת העץ במשתנה גלובלי
    XmlDocument xmlAllGames;

    // שיטה שמתבצעת פעם אחת בטעינת העמוד 
    protected void Page_init(object sender, EventArgs e)
    {
        // בדיקה אם המשתמשת לא מחוברת - לשלוח אותה לעמוד הכניסה
        if (Session["user"] == null)
        {
            Response.Redirect("sessionTimeout.aspx");
        }

        // טעינה של העץ למשתנה 
        xmlAllGames = GamesXmlDataSource.GetXmlDocument();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // בודקת מה צריך להיות מוצג באזור של טבלת המשחקים       
        // הצגת טבלה ריקה אם אין משחקים בכלל
        zeroGamesTable.Visible = xmlAllGames.SelectNodes("//game").Count == 0;

        // מחיקת תמונות שלא נמצאות בשימוש באף אחד מהמשחקים
        // ניקוי התיקייה בשרת
        DeleteUnusedImageFiles();
    }

    // שיטה ליצירת משחק חדש
    protected void AddGame_Click(object sender, ImageClickEventArgs e)
    {
        // במידה וזה המשחק הראשון שאנו יוצרות, קוד המשחק יהיה 101
        int MaxGameCode = 101;

        // במידה וכבר קיימים משחקים
        // נבצע בדיקה האם אכן קיימים משחקים בעץ הכללי
        if (xmlAllGames.SelectNodes("//game/@gameCode").Count != 0)
        {
            // מציאת קוד המשחק הגבוה ביותר שקיים 
            // שימוש בתנאי שבודק איזה קוד משחק הכי גדול
            // ושמירת הערך של קוד המשחק הזה       
            MaxGameCode = Convert.ToInt16(xmlAllGames.SelectSingleNode("//game[not(@gameCode < //game/@gameCode)]/@gameCode").Value);
            // העלאת ערך קוד המשחק ב-1
            MaxGameCode++;
        }

        // שמירת האיידי של המשחק החדש בסשן
        Session["gameIDSession"] = MaxGameCode;

        // יצירת ענף של משחק חדש    
        XmlElement myNewGameNode = xmlAllGames.CreateElement("game");        myNewGameNode.SetAttribute("gameCode", MaxGameCode.ToString());        myNewGameNode.SetAttribute("timePerQuest", "30");
        myNewGameNode.SetAttribute("isPublish", "false");

        // יצירת ענף של שם המשחק
        XmlElement myNewNameNode = xmlAllGames.CreateElement("gameName");        myNewNameNode.InnerXml = Server.UrlEncode(addNewNameTB.Text); // קידוד - כאשר אנחנו מוסיפים ערכים טקסטואלים לעץ
        myNewGameNode.AppendChild(myNewNameNode);

        // יצירת ענף שיכיל את כל השאלות במשחק
        XmlElement gameQuestionsNode = xmlAllGames.CreateElement("gameQuestions");        myNewGameNode.AppendChild(gameQuestionsNode);

        // הוספת ענף המשחק החדש לעץ כמשחק הראשון
        XmlNode FirstGame = xmlAllGames.SelectNodes("/LaCasaDeCookie/game").Item(0); // מוצאת מי נמצא ראשון - הענף הראשון 
        xmlAllGames.SelectSingleNode("/LaCasaDeCookie").InsertBefore(myNewGameNode, FirstGame);        GamesXmlDataSource.Save(); // שמירת השינויים 
        myGamesGridView.DataBind();

        // מעבירות לעמוד הגדרות משחק
        Response.Redirect("settings.aspx");
    }

    // שיטה שמתבצעת בעת לחיצה על אחד הכפתורים (עריכה או מחיקה) בתוך טבלת המשחקים
    protected void myGamesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // תחילה אנו מבררים מהו האיידי של המשחק בעץ       
        // e = כל הארגומנטים של הגריד וויו
        // e.CommandSource = הכפתור שנלחץ 
        // שמירת האוביקט של הכפתור שנלחץ
        ImageButton imgBtnClicked = (ImageButton)e.CommandSource;
        // אנו מושכים את האיידי של המשחק באמצעות מאפיין שלא שמור בעץ, אלא שהוספנו באופן ידני לכפתור-תמונה
        string gameId = imgBtnClicked.Attributes["theGameId"];
        // שמירה בסשן בגלל מעבר בין דפים
        Session["gameIDSession"] = gameId;

        // עלינו לברר איזו פקודה צריכה להתבצע - הפקודה רשומה בכל כפתור             
        switch (e.CommandName)
        {
            // אם נלחץ על כפתור עריכת הגדרות כלליות נעבור לדף עריכת הגדרות כלליות                
            case "editSettings":
                Response.Redirect("settings.aspx");
                break;

            // אם נלחץ על כפתור עריכת שאלות נעבור לדף עריכת שאלות                      
            case "editQuestions":
                Response.Redirect("edit.aspx");
                break;

            //אם נלחץ על כפתור מחיקה יקרא לפונקציה של מחיקה                    
            case "deleteGame":
                // הדפסת שם המשחק ללייבל המתאים בפופאפ
                modalGameToDeleteLbl.Text = Server.UrlDecode(xmlAllGames.SelectSingleNode("//game[@gameCode='" + gameId + "']/gameName").InnerXml);

                // הדפסת מספר השאלות שיש במשחק
                modalQuestCounterLbl.Text = xmlAllGames.SelectNodes("//game[@gameCode='" + gameId + "']/gameQuestions/question").Count.ToString();

                // פתיחת המודל באמצעות הפונקציה שנמצאת בג'אווה סקריפט
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "warningModal();", true);

                break;

            default:
                return;
        }
    }

    // מחיקת משחק
    protected void DeleteGame(object sender, EventArgs e)
    {
        // בדיקת הגנה במקרה של קליר קאש
        if (Session["gameIDSession"] == null)
        {
            return;
        }
        // הסרת ענף של משחק קיים באמצעות זיהוי האיי דיי שניתן לו על ידי לחיצה עליו מתוך הטבלה
        // שמירה ועדכון לתוך העץ ולגריד ויו
        string gameIdToDelete = Session["gameIDSession"].ToString();

        // מציאת הענף של המשחק שצריך למחוק
        XmlNode gameNodeToDelete = xmlAllGames.SelectSingleNode("//game[@gameCode='" + gameIdToDelete + "']");
        // מחיקת הענף מהעץ
        gameNodeToDelete.ParentNode.RemoveChild(gameNodeToDelete);

        // שמירת שינויים
        GamesXmlDataSource.Save();
        myGamesGridView.DataBind();

        // איפוס האיידי שבסשן בגלל שהמשחק לא קיים יותר
        Session["gameIDSession"] = null;

        // הגנה במקרה של קליר קאש
        Response.Redirect("myGames.aspx");
    }



    // שיטה שמתבצעת בעת לחיצה על צ'קבוקס פרסום
    protected void IsPublishCB_CheckedChanged(object sender, EventArgs e)
    {
        // תחילה אנו מבררים מהו ה -אי די- של המשחק
        ImageButton publishCB = (ImageButton)sender;

        // מושכים את האיידי של המשחק באמצעות המאפיין
        string gameId = publishCB.Attributes["theGameId"];

        // שאילתא למציאת המשחק שברצוננו לעדכן
        XmlNode theGame = xmlAllGames.SelectSingleNode("//game[@gameCode=" + gameId + "]");

        // שמות את הערך ההפוך ממה שיש בעץ
        theGame.Attributes["isPublish"].InnerText = (theGame.Attributes["isPublish"].InnerText != "true").ToString().ToLower();

        //שמירה בעץ והצגה
        GamesXmlDataSource.Save();
        myGamesGridView.DataBind();

        // איפוס האיידי שבסשן אחרי שעדכנו את הפרסום שלו
        Session["gameIDSession"] = null;

        // הגנה במקרה של קליר קאש
        Response.Redirect("myGames.aspx");
    }


    // מתרחשת בכל יצירת שורה בטבלת המשחקים שלי
    // בשביל הטולטיפ של פרסום משחק שבתוך הטבלה
    // וגם בשביל שינוי צבע של מספר השאלות בטבלת המשחקים
    protected void myGamesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // בכל יצירת שורה
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // הפעלת הטולטיפ של פרסום משחק
            ImageButton publishBtn = (ImageButton)e.Row.FindControl("isPublishFalse");
            string panelParentTooltip = publishBtn.Parent.ClientID;

            string enableOrDisable = publishBtn.Enabled ? "disable" : "enable";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Tooltip" + e.Row.RowIndex, "openPublishCBTooltip(" + panelParentTooltip + ", '" + enableOrDisable + "');", true);


            // שינוי צבע של מספר השאלות
            // אינדיקציה שמראה האם אפשר לפרסם את המשחק
            Label numberOfQuestionsLabel = (Label)e.Row.FindControl("numberOfQuestions");

            int numberOfQuestionsInt = Convert.ToInt32(numberOfQuestionsLabel.Text);

            // אם מספר השאלות קטן מ-10
            // שינוי צבע לאדום
            if (numberOfQuestionsInt < 10)
            {
                numberOfQuestionsLabel.Style.Add("color", "red");
            }
            // אם יש יותר מ-10 שאלות במשחק
            // חזרה לצבע המקורי
            else
            {
                numberOfQuestionsLabel.Style.Add("color", "#495867");
            }
        }
    }



    // מחיקת תמונות מהשרת שלא נמצאות בשימוש
    protected void DeleteUnusedImageFiles()
    {
        // רשימה של כל התמונות בעץ
        List<string> allImageFileNamesInXML = new List<string>();

        // רשימה של כל התמונות שנמצאות בגזע השאלה בלבד בעץ 
        List<string> allGezaImagesList = xmlAllGames.SelectNodes("//question/img").Cast<XmlNode>().Select(node => node.InnerXml).ToList();

        // רשימה של כל התמונות שנמצאות במסיחים בעץ 
        List<string> allAnswerImagesList = xmlAllGames.SelectNodes("//answer[@AnsType='image']").Cast<XmlNode>().Select(node => node.InnerXml).ToList();

        // הוספה לרשימה של כל התמונות
        allImageFileNamesInXML.AddRange(allGezaImagesList);
        allImageFileNamesInXML.AddRange(allAnswerImagesList);

        // מערך של הנתיבים של כל התמונות ששמורות בתיקייה שבשרת
        string[] allUplodedImageFilePathInServer = System.IO.Directory.GetFiles(Server.MapPath("~/uploadedFiles/"));

        // לולאה שעוברת על כל התמונות ששמורות בעץ
        foreach (string imageFilePathInServer in allUplodedImageFilePathInServer)
        {
            // בודקת האם הנתיב של התמונה ששמורה בשרת מוכל ברשימה של כל התמונות בעץ
            if (!allImageFileNamesInXML.Exists(fileName => Server.MapPath("~/uploadedFiles/" + fileName) == imageFilePathInServer))
            {
                // מחיקת תמונות שלא מוכלות ברשימה שבעץ
                // כלומר שלא משתמשות בהם באף משחק
                System.IO.File.Delete(imageFilePathInServer);
            }
        }
    }


}
