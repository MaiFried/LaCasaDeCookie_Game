<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>עריכת שאלות - La Casa De Cookie</title>
    <meta name="description" content="La Casa De Cookie" />
    <meta name="keywords" content="חג המולד, אפייה, מחולל משחקים, מחולל, עריכה, משחק לימודי, למידה, איש לחם זנגביל" />
    <meta name="author" content="Mai Fried, Dolly Gotie" />
    <link rel="icon" href="images/face.png" />

    <!-- FONTS -->
    <link href="https://fonts.googleapis.com/css2?family=Assistant:wght@200;300;400;600;700;800&display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@900&display=swap" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/088d06e81f.js" crossorigin="anonymous"></script>

    <!-- CSS -->
    <link href="bootstrap/css/bootstrap-theme.css" rel="stylesheet" />
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="styles/myStyle.css" rel="stylesheet" />

    <!--jQuery library-->
    <script src="jscripts/jquery-1.12.0.min.js"></script>
    <!-- Scripts -->
    <script src="bootstrap/js/bootstrap.js"></script>
    <script src="jscripts/myScript.js"></script>
</head>

<body>
    <!--הבר העליון עם הלוגו ותפריט הניווט-->
    <header>
        <!--קישור לדף עצמו כדי להתחיל את המשחק מחדש בלחיצה על הלוגו-->
        <a href="index.html">
            <!--הלוגו של המשחק-->
            <img id="logo" src="images/face.png" />
            <%--שם המשחק שמופיע ליד הלוגו--%>
            <p>La Casa De Cookie</p>
        </a>
        <!--סוף הקישור למשחק-->
        <!--תפריט הניווט בראש העמוד-->
        <nav>
            <ul>
                <li><a class="about">אודות</a></li>
                <li><a class="howToPlay">איך לשחק?</a></li>
                <%--<li><a class="editor" href="login.aspx">המשחקים שלי</a></li>--%>
                <li><a class="editor" href="index.html">למשחק</a></li>
            </ul>
        </nav>
        <!--סוף תפריט ניווט-->
    </header>
    <!--סוף הבר העליון-->
    <!-------------------חלון פופ אפ אודות--------------->
    <div id="aboutDiv" class="modal fade navModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <p class="bold title">אודות La Casa De Cookie</p>
                    <span class="game_icon"></span>
                    <p class="bold">אפיון ופיתוח</p>
                    <p>דולי גוטיה ומאי פריד</p>
                    <p class="bold">במסגרת הקורסים </p>
                    <ul>
                        <li>סביבות לימוד אינטראקטיביות 2</li>
                        <li>תכנות אינטראקציה ואנימציה 2</li>
                        <li>תכנות 2</li>
                    </ul>
                    <p>תש"פ 2020</p>
                    <a href="https://www.hit.ac.il/telem/overview" target="_blank">הפקולטה לטכנולוגיות למידה</a>
                    <a class="logo_link" href="https://www.hit.ac.il" target="_blank">
                        <div class="hit_logo"></div>
                    </a>
                </div>
            </div>
        </div>
    </div>
    <!---------------------סוף חלון אודות----------------------->
    <!------------------חלון פופ אפ איך לשחק-------------->
    <div id="howToPlayDiv" class="modal fade navModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <p class="bold title">איך לשחק?</p>
                    <p class="bold">עזרו לאיש העוגיה לבנות מחדש את ביתו</p>
                    <ul>
                        <li>
                            <div>
                                <img id="chosenCookie" src="images/choose_ans.png" alt="how to choose an answer" class="howToImg" />
                            </div>
                            <div class="howToTxt">
                                <p>בחרו את התשובה הנכונה באמצעות העכבר</p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <img id="correctCookie" src="images/correct_ans.png" alt="correct answer" class="howToImg" />
                            </div>
                            <div class="howToTxt">
                                <p>אם התשובה שנבחרה נכונה - יתווספו חלקים לבית</p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <img id="house" src="images/house.png" alt="built house" class="howToImg" />
                            </div>
                            <div class="howToTxt">
                                <p id="houseTxt">
                                    כאשר כל התשובות נענו בצורה נכונה - הבית יהיה בנוי
                                    <br />
                                    ואיש העוגיה יוכל לחזור ולגור בו!
                                </p>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <!----------------------סוף חלון איך לשחק---------------------->

    <form id="editGameForm" runat="server" class="container">

        <asp:LinkButton ID="returnMyGamesLinkInEdit" CssClass="returnMyGamesLink" OnClick="ReturnMyGamesLinkInEdit_Click" runat="server">
              <i class="fas fa-angle-left"></i>
            חזרה למשחקים שלי
        </asp:LinkButton>

        <div>
            <%-- אלמנס מוסתר לשמירת התשובה הנכונה --%>
            <asp:HiddenField ID="chosenRightAnswer" runat="server" Value="1" />
            <%-- התנאים שנדרשים, בהתחלה עומדים על 0 --%>
            <%--  מתעדכן בכל הזנת / מחיקת טקסט ותמונה--%>
            <%-- ביטפילד שמייצג את קיום התנאים מצד המשתמשת - באילו תנאים היא עומדת עכשיו --%>
            <asp:HiddenField ID="submitConditionsHiddenField" runat="server" Value="0" />
            <%-- בהתחלה יש 5 פאנלים שמוצגים --%>
            <%-- מתעדכן רק בכל מחיקה/הוספת תשובה --%>
            <asp:HiddenField ID="visiblePanelsHiddenField" runat="server" Value="31" />
            <%-- שמירת האיידי של השאלה שרוצות לעדכן --%>
            <asp:HiddenField ID="questionIdFromBankToEdit" runat="server" Value="edit" />
            <%-- שמירת האיידי של השאלה שרוצות למחוק --%>
            <asp:HiddenField ID="questionIdFromBankToDelete" runat="server" Value="dlt" />
            <asp:HiddenField ID="currentRowIndexHiddenField" runat="server" Value="row" />
            <%-- שמירת האינדקס של השורה והאיידי של השאלה הבאה בגלל הפופאפ --%>
            <asp:HiddenField ID="nextRowIndexAndIDHiddenField" runat="server" Value="nextRow,id" />

            <h1>
                <asp:Label ID="gameTitlelbl" runat="server" Text="עריכת התוכן למשחק:"></asp:Label>
                <asp:Label ID="gameNameLbl" runat="server" Text=""></asp:Label>
            </h1>

            <%--הפאנל של השאלה--%>
            <h2>שאלה</h2>
            <asp:Panel ID="questionPanel" runat="server">
                <%-- פאנל להזנת טקסט בשאלה--%>
                <asp:Panel ID="inputTxtQuestDiv" runat="server" class="inputTxtDiv">
                    <%-- תיבת טקסט לשאלה --%>
                    <asp:TextBox ID="questionTxtBox" runat="server" TextMode="MultiLine" CharacterForLabel="labelLimitQuest" CssClass="TextBox"></asp:TextBox>
                    <%-- לייבל המציג את מספר התווים - ההגבלה --%>
                    <asp:Label ID="labelLimitQuest" runat="server" Text="0/80" CssClass="showLimitLabel"></asp:Label>
                    <asp:Label ID="warningTxtLblQuest" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
                </asp:Panel>
                <%-- דיב להעלאת תמונה --%>
                <div class="uploadImgDiv">

                    <asp:Panel ID="showUploadImgBtnDiv0" runat="server" CssClass="showUploadImgBtnDiv">

                        <asp:Image ID="ImageHolder0" runat="server" CssClass="imgHolder" Visible="False" />

                        <%-- כפתור להעלאת תמונה --%>
                        <asp:ImageButton ID="uploadImgInQuestionBtn" ImageUrl="~/images/add_photo.png" CssClass="uploadImgBtn" runat="server" OnClientClick="openFileUploader('FileUploadImgQuest'); return false;" />

                    </asp:Panel>

                    <%-- לייבל להתרעה התמונה לא תקינה --%>
                    <asp:Label ID="warningLBLQuestion" runat="server" Text="" CssClass="warningLblImg"></asp:Label>
                    <%-- כפתור מוסתר להגדלת תמונה --%>
                    <asp:ImageButton ID="zoomImgQuest" ImageUrl="~/images/zoom.png" runat="server" CssClass="zoomImgBtn" Visible="True" data-target="#zoomImgModal" OnClick="ZoomImage_BtnClick" />
                    <%-- כפתור מוסתר למחיקת תמונה --%>
                    <asp:ImageButton ID="deleteImgQuestBtn" ImageUrl="~/images/delete.png" runat="server" CssClass="deleteImgBtn" Visible="True" OnClick="DeleteImgBtn_Click" />
                    <%-- פקד מוסתר להעלאת תמונה --%>
                    <div style="display: none">
                        <asp:FileUpload ID="FileUploadImgQuest" onchange="this.form.submit()" accept=".png,.jpg,.jpeg,.gif,.bmp,.tiff" runat="server" />
                    </div>
                </div>
            </asp:Panel>

            <%-----התשובות-----%>
            <h2>תשובות </h2>
            <asp:Label ID="ansNumOptions" runat="server" Text="(2 עד 6 תשובות אפשריות)"></asp:Label>
            <div id="answersDiv">

                <%-- תשובה 1 --%>
                <asp:Panel ID="answerPanel1" runat="server" CssClass="answerPanel">
                    <%-- פאנל לבחירת התשובה --%>
                    <asp:Panel ID="chooseAnswerPanel1" onclick="chooseAnswerlClick('1')" onmouseover="changeBorderColorOver(this.id)" onmouseout="changeBorderColorOut(this.id)" runat="server" CssClass="chooseAnswerPanel">
                    </asp:Panel>
                    <%-- לייבל אם התשובה נכונה --%>
                    <asp:Label ID="rightAnswerLabel1" CssClass="rightAnswerLabel" runat="server" Text="התשובה הנכונה"></asp:Label>
                    <%-- פאנל להזנת טקסט --%>
                    <asp:Panel ID="inputTxtAnsPanel1" runat="server" CssClass="inputTxtDiv inputTxtAnsDiv">
                        <%-- תיבת טקסט עם הלייבל המקושר אליה --%>
                        <asp:TextBox ID="TxtBoxAnswer1" runat="server" CssClass="TxtBoxAnswers TextBox CharacterCount" CharacterForLabel="labelLimitAnswer1" TextMode="MultiLine"></asp:TextBox>
                        <%-- לייבל המציג את מספר התווים - ההגבלה --%>
                        <asp:Label ID="labelLimitAnswer1" runat="server" Text="0/50" CssClass="showLimitLabel limitAnsLabel"></asp:Label>
                        <%-- לייבל כשהגענו לתקרת תווים המקסימלית --%>
                        <asp:Label ID="warningTxtLblAns1" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
                    </asp:Panel>
                    <asp:Label ID="OrAnsLbl1" runat="server" Text="או" CssClass="OrAnsLbl"></asp:Label>
                    <%-- דיב להעלאת תמונה --%>
                    <div class="uploadImgDiv uploadImgDivAns">

                        <asp:Panel ID="showUploadImgBtnDiv1" runat="server" CssClass="showUploadImgBtnDiv">

                            <asp:Image ID="ImageHolder1" runat="server" CssClass="imgHolder" Visible="False" />

                            <%-- כפתור להעלאת תמונה --%>
                            <asp:ImageButton ID="ImageAnswerBtn1" ImageUrl="~/images/add_photo.png" runat="server" OnClientClick="openFileUploader('FileUploadImgAnswer1'); return false;" CssClass="uploadImgBtn" />

                        </asp:Panel>

                        <%-- כפתור מוסתר להגדלת תמונה --%>
                        <asp:ImageButton ID="zoomImgAnsBtn1" ImageUrl="~/images/zoom.png" runat="server" Visible="True" CssClass="zoomImgBtn zoomImgAnsBtn" data-target="#zoomImgModal" OnClick="ZoomImage_BtnClick" />
                        <%-- כפתור מוסתר למחיקת תמונה --%>
                        <asp:ImageButton ID="deleteImgAnsBtn1" ImageUrl="~/images/delete.png" runat="server" Visible="True" CssClass="deleteImgBtn deleteImgAnsBtn" OnClick="DeleteImgBtn_Click" />
                        <%-- פקד פיילאפלוד מוסתר להעלאת תמונה --%>
                        <div style="display: none">
                            <asp:FileUpload ID="FileUploadImgAnswer1" onchange="this.form.submit()" accept=".png,.jpg,.jpeg,.gif,.bmp,.tiff" runat="server" />
                        </div>
                    </div>
                    <%-- לייבל להתרעה התמונה לא תקינה --%>
                    <asp:Label ID="warningLBLAns1" runat="server" Text="" CssClass="warningLblImg"></asp:Label>
                    <%-- כפתור מוסתר למחיקת כל התשובה --%>
                    <asp:ImageButton ID="deleteAnswerBtn1" ImageUrl="~/images/delete.png" OnClick="DeleteAnswerBtn_Click" runat="server" CssClass="deleteAnsBtn" Style="visibility: hidden" data-toggle="tooltip" data-placement="bottom" data-title="לחיצה תגרום לאיפוס התשובה" />
                </asp:Panel>


                <%-- תשובה 2 --%>
                <asp:Panel ID="answerPanel2" runat="server" CssClass="answerPanel">
                    <%-- פאנל לבחירת התשובה --%>
                    <asp:Panel ID="chooseAnswerPanel2" onclick="chooseAnswerlClick('2')" onmouseover="changeBorderColorOver(this.id)" onmouseout="changeBorderColorOut(this.id)" runat="server" CssClass="chooseAnswerPanel">
                    </asp:Panel>
                    <%-- לייבל אם התשובה נכונה --%>
                    <asp:Label ID="rightAnswerLabel2" CssClass="rightAnswerLabel" runat="server" Text="התשובה הנכונה"></asp:Label>
                    <%-- פאנל להזנת טקסט --%>
                    <asp:Panel ID="inputTxtAnsPanel2" runat="server" CssClass="inputTxtDiv inputTxtAnsDiv">
                        <%-- תיבת טקסט עם הלייבל המקושר אליה --%>
                        <asp:TextBox ID="TxtBoxAnswer2" runat="server" CssClass="TxtBoxAnswers TextBox CharacterCount" CharacterForLabel="labelLimitAnswer2" TextMode="MultiLine"></asp:TextBox>
                        <%-- לייבל המציג את מספר התווים - ההגבלה --%>
                        <asp:Label ID="labelLimitAnswer2" runat="server" Text="0/50" CssClass="showLimitLabel limitAnsLabel"></asp:Label>
                        <%-- לייבל כשהגענו לתקרת תווים המקסימלית --%>
                        <asp:Label ID="warningTxtLblAns2" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
                    </asp:Panel>
                    <asp:Label ID="OrAnsLbl2" runat="server" Text="או" CssClass="OrAnsLbl"></asp:Label>

                    <%-- דיב להעלאת תמונה --%>
                    <div class="uploadImgDiv uploadImgDivAns">

                        <asp:Panel ID="showUploadImgBtnDiv2" CssClass="showUploadImgBtnDiv" runat="server">

                            <asp:Image ID="ImageHolder2" runat="server" CssClass="imgHolder" Visible="False" />

                            <%-- כפתור להעלאת תמונה --%>
                            <asp:ImageButton ID="ImageAnswerBtn2" ImageUrl="~/images/add_photo.png" runat="server" OnClientClick="openFileUploader('FileUploadImgAnswer2'); return false;" CssClass="uploadImgBtn" />
                        </asp:Panel>


                        <%-- כפתור מוסתר להגדלת תמונה --%>
                        <asp:ImageButton ID="zoomImgAnsBtn2" ImageUrl="~/images/zoom.png" runat="server" Visible="True" CssClass="zoomImgBtn zoomImgAnsBtn" data-target="#zoomImgModal" OnClick="ZoomImage_BtnClick" />
                        <%-- כפתור מוסתר למחיקת תמונה --%>
                        <asp:ImageButton ID="deleteImgAnsBtn2" ImageUrl="~/images/delete.png" runat="server" Visible="True" CssClass="deleteImgBtn deleteImgAnsBtn" OnClick="DeleteImgBtn_Click" />
                        <%-- פקד פיילאפלוד מוסתר להעלאת תמונה --%>
                        <div style="display: none">
                            <asp:FileUpload ID="FileUploadImgAnswer2" onchange="this.form.submit()" accept=".png,.jpg,.jpeg,.gif,.bmp,.tiff" runat="server" />
                        </div>
                    </div>
                    <%-- לייבל להתרעה התמונה לא תקינה --%>
                    <asp:Label ID="warningLBLAns2" runat="server" Text="" CssClass="warningLblImg"></asp:Label>
                    <%-- כפתור מוסתר למחיקת כל התשובה --%>
                    <asp:ImageButton ID="deleteAnswerBtn2" ImageUrl="~/images/delete.png" runat="server" OnClick="DeleteAnswerBtn_Click" CssClass="deleteAnsBtn" Style="visibility: hidden" data-toggle="tooltip" data-placement="bottom" data-title="לחיצה תגרום לאיפוס התשובה" />
                </asp:Panel>


                <%-- תשובה 3 --%>
                <asp:Panel ID="answerPanel3" runat="server" CssClass="answerPanel">
                    <%-- פאנל לבחירת התשובה --%>
                    <asp:Panel ID="chooseAnswerPanel3" onclick="chooseAnswerlClick('3')" onmouseover="changeBorderColorOver(this.id)" onmouseout="changeBorderColorOut(this.id)" runat="server" CssClass="chooseAnswerPanel">
                    </asp:Panel>
                    <%-- לייבל אם התשובה נכונה --%>
                    <asp:Label ID="rightAnswerLabel3" CssClass="rightAnswerLabel" runat="server" Text="התשובה הנכונה"></asp:Label>
                    <%-- פאנל להזנת טקסט --%>
                    <asp:Panel ID="inputTxtAnsPanel3" runat="server" CssClass="inputTxtDiv inputTxtAnsDiv">
                        <%-- תיבת טקסט עם הלייבל המקושר אליה --%>
                        <asp:TextBox ID="TxtBoxAnswer3" runat="server" CssClass="TxtBoxAnswers TextBox CharacterCount" CharacterForLabel="labelLimitAnswer3" TextMode="MultiLine"></asp:TextBox>
                        <%-- לייבל המציג את מספר התווים - ההגבלה --%>
                        <asp:Label ID="labelLimitAnswer3" runat="server" Text="0/50" CssClass="showLimitLabel limitAnsLabel"></asp:Label>
                        <asp:Label ID="warningTxtLblAns3" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
                    </asp:Panel>
                    <asp:Label ID="OrAnsLbl3" runat="server" Text="או" CssClass="OrAnsLbl"></asp:Label>
                    <%-- דיב להעלאת תמונה --%>
                    <div class="uploadImgDiv uploadImgDivAns">

                        <asp:Panel ID="showUploadImgBtnDiv3" runat="server" CssClass="showUploadImgBtnDiv">

                            <asp:Image ID="ImageHolder3" runat="server" CssClass="imgHolder" Visible="False" />

                            <%-- כפתור להעלאת תמונה --%>
                            <asp:ImageButton ID="ImageAnswerBtn3" ImageUrl="~/images/add_photo.png" runat="server" OnClientClick="openFileUploader('FileUploadImgAnswer3'); return false;" CssClass="uploadImgBtn" />
                        </asp:Panel>

                        <%-- כפתור מוסתר להגדלת תמונה --%>
                        <asp:ImageButton ID="zoomImgAnsBtn3" ImageUrl="~/images/zoom.png" runat="server" Visible="True" CssClass="zoomImgBtn zoomImgAnsBtn" data-target="#zoomImgModal" OnClick="ZoomImage_BtnClick" />
                        <%-- כפתור מוסתר למחיקת תמונה --%>
                        <asp:ImageButton ID="deleteImgAnsBtn3" ImageUrl="~/images/delete.png" runat="server" Visible="True" CssClass="deleteImgBtn deleteImgAnsBtn" OnClick="DeleteImgBtn_Click" />
                        <%-- פקד פיילאפלוד מוסתר להעלאת תמונה --%>
                        <div style="display: none">
                            <asp:FileUpload ID="FileUploadImgAnswer3" onchange="this.form.submit()" accept=".png,.jpg,.jpeg,.gif,.bmp,.tiff" runat="server" />
                        </div>
                    </div>
                    <%-- לייבל להתרעה התמונה לא תקינה --%>
                    <asp:Label ID="warningLBLAns3" runat="server" Text="" CssClass="warningLblImg"></asp:Label>
                    <%-- כפתור למחיקת כל התשובה --%>
                    <asp:ImageButton ID="deleteAnswerBtn3" ImageUrl="~/images/delete.png" runat="server" OnClick="DeleteAnswerBtn_Click" CssClass="deleteAnsBtn" data-toggle="tooltip" data-placement="bottom" data-title="לחיצה תגרום למחיקת התשובה לצמיתות" />
                </asp:Panel>


                <%-- תשובה 4 --%>
                <asp:Panel ID="answerPanel4" runat="server" CssClass="answerPanel">
                    <%-- פאנל לבחירת התשובה --%>
                    <asp:Panel ID="chooseAnswerPanel4" onclick="chooseAnswerlClick('4')" onmouseover="changeBorderColorOver(this.id)" onmouseout="changeBorderColorOut(this.id)" runat="server" CssClass="chooseAnswerPanel">
                    </asp:Panel>
                    <%-- לייבל אם התשובה נכונה --%>
                    <asp:Label ID="rightAnswerLabel4" CssClass="rightAnswerLabel" runat="server" Text="התשובה הנכונה"></asp:Label>
                    <%-- פאנל להזנת טקסט --%>
                    <asp:Panel ID="inputTxtAnsPanel4" runat="server" CssClass="inputTxtDiv inputTxtAnsDiv">
                        <%-- תיבת טקסט עם הלייבל המקושר אליה --%>
                        <asp:TextBox ID="TxtBoxAnswer4" runat="server" CssClass="TxtBoxAnswers TextBox CharacterCount" CharacterForLabel="labelLimitAnswer4" TextMode="MultiLine"></asp:TextBox>
                        <%-- לייבל המציג את מספר התווים - ההגבלה --%>
                        <asp:Label ID="labelLimitAnswer4" runat="server" Text="0/50" CssClass="showLimitLabel limitAnsLabel"></asp:Label>
                        <asp:Label ID="warningTxtLblAns4" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
                    </asp:Panel>
                    <asp:Label ID="OrAnsLbl4" runat="server" Text="או" CssClass="OrAnsLbl"></asp:Label>
                    <%-- דיב להעלאת תמונה --%>
                    <div class="uploadImgDiv uploadImgDivAns">

                        <asp:Panel ID="showUploadImgBtnDiv4" runat="server" CssClass="showUploadImgBtnDiv">

                            <asp:Image ID="ImageHolder4" runat="server" CssClass="imgHolder" Visible="False" />

                            <%-- כפתור להעלאת תמונה --%>
                            <asp:ImageButton ID="ImageAnswerBtn4" ImageUrl="~/images/add_photo.png" runat="server" OnClientClick="openFileUploader('FileUploadImgAnswer4'); return false;" CssClass="uploadImgBtn" />
                        </asp:Panel>

                        <%-- כפתור מוסתר להגדלת תמונה --%>
                        <asp:ImageButton ID="zoomImgAnsBtn4" ImageUrl="~/images/zoom.png" runat="server" Visible="True" CssClass="zoomImgBtn zoomImgAnsBtn" data-target="#zoomImgModal" OnClick="ZoomImage_BtnClick" />
                        <%-- כפתור מוסתר למחיקת תמונה --%>
                        <asp:ImageButton ID="deleteImgAnsBtn4" ImageUrl="~/images/delete.png" runat="server" Visible="True" CssClass="deleteImgBtn deleteImgAnsBtn" OnClick="DeleteImgBtn_Click" />
                        <%-- פקד פיילאפלוד מוסתר להעלאת תמונה --%>
                        <div style="display: none">
                            <asp:FileUpload ID="FileUploadImgAnswer4" onchange="this.form.submit()" accept=".png,.jpg,.jpeg,.gif,.bmp,.tiff" runat="server" />
                        </div>
                    </div>
                    <%-- לייבל להתרעה התמונה לא תקינה --%>
                    <asp:Label ID="warningLBLAns4" runat="server" Text="" CssClass="warningLblImg"></asp:Label>
                    <%-- כפתור למחיקת כל התשובה --%>
                    <asp:ImageButton ID="deleteAnswerBtn4" ImageUrl="~/images/delete.png" runat="server" OnClick="DeleteAnswerBtn_Click" CssClass="deleteAnsBtn" data-toggle="tooltip" data-placement="bottom" data-title="לחיצה תגרום למחיקת התשובה לצמיתות" />
                </asp:Panel>


                <%-- תשובה 5 --%>
                <asp:Panel ID="answerPanel5" runat="server" CssClass="answerPanel" Visible="False">
                    <%-- פאנל לבחירת התשובה --%>
                    <asp:Panel ID="chooseAnswerPanel5" onclick="chooseAnswerlClick('5')" onmouseover="changeBorderColorOver(this.id)" onmouseout="changeBorderColorOut(this.id)" runat="server" CssClass="chooseAnswerPanel">
                    </asp:Panel>
                    <%-- לייבל אם התשובה נכונה --%>
                    <asp:Label ID="rightAnswerLabel5" CssClass="rightAnswerLabel" runat="server" Text="התשובה הנכונה"></asp:Label>
                    <%-- פאנל להזנת טקסט --%>
                    <asp:Panel ID="inputTxtAnsPanel5" runat="server" CssClass="inputTxtDiv inputTxtAnsDiv">
                        <%-- תיבת טקסט עם הלייבל המקושר אליה --%>
                        <asp:TextBox ID="TxtBoxAnswer5" runat="server" CssClass="TxtBoxAnswers TextBox CharacterCount" CharacterForLabel="labelLimitAnswer5" TextMode="MultiLine"></asp:TextBox>
                        <%-- לייבל המציג את מספר התווים - ההגבלה --%>
                        <asp:Label ID="labelLimitAnswer5" runat="server" Text="0/50" CssClass="showLimitLabel limitAnsLabel"></asp:Label>
                        <asp:Label ID="warningTxtLblAns5" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
                    </asp:Panel>
                    <asp:Label ID="OrAnsLbl5" runat="server" Text="או" CssClass="OrAnsLbl"></asp:Label>
                    <%-- דיב להעלאת תמונה --%>
                    <div class="uploadImgDiv uploadImgDivAns">

                        <asp:Panel ID="showUploadImgBtnDiv5" runat="server" CssClass="showUploadImgBtnDiv">

                            <asp:Image ID="ImageHolder5" runat="server" CssClass="imgHolder" Visible="False" />

                            <%-- כפתור להעלאת תמונה --%>
                            <asp:ImageButton ID="ImageAnswerBtn5" ImageUrl="~/images/add_photo.png" runat="server" OnClientClick="openFileUploader('FileUploadImgAnswer5'); return false;" CssClass="uploadImgBtn" />
                        </asp:Panel>

                        <%-- כפתור מוסתר להגדלת תמונה --%>
                        <asp:ImageButton ID="zoomImgAnsBtn5" ImageUrl="~/images/zoom.png" runat="server" Visible="True" CssClass="zoomImgBtn zoomImgAnsBtn" data-target="#zoomImgModal" OnClick="ZoomImage_BtnClick" />
                        <%-- כפתור מוסתר למחיקת תמונה --%>
                        <asp:ImageButton ID="deleteImgAnsBtn5" ImageUrl="~/images/delete.png" runat="server" Visible="True" CssClass="deleteImgBtn deleteImgAnsBtn" OnClick="DeleteImgBtn_Click" />
                        <%-- פקד פיילאפלוד מוסתר להעלאת תמונה --%>
                        <div style="display: none">
                            <asp:FileUpload ID="FileUploadImgAnswer5" onchange="this.form.submit()" accept=".png,.jpg,.jpeg,.gif,.bmp,.tiff" runat="server" />
                        </div>
                    </div>
                    <%-- לייבל להתרעה התמונה לא תקינה --%>
                    <asp:Label ID="warningLBLAns5" runat="server" Text="" CssClass="warningLblImg"></asp:Label>
                    <%-- כפתור למחיקת כל התשובה --%>
                    <asp:ImageButton ID="deleteAnswerBtn5" ImageUrl="~/images/delete.png" runat="server" OnClick="DeleteAnswerBtn_Click" CssClass="deleteAnsBtn" data-toggle="tooltip" data-placement="bottom" data-title="לחיצה תגרום למחיקת התשובה לצמיתות" />
                </asp:Panel>


                <%-- תשובה 6 --%>
                <asp:Panel ID="answerPanel6" runat="server" CssClass="answerPanel" Visible="False">
                    <%-- פאנל לבחירת התשובה --%>
                    <asp:Panel ID="chooseAnswerPanel6" onclick="chooseAnswerlClick('6')" onmouseover="changeBorderColorOver(this.id)" onmouseout="changeBorderColorOut(this.id)" runat="server" CssClass="chooseAnswerPanel">
                    </asp:Panel>
                    <%-- לייבל אם התשובה נכונה --%>
                    <asp:Label ID="rightAnswerLabel6" CssClass="rightAnswerLabel" runat="server" Text="התשובה הנכונה"></asp:Label>
                    <%-- פאנל להזנת טקסט --%>
                    <asp:Panel ID="inputTxtAnsPanel6" runat="server" CssClass="inputTxtDiv inputTxtAnsDiv">
                        <%-- תיבת טקסט עם הלייבל המקושר אליה --%>
                        <asp:TextBox ID="TxtBoxAnswer6" runat="server" CssClass="TxtBoxAnswers TextBox CharacterCount" CharacterForLabel="labelLimitAnswer6" TextMode="MultiLine"></asp:TextBox>
                        <%-- לייבל המציג את מספר התווים - ההגבלה --%>
                        <asp:Label ID="labelLimitAnswer6" runat="server" Text="0/50" CssClass="showLimitLabel limitAnsLabel"></asp:Label>
                        <asp:Label ID="warningTxtLblAns6" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
                    </asp:Panel>
                    <asp:Label ID="OrAnsLbl6" runat="server" Text="או" CssClass="OrAnsLbl"></asp:Label>
                    <%-- דיב להעלאת תמונה --%>
                    <div class="uploadImgDiv uploadImgDivAns">

                        <asp:Panel ID="showUploadImgBtnDiv6" runat="server" CssClass="showUploadImgBtnDiv">

                            <asp:Image ID="ImageHolder6" runat="server" CssClass="imgHolder" Visible="False" />

                            <%-- כפתור להעלאת תמונה --%>
                            <asp:ImageButton ID="ImageAnswerBtn6" ImageUrl="~/images/add_photo.png" runat="server" OnClientClick="openFileUploader('FileUploadImgAnswer6'); return false;" CssClass="uploadImgBtn" />
                        </asp:Panel>

                        <%-- כפתור מוסתר להגדלת תמונה --%>
                        <asp:ImageButton ID="zoomImgAnsBtn6" ImageUrl="~/images/zoom.png" runat="server" Visible="True" CssClass="zoomImgBtn zoomImgAnsBtn" data-target="#zoomImgModal" OnClick="ZoomImage_BtnClick" />
                        <%-- כפתור מוסתר למחיקת תמונה --%>
                        <asp:ImageButton ID="deleteImgAnsBtn6" ImageUrl="~/images/delete.png" runat="server" Visible="True" CssClass="deleteImgBtn deleteImgAnsBtn" OnClick="DeleteImgBtn_Click" />
                        <%-- פקד פיילאפלוד מוסתר להעלאת תמונה --%>
                        <div style="display: none">
                            <asp:FileUpload ID="FileUploadImgAnswer6" onchange="this.form.submit()" accept=".png,.jpg,.jpeg,.gif,.bmp,.tiff" runat="server" />
                        </div>
                    </div>
                    <%-- לייבל להתרעה התמונה לא תקינה --%>
                    <asp:Label ID="warningLBLAns6" runat="server" Text="" CssClass="warningLblImg"></asp:Label>
                    <%-- כפתור למחיקת כל התשובה --%>
                    <asp:ImageButton ID="deleteAnswerBtn6" ImageUrl="~/images/delete.png" runat="server" OnClick="DeleteAnswerBtn_Click" CssClass="deleteAnsBtn" data-toggle="tooltip" data-placement="bottom" data-title="לחיצה תגרום למחיקת התשובה לצמיתות" />
                </asp:Panel>
                <%-- פאנל להוספת תשובה חדשה --%>
                <asp:Panel ID="addAnsPanel" runat="server">
                    <asp:Button ID="addAnsBtn" CssClass="button addAns" runat="server" Text="הוספת תשובה" OnClick="AddAnsBtn_Click" />
                </asp:Panel>
            </div>

            <div id="submitQuestionDiv">
                <div>
                    <%-- כפתור ביטול --%>
                    <asp:Button ID="cancelChangesBTN" CssClass="button cancel" runat="server" Text="ביטול שינויים" OnClick="CancelChangesBTN_Click" Enabled="False" />
                    <%-- כפתור שמירה --%>
                    <div id="saveQuestTooltip" class="tooltip-wrapper disabled" data-toggle="tooltip" data-title="לא ניתן לשמור שאלה שלא עומדת במינימום הנדרש (שאלה ושתי תשובות)">
                        <asp:Button ID="saveQuestBtn" CssClass="button saveQuest" runat="server" Text="שמירת שאלה" OnClick="SubmitQuestionBtn_Click" Enabled="False" />
                    </div>
                </div>
            </div>




            <!-- Modal POPUP -->
            <%-- פופ אפ הגדלת תמונה --%>
            <div class="modal fade" id="zoomImgModal" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-body">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <asp:Image ID="modalZoomImg" runat="server" CssClass="center-block img-responsive" />
                        </div>
                    </div>
                </div>
            </div>

            <%-- פופ אפ וידוא מחיקת שאלה --%>
            <div class="modal fade alertModal" id="alertPopUpModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog alert-modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel">מחיקת שאלה</h4>
                        </div>
                        <div class="modal-body">
                            <p>
                                פעולה זו תמחק את השאלה:
                                <asp:Label ID="modalQuestToDeleteLbl" runat="server" Text="" Style="font-weight: 700"></asp:Label>
                            </p>
                            <p>
                                המכילה
                                <asp:Label ID="modalAnsCounterLbl" runat="server" Text="" Style="font-weight: 700"></asp:Label>
                                תשובות.
                            </p>
                            <p>
                                השאלה תימחק ולא ניתן יהיה לשחזר אותה. האם ברצונך להמשיך?
                            </p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="modalBtn stay" data-dismiss="modal">התחרטתי</button>
                            <asp:Button ID="deleteQuestionBtn" CssClass="modalBtn delete" runat="server" Text="למחוק את השאלה" OnClick="DeleteQuestionBtn_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <%-- פופ אפ שינויים לא יישמרו --%>
            <div class="modal fade alertModal" id="saveChangesPopUpModal" tabindex="-1" role="dialog" aria-labelledby="saveChangesModalLabel">
                <div class="modal-dialog alert-modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="saveChangesModalLabel">עריכת שאלה</h4>
                        </div>
                        <div class="modal-body">
                            <p>השינויים שביצעת לא יישמרו</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="modalBtn stay" data-dismiss="modal">להישאר כאן</button>
                            <asp:Button ID="editQuestionModalBtn" OnClick="EditQuestionModalBtn_Click" CssClass="modalBtn backMyGames" runat="server" Text="לערוך שאלה חדשה" />
                        </div>
                    </div>
                </div>
            </div>

            <%-- פופ אפ חזרה למשחקים שלי --%>
            <div class="modal fade alertModal" id="backToGamesPopUpModal" tabindex="-1" role="dialog" aria-labelledby="backToGamesModalLabel">
                <div class="modal-dialog alert-modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="backToGamesModalLabel">חזרה למשחקים שלי</h4>
                        </div>
                        <div class="modal-body">
                            <p>השינויים שביצעת לא יישמרו</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="modalBtn stay" data-dismiss="modal">להישאר כאן</button>
                            <button type="button" onclick="location.href = 'myGames.aspx';" class="modalBtn backMyGames" data-dismiss="modal">לחזור למשחקים שלי</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- END MODAL POPUP -->


            <%--  GridView  --%>
            <asp:XmlDataSource ID="XmlGameSource" runat="server" DataFile="~/XML/LaCasaDeCookie.xml"></asp:XmlDataSource>
            <asp:Panel ID="questionsGridviewPanel" runat="server">
                <h2>השאלות שלי 
                    <asp:Label ID="numberOfQuestions" runat="server" Text="(0)"></asp:Label>
                </h2>
                <asp:Panel ID="publishGamePanel" runat="server" CssClass="checkbox">
                    <asp:CheckBox ID="publishGameCB" runat="server" AutoPostBack="True" OnCheckedChanged="PublishGameCB_CheckedChanged" />
                    <asp:Label ID="minimumReqLbl" runat="server" Text="מינימום 10 שאלות לפרסום"></asp:Label>
                </asp:Panel>

                <%-- טבלה ריקה במקום הגרידוויו - כשאין שאלות --%>
                <asp:Table ID="zeroQuestionsTable" CssClass="GridView questionsGridView table table-bordered table-condensed" runat="server">
                    <asp:TableHeaderRow runat="server">
                        <asp:TableHeaderCell>שאלה</asp:TableHeaderCell>
                        <asp:TableHeaderCell>עריכה</asp:TableHeaderCell>
                        <asp:TableHeaderCell>מחיקה</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell Style="color: gray">אין פריטים להצגה</asp:TableCell>
                        <asp:TableCell></asp:TableCell>
                        <asp:TableCell></asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                <asp:GridView ID="questionsGridView" runat="server" CssClass="GridView questionsGridView table table-bordered table-hover table-condensed" AutoGenerateColumns="False" DataSourceID="XmlGameSource" OnRowCommand="QuestionsGridview_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="שאלה" ItemStyle-CssClass="questTDgridview">
                            <ItemTemplate>
                                <asp:Label ID="QuestionNameLbl" runat="server" Text='<%#Server.UrlDecode(XPathBinder.Eval(Container.DataItem, "questionText").ToString())%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="עריכה">
                            <ItemTemplate>
                                <asp:ImageButton ID="EditQuestionBtn" CssClass="gridviewBtn" runat="server" ImageUrl="~/images/edit.png" theItemId='<%#XPathBinder.Eval(Container.DataItem, "@id").ToString()%>' CommandName="editQ" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="מחיקה">
                            <ItemTemplate>
                                <asp:ImageButton ID="DeleteQuestionBtn" CssClass="gridviewBtn deleteQuestionBtn" runat="server" ImageUrl="~/images/delete.png" theItemId='<%#XPathBinder.Eval(Container.DataItem, "@id").ToString()%>' CommandName="deleteQ" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
