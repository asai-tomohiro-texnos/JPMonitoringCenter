'***********************************************
' ＪＡ担当者連絡先エクセル出力
'***********************************************
' 変更履歴
' 2010/03/30 T.Watabe 新規作成

Option Explicit On
Option Strict On

Imports Common
Imports JPG.Common
Imports JPG.Common.log

Imports System.Text

Partial Class MSTAEJCG00
    Inherits System.Web.UI.Page

    '******************************************************************************
    ' HttpHeader
    '******************************************************************************
    Protected HttpHeaderC As New CHttpHeader

    '******************************************************************************
    ' 親画面クラス
    '******************************************************************************
    Protected MSTAEJAG00C As MSTAEJAG00

    '<TODO>宣言共通仕様
    '******************************************************************************
    ' 認証クラス
    '******************************************************************************
    Protected AuthC As CAuthenticate

    '******************************************************************************
    ' クッキー
    '******************************************************************************
    Protected ConstC As New CConst

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '//------------------------------------------
        '//　HTTPヘッダを送信
        HttpHeaderC.mNoCache(Response)

        '//<TODO>Transferしてきた親のハンドルを引き継ぐ
        MSTAEJAG00C = CType(Context.Handler, MSTAEJAG00)

        Dim strRec As String
        Dim strRecMsg As String
        Dim MSTAEJCG00C As New MSTAEJAG00MSTAEJAW00.MSTAEJAW00

        strRec = MSTAEJCG00C.mCheck( _
                               MSTAEJAG00C.pKuracd, _
                               MSTAEJAG00C.pJacd, _
                               65000 _
                               )

        If strRec = "DATA0" Then
            'データが0件の場合
            strRecMsg = "対象データが存在しません"
            strMsg.Append("alert('" & strRecMsg & "');" & vbCrLf)
            strMsg.Append("parent.Data.Form1.btnSelect.focus();" & vbCrLf)

        ElseIf strRec = "DATAMAX" Then
            strRec = "CHK"
            '存在する為、上書きの確認を行う
            strMsg.Append(vbCrLf)
            strMsg.Append("function fncChkMessage(){" & vbCrLf)
            strMsg.Append("var strRes;" & vbCrLf)
            strMsg.Append("strRes = confirm('最大出力件数を超えました。\n出力しますか？');" & vbCrLf)
            strMsg.Append("if (strRes==false){" & vbCrLf)
            strMsg.Append("  parent.Data.Form1.btnSelect.disabled = false;")     '//制御を戻す
            strMsg.Append("  return;" & vbCrLf)
            strMsg.Append("}" & vbCrLf)
            strMsg.Append("" & vbCrLf)
            strMsg.Append("parent.Data.doPostBack('btnSelect','');" & vbCrLf)
            strMsg.Append("}" & vbCrLf)
            strMsg.Append("window.onload = fncChkMessage;" & vbCrLf)
            strMsg.Append(vbCrLf)
        Else
            strMsg.Append("parent.Data.doPostBack('btnSelect','');" & vbCrLf)
        End If
    End Sub

End Class
