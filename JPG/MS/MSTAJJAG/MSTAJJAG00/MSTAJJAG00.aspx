<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MSTAJJAG00.aspx.vb" Inherits="MSTAJJAG00.MSTAJJAG00" EnableSessionState="ReadOnly" enableViewState="False" validateRequest="false" %>
<%@ Register TagPrefix="cc1" Namespace="JPG.Common.Controls" Assembly="COCNTRLL00" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>MSTAJJAG00</title>
		<% 
'***********************************************
' 担当者マスタ  メイン画面
'***********************************************
' 変更履歴
' 2011/04/14 T.Watabe 自動FAX複数メール送信に対応する為に、JA連絡担当者毎にメールアドレスを設定できるように変更
' 2011/06/21 T.Watabe 自動FAXメール送信のメールアドレス最大桁数を30桁から50桁に変更
' 2011/11/08 H.Uema   JA毎の注意事項を登録できるように改修
' 2011/12/01 H.Uema   FAX不要初期値設定をできるように改修
' 2012/03/30 W.GANEKO スポットFAX用アドレス及びﾊﾟｽﾜｰﾄﾞ追加
' 2013/05/17 T.Ono 顧客単位での登録機能追加		    
' 2015/11    T.ONO MSTAGJAGに移行。当プログラムは未使用
%>
		<!-- ▼▼▼ 2011/11/08 H.Uema ADD タブ対応 ▼▼▼ -->
		<style type="text/css">
			#tab { PADDING-LEFT: 0px; MARGIN-BOTTOM: 1em; MARGIN-LEFT: 0px; OVERFLOW: hidden; BORDER-BOTTOM: #333 2px solid; HEIGHT: 1.5em }
			#tab LI { FLOAT: left; WIDTH: 150px; HEIGHT: 1.5em }
			#tab LI A { BORDER-RIGHT: #ccc 1px solid; BORDER-TOP: #ccc 1px solid; DISPLAY: block; BORDER-LEFT: #ccc 5px solid; WIDTH: 150px; COLOR: #777; BORDER-BOTTOM: 0px; HEIGHT: 1.5em; TEXT-ALIGN: center }
			#tab LI A:hover { BORDER-LEFT-COLOR: #333; BORDER-BOTTOM-COLOR: #333; COLOR: #000; BORDER-TOP-COLOR: #333; BORDER-RIGHT-COLOR: #333 }
			#tab LI.present A { BORDER-LEFT-COLOR: #333; BORDER-BOTTOM-COLOR: #333; COLOR: #000; BORDER-TOP-COLOR: #333; BORDER-RIGHT-COLOR: #333 }
			#page1 { PADDING-TOP: 0em }
			#page2 { PADDING-TOP: 0em }
			/* 2013/06/19 mod T.Ono */
			/*.preview { OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100% }*/
			.preview { OVERFLOW: hidden; WIDTH: 100%; HEIGHT: 100% }
			.style1
            {
                width: 342px;
            }
			.style2
            {
                width: 150px;
            }
			</style>
		<!-- ▲▲▲ 2011/11/08 H.Uema ADD タブ対応 ▲▲▲ -->
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server" enctype="multipart/form-data">
			<asp:label id="lblScript" runat="server"></asp:label>
			<INPUT id="hdnMyAspx" type="hidden" name="hdnMyAspx" runat="server"> <INPUT id="hdnBackUrl" type="hidden" name="hdnBackUrl" runat="server">
			<INPUT id="hdnPopcrtl" type="hidden" name="hdnPopcrtl" runat="server">
			<br>
			<table cellSpacing="0" cellPadding="0" width="100%">
				<tr>
					<td class="WW">
						<table cellSpacing="2" cellPadding="0" width="900">
							<tr>
								<td width="*"><input class="bt-JIK" id="btnSelect" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnSelect_onclick();"
										tabIndex="1001" type="button" value="検索" name="btnSelect" runat="server">
								</td>
								<td width="300">&nbsp;</td>
								<td width="220"><input class="bt-JIK" id="btnUpdate" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnUpdate_onclick();"
										tabIndex="1002" type="button" value="登録" name="btnUpdate" runat="server">
								</td>
								<td width="220"><input class="bt-JIK" id="btnDelete" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnDelete_onclick();"
										tabIndex="1003" type="button" value="削除" name="btnDelete" runat="server">
								</td>
								<td width="70"><input class="bt-JIK" id="btnClear" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnClear_onclick();"
										tabIndex="1005" type="button" value="取消" name="btnClear" runat="server">
								</td>
								<td width="30">&nbsp;</td>
								<td align="right" width="80"><input class="bt-JIK" id="btnExit" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnExit_onclick();"
										tabIndex="1006" type="button" value="終了" name="btnExit" runat="server">
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td height="5"></td>
				</tr>
				<tr>
					<td vAlign="bottom">
						<table cellSpacing="0" cellPadding="0" width="900">
							<tr>
								<td width="20"></td>
								<td vAlign="middle" width="710">
									<table cellSpacing="0" cellPadding="2" width="100%">
										<tr>
											<%-- <td class="TITLE" vAlign="middle">JA担当者マスタ</td> 2015/02/17 T.Ono mod 2014改善開発 No15--%>
											<td class="TITLE" vAlign="middle">JA担当者・報告先マスタ</td>
										</tr>
									</table>
								</td>
								<td vAlign="middle" align="right" width="170">作成日：<asp:textbox id="txtAYMD" tabIndex="-1" runat="server" CssClass="c-RO" Width="72px" BorderStyle="Solid"
										BorderWidth="1px"></asp:textbox><br>
									更新日：<asp:textbox id="txtUYMD" tabIndex="-1" runat="server" CssClass="c-RO" Width="72px" BorderStyle="Solid"
										BorderWidth="1px"></asp:textbox>
								</td>
							</tr>
						</table>
						<INPUT id="hdnKBN" type="hidden" name="hdnKBN" runat="server"> <INPUT id="hdnTIME" type="hidden" name="hdnTIME" runat="server">
						<INPUT id="hdnEDT_DT_1" type="hidden" name="hdnEDT_DT_1" runat="server"> <INPUT id="hdnEDT_DT_2" type="hidden" name="hdnEDT_DT_2" runat="server">
						<INPUT id="hdnEDT_DT_3" type="hidden" name="hdnEDT_DT_3" runat="server"> <INPUT id="hdnEDT_DT_4" type="hidden" name="hdnEDT_DT_4" runat="server">
						<INPUT id="hdnEDT_DT_5" type="hidden" name="hdnEDT_DT_5" runat="server"> <INPUT id="hdnEDT_DT_6" type="hidden" name="hdnEDT_DT_6" runat="server">
						<INPUT id="hdnEDT_DT_7" type="hidden" name="hdnEDT_DT_7" runat="server"> <INPUT id="hdnEDT_DT_8" type="hidden" name="hdnEDT_DT_8" runat="server">
						<INPUT id="hdnEDT_DT_9" type="hidden" name="hdnEDT_DT_9" runat="server"> <INPUT id="hdnEDT_DT_10" type="hidden" name="hdnEDT_DT_10" runat="server">
						<!-- ▼▼▼ 2010/04/12 T.Watabe add ▼▼▼ -->
						<INPUT id="hdnEDT_DT_11" type="hidden" name="hdnEDT_DT_11" runat="server"> <INPUT id="hdnEDT_DT_12" type="hidden" name="hdnEDT_DT_12" runat="server">
						<INPUT id="hdnEDT_DT_13" type="hidden" name="hdnEDT_DT_13" runat="server"> <INPUT id="hdnEDT_DT_14" type="hidden" name="hdnEDT_DT_14" runat="server">
						<INPUT id="hdnEDT_DT_15" type="hidden" name="hdnEDT_DT_15" runat="server"> <INPUT id="hdnEDT_DT_16" type="hidden" name="hdnEDT_DT_16" runat="server">
						<INPUT id="hdnEDT_DT_17" type="hidden" name="hdnEDT_DT_17" runat="server"> <INPUT id="hdnEDT_DT_18" type="hidden" name="hdnEDT_DT_18" runat="server">
						<INPUT id="hdnEDT_DT_19" type="hidden" name="hdnEDT_DT_19" runat="server"> <INPUT id="hdnEDT_DT_20" type="hidden" name="hdnEDT_DT_20" runat="server">
						<INPUT id="hdnEDT_DT_21" type="hidden" name="hdnEDT_DT_21" runat="server"> <INPUT id="hdnEDT_DT_22" type="hidden" name="hdnEDT_DT_22" runat="server">
						<INPUT id="hdnEDT_DT_23" type="hidden" name="hdnEDT_DT_23" runat="server"> <INPUT id="hdnEDT_DT_24" type="hidden" name="hdnEDT_DT_24" runat="server">
						<INPUT id="hdnEDT_DT_25" type="hidden" name="hdnEDT_DT_25" runat="server"> <INPUT id="hdnEDT_DT_26" type="hidden" name="hdnEDT_DT_26" runat="server">
						<INPUT id="hdnEDT_DT_27" type="hidden" name="hdnEDT_DT_27" runat="server"> <INPUT id="hdnEDT_DT_28" type="hidden" name="hdnEDT_DT_28" runat="server">
						<INPUT id="hdnEDT_DT_29" type="hidden" name="hdnEDT_DT_29" runat="server"> <INPUT id="hdnEDT_DT_30" type="hidden" name="hdnEDT_DT_30" runat="server">
						<!-- ▲▲▲ 2010/04/12 T.Watabe add ▲▲▲ -->
					</td>
				</tr>
			</table>
			<hr>
			<!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
            <!-- <table cellSpacing="1" cellPadding="5" width="980"> -->
            <table cellSpacing="1" cellPadding="5" width="1150">
				<tr>
					<td width="10">&nbsp;</td>
					<td width="100" class="TXTKY" align="right">クライアントコード&nbsp;&nbsp;</td>
                    <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
					<%--<td width="350"><asp:textbox id="txtKURACD" tabIndex="-1" runat="server" CssClass="c-rNM" Width="250px" BorderStyle="Solid"
							BorderWidth="1px" ReadOnly="True"></asp:textbox><input class="bt-KS" id="btnKURACD" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('1');"
							tabIndex="2" type="button" value="▼" name="btnKURACD" runat="server"> <INPUT id="hdnKURACD" type="hidden" name="hdnKURACD" runat="server">
						<INPUT id="hdnKURACD_MOTO" type="hidden" name="hdnKURACD_MOTO" runat="server">
					</td>--%>
                    <td width="350"><asp:textbox id="txtKURACD" tabIndex="-1" runat="server" CssClass="c-rNM" Width="250px" BorderStyle="Solid"
							BorderWidth="1px"></asp:textbox><input class="bt-KS" id="btnKURACD" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('1');"
							tabIndex="1" type="button" value="▼" name="btnKURACD" runat="server"> <INPUT id="hdnKURACD" type="hidden" name="hdnKURACD" runat="server">
						<INPUT id="hdnKURACD_MOTO" type="hidden" name="hdnKURACD_MOTO" runat="server">
					</td>
					<td colspan="2" rowspan="3" class="style1">
						<div style="BORDER-RIGHT:black 1px dotted; BORDER-TOP:black 1px dotted; Z-INDEX:1; BORDER-LEFT:black 1px dotted; WIDTH:350px; BORDER-BOTTOM:black 1px dotted; BACKGROUND-COLOR:#edffdb">
							<!-- form id="Form2"  enctype="multipart/form-data" -->
							<div>
								ファイル登録 <INPUT type="file" ID="FileUpload1" name="FileUpload1" width="60px">
								<asp:Button ID="btnFileUpload" runat="server" Text="upload" />
							</div>
							<br>
                            <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
							<%--<asp:textbox id="hdnFileKey" Visible="False" runat="server"></asp:textbox>
							<asp:textbox id="txtFileName1" tabIndex="-1" runat="server" CssClass="c-rNM" Width="250px" BorderStyle="Solid"
								BorderWidth="1px" ReadOnly="True"></asp:textbox><asp:button ID="btnFileDownload1" runat="server" text="開く" /><asp:button ID="btnFileDelete1" runat="server" text="削除" /><br>
							<asp:textbox id="txtFileName2" tabIndex="-1" runat="server" CssClass="c-rNM" Width="250px" BorderStyle="Solid"
								BorderWidth="1px" ReadOnly="True"></asp:textbox><asp:button ID="btnFileDownload2" runat="server" text="開く" /><asp:button ID="btnFileDelete2" runat="server" text="削除" />--%>
                            <asp:textbox id="hdnFileKey" Visible="False" runat="server"></asp:textbox>
							<asp:textbox id="txtFileName1" tabIndex="-1" runat="server" CssClass="c-rNM" Width="250px" BorderStyle="Solid"
								BorderWidth="1px"></asp:textbox><asp:button ID="btnFileDownload1" runat="server" text="開く" /><asp:button ID="btnFileDelete1" runat="server" text="削除" /><br>
							<asp:textbox id="txtFileName2" tabIndex="-1" runat="server" CssClass="c-rNM" Width="250px" BorderStyle="Solid"
								BorderWidth="1px"></asp:textbox><asp:button ID="btnFileDownload2" runat="server" text="開く" /><asp:button ID="btnFileDelete2" runat="server" text="削除" />
							<!-- /form -->
						</div>
					</td>
                <!-- ▼▼▼ 2013/05/17 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
					<!-- ▼▼▼ 2011/12/01 H.Uema ADD FAX不要初期値設定ラジオボタン追加 ▼▼▼ -->
					<td rowspan="3">
						<%-- 2013/12/13 T.Ono mod 監視改善2013　id追加
                        <table cellSpacing="1" cellPadding="1" class="W"> --%>
                        <table id="tblFAX_Default" cellSpacing="1" cellPadding="1" class="W">
							<tr>
                                <%-- 2013/11/28 T.Ono mod 監視改善2013 --%>
								<%-- <td colspan="2">（ FAX不要初期値 ）</td> --%>
								<td colspan="3">（ 報告要・不要初期値 ）</td>
							</tr>
							<tr>
								<td align="right">JA&nbsp;&nbsp;</td>
								<td vAlign="middle"><input id="rdoFAXJA1" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" CHECKED value="1" name="rdoFAXJA" runat="server"></td>
								<%-- 2013/11/28 T.Ono mod 監視改善2013 --%>
                                <%-- <td vAlign="middle"><label for="rdoFAXJA1">未設定&nbsp;&nbsp;&nbsp;&nbsp;</label></td> --%>
                                <td vAlign="middle"><label for="rdoFAXJA1">上位設定参照&nbsp;</label></td>
								<td vAlign="middle"><input id="rdoFAXJA2" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" value="2" name="rdoFAXJA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXJA2">必要&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
								<td vAlign="middle"><input id="rdoFAXJA3" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" value="3" name="rdoFAXJA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXJA3">不要&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                <input id="hdnFAXJA_MOTO" type="hidden" name="hdnFAXJA_MOTO" value="9" runat="server"/><!-- 2015/02/18 T.Ono add 2014改善開発 No15 -->
                                </td>
							</tr>
							<tr>
								<td align="right">ｸﾗｲｱﾝﾄ&nbsp;&nbsp;</td>
								<td vAlign="middle"><input id="rdoFAXKURA1" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" CHECKED value="1" name="rdoFAXKURA" runat="server"></td>
								<%-- 2013/11/28 T.Ono mod 監視改善2013 --%>
                                <%-- <td vAlign="middle"><label for="rdoFAXKURA1">未設定&nbsp;&nbsp;&nbsp;&nbsp;</label></td> --%>
                                <td vAlign="middle"><label for="rdoFAXKURA1">上位設定参照&nbsp;</label></td>
								<td vAlign="middle"><input id="rdoFAXKURA2" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" value="2" name="rdoFAXKURA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXKURA2">必要&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
								<td vAlign="middle"><input id="rdoFAXKURA3" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" value="3" name="rdoFAXKURA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXKURA3">不要&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                <input id="hdnFAXKURA_MOTO" type="hidden" name="hdnFAXKURA_MOTO" value="9" runat="server"/><!-- 2015/02/18 T.Ono add 2014改善開発 No15 -->
                                </td>
							</tr>
						</table>
					</td>
					<!-- ▲▲▲ 2011/12/01 H.Uema ADD FAX不要初期値設定ラジオボタン追加 ▲▲▲ -->
                    <!-- ▲▲▲ 2013/05/17 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td class="TXTKY" align="right">ＪＡ支所コード&nbsp;&nbsp;</td>
                    <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
					<%--<td><asp:textbox id="txtCODE" tabIndex="-1" runat="server" CssClass="c-rNM" Width="300px" BorderStyle="Solid"
							BorderWidth="1px" ReadOnly="True"></asp:textbox><input class="bt-KS" id="btnCODECD" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('2');"
							tabIndex="3" type="button" value="▼" name="btnCODECD" runat="server"> <INPUT id="hdnCODE" type="hidden" name="hdnCODE" runat="server">
						<INPUT id="hdnCODE_MOTO" type="hidden" name="hdnCODE_MOTO" runat="server">
					</td>--%>
                    <td><asp:textbox id="txtCODE" tabIndex="-1" runat="server" CssClass="c-rNM" Width="300px" BorderStyle="Solid"
							BorderWidth="1px"></asp:textbox><input class="bt-KS" id="btnCODECD" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('2');"
							tabIndex="2" type="button" value="▼" name="btnCODECD" runat="server"> <INPUT id="hdnCODE" type="hidden" name="hdnCODE" runat="server">
						<INPUT id="hdnCODE_MOTO" type="hidden" name="hdnCODE_MOTO" runat="server">
					</td>
				</tr>
                <!-- ▼▼▼ 2013/05/17 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
    			<tr>
					<td></td>
					<td class="TXTKY" align="right" >お客様コード&nbsp;&nbsp;</td>
                    <td>
                        <asp:textbox id="txtUSER_CD_FROM" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
                        tabIndex="3" runat="server" CssClass="c-f" Width="150px"  MaxLength="20" BorderStyle="Solid" BorderWidth="1px"></asp:textbox>
                        <INPUT id="hdnUSER_CD_FROM_MOTO" type="hidden" name="hdnUSER_CD_FROM_MOTO" runat="server">
                        〜
                        <asp:textbox id="txtUSER_CD_TO" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
                        tabIndex="3" runat="server" CssClass="c-f" Width="150px"  MaxLength="20" BorderStyle="Solid" BorderWidth="1px"></asp:textbox>
                        <INPUT id="hdnUSER_CD_TO_MOTO" type="hidden" name="hdnUSER_CD_TO_MOTO" runat="server">
                        <input id="hdnUSER_CD_TEMP" type="hidden" name="hdnUSER_CD_TEMP" runat="server"> <!-- ﾎﾟｯﾌﾟｱｯﾌﾟからの戻り値一時保管用 2015/02/18 T.Ono add 2014改善開発 No15 -->
					</td>
				</tr>
				<tr>
					<td></td>
					<td align="right">お客様氏名&nbsp;&nbsp;</td>
                    <td>
                        <asp:textbox id="txtUSER_NM" tabIndex="-1" runat="server" CssClass="c-rNM" Width="300px" BorderStyle="Solid" BorderWidth="1px"></asp:textbox>
					</td>
                    <td colspan="3" align="right"><span id="spS1"><a href="MSTAJJAG00_K.pdf" tabIndex="6" target="_blank"><img src="../../../Script/icon_pdf.gif" border="0">マニュアル&nbsp;&nbsp;</a></span>
                    <span id="spS2"><a href="MSTAJJAG00_O.pdf" tabIndex="6" target="_blank"><img src="../../../Script/icon_pdf.gif" border="0">マニュアル&nbsp;&nbsp;</a></span></td>
				</tr>
                <%-- 2015/02/17 T.Ono add 2014改善開発 No15 START --%>
                <tr>
                    <td></td>
					<td align="right">登録済み選択&nbsp;&nbsp;</td>
                    <td><input class="bt-RNW" id="btnTOUROKUZUMI" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('3');"
							tabindex="4" type="button" value="登録済み一覧" name="btnTOUROKUZUMI" runat="server"/></td>
                    <td></td>                
                </tr>         
                <%-- 2015/02/17 T.Ono add 2014改善開発 No15 END --%>
                <%--2013/05/17 T.Ono del FAX不要初期値設定ラジオボタン移動　            
				<tr>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
					<!-- ▼▼▼ 2011/12/01 H.Uema ADD FAX不要初期値設定ラジオボタン追加 ▼▼▼ -->
					<td>
						<table cellSpacing="1" cellPadding="1" class="W">
							<tr>
								<td>（ FAX不要初期値 ）</td>
							</tr>
							<tr>
								<td align="right">JA&nbsp;&nbsp;</td>
								<td vAlign="middle"><input id="rdoFAXJA1" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="4" type="radio" CHECKED value="1" name="rdoFAXJA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXJA1">未設定&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
								<td vAlign="middle"><input id="rdoFAXJA2" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="4" type="radio" value="2" name="rdoFAXJA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXJA2">必要&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
								<td vAlign="middle"><input id="rdoFAXJA3" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="4" type="radio" value="3" name="rdoFAXJA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXJA3">不要&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
							</tr>
							<tr>
								<td align="right">ｸﾗｲｱﾝﾄ&nbsp;&nbsp;</td>
								<td vAlign="middle"><input id="rdoFAXKURA1" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" CHECKED value="1" name="rdoFAXKURA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXKURA1">未設定&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
								<td vAlign="middle"><input id="rdoFAXKURA2" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" value="2" name="rdoFAXKURA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXKURA2">必要&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
								<td vAlign="middle"><input id="rdoFAXKURA3" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										tabIndex="5" type="radio" value="3" name="rdoFAXKURA" runat="server"></td>
								<td vAlign="middle"><label for="rdoFAXKURA3">不要&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
							</tr>
						</table>
					</td>
					<!-- ▲▲▲ 2011/12/01 H.Uema ADD FAX不要初期値設定ラジオボタン追加 ▲▲▲ -->
				</tr>--%>
                <!-- ▲▲▲ 2013/05/17 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
			</table>
			<INPUT id="hdnKensaku" type="hidden" name="hdnKensaku" runat="server"> 
			<!-- ▼▼▼ 2011/11/08 H.Uema ADD タブ対応 ▼▼▼ -->
            <input id="hdntab" type="hidden" name="hdntab" runat="server"/> <!-- 2013/07/09 T.Ono add -->
            <ul id="tab">
				<li class="present">
					<a href="#page1">一覧</a></li>
				<li>
					<!-- <a href="#page2">JA注意事項</a></li> -->
                    <!-- <a href="#page2"><INPUT id="txtPageNM2" type="text" name="txtPageNM2" value="JA注意事項" runat="server"></a></li> -->
                    <!--　<a href="#page2" ><asp:label id="lblPageNM2" Runat="server" Text="JA注意事項"></asp:label></a></li> 2013/07/26 T.Ono mof -->
                    <a href="#page2" ><asp:label id="Label1" Runat="server" Text="注意事項"></asp:label></a></li>
			</ul>
			<!-- ▲▲▲ 2011/11/08 H.Uema ADD タブ対応 ▲▲▲ -->
			<div id="page1">
                <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                <div id="hyoji">
                    <table>
                        <tr>
                            <%-- <td Width="630px"></td> 2015/02/17 T.Ono mod 2014改善開発 No15 START --%>
                            <td width="630px"><input class="bt-RNW" id="btnICHIRAN_COPY" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="fncIchiran_Copy()"
							    tabindex="7" type="button" value="コピー" name="btnICHIRAN_COPY" runat="server"/>
                                <input class="bt-RNW" id="btnICHIRAN_PASTE" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="fncIchiran_Paste()"
							    tabindex="8" type="button" value="ペースト" name="btnICHIRAN_PASTE" runat="server"/>
                                <input class="bt-RNW" id="btnICHIRAN_CLEAR" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="fncIchiran_Clear()"
							    tabindex="9" type="button" value="クリア" name="btnICHIRAN_CLEAR" runat="server"/></td>
                            <%-- 2015/02/17 T.Ono mod 2014改善開発 No15 END --%>
                            <td>
                                <table cellSpacing="1" cellPadding="1" class="W">
							        <tr>
                                        <td vAlign="middle"><label for="rdoHYOJI1">FAX入力選択：&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
								        <td vAlign="middle"><input id="rdoHYOJI1" onclick="hyojicg()" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										        tabIndex="10" type="radio" CHECKED value="1" name="rdoHYOJI"></td>
								        <td vAlign="middle"><label for="rdoHYOJI1">スポットFAX&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
								        <td vAlign="middle"><input id="rdoHYOJI2" onclick="hyojicg()" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
										        tabIndex="10" type="radio" value="2" name="rdoHYOJI"></td>
								        <td vAlign="middle"><label for="rdoHYOJI2">自動FAX&nbsp;&nbsp;&nbsp;&nbsp;</label></td>
							        </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
				<table cellSpacing="0" cellPadding="0">
                    <!-- ▼▼▼ 2013/05/22 T.Ono mod 顧客単位登録機能追加 tr/tdへid付与 ▼▼▼ -->
					<tr id="koumoku">
						<td id="DispAlwy_100" align="left" height="25">ｺﾋﾟﾍﾟ </td> <!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<td id="DispAlwy_1" align="left" height="25">ｺｰﾄﾞ</td>
                        <!-- <td id="DispAlwy_2" align="left" height="25">担当者名漢字</td> 2013/07/26 T.Ono mod 顧客単位登録機能追加 -->
						<td id="DispAlwy_2" align="left" height="25">&nbsp;&nbsp;担当者名漢字</td>
						<td id="DispSpot_1" align="left" height="25">電話番号１</td>
						<td id="DispSpot_2" align="left" height="25">電話番号２</td>
						<td id="DispSpot_3" align="left" height="25">電話番号３</td>             <!-- 2013/05/22 T.Ono add 顧客単位登録機能追加 -->
						<!-- <td id="DispSpot_4" align="left" height="25">FAX番号</td> 2013/07/26 T.Ono mod 顧客単位登録機能追加 -->
                        <td id="DispSpot_4" align="left" height="25">ｽﾎﾟｯﾄFAX番号</td>
						<!-- <td id="DispSpot_5" align="left" height="25">備考(※印刷対象外)</td> 2013/11/28 T.Ono mod 監視改善2013 -->
						<td id="DispSpot_5" align="left" height="25">記事(※印刷対象外)</td>
						<!-- <td align="left" height="25">自動FAX送信先ﾒｰﾙｱﾄﾞﾚｽ</td>              2013/05/23 T.Ono del 顧客単位登録機能追加 -->
						<td id="DispSpot_6" align="left" height="25">ｽﾎﾟｯﾄFAX送信先ﾒｰﾙｱﾄﾞﾚｽ</td>
						<td id="DispSpot_7" align="left" height="25" width="100px">ｽﾎﾟｯﾄFAX添付ﾌｧｲﾙﾊﾟｽﾜｰﾄﾞ</td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                        <td id="DispAUTO_186" align="left" height="25">自動FAX送信名</td>
                        <td id="DispAUTO_1" align="left" height="25">自動FAX送信先ﾒｰﾙｱﾄﾞﾚｽ</td>
                        <td id="DispAUTO_2" align="left" height="25" width="100px">自動FAX添付ﾌｧｲﾙﾊﾟｽﾜｰﾄﾞ</td>
                        <td id="DispAUTO_3" align="left" height="25">自動FAX番号</td>
                        <td id="DispAUTO_4" align="left" height="25">自動送信区分</td>
                        <td id="DispAUTO_5" align="left" height="25">ゼロ件送信フラグ</td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_1">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_101"><asp:checkbox id="chkCopy_1" tabIndex="11" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_1" type="hidden" name="hdnDISP_NO_1" runat="server">
							<asp:textbox id="txtTANCD_1" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_3"><INPUT id="hdnDISP_NO_1" type="hidden" name="hdnDISP_NO_1" runat="server">
							<asp:textbox id="txtTANCD_1" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_4"><asp:textbox id="txtTANNM_1" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="12" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_8"><asp:textbox id="txtRENTEL1_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="13" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_9"><asp:textbox id="txtRENTEL2_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="14" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_10"><asp:textbox id="txtRENTEL3_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="15" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_11"><asp:textbox id="txtFAXNO_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="16" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_12"><asp:textbox id="txtBIKO_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="17" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="17" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_13"><asp:textbox id="txtSPOT_MAIL_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="18" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_14"><asp:textbox id="txtMAIL_PASS_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="19" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
	                    <td id="DispAUTO_156"><asp:textbox id="txtAUTO_FAXNM_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="20" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_6"><asp:textbox id="txtAUTO_MAIL_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="21" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_7"><asp:textbox id="txtAUTO_MAIL_PASS_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="22" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_8"><asp:textbox id="txtAUTO_FAXNO_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="23" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_9"><cc1:ctlcombo id="cboAUTO_KBN_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="24" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_10"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_1" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="25" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_2">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_102"><asp:checkbox id="chkCopy_2" tabIndex="31" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_2" type="hidden" name="hdnDISP_NO_2" runat="server">
							<asp:textbox id="txtTANCD_2" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_5"><INPUT id="hdnDISP_NO_2" type="hidden" name="hdnDISP_NO_2" runat="server">
							<asp:textbox id="txtTANCD_2" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_6"><asp:textbox id="txtTANNM_2" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="32" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_15"><asp:textbox id="txtRENTEL1_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="33" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_16"><asp:textbox id="txtRENTEL2_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="34" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_17"><asp:textbox id="txtRENTEL3_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="35" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_18"><asp:textbox id="txtFAXNO_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="36" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_19"><asp:textbox id="txtBIKO_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="37" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
					    <%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="27" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_20"><asp:textbox id="txtSPOT_MAIL_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="38" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_21"><asp:textbox id="txtMAIL_PASS_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="39" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_157"><asp:textbox id="txtAUTO_FAXNM_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="40" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_11"><asp:textbox id="txtAUTO_MAIL_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="41" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_12"><asp:textbox id="txtAUTO_MAIL_PASS_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="42" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_13"><asp:textbox id="txtAUTO_FAXNO_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="43" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_14"><cc1:ctlcombo id="cboAUTO_KBN_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="44" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_15"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_2" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="45" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_3">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_103"><asp:checkbox id="chkCopy_3" tabIndex="51" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_3" type="hidden" name="hdnDISP_NO_3" runat="server">
							<asp:textbox id="txtTANCD_3" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_7"><INPUT id="hdnDISP_NO_3" type="hidden" name="hdnDISP_NO_3" runat="server">
							<asp:textbox id="txtTANCD_3" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_8"><asp:textbox id="txtTANNM_3" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="52" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_22"><asp:textbox id="txtRENTEL1_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="53" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_23"><asp:textbox id="txtRENTEL2_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="54" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_24"><asp:textbox id="txtRENTEL3_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="55" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_25"><asp:textbox id="txtFAXNO_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="56" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_26"><asp:textbox id="txtBIKO_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="57" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="37" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_27"><asp:textbox id="txtSPOT_MAIL_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="58" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_28"><asp:textbox id="txtMAIL_PASS_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="59" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_158"><asp:textbox id="txtAUTO_FAXNM_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="60" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_16"><asp:textbox id="txtAUTO_MAIL_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="61" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_17"><asp:textbox id="txtAUTO_MAIL_PASS_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="62" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_18"><asp:textbox id="txtAUTO_FAXNO_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="63" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_19"><cc1:ctlcombo id="cboAUTO_KBN_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="64" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_20"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_3" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="65" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_4">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_104"><asp:checkbox id="chkCopy_4" tabIndex="71" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_4" type="hidden" name="hdnDISP_NO_4" runat="server">
							<asp:textbox id="txtTANCD_4" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_9"><INPUT id="hdnDISP_NO_4" type="hidden" name="hdnDISP_NO_4" runat="server">
							<asp:textbox id="txtTANCD_4" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_10"><asp:textbox id="txtTANNM_4" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="72" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_29"><asp:textbox id="txtRENTEL1_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="73" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_30"><asp:textbox id="txtRENTEL2_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="74" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_31"><asp:textbox id="txtRENTEL3_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="75" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_32"><asp:textbox id="txtFAXNO_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="76" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_33"><asp:textbox id="txtBIKO_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="77" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="47" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_34"><asp:textbox id="txtSPOT_MAIL_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="78" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_35"><asp:textbox id="txtMAIL_PASS_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="79" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_159"><asp:textbox id="txtAUTO_FAXNM_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="80" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_21"><asp:textbox id="txtAUTO_MAIL_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="81" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_22"><asp:textbox id="txtAUTO_MAIL_PASS_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="82" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_23"><asp:textbox id="txtAUTO_FAXNO_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="83" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_24"><cc1:ctlcombo id="cboAUTO_KBN_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="84" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_25"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_4" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="85" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_5">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_105"><asp:checkbox id="chkCopy_5" tabIndex="91" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_5" type="hidden" name="hdnDISP_NO_5" runat="server">
							<asp:textbox id="txtTANCD_5" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_11"><INPUT id="hdnDISP_NO_5" type="hidden" name="hdnDISP_NO_5" runat="server">
							<asp:textbox id="txtTANCD_5" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_12"><asp:textbox id="txtTANNM_5" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="92" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_36"><asp:textbox id="txtRENTEL1_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="93" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_37"><asp:textbox id="txtRENTEL2_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="94" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_38"><asp:textbox id="txtRENTEL3_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="95" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_39"><asp:textbox id="txtFAXNO_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="96" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_40"><asp:textbox id="txtBIKO_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="97" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="57" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_41"><asp:textbox id="txtSPOT_MAIL_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="98" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_42"><asp:textbox id="txtMAIL_PASS_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="99" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_160"><asp:textbox id="txtAUTO_FAXNM_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="100" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_26"><asp:textbox id="txtAUTO_MAIL_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="101" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_27"><asp:textbox id="txtAUTO_MAIL_PASS_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="102" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_28"><asp:textbox id="txtAUTO_FAXNO_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="103" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_29"><cc1:ctlcombo id="cboAUTO_KBN_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="104" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_30"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_5" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="105" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_6">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_106"><asp:checkbox id="chkCopy_6" tabIndex="111" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_6" type="hidden" name="hdnDISP_NO_6" runat="server">
							<asp:textbox id="txtTANCD_6" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_13"><INPUT id="hdnDISP_NO_6" type="hidden" name="hdnDISP_NO_6" runat="server">
							<asp:textbox id="txtTANCD_6" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_14"><asp:textbox id="txtTANNM_6" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="112" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_43"><asp:textbox id="txtRENTEL1_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="113" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_44"><asp:textbox id="txtRENTEL2_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="114" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_45"><asp:textbox id="txtRENTEL3_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="115" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_46"><asp:textbox id="txtFAXNO_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="116" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_47"><asp:textbox id="txtBIKO_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="117" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="67" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_48"><asp:textbox id="txtSPOT_MAIL_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="118" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_49"><asp:textbox id="txtMAIL_PASS_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="119" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_161"><asp:textbox id="txtAUTO_FAXNM_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="120" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_31"><asp:textbox id="txtAUTO_MAIL_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="121" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_32"><asp:textbox id="txtAUTO_MAIL_PASS_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="122" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_33"><asp:textbox id="txtAUTO_FAXNO_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="123" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_34"><cc1:ctlcombo id="cboAUTO_KBN_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="124" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_35"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_6" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="125" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_7">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_107"><asp:checkbox id="chkCopy_7" tabIndex="131" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_7" type="hidden" name="hdnDISP_NO_7" runat="server">
							<asp:textbox id="txtTANCD_7" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_15"><INPUT id="hdnDISP_NO_7" type="hidden" name="hdnDISP_NO_7" runat="server">
							<asp:textbox id="txtTANCD_7" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_16"><asp:textbox id="txtTANNM_7" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="132" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_50"><asp:textbox id="txtRENTEL1_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="133" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_51"><asp:textbox id="txtRENTEL2_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="134" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_52"><asp:textbox id="txtRENTEL3_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="135" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_53"><asp:textbox id="txtFAXNO_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="136" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_54"><asp:textbox id="txtBIKO_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="137" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="77" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_55"><asp:textbox id="txtSPOT_MAIL_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="138" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_56"><asp:textbox id="txtMAIL_PASS_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="139" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_162"><asp:textbox id="txtAUTO_FAXNM_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="140" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_36"><asp:textbox id="txtAUTO_MAIL_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="141" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_37"><asp:textbox id="txtAUTO_MAIL_PASS_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="142" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_38"><asp:textbox id="txtAUTO_FAXNO_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="143" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_39"><cc1:ctlcombo id="cboAUTO_KBN_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="144" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_40"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_7" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="145" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_8">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_108"><asp:checkbox id="chkCopy_8" tabIndex="151" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_8" type="hidden" name="hdnDISP_NO_8" runat="server">
							<asp:textbox id="txtTANCD_8" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_17"><INPUT id="hdnDISP_NO_8" type="hidden" name="hdnDISP_NO_8" runat="server">
							<asp:textbox id="txtTANCD_8" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_18"><asp:textbox id="txtTANNM_8" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="152" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_57"><asp:textbox id="txtRENTEL1_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="153" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_58"><asp:textbox id="txtRENTEL2_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="154" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_59"><asp:textbox id="txtRENTEL3_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="155" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_60"><asp:textbox id="txtFAXNO_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="156" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_61"><asp:textbox id="txtBIKO_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="157" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="87" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_62"><asp:textbox id="txtSPOT_MAIL_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="158" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_63"><asp:textbox id="txtMAIL_PASS_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="159" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_163"><asp:textbox id="txtAUTO_FAXNM_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="160" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_41"><asp:textbox id="txtAUTO_MAIL_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="161" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_42"><asp:textbox id="txtAUTO_MAIL_PASS_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="162" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_43"><asp:textbox id="txtAUTO_FAXNO_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="163" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_44"><cc1:ctlcombo id="cboAUTO_KBN_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="164" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_45"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_8" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="165" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_9">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_109"><asp:checkbox id="chkCopy_9" tabIndex="171" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_9" type="hidden" name="hdnDISP_NO_9" runat="server">
							<asp:textbox id="txtTANCD_9" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_19"><INPUT id="hdnDISP_NO_9" type="hidden" name="hdnDISP_NO_9" runat="server">
							<asp:textbox id="txtTANCD_9" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_20"><asp:textbox id="txtTANNM_9" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="172" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_64"><asp:textbox id="txtRENTEL1_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="173" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_65"><asp:textbox id="txtRENTEL2_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="174" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_66"><asp:textbox id="txtRENTEL3_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="175" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_67"><asp:textbox id="txtFAXNO_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="176" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_68"><asp:textbox id="txtBIKO_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="177" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="97" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_69"><asp:textbox id="txtSPOT_MAIL_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="178" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_70"><asp:textbox id="txtMAIL_PASS_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="179" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_164"><asp:textbox id="txtAUTO_FAXNM_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="180" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_46"><asp:textbox id="txtAUTO_MAIL_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="181" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_47"><asp:textbox id="txtAUTO_MAIL_PASS_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="182" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_48"><asp:textbox id="txtAUTO_FAXNO_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="183" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_49"><cc1:ctlcombo id="cboAUTO_KBN_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="184" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_50"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_9" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="185" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_10">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_110"><asp:checkbox id="chkCopy_10" tabIndex="191" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_10" type="hidden" name="hdnDISP_NO_10" runat="server">
							<asp:textbox id="txtTANCD_10" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_21"><INPUT id="hdnDISP_NO_10" type="hidden" name="hdnDISP_NO_10" runat="server">
							<asp:textbox id="txtTANCD_10" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_22"><asp:textbox id="txtTANNM_10" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="192" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_71"><asp:textbox id="txtRENTEL1_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="193" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_72"><asp:textbox id="txtRENTEL2_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="194" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_73"><asp:textbox id="txtRENTEL3_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="195" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_74"><asp:textbox id="txtFAXNO_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="196" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_75"><asp:textbox id="txtBIKO_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="197" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="107" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
  						<td id="DispSpot_76"><asp:textbox id="txtSPOT_MAIL_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="198" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_77"><asp:textbox id="txtMAIL_PASS_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="199" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                	    <td id="DispAUTO_165"><asp:textbox id="txtAUTO_FAXNM_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="200" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_51"><asp:textbox id="txtAUTO_MAIL_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="201" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_52"><asp:textbox id="txtAUTO_MAIL_PASS_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="202" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_53"><asp:textbox id="txtAUTO_FAXNO_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="203" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_54"><cc1:ctlcombo id="cboAUTO_KBN_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="204" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_55"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_10" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="205" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<!-- ▼▼▼ 2010/04/12 T.Watabe add ▼▼▼ -->
					<tr id="list_11">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_111"><asp:checkbox id="chkCopy_11" tabIndex="211" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_11" type="hidden" name="hdnDISP_NO_11" runat="server">
							<asp:textbox id="txtTANCD_11" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_23"><INPUT id="hdnDISP_NO_11" type="hidden" name="hdnDISP_NO_11" runat="server">
							<asp:textbox id="txtTANCD_11" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_24"><asp:textbox id="txtTANNM_11" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="212" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_78"><asp:textbox id="txtRENTEL1_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="213" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_79"><asp:textbox id="txtRENTEL2_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="214" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_80"><asp:textbox id="txtRENTEL3_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="215" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_81"><asp:textbox id="txtFAXNO_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="216" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_82"><asp:textbox id="txtBIKO_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="217" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="117" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_83"><asp:textbox id="txtSPOT_MAIL_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="218" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_84"><asp:textbox id="txtMAIL_PASS_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="219" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_166"><asp:textbox id="txtAUTO_FAXNM_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="220" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_56"><asp:textbox id="txtAUTO_MAIL_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="221" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_57"><asp:textbox id="txtAUTO_MAIL_PASS_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="222" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_58"><asp:textbox id="txtAUTO_FAXNO_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="223" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_59"><cc1:ctlcombo id="cboAUTO_KBN_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="224" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_60"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_11" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="225" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_12">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_112"><asp:checkbox id="chkCopy_12" tabIndex="231" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_12" type="hidden" name="hdnDISP_NO_12" runat="server">
							<asp:textbox id="txtTANCD_12" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_25"><INPUT id="hdnDISP_NO_12" type="hidden" name="hdnDISP_NO_12" runat="server">
							<asp:textbox id="txtTANCD_12" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_26"><asp:textbox id="txtTANNM_12" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="232" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_85"><asp:textbox id="txtRENTEL1_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="233" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_86"><asp:textbox id="txtRENTEL2_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="234" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_87"><asp:textbox id="txtRENTEL3_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="235" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_88"><asp:textbox id="txtFAXNO_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="236" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_89"><asp:textbox id="txtBIKO_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="237" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="127" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_90"><asp:textbox id="txtSPOT_MAIL_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="238" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_91"><asp:textbox id="txtMAIL_PASS_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="239" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_167"><asp:textbox id="txtAUTO_FAXNM_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="240" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_61"><asp:textbox id="txtAUTO_MAIL_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="241" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_62"><asp:textbox id="txtAUTO_MAIL_PASS_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="242" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_63"><asp:textbox id="txtAUTO_FAXNO_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="243" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_64"><cc1:ctlcombo id="cboAUTO_KBN_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="244" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_65"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_12" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="245" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_13">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_113"><asp:checkbox id="chkCopy_13" tabIndex="251" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_13" type="hidden" name="hdnDISP_NO_13" runat="server">
							<asp:textbox id="txtTANCD_13" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_27"><INPUT id="hdnDISP_NO_13" type="hidden" name="hdnDISP_NO_13" runat="server">
							<asp:textbox id="txtTANCD_13" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_28"><asp:textbox id="txtTANNM_13" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="252" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_92"><asp:textbox id="txtRENTEL1_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="253" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_93"><asp:textbox id="txtRENTEL2_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="254" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_94"><asp:textbox id="txtRENTEL3_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="255" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_95"><asp:textbox id="txtFAXNO_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="256" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_96"><asp:textbox id="txtBIKO_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="257" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="137" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_97"><asp:textbox id="txtSPOT_MAIL_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="258" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_98"><asp:textbox id="txtMAIL_PASS_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="259" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_168"><asp:textbox id="txtAUTO_FAXNM_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="260" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_66"><asp:textbox id="txtAUTO_MAIL_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="261" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_67"><asp:textbox id="txtAUTO_MAIL_PASS_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="262" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_68"><asp:textbox id="txtAUTO_FAXNO_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="263" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_69"><cc1:ctlcombo id="cboAUTO_KBN_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="264" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_70"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_13" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="265" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_14">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_114"><asp:checkbox id="chkCopy_14" tabIndex="271" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_14" type="hidden" name="hdnDISP_NO_14" runat="server">
							<asp:textbox id="txtTANCD_14" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_29"><INPUT id="hdnDISP_NO_14" type="hidden" name="hdnDISP_NO_14" runat="server">
							<asp:textbox id="txtTANCD_14" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_30"><asp:textbox id="txtTANNM_14" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="272" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_99"><asp:textbox id="txtRENTEL1_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="273" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_100"><asp:textbox id="txtRENTEL2_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="274" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_101"><asp:textbox id="txtRENTEL3_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="275" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_102"><asp:textbox id="txtFAXNO_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="276" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_103"><asp:textbox id="txtBIKO_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="277" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="147" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_104"><asp:textbox id="txtSPOT_MAIL_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="278" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_105"><asp:textbox id="txtMAIL_PASS_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="279" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_169"><asp:textbox id="txtAUTO_FAXNM_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="280" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_71"><asp:textbox id="txtAUTO_MAIL_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="281" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_72"><asp:textbox id="txtAUTO_MAIL_PASS_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="282" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_73"><asp:textbox id="txtAUTO_FAXNO_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="283" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_74"><cc1:ctlcombo id="cboAUTO_KBN_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="284" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_75"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_14" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="285" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_15">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_115"><asp:checkbox id="chkCopy_15" tabIndex="291" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_15" type="hidden" name="hdnDISP_NO_15" runat="server">
							<asp:textbox id="txtTANCD_15" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_31"><INPUT id="hdnDISP_NO_15" type="hidden" name="hdnDISP_NO_15" runat="server">
							<asp:textbox id="txtTANCD_15" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_32"><asp:textbox id="txtTANNM_15" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="292" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_106"><asp:textbox id="txtRENTEL1_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="293" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_107"><asp:textbox id="txtRENTEL2_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="294" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_108"><asp:textbox id="txtRENTEL3_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="295" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_109"><asp:textbox id="txtFAXNO_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="296" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_110"><asp:textbox id="txtBIKO_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="297" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono del 顧客単位登録機能追加 ▼▼▼ -->
						<%-- <td><asp:textbox id="txtAUTO_MAIL_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="157" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
                        <!-- ▲▲▲ 2013/05/23 T.Ono del 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_111"><asp:textbox id="txtSPOT_MAIL_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="298" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_112"><asp:textbox id="txtMAIL_PASS_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="299" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_170"><asp:textbox id="txtAUTO_FAXNM_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="300" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_76"><asp:textbox id="txtAUTO_MAIL_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="301" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_77"><asp:textbox id="txtAUTO_MAIL_PASS_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="302" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_78"><asp:textbox id="txtAUTO_FAXNO_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="303" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_79"><cc1:ctlcombo id="cboAUTO_KBN_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="304" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_80"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_15" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="305" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_16">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_116"><asp:checkbox id="chkCopy_16" tabIndex="311" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_16" type="hidden" name="hdnDISP_NO_16" runat="server">
							<asp:textbox id="txtTANCD_16" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_33"><INPUT id="hdnDISP_NO_16" type="hidden" name="hdnDISP_NO_16" runat="server">
							<asp:textbox id="txtTANCD_16" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_34"><asp:textbox id="txtTANNM_16" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="312" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_113"><asp:textbox id="txtRENTEL1_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="313" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_114"><asp:textbox id="txtRENTEL2_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="314" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_115"><asp:textbox id="txtRENTEL3_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="315" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_116"><asp:textbox id="txtFAXNO_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="316" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_117"><asp:textbox id="txtBIKO_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="317" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="167" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_118"><asp:textbox id="txtSPOT_MAIL_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="318" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_119"><asp:textbox id="txtMAIL_PASS_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="319" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_171"><asp:textbox id="txtAUTO_FAXNM_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="320" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_81"><asp:textbox id="txtAUTO_MAIL_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="321" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_82"><asp:textbox id="txtAUTO_MAIL_PASS_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="322" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_83"><asp:textbox id="txtAUTO_FAXNO_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="323" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_84"><cc1:ctlcombo id="cboAUTO_KBN_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="324" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_85"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_16" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="325" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_17">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_117"><asp:checkbox id="chkCopy_17" tabIndex="331" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_17" type="hidden" name="hdnDISP_NO_17" runat="server">
							<asp:textbox id="txtTANCD_17" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_35"><INPUT id="hdnDISP_NO_17" type="hidden" name="hdnDISP_NO_17" runat="server">
							<asp:textbox id="txtTANCD_17" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_36"><asp:textbox id="txtTANNM_17" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="332" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_120"><asp:textbox id="txtRENTEL1_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="333" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_121"><asp:textbox id="txtRENTEL2_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="334" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_122"><asp:textbox id="txtRENTEL3_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="335" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_123"><asp:textbox id="txtFAXNO_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="336" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_124"><asp:textbox id="txtBIKO_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="337" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="177" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_125"><asp:textbox id="txtSPOT_MAIL_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="338" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_126"><asp:textbox id="txtMAIL_PASS_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="339" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_172"><asp:textbox id="txtAUTO_FAXNM_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="340" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_86"><asp:textbox id="txtAUTO_MAIL_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="341" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_87"><asp:textbox id="txtAUTO_MAIL_PASS_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="342" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_88"><asp:textbox id="txtAUTO_FAXNO_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="343" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_89"><cc1:ctlcombo id="cboAUTO_KBN_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="344" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_90"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_17" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="345" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_18">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_118"><asp:checkbox id="chkCopy_18" tabIndex="351" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_18" type="hidden" name="hdnDISP_NO_18" runat="server">
							<asp:textbox id="txtTANCD_18" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_37"><INPUT id="hdnDISP_NO_18" type="hidden" name="hdnDISP_NO_18" runat="server">
							<asp:textbox id="txtTANCD_18" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_38"><asp:textbox id="txtTANNM_18" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="352" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_127"><asp:textbox id="txtRENTEL1_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="353" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_128"><asp:textbox id="txtRENTEL2_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="354" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_129"><asp:textbox id="txtRENTEL3_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="355" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_130"><asp:textbox id="txtFAXNO_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="356" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_131"><asp:textbox id="txtBIKO_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="357" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="187" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_132"><asp:textbox id="txtSPOT_MAIL_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="358" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_133"><asp:textbox id="txtMAIL_PASS_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="359" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_173"><asp:textbox id="txtAUTO_FAXNM_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="360" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_91"><asp:textbox id="txtAUTO_MAIL_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="361" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_92"><asp:textbox id="txtAUTO_MAIL_PASS_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="362" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_93"><asp:textbox id="txtAUTO_FAXNO_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="363" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_94"><cc1:ctlcombo id="cboAUTO_KBN_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="364" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_95"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_18" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="365" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_19">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_119"><asp:checkbox id="chkCopy_19" tabIndex="371" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_19" type="hidden" name="hdnDISP_NO_19" runat="server">
							<asp:textbox id="txtTANCD_19" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_39"><INPUT id="hdnDISP_NO_19" type="hidden" name="hdnDISP_NO_19" runat="server">
							<asp:textbox id="txtTANCD_19" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_40"><asp:textbox id="txtTANNM_19" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="372" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_134"><asp:textbox id="txtRENTEL1_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="373" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_135"><asp:textbox id="txtRENTEL2_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="374" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_136"><asp:textbox id="txtRENTEL3_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="375" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_137"><asp:textbox id="txtFAXNO_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="376" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_138"><asp:textbox id="txtBIKO_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="377" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="197" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_139"><asp:textbox id="txtSPOT_MAIL_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="378" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_140"><asp:textbox id="txtMAIL_PASS_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="379" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_174"><asp:textbox id="txtAUTO_FAXNM_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="380" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_96"><asp:textbox id="txtAUTO_MAIL_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="381" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_97"><asp:textbox id="txtAUTO_MAIL_PASS_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="382" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_98"><asp:textbox id="txtAUTO_FAXNO_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="383" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_99"><cc1:ctlcombo id="cboAUTO_KBN_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="384" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_100"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_19" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="385" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_20">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_120"><asp:checkbox id="chkCopy_20" tabIndex="391" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_20" type="hidden" name="hdnDISP_NO_20" runat="server">
							<asp:textbox id="txtTANCD_20" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_41"><INPUT id="hdnDISP_NO_20" type="hidden" name="hdnDISP_NO_20" runat="server">
							<asp:textbox id="txtTANCD_20" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_42"><asp:textbox id="txtTANNM_20" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="392" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_141"><asp:textbox id="txtRENTEL1_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="393" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_142"><asp:textbox id="txtRENTEL2_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="394" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_143"><asp:textbox id="txtRENTEL3_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="395" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_144"><asp:textbox id="txtFAXNO_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="396" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_145"><asp:textbox id="txtBIKO_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="397" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="207" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_146"><asp:textbox id="txtSPOT_MAIL_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="398" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_147"><asp:textbox id="txtMAIL_PASS_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="399" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_175"><asp:textbox id="txtAUTO_FAXNM_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="400" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_101"><asp:textbox id="txtAUTO_MAIL_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="401" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_102"><asp:textbox id="txtAUTO_MAIL_PASS_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="402" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_103"><asp:textbox id="txtAUTO_FAXNO_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="403" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_104"><cc1:ctlcombo id="cboAUTO_KBN_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="404" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_105"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_20" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="405" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr>
						<td colspan="6">↓21〜30は印刷対象外↓</td>
					</tr>
					<tr id="list_21">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_121"><asp:checkbox id="chkCopy_21" tabIndex="411" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_21" type="hidden" name="hdnDISP_NO_21" runat="server">
							<asp:textbox id="txtTANCD_21" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_43"><INPUT id="hdnDISP_NO_21" type="hidden" name="hdnDISP_NO_21" runat="server">
							<asp:textbox id="txtTANCD_21" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_44"><asp:textbox id="txtTANNM_21" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="412" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_148"><asp:textbox id="txtRENTEL1_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="413" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_149"><asp:textbox id="txtRENTEL2_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="414" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_150"><asp:textbox id="txtRENTEL3_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="415" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_151"><asp:textbox id="txtFAXNO_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="416" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_152"><asp:textbox id="txtBIKO_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="417" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="217" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_153"><asp:textbox id="txtSPOT_MAIL_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="418" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_154"><asp:textbox id="txtMAIL_PASS_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="419" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_176"><asp:textbox id="txtAUTO_FAXNM_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="420" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_106"><asp:textbox id="txtAUTO_MAIL_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="421" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_107"><asp:textbox id="txtAUTO_MAIL_PASS_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="422" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_108"><asp:textbox id="txtAUTO_FAXNO_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="423" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_109"><cc1:ctlcombo id="cboAUTO_KBN_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="424" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_110"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_21" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="425" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_22">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_122"><asp:checkbox id="chkCopy_22" tabIndex="451" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_22" type="hidden" name="hdnDISP_NO_22" runat="server">
							<asp:textbox id="txtTANCD_22" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_45"><INPUT id="hdnDISP_NO_22" type="hidden" name="hdnDISP_NO_22" runat="server">
							<asp:textbox id="txtTANCD_22" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_46"><asp:textbox id="txtTANNM_22" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="452" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_155"><asp:textbox id="txtRENTEL1_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="453" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_156"><asp:textbox id="txtRENTEL2_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="454" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_157"><asp:textbox id="txtRENTEL3_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="455" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_158"><asp:textbox id="txtFAXNO_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="456" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_159"><asp:textbox id="txtBIKO_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="457" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="227" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_160"><asp:textbox id="txtSPOT_MAIL_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="458" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_161"><asp:textbox id="txtMAIL_PASS_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="459" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_177"><asp:textbox id="txtAUTO_FAXNM_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="460" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_111"><asp:textbox id="txtAUTO_MAIL_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="461" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_112"><asp:textbox id="txtAUTO_MAIL_PASS_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="462" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_113"><asp:textbox id="txtAUTO_FAXNO_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="463" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_114"><cc1:ctlcombo id="cboAUTO_KBN_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="464" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_115"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_22" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="465" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_23">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_123"><asp:checkbox id="chkCopy_23" tabIndex="471" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_23" type="hidden" name="hdnDISP_NO_23" runat="server">
							<asp:textbox id="txtTANCD_23" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_47"><INPUT id="hdnDISP_NO_23" type="hidden" name="hdnDISP_NO_23" runat="server">
							<asp:textbox id="txtTANCD_23" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_48"><asp:textbox id="txtTANNM_23" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="472" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_162"><asp:textbox id="txtRENTEL1_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="473" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_163"><asp:textbox id="txtRENTEL2_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="474" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_164"><asp:textbox id="txtRENTEL3_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="475" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_165"><asp:textbox id="txtFAXNO_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="476" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_166"><asp:textbox id="txtBIKO_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="477" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="237" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_167"><asp:textbox id="txtSPOT_MAIL_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="478" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_168"><asp:textbox id="txtMAIL_PASS_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="479" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_178"><asp:textbox id="txtAUTO_FAXNM_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="480" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_116"><asp:textbox id="txtAUTO_MAIL_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="481" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_117"><asp:textbox id="txtAUTO_MAIL_PASS_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="482" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_118"><asp:textbox id="txtAUTO_FAXNO_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="483" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_119"><cc1:ctlcombo id="cboAUTO_KBN_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="484" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_120"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_23" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="485" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_24">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_124"><asp:checkbox id="chkCopy_24" tabIndex="491" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_24" type="hidden" name="hdnDISP_NO_24" runat="server">
							<asp:textbox id="txtTANCD_24" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_49"><INPUT id="hdnDISP_NO_24" type="hidden" name="hdnDISP_NO_24" runat="server">
							<asp:textbox id="txtTANCD_24" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>  
						<td id="DispAlwy_50"><asp:textbox id="txtTANNM_24" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="492" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_169"><asp:textbox id="txtRENTEL1_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="493" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_170"><asp:textbox id="txtRENTEL2_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="494" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_171"><asp:textbox id="txtRENTEL3_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="495" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_172"><asp:textbox id="txtFAXNO_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="496" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_173"><asp:textbox id="txtBIKO_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="497" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="247" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_174"><asp:textbox id="txtSPOT_MAIL_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="498" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_175"><asp:textbox id="txtMAIL_PASS_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="499" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_179"><asp:textbox id="txtAUTO_FAXNM_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="500" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_121"><asp:textbox id="txtAUTO_MAIL_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="501" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_122"><asp:textbox id="txtAUTO_MAIL_PASS_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="502" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_123"><asp:textbox id="txtAUTO_FAXNO_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="503" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_124"><cc1:ctlcombo id="cboAUTO_KBN_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="504" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_125"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_24" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="505" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_25">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_125"><asp:checkbox id="chkCopy_25" tabIndex="511" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_25" type="hidden" name="hdnDISP_NO_25" runat="server">
							<asp:textbox id="txtTANCD_25" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
                        <td id="DispAlwy_51"><INPUT id="hdnDISP_NO_25" type="hidden" name="hdnDISP_NO_25" runat="server">
							<asp:textbox id="txtTANCD_25" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_52"><asp:textbox id="txtTANNM_25" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="512" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_176"><asp:textbox id="txtRENTEL1_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="513" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_177"><asp:textbox id="txtRENTEL2_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="514" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_178"><asp:textbox id="txtRENTEL3_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="515" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_179"><asp:textbox id="txtFAXNO_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="516" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_180"><asp:textbox id="txtBIKO_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="517" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="257" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_181"><asp:textbox id="txtSPOT_MAIL_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="518" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_182"><asp:textbox id="txtMAIL_PASS_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="519" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_180"><asp:textbox id="txtAUTO_FAXNM_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="520" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_126"><asp:textbox id="txtAUTO_MAIL_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="521" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_127"><asp:textbox id="txtAUTO_MAIL_PASS_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="522" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_128"><asp:textbox id="txtAUTO_FAXNO_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="523" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_129"><cc1:ctlcombo id="cboAUTO_KBN_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="524" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_130"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_25" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="525" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_26">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_126"><asp:checkbox id="chkCopy_26" tabIndex="531" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_26" type="hidden" name="hdnDISP_NO_26" runat="server">
							<asp:textbox id="txtTANCD_26" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_53"><INPUT id="hdnDISP_NO_26" type="hidden" name="hdnDISP_NO_26" runat="server">
							<asp:textbox id="txtTANCD_26" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_54"><asp:textbox id="txtTANNM_26" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="532" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_183"><asp:textbox id="txtRENTEL1_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="533" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_184"><asp:textbox id="txtRENTEL2_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="534" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_185"><asp:textbox id="txtRENTEL3_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="535" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_186"><asp:textbox id="txtFAXNO_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="536" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_187"><asp:textbox id="txtBIKO_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="537" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="267" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_188"><asp:textbox id="txtSPOT_MAIL_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="538" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_189"><asp:textbox id="txtMAIL_PASS_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="539" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_181"><asp:textbox id="txtAUTO_FAXNM_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="540" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_131"><asp:textbox id="txtAUTO_MAIL_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="541" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_132"><asp:textbox id="txtAUTO_MAIL_PASS_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="542" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_133"><asp:textbox id="txtAUTO_FAXNO_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="543" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_134"><cc1:ctlcombo id="cboAUTO_KBN_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="544" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_135"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_26" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="545" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_27">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_127"><asp:checkbox id="chkCopy_27" tabIndex="551" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_27" type="hidden" name="hdnDISP_NO_27" runat="server">
							<asp:textbox id="txtTANCD_27" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_55"><INPUT id="hdnDISP_NO_27" type="hidden" name="hdnDISP_NO_27" runat="server">
							<asp:textbox id="txtTANCD_27" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_56"><asp:textbox id="txtTANNM_27" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="552" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_190"><asp:textbox id="txtRENTEL1_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="553" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_191"><asp:textbox id="txtRENTEL2_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="554" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_192"><asp:textbox id="txtRENTEL3_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="555" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_193"><asp:textbox id="txtFAXNO_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="556" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_194"><asp:textbox id="txtBIKO_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="557" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="277" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_195"><asp:textbox id="txtSPOT_MAIL_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="558" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_196"><asp:textbox id="txtMAIL_PASS_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="559" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_182"><asp:textbox id="txtAUTO_FAXNM_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="560" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_136"><asp:textbox id="txtAUTO_MAIL_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="561" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_137"><asp:textbox id="txtAUTO_MAIL_PASS_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="562" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_138"><asp:textbox id="txtAUTO_FAXNO_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="563" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_139"><cc1:ctlcombo id="cboAUTO_KBN_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="564" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_140"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_27" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="565" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_28">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_128"><asp:checkbox id="chkCopy_28" tabIndex="571" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_28" type="hidden" name="hdnDISP_NO_28" runat="server">
							<asp:textbox id="txtTANCD_28" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_57"><INPUT id="hdnDISP_NO_28" type="hidden" name="hdnDISP_NO_28" runat="server">
							<asp:textbox id="txtTANCD_28" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_58"><asp:textbox id="txtTANNM_28" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="572" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_197"><asp:textbox id="txtRENTEL1_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="573" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_198"><asp:textbox id="txtRENTEL2_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="574" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_199"><asp:textbox id="txtRENTEL3_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="575" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_200"><asp:textbox id="txtFAXNO_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="576" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_201"><asp:textbox id="txtBIKO_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="577" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="287" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_202"><asp:textbox id="txtSPOT_MAIL_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="578" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_203"><asp:textbox id="txtMAIL_PASS_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="579" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_183"><asp:textbox id="txtAUTO_FAXNM_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="580" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_141"><asp:textbox id="txtAUTO_MAIL_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="581" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_142"><asp:textbox id="txtAUTO_MAIL_PASS_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="582" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_143"><asp:textbox id="txtAUTO_FAXNO_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="583" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_144"><cc1:ctlcombo id="cboAUTO_KBN_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="584" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_145"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_28" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="585" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_29">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_129"><asp:checkbox id="chkCopy_29" tabIndex="591" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_29" type="hidden" name="hdnDISP_NO_29" runat="server">
							<asp:textbox id="txtTANCD_29" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_59"><INPUT id="hdnDISP_NO_29" type="hidden" name="hdnDISP_NO_29" runat="server">
							<asp:textbox id="txtTANCD_29" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_60"><asp:textbox id="txtTANNM_29" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="592" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_204"><asp:textbox id="txtRENTEL1_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="593" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_205"><asp:textbox id="txtRENTEL2_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="594" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_206"><asp:textbox id="txtRENTEL3_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="595" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_207"><asp:textbox id="txtFAXNO_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="596" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_208"><asp:textbox id="txtBIKO_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="597" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="297" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_209"><asp:textbox id="txtSPOT_MAIL_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="598" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_210"><asp:textbox id="txtMAIL_PASS_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="599" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_184"><asp:textbox id="txtAUTO_FAXNM_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="600" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_146"><asp:textbox id="txtAUTO_MAIL_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="601" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_147"><asp:textbox id="txtAUTO_MAIL_PASS_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="602" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_148"><asp:textbox id="txtAUTO_FAXNO_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="603" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_149"><cc1:ctlcombo id="cboAUTO_KBN_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="604" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_150"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_29" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="605" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<tr id="list_30">
                        <%--2012/04/04 .NET 使用変更により、ReadOnlyはVB側でAttributeでつける--%>
                        <td id="DispAlwy_130"><asp:checkbox id="chkCopy_30" tabIndex="611" runat="server" Width="32px" onkeydown="fncFc(this)" /></td><!-- 2015/02/17 T.Ono add 2014改善開発 No15 -->
						<%--<td><INPUT id="hdnDISP_NO_30" type="hidden" name="hdnDISP_NO_30" runat="server">
							<asp:textbox id="txtTANCD_30" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4" ReadOnly="True"></asp:textbox></td>--%>
						<td id="DispAlwy_61"><INPUT id="hdnDISP_NO_30" type="hidden" name="hdnDISP_NO_30" runat="server">
							<asp:textbox id="txtTANCD_30" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="-1" runat="server" CssClass="c-RO" Width="26px" MaxLength="4"></asp:textbox></td>
						<td id="DispAlwy_62"><asp:textbox id="txtTANNM_30" onkeydown="fncFc(this)" onblur="fncFo(this,3)" onfocus="fncFo(this,2)"
								tabIndex="612" runat="server" CssClass="c-hI" Width="250px" MaxLength="30"></asp:textbox></td>
						<td id="DispSpot_211"><asp:textbox id="txtRENTEL1_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="613" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_212"><asp:textbox id="txtRENTEL2_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="614" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
						<td id="DispSpot_213"><asp:textbox id="txtRENTEL3_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="615" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
						<td id="DispSpot_214"><asp:textbox id="txtFAXNO_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="616" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
						<td id="DispSpot_215"><asp:textbox id="txtBIKO_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="617" runat="server" CssClass="c-fI" Width="300px" MaxLength="30"></asp:textbox></td>
						<%-- 2013/05/23 T.Ono del
                        <td><asp:textbox id="txtAUTO_MAIL_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="307" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td> --%>
						<td id="DispSpot_216"><asp:textbox id="txtSPOT_MAIL_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="618" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
						<td id="DispSpot_217"><asp:textbox id="txtMAIL_PASS_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="619" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <!-- ▼▼▼ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
                    	<td id="DispAUTO_185"><asp:textbox id="txtAUTO_FAXNM_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
				                tabIndex="620" runat="server" CssClass="c-fI" Width="200px" MaxLength="30"></asp:textbox></td>
						<td id="DispAUTO_151"><asp:textbox id="txtAUTO_MAIL_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="621" runat="server" CssClass="c-fI" Width="200px" MaxLength="50"></asp:textbox></td>
                        <td id="DispAUTO_152"><asp:textbox id="txtAUTO_MAIL_PASS_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="622" runat="server" CssClass="c-fI" Width="100px" MaxLength="10"></asp:textbox></td>
                        <td id="DispAUTO_153"><asp:textbox id="txtAUTO_FAXNO_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								tabIndex="623" runat="server" CssClass="c-f" Width="120px" MaxLength="15"></asp:textbox></td>
                        <td id="DispAUTO_154"><cc1:ctlcombo id="cboAUTO_KBN_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="624" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <td id="DispAUTO_155"><cc1:ctlcombo id="cboAUTO_ZERO_FLG_30" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onfocus="fncFo(this,2)" 
		                        tabindex="625" runat="server" width="120px" cssclass="cb"></cc1:ctlcombo></td> 
                        <!-- ▲▲▲ 2013/05/23 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
					</tr>
					<!-- ▲▲▲ 2010/04/12 T.Watabe add ▲▲▲ -->
                    <!-- ▲▲▲ 2013/05/22 T.Ono mod 顧客単位登録機能追加 ▲▲▲ -->
				</table>
			</div>
			<!-- ▼▼▼ 2011/11/08 H.Uema add 注意事項登録フォーム作成 ▼▼▼ -->
			<div id="page2">
				<table>
					<tr valign="top">
						<td align="right" class="style2">
							<%-- 2013/0627 T.Ono mod 
                            <b>JA注意事項</b><br> --%>
                            <b><asp:label id="lblpre" style="font-size:10pt" Runat="server" Text="JA注意事項"></asp:label></b><br>
							<br>
							<!-- 2011.12.07 add h.uema start -->
							<input id="btnTips" style="BACKGROUND-COLOR:buttonface" onblur="fncFo(this,5)" onfocus="fncFo(this,2)"
								onclick="return btnTips_onclick();" tabIndex="308" type="button" value="tips" name="btnTips"
								runat="server"> 
							<!-- 2011.12.07 add h.uema end -->
                            <!-- 2013/06/19 mod T.Ono START -->
							 <input style="BACKGROUND-COLOR:buttonface" id="preview" onblur="fncFo(this,5)" onfocus="fncFo(this,2)"
								onclick="hpre(txtGUIDELINE);" tabIndex="309" type="button" value="プレビュー" name="preview"
								runat="server"> 
        <%--                    <input style="BACKGROUND-COLOR:buttonface" id="preview" onblur="fncFo(this,5)" onfocus="fncFo(this,2)"
								onclick="hpre(txtGUIDELINE,lblpre);" tabIndex="309" type="button" value="プレビュー" name="preview"
								runat="server">--%>
                            <!-- 2013/06/19 mod T.Ono END -->
						</td>
						<td>
							<asp:textbox id="txtGUIDELINE" name="txtGUIDELINE" onblur="fncFo(this,1)" onfocus="fncFo(this,2)"
								TabIndex="310" Runat="server" TextMode="MultiLine" Columns="40" Rows="25" />
						</td>
						<%-- 2013/06/19 mod T.Ono --%>
                        <%-- <td nowrap width="200" height="400"> 
                                <span id="pre" class="preview">&nbsp;<!-- プレビュー領域 --></span>--%>
                        <td nowrap width="400" height="420">
							<asp:label id="pre" class="preview" width="400" height="420" Runat="server"></asp:label><!-- プレビュー領域 -->
						</td>
					</tr>
				</table>
			</div>
			<!-- ▲▲▲ 2011/11/08 H.Uema add 注意事項登録フォーム作成 ▲▲▲ -->
		</form>
		<!-- ▼▼▼ 2011/11/08 H.Uema add タブ切替用変数の設定 ▼▼▼ -->
		<script type="text/javascript">
		// <![CDATA[
			tab.setup = {
				tabs: document.getElementById('tab').getElementsByTagName('li'),
				pages: [
					document.getElementById('page1'),
					document.getElementById('page2')
				]
			}

			tab.init();
		// ]]>
		</script>
		<!-- ▲▲▲ 2011/11/08 H.Uema add タブ切替用変数の設定 ▲▲▲ -->
        <!-- ▼▼▼ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▼▼▼ -->
        <script type="text/javascript">
            function hyojicg() {
                var elm;
                var elmid;
                var i;
                
                radio = document.getElementsByName("rdoHYOJI")

                //項目
                elm = document.getElementById("koumoku").getElementsByTagName("td")
                
//                for(i=0;i<elm.length;i++){
//                    alert(elm.item(i).id  + ":" + elm.item(i).nodeName +":"+ elm.item(i).nodeType +":"+ elm.item(i).nodeValue )
//                }

                if (radio[0].checked) {
                    for (i = 0; i < elm.length; i++) {
                        elmid = String(elm.item(i).id);
                        if (elmid.indexOf("DispAlw") != -1) {
                            document.getElementById(elm.item(i).id).style.display = "block";
                        } else if (elmid.indexOf("DispSpot") != -1) {
                            document.getElementById(elm.item(i).id).style.display = "block";
                        } else if (elmid.indexOf("DispAUTO") != -1) {
                            document.getElementById(elm.item(i).id).style.display = "none";
                        }
                    }
                } else if (radio[1].checked) {
                    for (i = 0; i < elm.length; i++) {
                        elmid = String(elm.item(i).id);
                        if (elmid.indexOf("DispAlw") != -1) {
                            document.getElementById(elm.item(i).id).style.display = "block";
                        } else if (elmid.indexOf("DispSpot") != -1) {
                            document.getElementById(elm.item(i).id).style.display = "none";
                        } else if (elmid.indexOf("DispAUTO") != -1) {
                            document.getElementById(elm.item(i).id).style.display = "block";
                        }
                    }
                }


                //リスト
                for (j = 1; j < 31; j = j + 1) {
                    elm = document.getElementById("list_" + j).getElementsByTagName("td")

                    if (radio[0].checked) {
                        for (i = 0; i < elm.length; i++) {
                            elmid = String(elm.item(i).id);
                            if (elmid.indexOf("DispAlw") != -1) {
                                document.getElementById(elm.item(i).id).style.display = "block";
                            } else if (elmid.indexOf("DispSpot") != -1) {
                                document.getElementById(elm.item(i).id).style.display = "block";
                            } else if (elmid.indexOf("DispAUTO") != -1) {
                                document.getElementById(elm.item(i).id).style.display = "none";
                            }
                        }
                    } else if (radio[1].checked) {
                        for (i = 0; i < elm.length; i++) {
                            elmid = String(elm.item(i).id);
                            if (elmid.indexOf("DispAlw") != -1) {
                                document.getElementById(elm.item(i).id).style.display = "block";
                            } else if (elmid.indexOf("DispSpot") != -1) {
                                document.getElementById(elm.item(i).id).style.display = "none";
                            } else if (elmid.indexOf("DispAUTO") != -1) {
                                document.getElementById(elm.item(i).id).style.display = "block";
                            }
                        }
                    }
                
                
                }

            }
            window.onload = hyojicg;
		</script>
        <!-- ▲▲▲ 2013/05/22 T.Ono add 顧客単位登録機能追加 ▲▲▲ -->
	</body>
</HTML>
