using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // בדיקה אם המשתמשת מחוברת ובהתאם לשלוח אותה לדף טבלת המשחקים 
        if (Session["user"] != null)
        {
           Response.Redirect("myGames.aspx");
        }

        // בדיקה האם לאפשר כפתור כניסה או לא
        loginBtn.Enabled = usernameTB.Text != "" && passwordTB.Text != "";

        // אם זה אחרי פוסטבק - ממשיכות
        if (!IsPostBack)
        {
            return;
        }

        passwordTB.Attributes["value"] = passwordTB.Text;
    }

    // בלחיצה על כניסה לעורך
    protected void loginBtn_Click(object sender, EventArgs e)
    {
        // אם הפרטים נכונים
        if (usernameTB.Text == "admin" && passwordTB.Text == "telem") //בדיקה אם שם המשתמשת והסיסמה תקינים
        {
            // שמירת הסשן של המשתמשת
            Session["user"] = "admin";
            // מעבר לעמוד כל המשחקים
            Response.Redirect("myGames.aspx");
        }
        // אם הסיסמה לא נכונה
        else if (usernameTB.Text == "admin")
        {
            incorrectLbl.Text = "הסיסמה שהוזנה שגויה";
            incorrectLbl.Style.Add("visibility", "visible");
            // שינוי צבע בורדר באמצעות הפונקציה שנמצאת בג'אווה סקריפט
            ScriptManager.RegisterStartupScript(this, this.GetType(), "border", "changeWrongTBColor(false, true);", true);

        }
        // אם היוזר לא נכון
        else if (passwordTB.Text == "telem")
        {
            incorrectLbl.Text = "שם המשתמש/ת לא קיים במערכת";
            incorrectLbl.Style.Add("visibility", "visible");
            // שינוי צבע בורדר באמצעות הפונקציה שנמצאת בג'אווה סקריפט
            ScriptManager.RegisterStartupScript(this, this.GetType(), "border", "changeWrongTBColor(true, false);", true);
        }
        // אם הכל לא נכון
        else
        {
            incorrectLbl.Text = "שם המשתמש/ת והסיסמה שהוזנו שגויים";
            incorrectLbl.Style.Add("visibility", "visible");
            // שינוי צבע בורדר באמצעות הפונקציה שנמצאת בג'אווה סקריפט
            ScriptManager.RegisterStartupScript(this, this.GetType(), "border", "changeWrongTBColor(true, true);", true);
        }
    }
}