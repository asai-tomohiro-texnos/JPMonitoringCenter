Option Explicit On
Option Strict On

Imports Common

Imports System.Text
Imports System.Text.RegularExpressions

'******************************************************************************
' ポップアップ (一覧部)
'******************************************************************************
' 変更履歴


Partial Class MSTAGJBG00
    Inherits System.Web.UI.Page
    Protected WithEvents dbSet As System.Data.DataSet

    Protected MSTAGJCG00_C As MSTAGJCG00

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
        '<独自スクリプト>
        strScript.Append(cscript1.mWriteScript( _
             MyBase.MapPath("../../../MS/MSTAGJAG/MSTAGJAG00/") & "MSTAGJCG00.js"))
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
        MSTAGJCG00_C = CType(Context.Handler, MSTAGJCG00)
        '//------------------------------------------
        '********************************************

        '********************************************
        '//------------------------------------------
        '// Select文の作成
        Dim SQLC As New MSTAGJAG00CCSQL.CSQL
        Dim SqlParamC As New CSQLParam
        Dim strSQL As New StringBuilder
        Dim strDBFlg As String = ""

        '// データの取得
        Call mMakeSQL_TOUROKUZUMI(strSQL, SqlParamC)
        dbSet = SQLC.mGetData(strSQL.ToString, SqlParamC.pParamDataSet, True)

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
    End Sub

    '******************************************************************************
    ' 登録済みJA支所一覧
    '******************************************************************************
    Private Sub mMakeSQL_TOUROKUZUMI(ByVal strSQL As StringBuilder, ByVal SqlParamC As CSQLParam)

        strSQL.Append("SELECT ")
        strSQL.Append(" ' ' AS CODE, ")
        strSQL.Append(" ' ' AS NAME, ")
        strSQL.Append(" ' ' AS CDNM, ")
        strSQL.Append(" ' ' AS CODE2, ")
        strSQL.Append(" ' ' AS CDNM2 ")
        strSQL.Append("FROM ")
        strSQL.Append("DUAL ")
        strSQL.Append("UNION ALL ")
        strSQL.Append("SELECT ")
        strSQL.Append("	B.HAN_CD AS CODE,  ")
        strSQL.Append("	B.JAS_NAME AS NAME,  ")
        strSQL.Append("	B.HAN_CD || ' : ' || B.JAS_NAME AS CDNM,  ")
        strSQL.Append("	' ' AS CODE2,  ")
        strSQL.Append("	' ' AS CDNM2  ")
        strSQL.Append("FROM M09_JAGROUP A ")
        strSQL.Append("	,HN2MAS B ")
        strSQL.Append("WHERE A.KBN = '002' ")
        strSQL.Append("AND GROUPCD = :GROUPCD ")
        strSQL.Append("AND A.KURACD = B.CLI_CD ")
        strSQL.Append("AND A.ACBCD = B.HAN_CD ")
        strSQL.Append("AND B.DEL_FLG IS NULL ")
        strSQL.Append("ORDER BY CODE ")

        SqlParamC.fncSetParam("GROUPCD", True, MSTAGJCG00_C.pCode1)

    End Sub

End Class
