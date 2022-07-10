<%@ Page Language="C#" AutoEventWireup="true" CodeFile="settings.aspx.cs" Inherits="settings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>הגדרות משחק - La Casa De Cookie</title>
    <meta name="description" content="La Casa De Cookie" />
    <meta name="keywords" content="חג המולד, אפייה, הגדרות, הגדרות משחק, מחולל, מחולל משחקים, משחק, משחק לימודי, למידה, איש לחם זנגביל" />
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
                                    כאשר כל התשובות נענו בצורה נכונה - הבית יהיה בנוי <br />
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

    <form id="settingsForm" runat="server" class="container">


        <asp:LinkButton ID="returnMyGamesLinkInSettings" CssClass="returnMyGamesLink" OnClick="ReturnMyGamesLinkInSettings_Click" runat="server">
              <i class="fas fa-angle-left"></i>
            חזרה למשחקים שלי
        </asp:LinkButton>


        <div class="gridDisplay">
            <h1>הגדרות כלליות</h1>
            <h2>שם המשחק</h2>
            <%-- פאנל להזנת שם המשחק --%>
            <asp:Panel ID="inputTxtGameName" runat="server" class="inputTxtDiv">
                <%-- תיבת טקסט --%>
                <asp:TextBox ID="gameNameTxtBox" runat="server" TextMode="MultiLine" CharacterForLabel="labelLimitGameName" CssClass="TextBox"></asp:TextBox>
                <%-- לייבל המציג את הגבלת התווים --%>
                <asp:Label ID="labelLimitGameName" runat="server" Text="0/40" CssClass="showLimitLabel"></asp:Label>
                <asp:Label ID="warningTxtLblGameName" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
            </asp:Panel>

            <h2>זמן לשאלה</h2>
            <asp:Panel ID="AllTimePerQuestPanel" runat="server">
                <asp:RadioButtonList ID="timePerQuestRB" runat="server">
                    <asp:ListItem Value="30">30 שניות</asp:ListItem>
                    <asp:ListItem Value="60">60 שניות</asp:ListItem>
                    <asp:ListItem Value="90">90 שניות</asp:ListItem>
                    <asp:ListItem Value="0">ללא הגבלת זמן</asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>
        </div>

        <%-- כפתור שמירה וחזרה למשחקים שלי --%>
        <div id="saveSettingsTootip" class="tooltip-wrapper disabled" data-toggle="tooltip" data-placement="bottom" data-title="לא ניתן לשמור משחק ללא שם">
            <asp:Button ID="saveSettingsBtn" CssClass="button saveBackBtn" runat="server" Text="שמירה וחזרה למשחקים שלי" OnClick="SaveSettingsBtn_Click" />
        </div>

        <%-- כפתור שמירת ומעבר לעריכת שאלות --%>
        <div id="saveSettingsAndEditTootip" class="tooltip-wrapper disabled" data-toggle="tooltip" data-placement="bottom" data-title="לא ניתן לשמור משחק ללא שם">
            <asp:Button ID="saveSettingsAndEditBtn" CssClass="button saveEditBtn" runat="server" Text="שמירה ומעבר לעריכת שאלות" OnClick="SaveSettingsAndEditBtn_Click" />
        </div>

        <%-- פופ אפ חזרה למשחקים שלי --%>
        <div class="modal fade alertModal" id="backToGamesPopUpModal" tabindex="-1" role="dialog" aria-labelledby="saveChangesModalLabel">
            <div class="modal-dialog alert-modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="saveChangesModalLabel">חזרה למשחקים שלי</h4>
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
        <div id="settingsImage"></div>
    </form>
</body>
</html>
