<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>La Casa De Cookie - התחברות</title>
    <meta name="description" content="La Casa De Cookie" />
    <meta name="keywords" content="חג המולד, אפייה, משחק, משחק לימודי, כניסה למחולל, מחולל, מחולל משחקים, התחברות, למידה, איש לחם זנגביל" />
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
</head>

<body class="snowCoverBg">
    <script>
        $(document).ready(function () {
            //הקוד שמפעיל את הפופ אפ בתפריט הניווט
            $(".about").click(function () {
                $('#aboutDiv').modal('show');
            });
            $(".howToPlay").click(function () {
                $('#howToPlayDiv').modal('show');
            });

            enableOrDisableSubmit();
            enableOrDisableTooltip();

            $('#usernameTB').on("input", function () {
                enableOrDisableSubmit();
                enableOrDisableTooltip();
            });
            $('#passwordTB').on("input", function () {
                enableOrDisableSubmit();
                enableOrDisableTooltip();
            });

            // הפעלה/ביטול טולטיפ על הכפתור
            function enableOrDisableTooltip() {
                if ($('#loginBtn').is(':disabled')) {
                    $('#loginTooltip').tooltip('enable');
                }
                else {
                    $('#loginTooltip').tooltip('disable');
                }
            }

            function enableOrDisableSubmit() {
                if ($('#usernameTB').val() != '' && $('#passwordTB').val() != '') {
                    $('#loginBtn').prop('disabled', false);
                }
                else {
                    $('#loginBtn').prop('disabled', true);

                }
            }
        });
        function changeWrongTBColor(isUserTB, isPassTB) {
            $("#usernameTB").css("border-color", isUserTB ? "red" : "rgb(229, 227, 221)");
            $("#passwordTB").css("border-color", isPassTB ? "red" : "rgb(229, 227, 221)");
        }
        function MouseOver(elem) {
            if (elem.style.borderColor == "red") { return; }
            elem.style.borderColor = "rgb(177, 172, 163)";
        }
        function MouseOut(elem) {
            if (elem.style.borderColor == "red") { return; }
            elem.style.borderColor = "rgb(229, 227, 221)";
        }

    </script>

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
                <li><a class="editor" href="login.aspx">המשחקים שלי</a></li>
            </ul>
        </nav>
        <!--סוף תפריט ניווט-->
    </header>
    <!--סוף הבר העליון-->
    <!-------------------חלון פופ אפ אודות--------------->
    <div id="aboutDiv" class="modal fade navModal" tabindex="0" role="dialog">
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
      <div id="howToPlayDiv" class="modal fade navModal" tabindex="0" role="dialog">
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

    <form id="loginForm" runat="server" class="container">
        <div id="loginDiv">
            <h1>La Casa De Cookie</h1>
            <h2>התחברות לעריכת משחקים</h2>

            <asp:Label ID="userLBL" for="usernameTB" CssClass="loginLabel" runat="server" Text="שם משתמש/ת"></asp:Label>
            <asp:TextBox ID="usernameTB" CssClass="loginTB" autocomplete="username" onmouseover="MouseOver(this);" onmouseout="MouseOut(this);" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="passLBL" for="passwordTB" CssClass="loginLabel" runat="server" Text="סיסמה"></asp:Label>
            <asp:TextBox ID="passwordTB" CssClass="loginTB" autocomplete="current-password" TextMode="Password" onmouseover="MouseOver(this);" onmouseout="MouseOut(this);" runat="server"></asp:TextBox>
            <br />

            <%-- כפתור כניסה --%>
            <div id="loginTooltip" class="tooltip-wrapper disabled" data-toggle="tooltip" data-placement="bottom" data-title="יש להזין שם וסיסמה">
                <asp:Button ID="loginBtn" CssClass="button" runat="server" Text="כניסה" OnClick="loginBtn_Click" Enabled="false" />
            </div>
            <asp:Label ID="incorrectLbl" runat="server" Text="טקסט לדוגמה"></asp:Label>
            <asp:Label ID="exmapleLoginLbl" runat="server" Text="שם משתמש/ת: admin סיסמה: telem"></asp:Label>
        </div>
    </form>
</body>
</html>
