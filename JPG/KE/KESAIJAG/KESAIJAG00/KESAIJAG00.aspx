<%@ Register TagPrefix="cc1" Namespace="JPG.Common.Controls" Assembly="COCNTRLL00" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="KESAIJAG00.aspx.vb" Inherits="JPG.KESAIJAG00.KESAIJAG00" EnableSessionState="ReadOnly" enableViewState="False" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>KESAIJAG00</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:label id="lblScript" runat="server"></asp:label><INPUT id="hdnMyAspx" type="hidden" name="hdnMyAspx" runat="server"><br>
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%">
				<tr>
					<td class="WW">
						<table id="Table2" cellSpacing="2" cellPadding="0" width="900">
							<tr>
								<td width="200">&nbsp;</td>
								<td width="300">&nbsp;</td>
								<td width="220">&nbsp;</td>
								<td width="70">&nbsp;</td>
								<td width="30">&nbsp;</td>
								<td align="right" width="80"><input class="bt-JIK" id="btnExit" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnExit_onclick();"
										type="button" value="終了" name="btnExit" runat="server" tabindex="91">
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
						<table id="Table3" cellSpacing="0" cellPadding="0" width="900">
							<tr>
								<td width="20"></td>
                                <td vAlign="middle" width="710">
								<!--<td vAlign="middle" width="1260">-->                                 <!--2021/10/01saka 2021年度監視改善�E感震器遮断出力に3つ目の帳票を追加して文字表示枠広げる予定だったが帳票は増やさないこととなった-->
									<table id="Table4" cellSpacing="0" cellPadding="2" width="100%">
										<tr>
											<td class="TITLE" vAlign="middle">災害対応帳票+対応結果明細出力</td>
										</tr>
									</table>
								</td>
								<td width="170">&nbsp;</td>
							</tr>
						</table>
						<INPUT id="hdnPopcrtl" type="hidden" name="hdnPopcrtl" runat="server"><INPUT id="hdnKensaku" type="hidden" name="hdnKensaku" runat="server">
					</td>
				</tr>
			</table>
			<hr>

			<DIV style="LEFT: 13px; WIDTH: 900px; POSITION: absolute; TOP: 100px; HEIGHT: 30px" ms_positioning="FlowLayout">
				<table id="Table5" cellSpacing="1" cellPadding="2">
					<tr>
						<td align="right" colSpan="2" width="90">クライアント</td>
                           <td><asp:textbox id="txtKURACD_From" tabIndex="-1" runat="server" BorderWidth="1px" BorderStyle="Solid"
								BorderColor="Black" cssclass="c-rNM" Width="280px"></asp:textbox><INPUT class="bt-KS" id="btnKURACD_From" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('0');"
								tabIndex="1" type="button" value="▼" name="btnKURACD_From" runat="server">&nbsp;〜&nbsp;
                           </td>
						<td><asp:textbox id="txtKURACD_To" tabIndex="-1" runat="server" BorderWidth="1px" BorderStyle="Solid"
								BorderColor="Black" cssclass="c-rNM" Width="280px"></asp:textbox><INPUT class="bt-KS" id="btnKURACD_To" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('1');"
								tabIndex="2" type="button" value="▼" name="btnKURACD_To" runat="server">
						</td>
						<td><INPUT id="hdnKURACD_From" type="hidden" name="hdnKURACD_From" runat="server">
						    <INPUT id="hdnKURACD_To" type="hidden" name="hdnKURACD_To" runat="server"></td>
					</tr>
				</table>
			</DIV>

			<DIV style="LEFT: 13px; WIDTH: 900px; POSITION: absolute; TOP: 130px; HEIGHT: 30px" ms_positioning="FlowLayout">
				<table id="Table6" cellSpacing="1" cellPadding="2" >
					<tr>
						<td align="right" colSpan="2" width="90">ＪＡ</td>
                           <td><asp:textbox id="txtJACD_From" tabIndex="-1" runat="server" BorderWidth="1px" BorderStyle="Solid"
								BorderColor="Black" cssclass="c-rNM" Width="280px"></asp:textbox><INPUT class="bt-KS" id="btnJACD_From" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('2');"
								tabIndex="3" type="button" value="▼" name="btnJACD_From" runat="server">&nbsp;〜&nbsp;</td>
						<td><asp:textbox id="txtJACD_To" tabIndex="-1" runat="server" BorderWidth="1px" BorderStyle="Solid"
								BorderColor="Black" cssclass="c-rNM" Width="280px"></asp:textbox><INPUT class="bt-KS" id="btnJACD_To" onblur="fncFo(this,5)" onfocus="fncFo(this,2)" onclick="return btnPopup_onclick('3');"
								tabIndex="4" type="button" value="▼" name="btnJACD_To" runat="server"></td>
						<td><input id="hdnJACD_From" type="hidden" name="hdnJACD_From" runat="server">
						    <input id="hdnJACD_To" type="hidden" name="hdnJACD_To" runat="server">
                            <input id="hdnJACD_From_CLI" type="hidden" name="hdnJACD_From_CLI" runat="server" />
                            <input id="hdnJACD_To_CLI" type="hidden" name="hdnJACD_To_CLI" runat="server" /></td>
					</tr>
				</table>
			</DIV>
			<DIV style="LEFT: 13px; WIDTH: 500px; POSITION: absolute; TOP: 160px; HEIGHT: 30px" ms_positioning="FlowLayout">
				<table id="Table7" cellSpacing="1" cellPadding="2">
					<tr>
						<td align="right" colSpan="2" width="90">対象期間</td>
						<td><asp:textbox id="txtTRGDATE_From" onkeydown="fncFc(this)" onblur="fncFo_date(this,1)" onfocus="fncFo_date(this,2)"
								tabIndex="5" runat="server" cssclass="c-f" Width="72px" MaxLength="8"></asp:textbox>
								<INPUT id="btnCalendar1" name="btnCalendar1" class="bt-KS" onblur="fncFo(this,5)" onfocus="fncFo(this,2)"
								onclick="btnCalendar_onclick(1);" type="button" value="..." tabIndex="6"> &nbsp;〜&nbsp;
							<asp:textbox id="txtTRGDATE_To" onkeydown="fncFc(this)" onblur="fncFo_date(this,1)" onfocus="fncFo_date(this,2)"
								tabIndex="7" runat="server" cssclass="c-f" Width="72px" MaxLength="8"></asp:textbox>
								<INPUT id="btnCalendar2" name="btnCalendar2" class="bt-KS" onblur="fncFo(this,5)" onfocus="fncFo(this,2)"
								onclick="btnCalendar_onclick(2);" type="button" value="..." tabIndex="8">
                            <INPUT id="hdnCalendar" type="hidden" name="hdnCalendar" runat="server">
						</td>
			       </tr>
                </table>                   <!--2021/10/01ADDsaka 2021年度監視改善�E-->
            </DIV>                         <!--2021/10/01ADDsaka 2021年度監視改善�EStartラジオボタンを1つ→2つにするので日付範囲と分けてデザイン-->
			<DIV style="LEFT: 330px; WIDTH: 1500px; POSITION: absolute; TOP: 160px; HEIGHT: 30px" ms_positioning="FlowLayout">

                <table class="W" id="Table14" cellSpacing="1" cellPadding="2" >
                    <tr>
                        <td>
							<input id="rdoKIKAN1" onblur="fncFo(this,4)" onfocus="fncFo(this,2)" tabIndex="9" type="radio"
								value="1" name="rdoKIKAN" runat="server" CHECKED onclick="chkRadio_KIKAN()" onkeydown="fncFc(this)"><label for="rdoKIKAN1">対応完了日&nbsp;&nbsp;</label>
                            &nbsp;&nbsp; <input id="rdoKIKAN2" onblur="fncFo(this,4)" onfocus="fncFo(this,2)" type="radio"
									value="2" name="rdoKIKAN" runat="server" onclick="chkRadio_KIKAN()" onkeydown="fncFc(this)"><label for="rdoKIKAN2">受信日（未処理データも出力）&nbsp;&nbsp;&nbsp;&nbsp;</label>
						</td>
					</tr>                                  <!--2021/10/01ADDsaka 2021年度監視改善�EEnd -->
				</table>
			</DIV>
               <DIV style="LEFT: 13px; WIDTH: 320px; POSITION: absolute; TOP: 190px; HEIGHT: 30px" ms_positioning="FlowLayout">
				<table id="Table8" cellSpacing="1" cellPadding="2">
					<tr>
						<td align="right" colSpan="2" width="90">対象時間</td>
						<td><asp:textbox id="txtTRGTIME_From" onkeydown="fncFc(this)" onblur="fncFo_time(this,1)" onfocus="fncFo_time(this,2)"
									tabIndex="10" runat="server" Width="45px" cssclass="c-f" MaxLength="4"></asp:textbox>
							    &nbsp;〜&nbsp;
							    <asp:textbox id="txtTRGTIME_To" onkeydown="fncFc(this)" onblur="fncFo_time(this,1)" onfocus="fncFo_time(this,2)"
									tabIndex="11" runat="server" Width="45px" cssclass="c-f" MaxLength="4"></asp:textbox>
						</td>
					</tr>
				</table>
			</DIV>
               <DIV style="LEFT: 13px; WIDTH: 500px; POSITION: absolute; TOP: 220px; HEIGHT: 30px" ms_positioning="FlowLayout">
				<table id="Table9" cellSpacing="1" cellPadding="2">
					<tr>
						<td align="right" width="90">対応区分</td>
						<td>
							<INPUT id="chkTAI_TEL" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2) "
								tabIndex="12" type="checkbox" name="chkTAI_TEL" runat="server" CHECKED>
								<label for="chkTAI_TEL">電話</label>
						</td>
						<td>
							<INPUT id="chkTAI_SHU" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
								tabIndex="13" type="checkbox" name="chkTAI_SHU" runat="server" CHECKED>
								<label for="chkTAI_SHU">出動</label>
						</td>
						<td>
							<INPUT id="chkTAI_JUF" onkeydown="fncFc(this)" onblur="fncFo(this,4)" onfocus="fncFo(this,2)"
								tabIndex="14" type="checkbox" name="chkTAI_JUF" runat="server" disabled="disabled">
								<label for="chkTAI_JUF">重複</label>
						</td>
					</tr>
				</table>
			</DIV>
               <DIV style="LEFT: 13px; WIDTH: 600px; POSITION: absolute; TOP: 250px; HEIGHT: 30px" ms_positioning="FlowLayout">
				<table id="Table10" cellSpacing="1" cellPadding="2">
					<tr>
						<td align="right" width="90">出力項目</td>
						<td class="W" style="height:30px;vertical-align:middle">
							<input id="rdoOUTLIST3" onblur="fncFo(this,4)" onfocus="fncFo(this,2)" tabIndex="15" type="radio"
								value="3" name="rdoOUTLIST"  CHECKED runat="server" onkeydown="fncFc(this)"><label for="rdoOUTLIST3">日次報告と同じ(出動会社なし)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                        </td>
                        <td>&nbsp;※対応結果明細</td>
					</tr>
				</table>
			</DIV>
            <!-- 2020/09/15 T.Ono add 監視改善2020 START -->
            <DIV style="LEFT: 13px; WIDTH: 500px; POSITION: absolute; TOP: 290px; HEIGHT: 30px" ms_positioning="FlowLayout">
                <table id="Table13" cellSpacing="1" cellPadding="2">
                    <tr>
                    <td align="right" width="90">個人情報&nbsp;</td>                    
                    <td class="W" style="height:30px;width:181px;vertical-align:middle">
	                    <input id="rdoKOJIN1" onblur="fncFo(this,4)" onfocus="fncFo(this,2)" tabIndex="15" type="radio"
		                    value="0" name="rdoKOJIN"  CHECKED runat="server" onkeydown="fncFc(this)"><label for="rdoOUTLIST3">なし&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
	                    <input id="rdoKOJIN2" onblur="fncFo(this,4)" ondocus="fncFo(this,2)" tabIndex="15" type="radio"
		                    value="1" name="rdoKOJIN" runat="server" onkeydown="fncFc(this)"><label for="rdoKOJIN2">通常版</label>
                    </td>
                    <td>&nbsp;※対応結果明細</td>
                    </tr>
                </table>
            </DIV>
            <!-- 2020/09/15 T.Ono add 監視改善2020 END -->
            <!-- 2020/09/15 T.Ono mod 監視改善2020 DIV style="LEFT: 13px; WIDTH: 500px; POSITION: absolute; TOP: 290px; HEIGHT: 30px" ms_positioning="FlowLayout" -->
            <DIV style="LEFT: 13px; WIDTH: 500px; POSITION: absolute; TOP: 330px; HEIGHT: 30px" ms_positioning="FlowLayout">
				<table id="Table11" cellSpacing="1" cellPadding="2">

					<td align="right" width="90">作動原因&nbsp;</td>
					<td>
                           <cc1:ctlcombo id="cboTSADCD" onkeydown="fncFc(this)" onblur="fncFo(this,1)" onmousedown="fncFo(this,2)" onfocus="fncFo(this,2)" onChange="fncSetFocus()"
							tabindex="16" runat="server" width="360px" cssclass="cb"></cc1:ctlcombo>
					</td>
				</table>
			</DIV>
            <!-- 2020/09/15 T.Ono mod 監視改善2020 DIV style="LEFT: 500px; WIDTH: 500px; POSITION: absolute; TOP: 290px; HEIGHT: 30px" ms_positioning="FlowLayout" -->
            <DIV style="LEFT: 500px; WIDTH: 500px; POSITION: absolute; TOP: 330px; HEIGHT: 30px" ms_positioning="FlowLayout">
				<table id="Table12" cellSpacing="1" cellPadding="2">
					<tr>
						<td><INPUT language="javascript" class="bt-RNW" id="btnOutput" onblur="return fncFo(this,5);"
						onfocus="fncFo(this,2)" onclick="return btnSelect_onclick('1');" tabIndex="21" type="button"
						value="集計表" name="btnOutput" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;	
						</td>						
                           <td><INPUT language="javascript" class="bt-RNW" id="btnSelect" onblur="return fncFo(this,5);"
						onfocus="fncFo(this,2)" onclick="return btnSelect_onclick('2');" tabIndex="22" type="button"
						value="明細表" name="btnSelect" runat="server">
						</td>
					</tr>
				</table>
			</DIV>
		</form>
	</body>
</HTML>
