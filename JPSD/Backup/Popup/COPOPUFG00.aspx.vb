Option Explicit On
Option Strict On

Imports Common

Imports System.Text
Imports System.Text.RegularExpressions

'******************************************************************************
' ポップアップ (一覧部)
'******************************************************************************
' 変更履歴
' 2013/12/12 T.Ono add 監視改善2013 クライアント・JA選択ポップアップ追加のためJPGからコピー

Partial Class COPOPUFG00
    Inherits System.Web.UI.Page
    Protected WithEvents dbSet As System.Data.DataSet

    Protected COPOPUPG00_C As COPOPUPG00

    '******************************************************************************
    ' HttpHeader
    '******************************************************************************
    Protected HttpHeaderC As New CHttpHeader

    '******************************************************************************
    ' ScriptMessage
    '******************************************************************************
    Private strMsg As New StringBuilder("")      '//<script>は必要なし

    '******************************************************************************
    ' Render
    '******************************************************************************
    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        MyBase.Render(writer)

        Dim strWrite As New StringBuilder("")

        strWrite.Append("<script language='JavaScript'>")
        strWrite.Append(strMsg.ToString())
        strWrite.Append("</script>")
        writer.Write(strWrite.ToString())
    End Sub

#Region " Web フォーム デザイナで生成されたコード "

    'この呼び出しは Web フォーム デザイナで必要です。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.dbSet = New System.Data.DataSet
        CType(Me.dbSet, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'dbSet
        '
        Me.dbSet.DataSetName = "NewDataSet"
        Me.dbSet.Locale = New System.Globalization.CultureInfo("ja-JP")
        CType(Me.dbSet, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub


    'メモ : 次のプレースホルダ宣言は Web フォーム デザイナで必要です。
    '削除および移動しないでください。
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        ' CODEGEN: このメソッド呼び出しは Web フォーム デザイナで必要です。
        ' コード エディタを使って変更しないでください。
        InitializeComponent()
    End Sub

#End Region

    '******************************************************************************
    ' Page_Load
    '******************************************************************************
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                                                                Handles MyBase.Load
        '//------------------------------------------
        '//　HTTPヘッダを送信
        HttpHeaderC.mNoCache(Response)

        '//------------------------------------------
        '// Script書込み用変数宣言
        Dim strScript As New StringBuilder("")
        Dim cscript1 As New CScript
        Dim strScriptPath As String
        strScriptPath = ConfigurationSettings.AppSettings("SCRIPTPATH")
        '------------------------------------------------------------------------------
        '<TODO>HTML内に必要なJavaScript/CSSはここで[strScript]変数に格納後
        '      画面上[lblScript]に書き込みを行います(SPANタグとしてクライアントにスクリプトが
        '      出力されます。)
        '//------------------------------------------
        '//　JavaScript格納
        strScript.Append("<Script language=javascript>")
        '＜一覧スクリプト＞
        strScript.Append(cscript1.mWriteScript(strScriptPath & "\" & "fncBG.js"))
        strScript.Append("</Script>")
        '//------------------------------------------
        '//　Css格納
        strScript.Append("<Style>")
        strScript.Append(cscript1.mWriteScript(strScriptPath & "\" & "CssIframe.css"))
        strScript.Append("</Style>")
        '//------------------------------------------
        '//　Script書込み
        lblScript.Text = strScript.ToString

        '********************************************
        '//------------------------------------------
        '// 呼び出し元クラスのインスタンス作成
        COPOPUPG00_C = CType(Context.Handler, COPOPUPG00)
        '//------------------------------------------
        '********************************************

        '********************************************
        '//------------------------------------------
        '// Select文の作成
        Dim SQLC As New JPSDCSQL.CSQL
        Dim SqlParamC As New CSQLParam
        Dim strSQL As New StringBuilder
        Dim strDBFlg As String = ""

        '// データの取得
        strSQL = New StringBuilder("")
        Select Case COPOPUPG00_C.pListCd
            Case "CLI"                      'クライアント一覧を表示      ※DBリンク
                Call mMakeSQL_CLIMAS(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "JAJASS"                   'ＪＡ/ＪＡ支所一覧を表示(ＪＡ/ＪＡ支所形式で出力)
                Call mMakeSQL_JAJASS(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "JA"                       'ＪＡ一覧を表示
                Call mMakeSQL_JA(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "JASS"                     'ＪＡ支所一覧を表示
                Call mMakeSQL_JASS(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "JASS2"                     'ＪＡ支所一覧を表示(JA名も出力する)
                Call mMakeSQL_JASS2(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "KYO"                      '供給センター一覧を表示
                Call mMakeSQL_KYO(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "KANSHI"                   '監視センター一覧を表示
                Call mMakeSQL_KANSHI(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "SYUTUDOU"                 '出動会社一覧を表示
                Call mMakeSQL_SYUTUDOU(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "PULLKBN"                  'プルダウン区分一覧を表示
                Call mMakeSQL_PULLKBN(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "PULLCODE"                 'プルダウンコード一覧を表示
                Call mMakeSQL_PULLCODE(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "TKTANCD"                  '監視センター担当者一覧を表示(クライアントコードより出力)
                Call mMakeSQL_TKTANCD(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "TKTANCDKN"                '監視センター担当者一覧を表示(監視センターコードより出力)
                Call mMakeSQL_TKTANCDKN(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "TSTANCD"                  '出動会社担当者一覧 '2014/10/23 H.Hosoda add 2014改善開発 No11
                Call mMakeSQL_TSTANCD(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case "HANG"                     '販売事業者グループ一覧 '2014/10/21 H.Hosoda add 2014改善開発 No10
                Call mMakeSQL_HANG(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
            Case Else                       ' エラー指示
                Call mMakeSQL_ELSE(strSQL, SqlParamC)
                dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)
                strDBFlg = "NODATA"
        End Select

        '// 取得データの編集を行う--------------------
        Dim DateFncC As New CDateFnc
        If Convert.ToString(dbSet.Tables(0).Rows(0).Item(0)) = "XYZ" Then
            dbSet.Tables(0).Rows(0).Item("CODE") = ""
            dbSet.Tables(0).Rows(0).Item("NAME") = ""
            strDBFlg = "NODATA"
        End If
        '// リピータにバインドする--------------------
        rptIframe.DataBind()
        '//------------------------------------------
        '********************************************

        '********************************************
        '//------------------------------------------
        '// データ選択時処理のJavaScript発行
        '2014/10/30 H.Hosoda mod 2014改善開発 No11 START
        'strMsg.Append("function fncPutData(strcd,strnm){")
        strMsg.Append("function fncPutData(strcd,strnm,strnmonly){")
        '2014/10/30 H.Hosoda mod 2014改善開発 No11 END
        If strDBFlg = "" Then
            strMsg.Append("if(strcd==' '){strcd=='';}")
            strMsg.Append("if(strnm==' '){strnm=='';}")
            strMsg.Append("if(strnmonly==' '){strnmonly=='';}") '2014/10/21 H.Hosoda add 2014改善開発 No11
            '// コードデータのセット
            If COPOPUPG00_C.pBackCode <> "" Then
                strMsg.Append("obj1=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pBackCode & """);")
                strMsg.Append("if(obj1!=null) {")
                strMsg.Append("if(obj1.value!=strcd) {")
                If COPOPUPG00_C.pClear1.Length > 0 Then
                    strMsg.Append("obj4=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pClear1 & """);")
                    strMsg.Append("obj4.value='';")
                End If
                If COPOPUPG00_C.pClear2.Length > 0 Then
                    strMsg.Append("obj5=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pClear2 & """);")
                    strMsg.Append("obj5.value='';")
                End If
                If COPOPUPG00_C.pClear3.Length > 0 Then
                    strMsg.Append("obj6=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pClear3 & """);")
                    strMsg.Append("obj6.value='';")
                End If
                If COPOPUPG00_C.pClear4.Length > 0 Then
                    strMsg.Append("obj7=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pClear4 & """);")
                    strMsg.Append("obj7.value='';")
                End If
                If COPOPUPG00_C.pClear5.Length > 0 Then
                    strMsg.Append("obj8=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pClear5 & """);")
                    strMsg.Append("obj8.value='';")
                End If
                If COPOPUPG00_C.pClear6.Length > 0 Then
                    strMsg.Append("obj9=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pClear6 & """);")
                    strMsg.Append("obj9.value='';")
                End If
                strMsg.Append("}")
                strMsg.Append("}")
                strMsg.Append("obj1.value=strcd;")
            End If
            '// 名前データのセット
            If COPOPUPG00_C.pBackName <> "" Then
                strMsg.Append("obj2=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pBackName & """);")
                strMsg.Append("obj2.value=strnm;")
            End If
            '2014/10/30 H.Hosoda add 2014改善開発 No11 START
            If COPOPUPG00_C.pBackNameOnly <> "" Then
                strMsg.Append("obj2=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pBackNameOnly & """);")
                strMsg.Append("obj2.value=strnmonly;")
            End If
            '2014/10/30 H.Hosoda add 2014改善開発 No11 END
            '// フォーカスのセット
            If COPOPUPG00_C.pBackFocs <> "" Then
                strMsg.Append("obj3=parent.opener.frames(""data"").document.getElementById(""" & COPOPUPG00_C.pBackFocs & """);")
                strMsg.Append("obj3.focus();")
            End If
            '// JavaScriptの実行
            If COPOPUPG00_C.pBackScript <> "" Then
                strMsg.Append("parent.opener.frames(""data"")." & COPOPUPG00_C.pBackScript & "();")
            End If
            strMsg.Append("parent.window.close();")
        End If
        strMsg.Append("}")
        '//------------------------------------------
        '********************************************
    End Sub

    '******************************************************************************
    ' クライアント一覧
    '******************************************************************************
    Private Sub mMakeSQL_CLIMAS(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        '関連する監視センターのクライアントコードのみ出力すること
        'ＡＤ認証された「使用可能監視センター」に所属するクライアントを出力
        Dim i As Integer
        Dim arrTemp() As String
        Dim strCenter As String = ""
        arrTemp = COPOPUPG00_C.pCode1.Split(Convert.ToChar(","))
        For i = 0 To arrTemp.Length - 1
            If strCenter.Length > 0 Then
                strCenter = strCenter & ","
            End If
            strCenter = strCenter & "'" & arrTemp(i) & "'"
        Next
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("CLI_CD AS CODE, ")
        strSQL.Append("CLI_NAME AS NAME, ")
        strSQL.Append("CLI_CD || ' : ' || CLI_NAME AS CDNM ")
        strSQL.Append("FROM CLIMAS ")
        If COPOPUPG00_C.pCode1.Length > 0 Then
            strSQL.Append("WHERE KANSI_CODE IN (" & strCenter & ") ")
        End If
        strSQL.Append("ORDER BY CODE ")
    End Sub

    '******************************************************************************
    ' ＪＡ/ＪＡ支所一覧
    '******************************************************************************
    Private Sub mMakeSQL_JAJASS(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("HAN_CD AS CODE, ")
        strSQL.Append("JAS_NAME AS NAME, ")
        strSQL.Append("HAN_CD || ' : ' || JA_NAME || '/' || JAS_NAME AS CDNM ")
        strSQL.Append("FROM HN2MAS ")
        strSQL.Append("WHERE CLI_CD = :CLI_CD ")
        strSQL.Append("AND NVL(DEL_FLG,'0') <> '1' ") ' 2008/10/29 T.Watabe add
        '''''同マスタにはＪＡ支所データしか存在しない
        ''''strSQL.Append("  AND LENGTH(HAN_CD) = TO_NUMBER(HAN_KETA) + TO_NUMBER(JA_KETA) ")
        strSQL.Append("ORDER BY CODE ")

        SqlParamC.fncSetParam("CLI_CD", True, COPOPUPG00_C.pCode1)
    End Sub

    '******************************************************************************
    ' ＪＡ一覧
    '******************************************************************************
    Private Sub mMakeSQL_JA(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("DISTINCT ")
        strSQL.Append("JA_CD AS CODE, ")
        strSQL.Append("JA_NAME AS NAME, ")
        strSQL.Append("JA_CD || ' : ' || JA_NAME  AS CDNM ")
        strSQL.Append("FROM HN2MAS ")
        strSQL.Append("WHERE CLI_CD = :CLI_CD ")
        strSQL.Append("AND NVL(DEL_FLG,'0') <> '1' ") ' 2008/10/29 T.Watabe add
        '--- ↓2005/05/24 MOD Falcon↓ ---
        If COPOPUPG00_C.pCode2.Length > 0 Then
            strSQL.Append("  AND HAISO_CD = :HAISO_CD ")
        End If
        '--- ↑2005/05/24 MOD Falcon↑ ---
        ''''DISTINCTにてＪＡの抽出とする(以上はマスタ精査を行う)
        ''''strSQL.Append("  AND LENGTH(HAN_CD) = TO_NUMBER(HAN_KETA) ")
        strSQL.Append("ORDER BY CODE ")

        SqlParamC.fncSetParam("CLI_CD", True, COPOPUPG00_C.pCode1)
        '--- ↓2005/05/24 MOD Falcon↓ ---
        If COPOPUPG00_C.pCode2.Length > 0 Then
            SqlParamC.fncSetParam("HAISO_CD", True, COPOPUPG00_C.pCode2)
        End If
        '--- ↑2005/05/24 MOD Falcon↑ ---
    End Sub

    '******************************************************************************
    ' ＪＡ支所一覧
    '******************************************************************************
    Private Sub mMakeSQL_JASS(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("HAN_CD AS CODE, ")
        strSQL.Append("JAS_NAME AS NAME, ")
        strSQL.Append("HAN_CD || ' : ' || JAS_NAME AS CDNM ")
        strSQL.Append("FROM HN2MAS ")
        strSQL.Append("WHERE CLI_CD = :CLI_CD ")
        strSQL.Append("AND NVL(DEL_FLG,'0') <> '1' ") ' 2008/10/29 T.Watabe add
        If COPOPUPG00_C.pCode2.Length > 0 Then
            strSQL.Append("  AND JA_CD = :JA_CD ")
        End If
        '''''同マスタにはＪＡ支所データしか存在しない
        ''''strSQL.Append("  AND LENGTH(HAN_CD) = TO_NUMBER(HAN_KETA) + TO_NUMBER(JA_KETA) ")
        strSQL.Append("ORDER BY CODE ")

        SqlParamC.fncSetParam("CLI_CD", True, COPOPUPG00_C.pCode1)
        If COPOPUPG00_C.pCode2.Length > 0 Then
            SqlParamC.fncSetParam("JA_CD", True, COPOPUPG00_C.pCode2)
        End If
    End Sub

    '******************************************************************************
    ' ＪＡ支所一覧（ＪＡ名を出力する）
    '******************************************************************************
    Private Sub mMakeSQL_JASS2(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("HAN_CD AS CODE, ")
        '--- ↓2005/05/21 ADD Falcon↓ ---
        strSQL.Append("JA_NAME || JAS_NAME AS NAME, ")
        strSQL.Append("HAN_CD || ' : ' || JA_NAME || JAS_NAME AS CDNM ")
        '--- ↑2005/05/21 ADD Falcon↑ ---
        strSQL.Append("FROM HN2MAS ")
        strSQL.Append("WHERE CLI_CD = :CLI_CD ")
        strSQL.Append("AND NVL(DEL_FLG,'0') <> '1' ") ' 2008/10/29 T.Watabe add
        If COPOPUPG00_C.pCode2.Length > 0 Then
            strSQL.Append("  AND JA_CD = :JA_CD ")
        End If
        '''''同マスタにはＪＡ支所データしか存在しない
        ''''strSQL.Append("  AND LENGTH(HAN_CD) = TO_NUMBER(HAN_KETA) + TO_NUMBER(JA_KETA) ")
        strSQL.Append("ORDER BY CODE ")

        SqlParamC.fncSetParam("CLI_CD", True, COPOPUPG00_C.pCode1)
        If COPOPUPG00_C.pCode2.Length > 0 Then
            SqlParamC.fncSetParam("JA_CD", True, COPOPUPG00_C.pCode2)
        End If
    End Sub

    '******************************************************************************
    ' 供給センター一覧
    '******************************************************************************
    Private Sub mMakeSQL_KYO(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("HAISO_CD AS CODE, ")
        strSQL.Append("NAME AS NAME, ")
        strSQL.Append("HAISO_CD || ' : ' || NAME AS CDNM ")
        strSQL.Append("FROM HAIMAS ")
        strSQL.Append("WHERE KEN_CD = :KEN_CD ")
        strSQL.Append("ORDER BY CODE ")

        SqlParamC.fncSetParam("KEN_CD", True, COPOPUPG00_C.pCode1)
    End Sub

    '******************************************************************************
    ' 監視センター一覧
    '******************************************************************************
    Private Sub mMakeSQL_KANSHI(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        '関連する監視センターのコードのみ出力すること
        'ＡＤ認証された「使用可能監視センター」の監視センターを出力
        Dim i As Integer
        Dim arrTemp() As String
        Dim strCenter As String = ""
        arrTemp = COPOPUPG00_C.pCode1.Split(Convert.ToChar(","))
        For i = 0 To arrTemp.Length - 1
            If strCenter.Length > 0 Then
                strCenter = strCenter & ","
            End If
            strCenter = strCenter & "'" & arrTemp(i) & "'"
        Next

        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("KANSI_CD AS CODE, ")
        strSQL.Append("KANSI_NAME AS NAME, ")
        strSQL.Append("KANSI_CD || ' : ' || KANSI_NAME AS CDNM ")
        strSQL.Append("FROM KANSIMAS ")
        '--- ↓2005/04/29 DEL Falcon↓ ---
        'If arrTemp.Length = 0 Then
        '    strSQL.Append("WHERE KANSI_CD = '' ")
        'Else
        '    strSQL.Append("WHERE KANSI_CD IN (" & strCenter & ") ")
        'End If
        '--- ↑2005/04/29 DEL Falcon↑ ---
        '--- ↓2005/04/29 MOD Falcon↓ ---
        If COPOPUPG00_C.pCode1.Length > 0 Then
            strSQL.Append("  WHERE KANSI_CD IN (" & strCenter & ") ")
        End If
        '--- ↑2005/04/29 MOD Falcon↑ ---
        strSQL.Append("ORDER BY CODE ")

    End Sub

    '******************************************************************************
    ' 出動会社一覧
    '******************************************************************************
    Private Sub mMakeSQL_SYUTUDOU(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        '--- ↓2005/04/29 ADD Falcon↓ ---
        '関連する監視センターの出動会社コードのみ出力すること
        'ＡＤ認証された「使用可能監視センター」に所属する出動会社を出力
        Dim i As Integer
        Dim arrTemp() As String
        Dim strCenter As String = ""
        arrTemp = COPOPUPG00_C.pCode1.Split(Convert.ToChar(","))
        For i = 0 To arrTemp.Length - 1
            If strCenter.Length > 0 Then
                strCenter = strCenter & ","
            End If
            strCenter = strCenter & "'" & arrTemp(i) & "'"
        Next
        '--- ↑2005/04/29 ADD Falcon↑ ---
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        '--- ↓2005/05/21 MOD Falcon↓ ---
        '出動会社＋拠点にコードと名称を修正
        'strSQL.Append("SHUTU_CD AS CODE, ")
        'strSQL.Append("KAISYA_NAME AS NAME, ")
        'strSQL.Append("SHUTU_CD || ' : ' || KAISYA_NAME AS CDNM ")
        strSQL.Append("SHUTU_CD || KYOTEN_CD AS CODE, ")
        strSQL.Append("KAISYA_NAME || KYOTEN_NAME AS NAME, ")
        strSQL.Append("SHUTU_CD || KYOTEN_CD || ' : ' || KAISYA_NAME || KYOTEN_NAME AS CDNM ")
        '--- ↑2005/05/21 MOD Falcon↑ ---
        strSQL.Append("FROM SHUTUDOMAS ")
        strSQL.Append("WHERE KUBUN = :KUBUN ")
        '--- ↓2005/04/29 ADD Falcon↓ ---
        If COPOPUPG00_C.pCode1.Length > 0 Then
            strSQL.Append("  AND KANSI_CD IN (" & strCenter & ") ")
        End If
        '--- ↑2005/04/29 ADD Falcon↑ ---
        '--- ↓2005/05/21 ADD Falcon↓ ---
        strSQL.Append("ORDER BY CODE")
        '--- ↑2005/05/21 ADD Falcon↑ ---
        SqlParamC.fncSetParam("KUBUN", True, "1")
    End Sub

    '******************************************************************************
    ' プルダウン区分名一覧
    '******************************************************************************
    Private Sub mMakeSQL_PULLKBN(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("CD AS CODE, ")
        strSQL.Append("NAME AS NAME, ")
        strSQL.Append("NAME AS CDNM ")          'プルダウンマスタにて使用する。コンボ系ではなく入力補助ポップ系
        strSQL.Append("FROM ")
        strSQL.Append("M06_PULLDOWN ")
        strSQL.Append("WHERE KBN = '00' ")
        If COPOPUPG00_C.pCode1.Length > 0 Then
            strSQL.Append("  AND CD LIKE :CD || '%' ")
        End If
        strSQL.Append("ORDER BY CODE ")

        If COPOPUPG00_C.pCode1.Length > 0 Then
            SqlParamC.fncSetParam("CD", True, COPOPUPG00_C.pCode1)
        End If
    End Sub

    '******************************************************************************
    ' プルダウンコード一覧
    '******************************************************************************
    Private Sub mMakeSQL_PULLCODE(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM, ")
        '--- ↓2005/04/23 MOD　Falcon↓ -----------------
        'strSQL.Append("' ' AS JUNJO ")
        strSQL.Append("0 AS JUNJO ")
        '--- ↑2005/04/23 MOD　Falcon↑ -----------------
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("CD AS CODE, ")
        strSQL.Append("NAME AS NAME, ")
        strSQL.Append("NAME AS CDNM, ")          'プルダウンマスタにて使用する。コンボ系ではなく入力補助ポップ系
        '--- ↓2005/04/23 MOD　Falcon↓ -----------------
        'strSQL.Append("TO_CHAR(DISP_NO) AS JUNJO ")
        strSQL.Append("TO_NUMBER(NVL(DISP_NO,9999)) AS JUNJO ")
        '--- ↑2005/04/23 MOD　Falcon↑ -----------------
        strSQL.Append("FROM ")
        strSQL.Append("M06_PULLDOWN ")

        If (COPOPUPG00_C.pCode1.Length > 0) Or (COPOPUPG00_C.pCode2.Length > 0) Then
            strSQL.Append("WHERE ")
        End If
        If COPOPUPG00_C.pCode1.Length > 0 Then
            strSQL.Append(" KBN = :KBN ")
            If COPOPUPG00_C.pCode2.Length > 0 Then
                strSQL.Append(" AND ")
            End If
        End If
        If COPOPUPG00_C.pCode2.Length > 0 Then
            strSQL.Append(" CD LIKE :CD || '%' ")
        End If
        '--- ↓2005/04/23 DEL　Falcon↓ -----------------
        'strSQL.Append("ORDER BY JUNJO ")
        '--- ↑2005/04/23 DEL　Falcon↑ -----------------
        '--- ↓2005/04/23 MOD　Falcon↓ -----------------
        strSQL.Append("ORDER BY JUNJO,CODE ")
        '--- ↑2005/04/23 MOD　Falcon↑ -----------------

        If COPOPUPG00_C.pCode1.Length > 0 Then
            SqlParamC.fncSetParam("KBN", True, COPOPUPG00_C.pCode1)
        End If
        If COPOPUPG00_C.pCode2.Length > 0 Then
            SqlParamC.fncSetParam("CD", True, COPOPUPG00_C.pCode2)
        End If
    End Sub

    '******************************************************************************
    ' 監視センター担当者一覧
    '******************************************************************************
    Private Sub mMakeSQL_TKTANCD(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        '--- ↓2005/04/23 ADD　Falcon↓ -----------------
        strSQL.Append("0 AS DISP_NO, ")
        '--- ↑2005/04/23 ADD　Falcon↑ -----------------
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("TN.TANCD AS CODE, ")
        strSQL.Append("TN.TANNM AS NAME, ")
        '--- ↓2005/04/23 ADD　Falcon↓ -----------------
        strSQL.Append("TO_NUMBER(NVL(TN.DISP_NO,9999)) AS DISP_NO, ")
        '--- ↑2005/04/23 ADD　Falcon↑ -----------------
        strSQL.Append("TN.TANCD || ' : ' || TN.TANNM  AS CDNM ")
        strSQL.Append("FROM CLIMAS CL, ")
        strSQL.Append("     M05_TANTO TN ")
        strSQL.Append("WHERE CL.CLI_CD = :CLI_CD ")
        strSQL.Append("  AND TN.KBN = '1' ")
        strSQL.Append("  AND TN.KURACD = 'ZZZZ' ")
        strSQL.Append("  AND TN.CODE = CL.KANSI_CODE ")
        '監視センターコードが未指定の場合は空となる
        '--- ↓2005/04/23 DEL　Falcon↓ -----------------
        'strSQL.Append("ORDER BY CODE ")
        '--- ↑2005/04/23 DEL　Falcon↑ -----------------
        '--- ↓2005/04/23 MOD　Falcon↓ -----------------
        strSQL.Append("ORDER BY DISP_NO,CODE ")
        '--- ↑2005/04/23 MOD　Falcon↑ -----------------

        'クライアントコードの設定
        SqlParamC.fncSetParam("CLI_CD", True, COPOPUPG00_C.pCode1)
    End Sub

    '******************************************************************************
    ' 監視センター担当者一覧（監視センターコードより出力）
    '******************************************************************************
    Private Sub mMakeSQL_TKTANCDKN(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        '関連する監視センターのクライアントコードのみ出力すること
        'ＡＤ認証された「使用可能監視センター」の監視センターを出力
        Dim i As Integer
        Dim arrTemp() As String
        Dim strCenter As String = ""
        arrTemp = COPOPUPG00_C.pCode1.Split(Convert.ToChar(","))
        For i = 0 To arrTemp.Length - 1
            If strCenter.Length > 0 Then
                strCenter = strCenter & ","
            End If
            strCenter = strCenter & "'" & arrTemp(i) & "'"
        Next

        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        '--- ↓2005/04/23 ADD　Falcon↓ -----------------
        strSQL.Append("0 AS DISP_NO, ")
        '--- ↑2005/04/23 ADD　Falcon↑ -----------------
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("TN.TANCD AS CODE, ")
        strSQL.Append("TN.TANNM AS NAME, ")
        '--- ↓2005/04/23 ADD　Falcon↓ -----------------
        strSQL.Append("TO_NUMBER(NVL(TN.DISP_NO,9999)) AS DISP_NO, ")
        '--- ↑2005/04/23 ADD　Falcon↑ -----------------
        strSQL.Append("TN.TANCD || ' : ' || TN.TANNM  AS CDNM ")
        strSQL.Append("FROM M05_TANTO TN ")
        strSQL.Append("WHERE TN.KBN = '1' ")
        strSQL.Append("  AND TN.KURACD = 'ZZZZ' ")
        '監視センターコードが未指定の場合は空となる
        If strCenter.Length = 0 Then
            strSQL.Append("  AND TN.CODE = '' ")
        Else
            strSQL.Append("  AND TN.CODE IN (" & strCenter & ") ")
        End If
        '--- ↓2005/04/23 DEL　Falcon↓ -----------------
        'strSQL.Append("ORDER BY CODE ")
        '--- ↑2005/04/23 DEL　Falcon↑ -----------------
        '--- ↓2005/04/23 MOD　Falcon↓ -----------------
        strSQL.Append("ORDER BY DISP_NO,CODE ")
        '--- ↑2005/04/23 MOD　Falcon↑ -----------------
    End Sub

    '******************************************************************************
    ' 出動会社担当者一覧 2014/10/23 H.Hosoda add 2014改善開発 No11
    '******************************************************************************
    Private Sub mMakeSQL_TSTANCD(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)

        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("0 AS DISP_NO, ")
        strSQL.Append("' ' AS CDNM ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("TN.TANCD AS CODE, ")
        strSQL.Append("TN.TANNM AS NAME, ")
        strSQL.Append("TO_NUMBER(NVL(TN.DISP_NO,9999)) AS DISP_NO, ")
        strSQL.Append("TN.TANCD || ' : ' || TN.TANNM  AS CDNM ")
        strSQL.Append("FROM M05_TANTO TN ")
        strSQL.Append("WHERE TN.KBN = '2' ")
        strSQL.Append("  AND CODE like :CODE || '%' ")
        strSQL.Append("  AND TN.KURACD = 'ZZZZ' ")
        strSQL.Append("ORDER BY DISP_NO,CODE ")

        '出動会社コードの設定
        SqlParamC.fncSetParam("CODE", True, COPOPUPG00_C.pCode1)

    End Sub

    '******************************************************************************
    ' グループコード一覧 2014/10/21 H.Hosoda add 2014改善開発 No10
    '******************************************************************************
    Private Sub mMakeSQL_HANG(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)

        strSQL.Append("SELECT ")
        strSQL.Append("' ' AS CODE, ")
        strSQL.Append("' ' AS NAME, ")
        strSQL.Append("' ' AS CDNM, ")
        strSQL.Append("' ' AS CODE2, ")
        strSQL.Append("' ' AS CDNM2 ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        '使用（紐付けされた）グループ
        strSQL.Append("SELECT  ")
        strSQL.Append("A.GROUPCD AS CODE, ")
        strSQL.Append("A.GROUPNM AS NAME, ")
        strSQL.Append("A.GROUPCD || ' : ' || A.GROUPNM AS CDNM, ")
        strSQL.Append("A.GROUPCD AS CODE2, ")
        strSQL.Append("A.HANJIGYOSYANM AS CDNM2 ")
        strSQL.Append("FROM  ")
        strSQL.Append("M10_HANJIGYOSYA A, ")
        strSQL.Append("M09_JAGROUP B, ")
        strSQL.Append("CLIMAS C ")
        strSQL.Append("WHERE 1=1 ")
        strSQL.Append("AND B.KURACD = C.CLI_CD ")
        strSQL.Append("AND A.GROUPCD = B.GROUPCD ")
        'クライアントコード。
        If COPOPUPG00_C.pCode1.Length > 0 Then
            strSQL.Append("AND C.CLI_CD = :CLI_CD ")
        End If
        strSQL.Append("GROUP BY A.GROUPCD, A.GROUPNM, A.HANJIGYOSYANM ")
        strSQL.Append("ORDER BY 1 ")

        'クライアントコード
        If COPOPUPG00_C.pCode1.Length > 0 Then
            SqlParamC.fncSetParam("CLI_CD", True, COPOPUPG00_C.pCode1)
        End If
    End Sub
    '******************************************************************************
    ' その他
    '******************************************************************************
    Private Sub mMakeSQL_ELSE(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)
        strSQL.Append("SELECT ")
        strSQL.Append("'' AS CODE,")
        strSQL.Append("'表示データ指示が間違っています' AS CDNM, ")
        strSQL.Append("'表示データ指示が間違っています' AS NAME ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
    End Sub
End Class
