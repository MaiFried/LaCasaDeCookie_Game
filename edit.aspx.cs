using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing;


public partial class edit : System.Web.UI.Page
{
    // משתנה גלבולי לשמירת עץ ה-XML 
    XmlDocument LaCasaDeCookieXML = new XmlDocument();

    // שמירת קיצור לענף של המשחק הנבחר
    XmlNode gameNode;

    // שמירת קיצור לענף של כל השאלות במשחק
    XmlNode gameQuestions;


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

        // בשביל הגרידוויו של השאלות
        XmlGameSource.XPath = "//game[@gameCode=" + gameID + "]/gameQuestions/question";

        // טעינת העץ לתוך המשתנה הגלובלי
        LaCasaDeCookieXML.Load(Server.MapPath("XML/LaCasaDeCookie.xml"));

        // מציאת הענף שמכיל את המשחק ושמירה במשתנה הגלובלי
        gameNode = LaCasaDeCookieXML.SelectSingleNode("//game[@gameCode=" + gameID + "]");

        // מציאת הענף שמכיל את כל השאלות במשחק ושמירה במשתנה הגלובלי
        gameQuestions = gameNode.SelectSingleNode("gameQuestions");

        // הדפסת שם המשחק ללייבל
        gameNameLbl.Text = Server.UrlDecode(gameNode.SelectSingleNode("gameName").InnerXml);
    }


    // שיטה שמתבצעת בכל טעינת עמוד
    protected void Page_Load(object sender, EventArgs e)
    { 
        // סימון תשובה נכונה 
        ChooseAnswerMakeGreen(chosenRightAnswer.Value);

        // בודקת מה צריך להיות מוצג באזור של טבלת השאלות 
        UpdateGridviewArea();

        // אם זה אחרי פוסטבק - ממשיכות
        if (!IsPostBack)
        {
            return;
        }

        this.MaintainScrollPositionOnPostBack = true;

        // קריאה לשיטה שבודקת אם הועלתה תמונה ומציגה אותה
        // אם התמונה בגזע השאלה
        ShowImageInImageBtn();
        // אם התמונה בתשובות
        for (int i = 1; i <= 6; i++)
        {
            ShowImageInImageBtn(i);
        }

        // בדיקה האם קיימת תמונה בכפתור להעלאת תמונה, ועדכון קלאס בהתאם
        CheckIfImageExistsAndToggleClass();
    }


    // שיטה שמתבצעת בלחיצה על כפתור שמירת שאלה
    protected void SubmitQuestionBtn_Click(object sender, EventArgs e)
    {
        // בדיקת הגנה
        // האם המשתמשת עומדת בתנאים לשמירת שאלה
        if (submitConditionsHiddenField.Value != visiblePanelsHiddenField.Value)
        {
            // אם לא - מנטרלת את הכפתור לשמירת שאלה
            saveQuestBtn.Enabled = false;
            return;
        }

        // בדיקה אם המשתמשת מחוברת ובהתאם לשלוח אותה לעמוד המתאים
        if (Session["user"] == null)
        {
            Response.Redirect("sessionTimeout.aspx");
        }

        // האם מדובר בעריכת שאלה קיימת
        if (questionIdFromBankToEdit.Value != "edit")
        {
            SaveChangesToExistQuestion();
        }
        // או הוספת שאלה חדשה למאגר השאלות
        else
        {
            AddNewQuestion();
        }
    }



    // שמירת שינויים בשאלה קיימת
    protected void SaveChangesToExistQuestion()
    {
        // מציאת הענף של השאלה שצריך לאפס לפני
        XmlNode questionNodeToUpdate = gameQuestions.SelectSingleNode("question[@id='" + questionIdFromBankToEdit.Value + "']");

        XmlNodeList xmlNodeList = questionNodeToUpdate.Clone().ChildNodes;

        // איפוס כל הענף של השאלה
        questionNodeToUpdate.RemoveAll();

        // 
        XmlElement questionXMLElement = (XmlElement)questionNodeToUpdate;

        // לשאלה ID הכנסת מאפיין
        questionXMLElement.SetAttribute("id", questionIdFromBankToEdit.Value);

        // איפוס האיידי של השאלה שרוצות לעדכן לערך ההתחלתי 
        questionIdFromBankToEdit.Value = "edit";

        // קריאה לשיטה שמעדכנת את התוכן של השאלה 
        UpdateQuestionContentToXMLTree(questionXMLElement, isNewQuestion: false);
    }


    // הוספת שאלה חדשה למאגר השאלות
    protected void AddNewQuestion()
    {
        // יצירת אלמנט שיכיל את השאלה 
        XmlElement newQuestion_XmlElement = LaCasaDeCookieXML.CreateElement("question");

        // במידה וזאת השאלה הראשונה במשחק, איידי השאלה יהיה 1
        int MaxQuestID = 1;

        // במידה וכבר קיימות שאלות
        if (gameQuestions.SelectNodes("question").Count != 0)
        {
            // מציאת האיידי הגבוה ביותר שקיים 
            // שימוש בלמבדה אקספרשן
            // Select = מיפוי לרשימה של מספרים
            // מבצעת מקס לרשימת המספרים שקיבלתי - מקבלת את האיידי הכי גבוה מהרשימה של המספרים
            MaxQuestID = gameQuestions.SelectNodes("question").Cast<XmlElement>().Select(node => Convert.ToInt32(node.GetAttribute("id"))).Max();
            // העלאת איידי השאלה ב-1
            MaxQuestID++;
        }

        // לשאלה ID הכנסת מאפיין
        newQuestion_XmlElement.SetAttribute("id", MaxQuestID.ToString());

        // קריאה לשיטה שמעדכנת את התוכן של השאלה 
        // ושומרת את השינויים בעץ
        UpdateQuestionContentToXMLTree(newQuestion_XmlElement, isNewQuestion: true);
    }



    // עדכון תוכן של שאלה בעץ
    protected void UpdateQuestionContentToXMLTree(XmlElement questionXMLElement, bool isNewQuestion)
    {
        // יצירת אלמנט חדש שיכיל את הטקסט של השאלה
        XmlElement newQuestionText_XmlElement = LaCasaDeCookieXML.CreateElement("questionText");
        // הכנסת הטקסט מהממשק לתוך משתנה מחרוזתי
        string newQuestionText = ((TextBox)FindControl("questionTxtBox")).Text;
        // הכנסת הטקסט בתוך האלמנט של טקסט השאלה
        XmlText newQuestionText_XmlText = LaCasaDeCookieXML.CreateTextNode(Server.UrlEncode(newQuestionText));

        // הוספה לעץ - טקסט שאלה חדשה
        newQuestionText_XmlElement.AppendChild(newQuestionText_XmlText); // הוספת הטקסט של השאלה (הטקסט) לתוך אלמנט טקסט השאלה
        questionXMLElement.AppendChild(newQuestionText_XmlElement); // הוספת אלמנט טקסט השאלה לתוך אלמנט השאלה

        // אם הועלתה תמונה בגזע השאלה
        if (uploadImgInQuestionBtn.ImageUrl != "~/images/add_photo.png")
        {
            // פיצול נתיב התמונה למערך
            //string[] imageForQuestionArray = uploadImgInQuestionBtn.ImageUrl.Split('/');
            string[] imageForQuestionArray = ImageHolder0.ImageUrl.Split('/');

            // שמירת התא האחרון - שם התמונה - במערך לתוך משתנה מחרוזתי
            string imageForQuestion = imageForQuestionArray[imageForQuestionArray.Length - 1];

            // יצירת אלמנט חדש שיכיל את השם של התמונה
            XmlElement newQuestionImage_XmlElement = LaCasaDeCookieXML.CreateElement("img");
            // הכנסת שם התמונה בתוך האלמנט של התמונה
            XmlText newQuestionImage_XmlText = LaCasaDeCookieXML.CreateTextNode(imageForQuestion);

            // הוספה לעץ - תמונת בגזע השאלה
            // הוספת אלמנט הטקסט של התמונה לתוך אלמנט התמונה
            newQuestionImage_XmlElement.AppendChild(newQuestionImage_XmlText);
            // הוספת אלמנט התמונה השאלה לתוך אלמנט השאלה
            questionXMLElement.AppendChild(newQuestionImage_XmlElement);
        }

        // יצירת אלמנט חדש שיכיל את כל התשובות האפשריות של השאלה
        XmlElement newAnswers_XmlElement = LaCasaDeCookieXML.CreateElement("answers");

        // לולאה להכנסת טקסט או תמונה לאלמנט התשובות בעץ
        for (int i = 1; i <= 6; i++)
        {
            // אם הפאנל של התשובה מוסתר
            // מדלגת את האיטרציה הזאת ועוברת לאיטרציה הבאה
            if (!((Panel)FindControl("answerPanel" + i)).Visible)
            {
                continue;
            }

            // שמירת משתנה מחרוזתי של התשובה
            string newAnswerText;
            bool isImage;

            // בדיקה האם קיימת תמונה בתור תשובה
            if (((ImageButton)FindControl("ImageAnswerBtn" + i)).ImageUrl != "~/images/add_photo.png")
            {
                isImage = true;
                // פיצול נתיב התמונה למערך
                //string[] imageForAnswerArray = ((ImageButton)FindControl("ImageAnswerBtn" + i)).ImageUrl.Split('/');
                string[] imageForAnswerArray = ((System.Web.UI.WebControls.Image)FindControl("ImageHolder" + i)).ImageUrl.Split('/');

                // שמירת התא האחרון - שם התמונה - במערך לתוך משתנה מחרוזתי
                newAnswerText = imageForAnswerArray[imageForAnswerArray.Length - 1];
            }
            // אם אין תמונה, נשמור את הטקסט שבתוך תיבת הטקסט
            else
            {
                isImage = false;
                // שמירת טקסט התשובה
                newAnswerText = ((TextBox)FindControl("TxtBoxAnswer" + i)).Text;
            }

            // בדיקה האם התשובה נכונה או לא
            bool isCorrect = chosenRightAnswer.Value == i.ToString();

            // קריאה לשיטה ליצירת אלמנט חדש של תשובה אפשרית
            XmlElement newAnswer_XmlElement = CreateAnswer(newAnswerText, isCorrect, isImage);
            // הוספה לעץ - תשובה חדשה
            newAnswers_XmlElement.AppendChild(newAnswer_XmlElement); // הוספת אלמנט התשובה לתוך אלמנט כל התשובות האפשריות
        }

        // הוספת אלמנט כל התשובות האפשריות לתוך אלמנט השאלה
        questionXMLElement.AppendChild(newAnswers_XmlElement);

        // אם מדובר בהוספת שאלה חדשה
        if (isNewQuestion)
        {
            // הוספת אלמנט השאלה לתוך אלמנט כל השאלות
            gameQuestions.AppendChild(questionXMLElement);
        }

        // שמירת השינויים בעץ
        LaCasaDeCookieXML.Save(Server.MapPath("XML/LaCasaDeCookie.xml"));

        //שמירה בעץ והצגה בגרידוויו
        XmlGameSource.Save();
        questionsGridView.DataBind();

        // איפוס אזור הזנת תוכן
        // בשביל להיות בפוקוס על הסשן                  
        Response.Redirect("edit.aspx");
    }


    // שיטה ליצירת תשובה אפשרית לשאלה
    protected XmlElement CreateAnswer(string ansText, bool isCorrect, bool isImage)
    {
        // יצירת אלמנט חדש של תשובה אפשרית לשאלה
        XmlElement newAnswer_XmlElement = LaCasaDeCookieXML.CreateElement("answer");
        // יצירת טקסט שיוכל בתוך האלמנט שיצרנו
        XmlText newAnswer_XmlText = LaCasaDeCookieXML.CreateTextNode(Server.UrlEncode(ansText));

        // הכנסת מאפיינים לאלמנט החדש - הענף שיצרנו 
        newAnswer_XmlElement.SetAttribute("isCorrect", isCorrect.ToString().ToLower()); // הכנסת מאפיין האם נכון או לא נכון
        newAnswer_XmlElement.SetAttribute("AnsType", isImage ? "image" : "text"); // הכנסת מאפיין סוג התשובה - תמונה / טקסט

        // הוספה לעץ - תשובות
        newAnswer_XmlElement.AppendChild(newAnswer_XmlText); // הוספת הטקסט של התשובה (הטקסט) לתוך אלמנט התשובה

        // מחזירה אלמנט של התשובה החדשה
        return newAnswer_XmlElement;
    }


    // שיטה להוספת תמונה לתיקייה
    protected string AddPhotoToLibrary(FileUpload fileUploadImg)
    {
        //איתור שם הקובץ
        string fileName = fileUploadImg.PostedFile.FileName;
        //איתור סיומת הקובץ
        string endOfFileName = fileName.Substring(fileName.LastIndexOf("."));
        //איתור זמן העלת הקובץ
        string myTime = DateTime.Now.ToString("dd_MM_yy_HH_mm_ss_ffff");
        //הגדרת שם חדש לקובץ
        string imageNewName = myTime + endOfFileName;

        // Bitmap המרת הקובץ שיתקבל למשתנה מסוג
        System.Drawing.Bitmap bmpPostedImage = new System.Drawing.Bitmap(fileUploadImg.PostedFile.InputStream);

        //קריאה לפונקציה המקטינה את התמונה
        //אנו שולחים לה את התמונה שלנו בגירסאת הביטמאפ ואת האורך והרוחב שאנו רוצות לתמונה החדשה
        System.Drawing.Image objImage = ResizeImage(bmpPostedImage, 1300, 800);

        //שמירת הקובץ בגודלו החדש בתיקייה
        objImage.Save(Server.MapPath("uploadedFiles/") + imageNewName);
        // החזרת שם הקובץ החדש שנשמר
        return imageNewName;
    }


    /// <summary>
    /// Resize the image to the specified width and height.
    /// </summary>
    /// <param name="image">The image to resize.</param>
    /// <param name="maxPixelsSize"></param>
    /// <returns>The resized image.</returns>
    protected System.Drawing.Image ResizeImage(System.Drawing.Image image, int maxWidth, int maxHeight)
    {
        int sourceWidth = Convert.ToInt32(image.Width);
        int sourceHeight = Convert.ToInt32(image.Height);

        double nPercent = 0;
        double nPercentW = 0;
        double nPercentH = 0;

        nPercentW = ((double)maxWidth / sourceWidth);
        nPercentH = ((double)maxHeight / sourceHeight);

        nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

        // כדי שלא נגדיל את התמונה
        if (nPercent > 1)
        {
            nPercent = 1;
        }

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);


        var destRect = new Rectangle(0, 0, destWidth, destHeight);
        var destImage = new Bitmap(destWidth, destHeight);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }


    // בדיקה האם לאפשר/לנטרל כפתורים 
    // שמירת שאלה // ביטול שינויים //
    protected void SetCancelBtnEnableOrDisable()
    {
        // כפתור שמירת שאלה //
        // אם התנאים חופפים - נאפשר את הכפתור לשמירת שאלה
        if (submitConditionsHiddenField.Value == visiblePanelsHiddenField.Value)
        {
            saveQuestBtn.Enabled = true;
        }
        // אם לא - מנטרלת את הכפתור
        else
        {
            saveQuestBtn.Enabled = false;
        }

        // כפתור ביטול שינויים //
        // בדיקה האם לאפשר כפתור לביטול שינויים
        if (uploadImgInQuestionBtn.ImageUrl != "~/images/add_photo.png" || submitConditionsHiddenField.Value != "0" ||
            questionIdFromBankToEdit.Value != "edit")
        {
            // לאפשר ביטול שינויים
            cancelChangesBTN.Enabled = true;
        }
        else
        {
            // לנטרל ביטול שינויים
            cancelChangesBTN.Enabled = false;
        }
    }


    // בדיקה האם קיימת תמונה בכפתור להעלאת תמונה, ועדכון קלאס בהתאם
    protected void CheckIfImageExistsAndToggleClass()
    {
        ImageButton imageBtnUpload;
        //string imageBtnUploadID;

        Panel showUploadImgBtnDiv;

        // מעבר על כל התמונות בממשק
        for (int i = 0; i <= 6; i++)
        {
            // אם התמונה בגזע השאלה
            if (i == 0)
            {
                imageBtnUpload = uploadImgInQuestionBtn;
                //imageBtnUploadID = imageBtnUpload.ClientID;

                // פאנל להחזקת התמונ
                showUploadImgBtnDiv = showUploadImgBtnDiv0;
            }
            // אם התמונה בתשובות
            else
            {
                imageBtnUpload = (ImageButton)FindControl("ImageAnswerBtn" + i);
                //imageBtnUploadID = imageBtnUpload.ClientID;

                // פאנל להחזקת התמונה
                showUploadImgBtnDiv = (Panel)FindControl("showUploadImgBtnDiv" + i);
            }


            // בדיקה האם קיימת תמונה
            // אם כן מוסיפה קלאס שמציין שיש תמונה בכפתור להעלאת תמונה
            if (imageBtnUpload.ImageUrl != "~/images/add_photo.png")
            {
                if (!imageBtnUpload.CssClass.Contains("yesImg"))
                {
                    imageBtnUpload.CssClass += imageBtnUpload.CssClass == "" ? "yesImg" : " yesImg";
                }
            }
            // אם לא מורידה קלאס
            else
            {
                if (imageBtnUpload.CssClass.Contains(" yesImg"))
                {
                    imageBtnUpload.CssClass = imageBtnUpload.CssClass.Replace(" yesImg", "");
                }
                else if (imageBtnUpload.CssClass.Contains("yesImg"))
                {
                    imageBtnUpload.CssClass = imageBtnUpload.CssClass.Replace("yesImg", "");
                }
            }
        }
    }


    // הצגת תמונה במממשק - בכפתור להעלאת תמונה
    protected void ShowImageInImageBtn(int ansIndex = 0)
    {
        // שמירת קיצורים
        FileUpload fileUpload;
        ImageButton imageBtnUpload;
        ImageButton imageBtnZoom;
        ImageButton imageBtnDelete;
        Label warningLbl;
        TextBox txtBox;

        System.Web.UI.WebControls.Image ImageHolder = (System.Web.UI.WebControls.Image)FindControl("ImageHolder" + ansIndex);

        // אם התמונה בשאלה - שמירת המשתנים בהתאם
        if (ansIndex == 0)
        {
            fileUpload = FileUploadImgQuest;
            imageBtnUpload = uploadImgInQuestionBtn;
            imageBtnZoom = zoomImgQuest;
            imageBtnDelete = deleteImgQuestBtn;
            warningLbl = warningLBLQuestion;
            txtBox = questionTxtBox;
        }
        // אם התמונה בתשובות - שמירת המשתנים בהתאם
        else
        {
            fileUpload = (FileUpload)FindControl("FileUploadImgAnswer" + ansIndex);
            imageBtnUpload = (ImageButton)FindControl("ImageAnswerBtn" + ansIndex);
            imageBtnZoom = (ImageButton)FindControl("zoomImgAnsBtn" + ansIndex);
            imageBtnDelete = (ImageButton)FindControl("deleteImgAnsBtn" + ansIndex);
            warningLbl = (Label)FindControl("warningLBLAns" + ansIndex);
            txtBox = (TextBox)FindControl("TxtBoxAnswer" + ansIndex);
        }

        // איפוס לייבל אזהרה
        warningLbl.Text = "";

        // אם הפיילאפלווד לא מכיל קובץ
        if (!fileUpload.HasFile)
        {
            return;
        }

        // שמירת התמונה שקיימת עכשיו בממשק
        //string previousImagUrl = imageBtnUpload.ImageUrl;


        // שמירת סוג הקובץ שהועלה במשתנה מחרוזתי
        string endOfFileName = fileUpload.PostedFile.FileName.Substring(fileUpload.PostedFile.FileName.LastIndexOf(".")).ToLower();

        string[] imageTypesArray = { "png", "jpg", "jpeg", "gif", "bmp", "tiff" };

        // בדיקה האם הקובץ הוא מסוג תמונה מאחד התאים שבמערך למעלה
        bool fileUploadContainsImageType = imageTypesArray.Any(endOfFileName.Contains);

        // קריאה לפונקציה שמעדכנת את הפאנל של התשובה - בתהאם ליש/אין תמונה
        UpdateInputPanelDisplay(ansIndex, fileUploadContainsImageType);

        // מחיקת תמונה מהשרת - אם הייתה תמונה קודמת
        if (ImageHolder.Visible)
        {
            DeleteFileFromServer(ImageHolder.ImageUrl);
            ImageHolder.ImageUrl = "";
            ImageHolder.Visible = false;
        }

        // אם הקובץ לא מסוג תמונה
        if (!fileUploadContainsImageType)
        {
            warningLbl.Text = "הקובץ שנבחר אינו מסוג תמונה";

            // איפוס הכפתור שיציג אייקון של הוספת תמונה
            imageBtnUpload.ImageUrl = "~/images/add_photo.png";

        }
        // אם הקובץ מכיל תמונה
        else
        {
            // שינוי אייקון להחלפת תמונה
            imageBtnUpload.ImageUrl = "~/images/exchange.png";
            // שמירת התמונה החדשה בספרייה והצגת התמונה בממשק
            ImageHolder.ImageUrl = "~/uploadedFiles/" + AddPhotoToLibrary(fileUpload);
            ImageHolder.Visible = true;

            // בדיקה האם לאפשר/לנטרל כפתור ביטול שינויים
            SetCancelBtnEnableOrDisable();

            // בדיקה האם המשתמשת עדיין עומדת בתנאים לשמירת שאלה
            SetSubmitBtnEnableOrDisable();

            //  בדיקה האם להראות כפתורים לאיפוס תשובה ב-2 התשובות הראשונות
            SetClearButtonsVisibility();
        }

        //// מחיקת תמונה מהשרת - אם הייתה תמונה קודמת
        //if (previousImagUrl != "~/images/add_photo.png")
        //{
        //    DeleteFileFromServer(previousImagUrl);
        //}     
    }


    // מעדכנת פאנל מלא של הזנת תוכן - אם יש / אין תמונה
    // מראה / מסתירה אייקונים למחיקה והגדלת תמונה
    // מנטרלת / מאפשרת אזור הזנת טקסט - רק אם בתשובות
    protected void UpdateInputPanelDisplay(int ansIndex, bool isImage, bool isCallSetSubmitBtnEnableOrDisable = true)
    {
        // שמירת קיצורים
        ImageButton imageBtnZoom;
        ImageButton imageBtnDelete;
        // שמירת קיצור לתיבת טקסט של תשובה
        TextBox answerTB = (TextBox)FindControl("TxtBoxAnswer" + ansIndex);

        // אם מדובר בתשובות
        if (ansIndex != 0)
        {
            imageBtnZoom = (ImageButton)FindControl("zoomImgAnsBtn" + ansIndex);
            imageBtnDelete = (ImageButton)FindControl("deleteImgAnsBtn" + ansIndex);

            // קריאה לפונקציה שמעדכנת נראות של תיבת טקסט בתשובות
            UpdateTextInputPanelInAnswer(answerTB, isImage);

            // עדכון ביטפילד
            // בתהאם ליש/אין תמונה
            // עדכון התנאים מצד המשתמשת - התנאים שהיא עומדת בהם עכשיו
            SetBitInBitfield(submitConditionsHiddenField, ansIndex, setBitOn: isImage);

            //// בדיקה האם המשתמשת עומדת בתנאים לשמירת שאלה
            //if (isCallSetSubmitBtnEnableOrDisable)
            //{
            //    SetSubmitBtnEnableOrDisable();
            //}

        }
        // אם בשאלה
        else
        {
            imageBtnZoom = zoomImgQuest;
            imageBtnDelete = deleteImgQuestBtn;
        }

        // הסתרת/הצגת אייקןן זום להגדלת תמונה
        imageBtnZoom.Style.Add("visibility", isImage ? "visible" : "hidden");
        // הסתרת/הצגת אייקון למחיקת תמונה
        imageBtnDelete.Style.Add("visibility", isImage ? "visible" : "hidden");
    }

    // עדכון אזור הזנת הטקסט לאחר הכנסת/מחיקת תמונה בתשובות
    protected void UpdateTextInputPanelInAnswer(TextBox ansTxtBox, bool disableTextPanel)
    {
        // בדיקה איזה תשובה צריכה להתעדכן
        string index = ansTxtBox.ID.Last().ToString();
        Label labelLimitAnswer = (Label)FindControl("labelLimitAnswer" + index);
        // הסתרת האלמנטים באזור הזנת הטקסט
        ansTxtBox.Visible = !disableTextPanel;
        labelLimitAnswer.Visible = !disableTextPanel;
        // שינוי צבע רקע של אזור הזנת טקסט
        string color = disableTextPanel ? "#e9e9e9" : "#FFFFFF";
        Panel txtBoxParent = (Panel)ansTxtBox.Parent;
        txtBoxParent.Style.Add("background-color", color);
        txtBoxParent.Style.Add("border-color", color);
        // שינוי סמן עכבר
        string cursorSign = disableTextPanel ? "not-allowed" : "text";
        txtBoxParent.Style.Add("cursor", cursorSign);
    }


    // מחיקת תמונה - בגזע השאלה או בתשובות
    protected void DeleteImgBtn_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imgDltBtn = (ImageButton)sender;
        // שמירת קיצורים        
        ImageButton imgUploadBtn; // הכפתור להעלאת תמונה שמקושר לכפתור למחיקת תמונה
        ImageButton imageBtnZoom; // אייקון להגדלת תמונה
        ImageButton imageBtnDelete; // אייקון למחיקת תמונה

        int answerIndex = 0;

        // בדיקה אם התמונה היא בתשובות
        if (imgDltBtn.ID.StartsWith("deleteImgAnsBtn"))
        {
            // שמירת התו האחרון = האינדקס
            answerIndex = Convert.ToInt32(imgDltBtn.ID.Last().ToString());

            imgUploadBtn = (ImageButton)FindControl("ImageAnswerBtn" + answerIndex); // הכפתור שצריך לעדכן
            imageBtnZoom = (ImageButton)FindControl("zoomImgAnsBtn" + answerIndex);  // אייקון זום
            imageBtnDelete = (ImageButton)FindControl("deleteImgAnsBtn" + answerIndex);  // אייקון מחיקת תמונה
        }
        // אם לא - אז התמונה בשאלה
        else
        {
            imgUploadBtn = uploadImgInQuestionBtn; // הכפתור שצריך לעדכן
            imageBtnZoom = zoomImgQuest; // אייקון זום
            imageBtnDelete = deleteImgQuestBtn; // אייקון מחיקת תמונה
        }

        System.Web.UI.WebControls.Image ImageHolder = (System.Web.UI.WebControls.Image)FindControl("ImageHolder" + answerIndex);

        // מחיקת התמונה מהשרת לצמיתות
        DeleteFileFromServer(ImageHolder.ImageUrl);

        // אם התמונה בתשובות
        if (imgUploadBtn.ID.StartsWith("ImageAnswerBtn"))
        {
            TextBox ansTxtBox = (TextBox)FindControl("TxtBoxAnswer" + answerIndex); // הכפתור שצריך לעדכן
            UpdateTextInputPanelInAnswer(ansTxtBox, false); // הצגת הפאנל של תיבת הטקסט

            // אם אין טקסט בתיבת הטקסט
            if (ansTxtBox.Text.Length == 0)
            {
                // מכבה את הביט  
                // עדכון התנאים מצד המשתמשת - התנאים שהיא עומדת בהם עכשיו
                SetBitInBitfield(submitConditionsHiddenField, answerIndex, setBitOn: false);

                // בדיקה האם המשתמשת עדיין עומדת בתנאים לשמירת שאלה
                SetSubmitBtnEnableOrDisable();

                //  בדיקה האם להראות כפתורים לאיפוס תשובה ב-2 התשובות הראשונות
                SetClearButtonsVisibility();
            }
        }
        // החלפה לאייקון דיפולטיבי של העלאת תמונה
        imgUploadBtn.ImageUrl = "~/images/add_photo.png";
        imageBtnZoom.Style.Add("visibility", "hidden"); // הסתרת אייקןם זום להגדלת תמונה
        imageBtnDelete.Style.Add("visibility", "hidden"); // הסתרת אייקון למחיקת תמונה

        // הסתרת התמונה
        ImageHolder.Visible = false;
        ImageHolder.ImageUrl = "";


        // בדיקה האם לאפשר/לנטרל כפתור ביטול שינויים
        SetCancelBtnEnableOrDisable();

        // בדיקה האם קיימת תמונה בכפתור להעלאת תמונה, ועדכון קלאס בהתאם
        CheckIfImageExistsAndToggleClass();
    }


    // הגדלת תמונה
    protected void ZoomImage_BtnClick(object sender, ImageClickEventArgs e)
    {
        ImageButton imgZoomBtn = (ImageButton)sender;
        // שמירת הכפתור להעלאת תמונה שמקושר לכפתור להגדלת תמונה
        ImageButton imgUploadBtn;
        int answerIndex = 0;
        // בדיקה אם התמונה היא בתשובות
        if (imgZoomBtn.ID.StartsWith("zoomImgAnsBtn"))
        {
            // שמירת התו האחרון = האינדקס
            answerIndex = Convert.ToInt32(imgZoomBtn.ID.Last().ToString());
            imgUploadBtn = (ImageButton)FindControl("ImageAnswerBtn" + answerIndex);
        }
        // אם לא - אז התמונה בשאלה
        else
        {
            imgUploadBtn = (ImageButton)FindControl("uploadImgInQuestionBtn");
        }


        System.Web.UI.WebControls.Image ImageHolder = (System.Web.UI.WebControls.Image)FindControl("ImageHolder" + answerIndex);

        // של התמונה שצריך להגדיל URL-שמירת 
        //string imgUrlToZoom = imgUploadBtn.ImageUrl;
        string imgUrlToZoom = ImageHolder.ImageUrl;

        // אם התמונה היא האייקון הדיפולטיבי לא צריך להגדיל
        if (imgUrlToZoom == "~/images/add_photo.png")
        {
            return;
        }

        modalZoomImg.ImageUrl = imgUrlToZoom;

        // פתיחת המודל באמצעות הפונקציה שנמצאת בג'אווה סקריפט
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "zoomImgModal();", true);
    }


    // מחיקה + איפוס תשובה
    protected void DeleteAnswerBtn_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton deleteAnsBtn = (ImageButton)sender;
        // שמירת התו האחרון = האינדקס
        //string answerIndex = deleteAnsBtn.ID.Last().ToString();
        int answerIndex = Convert.ToInt32(deleteAnsBtn.ID.Last().ToString());

        // אם זה לא 2 התשובות הראשונות - אפשר למחוק אותן לחלוטין
        if (answerIndex > 2)
        {
            for (int i = answerIndex; i <= 6; i++)
            {
                // אם מוחקת את התשובה השישית
                // אז מאפסות את הפאנל, מעלימות אותו ויוצאות מהלולאה
                if (i == 6)
                {
                    // איפוס ומחיקת התמונה
                    ImageButton imgDltBtn = (ImageButton)FindControl("deleteImgAnsBtn" + i);
                    DeleteImgBtn_Click(imgDltBtn, null);
                    // איפוס תיבת הטקסט
                    TextBox ansTxtBox = (TextBox)FindControl("TxtBoxAnswer" + i);
                    ansTxtBox.Text = "";
                    // העלמת הפאנל של התשובה
                    Panel answerPanel = (Panel)FindControl("answerPanel" + i);
                    answerPanel.Visible = false;

                    // עדכון ביטפילד
                    // מכבה את הביט בביטפילד שמייצג את על הפאנלים שמוצגים
                    // עדכון התנאים מצד הממשק - התנאים שהמשתמשת צריכה לעמוד בהם עכשיו
                    SetBitInBitfield(visiblePanelsHiddenField, i, setBitOn: false);


                    // עדכון הביטפילד שמייצג את קיום התנאים מצד המשתשמשת - באילו תנאים היא עומדת עכשיו
                    // מכבה את הביט  
                    SetBitInBitfield(submitConditionsHiddenField, i, setBitOn: false);

                    break;
                }

                // שמירת הפאנל של 
                Panel nextAnswerPanel = (Panel)FindControl("answerPanel" + (i + 1));

                //ImageButton imgDltBtn = (ImageButton)FindControl("deleteImgAnsBtn" + (i+1));

                if (nextAnswerPanel.Visible)
                {

                    TextBox currentAnswerTB = (TextBox)FindControl("TxtBoxAnswer" + i);
                    TextBox nextAnswerTB = (TextBox)FindControl("TxtBoxAnswer" + (i + 1));

                    // העברת הטקסט מהפאנל הבא לפאנל שכביכול מחקנו עכשיו
                    currentAnswerTB.Text = nextAnswerTB.Text;

                    // התמונה מהפאנל שאנחנו רוצות למחוק
                    ImageButton currentImgAnsBtn = (ImageButton)FindControl("ImageAnswerBtn" + i);
                    System.Web.UI.WebControls.Image currentImageHolder = (System.Web.UI.WebControls.Image)FindControl("ImageHolder" + i);

                    // התמונה מהפאנל שאנחנו רוצות להעביר
                    ImageButton nextImgAnsBtn = (ImageButton)FindControl("ImageAnswerBtn" + (i + 1));
                    System.Web.UI.WebControls.Image nextImageHolder = (System.Web.UI.WebControls.Image)FindControl("ImageHolder" + (i + 1));

                    // העברת האייקון לשינוי/הוספת מהפאנל הבא לפאנל שכביכול מחקנו עכשיו
                    currentImgAnsBtn.ImageUrl = nextImgAnsBtn.ImageUrl;

                    // העברת התמונה מהפאנל הבא לפאנל שכביכול מחקנו עכשיו
                    currentImageHolder.ImageUrl = nextImageHolder.ImageUrl;

                    currentImageHolder.Visible = nextImageHolder.Visible;

                    // בדיקה האם התמונה היא לא התמונה הדיפולטיבית
                    // כלומר יש/אין תמונה בתשובה
                    var isImage = nextImgAnsBtn.ImageUrl != "~/images/add_photo.png";

                    // מעדכנת פאנל מלא של הזנת תוכן - אם יש / אין תמונה
                    UpdateInputPanelDisplay(i, isImage, isCallSetSubmitBtnEnableOrDisable: false);
                }

                // 
                else
                {
                    // איפוס ומחיקת התמונה
                    ImageButton imgDltBtn = (ImageButton)FindControl("deleteImgAnsBtn" + i);
                    DeleteImgBtn_Click(imgDltBtn, null);
                    // איפוס תיבת הטקסט
                    TextBox ansTxtBox = (TextBox)FindControl("TxtBoxAnswer" + i);
                    ansTxtBox.Text = "";
                    // העלמת הפאנל של התשובה
                    Panel answerPanel = (Panel)FindControl("answerPanel" + i);
                    answerPanel.Visible = false;

                    // עדכון ביטפילד
                    // מכבה את הביט בביטפילד שמייצג את על הפאנלים שמוצגים
                    // עדכון התנאים מצד הממשק - התנאים שהמשתמשת צריכה לעמוד בהם עכשיו
                    SetBitInBitfield(visiblePanelsHiddenField, i, setBitOn: false);

                    // עדכון הביטפילד שמייצג את קיום התנאים מצד המשתשמשת - באילו תנאים היא עומדת עכשיו
                    // מכבה את הביט  
                    SetBitInBitfield(submitConditionsHiddenField, i, setBitOn: false);

                    break;
                }
            }

            // אם התשובה שנמחקת מסומנת כתשובה הנכונה
            if (chosenRightAnswer.Value == answerIndex.ToString())
            {
                // סימון תשובה ראשונה כתשובה נכונה
                chosenRightAnswer.Value = "1";
                ChooseAnswerMakeGreen("1");
            }
            // הצגת כפתור להוספת תשובה
            addAnsBtn.Visible = true;
        }

        // אם מדובר ב2 התשובות הראשונות שאי אפשר למחוק - רק לאפס
        else
        {
            // איפוס ומחיקת התמונה
            ImageButton imgDltBtn = (ImageButton)FindControl("deleteImgAnsBtn" + answerIndex);
            DeleteImgBtn_Click(imgDltBtn, null);
            // איפוס תיבת הטקסט
            TextBox ansTxtBox = (TextBox)FindControl("TxtBoxAnswer" + answerIndex);
            ansTxtBox.Text = "";

            // עדכון הביטפילד שמייצג את קיום התנאים מצד המשתשמשת - באילו תנאים היא עומדת עכשיו
            // מכבה את הביט  
            SetBitInBitfield(submitConditionsHiddenField, answerIndex, setBitOn: false);
        }

        // בדיקה האם המשתמשת עומדת בתנאים לשמירת שאלה
        SetSubmitBtnEnableOrDisable();

        //  בדיקה האם להראות כפתורים לאיפוס תשובה ב-2 התשובות הראשונות
        SetClearButtonsVisibility();

        // בדיקה האם לאפשר/לנטרל כפתור ביטול שינויים
        SetCancelBtnEnableOrDisable();

        // בדיקה האם קיימת תמונה בכפתור להעלאת תמונה, ועדכון קלאס בהתאם
        CheckIfImageExistsAndToggleClass();
    }



    // הוספת תשובה
    protected void AddAnsBtn_Click(object sender, EventArgs e)
    {
        // ספירת התשובות שלא מוצגות בממשק
        int counter = 0;

        // בדיקה איזה תשובה להציג  
        for (int i = 3; i <= 6; i++)
        {
            Panel answerPanel = (Panel)FindControl("answerPanel" + i);
            // אם התשובה לא מוצגת
            if (!answerPanel.Visible)
            {
                // נציג את התשובה הראשונה שנקבל
                if (counter == 0)
                {
                    answerPanel.Visible = true;

                    // עדכון הביטפילד שמייצג את כל הפאנלים שמוצגים
                    // מדליקה את הביט  
                    // עדכון התנאים מצד הממשק - התנאים שהמשתמשת צריכה לעמוד בהם עכשיו
                    SetBitInBitfield(visiblePanelsHiddenField, i, setBitOn: true);

                    // בדיקה האם המשתמשת עומדת בתנאים לשמירת שאלה
                    SetSubmitBtnEnableOrDisable();
                }
                counter++;
            }
        }

        // אם תשובה אחת לא הייתה מוצגת
        // כלומר הוספנו את התשובה האחרונה
        if (counter == 1)
        {
            // נסתיר את כפתור הוספת תשובה
            addAnsBtn.Visible = false;
        }
    }


    // מחיקת תמונה מהשרת לצמיתות
    protected void DeleteFileFromServer(string imgUrlToDlt)
    {
        // לא מוחקת מהשרת
        // אם התמונה היא האייקון הדיפולטיבי
        // או שאני במצב של עריכת שאלה
        if (imgUrlToDlt == "~/images/add_photo.png" || questionIdFromBankToEdit.Value != "edit")
        {
            return;
        }

        string filePath = Server.MapPath(imgUrlToDlt);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    // סימון תשובה נכונה בבורדר ירוק והצגת לייבל תשובה נכונה
    protected void ChooseAnswerMakeGreen(string rightAnsIndex)
    {
        // שינוי כל הבורדרים לצבע נייטרלי והסתרת לייבל ירוק לתשובה נכונה
        for (int i = 1; i <= 6; i++)
        {
            Panel answerPanel = (Panel)FindControl("chooseAnswerPanel" + i);
            answerPanel.Style.Add("border-color", "rgb(240, 240, 250)");
            Label notRightAnswerLabel = (Label)FindControl("rightAnswerLabel" + i);
            notRightAnswerLabel.Style.Add("visibility", "hidden");
        }

        // שינוי צבע הבורדר לירוק - סימון תשובה נכונה
        Panel chosenAnswerPanel = (Panel)FindControl("chooseAnswerPanel" + rightAnsIndex);
        chosenAnswerPanel.Style.Add("border-color", "#17AD9C");
        // הצגת לייבל תשובה נכונה
        Label rightAnswerLabel = (Label)FindControl("rightAnswerLabel" + rightAnsIndex);
        rightAnswerLabel.Style.Add("visibility", "visible");
    }


    //  בדיקה האם להראות כפתורים לאיפוס תשובה ב-2 התשובות הראשונות
    protected void SetClearButtonsVisibility()
    {
        // מעבר על 2 התשובות הראשונות
        // בשביל לדעת אם להראות כפתור לאיפוס תשובה או לא
        for (int i = 1; i <= 2; i++)
        {
            // הייצוג הבינארי של האינדקס 
            int bitPanelIndex = (int)Math.Pow(2, i);
            // בדיקה האם התנאים של התשובה הנבדקת נכונים - טקסט או תמונה
            // האם הביט מודלק (שונה מאפס)
            if ((Convert.ToInt32(submitConditionsHiddenField.Value) & bitPanelIndex) != 0)
            {
                // הצגת כפתור לאיפוס תשובה - בגלל שיש תוכן
                ((ImageButton)FindControl("deleteAnswerBtn" + i)).Style.Add("visibility", "visible");
            }
            else
            {
                // נסתיר את הכפתור לאיפוס התשובה - בגלל שלא קיים תוכן
                ((ImageButton)FindControl("deleteAnswerBtn" + i)).Style.Add("visibility", "hidden");
            }
        }
    }


    // בדיקה האם המשתמשת עומדת בתנאים לשמירת שאלה
    protected void SetSubmitBtnEnableOrDisable()
    {
        // אם התנאים חופפים - נאפשר את הכפתור לשמירת שאלה
        if (submitConditionsHiddenField.Value == visiblePanelsHiddenField.Value)
        {
            saveQuestBtn.Enabled = true;
        }
        // אם לא - מנטרלת את הכפתור
        else
        {
            saveQuestBtn.Enabled = false;
        }
    }


    // שיטה שמתבצעת בעת לחיצה על אחד הכפתורים (עריכה או מחיקה) בתוך טבלת השאלות
    protected void QuestionsGridview_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // תחילה אנו מבררים מהו האיידי של השאלה בעץ       
        // e = כל הארגומנטים של הגריד וויו
        // e.CommandSource = הכפתור שנלחץ 
        // שמירת האוביקט של הכפתור שנלחץ
        ImageButton imgBtnClicked = (ImageButton)e.CommandSource;
        // שמירת האיידי של השאלה
        string questionId = imgBtnClicked.Attributes["theItemId"];

        // עלינו לברר איזו פקודה צריכה להתבצע - הפקודה רשומה בכל כפתור             
        switch (e.CommandName)
        {
            // בלחיצה על כפתור עריכת שאלה
            case "editQ":

                // שמירת השורה שנבחרה כדי שנוכל לשנות את הצבע בג'אווה סקריפט
                GridViewRow row = (GridViewRow)imgBtnClicked.NamingContainer;

                // אם לחצנו על עריכת שאלה כשאנחנו כבר באמצע עריכת שאלה אחרת
                if (currentRowIndexHiddenField.Value != "row")
                {
                    //  אם התבצעו שינויים - צריך לקפוץ פופאפ
                    // בדיקה האם קיים תוכן חדש ולא שמור בממשק                 
                    if (WereChangesMade(questionIdFromBankToEdit.Value))
                    {
                        // שמירת המידע של השורה הבאה שאולי נערוך בפקד הנסתר
                        nextRowIndexAndIDHiddenField.Value = (row.RowIndex + 2).ToString() + "," + questionId;
                        // פתיחת המודל באמצעות הג'אווה סקריפט - השינויים שביצעת לא יישמרו
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "saveChangesModal();", true);
                        return;
                    }
                }

                // אם אנחנו לא באמצע עריכת שאלה חדשה
                else
                {
                    // נבדוק אם אכן יש תוכן בממשק - צריך לקפוץ פופאפ
                    if (submitConditionsHiddenField.Value != "0" || uploadImgInQuestionBtn.ImageUrl != "~/images/add_photo.png")
                    {
                        // שמירת המידע של השורה הבאה שאולי נערוך בפקד הנסתר
                        nextRowIndexAndIDHiddenField.Value = (row.RowIndex + 2).ToString() + "," + questionId;
                        // פתיחת המודל באמצעות הג'אווה סקריפט - השינויים שביצעת לא יישמרו
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "saveChangesModal();", true);
                        return;
                    }
                }

                // אם לא צריך פופ אפ

                // שמירת השורה שעורכות עכשיו בהידן פילד
                currentRowIndexHiddenField.Value = (row.RowIndex + 2).ToString();
                // עדכון האיידי של השאלה שרוצות לעדכן בפקד הנסתר
                questionIdFromBankToEdit.Value = questionId;
                // עדכון הטקסט על הכפתור לשמירת שינויים
                saveQuestBtn.Text = "שמירת שינויים";


                // קריאה לשיטה שמציגה את התוכן של השאלה בממשק
                EditQuestionFromBank(questionId);
                break;

            // בלחיצה על כפתור מחיקת שאלה                
            // נפתח פופ אפ וידוא לפני מחיקה - חלון האם למחוק
            case "deleteQ":

                // עדכון האיידי של השאלה שרוצות למחוק בפקד הנסתר
                questionIdFromBankToDelete.Value = questionId;

                XmlNode questionNodeToDelete = gameQuestions.SelectSingleNode("question[@id='" + questionId + "']");

                // בדיקת הגנה
                // אם קרתה תקלה והשאלה לא קיימת מפנה לעמוד השגיאה
                if (questionNodeToDelete == null)
                {
                    Response.Redirect("error.aspx");
                }

                // הדפסת טקסט השאלה ללייבל המתאים בפופאפ
                modalQuestToDeleteLbl.Text = Server.UrlDecode(questionNodeToDelete.SelectSingleNode("questionText").InnerXml);

                // הדפסת מספר התשובות שיש בשאלה
                modalAnsCounterLbl.Text = gameQuestions.SelectNodes("question[@id='" + questionId + "']/answers/answer").Count.ToString();

                // פתיחת המודל באמצעות הפונקציה שנמצאת בג'אווה סקריפט
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "warningModal();", true);

                break;

            default:
                return;
        }
    }


    // בדיקה האם קיים תוכן חדש ולא שמור בממשק
    protected bool WereChangesMade(string questionId)
    {
        // משיכת הטקסט בגזע השאלה השמור בעץ 
        string questTextXML = Server.UrlDecode(gameQuestions.SelectSingleNode("question[@id='" + questionId + "']/questionText").InnerXml);

        // אם התוכן בגזע השאלה ששמור בעץ שונה מהתוכן בממשק
        if (questTextXML != questionTxtBox.Text)
        {
            return true;
        }

        // שמירת הענף של התמונה בשאלה
        XmlNode imgQuest = gameQuestions.SelectSingleNode("question[@id='" + questionId + "']/img");

        // אם יש תמונה בגזע השאלה השמורה בעץ
        if (imgQuest != null)
        {
            // אם קיימת תמונה בממשק
            if (ImageHolder0.Visible)
            {
                // בדיקה האם התמונה שנמצאת בממשק שונה מהתמונה ששמורה בעץ
                if (ImageHolder0.ImageUrl != "~/uploadedFiles/" + imgQuest.InnerXml)
                {
                    return true;
                }
            }

        }
        // אם אין תמונה בגזע השאלה ששמורה בעץ
        else
        {
            // בדיקה האם קיימת תמונה חדשה בגזע השאלה בממשק
            if (uploadImgInQuestionBtn.ImageUrl != "~/images/add_photo.png")
            {
                return true;
            }
        }

        // רשימה המכילה עותק של התשובות ששמורות בעץ
        List<XmlNode> answersInXml = gameQuestions.SelectNodes("question[@id='" + questionId + "']/answers/answer").Cast<XmlNode>().ToList();

        // לולאה למעבר על תוכן המסיחים בממשק והשוואה לרשימה
        for (int i = 1; i <= 6; i++)
        {
            if (!((Panel)FindControl("answerPanel" + i)).Visible)
            {
                continue;
            }

            ImageButton imageAnswerBtn = ((ImageButton)FindControl("ImageAnswerBtn" + i));

            System.Web.UI.WebControls.Image ImageHolder = (System.Web.UI.WebControls.Image)FindControl("ImageHolder" + i);

            TextBox ansTxtBox = (TextBox)FindControl("TxtBoxAnswer" + i);

            // בדיקה האם יש תמונה בממשק
            if (imageAnswerBtn.ImageUrl != "~/images/add_photo.png")
            {
                // נחפש ברשימת התשובות את האינדקס הראשון שמכיל תמונה זהה
                int indexToRemove = answersInXml.FindIndex(node => node.Attributes["AnsType"].InnerText == "image" && "~/uploadedFiles/" + node.InnerXml == ImageHolder.ImageUrl);

                // אם לא קיימת תשובה בעץ שמכילה תמונה זהה לתמונה בתשובה בממשק
                if (indexToRemove == -1)
                {
                    return true;
                }

                // נמצאה תשובה המכילה תמונה בעץ שזהה לתמונה בממשק
                // נמחק את התשובה מהרשימה
                answersInXml.RemoveAt(indexToRemove);
            }
            // אם יש טקסט בממשק
            else
            {
                // נחפש ברשימת התשובות את האינדקס הראשון שמכיל טקסט זהה
                int indexToRemove = answersInXml.FindIndex(node => node.Attributes["AnsType"].InnerText == "text" && Server.UrlDecode(node.InnerXml) == ansTxtBox.Text);

                // אם לא קיימת תשובה בעץ שמכילה טקסט זהה לטקסט בתשובה בממשק
                // יש בממשק יותר תוכן ממה ששמור בעץ
                if (indexToRemove == -1)
                {
                    return true;
                }

                // נמצאה תשובה המכילה טקסט בעץ שזהה לטקסט בממשק
                // נמחק את התשובה מהרשימה
                answersInXml.RemoveAt(indexToRemove);
            }
        }

        // אם מספר התשובות ששמור בעץ לא זהה למספר התשובות שנמצא בממשק
        // יש בעץ יותר תוכן ממה ששמור בממשק
        if (answersInXml.Count > 0)
        {
            return true;
        }

        // לא קיים תוכן חדש ולא שמור
        return false;

    }


    // שיטה שמתבצעת לאחר לחיצה על עריכת שאלה חדשה דרך הפופ אפ 
    protected void EditQuestionModalBtn_Click(object sender, EventArgs e)
    {
        // שליפת האינדקס של השורה והאיידי של השאלה מהפקד הנסתר
        string[] rowAndIDArr = nextRowIndexAndIDHiddenField.Value.Split(',');

        // עדכון השורה שעורכות עכשיו
        currentRowIndexHiddenField.Value = rowAndIDArr[0];

        // עדכון האיידי של השאלה שעורכות עכשיו
        questionIdFromBankToEdit.Value = rowAndIDArr[1];

        // קריאה לפונקציה לעריכת שאלה ממאגר השאלות
        EditQuestionFromBank(rowAndIDArr[1]);
    }


    // עריכת שאלה ממאגר השאלות
    // מציגה תוכן של שאלה קיימת בממשק של עריכת השאלה
    protected void EditQuestionFromBank(string id)
    {
        // אם קרתה שגיאה והשאלה לא קיימת/ לא נמצאה
        if (gameQuestions.SelectSingleNode("question[@id='" + id + "']") == null)
        {
            Response.Redirect("error.aspx");
        }

        // מאפסת את כל הפאנלים להזנת תוכן
        ResetAllPanels();

        // הדפסת טקסט השאלה לתיבת הטקסט
        questionTxtBox.Text = Server.UrlDecode(gameQuestions.SelectSingleNode("question[@id='" + id + "']/questionText").InnerXml);

        // עדכון ביטפילד
        // בגלל שיש טקסט בגזע השאלה
        // עדכון התנאים מצד המשתמשת - התנאים שהיא עומדת בהם עכשיו
        SetBitInBitfield(submitConditionsHiddenField, 0, setBitOn: questionTxtBox.Text != "");

        // שמירת הענף של התמונה בשאלה
        XmlNode imgQuest = gameQuestions.SelectSingleNode("question[@id='" + id + "']/img");

        // בדיקה האם יש תמונה בשאלה והצגה שלה בכפתור תמונה
        if (imgQuest != null)
        {
            // השמת התמונה בתוך הכפתור של העלאת תמונה
            //uploadImgInQuestionBtn.ImageUrl = "~/uploadedFiles/" + imgQuest.InnerXml;

            // השמת התמונה בתוך האלמנט להחזקת תמונה
            ImageHolder0.ImageUrl = "~/uploadedFiles/" + imgQuest.InnerXml;
            ImageHolder0.Visible = true; // הצגת התמונה
            uploadImgInQuestionBtn.ImageUrl = "~/images/exchange.png"; // שינוי לאייקון של החלפת תמונה

            // מעדכנת נראות של פאנל להזנת תוכן
            UpdateInputPanelDisplay(0, true, false);
        }

        // שמירת קיצור לשאילתא שמוצאת את התשובות בשאלה
        //string answersXPath = "question[@id='" + id + "']/answers/answer";

        // שמירת רשימת ענפים שמכילה את כל התשובות מהעץ
        XmlNodeList answersNodeList = gameQuestions.SelectNodes("question[@id='" + id + "']/answers/answer");

        // שמירת האינדקס של פאנל השאלה
        int ansPanelIndex = 1;

        // בדיקה מה סוג תשובה והצגה בממשק בהתאם - טקסט או תמונה
        foreach (XmlElement answer in answersNodeList)
        {
            // בדיקה האם התשובה מסוג תמונה ושמירה במשתנה בוליאני
            bool isImage = answer.GetAttribute("AnsType") == "image";
            bool setBitOn;

            // מעדכנת נראות של פאנל להזנת תוכן
            // בשביל אייקון של תמונה
            UpdateInputPanelDisplay(ansPanelIndex, isImage, false);

            // אם התשובה מכילה טקסט
            // הדפסת טקסט התשובה לתיבת הטקסט
            if (!isImage)
            {
                TextBox answerTB = (TextBox)FindControl("TxtBoxAnswer" + ansPanelIndex);
                answerTB.Text = Server.UrlDecode(answer.InnerXml);
                // בדיקה האם יש טקסט בתשובה
                setBitOn = answerTB.Text != "";
            }
            // אם התשובה מכילה תמונה
            // השמת התמונה בתוך הכפתור של העלאת תמונה
            else
            {
                ImageButton imageAnswerBtn = (ImageButton)FindControl("ImageAnswerBtn" + ansPanelIndex);

                System.Web.UI.WebControls.Image ImageHolder = (System.Web.UI.WebControls.Image)FindControl("ImageHolder" + ansPanelIndex);

                ImageHolder.ImageUrl = "~/uploadedFiles/" + answer.InnerXml;
                ImageHolder.Visible = true;

                imageAnswerBtn.ImageUrl = "~/images/exchange.png";

                // בדיקה האם יש תמונה בתשובה
                setBitOn = true;
            }
            // ב2 התשובות הראשונות מציגה אייקון לאיפוס תשובה
            if (ansPanelIndex <= 2)
            {
                ((ImageButton)FindControl("deleteAnswerBtn" + ansPanelIndex)).Style.Add("visibility", "visible");
            }
            // אחרי 2 תשובות, צריך לוודא שהפאנלים של התשובות הקיימות מוצגות
            else
            {
                // הצגת הפאנל של התשובה 
                ((Panel)FindControl("answerPanel" + ansPanelIndex)).Visible = true;

                // עדכון ביטפילד
                // מדליקה את הביט בביטפילד שמייצג את כל הפאנלים שמוצגים
                // עדכון התנאים מצד הממשק - התנאים שהמשתמשת צריכה לעמוד בהם עכשיו
                SetBitInBitfield(visiblePanelsHiddenField, ansPanelIndex, setBitOn: true);
            }

            // בדיקה האם זאת התשובה הנכונה
            if (answer.GetAttribute("isCorrect") == "true")
            {
                // עדכון בפילד המוסתר
                chosenRightAnswer.Value = ansPanelIndex.ToString();
            }

            // עדכון ביטפילד
            // בגלל שיש תוכן בתשובה - מדליקה את הביט
            // עדכון התנאים מצד המשתמשת - התנאים שהיא עומדת בהם עכשיו
            SetBitInBitfield(submitConditionsHiddenField, ansPanelIndex, setBitOn);

            // העלאת האינדקס לפאנל של השאלה הבאה
            ansPanelIndex++;
        }

        // סימון תשובה נכונה 
        ChooseAnswerMakeGreen(chosenRightAnswer.Value);

        // בסיום צריך לוודא ששאר הפאנלים של התשובות מוסתרים
        for (int i = answersNodeList.Count + 1; i <= 6; i++)
        {
            // הסתרת הפאנל של התשובה 
            ((Panel)FindControl("answerPanel" + i)).Visible = false;

            // עדכון ביטפילד
            // מכבה את הביט בביטפילד שמייצג את כל הפאנלים שמוצגים
            // עדכון התנאים מצד הממשק - התנאים שהמשתמשת צריכה לעמוד בהם עכשיו
            SetBitInBitfield(visiblePanelsHiddenField, i, setBitOn: false);
        }

        // אם יש בול 6 תשובות
        // הסתרת הכפתור להוספת תשובה חדשה
        addAnsBtn.Visible = answersNodeList.Count != 6;

        // בדיקה האם המשתמשת עומדת בתנאים לשמירת שאלה
        SetSubmitBtnEnableOrDisable();

        // בדיקה האם לאפשר/לנטרל כפתור ביטול שינויים
        SetCancelBtnEnableOrDisable();

        //  בדיקה האם להראות כפתורים לאיפוס תשובה ב-2 התשובות הראשונות
        SetClearButtonsVisibility();

        // בדיקה האם קיימת תמונה בכפתור להעלאת תמונה, ועדכון קלאס בהתאם
        CheckIfImageExistsAndToggleClass();
    }


    // מאפסת את הפאנלים להזנת תוכן בממשק
    protected void ResetAllPanels()
    {
        // איפוס הטקסט בשאלה
        questionTxtBox.Text = "";

        // איפוס אייקון להעלאת תמונה בשאלה
        uploadImgInQuestionBtn.ImageUrl = "~/images/add_photo.png";
        ImageHolder0.Visible = false;
        ImageHolder0.ImageUrl = "";

        // מחיקת טקסט + תמונה מהתשובות, רק תמונה מהשאלה
        for (int i = 0; i <= 6; i++)
        {
            // בתשובות
            if (i != 0)
            {
                // איפוס הטקסט
                TextBox TxtBoxAnswer = (TextBox)FindControl("TxtBoxAnswer" + i);
                TxtBoxAnswer.Text = "";

                // איפוס אייקון תמונה
                ImageButton ImageAnswerBtn = (ImageButton)FindControl("ImageAnswerBtn" + i);
                ImageAnswerBtn.ImageUrl = "~/images/add_photo.png";

                System.Web.UI.WebControls.Image ImageHolder = (System.Web.UI.WebControls.Image)FindControl("ImageHolder" + i);
                ImageHolder.Visible = false;
                ImageHolder.ImageUrl = "";
            }

            // איפוס אזור הזנת תוכן בכל הפאנלים
            // מעבירה שאין תמונה
            // מאפשרת הזנת טקסט בתשובות
            // ומעלימה אייקונים של מחיקה והגדלת תמונה
            UpdateInputPanelDisplay(i, isImage: false, isCallSetSubmitBtnEnableOrDisable: false);
        }

    }

    // עדכון ביטפלד!!!!!!
    // מכבה או מדליק את הביט
    protected void SetBitInBitfield(HiddenField bitfieldHiddenField, int index, bool setBitOn)
    {
        // הייצוג הבינארי של האינדקס
        // ביט ספציפי בביטפילד
        int indexToBit = Convert.ToInt32(Math.Pow(2, index));
        // המרה של תוכן ההידןפילד למספר
        // מכיל את כל הביטפילד
        int bitfieldHiddenFieldInt = Convert.ToInt32(bitfieldHiddenField.Value);

        // בדיקה האם להדליק את הביט בביטפילד
        if (setBitOn)
        {
            // ביטוויז אור מדליק את הביט
            bitfieldHiddenField.Value = (bitfieldHiddenFieldInt |= indexToBit).ToString();
        }
        // לכבות את הביט ביטפילד
        else
        {
            // ביטוויז נוט וביטוויז אנד מכבה את הביט
            bitfieldHiddenField.Value = (bitfieldHiddenFieldInt &= ~indexToBit).ToString();
        }

    }

    // מחיקת שאלה לצמיתות מהעץ
    protected void DeleteQuestionBtn_Click(object sender, EventArgs e)
    {
        // מציאת הענף של השאלה שצריך למחוק
        XmlNode questionNodeToDelete = gameQuestions.SelectSingleNode("question[@id='" + questionIdFromBankToDelete.Value + "']");

        // בדיקת הגנה
        // אם קרתה שגיאה מפנה לעמוד המתאים
        if (questionNodeToDelete == null)
        {
            Response.Redirect("error.aspx");
        }

        // מחיקת תמונות מהשרת - במידה ויש   
        // שמירת הענף של התמונה בשאלה
        XmlNode imgQuest = questionNodeToDelete.SelectSingleNode("img");

        // בדיקה האם יש תמונה בשאלה
        if (imgQuest != null)
        {
            // מחיקת התמונה מהשרת
            string imgUrlToDlt = "~/uploadedFiles/" + imgQuest.InnerXml;
            DeleteFileFromServer(imgUrlToDlt);
        }

        // שמירת רשימת נודים שמכילה את כל התשובות מהעץ
        XmlNodeList answersNodeList = questionNodeToDelete.SelectNodes("answers/answer");

        // בדיקה האם התשובה מסוג תמונה
        foreach (XmlElement answer in answersNodeList)
        {
            if (answer.GetAttribute("AnsType") == "image")
            {
                // מחיקת התמונה מהשרת
                string imgUrlToDlt = "~/uploadedFiles/" + answer.InnerXml;
                DeleteFileFromServer(imgUrlToDlt);
            }
        }

        // מחיקת הענף של השאלה מהעץ
        questionNodeToDelete.ParentNode.RemoveChild(questionNodeToDelete);

        // שמירת השינויים בעץ
        LaCasaDeCookieXML.Save(Server.MapPath("XML/LaCasaDeCookie.xml"));

        // שמירת שינויים והצגה בגרידוויו
        XmlGameSource.Save();
        questionsGridView.DataBind();

        // אם השאלה שמחקתי זאת השאלה שאני עורכת עכשיו בממשק - לאפס את הממשק
        if (questionIdFromBankToDelete.Value == questionIdFromBankToEdit.Value)
        {
            // איפוס אזור הזנת תוכן + החזרה לערכים התחלתיים של הכל
            Response.Redirect("edit.aspx");
        }

        // איפוס האיידי של השאלה שרוצות למחוק לערך ההתחלתי
        questionIdFromBankToDelete.Value = "dlt";

        // עדכון האזור של טבלת השאלות
        UpdateGridviewArea();
    }

    // ביטול שינויים של שאלה
    protected void CancelChangesBTN_Click(object sender, EventArgs e)
    {
        Response.Redirect("edit.aspx");
    }


    // בודקת מה צריך להיות מוצג באזור של טבלת השאלות
    protected void UpdateGridviewArea()
    {
        // בדיקה מה להציג בפאנל פרסום משחק
        CheackIfPublish();

        // מספר השאלות במשחק
        int numQuestions = gameQuestions.SelectNodes("question").Count;

        // הצגת מספר השאלות במשחק - בטבלת השאלות
        numberOfQuestions.Text = "(" + numQuestions + ")";

        // הצגת טבלה ריקה אם אין שאלות בכלל
        zeroQuestionsTable.Visible = numQuestions == 0;

        // הצגת טבלה אמיתית רק אם יש שאלות
        // הגנה במקרה של רענון העמוד
        questionsGridView.Visible = numQuestions != 0;
    }


    // בדיקה האם המשחק עומד בתנאי הפרסום
    protected void CheackIfPublish()
    {
        // אם יש פחות מ10 שאלות במשחק
        // ואי אפשר לפרסם את המשחק
        if (gameQuestions.SelectNodes("question").Count < 10)
        {
            // אם המשחק כבר מפורסם
            if (gameNode.Attributes["isPublish"].InnerText == "true")
            {
                // עדכון של המאפיין בעץ המשחק - לא יכול להיות מפורסם
                gameNode.Attributes["isPublish"].InnerText = "false";

                // שמירת השינויים בעץ
                LaCasaDeCookieXML.Save(Server.MapPath("XML/LaCasaDeCookie.xml"));
            }
            minimumReqLbl.Text = "מינימום 10 שאלות לפרסום";
            minimumReqLbl.Style.Add("color", "red");
            publishGameCB.Enabled = false;
            publishGameCB.Style.Add("cursor", "not-allowed");
        }
        // אם אפשר לפרסם את המשחק
        else
        {
            minimumReqLbl.Text = "פרסום משחק";
            minimumReqLbl.Style.Add("color", "black");
            publishGameCB.Enabled = true;
            publishGameCB.Style.Add("cursor", "pointer");
        }

        // עדכון צ'קבוקס לפי המידע מהעץ
        publishGameCB.Checked = gameNode.Attributes["isPublish"].InnerText == "true";

    }

    // פרסום משחק דרך טבלת השאלות
    protected void PublishGameCB_CheckedChanged(object sender, EventArgs e)
    {
        // שמות את הערך ההפוך ממה שיש בעץ
        bool opposite = gameNode.Attributes["isPublish"].InnerText == "true";

        // עדכון צ'קבוקס 
        publishGameCB.Checked = !opposite;

        //עדכון של המאפיין בעץ
        gameNode.Attributes["isPublish"].InnerText = (!opposite).ToString().ToLower();

        // שמירת השינויים בעץ
        LaCasaDeCookieXML.Save(Server.MapPath("XML/LaCasaDeCookie.xml"));

        //שמירה בעץ והצגה
        XmlGameSource.Save();
        questionsGridView.DataBind();
    }


    // שיטה שמתרחשת בלחיצה על הלינק בחזרה למשחקים שלי
    //  אם התבצעו שינויים - צריך לקפוץ פופאפ
    protected void ReturnMyGamesLinkInEdit_Click(object sender, EventArgs e)
    {
        // אם לחצנו על בחזרה למשחקים כשאנחנו כבר באמצע עריכת שאלה
        if (currentRowIndexHiddenField.Value != "row")
        {
            // בדיקה האם קיים תוכן חדש ולא שמור בממשק                 
            if (WereChangesMade(questionIdFromBankToEdit.Value))
            {
                // פתיחת המודל באמצעות הג'אווה סקריפט - וידוא לפני חזרה למשחקים שלי
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "backMyGamesModal();", true);
                return;
            }
        }
        // אם אנחנו לא באמצע עריכת שאלה חדשה
        else
        {
            // נבדוק אם אכן יש תוכן בממשק - צריך לקפוץ פופאפ
            if (submitConditionsHiddenField.Value != "0" || uploadImgInQuestionBtn.ImageUrl != "~/images/add_photo.png")
            {
                // פתיחת המודל באמצעות הג'אווה סקריפט - וידוא לפני חזרה למשחקים שלי
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "backMyGamesModal();", true);
                return;
            }
        }

        // אם לא התבצעו שינויים - מעבירה בחזרה למשחקים שלי
        Response.Redirect("myGames.aspx");
    }

  
}



