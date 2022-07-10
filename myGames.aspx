<%@ Page Language="C#" AutoEventWireup="true" CodeFile="myGames.aspx.cs" Inherits="myGames" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>המשחקים שלי - La Casa De Cookie</title>
    <meta name="description" content="La Casa De Cookie" />
    <meta name="keywords" content="חג המולד, אפייה, משחק, משחק לימודי, מחולל, מחולל משחקים, המשחקים שלי, למידה, איש לחם זנגביל" />
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

    <form id="myGamesForm" runat="server" class="container">
        <div id="main">

            <h1>המשחקים שלי</h1>
            <%-- פאנל להוספת משחק חדש --%>
            <asp:Panel ID="addNewGamePanel" runat="server">
                <%-- תיבת טקסט --%>
                <asp:Label ID="newGameNameLBL" runat="server" Text="משחק חדש: "></asp:Label>
                <asp:TextBox ID="addNewNameTB" runat="server" CharacterForLabel="labelLimitNewGame" placeholder="דוגמה: דגלים"></asp:TextBox>
                <%-- לייבל המציג את הגבלת התווים --%>
                <asp:Label ID="labelLimitNewGame" runat="server" Text="0/40" CssClass="showLimitLabel"></asp:Label>
                <asp:Label ID="warningTxtLblNewGame" runat="server" Text="" CssClass="warningLblTxtBox"></asp:Label>
                <%-- כפתור ליצירת משחק חדש --%>
                <div id="newGameTootip" class="tooltip-wrapper disabled" data-toggle="tooltip" data-placement="left" data-title="יש לרשום לפחות תו אחד כדי ליצור משחק">
                    <asp:ImageButton ID="addGameBTN" CssClass="button" runat="server" ImageUrl='~/images/plus.png' OnClick="AddGame_Click" Enabled="False" />
                </div>
            </asp:Panel>

            <asp:Panel ID="myGamesGridviewPanel" runat="server">

                <asp:XmlDataSource ID="GamesXmlDataSource" runat="server" DataFile="~/XML/LaCasaDeCookie.xml" XPath="/LaCasaDeCookie/game"></asp:XmlDataSource>

                <%-- טבלה ריקה במקום הגרידוויו - כשאין משחקים --%>
                <asp:Panel ID="zeroGamesTable" runat="server" Visible="false">
                    <table class="GridView table table-bordered table-condensed">
                        <tr>
                            <th>שם המשחק</th>
                            <th>קוד המשחק</th>
                            <th>מספר שאלות</th>
                            <th>הגדרות</th>
                            <th>עריכה</th>
                            <th>פרסום</th>
                            <th>מחיקה</th>
                        </tr>
                        <tr>
                            <td colspan="7" style="color: gray">עדיין לא יצרת משחקים</td>
                        </tr>
                    </table>
                </asp:Panel>

                <%-- הגרידוויו של כל המשחקים --%>
                <asp:GridView ID="myGamesGridView" runat="server" AutoGenerateColumns="False" DataSourceID="GamesXmlDataSource" OnRowCommand="myGamesGridView_RowCommand" CssClass="GridView table table-bordered table-hover table-condensed" OnRowDataBound="myGamesGridView_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="שם המשחק">
                            <ItemTemplate>
                                <asp:Label ID="gameName" runat="server" Text='<%#Server.UrlDecode(XPathBinder.Eval(Container.DataItem, "gameName").ToString())%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="קוד המשחק">
                            <ItemTemplate>
                                <asp:Label ID="gameCode" runat="server" Text='<%#XPathBinder.Eval(Container.DataItem, "@gameCode").ToString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="מספר שאלות">
                            <ItemTemplate>
                                <asp:Label ID="numberOfQuestions" runat="server" Text='<%#XPathBinder.Eval(Container.DataItem, "count(gameQuestions/question)").ToString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="הגדרות">
                            <ItemTemplate>
                                <asp:ImageButton ID="editGameBtn" CssClass="gridviewBtn" runat="server" ImageUrl="~/images/settings.png" CommandName="editSettings" theGameId='<%#XPathBinder.Eval(Container.DataItem, "@gameCode").ToString()%>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="עריכה">
                            <ItemTemplate>
                                <asp:ImageButton ID="editQuestionsBtn" CssClass="gridviewBtn" runat="server" ImageUrl="~/images/edit.png" CommandName="editQuestions" theGameId='<%#XPathBinder.Eval(Container.DataItem, "@gameCode").ToString()%>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="פרסום">
                            <ItemTemplate>
                                <asp:Panel ID="publishCBTooltip" CssClass="tooltip-wrapper disabled" runat="server" data-toggle="tooltip" data-placement="bottom" data-html="true" data-title="יש להוסיף לפחות 10 שאלות <br/> כדי לפרסם את המשחק">
                                    <asp:ImageButton ID="isPublishFalse" theGameId='<%#XPathBinder.Eval(Container.DataItem, "@gameCode").ToString()%>' ImageUrl="~/images/checkbox_empty.png" CssClass="gridviewBtn button" runat="server" Visible='<%#!Convert.ToBoolean(XPathBinder.Eval(Container.DataItem, "@isPublish").ToString())%>' OnClick="IsPublishCB_CheckedChanged" Enabled='<%#(XPathBinder.Eval(Container.DataItem, "count(gameQuestions/question) >= 10"))%>' />
                                </asp:Panel>

                                <asp:ImageButton ID="isPublishTrue" theGameId='<%#XPathBinder.Eval(Container.DataItem, "@gameCode").ToString()%>' ImageUrl="~/images/checkbox_full.png" CssClass="gridviewBtn button" runat="server" Visible='<%#Convert.ToBoolean(XPathBinder.Eval(Container.DataItem, "@isPublish").ToString())%>' OnClick="IsPublishCB_CheckedChanged" Enabled='<%#(XPathBinder.Eval(Container.DataItem, "count(gameQuestions/question) >= 10"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="מחיקה">
                            <ItemTemplate>
                                <asp:ImageButton ID="deleteGameBtn" CssClass="gridviewBtn deleteGameBtn" runat="server" ImageUrl="~/images/delete.png" CommandName="deleteGame" theGameId='<%#XPathBinder.Eval(Container.DataItem, "@gameCode").ToString()%>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </asp:Panel>


            <%-- פופ אפ וידוא מחיקת משחק --%>
            <div class="modal fade alertModal" id="alertPopUpModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog alert-modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel">מחיקת משחק</h4>
                        </div>
                        <div class="modal-body">
                            <p>
                                פעולה זו תמחק את המשחק:
                                <asp:Label ID="modalGameToDeleteLbl" runat="server" Text="" Style="font-weight: 700"></asp:Label>
                            </p>
                            <p>
                                המכיל
                                <asp:Label ID="modalQuestCounterLbl" runat="server" Text="" Style="font-weight: 700"></asp:Label>
                                שאלות
                            </p>
                            <p>
                                המשחק יימחק ולא ניתן יהיה לשחזר אותו. האם ברצונך להמשיך?
                            </p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="modalBtn stay" data-dismiss="modal">התחרטתי</button>
                            <asp:Button ID="deleteGameBtn" CssClass="modalBtn delete" runat="server" Text="למחוק את המשחק" OnClick="DeleteGame" />
                        </div>
                    </div>
                </div>
            </div>


        </div>
    </form>
</body>
</html>
