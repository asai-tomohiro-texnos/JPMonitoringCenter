'***********************************************
' 対応内容明細（ＦＡＸ）　帳票出力 (複数メール送信対応版)
' ID:BTFAXJEW00
'***********************************************
' 変更履歴
' 2016/02/03 T.Ono    2015改善開発 M11_JAHOKOKUマスタ参照に変更。BTFAXJDW00からコピーして作成。

Option Explicit On
Option Strict On

Imports Common
Imports Common.DB

Imports System.Web.Services
Imports System.Configuration
Imports System.Text
Imports System.IO
Imports System.Diagnostics
Imports System.IO.Compression

'Imports java.util.zip 'vjslib.dllへの参照設定が必要です 
<System.Web.Services.WebService(Namespace:="http://tempuri.org/JPGAP.BTFAXJEW00/BTFAXJEW00")> _
Public Class BTFAXJEW00
    Inherits System.Web.Services.WebService

#Region " Web サービス デザイナで生成されたコード "

    Public Sub New()
        MyBase.New()

        'この呼び出しは Web サービス デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後に独自の初期化コードを追加してください。

    End Sub

    'Web サービス デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ : 以下のプロシージャは、Web サービス デザイナで必要です。
    'Web サービス デザイナを使って変更することができます。  
    'コード エディタによる変更は行わないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        'CODEGEN: このプロシージャは Web サービス デザイナで必要です。
        'コード エディタによる変更は行わないでください。
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

    'プログラムＩＤ
    Dim strPGID As String

    Const strFAXKBN As String = "1"  'FAX送信
    Const strMAILKBN As String = "2" 'ﾒｰﾙ送信
    Const strBoth As String = "3"    'FAX,ﾒｰﾙ両方

    '2013/09/25 T.Watabe add
    'ログ出力に使用
    Dim strEXEC_YMD As String
    Dim strEXEC_TIME As String
    Dim strGUID As String
    Dim intSEQNO As Integer

    '******************************************************************************
    '*　概　要：監視センターの存在チェック
    '*　備　考：
    '******************************************************************************
    <WebMethod()> Public Function mChkKans( _
                                        ByVal pstrKANSI_CD As String, _
                                        ByRef pstrKANSI_NAME As String, _
                                        ByRef pstrKANSI_KANA As String, _
                                        ByRef pstrTEL As String, _
                                        ByRef pstrKINKYU_TEL As String, _
                                        ByRef pstrPOST_NO As String, _
                                        ByRef pstrADDRESS1 As String, _
                                        ByRef pstrADDRESS2 As String, _
                                        ByRef pstrBIKOU As String) As String
        '--------------------------------------------------
        Dim cdb As New CDB
        Dim strSQL As New StringBuilder("")
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim strRec As String

        strRec = "OK"

        '---------------------------------------------
        'プールの最小値設定
        '---------------------------------------------
        cdb.pConnectPoolSize = 1

        '--------------------------------------------------
        '接続OPEN
        Try
            cdb.mOpen()
        Catch ex As Exception
            Return ex.ToString
        Finally

        End Try

        '--------------------------------------------------
        Try
            'データのSELECT
            strSQL = New StringBuilder("")
            'SQL作成開始
            strSQL.Append("SELECT ")
            strSQL.Append("    KANSI_NAME, ")
            strSQL.Append("    KANSI_KANA, ")
            strSQL.Append("    TEL, ")
            strSQL.Append("    KINKYU_TEL, ")
            strSQL.Append("    POST_NO, ")
            strSQL.Append("    ADDRESS1, ")
            strSQL.Append("    ADDRESS2, ")
            strSQL.Append("    BIKOU ")
            strSQL.Append("FROM  KANSIMAS ")
            strSQL.Append("WHERE KANSI_CD = :KANSI_CD ")
            '//パラメータセット
            cdb.pSQLParamStr("KANSI_CD") = pstrKANSI_CD
            '//SQLセット
            cdb.pSQL = strSQL.ToString
            '//SQL実行
            cdb.mExecQuery()
            '//データセットに格納
            ds = cdb.pResult
            '//データが存在しない場合
            If ds.Tables(0).Rows.Count = 0 Then
                '//データが0件であることを示す文字列を返す
                Return "DATA0"
            End If
            '//データローにデータを格納
            dr = ds.Tables(0).Rows(0)
            '//データの取得
            pstrKANSI_NAME = Convert.ToString(dr.Item("KANSI_NAME"))
            pstrKANSI_KANA = Convert.ToString(dr.Item("KANSI_KANA"))
            pstrTEL = Convert.ToString(dr.Item("TEL"))
            pstrKINKYU_TEL = Convert.ToString(dr.Item("KINKYU_TEL"))
            pstrPOST_NO = Convert.ToString(dr.Item("POST_NO"))
            pstrADDRESS1 = Convert.ToString(dr.Item("ADDRESS1"))
            pstrADDRESS2 = Convert.ToString(dr.Item("ADDRESS2"))
            pstrBIKOU = Convert.ToString(dr.Item("BIKOU"))

        Catch ex As Exception
            strRec = "ERROR:" & ex.ToString
        Finally
            '接続クローズ
            cdb.mClose()
        End Try

        Return strRec
    End Function

    '******************************************************************************
    '*　概　要：クライアントマスタの存在チェック
    '*　備　考：
    '*  履  歴：2015/02/09 T.Watabe add
    '******************************************************************************
    <WebMethod()> Public Function isExistsClientCode( _
                                        ByVal pstrKANSI_CD As String, _
                                        ByRef pstrCLI_CD As String) As String

        '--------------------------------------------------
        Dim cdb As New CDB
        Dim strSQL As New StringBuilder("")
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim strRec As String

        strRec = "OK"

        '---------------------------------------------
        'プールの最小値設定
        '---------------------------------------------
        cdb.pConnectPoolSize = 1

        '--------------------------------------------------
        '接続OPEN
        Try
            cdb.mOpen()
        Catch ex As Exception
            Return ex.ToString
        Finally

        End Try

        '--------------------------------------------------
        Try
            'データのSELECT
            strSQL = New StringBuilder("")
            'SQL作成開始
            strSQL.Append("SELECT * ")
            strSQL.Append("FROM  CLIMAS ")
            strSQL.Append("WHERE KANSI_CODE = :KANSI_CODE ")
            strSQL.Append("AND CLI_CD = :CLI_CD ")
            '//パラメータセット
            cdb.pSQLParamStr("KANSI_CODE") = pstrKANSI_CD
            cdb.pSQLParamStr("CLI_CD") = pstrCLI_CD
            '//SQLセット
            cdb.pSQL = strSQL.ToString
            '//SQL実行
            cdb.mExecQuery()
            '//データセットに格納
            ds = cdb.pResult
            '//データが存在しない場合
            If ds.Tables(0).Rows.Count = 0 Then
                '//データが0件であることを示す文字列を返す
                Return "DATA0"
            Else
                strRec = "OK"
            End If

        Catch ex As Exception
            strRec = "ERROR:" & ex.ToString
        Finally
            '接続クローズ
            cdb.mClose()
        End Try

        Return strRec
    End Function

    ' 2010/09/14 T.Watabe add 旧PG(BTFAXJAE00)からも参照できるようにﾒｿｯﾄﾞを追加
    '                           旧からはmExcel,新からはmExcel2を呼び出し
    '******************************************************************************
    '*　概　要：監視センター対応内容明細（ＦＡＸ）の出力
    '*　備　考：
    '******************************************************************************
    <WebMethod()> Public Function mExcel( _
                                        ByVal pstrKANSI_CODE As String, _
                                        ByVal pstrSESSION As String, _
                                        ByVal pstrTAISYOUBI As String, _
                                        ByVal pstrKURACD_F As String, _
                                        ByVal pstrKURACD_T As String, _
                                        ByVal pstrCreateFilePath As String, _
                                        ByVal pstrSEND_JALP_NAME As String, _
                                        ByVal pstrSEND_CENT_NAME As String _
                                       ) As String

        Return mExcel3(pstrKANSI_CODE, _
                        pstrSESSION, _
                        pstrTAISYOUBI, _
                        pstrKURACD_F, _
                        pstrKURACD_T, _
                        "",
                        "",
                        "",
                        pstrCreateFilePath, _
                        pstrSEND_JALP_NAME, _
                        pstrSEND_CENT_NAME, _
                        "1", _
                        0)
        'pstrSEND_KBN 未指定は1:販売所とする。
    End Function
    '******************************************************************************
    '*　概　要：監視センター対応内容明細（ＦＡＸ）の出力
    '*　備　考：
    '******************************************************************************
    <WebMethod()> Public Function mExcel2( _
                                        ByVal pstrKANSI_CODE As String, _
                                        ByVal pstrSESSION As String, _
                                        ByVal pstrTAISYOUBI As String, _
                                        ByVal pstrKURACD_F As String, _
                                        ByVal pstrKURACD_T As String, _
                                        ByVal pstrJASCD_F As String, _
                                        ByVal pstrJASCD_T As String, _
                                        ByVal pstrFAXMAIL As String, _
                                        ByVal pstrCreateFilePath As String, _
                                        ByVal pstrSEND_JALP_NAME As String, _
                                        ByVal pstrSEND_CENT_NAME As String, _
                                        ByVal pstrSEND_KBN As String _
                                       ) As String

        Return mExcel3(pstrKANSI_CODE, _
                        pstrSESSION, _
                        pstrTAISYOUBI, _
                        pstrKURACD_F, _
                        pstrKURACD_T, _
                        pstrJASCD_F, _
                        pstrJASCD_T, _
                        pstrFAXMAIL, _
                        pstrCreateFilePath, _
                        pstrSEND_JALP_NAME, _
                        pstrSEND_CENT_NAME, _
                        pstrSEND_KBN, _
                        0)
    End Function
    '******************************************************************************
    '*　概　要：監視センター対応内容明細（ＦＡＸ）の出力
    '*　備　考：
    '******************************************************************************
    ' 引数
    ' pstrSEND_KBN 送信区分 1:販売所(JA・支所)/2:ｸﾗｲｱﾝﾄ
    ' pintDebugSQLNo SQLデバッグ用�� 0:デバッグなし/1〜:デバッグあり
    <WebMethod()> Public Function mExcel3( _
                                        ByVal pstrKANSI_CODE As String, _
                                        ByVal pstrSESSION As String, _
                                        ByVal pstrTAISYOUBI As String, _
                                        ByVal pstrKURACD_F As String, _
                                        ByVal pstrKURACD_T As String, _
                                        ByVal pstrJASCD_F As String, _
                                        ByVal pstrJASCD_T As String, _
                                        ByVal pstrFAXMAIL As String, _
                                        ByVal pstrCreateFilePath As String, _
                                        ByVal pstrSEND_JALP_NAME As String, _
                                        ByVal pstrSEND_CENT_NAME As String, _
                                        ByVal pstrSEND_KBN As String, _
                                        ByVal pintDebugSQLNo As Integer _
                                       ) As String

        '--------------------------------------------------
        '自動ＦＡＸ番号に入力された番号毎にＦＡＸ送信・ファイル作成を行う
        Dim cdb As New CDB
        Dim strSQL As New StringBuilder("")
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim strRec As String
        Dim GUID As String = System.Guid.NewGuid().ToString() ' 2011/06/14 T.Watabe add
        Dim strFAXNO As String = "" ' 2015/01/22 T.Watabe add
        Dim strMAIL As String = "" ' 2015/01/22 T.Watabe add

        strEXEC_YMD = Format(DateTime.Now, "yyyyMMdd")
        strEXEC_TIME = Format(DateTime.Now, "HHmmss")
        strGUID = GUID
        intSEQNO = 0

        ' 2015/01/22 T.Watabe add
        ' FAX番号orﾒｰﾙｱﾄﾞﾚｽの切り分け
        If pstrFAXMAIL.Length <= 0 Then
            '空文字→未設定
        ElseIf pstrFAXMAIL.IndexOf("@") >= 0 Then
            'ﾒｰﾙｱﾄﾞﾚｽ
            strMAIL = pstrFAXMAIL
        Else
            'FAX番号
            strFAXNO = pstrFAXMAIL
        End If

        '--------------------------------------------------
        'プログラムＩＤ(作成帳票に使用)
        strPGID = "BTFAXJAX00"
        mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, strPGID & ":mExcel3：1") '2014/04/11 T.Ono add ログ強化
        '---------------------------------------------
        'プールの最小値設定
        '---------------------------------------------
        cdb.pConnectPoolSize = 1

        '--------------------------------------------------
        '接続OPEN
        Try
            cdb.mOpen()
        Catch ex As Exception
            Return ex.ToString
        Finally

        End Try

        '--------------------------------------------------
        '対象日の検索条件の設定
        '対応済みのデータで
        '対応完了日がシステム日付の前の日の締め日からシステム日付の締め日まで

        Dim strTaisyoStDT As String '開始日 2008/10/14 T.Watabe edit
        Dim strTaisyoEdDT As String '終了日 2008/10/14 T.Watabe edit
        strTaisyoStDT = fncAdd_Date(pstrTAISYOUBI, -1) & ConfigurationSettings.AppSettings("JIDOFAXSIME")
        strTaisyoEdDT = pstrTAISYOUBI & ConfigurationSettings.AppSettings("JIDOFAXSIME")

        Dim strWHERE_TAIOU As New StringBuilder("")
        Dim strWHERE_CLI As New StringBuilder("") '2010/12/21 T.Watabe add
        Dim strWHERE_JAS1 As New StringBuilder("") '2015/01/22 T.Watabe add
        Dim strWHERE_JAS2 As New StringBuilder("") '2015/01/22 T.Watabe add
        Dim strWHERE_FAXMAIL As New StringBuilder("") '2015/01/22 T.Watabe add
        If pstrSEND_KBN = "1" Then '送信区分 1:販売所(JA・支所)/2:ｸﾗｲｱﾝﾄ
            strWHERE_TAIOU.Append("  AND TAI.FAXKBN = '2' " & vbCrLf)            '//ＦＡＸ必要(JA)
        Else
            strWHERE_TAIOU.Append("  AND TAI.FAXKURAKBN = '2' " & vbCrLf)        '//ＦＡＸ必要(ｸﾗｲｱﾝﾄ供給ｾﾝﾀ)
        End If
        strWHERE_TAIOU.Append("  AND TAI.TMSKB = '2' " & vbCrLf)             '//処理済み
        '2017/02/16 W.GANEKO UPD 2016監視改善 ��12
        'strWHERE_TAIOU.Append("  AND ( " & vbCrLf)
        'strWHERE_TAIOU.Append("          (TAI.SYOYMD || TAI.SYOTIME > '" & strTaisyoStDT & "' " & vbCrLf)
        'strWHERE_TAIOU.Append("          AND TAI.SYOYMD || TAI.SYOTIME   <='" & strTaisyoEdDT & "') " & vbCrLf)
        'strWHERE_TAIOU.Append("      OR  " & vbCrLf)
        'strWHERE_TAIOU.Append("          (TAI.SYOKANYMD || TAI.SYOKANTIME > '" & strTaisyoStDT & "' " & vbCrLf)
        'strWHERE_TAIOU.Append("          AND TAI.SYOKANYMD || TAI.SYOKANTIME <='" & strTaisyoEdDT & "') " & vbCrLf)
        'strWHERE_TAIOU.Append("      ) " & vbCrLf)
        strWHERE_TAIOU.Append("  AND ( " & vbCrLf)
        strWHERE_TAIOU.Append("          (TAI.SYOYMD || TAI.SYOTIME >= '" & strTaisyoStDT & "' " & vbCrLf)
        strWHERE_TAIOU.Append("          AND TAI.SYOYMD || TAI.SYOTIME   < '" & strTaisyoEdDT & "') " & vbCrLf)
        strWHERE_TAIOU.Append("      OR  " & vbCrLf)
        strWHERE_TAIOU.Append("          (TAI.SYOKANYMD || TAI.SYOKANTIME >= '" & strTaisyoStDT & "' " & vbCrLf)
        strWHERE_TAIOU.Append("          AND TAI.SYOKANYMD || TAI.SYOKANTIME < '" & strTaisyoEdDT & "') " & vbCrLf)
        strWHERE_TAIOU.Append("      ) " & vbCrLf)
        '2017/02/16 W.GANEKO UPD 2016監視改善 ��12

        '--- ↓2005/09/10 ADD Falcon↓ ---
        'クライアントコードの範囲指定を追加
        If pstrKURACD_F.Length > 0 Then
            strWHERE_TAIOU.Append("  AND TAI.KURACD >= '" & pstrKURACD_F & "' " & vbCrLf)
            strWHERE_CLI.Append("  AND CLI.CLI_CD >= '" & pstrKURACD_F & "' " & vbCrLf) '2010/12/21 T.Watabe add
            strWHERE_JAS1.Append("  AND KURACD >= '" & pstrKURACD_F & "' " & vbCrLf) '2015/02/05 T.Watabe add
            strWHERE_JAS2.Append("  AND H.CLI_CD >= '" & pstrKURACD_F & "' " & vbCrLf) '2015/02/05 T.Watabe add
        End If
        If pstrKURACD_T.Length > 0 Then
            strWHERE_TAIOU.Append("  AND TAI.KURACD <= '" & pstrKURACD_T & "' " & vbCrLf)
            strWHERE_CLI.Append("  AND CLI.CLI_CD <= '" & pstrKURACD_T & "' " & vbCrLf) '2010/12/21 T.Watabe add
            strWHERE_JAS1.Append("  AND KURACD <= '" & pstrKURACD_T & "' " & vbCrLf) '2015/02/05 T.Watabe add
            strWHERE_JAS2.Append("  AND H.CLI_CD <= '" & pstrKURACD_T & "' " & vbCrLf) '2015/02/05 T.Watabe add
        End If

        '2015/01/22 T.Watabe add
        'JA支所FROM-TOの条件追加
        If pstrJASCD_F.Length > 0 Or pstrJASCD_T.Length > 0 Then
            strWHERE_TAIOU.Append("  AND TAI.ACBCD >= '" & pstrJASCD_F & "' " & vbCrLf)
            strWHERE_TAIOU.Append("  AND TAI.ACBCD <= '" & pstrJASCD_T & "' " & vbCrLf)
            strWHERE_JAS1.Append("  AND ACBCD >= '" & pstrJASCD_F & "' " & vbCrLf)
            strWHERE_JAS1.Append("  AND ACBCD <= '" & pstrJASCD_T & "' " & vbCrLf)
            strWHERE_JAS2.Append("  AND H.HAN_CD >= '" & pstrJASCD_F & "'  " & vbCrLf) '2015/02/05 T.Watabe add
            strWHERE_JAS2.Append("  AND H.HAN_CD <= '" & pstrJASCD_T & "' " & vbCrLf) '2015/02/05 T.Watabe add
        End If

        '監視センターコードを追加（画面で必須なのでここで追加）
        strWHERE_TAIOU.Append("  AND TAI.KANSCD  = '" & pstrKANSI_CODE & "' " & vbCrLf) '2011/05/20 T.Watabe add
        strWHERE_CLI.Append("  AND CLI.KANSI_CODE  = '" & pstrKANSI_CODE & "' " & vbCrLf) '2011/05/20 T.Watabe add

        Dim strWHERE_TAIOU_COPY As New StringBuilder("") '2011/06/14 T.Watabe add
        strWHERE_TAIOU_COPY.Append(strWHERE_TAIOU)
        strWHERE_TAIOU.Append("  AND TAI.GUID  = '" & GUID & "' " & vbCrLf) '2011/06/14 T.Watabe add

        '2015/01/22 T.Watabe add
        'FAX番号orﾒｰﾙｱﾄﾞﾚｽの条件追加(SQLの最後でフィルターを掛ける)
        If strMAIL.Length > 0 Then strWHERE_FAXMAIL.Append("  AND AUTO_MAIL = '" & strMAIL & "' " & vbCrLf)
        If strFAXNO.Length > 0 Then strWHERE_FAXMAIL.Append("  AND REPLACE(AUTO_FAX,'-','') = REPLACE('" & strFAXNO & "','-','') " & vbCrLf)

        Try
            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, strPGID & ":mExcel3：2") '2014/04/11 T.Ono add ログ強化
            '2011/06/14 T.Watabe add
            '--------------------------------
            ' D20の事前コピー
            '--------------------------------
            strRec = mCopyD20Taiou(cdb, GUID, strWHERE_TAIOU_COPY.ToString)
            If strRec <> "OK" Then
                cdb.mClose() '接続クローズ
                Return strRec
            End If
        Catch ex As Exception
            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, _
     "[" & ex.ToString & "]" & Environment.StackTrace)

            strRec = "ERROR:" & ex.ToString
            cdb.mClose() '接続クローズ
            Return strRec
        Finally
        End Try

        '--------------------------------------------------
        Try
            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, strPGID & ":mExcel3：3 送信区分：" & pstrSEND_KBN) '2014/04/11 T.Ono add ログ強化
            'データのSELECT
            strSQL = New StringBuilder("")
            'SQL作成開始
            If True Then
                If pstrSEND_KBN = "1" Then '送信区分 1:販売所(JA・支所)/2:ｸﾗｲｱﾝﾄ

                    '1:販売所(JA・支所)
                    'JA報告先マスタの絞り込み
                    strSQL.Append("WITH HOKOKU AS ( " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      A.KURACD " & vbCrLf)
                    strSQL.Append("      ,A.ACBCD " & vbCrLf)
                    strSQL.Append("      ,A.USERCD_FROM " & vbCrLf)
                    strSQL.Append("      ,A.USERCD_TO " & vbCrLf)
                    strSQL.Append("      ,B.GROUPCD " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_FAXNM " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_MAIL " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_MAIL_PASS " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_FAXNO " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_KBN " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_ZERO_FLG " & vbCrLf)
                    strSQL.Append("      ,B.SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      M09_JAGROUP A " & vbCrLf)
                    strSQL.Append("      ,M11_JAHOKOKU B " & vbCrLf)
                    strSQL.Append("      ,CLIMAS CLI " & vbCrLf)
                    strSQL.Append("  WHERE " & vbCrLf)
                    strSQL.Append("      A.KURACD = CLI.CLI_CD " & vbCrLf)
                    strSQL.Append("  AND A.KBN = '002' " & vbCrLf)
                    strSQL.Append("  AND B.GROUPCD = A.GROUPCD " & vbCrLf)
                    strSQL.Append("  AND B.KBN = '2' " & vbCrLf)
                    strSQL.Append("  AND B.AUTO_KBN IN ('" & strFAXKBN & "', '" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append(strWHERE_CLI.ToString & vbCrLf)
                    strSQL.Append("), " & vbCrLf)
                    '対応テーブルを送信対象に絞る
                    strSQL.Append("T AS (" & vbCrLf)
                    strSQL.Append("  SELECT" & vbCrLf)
                    strSQL.Append("    DISTINCT" & vbCrLf)
                    strSQL.Append("    TAI.KURACD," & vbCrLf)
                    strSQL.Append("    TAI.JACD," & vbCrLf)
                    strSQL.Append("    TAI.ACBCD," & vbCrLf)
                    strSQL.Append("    TAI.USER_CD" & vbCrLf)
                    strSQL.Append("  FROM D20_TAIOU_COPY TAI " & vbCrLf)
                    strSQL.Append("  WHERE 1=1 " & vbCrLf)
                    strSQL.Append(strWHERE_TAIOU.ToString)
                    strSQL.Append(") " & vbCrLf)
                    strSQL.Append("/* メイン部 */ " & vbCrLf)
                    strSQL.Append("SELECT  " & vbCrLf)
                    strSQL.Append("    AUTO_KUBUN," & vbCrLf)
                    strSQL.Append("    AUTO_FAX_SORT," & vbCrLf)
                    strSQL.Append("    AUTO_FAX," & vbCrLf)
                    strSQL.Append("    AUTO_MAIL," & vbCrLf)
                    strSQL.Append("    CNT " & vbCrLf)
                    strSQL.Append("    ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("FROM ( " & vbCrLf)
                    strSQL.Append("/* fax部 */ " & vbCrLf)
                    '' 0件報告ありデータ---------------------------
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '1' AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      DECODE(LENGTH(AUTO_FAXNO),1,2,1) AS AUTO_FAX_SORT, " & vbCrLf)
                    strSQL.Append(ReplaceHyphen("AUTO_FAXNO") & " AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("      0 AS CNT " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      HOKOKU " & vbCrLf)
                    strSQL.Append("  WHERE  " & vbCrLf)
                    strSQL.Append("      AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND AUTO_ZERO_FLG ='1' " & vbCrLf)
                    strSQL.Append(strWHERE_JAS1.ToString & vbCrLf)

                    '' 対応データあり---------------------------
                    '顧客個別
                    strSQL.Append("  UNION " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '1' AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      DECODE(LENGTH(HOKOKU.AUTO_FAXNO),1,2,1) AS AUTO_FAX_SORT, " & vbCrLf)
                    strSQL.Append(ReplaceHyphen("HOKOKU.AUTO_FAXNO") & " AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("      0 AS CNT " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      HOKOKU, " & vbCrLf)
                    strSQL.Append("      T " & vbCrLf)
                    strSQL.Append("  WHERE " & vbCrLf)
                    strSQL.Append("      HOKOKU.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.USERCD_FROM = T.USER_CD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.USERCD_TO IS NULL " & vbCrLf)
                    '顧客範囲
                    strSQL.Append("  UNION " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '1' AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      DECODE(LENGTH(HOKOKU.AUTO_FAXNO),1,2,1) AS AUTO_FAX_SORT, " & vbCrLf)
                    strSQL.Append(ReplaceHyphen("HOKOKU.AUTO_FAXNO") & " AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("      0 AS CNT " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      HOKOKU , " & vbCrLf)
                    strSQL.Append("      T " & vbCrLf)
                    strSQL.Append("  WHERE " & vbCrLf)
                    strSQL.Append("      HOKOKU.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND T.USER_CD BETWEEN HOKOKU.USERCD_FROM AND HOKOKU.USERCD_TO " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.USERCD_TO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND NOT EXISTS( " & vbCrLf)
                    strSQL.Append("           SELECT 'X' FROM HOKOKU C " & vbCrLf)
                    strSQL.Append("           WHERE " & vbCrLf)
                    strSQL.Append("               C.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("           AND C.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_FROM = T.USER_CD " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_TO IS NULL " & vbCrLf)
                    strSQL.Append("      ) " & vbCrLf)
                    'JA支所単位
                    strSQL.Append("  UNION " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '1' AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      DECODE(LENGTH(HOKOKU.AUTO_FAXNO),1,2,1) AS AUTO_FAX_SORT, " & vbCrLf)
                    strSQL.Append(ReplaceHyphen("HOKOKU.AUTO_FAXNO") & " AS AUTO_FAX," & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("      0 AS CNT " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      HOKOKU , " & vbCrLf)
                    strSQL.Append("      T " & vbCrLf)
                    strSQL.Append("  WHERE " & vbCrLf)
                    strSQL.Append("      HOKOKU.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.USERCD_FROM IS NULL " & vbCrLf)
                    strSQL.Append("  AND NOT EXISTS( " & vbCrLf)
                    strSQL.Append("           SELECT 'X' FROM HOKOKU C " & vbCrLf)
                    strSQL.Append("           WHERE " & vbCrLf)
                    strSQL.Append("               C.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("           AND C.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_FROM = T.USER_CD " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_TO IS NULL " & vbCrLf)
                    strSQL.Append("      ) " & vbCrLf)
                    strSQL.Append("  AND NOT EXISTS( " & vbCrLf)
                    strSQL.Append("           SELECT 'X' FROM HOKOKU C " & vbCrLf)
                    strSQL.Append("           WHERE " & vbCrLf)
                    strSQL.Append("               C.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("           AND C.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("           AND T.USER_CD BETWEEN C.USERCD_FROM AND C.USERCD_TO " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_TO IS NOT NULL  " & vbCrLf)
                    strSQL.Append("      ) " & vbCrLf)
                    strSQL.Append("/* MAIL部 */ " & vbCrLf)
                    '' 0件報告ありデータ---------------------------
                    strSQL.Append("  UNION   " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '2' AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX_SORT, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      AUTO_MAIL AS AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("      0 AS CNT " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      HOKOKU " & vbCrLf)
                    strSQL.Append("  WHERE " & vbCrLf)
                    strSQL.Append("      AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND AUTO_MAIL IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND AUTO_ZERO_FLG ='1' " & vbCrLf)
                    strSQL.Append(strWHERE_JAS1.ToString & vbCrLf)
                    '' 対応データあり---------------------------
                    '顧客個別
                    strSQL.Append("  UNION " & vbCrLf)
                    strSQL.Append("  SELECT   " & vbCrLf)
                    strSQL.Append("      '2' AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX_SORT, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      HOKOKU.AUTO_MAIL AS AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("      0 AS CNT  " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM   " & vbCrLf)
                    strSQL.Append("      HOKOKU,  " & vbCrLf)
                    strSQL.Append("      T   " & vbCrLf)
                    strSQL.Append("  WHERE  " & vbCrLf)
                    strSQL.Append("      HOKOKU.KURACD = T.KURACD  " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.ACBCD = T.ACBCD  " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_MAIL IS NOT NULL  " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.USERCD_FROM = T.USER_CD  " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.USERCD_TO IS NULL  " & vbCrLf)
                    '顧客範囲
                    strSQL.Append("  UNION " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '2' AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX_SORT, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      HOKOKU.AUTO_MAIL AS AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("      0 AS CNT " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      HOKOKU , " & vbCrLf)
                    strSQL.Append("      T " & vbCrLf)
                    strSQL.Append("  WHERE " & vbCrLf)
                    strSQL.Append("      HOKOKU.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND T.USER_CD BETWEEN HOKOKU.USERCD_FROM AND HOKOKU.USERCD_TO " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.USERCD_TO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND NOT EXISTS( " & vbCrLf)
                    strSQL.Append("           SELECT 'X' FROM HOKOKU C " & vbCrLf)
                    strSQL.Append("           WHERE " & vbCrLf)
                    strSQL.Append("               C.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("           AND C.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_MAIL IS NOT NULL " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_FROM = T.USER_CD " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_TO IS NULL " & vbCrLf)
                    strSQL.Append("      ) " & vbCrLf)
                    'JA支所単位
                    strSQL.Append("  UNION " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '2' AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX_SORT, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      HOKOKU.AUTO_MAIL AS AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("      0 AS CNT " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      HOKOKU , " & vbCrLf)
                    strSQL.Append("      T " & vbCrLf)
                    strSQL.Append("  WHERE " & vbCrLf)
                    strSQL.Append("      HOKOKU.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.AUTO_MAIL IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND HOKOKU.USERCD_FROM IS NULL " & vbCrLf)
                    strSQL.Append("  AND NOT EXISTS( " & vbCrLf)
                    strSQL.Append("           SELECT 'X' FROM HOKOKU C " & vbCrLf)
                    strSQL.Append("           WHERE " & vbCrLf)
                    strSQL.Append("               C.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("           AND C.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_MAIL IS NOT NULL " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_FROM = T.USER_CD " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_TO IS NULL " & vbCrLf)
                    strSQL.Append("      ) " & vbCrLf)
                    strSQL.Append("  AND NOT EXISTS( " & vbCrLf)
                    strSQL.Append("           SELECT 'X' FROM HOKOKU C " & vbCrLf)
                    strSQL.Append("           WHERE " & vbCrLf)
                    strSQL.Append("               C.KURACD = T.KURACD " & vbCrLf)
                    strSQL.Append("           AND C.ACBCD = T.ACBCD " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append("           AND C.AUTO_MAIL IS NOT NULL " & vbCrLf)
                    strSQL.Append("           AND T.USER_CD BETWEEN C.USERCD_FROM AND C.USERCD_TO " & vbCrLf)
                    strSQL.Append("           AND C.USERCD_TO IS NOT NULL  " & vbCrLf)
                    strSQL.Append("      ) " & vbCrLf)
                    strSQL.Append(") " & vbCrLf)
                    strSQL.Append("WHERE 1=1 " & vbCrLf)
                    strSQL.Append(strWHERE_FAXMAIL.ToString & vbCrLf)

                Else

                    '2:ｸﾗｲｱﾝﾄ(供給ｾﾝﾀ)
                    strSQL.Append("WITH T AS ( /* T:対応データテーブル内の条件に合致するKURACD */ " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("       DISTINCT " & vbCrLf)
                    strSQL.Append("       TAI.KURACD " & vbCrLf)
                    strSQL.Append("  FROM D20_TAIOU_COPY TAI " & vbCrLf)
                    strSQL.Append("  WHERE 1=1 " & vbCrLf)
                    strSQL.Append(strWHERE_TAIOU.ToString & vbCrLf)
                    strSQL.Append(") " & vbCrLf)
                    strSQL.Append("SELECT " & vbCrLf)
                    strSQL.Append("    AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("    AUTO_FAX, " & vbCrLf)
                    strSQL.Append("    AUTO_MAIL, " & vbCrLf)
                    strSQL.Append("    0 AS CNT " & vbCrLf)
                    strSQL.Append("    ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("FROM " & vbCrLf)
                    strSQL.Append("( " & vbCrLf)
                    strSQL.Append("  /* �@FAX通常 */ " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '1'    AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append(ReplaceHyphen("A.AUTO_FAXNO") & " AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      NULL   AS AUTO_MAIL " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      CLIMAS CLI, " & vbCrLf)
                    strSQL.Append("      M11_JAHOKOKU A, " & vbCrLf)
                    strSQL.Append("      T " & vbCrLf)
                    strSQL.Append("  WHERE 1=1 " & vbCrLf)
                    strSQL.Append("  AND A.KBN = '1' " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_KBN IN ('" & strFAXKBN & "','" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND A.GROUPCD = CLI.CLI_CD " & vbCrLf)
                    strSQL.Append("  AND T.KURACD = CLI.CLI_CD " & vbCrLf)
                    strSQL.Append(strWHERE_CLI.ToString & vbCrLf)
                    strSQL.Append("  UNION ALL " & vbCrLf)
                    strSQL.Append("  /* �AFAX０件表示 */ " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '1'    AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append(ReplaceHyphen("A.AUTO_FAXNO") & " AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      NULL   AS AUTO_MAIL " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      CLIMAS CLI, " & vbCrLf)
                    strSQL.Append("      M11_JAHOKOKU A " & vbCrLf)
                    strSQL.Append("  WHERE 1=1 " & vbCrLf)
                    strSQL.Append("  AND A.KBN = '1' " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_KBN IN ('" & strFAXKBN & "','" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_ZERO_FLG = '1' " & vbCrLf)
                    strSQL.Append("  AND A.GROUPCD = CLI.CLI_CD " & vbCrLf)
                    strSQL.Append(strWHERE_CLI.ToString & vbCrLf)
                    strSQL.Append("  UNION ALL " & vbCrLf)
                    strSQL.Append("  /* �Bﾒｰﾙ通常 */  " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '2'    AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      A.AUTO_MAIL   AS AUTO_MAIL " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      CLIMAS CLI, " & vbCrLf)
                    strSQL.Append("      M11_JAHOKOKU A, " & vbCrLf)
                    strSQL.Append("      T " & vbCrLf)
                    strSQL.Append("  WHERE 1=1 " & vbCrLf)
                    strSQL.Append("  AND A.KBN = '1' " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_KBN IN ('" & strMAILKBN & "','" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_MAIL IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND A.GROUPCD = CLI.CLI_CD " & vbCrLf)
                    strSQL.Append("  AND T.KURACD = CLI.CLI_CD " & vbCrLf)
                    strSQL.Append(strWHERE_CLI.ToString & vbCrLf)

                    strSQL.Append("  UNION ALL " & vbCrLf)
                    strSQL.Append("  /* �Cﾒｰﾙ０件表示 */ " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      '2'    AS AUTO_KUBUN, " & vbCrLf)
                    strSQL.Append("      NULL AS AUTO_FAX, " & vbCrLf)
                    strSQL.Append("      A.AUTO_MAIL   AS AUTO_MAIL " & vbCrLf)
                    strSQL.Append("      ,SD_PRT_FLG " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      CLIMAS CLI, " & vbCrLf)
                    strSQL.Append("      M11_JAHOKOKU A " & vbCrLf)
                    strSQL.Append("  WHERE 1=1 " & vbCrLf)
                    strSQL.Append("  AND A.KBN = '1' " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_KBN IN ('" & strMAILKBN & "','" & strBoth & "') " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_MAIL IS NOT NULL " & vbCrLf)
                    strSQL.Append("  AND A.AUTO_ZERO_FLG = '1' " & vbCrLf)
                    strSQL.Append("  AND A.GROUPCD = CLI.CLI_CD " & vbCrLf)
                    strSQL.Append(strWHERE_CLI.ToString & vbCrLf)

                    strSQL.Append(")  " & vbCrLf)
                    strSQL.Append("WHERE 1=1 " & vbCrLf) ' 2015/01/22 T.Watabe add
                    strSQL.Append(strWHERE_FAXMAIL.ToString & vbCrLf) ' 2015/01/22 T.Watabe add
                    strSQL.Append("GROUP BY  " & vbCrLf)
                    strSQL.Append("    AUTO_KUBUN," & vbCrLf)
                    strSQL.Append("    AUTO_FAX, " & vbCrLf)
                    strSQL.Append("    AUTO_MAIL " & vbCrLf)
                    strSQL.Append("    ,SD_PRT_FLG " & vbCrLf)   '2020/11/01 T.Ono add 2020監視改善
                End If
            End If

            'DEBUG
            If pintDebugSQLNo = 2 Then '2:送信先収集SQL
                Return "DEBUG[" & strSQL.ToString & "]"
            End If

            '//パラメータのセット
            'cdb.pSQLParamStr("KANSI_CODE") = pstrKANSI_CODE

            cdb.pSQL = strSQL.ToString          '//SQLセット
            cdb.mExecQuery()                    '//SQL実行
            ds = cdb.pResult                    '//データセットに格納
            If ds.Tables(0).Rows.Count = 0 Then '//データが存在しない？
                Return "DATA0"                  '//データが0件であることを示す文字列を返す
            End If
            dr = ds.Tables(0).Rows(0)           '//データローにデータを格納

            'DEBUG
            If pintDebugSQLNo = 101 Then
                Return "DEBUG[" & strSQL.ToString & "]"
            End If
            If pintDebugSQLNo = 102 Then
                Dim buf As String = ""
                Dim j As Integer = 0
                For Each dr In ds.Tables(0).Rows
                    If j >= 100 Then
                        buf = buf & "...[100件以上は省略]" & vbCrLf
                        Exit For
                    End If
                    If (Convert.ToString(dr.Item("AUTO_FAX")).Length > 0) Then
                        j = j + 1
                        buf = buf & j & ".FAX :" & Convert.ToString(dr.Item("AUTO_FAX")) & vbCrLf
                    End If
                    If Convert.ToString(dr.Item("AUTO_MAIL")).Length > 0 Then
                        j = j + 1
                        buf = buf & j & ".MAIL:" & Convert.ToString(dr.Item("AUTO_MAIL")) & vbCrLf
                    End If
                Next
                Return "DEBUG[" & buf & "]"
            End If

            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, strPGID & ":mExcel3：4 Count：" & ds.Tables(0).Rows.Count) '2014/04/11 T.Ono add ログ強化
            '//データを出力
            Dim strFlg As String = ""
            Dim intLoop As Integer = 1
            Dim intDataRows As Integer = ds.Tables(0).Rows.Count
            Dim intData As Integer = 0
            Dim arrAUTO() As String
            Dim arrAUTO_FAX() As String
            Dim arrAUTO_MAIL() As String
            Dim arrAUTO_CNT() As Integer
            Dim arrSD_PRT_FLG() As String    '2020/11/01 T.Ono add 2020監視改善
            Dim autoKbn As String = ""

            ReDim Preserve arrAUTO(0)
            ReDim Preserve arrAUTO_FAX(0)
            ReDim Preserve arrAUTO_MAIL(0)
            ReDim Preserve arrAUTO_CNT(0)
            ReDim Preserve arrSD_PRT_FLG(0)    '2020/11/01 T.Ono add 2020監視改善

            For Each dr In ds.Tables(0).Rows
                '--- ↓2005/05/19 MOD Falcon↓ ---      AUTO⇒AUTO_KUBUN
                autoKbn = Convert.ToString(dr.Item("AUTO_KUBUN"))
                If ((autoKbn = strFAXKBN Or autoKbn = strBoth) And Convert.ToString(dr.Item("AUTO_FAX")).Length > 0) Or _
                   ((autoKbn = strMAILKBN Or autoKbn = strBoth) And Convert.ToString(dr.Item("AUTO_MAIL")).Length > 0) Then

                    ' 2011/03/10 T.Watabe edit FAXとﾒｰﾙを両方送信可能に変更
                    If autoKbn = strFAXKBN Or autoKbn = strBoth Then '自動区分が1:FAX送信/3:FAX,ﾒｰﾙ両方
                        ReDim Preserve arrAUTO(intData)
                        ReDim Preserve arrAUTO_FAX(intData)
                        ReDim Preserve arrAUTO_MAIL(intData)
                        ReDim Preserve arrAUTO_CNT(intData)
                        ReDim Preserve arrSD_PRT_FLG(intData)    '2020/11/01 T.Ono add 2020監視改善
                        arrAUTO(intData) = "1"      '1:FAX送信 固定
                        arrAUTO_FAX(intData) = Convert.ToString(dr.Item("AUTO_FAX"))
                        arrAUTO_MAIL(intData) = ""  'ﾒｰﾙ空欄
                        arrAUTO_CNT(intData) = Convert.ToInt16(dr.Item("CNT"))
                        arrSD_PRT_FLG(intData) = Convert.ToString(dr.Item("SD_PRT_FLG"))    '2020/11/01 T.Ono add 2020監視改善
                        intData += 1
                    End If
                    If autoKbn = strMAILKBN Or autoKbn = strBoth Then '自動区分が2:ﾒｰﾙ送信/3:FAX,ﾒｰﾙ両方
                        ReDim Preserve arrAUTO(intData)
                        ReDim Preserve arrAUTO_FAX(intData)
                        ReDim Preserve arrAUTO_MAIL(intData)
                        ReDim Preserve arrAUTO_CNT(intData)
                        ReDim Preserve arrSD_PRT_FLG(intData)    '2020/11/01 T.Ono add 2020監視改善
                        arrAUTO(intData) = "2"      '2:ﾒｰﾙ送信 固定
                        arrAUTO_FAX(intData) = ""   'FAX空欄
                        arrAUTO_MAIL(intData) = Convert.ToString(dr.Item("AUTO_MAIL"))
                        arrAUTO_CNT(intData) = Convert.ToInt16(dr.Item("CNT"))
                        arrSD_PRT_FLG(intData) = Convert.ToString(dr.Item("SD_PRT_FLG"))    '2020/11/01 T.Ono add 2020監視改善
                        intData += 1
                    End If
                End If
                '--- ↑2005/05/19 MOD Falcon↑ ---    
            Next
            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, strPGID & ":mExcel3：5 データカウント：" & intData) '2014/04/11 T.Ono add ログ強化
            If intData = 0 Then
                '//データが0件であることを示す文字列を返す
                Return "DATA0"
            End If

            'DEBUG
            If pintDebugSQLNo = 103 Then
                Return "DEBUG[" & strSQL.ToString & "]"
            End If

            Dim i As Integer
            For i = 0 To intData - 1
                If intLoop = intData Then
                    strFlg = "1"
                Else
                    strFlg = "0"
                End If
                mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, strPGID & ":mExcel3：6 " &
                     "自動送信区分:" & arrAUTO(i) & " FAX:" & arrAUTO_FAX(i) & " メール:" & arrAUTO_MAIL(i) & " CNT:" & arrAUTO_CNT(i) & " SD_PRT:" & arrSD_PRT_FLG(i)) '2014/04/11 T.Ono add ログ強化
                '2020/11/01 T.Ono add 2020監視改善 arrSD_PRT_FLG(i)追加
                strRec = mExcelOut(cdb,
                            pstrKANSI_CODE,
                            pstrSESSION,
                            pstrTAISYOUBI,
                            pstrKURACD_F,
                            pstrKURACD_T,
                            strWHERE_TAIOU.ToString,
                            strWHERE_CLI.ToString,
                            arrAUTO(i),
                            arrAUTO_FAX(i),
                            arrAUTO_MAIL(i),
                            arrAUTO_CNT(i),
                            arrSD_PRT_FLG(i),
                            pstrCreateFilePath,
                            strFlg,
                            pstrSEND_JALP_NAME,
                            pstrSEND_CENT_NAME,
                            pstrSEND_KBN,
                            i,
                            pintDebugSQLNo
                            )
                '//ＥＸＣＥＬファイルは、[strCOMPRESS]にセットした名前の圧縮ファイルに追加する
                Select Case strRec.Substring(0, 5)
                    'Case "DATA0"
                    '    Exit Try
                    Case "DEBUG"
                        'Exit Try
                        Return strRec
                    Case "ERROR"
                        Exit Try
                End Select
                intLoop += 1
            Next

            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, strPGID & ":mExcel3：7") '2014/04/11 T.Ono add ログ強化
            '2011/06/14 T.Watabe add
            '--------------------------------
            ' D20の処理後削除
            '--------------------------------
            mDeleteD20Taiou(cdb, GUID)

        Catch ex As Exception
            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, _
     "[" & ex.ToString & "]" & Environment.StackTrace)
            strRec = "ERROR:" & ex.ToString
        Finally
            '接続クローズ
            cdb.mClose()
        End Try

        '2016/11/29 T.Ono mod 2016改善開発 ��12
        'If pintDebugSQLNo <> 0 Then 'デバッグモード？
        If pintDebugSQLNo <> 0 And pintDebugSQLNo <> 1 Then '通常？デバッグ1？
            If strRec.Substring(0, 5) <> "DEBUG" And strRec.Substring(0, 5) <> "ERROR" Then
                strRec = "DEBUG:デバッグモードですが、データが作られてしまいました。"
            End If
        End If
        Return strRec
    End Function

    '******************************************************************************
    '*　概　要：D20_TAIOU_COPYを用意（D20_TAIOUから該当の日付＋条件のデータのみコピーする）
    '*　備　考：
    '*  作  成：2011/06/14 T.Watabe
    '******************************************************************************
    Private Function mCopyD20Taiou(ByVal cdb As CDB, ByVal GUID As String, ByVal pstrWHERE_TAIOU As String) As String

        Dim sql As New StringBuilder("")

        Try

            '/* 条件に合致するデータをコピーしておく */
            sql = New StringBuilder("")
            sql.Append("INSERT INTO D20_TAIOU_COPY ")
            ' ▼ 2013/09/09 T.Watabe add
            sql.Append("(")
            sql.Append("    KANSCD,")
            sql.Append("    SYONO,")
            sql.Append("    HATYMD,")
            sql.Append("    HATTIME,")
            sql.Append("    KENSIN,")
            sql.Append("    KEIHOSU,")
            sql.Append("    RYURYO,")
            sql.Append("    METASYU,")
            sql.Append("    UNYO,")
            sql.Append("    JUYMD,")
            sql.Append("    JUTIME,")
            sql.Append("    NUM_DIGIT,")
            sql.Append("    KMCD1,")
            sql.Append("    KMNM1,")
            sql.Append("    KMCD2,")
            sql.Append("    KMNM2,")
            sql.Append("    KMCD3,")
            sql.Append("    KMNM3,")
            sql.Append("    KMCD4,")
            sql.Append("    KMNM4,")
            sql.Append("    KMCD5,")
            sql.Append("    KMNM5,")
            sql.Append("    KMCD6,")
            sql.Append("    KMNM6,")
            sql.Append("    ZSISYO,")
            sql.Append("    KURACD,")
            sql.Append("    KENNM,")
            sql.Append("    JACD,")
            sql.Append("    JANM,")
            sql.Append("    ACBCD,")
            sql.Append("    ACBNM,")
            sql.Append("    USER_CD,")
            sql.Append("    JUSYONM,")
            sql.Append("    JUSYOKN,")
            sql.Append("    JUTEL1,")
            sql.Append("    JUTEL2,")
            sql.Append("    RENTEL,")
            sql.Append("    KTELNO,")
            sql.Append("    ADDR,")
            sql.Append("    USER_KIJI,")
            sql.Append("    NCU_SET,")
            sql.Append("    TIZUNO,")
            sql.Append("    GAS_STOP,")
            sql.Append("    GAS_DELE,")
            sql.Append("    GAS_RESTART,")
            sql.Append("    MET_KATA,")
            sql.Append("    MET_MAKER,")
            sql.Append("    BONB1_KKG,")
            sql.Append("    BONB1_HON,")
            sql.Append("    BONB1_YOBI,")
            sql.Append("    BONB2_KKG,")
            sql.Append("    BONB2_HON,")
            sql.Append("    BONB2_YOBI,")
            sql.Append("    BONB3_KKG,")
            sql.Append("    BONB3_HON,")
            sql.Append("    BONB3_YOBI,")
            sql.Append("    BONB4_KKG,")
            sql.Append("    BONB4_HON,")
            sql.Append("    BONB4_YOBI,")
            sql.Append("    BOMB_TYPE,")
            sql.Append("    ZENKAI_HAISO,")
            sql.Append("    ZENKAI_HAI_S,")
            sql.Append("    KONKAI_HAISO,")
            sql.Append("    KONKAI_HAI_S,")
            sql.Append("    JIKAI_HAISO,")
            sql.Append("    ZENKAI_KENSIN,")
            sql.Append("    ZENKAI_KEN_S,")
            sql.Append("    ZENKAI_KEN_SIYO,")
            sql.Append("    KONKAI_KENSIN,")
            sql.Append("    KONKAI_KEN_S,")
            sql.Append("    KONKAI_KEN_SIYO,")
            sql.Append("    ZENKAI_HASEI,")
            sql.Append("    ZENKAI_HAS_S,")
            sql.Append("    KONKAI_HASEI,")
            sql.Append("    KONKAI_HAS_S,")
            sql.Append("    G_ZAIKO,")
            sql.Append("    ICHI_SIYO,")
            sql.Append("    YOSOKU_ICHI_SIYO,")
            sql.Append("    GAS1_HINMEI,")
            sql.Append("    GAS1_DAISU,")
            sql.Append("    GAS1_SEIFL,")
            sql.Append("    GAS2_HINMEI,")
            sql.Append("    GAS2_DAISU,")
            sql.Append("    GAS2_SEIFL,")
            sql.Append("    GAS3_HINMEI,")
            sql.Append("    GAS3_DAISU,")
            sql.Append("    GAS3_SEIFL,")
            sql.Append("    GAS4_HINMEI,")
            sql.Append("    GAS4_DAISU,")
            sql.Append("    GAS4_SEIFL,")
            sql.Append("    GAS5_HINMEI,")
            sql.Append("    GAS5_DAISU,")
            sql.Append("    GAS5_SEIFL,")
            sql.Append("    HATKBN,")
            sql.Append("    HATKBN_NAI,")
            sql.Append("    TAIOKBN,")
            sql.Append("    TAIOKBN_NAI,")
            sql.Append("    TMSKB,")
            sql.Append("    TMSKB_NAI,")
            sql.Append("    TKTANCD,")
            sql.Append("    TKTANCD_NM,")
            sql.Append("    TAITCD,")
            sql.Append("    TAITNM,")
            sql.Append("    TAIO_ST_DATE,")
            sql.Append("    TAIO_ST_TIME,")
            sql.Append("    SYOYMD,")
            sql.Append("    SYOTIME,")
            sql.Append("    TAIO_SYO_TIME,")
            sql.Append("    FAXKBN,")
            sql.Append("    TELRCD,")
            sql.Append("    TELRNM,")
            sql.Append("    TFKICD,")
            sql.Append("    TFKINM,")
            sql.Append("    FUK_MEMO,")
            sql.Append("    TEL_MEMO1,")
            sql.Append("    TEL_MEMO2,")
            sql.Append("    MITOKBN,")
            sql.Append("    TKIGCD,")
            sql.Append("    TKIGNM,")
            sql.Append("    TSADCD,")
            sql.Append("    TSADNM,")
            sql.Append("    GENIN_KIJI,")
            sql.Append("    SDCD,")
            sql.Append("    SDNM,")
            sql.Append("    SIJIYMD,")
            sql.Append("    SIJITIME,")
            sql.Append("    SIJI_BIKO1,")
            sql.Append("    SIJI_BIKO2,")
            sql.Append("    STD_JASCD,")
            sql.Append("    STD_JANA,")
            sql.Append("    STD_JASNA,")
            sql.Append("    REN_NA,")
            sql.Append("    REN_TEL_1,")
            sql.Append("    REN_TEL_2,")
            sql.Append("    REN_FAX,")
            sql.Append("    REN_BIKO,")
            sql.Append("    REN_1_NA,")
            sql.Append("    REN_1_TEL1,")
            sql.Append("    REN_1_TEL2,")
            sql.Append("    REN_1_FAX,")
            sql.Append("    REN_1_BIKO,")
            sql.Append("    REN_2_NA,")
            sql.Append("    REN_2_TEL1,")
            sql.Append("    REN_2_TEL2,")
            sql.Append("    REN_2_FAX,")
            sql.Append("    REN_2_BIKO,")
            sql.Append("    REN_3_NA,")
            sql.Append("    REN_3_TEL1,")
            sql.Append("    REN_3_TEL2,")
            sql.Append("    REN_3_FAX,")
            sql.Append("    REN_3_BIKO,")
            sql.Append("    STD_CD,")
            sql.Append("    STD,")
            sql.Append("    STD_KYOTEN_CD,")
            sql.Append("    STD_KYOTEN,")
            sql.Append("    STD_TEL,")
            sql.Append("    TEL_BIKO,")
            sql.Append("    FAX_TITLE,")
            sql.Append("    FAX_REN,")
            sql.Append("    TSTANCD,")
            sql.Append("    TSTANNM,")
            sql.Append("    STD_KYOTEN_CD_I,")
            sql.Append("    STD_KYOTEN_I,")
            sql.Append("    SYUTDTNM,")
            sql.Append("    ORNCU,")
            sql.Append("    TYAKYMD,")
            sql.Append("    TYAKTIME,")
            sql.Append("    SYOKANYMD,")
            sql.Append("    SYOKANTIME,")
            sql.Append("    AITCD,")
            sql.Append("    AITNM,")
            sql.Append("    METHEIKAKU,")
            sql.Append("    RUSUHARI,")
            sql.Append("    METFUKKI,")
            sql.Append("    HOAN,")
            sql.Append("    GASGIRE,")
            sql.Append("    KIGKOSYO,")
            sql.Append("    CSNTGEN,")
            sql.Append("    CSNTNGAS,")
            sql.Append("    SDTBIK1,")
            sql.Append("    KIGCD,")
            sql.Append("    KIGNM,")
            sql.Append("    SADCD,")
            sql.Append("    SADNM,")
            sql.Append("    STACD,")
            sql.Append("    STANM,")
            sql.Append("    ASECD,")
            sql.Append("    ASENM,")
            sql.Append("    FKICD,")
            sql.Append("    FKINM,")
            sql.Append("    JAKENREN,")
            sql.Append("    RENTIME,")
            sql.Append("    KIGTAIYO,")
            sql.Append("    GASMUMU,")
            sql.Append("    ORGENIN,")
            sql.Append("    HAIKAN,")
            sql.Append("    GASGUMU,")
            sql.Append("    HOSKOKAN,")
            sql.Append("    METYOINA,")
            sql.Append("    TYOUYOINA,")
            sql.Append("    VALYOINA,")
            sql.Append("    KYUHAIUMU,")
            sql.Append("    COYOINA,")
            sql.Append("    SDTBIK2,")
            sql.Append("    SNTTOKKI,")
            sql.Append("    LTOS_DATE,")
            sql.Append("    ADD_DATE,")
            sql.Append("    EDT_DATE,")
            sql.Append("    EDT_TIME,")
            sql.Append("    BIKOU,")
            sql.Append("    FAX_TITLE_CD,")
            sql.Append("    SDTBIK3,")
            sql.Append("    SDYMD,")
            sql.Append("    SDTIME,")
            sql.Append("    SDSKBN,")
            sql.Append("    SDSKBN_NAI,")
            sql.Append("    NCUHATYMD,")
            sql.Append("    NCUHATTIME,")
            sql.Append("    FAXKURAKBN,")
            sql.Append("    SYORI_SERIAL, ")
            sql.Append("    KAITU_DAY, ") ' 2013/08/26 T.Ono add 監視改善2013��1
            sql.Append("    COPY_DATE, ")
            sql.Append("    GUID, ")
            sql.Append("    SHUGOU, ")    ' 2017/10/30 H.Mori add 2017改善開発 No9-1 
            sql.Append("    TEL_MEMO4, ")    '2020/11/01 T.Ono add 2020監視改善
            sql.Append("    TEL_MEMO5, ")    '2020/11/01 T.Ono add 2020監視改善
            sql.Append("    TEL_MEMO6  ")    '2020/11/01 T.Ono add 2020監視改善
            sql.Append(") ")
            ' ▲ 2013/09/09 T.Watabe add
            sql.Append("SELECT ")
            sql.Append("    KANSCD,")
            sql.Append("    SYONO,")
            sql.Append("    HATYMD,")
            sql.Append("    HATTIME,")
            sql.Append("    KENSIN,")
            sql.Append("    KEIHOSU,")
            sql.Append("    RYURYO,")
            sql.Append("    METASYU,")
            sql.Append("    UNYO,")
            sql.Append("    JUYMD,")
            sql.Append("    JUTIME,")
            sql.Append("    NUM_DIGIT,")
            'sql.Append("    KMCD1,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMNM1,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMCD2,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMNM2,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMCD3,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMNM3,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMCD4,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMNM4,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMCD5,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMNM5,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMCD6,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMNM6,") ' 2015/01/22 T.Watabe edit
            'sql.Append("    KMCD1, DECODE(RTRIM(KMCD1), NULL, NULL, KMNM1) AS KMNM1,")    '2020/11/01 T.Ono mod 2020監視改善
            'sql.Append("    KMCD2, DECODE(RTRIM(KMCD2), NULL, NULL, KMNM2) AS KMNM2,")    '2020/11/01 T.Ono mod 2020監視改善
            'sql.Append("    KMCD3, DECODE(RTRIM(KMCD3), NULL, NULL, KMNM3) AS KMNM3,")    '2020/11/01 T.Ono mod 2020監視改善
            'sql.Append("    KMCD4, DECODE(RTRIM(KMCD4), NULL, NULL, KMNM4) AS KMNM4,")    '2020/11/01 T.Ono mod 2020監視改善
            'sql.Append("    KMCD5, DECODE(RTRIM(KMCD5), NULL, NULL, KMNM5) AS KMNM5,")    '2020/11/01 T.Ono mod 2020監視改善
            'sql.Append("    KMCD6, DECODE(RTRIM(KMCD6), NULL, NULL, KMNM6) AS KMNM6,")    '2020/11/01 T.Ono mod 2020監視改善
            sql.Append("    KMCD1, KMNM1,")
            sql.Append("    KMCD2, KMNM2,")
            sql.Append("    KMCD3, KMNM3,")
            sql.Append("    KMCD4, KMNM4,")
            sql.Append("    KMCD5, KMNM5,")
            sql.Append("    KMCD6, KMNM6,")
            sql.Append("    ZSISYO,")
            sql.Append("    KURACD,")
            sql.Append("    KENNM,")
            sql.Append("    JACD,")
            sql.Append("    JANM,")
            sql.Append("    ACBCD,")
            sql.Append("    ACBNM,")
            sql.Append("    USER_CD,")
            sql.Append("    JUSYONM,")
            sql.Append("    JUSYOKN,")
            sql.Append("    JUTEL1,")
            sql.Append("    JUTEL2,")
            sql.Append("    RENTEL,")
            sql.Append("    KTELNO,")
            sql.Append("    ADDR,")
            sql.Append("    USER_KIJI,")
            sql.Append("    NCU_SET,")
            sql.Append("    TIZUNO,")
            sql.Append("    GAS_STOP,")
            sql.Append("    GAS_DELE,")
            sql.Append("    GAS_RESTART,")
            sql.Append("    MET_KATA,")
            sql.Append("    MET_MAKER,")
            sql.Append("    BONB1_KKG,")
            sql.Append("    BONB1_HON,")
            sql.Append("    BONB1_YOBI,")
            sql.Append("    BONB2_KKG,")
            sql.Append("    BONB2_HON,")
            sql.Append("    BONB2_YOBI,")
            sql.Append("    BONB3_KKG,")
            sql.Append("    BONB3_HON,")
            sql.Append("    BONB3_YOBI,")
            sql.Append("    BONB4_KKG,")
            sql.Append("    BONB4_HON,")
            sql.Append("    BONB4_YOBI,")
            sql.Append("    BOMB_TYPE,")
            sql.Append("    ZENKAI_HAISO,")
            sql.Append("    ZENKAI_HAI_S,")
            sql.Append("    KONKAI_HAISO,")
            sql.Append("    KONKAI_HAI_S,")
            sql.Append("    JIKAI_HAISO,")
            sql.Append("    ZENKAI_KENSIN,")
            sql.Append("    ZENKAI_KEN_S,")
            sql.Append("    ZENKAI_KEN_SIYO,")
            sql.Append("    KONKAI_KENSIN,")
            sql.Append("    KONKAI_KEN_S,")
            sql.Append("    KONKAI_KEN_SIYO,")
            sql.Append("    ZENKAI_HASEI,")
            sql.Append("    ZENKAI_HAS_S,")
            sql.Append("    KONKAI_HASEI,")
            sql.Append("    KONKAI_HAS_S,")
            sql.Append("    G_ZAIKO,")
            sql.Append("    ICHI_SIYO,")
            sql.Append("    YOSOKU_ICHI_SIYO,")
            sql.Append("    GAS1_HINMEI,")
            sql.Append("    GAS1_DAISU,")
            sql.Append("    GAS1_SEIFL,")
            sql.Append("    GAS2_HINMEI,")
            sql.Append("    GAS2_DAISU,")
            sql.Append("    GAS2_SEIFL,")
            sql.Append("    GAS3_HINMEI,")
            sql.Append("    GAS3_DAISU,")
            sql.Append("    GAS3_SEIFL,")
            sql.Append("    GAS4_HINMEI,")
            sql.Append("    GAS4_DAISU,")
            sql.Append("    GAS4_SEIFL,")
            sql.Append("    GAS5_HINMEI,")
            sql.Append("    GAS5_DAISU,")
            sql.Append("    GAS5_SEIFL,")
            sql.Append("    HATKBN,")
            sql.Append("    HATKBN_NAI,")
            sql.Append("    TAIOKBN,")
            sql.Append("    TAIOKBN_NAI,")
            sql.Append("    TMSKB,")
            sql.Append("    TMSKB_NAI,")
            sql.Append("    TKTANCD,")
            sql.Append("    TKTANCD_NM,")
            sql.Append("    TAITCD,")
            sql.Append("    TAITNM,")
            sql.Append("    TAIO_ST_DATE,")
            sql.Append("    TAIO_ST_TIME,")
            sql.Append("    SYOYMD,")
            sql.Append("    SYOTIME,")
            sql.Append("    TAIO_SYO_TIME,")
            sql.Append("    FAXKBN,")
            sql.Append("    TELRCD,")
            sql.Append("    TELRNM,")
            sql.Append("    TFKICD,")
            sql.Append("    TFKINM,")
            sql.Append("    FUK_MEMO,")
            sql.Append("    TEL_MEMO1,")
            sql.Append("    TEL_MEMO2,")
            sql.Append("    MITOKBN,")
            sql.Append("    TKIGCD,")
            sql.Append("    TKIGNM,")
            sql.Append("    TSADCD,")
            sql.Append("    TSADNM,")
            sql.Append("    GENIN_KIJI,")
            sql.Append("    SDCD,")
            sql.Append("    SDNM,")
            sql.Append("    SIJIYMD,")
            sql.Append("    SIJITIME,")
            sql.Append("    SIJI_BIKO1,")
            sql.Append("    SIJI_BIKO2,")
            sql.Append("    STD_JASCD,")
            sql.Append("    STD_JANA,")
            sql.Append("    STD_JASNA,")
            sql.Append("    REN_NA,")
            sql.Append("    REN_TEL_1,")
            sql.Append("    REN_TEL_2,")
            sql.Append("    REN_FAX,")
            sql.Append("    REN_BIKO,")
            sql.Append("    REN_1_NA,")
            sql.Append("    REN_1_TEL1,")
            sql.Append("    REN_1_TEL2,")
            sql.Append("    REN_1_FAX,")
            sql.Append("    REN_1_BIKO,")
            sql.Append("    REN_2_NA,")
            sql.Append("    REN_2_TEL1,")
            sql.Append("    REN_2_TEL2,")
            sql.Append("    REN_2_FAX,")
            sql.Append("    REN_2_BIKO,")
            sql.Append("    REN_3_NA,")
            sql.Append("    REN_3_TEL1,")
            sql.Append("    REN_3_TEL2,")
            sql.Append("    REN_3_FAX,")
            sql.Append("    REN_3_BIKO,")
            sql.Append("    STD_CD,")
            sql.Append("    STD,")
            sql.Append("    STD_KYOTEN_CD,")
            sql.Append("    STD_KYOTEN,")
            sql.Append("    STD_TEL,")
            sql.Append("    TEL_BIKO,")
            sql.Append("    FAX_TITLE,")
            sql.Append("    FAX_REN,")
            sql.Append("    TSTANCD,")
            sql.Append("    TSTANNM,")
            sql.Append("    STD_KYOTEN_CD_I,")
            sql.Append("    STD_KYOTEN_I,")
            sql.Append("    SYUTDTNM,")
            sql.Append("    ORNCU,")
            sql.Append("    TYAKYMD,")
            sql.Append("    TYAKTIME,")
            sql.Append("    SYOKANYMD,")
            sql.Append("    SYOKANTIME,")
            sql.Append("    AITCD,")
            sql.Append("    AITNM,")
            sql.Append("    METHEIKAKU,")
            sql.Append("    RUSUHARI,")
            sql.Append("    METFUKKI,")
            sql.Append("    HOAN,")
            sql.Append("    GASGIRE,")
            sql.Append("    KIGKOSYO,")
            sql.Append("    CSNTGEN,")
            sql.Append("    CSNTNGAS,")
            sql.Append("    SDTBIK1,")
            sql.Append("    KIGCD,")
            sql.Append("    KIGNM,")
            sql.Append("    SADCD,")
            sql.Append("    SADNM,")
            sql.Append("    STACD,")
            sql.Append("    STANM,")
            sql.Append("    ASECD,")
            sql.Append("    ASENM,")
            sql.Append("    FKICD,")
            sql.Append("    FKINM,")
            sql.Append("    JAKENREN,")
            sql.Append("    RENTIME,")
            sql.Append("    KIGTAIYO,")
            sql.Append("    GASMUMU,")
            sql.Append("    ORGENIN,")
            sql.Append("    HAIKAN,")
            sql.Append("    GASGUMU,")
            sql.Append("    HOSKOKAN,")
            sql.Append("    METYOINA,")
            sql.Append("    TYOUYOINA,")
            sql.Append("    VALYOINA,")
            sql.Append("    KYUHAIUMU,")
            sql.Append("    COYOINA,")
            sql.Append("    SDTBIK2,")
            sql.Append("    SNTTOKKI,")
            sql.Append("    LTOS_DATE,")
            sql.Append("    ADD_DATE,")
            sql.Append("    EDT_DATE,")
            sql.Append("    EDT_TIME,")
            sql.Append("    BIKOU,")
            sql.Append("    FAX_TITLE_CD,")
            sql.Append("    SDTBIK3,")
            sql.Append("    SDYMD,")
            sql.Append("    SDTIME,")
            sql.Append("    SDSKBN,")
            sql.Append("    SDSKBN_NAI,")
            sql.Append("    NCUHATYMD,")
            sql.Append("    NCUHATTIME,")
            sql.Append("    FAXKURAKBN,")
            ' 2013/08/13 T.Ono mod ----------Start
            'sql.Append("    SYSDATE, ")
            'sql.Append("    '" & GUID & "', ")
            'sql.Append("    SYORI_SERIAL ")
            sql.Append("    SYORI_SERIAL, ")
            sql.Append("    KAITU_DAY, ") ' 2013/08/26 T.Ono add 監視改善2013��1
            sql.Append("    SYSDATE, ")
            sql.Append("    '" & GUID & "' ")
            ' 2013/08/13 T.Ono mod ----------End
            ' 2017/10/30 H.Mori add 2017改善開発 No9-1 START
            sql.Append("    ,SHUGOU, ")
            ' 2017/10/30 H.Mori add 2017改善開発 No9-1 END
            sql.Append("    TEL_MEMO4, ")    '2020/11/01 T.Ono add 2020監視改善
            sql.Append("    TEL_MEMO5, ")    '2020/11/01 T.Ono add 2020監視改善
            sql.Append("    TEL_MEMO6  ")    '2020/11/01 T.Ono add 2020監視改善
            sql.Append("FROM D20_TAIOU TAI ")
            sql.Append("WHERE 1=1 ")
            sql.Append(pstrWHERE_TAIOU)

            cdb.mBeginTrans() 'トランザクション開始
            cdb.pSQL = sql.ToString '//SQLセット
            cdb.mExecNonQuery() '//SQL実行
            cdb.mCommit() 'トランザクション終了(コミット)
        Catch ex As Exception
            cdb.mRollback() 'トランザクション終了(ロールバック)
            'エラーの内容とＳＱＬ文を返す
            Return "ERROR:" & ex.ToString & vbCrLf & vbCrLf & Environment.StackTrace & vbCrLf & " 直前のSQL[" & sql.ToString & "]"
        Finally
        End Try

        Return "OK"

    End Function

    '******************************************************************************
    '*　概　要：D20_TAIOU_COPYを処理後にGUIDでクリアする。万一残ったデータも１日経過で削除。
    '*　備　考：
    '*  作  成：2011/06/14 T.Watabe
    '******************************************************************************
    Private Function mDeleteD20Taiou(ByVal cdb As CDB, ByVal GUID As String) As String

        Dim sql As New StringBuilder("")

        Try
            cdb.mBeginTrans() 'トランザクション開始

            '/* �@作成から12時間経過したデータは削除 */
            sql = New StringBuilder("")
            sql.Append("DELETE FROM D20_TAIOU_COPY WHERE COPY_DATE IS NULL OR COPY_DATE <= SYSDATE - 0.5 ")
            cdb.pSQL = sql.ToString '//SQLセット
            cdb.mExecNonQuery() '//SQL実行

            '/* �AGUIDでデータ削除 */
            sql = New StringBuilder("")
            sql.Append("DELETE FROM D20_TAIOU_COPY WHERE GUID = '" & GUID & "' ")
            cdb.pSQL = sql.ToString '//SQLセット
            cdb.mExecNonQuery() '//SQL実行

            cdb.mCommit() 'トランザクション終了(コミット)

        Catch ex As Exception
            cdb.mRollback() 'トランザクション終了(ロールバック)
            'エラーの内容とＳＱＬ文を返す
            Return "ERROR:" & ex.ToString & vbCrLf & vbCrLf & Environment.StackTrace & vbCrLf & " 直前のSQL[" & sql.ToString & "]"
        Finally
        End Try

        Return "OK"

    End Function

    '******************************************************************************
    '*　概　要：ナースセンター対応内容明細（ＦＡＸ）の出力
    '*　備　考：
    '******************************************************************************
    '　DATA0：対象データがありません
    '  2020/11/01 T.Ono mod 2020監視改善　arrSD_PRT_FLG(i)追加
    Private Function mExcelOut(
                                ByVal cdb As CDB,
                                ByVal pstrKANSI_CODE As String,
                                ByVal pstrSESSION As String,
                                ByVal pstrTAISYOUBI As String,
                                ByVal pstrKURACD_F As String,
                                ByVal pstrKURACD_T As String,
                                ByVal pstrWHERE_TAIOU As String,
                                ByVal pstrWHERE_CLI As String,
                                ByVal pstrAUTO As String,
                                ByVal pstrFAXNO As String,
                                ByVal pstrMAILAD As String,
                                ByVal pintCNT As Integer,
                                ByVal pstrSD_PRT_FLG As String,
                                ByVal pstrCreateFilePath As String,
                                ByVal pstrFlg As String,
                                ByVal pstrSEND_JALP_NAME As String,
                                ByVal pstrSEND_CENT_NAME As String,
                                ByVal pstrSEND_KBN As String,
                                ByVal pintLoopNo As Integer,
                                ByVal pintDebugSQLNo As Integer
                                ) As String

        Dim strSQL As New StringBuilder("")
        Dim ds As New DataSet
        Dim dr As DataRow


        Dim ExcelC As New CExcel 'Excelクラス
        'Dim compressC As New CCompress '圧縮クラス 2018/02/19 T.Ono del 圧縮方法変更
        Dim intGYOSU As Integer = 56 '改行制御を行う
        Dim DateFncC As New CDateFnc '日付変換クラス
        Dim FileToStrC As New CFileStr 'ファイルをBase64にエンコードするクラス
        Dim sZipFilePass As String = "" ' 2008/12/12 T.Watabe add
        Dim sendCD As String '2010/05/24 T.Watabe add

        Dim cliCd4FileHead As String = "" '2011/03/10 T.Watabe add
        Dim jaName4FileHead As String = ""  ' 2011/05/19 T.Watabe add
        Dim centerName As String = ""

        Dim wkstrTAIOU_SYONO As String = ""
        Dim wkstrTAIOU_KURACD As String = ""
        Dim wkstrTAIOU_JACD As String = ""
        Dim wkstrTAIOU_ACBCD As String = ""
        Dim wkstrTAIOU_USER_CD As String = ""
        Dim wkstrAUTO_ZERO_FLG As String = "0" ' 【ゼロ件送信フラグ】 0:データあり/1:ゼロ件！(データなし)


        If pstrSEND_KBN = "1" Then
            sendCD = "H" '販売所
        Else
            sendCD = "C" 'ｾﾝﾀｰ
        End If

        mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：1") '2014/04/11 T.Ono add ログ強化

        '自動FAX専用ログ更新(フラグクリア)　2013/09/25 T.Watabe add
        Call mUpdateFlgS05AutofaxLog(cdb,
                                pstrSEND_KBN,
                                pstrTAISYOUBI,
                                pstrAUTO,
                                pstrFAXNO,
                                pstrMAILAD
                                )

        Dim wit As StringBuilder = New StringBuilder("")
        Dim sel As StringBuilder = New StringBuilder("")
        Dim fro As StringBuilder = New StringBuilder("")
        Dim whe As StringBuilder = New StringBuilder("")
        Dim wheC As StringBuilder = New StringBuilder("")
        Dim wheJ As StringBuilder = New StringBuilder("")
        Dim ord As StringBuilder = New StringBuilder("")

        mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：2 送信区分" & pstrSEND_KBN) '2014/04/11 T.Ono add ログ強化

        If pstrSEND_KBN = "1" Then '1:販売所？
            'SQL作成開始
            'JA報告先マスタの絞り込み
            wit.Append("WITH HOKOKU AS ( " & vbCrLf)
            wit.Append("  SELECT " & vbCrLf)
            wit.Append("      A.KURACD " & vbCrLf)
            wit.Append("      ,A.ACBCD " & vbCrLf)
            wit.Append("      ,A.USERCD_FROM " & vbCrLf)
            wit.Append("      ,A.USERCD_TO " & vbCrLf)
            wit.Append("      ,B.GROUPCD " & vbCrLf)
            wit.Append("      ,B.AUTO_FAXNM " & vbCrLf)
            wit.Append("      ,B.AUTO_MAIL " & vbCrLf)
            wit.Append("      ,B.AUTO_MAIL_PASS " & vbCrLf)
            wit.Append("      ,B.AUTO_FAXNO " & vbCrLf)
            wit.Append("      ,B.AUTO_KBN " & vbCrLf)
            wit.Append("      ,B.AUTO_ZERO_FLG " & vbCrLf)
            wit.Append("  FROM " & vbCrLf)
            wit.Append("      M09_JAGROUP A " & vbCrLf)
            wit.Append("      ,M11_JAHOKOKU B " & vbCrLf)
            wit.Append("      ,CLIMAS CLI " & vbCrLf)
            wit.Append("  WHERE " & vbCrLf)
            wit.Append("      A.KURACD = CLI.CLI_CD " & vbCrLf)
            wit.Append("  AND A.KBN = '002' " & vbCrLf)
            wit.Append("  AND B.GROUPCD = A.GROUPCD " & vbCrLf)
            wit.Append("  AND B.KBN = '2' " & vbCrLf)
            wit.Append("  AND B.AUTO_KBN IN ('" & strFAXKBN & "', '" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
            wit.Append(pstrWHERE_CLI.ToString)
            wit.Append(") " & vbCrLf)

            sel.Append("    ,DECODE(HOK3.AUTO_MAIL_PASS,NULL,DECODE(HOK2.AUTO_MAIL_PASS,NULL,HOK1.AUTO_MAIL_PASS,HOK2.AUTO_MAIL_PASS),HOK3.AUTO_MAIL_PASS) AS YOBI5 " & vbCrLf) ' パスワード

            fro.Append("    ,HOKOKU HOK1 " & vbCrLf)
            fro.Append("    ,HOKOKU HOK2 " & vbCrLf)
            fro.Append("    ,HOKOKU HOK3 " & vbCrLf)
            fro.Append("    ,HN2MAS HN " & vbCrLf)

            If pstrAUTO = strFAXKBN Then 'ＦＡＸ送信？
                whe.Append("  AND HOK1.AUTO_KBN(+) IN ('" & strFAXKBN & "','" & strBoth & "') " & vbCrLf)
                whe.Append("  AND HOK2.AUTO_KBN(+) IN ('" & strFAXKBN & "','" & strBoth & "') " & vbCrLf)
                whe.Append("  AND HOK3.AUTO_KBN(+) IN ('" & strFAXKBN & "','" & strBoth & "') " & vbCrLf)
                whe.Append("  AND " & ReplaceHyphen("HOK1.AUTO_FAXNO(+)") & " = :AUTO_FAX " & vbCrLf)
                whe.Append("  AND " & ReplaceHyphen("HOK2.AUTO_FAXNO(+)") & " = :AUTO_FAX " & vbCrLf)
                whe.Append("  AND " & ReplaceHyphen("HOK3.AUTO_FAXNO(+)") & " = :AUTO_FAX " & vbCrLf)
                whe.Append("  AND ( " & vbCrLf)
                'JA支所指定除外条件
                whe.Append("    (HOK1.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                whe.Append("      AND NOT EXISTS( " & vbCrLf)
                whe.Append("             SELECT 'X' FROM HOKOKU C " & vbCrLf)
                whe.Append("             WHERE " & vbCrLf)
                whe.Append("                 C.KURACD = HOK1.KURACD " & vbCrLf)
                whe.Append("             AND C.ACBCD = HOK1.ACBCD " & vbCrLf)
                whe.Append("             AND C.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                whe.Append("             AND C.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                whe.Append("             AND REPLACE(C.AUTO_FAXNO,'-') <> REPLACE(HOK1.AUTO_FAXNO,'-') " & vbCrLf)
                whe.Append("             AND C.USERCD_FROM = TAI.USER_CD " & vbCrLf)
                whe.Append("             AND C.USERCD_TO IS NULL " & vbCrLf)
                whe.Append("      ) " & vbCrLf)
                whe.Append("      AND NOT EXISTS( " & vbCrLf)
                whe.Append("             SELECT 'X' FROM HOKOKU C " & vbCrLf)
                whe.Append("             WHERE " & vbCrLf)
                whe.Append("                 C.KURACD = HOK1.KURACD " & vbCrLf)
                whe.Append("             AND C.ACBCD = HOK1.ACBCD " & vbCrLf)
                whe.Append("             AND C.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                whe.Append("             AND C.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                whe.Append("             AND REPLACE(C.AUTO_FAXNO,'-') <> REPLACE(HOK1.AUTO_FAXNO,'-') " & vbCrLf)
                whe.Append("             AND TAI.USER_CD BETWEEN C.USERCD_FROM AND C.USERCD_TO " & vbCrLf)
                whe.Append("             AND C.USERCD_TO IS NOT NULL " & vbCrLf)
                whe.Append("      ) " & vbCrLf)
                whe.Append("    ) " & vbCrLf)

                '顧客範囲指定条件除外
                whe.Append("    OR " & vbCrLf)
                whe.Append("    (HOK2.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                whe.Append("      AND NOT EXISTS( " & vbCrLf)
                whe.Append("             SELECT 'X' FROM HOKOKU C " & vbCrLf)
                whe.Append("             WHERE " & vbCrLf)
                whe.Append("                 C.KURACD = HOK2.KURACD " & vbCrLf)
                whe.Append("             AND C.ACBCD = HOK2.ACBCD " & vbCrLf)
                whe.Append("             AND C.AUTO_KBN IN ('" & strFAXKBN & "', '" & strBoth & "') " & vbCrLf)
                whe.Append("             AND C.AUTO_FAXNO IS NOT NULL " & vbCrLf)
                whe.Append("             AND REPLACE(C.AUTO_FAXNO,'-') <> REPLACE(HOK2.AUTO_FAXNO,'-') " & vbCrLf)
                whe.Append("             AND C.USERCD_FROM = TAI.USER_CD " & vbCrLf)
                whe.Append("             AND C.USERCD_TO IS NULL " & vbCrLf)
                whe.Append("      ) " & vbCrLf)
                whe.Append("    ) " & vbCrLf)
                '顧客直接指定
                whe.Append("  OR " & vbCrLf)
                whe.Append("  HOK3.AUTO_FAXNO IS NOT NULL) " & vbCrLf)


            Else 'メール送信？
                whe.Append("  AND HOK1.AUTO_KBN(+) IN ('" & strMAILKBN & "','" & strBoth & "') " & vbCrLf)
                whe.Append("  AND HOK2.AUTO_KBN(+) IN ('" & strMAILKBN & "','" & strBoth & "') " & vbCrLf)
                whe.Append("  AND HOK3.AUTO_KBN(+) IN ('" & strMAILKBN & "','" & strBoth & "') " & vbCrLf)
                whe.Append("  AND HOK1.AUTO_MAIL(+) = :AUTO_MAIL " & vbCrLf)
                whe.Append("  AND HOK2.AUTO_MAIL(+) = :AUTO_MAIL " & vbCrLf)
                whe.Append("  AND HOK3.AUTO_MAIL(+) = :AUTO_MAIL " & vbCrLf)
                whe.Append("  AND (  " & vbCrLf)
                'JA支所除外条件
                whe.Append("  (HOK1.AUTO_MAIL IS NOT NULL " & vbCrLf)
                whe.Append("   AND NOT EXISTS( " & vbCrLf)
                whe.Append("             SELECT 'X' FROM HOKOKU C " & vbCrLf)
                whe.Append("             WHERE " & vbCrLf)
                whe.Append("                 C.KURACD = HOK1.KURACD " & vbCrLf)
                whe.Append("             AND C.ACBCD = HOK1.ACBCD " & vbCrLf)
                whe.Append("             AND C.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                whe.Append("             AND C.AUTO_MAIL IS NOT NULL " & vbCrLf)
                whe.Append("             AND C.AUTO_MAIL <> HOK1.AUTO_MAIL " & vbCrLf)
                whe.Append("             AND C.USERCD_FROM = TAI.USER_CD " & vbCrLf)
                whe.Append("             AND C.USERCD_TO IS NULL " & vbCrLf)
                whe.Append("   ) " & vbCrLf)
                whe.Append("   AND NOT EXISTS( " & vbCrLf)
                whe.Append("             SELECT 'X' FROM HOKOKU C " & vbCrLf)
                whe.Append("             WHERE " & vbCrLf)
                whe.Append("                 C.KURACD = HOK1.KURACD  " & vbCrLf)
                whe.Append("             AND C.ACBCD = HOK1.ACBCD " & vbCrLf)
                whe.Append("             AND C.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                whe.Append("             AND C.AUTO_MAIL IS NOT NULL " & vbCrLf)
                whe.Append("             AND C.AUTO_MAIL <> HOK1.AUTO_MAIL " & vbCrLf)
                whe.Append("             AND TAI.USER_CD BETWEEN C.USERCD_FROM AND C.USERCD_TO " & vbCrLf)
                whe.Append("             AND C.USERCD_TO IS NOT NULL " & vbCrLf)
                whe.Append("   ) " & vbCrLf)
                whe.Append("  ) " & vbCrLf)
                '顧客範囲指定除外条件
                whe.Append("  OR " & vbCrLf)
                whe.Append("  (HOK2.AUTO_MAIL IS NOT NULL " & vbCrLf)
                whe.Append("   AND NOT EXISTS( " & vbCrLf)
                whe.Append("             SELECT 'X' FROM HOKOKU C " & vbCrLf)
                whe.Append("             WHERE " & vbCrLf)
                whe.Append("                 C.KURACD = HOK2.KURACD " & vbCrLf)
                whe.Append("             AND C.ACBCD = HOK2.ACBCD " & vbCrLf)
                whe.Append("             AND C.AUTO_KBN IN ('" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                whe.Append("             AND C.AUTO_MAIL IS NOT NULL " & vbCrLf)
                whe.Append("             AND C.AUTO_MAIL <> HOK2.AUTO_MAIL " & vbCrLf)
                whe.Append("             AND C.USERCD_FROM = TAI.USER_CD " & vbCrLf)
                whe.Append("             AND C.USERCD_TO IS NULL " & vbCrLf)
                whe.Append("   ) " & vbCrLf)
                whe.Append("  ) " & vbCrLf)
                '顧客直接指定
                whe.Append("  OR   " & vbCrLf)
                whe.Append("  HOK3.AUTO_MAIL IS NOT NULL)  " & vbCrLf)

            End If
            whe.Append("AND TAI.KURACD = HOK1.KURACD(+) " & vbCrLf)
            whe.Append("AND TAI.ACBCD = HOK1.ACBCD(+) " & vbCrLf)
            whe.Append("AND HOK1.USERCD_TO(+) IS NULL " & vbCrLf)
            whe.Append("AND HOK1.USERCD_FROM(+) IS NULL " & vbCrLf)
            whe.Append("AND TAI.KURACD = HOK2.KURACD(+) " & vbCrLf)
            whe.Append("AND TAI.ACBCD = HOK2.ACBCD(+) " & vbCrLf)
            whe.Append("AND TAI.USER_CD BETWEEN HOK2.USERCD_FROM(+) AND HOK2.USERCD_TO(+) " & vbCrLf)
            whe.Append("AND HOK2.USERCD_FROM(+) IS NOT NULL " & vbCrLf)
            whe.Append("AND HOK2.USERCD_TO(+) IS NOT NULL " & vbCrLf)
            whe.Append("AND TAI.KURACD = HOK3.KURACD(+) " & vbCrLf)
            whe.Append("AND TAI.ACBCD = HOK3.ACBCD(+) " & vbCrLf)
            whe.Append("AND TAI.USER_CD = HOK3.USERCD_FROM(+) " & vbCrLf)
            whe.Append("AND HOK3.USERCD_FROM(+) IS NOT NULL " & vbCrLf)
            whe.Append("AND HOK3.USERCD_TO(+) IS NULL " & vbCrLf)
            whe.Append("AND TAI.KURACD = HN.CLI_CD(+) " & vbCrLf)
            whe.Append("AND TAI.ACBCD = HN.HAN_CD(+) " & vbCrLf)
            whe.Append("AND SUBSTR(HN.CLI_CD,2,2) = KYO.KEN_CD(+) " & vbCrLf)
            whe.Append("AND HN.HAISO_CD = KYO.HAISO_CD(+) " & vbCrLf)

            ord.Append("    HATYMDT " & vbCrLf) 'ソートを販売店とクライアントで変える

        Else '2:ｸﾗｲｱﾝﾄ？

            sel.Append("    ,HOK.AUTO_MAIL_PASS AS YOBI5 " & vbCrLf) ' パスワード
            sel.Append("    ,TAI.JACD AS JA_CD " & vbCrLf)

            fro.Append("    ,M11_JAHOKOKU HOK " & vbCrLf)
            fro.Append("    ,HN2MAS JAS " & vbCrLf)

            whe.Append("  AND HOK.KBN = '1' " & vbCrLf)
            whe.Append("  AND HOK.GROUPCD = TAI.KURACD " & vbCrLf)
            If pstrAUTO = strFAXKBN Then 'ＦＡＸ送信？
                whe.Append("  AND HOK.AUTO_KBN IN ('" & strFAXKBN & "','" & strBoth & "') " & vbCrLf)
                whe.Append("  AND " & ReplaceHyphen("HOK.AUTO_FAXNO") & " = :AUTO_FAX " & vbCrLf)
            Else 'メール送信？
                whe.Append("  AND HOK.AUTO_KBN IN ('" & strMAILKBN & "','" & strBoth & "') " & vbCrLf)
                whe.Append("  AND HOK.AUTO_MAIL = :AUTO_MAIL " & vbCrLf)
            End If
            whe.Append("  AND TAI.KURACD = JAS.CLI_CD(+) " & vbCrLf)
            whe.Append("  AND TAI.ACBCD = JAS.HAN_CD(+) " & vbCrLf)
            whe.Append("  AND SUBSTR(JAS.CLI_CD,2,2) = KYO.KEN_CD(+) " & vbCrLf)
            whe.Append("  AND JAS.HAISO_CD = KYO.HAISO_CD(+) " & vbCrLf)

            ord.Append("    CLI_CD, JA_CD, HATYMDT " & vbCrLf) ' 2011/05/27 T.Watabe add ソートを販売店とクライアントで変える

        End If
        Dim faxUser As String = ""
        Try
            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：3") '2014/04/11 T.Ono add ログ強化
            '//------------------------------------------------
            ' データのSELECT
            '//------------------------------------------------
            strSQL = New StringBuilder("")
            'SQL作成開始
            'JA報告先マスタの絞り込み
            strSQL.Append(wit)
            strSQL.Append("SELECT " & vbCrLf)
            strSQL.Append("    CLI.CLI_CD,  " & vbCrLf) '2011/03/10 T.Watabe add
            strSQL.Append("    CLI.CLI_NAME,  " & vbCrLf) '2011/05/27 T.Watabe add
            strSQL.Append("    KOK.USER_CD SH_USER, " & vbCrLf)
            strSQL.Append("    TAI.JACD,  " & vbCrLf) '2013/09/25 T.Watabe add
            strSQL.Append("    TAI.JANM, " & vbCrLf)
            strSQL.Append("    TAI.ACBNM, " & vbCrLf)
            strSQL.Append("    TAI.KENNM, " & vbCrLf)
            strSQL.Append("    KYO.NAME, " & vbCrLf)
            strSQL.Append("    TAI.JUSYONM, " & vbCrLf)
            strSQL.Append("    TAI.ACBCD, " & vbCrLf)
            strSQL.Append("    TAI.USER_CD, " & vbCrLf)
            strSQL.Append("    TAI.KTELNO, " & vbCrLf)
            strSQL.Append("    TAI.SDNM, " & vbCrLf)
            strSQL.Append("    TAI.JUTEL1, " & vbCrLf)
            strSQL.Append("    TAI.JUTEL2, " & vbCrLf)
            '2017/11/17 H.Mori mod 2017改善開発 No9-1 START
            'strSQL.Append("    KOK.USER_FLG, " & vbCrLf)
            strSQL.Append("    TAI.UNYO, " & vbCrLf)
            '2017/11/17 H.Mori mod 2017改善開発 No9-1 END
            strSQL.Append("    TAI.RENTEL, " & vbCrLf)
            strSQL.Append("    TAI.ADDR, " & vbCrLf)
            '2017/10/27 H.Mori mod 2017改善開発 No9-1 START
            'strSQL.Append("    KOK.GAS_STOP AS GAS_START, " & vbCrLf)
            'strSQL.Append("    KOK.GAS_DELE, " & vbCrLf)
            strSQL.Append("    TAI.GAS_STOP AS GAS_START, " & vbCrLf)
            strSQL.Append("    TAI.GAS_DELE, " & vbCrLf)
            '2017/10/27 H.Mori mod 2017改善開発 No9-1 END
            strSQL.Append("    TAI.TIZUNO, " & vbCrLf)
            '2017/10/27 H.Mori mod 2017改善開発 No9-1 START
            'strSQL.Append("    KOK.SHUGOU, " & vbCrLf)
            strSQL.Append("    TAI.SHUGOU, " & vbCrLf)
            '2017/10/27 H.Mori mod 2017改善開発 No9-1 END
            strSQL.Append("    TAI.NCU_SET, " & vbCrLf)
            strSQL.Append("    TAI.HATYMD, " & vbCrLf)
            strSQL.Append("    TAI.HATTIME, " & vbCrLf)
            strSQL.Append("    TAI.KENSIN, " & vbCrLf)
            strSQL.Append("    TAI.RYURYO, " & vbCrLf)
            strSQL.Append("    TAI.METASYU, " & vbCrLf)
            strSQL.Append("    TAI.KMNM1, " & vbCrLf)
            strSQL.Append("    TAI.KMNM2, " & vbCrLf)
            strSQL.Append("    TAI.KMNM3, " & vbCrLf)
            strSQL.Append("    TAI.KMNM4, " & vbCrLf)
            strSQL.Append("    TAI.KMNM5, " & vbCrLf)
            strSQL.Append("    TAI.KMNM6, " & vbCrLf)
            strSQL.Append("    TAI.MITOKBN, " & vbCrLf)
            strSQL.Append("    TAI.TAIOKBN_NAI, " & vbCrLf)
            strSQL.Append("    TAI.TMSKB_NAI, " & vbCrLf)
            strSQL.Append("    TAI.SYONO, " & vbCrLf)
            strSQL.Append("    TAI.TKTANCD_NM, " & vbCrLf)
            strSQL.Append("    TAI.SYOYMD, " & vbCrLf)
            strSQL.Append("    TAI.SYOTIME, " & vbCrLf)
            strSQL.Append("    TAI.TAITNM, " & vbCrLf)
            strSQL.Append("    TAI.TELRNM, " & vbCrLf)
            strSQL.Append("    TAI.FUK_MEMO, " & vbCrLf)
            strSQL.Append("    TAI.TEL_MEMO1, " & vbCrLf)
            strSQL.Append("    TAI.TEL_MEMO2, " & vbCrLf)
            strSQL.Append("    TAI.TEL_MEMO4, " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
            strSQL.Append("    TAI.TEL_MEMO5, " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
            strSQL.Append("    TAI.TEL_MEMO6, " & vbCrLf)    '2020/11/01 T.Ono add 2020監視改善
            strSQL.Append("    TAI.TKIGNM, " & vbCrLf)
            strSQL.Append("    TAI.TSADNM, " & vbCrLf)
            strSQL.Append("    TAI.SIJIYMD, " & vbCrLf) '2015/03/06 T.Ono add 2014改善開発 No16
            strSQL.Append("    TAI.SIJITIME, " & vbCrLf) '2015/03/06 T.Ono add 2014改善開発 No16
            strSQL.Append("    TAI.SIJI_BIKO1, " & vbCrLf)
            strSQL.Append("    TAI.SIJI_BIKO2, " & vbCrLf)
            strSQL.Append("    TAI.SYUTDTNM, " & vbCrLf)
            strSQL.Append("    TAI.STD_KYOTEN, " & vbCrLf)
            strSQL.Append("    TAI.TSTANNM, " & vbCrLf)
            strSQL.Append("    TAI.TYAKYMD, " & vbCrLf)
            strSQL.Append("    TAI.TYAKTIME, " & vbCrLf)
            strSQL.Append("    TAI.SYOKANYMD, " & vbCrLf)
            strSQL.Append("    TAI.SYOKANTIME, " & vbCrLf)
            strSQL.Append("    TAI.AITNM, " & vbCrLf)
            strSQL.Append("    TAI.SDTBIK1, " & vbCrLf)
            strSQL.Append("    TAI.FKINM, " & vbCrLf)
            strSQL.Append("    TAI.KIGTAIYO, " & vbCrLf)
            strSQL.Append("    TAI.JAKENREN, " & vbCrLf)
            strSQL.Append("    TAI.RENTIME, " & vbCrLf)
            strSQL.Append("    TAI.GASMUMU, " & vbCrLf)
            strSQL.Append("    TAI.ORGENIN, " & vbCrLf)
            strSQL.Append("    TAI.GASGUMU, " & vbCrLf)
            strSQL.Append("    TAI.HOSKOKAN, " & vbCrLf)
            strSQL.Append("    TAI.METYOINA, " & vbCrLf)
            strSQL.Append("    TAI.TYOUYOINA, " & vbCrLf)
            strSQL.Append("    TAI.VALYOINA, " & vbCrLf)
            strSQL.Append("    TAI.KYUHAIUMU, " & vbCrLf)
            strSQL.Append("    TAI.COYOINA, " & vbCrLf)
            strSQL.Append("    TAI.SDTBIK2, " & vbCrLf)
            strSQL.Append("    TAI.SDTBIK3, " & vbCrLf)
            strSQL.Append("    TAI.SNTTOKKI, " & vbCrLf)
            strSQL.Append("    TAI.METFUKKI, " & vbCrLf)
            strSQL.Append("    TAI.HOAN, " & vbCrLf)
            strSQL.Append("    TAI.GASGIRE, " & vbCrLf)
            strSQL.Append("    TAI.KIGKOSYO, " & vbCrLf)
            strSQL.Append("    TAI.CSNTGEN, " & vbCrLf)
            strSQL.Append("    TAI.CSNTNGAS, " & vbCrLf)
            strSQL.Append("    TAI.SDTBIK1, " & vbCrLf)
            strSQL.Append("    TAI.STD_CD, " & vbCrLf)
            strSQL.Append("    TAI.STD, " & vbCrLf)
            strSQL.Append("    TAI.TAIOKBN, " & vbCrLf)
            strSQL.Append("    TAI.TFKICD, " & vbCrLf)
            strSQL.Append("    PL3.NAME AS SHUGOUNM, " & vbCrLf)
            strSQL.Append("    PL5.NAME AS RYURYONM " & vbCrLf)
            strSQL.Append("    ,TAI.HATYMD || '-' || TAI.HATTIME AS HATYMDT " & vbCrLf)
            strSQL.Append("    ,TAI.SIJIYMD " & vbCrLf)  ' 出動指示日
            strSQL.Append("    ,TAI.SIJITIME " & vbCrLf) ' 出動指示時刻
            strSQL.Append("    ,TAI.SDYMD " & vbCrLf)    ' 出動日
            strSQL.Append("    ,TAI.SDTIME " & vbCrLf)   ' 出動時刻
            strSQL.Append("    ,TAI.KIGNM " & vbCrLf)    ' ガス器具
            strSQL.Append("    ,TAI.SADNM " & vbCrLf)    ' メータ作動原因１
            strSQL.Append("    ,TAI.SDSKBN_NAI " & vbCrLf)  ' 出動会社処理区分・内容
            strSQL.Append(sel) ' パスワード
            strSQL.Append("FROM CLIMAS CLI, " & vbCrLf)
            strSQL.Append("     D20_TAIOU_COPY TAI, " & vbCrLf)
            strSQL.Append("     HAIMAS KYO, " & vbCrLf)
            strSQL.Append("     SHAMAS KOK, " & vbCrLf)
            strSQL.Append("     M06_PULLDOWN PL3, " & vbCrLf)
            strSQL.Append("     M06_PULLDOWN PL5 " & vbCrLf)
            strSQL.Append(fro)
            strSQL.Append("WHERE " & vbCrLf)
            strSQL.Append("    CLI.KANSI_CODE = '" & pstrKANSI_CODE & "'  " & vbCrLf)
            strSQL.Append("AND CLI.CLI_CD     = TAI.KURACD " & vbCrLf)
            strSQL.Append(whe)
            strSQL.Append(pstrWHERE_TAIOU.ToString)
            strSQL.Append("  AND TAI.KURACD   = KOK.CLI_CD(+) " & vbCrLf)
            strSQL.Append("  AND TAI.ACBCD    = KOK.HAN_CD(+) " & vbCrLf)
            strSQL.Append("  AND TAI.USER_CD  = KOK.USER_CD(+) " & vbCrLf)
            strSQL.Append("  AND '03'         = PL3.KBN(+) " & vbCrLf)
            '2017/10/27 H.Mori mod 2017改善開発 No9-1 START
            'strSQL.Append("  AND KOK.SHUGOU   = PL3.CD(+) " & vbCrLf)
            strSQL.Append("  AND TAI.SHUGOU   = PL3.CD(+) " & vbCrLf)
            '2017/10/27 H.Mori mod 2017改善開発 No9-1 END
            strSQL.Append("  AND '05'         = PL5.KBN(+) " & vbCrLf)
            strSQL.Append("  AND TAI.RYURYO   = PL5.CD(+) " & vbCrLf)
            strSQL.Append("ORDER BY " & vbCrLf)
            strSQL.Append(ord) ' ソート

            If pintDebugSQLNo = 3 Or (pintDebugSQLNo >= 2000 And pintDebugSQLNo <= 2999) Then
                If pstrAUTO = strFAXKBN Then
                    Return "DEBUG[" & strSQL.ToString & "]"
                End If
            End If
            If pintDebugSQLNo = 4 Or (pintDebugSQLNo >= 3000 And pintDebugSQLNo <= 3999) Then
                If pstrAUTO <> strFAXKBN Then
                    Return "DEBUG[" & strSQL.ToString & "]"
                End If
            End If

            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：4") '2014/04/11 T.Ono add ログ強化
            cdb.pSQL = strSQL.ToString '//SQLセット

            '//パラメータのセット
            If pstrAUTO = strFAXKBN Then
                cdb.pSQLParamStr("AUTO_FAX") = pstrFAXNO 'ＦＡＸ送信の場合
            Else
                cdb.pSQLParamStr("AUTO_MAIL") = pstrMAILAD 'メール送信の場合
            End If

            cdb.mExecQuery() '//SQL実行
            ds = cdb.pResult  '//データセットに格納

            If pintDebugSQLNo = 104 Then ' DEBUG
                Return "DEBUG:(" & pintDebugSQLNo & ")[" & strSQL.ToString & "]" & pstrFAXNO & "|" & pstrMAILAD
            End If

            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：5 count:" & ds.Tables(0).Rows.Count) '2014/04/11 T.Ono add ログ強化
            '//------------------------------------------------
            '// ファイルの作成１（基本情報設定）
            '//------------------------------------------------
            If True Then
                ExcelC.pKencd = "00"            '県コードをセット
                ExcelC.pSessionID = pstrSESSION 'セッションID　※処理日付＆電話番号を設定する
                ExcelC.pRepoID = strPGID        '帳票ID
                ExcelC.pLandScape = False       '帳票縦
                ExcelC.mOpen()                  'ファイルオープン
                'タイトル
                '2014/03/20 T.Ono mod FAXタイトル変更要望　FAX・メール共通に
                'If pstrAUTO = strFAXKBN Then
                '    'ＦＡＸ送信の場合
                '    ExcelC.pTitle = "監視センター対応内容明細（ＦＡＸ）"
                'Else
                '    'メール送信の場合
                '    ExcelC.pTitle = "監視センター対応内容明細（メール）"
                'End If
                ExcelC.pTitle = "監視センター対応結果明細（ご報告）"
                'If pintDebugSQLNo = 0 Then '2013/10/01 T.Watabe edit デバッグ時は帳票の右上作成日を指定した日付にする。
                '    ExcelC.pDate = DateFncC.mGet(Now.ToString("yyyyMMdd")) '作成日
                'Else
                '    ExcelC.pDate = DateFncC.mGet(pstrTAISYOUBI) '作成日
                'End If
                ExcelC.pDate = DateFncC.mGet(pstrTAISYOUBI) '作成日
                'ExcelC.pScale = 95               '縮小拡大率 2013/11/26 T.Ono del
                If pstrSEND_KBN = "2" Then '2:クライアントの目印をフッタへ付ける
                    ExcelC.pFooter = "&R ."
                Else
                    'ExcelC.pFooter = "&R &P \/ " & pintCNT & " "
                End If

                '余白
                '2005/10/03 NEC UPDATE
                '2005/12/22 NEC UPDATE
                '2006/06/21 NEC UPDATE 
                ExcelC.pMarginTop = 1.8D
                ExcelC.pMarginBottom = 0.5D
                ExcelC.pMarginLeft = 1.2D
                ExcelC.pMarginRight = 1.5D
                ExcelC.pMarginHeader = 1D
                'ExcelC.pMarginFooter = 1D ' 2010/10/06 T.Watabe edit
                ExcelC.pMarginFooter = 0.5D
            End If

            If ds.Tables(0).Rows.Count = 0 Then  '//データが存在しない？

                '１ ゼロ件で、ゼロ件送信の場合。

                If pintDebugSQLNo = 105 Then ' DEBUG
                    Return "DEBUG:(" & pintDebugSQLNo & ")[" & strSQL.ToString & "]" & pstrFAXNO & "|" & pstrMAILAD
                End If

                wheC = New StringBuilder("")
                wheJ = New StringBuilder("")

                'クライアントコードの範囲指定を追加
                If pstrKURACD_F.Length > 0 Then
                    wheC.Append("AND CLI.CLI_CD >= '" & pstrKURACD_F & "' " & vbCrLf)
                    wheJ.Append("AND A.KURACD >= '" & pstrKURACD_F & "' " & vbCrLf)
                End If
                If pstrKURACD_T.Length > 0 Then
                    wheC.Append("AND CLI.CLI_CD <= '" & pstrKURACD_T & "' " & vbCrLf)
                    wheJ.Append("AND A.KURACD <= '" & pstrKURACD_T & "' " & vbCrLf)
                End If


                'Return "DATA0" '//データが0件であることを示す文字列を返す
                '-------------
                ' マスタ情報取得
                '-------------
                strSQL = New StringBuilder("")
                If pstrSEND_KBN = "1" Then '1:販売所？
                    'JA報告先マスタの絞り込み
                    strSQL.Append("WITH HOKOKU AS ( " & vbCrLf)
                    strSQL.Append("  SELECT " & vbCrLf)
                    strSQL.Append("      A.KURACD " & vbCrLf)
                    strSQL.Append("      ,A.ACBCD " & vbCrLf)
                    strSQL.Append("      ,A.USERCD_FROM " & vbCrLf)
                    strSQL.Append("      ,A.USERCD_TO " & vbCrLf)
                    strSQL.Append("      ,B.GROUPCD " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_FAXNM " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_MAIL " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_MAIL_PASS " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_FAXNO " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_KBN " & vbCrLf)
                    strSQL.Append("      ,B.AUTO_ZERO_FLG " & vbCrLf)
                    strSQL.Append("  FROM " & vbCrLf)
                    strSQL.Append("      M09_JAGROUP A " & vbCrLf)
                    strSQL.Append("      ,M11_JAHOKOKU B " & vbCrLf)
                    strSQL.Append("      ,CLIMAS CLI " & vbCrLf)
                    strSQL.Append("  WHERE " & vbCrLf)
                    strSQL.Append("      A.KURACD = CLI.CLI_CD " & vbCrLf)
                    strSQL.Append("  AND A.KBN = '002' " & vbCrLf)
                    strSQL.Append("  AND B.GROUPCD = A.GROUPCD " & vbCrLf)
                    strSQL.Append("  AND B.KBN = '2' " & vbCrLf)
                    strSQL.Append("  AND B.AUTO_KBN IN ('" & strFAXKBN & "', '" & strMAILKBN & "', '" & strBoth & "') " & vbCrLf)
                    strSQL.Append(pstrWHERE_CLI.ToString)
                    strSQL.Append(") " & vbCrLf)

                    strSQL.Append("SELECT DISTINCT " & vbCrLf)
                    strSQL.Append("    CLI.CLI_CD, " & vbCrLf)
                    strSQL.Append("    JAS.JA_NAME AS JANM, " & vbCrLf)
                    strSQL.Append("    KYO.NAME AS CENTER_NAME, " & vbCrLf)
                    strSQL.Append("    CLI.KEN_NAME, " & vbCrLf)
                    strSQL.Append("    KAN.TEL, " & vbCrLf)
                    strSQL.Append("    HOK.AUTO_MAIL_PASS AS YOBI5 " & vbCrLf)
                    strSQL.Append("FROM " & vbCrLf)
                    strSQL.Append("    CLIMAS CLI, " & vbCrLf)
                    strSQL.Append("    ( " & vbCrLf)
                    strSQL.Append("      SELECT " & vbCrLf)
                    strSQL.Append("        A.KURACD, " & vbCrLf)
                    strSQL.Append("        A.ACBCD, " & vbCrLf)
                    strSQL.Append("        A.AUTO_MAIL_PASS " & vbCrLf)
                    strSQL.Append("      FROM " & vbCrLf)
                    strSQL.Append("        HOKOKU A " & vbCrLf)
                    strSQL.Append("      WHERE 1=1 " & vbCrLf)
                    strSQL.Append(wheJ)
                    strSQL.Append("      AND A.AUTO_ZERO_FLG   = '1' " & vbCrLf)
                    If pstrAUTO = "1" Then '1:FAX送信
                        strSQL.Append("      AND A.AUTO_KBN IN ('" & strFAXKBN & "','" & strBoth & "') " & vbCrLf)
                        strSQL.Append("      AND " & ReplaceHyphen("A.AUTO_FAXNO(+)") & " = '" & pstrFAXNO & "' " & vbCrLf)
                    Else
                        strSQL.Append("      AND A.AUTO_KBN IN ('" & strMAILKBN & "','" & strBoth & "') " & vbCrLf)
                        strSQL.Append("      AND A.AUTO_MAIL = '" & pstrMAILAD & "' " & vbCrLf)
                    End If
                    strSQL.Append("    ) HOK, " & vbCrLf)
                    strSQL.Append("    HN2MAS JAS, " & vbCrLf)
                    strSQL.Append("    HAIMAS KYO, " & vbCrLf)
                    strSQL.Append("    KANSIMAS KAN " & vbCrLf)
                    strSQL.Append("WHERE " & vbCrLf)
                    strSQL.Append("    CLI.KANSI_CODE  = '" & pstrKANSI_CODE & "' " & vbCrLf)
                    strSQL.Append("AND CLI.CLI_CD      = HOK.KURACD " & vbCrLf)
                    strSQL.Append("AND HOK.KURACD      = JAS.CLI_CD(+) " & vbCrLf)
                    strSQL.Append("AND HOK.ACBCD       = JAS.HAN_CD(+) " & vbCrLf)
                    strSQL.Append("AND SUBSTR(JAS.CLI_CD,2,2) = KYO.KEN_CD(+) " & vbCrLf)
                    strSQL.Append("AND JAS.HAISO_CD    = KYO.HAISO_CD(+) " & vbCrLf)
                    strSQL.Append("AND CLI.KANSI_CODE  = KAN.KANSI_CD(+) " & vbCrLf)
                    strSQL.Append(wheC)

                Else '2:ｸﾗｲｱﾝﾄ？
                    strSQL.Append("SELECT DISTINCT " & vbCrLf)
                    strSQL.Append("    CLI.CLI_CD,  " & vbCrLf) '2011/03/10 T.Watabe add
                    strSQL.Append("    '' AS JANM,  " & vbCrLf) '2011/05/19 T.Watabe add
                    strSQL.Append("    CLI.CLI_NAME AS CENTER_NAME, " & vbCrLf)
                    strSQL.Append("    CLI.KEN_NAME, " & vbCrLf)
                    strSQL.Append("    KAN.TEL, " & vbCrLf)
                    strSQL.Append("    HOK.AUTO_MAIL_PASS AS YOBI5 " & vbCrLf) ' パスワード '2011/05/26 T.Watabe add
                    strSQL.Append("FROM " & vbCrLf)
                    strSQL.Append("    CLIMAS CLI, " & vbCrLf)
                    strSQL.Append("    M11_JAHOKOKU HOK, " & vbCrLf)
                    strSQL.Append("    KANSIMAS KAN " & vbCrLf)
                    strSQL.Append("WHERE " & vbCrLf)
                    strSQL.Append("    CLI.KANSI_CODE  = '" & pstrKANSI_CODE & "' " & vbCrLf)
                    strSQL.Append("AND HOK.KBN = '1' " & vbCrLf)
                    strSQL.Append("AND HOK.GROUPCD = CLI.CLI_CD  " & vbCrLf)
                    strSQL.Append("AND HOK.AUTO_ZERO_FLG   = '1' " & vbCrLf)
                    If pstrAUTO = "1" Then
                        strSQL.Append("AND HOK.AUTO_KBN IN ('" & strFAXKBN & "','" & strBoth & "') " & vbCrLf)
                        strSQL.Append("AND " & ReplaceHyphen("HOK.AUTO_FAXNO") & " = '" & pstrFAXNO & "' " & vbCrLf)
                    Else
                        strSQL.Append("AND HOK.AUTO_KBN IN ('" & strMAILKBN & "','" & strBoth & "') " & vbCrLf)
                        strSQL.Append("AND HOK.AUTO_MAIL = '" & pstrMAILAD & "' " & vbCrLf)
                    End If
                    strSQL.Append("AND KAN.KANSI_CD (+)= CLI.KANSI_CODE " & vbCrLf)
                    strSQL.Append(wheC) ' 2011/05/26 T.Watabe edit
                End If

                ' DEBUG
                If pintDebugSQLNo = 5 Or (pintDebugSQLNo >= 4000 And pintDebugSQLNo <= 4999) Then
                    If pstrAUTO = strFAXKBN Then
                        Return "DEBUG[" & strSQL.ToString & "]"
                    End If
                End If
                If pintDebugSQLNo = 6 Or (pintDebugSQLNo >= 5000 And pintDebugSQLNo <= 5999) Then
                    If pstrAUTO <> strFAXKBN Then
                        Return "DEBUG[" & strSQL.ToString & "]"
                    End If
                End If
                cdb.pSQL = strSQL.ToString   '//SQLセット
                cdb.mExecQuery() '//SQL実行

                Dim dsInfo As New DataSet
                Dim drInfo As DataRow
                dsInfo = cdb.pResult  '//データセットに格納

                Dim kenName As String = ""
                Dim jalpTel As String = ""

                If pintDebugSQLNo = 106 Then ' DEBUG
                    Return "DEBUG:(" & pintDebugSQLNo & ")[" & strSQL.ToString & "]"
                End If

                If dsInfo.Tables(0).Rows.Count > 0 Then  '//データが存在する？
                    drInfo = dsInfo.Tables(0).Rows(0)
                    kenName = Convert.ToString(drInfo.Item("KEN_NAME"))
                    centerName = Convert.ToString(drInfo.Item("CENTER_NAME"))
                    jalpTel = Convert.ToString(drInfo.Item("TEL"))
                    cliCd4FileHead = Convert.ToString(drInfo.Item("CLI_CD")) '2011/03/10 T.Watabe add
                    jaName4FileHead = Convert.ToString(drInfo.Item("JANM")).Replace(" ", "") '2011/05/19 T.Watabe add
                    sZipFilePass = Convert.ToString(drInfo.Item("YOBI5")) ' 2011/05/26 T.Watabe add
                End If

                Dim taisyoDate As String = fncAdd_Date(pstrTAISYOUBI, -1)
                taisyoDate = taisyoDate.Substring(0, 4) & "/" & taisyoDate.Substring(4, 2) & "/" & taisyoDate.Substring(6, 2)
                faxUser = pstrFAXNO & jaName4FileHead

                '//------------------------------------------------
                '// ファイルの作成２（データ設定）
                '//------------------------------------------------
                ExcelC.mHeader(intGYOSU, 30, 1)

                '各列の幅をピクセルでセット。枠線も消す。
                '1行目
                ExcelC.pCellStyle(1) = "width:32px;border-style:none"
                ExcelC.pCellStyle(2) = "width:122px;border-style:none"
                ExcelC.pCellStyle(3) = "width:72px;border-style:none"
                ExcelC.pCellStyle(4) = "width:72px;border-style:none"

                ExcelC.pCellStyle(5) = "width:90px;border-style:none"
                ExcelC.pCellStyle(6) = "width:115px;border-style:none"
                ExcelC.pCellStyle(7) = "width:72px;border-style:none"
                ExcelC.pCellStyle(8) = "width:52px;border-style:none"
                ExcelC.pCellStyle(9) = "width:80px;border-style:none"
                ExcelC.pCellStyle(10) = "width:3px;border-style:none" ' 2010/09/17 T.Watabe edit
                ExcelC.pCellStyle(11) = "width:3px;border-style:none" ' 2010/09/17 T.Watabe add
                ExcelC.pCellStyle(12) = "width:3px;border-style:none" ' 2010/09/17 T.Watabe add
                ExcelC.pCellVal(1) = ""
                ExcelC.pCellVal(2) = ""
                ExcelC.pCellVal(3) = ""
                ExcelC.pCellVal(4) = ""
                ExcelC.pCellVal(5) = ""
                ExcelC.pCellVal(6) = ""
                ExcelC.pCellVal(7) = ""
                ExcelC.pCellVal(8) = ""
                ExcelC.pCellVal(9) = ""
                ExcelC.pCellVal(10) = ""
                ExcelC.pCellVal(11) = "" ' 2010/09/17 T.Watabe add
                ExcelC.pCellVal(12) = "" ' 2010/09/17 T.Watabe add
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '1行目
                ExcelC.pCellStyle(1) = "border-style:none"
                If pstrAUTO = "1" Then
                    ExcelC.pCellVal(1, "colspan=6") = "送信先FAX番号：" & pstrFAXNO
                Else
                    ExcelC.pCellVal(1, "colspan=6") = ""
                End If
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '2行目
                ExcelC.pCellStyle(1) = "border-style:none"
                ExcelC.pCellVal(1, "colspan=6") = ""
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '3行目
                ExcelC.pCellStyle(1) = "border-style:none"
                'ExcelC.pCellVal(1, "colspan=10") = "ＪＡ名       ：" & jaName4FileHead '2015/03/06 T.Ono mod 2014改善開発 :の位置調整
                ExcelC.pCellVal(1, "colspan=10") = "ＪＡ名　　　 ：" & jaName4FileHead
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '4行目
                ExcelC.pCellStyle(1) = "border-style:none"
                'ExcelC.pCellVal(1, "colspan=10") = "県　　　名　 ：" & kenName '2015/03/06 T.Ono mod 2014改善開発 :の位置調整
                ExcelC.pCellVal(1, "colspan=10") = "県　　　名　 ：" & kenName
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '5行目
                ExcelC.pCellStyle(1) = "border-style:none"
                If pstrSEND_KBN = "1" Then '1:販売店／2:クライアント
                    'ExcelC.pCellVal(1, "colspan=10") = "供給ｾﾝﾀｰ名   ：" & centerName '2015/03/06 T.Ono mod 2014改善開発 :の位置調整
                    ExcelC.pCellVal(1, "colspan=10") = "供給ｾﾝﾀｰ名　 ：" & centerName
                Else
                    'ExcelC.pCellVal(1, "colspan=10") = "ｸﾗｲｱﾝﾄ名     ：" & centerName '2015/03/06 T.Ono mod 2014改善開発 :の位置調整
                    ExcelC.pCellVal(1, "colspan=10") = "ｸﾗｲｱﾝﾄ名　　 ：" & centerName
                End If
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '6行目
                ExcelC.pCellStyle(1) = "border-style:none"
                ExcelC.pCellVal(1, "colspan=10 align=right") = "発行者TEL：" & jalpTel
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '7行目
                ExcelC.pCellStyle(1) = "border-style:none"
                ExcelC.pCellVal(1, "colspan=10 align=right") = "発行者：" & pstrSEND_JALP_NAME   '//�開A-LPｶﾞｽ情報ｾﾝﾀｰ
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '8行目
                ExcelC.pCellStyle(1) = "border-style:none"
                ExcelC.pCellVal(1, "colspan=10 align=right") = pstrSEND_CENT_NAME   '//ＬＰガス集中センター
                ExcelC.mWriteLine("")   '行をファイルに書き込む
                '9行目
                ExcelC.pCellStyle(1) = "border-style:none"
                ExcelC.pCellVal(1, "colspan=6") = ""
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '10行目
                ExcelC.pCellStyle(1) = "border-style:none"
                ExcelC.pCellVal(1, "colspan=6") = "[" & taisyoDate & "] 対応０件"
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                'ログ出力用にデータセット　2013/09/25 T.Watabe add
                wkstrTAIOU_SYONO = ""
                wkstrTAIOU_KURACD = ""
                wkstrTAIOU_JACD = ""
                wkstrTAIOU_ACBCD = ""
                wkstrTAIOU_USER_CD = ""
                wkstrAUTO_ZERO_FLG = "1" ' 【ゼロ件送信フラグ】 0:データあり/1:ゼロ件！(データなし)
                intSEQNO = intSEQNO + 1

                'Throw New Exception("test")
                mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：6（ゼロ件）") '2014/04/11 T.Ono add ログ強化
                '自動FAX専用ログ出力　2013/09/25 T.Watabe add
                Call mInsertS05AutofaxLog(cdb,
                                        strEXEC_YMD,
                                        strEXEC_TIME,
                                        pstrSEND_KBN,
                                        strGUID,
                                        intSEQNO,
                                        pstrTAISYOUBI,
                                        "1",
                                        pstrKANSI_CODE,
                                        wkstrTAIOU_SYONO,
                                        wkstrTAIOU_KURACD,
                                        wkstrTAIOU_JACD,
                                        wkstrTAIOU_ACBCD,
                                        wkstrTAIOU_USER_CD,
                                        pstrAUTO,
                                        pstrFAXNO,
                                        pstrMAILAD,
                                        wkstrAUTO_ZERO_FLG,
                                        pstrWHERE_TAIOU,
                                        "" & pintDebugSQLNo,
                                        ""
                                        )

                '▲ゼロ件の場合、終わり
            Else
                '２　データありの場合

                If pintDebugSQLNo = 107 Then ' DEBUG
                    Return "DEBUG:(" & pintDebugSQLNo & ")[" & strSQL.ToString & "]"
                End If

                '//データローにデータを格納
                dr = ds.Tables(0).Rows(0)

                '2006/06/15 NEC ADD START
                Dim drKansimas As DataRow
                If True Then
                    Dim dsKansimas As New DataSet
                    Dim strSQL_Kansimas As New StringBuilder("")

                    strSQL_Kansimas.Append("SELECT TEL ")        '電話番号
                    strSQL_Kansimas.Append("FROM KANSIMAS ")
                    strSQL_Kansimas.Append("WHERE KANSI_CD = :KANSI_CD ")

                    cdb.pSQL = strSQL_Kansimas.ToString '//SQLセット
                    cdb.pSQLParamStr("KANSI_CD") = pstrKANSI_CODE '//パラメータセット
                    cdb.mExecQuery()  '//SQL実行
                    dsKansimas = cdb.pResult  '//データセットに格納

                    If dsKansimas.Tables(0).Rows.Count = 0 Then '//データが存在しない？

                        strSQL_Kansimas.Remove(0, strSQL_Kansimas.Length)
                        strSQL_Kansimas.Append("SELECT '' AS TEL ")      '電話番号
                        strSQL_Kansimas.Append("FROM DUAL ")

                        cdb.pSQL = strSQL_Kansimas.ToString  '//SQLセット
                        cdb.mExecQuery()  '//SQL実行
                        dsKansimas = cdb.pResult  '//データセットに格納
                        drKansimas = dsKansimas.Tables(0).Rows(0) '//データを格納
                    Else
                        drKansimas = dsKansimas.Tables(0).Rows(0) '//データを格納
                    End If
                End If
                '2006/06/15 NEC ADD END

                If pintDebugSQLNo = 108 Then ' DEBUG
                    Return "DEBUG:(" & pintDebugSQLNo & ")[" & strSQL.ToString & "]"
                End If
                '//------------------------------------------------
                '// ファイルの作成２（データ設定）
                '//------------------------------------------------

                'ヘッダ項目をセット。（1ページあたり行数、出力行数、先頭行から何行目までが行タイトルか）
                'ExcelC.mHeader(intGYOSU, intGYOSU, 1)
                ExcelC.mHeader(intGYOSU, ds.Tables(0).Rows.Count, 1)

                '各列の幅をピクセルでセット。枠線も消す。
                '1行目
                ExcelC.pCellStyle(1) = "width:32px;border-style:none"
                ExcelC.pCellStyle(2) = "width:122px;border-style:none"
                ExcelC.pCellStyle(3) = "width:72px;border-style:none"
                ExcelC.pCellStyle(4) = "width:72px;border-style:none"

                'ExcelC.pCellStyle(5) = "width:72px;border-style:none"  '20050803 NEC UPDATE
                ExcelC.pCellStyle(5) = "width:90px;border-style:none"
                'ExcelC.pCellStyle(6) = "width:115px;border-style:none" ' 2010/10/07
                'ExcelC.pCellStyle(7) = "width:72px;border-style:none" ' 2010/10/07
                ExcelC.pCellStyle(6) = "width:67px;border-style:none"
                ExcelC.pCellStyle(7) = "width:120px;border-style:none"
                ExcelC.pCellStyle(8) = "width:52px;border-style:none"
                ExcelC.pCellStyle(9) = "width:80px;border-style:none"
                'ExcelC.pCellStyle(10) = "width:80px;border-style:none"'20050803 NEC UPDATE
                'ExcelC.pCellStyle(10) = "width:62px;border-style:none" ' 2010/09/17 T.Watabe edit
                ExcelC.pCellStyle(10) = "width:3px;border-style:none"
                ExcelC.pCellStyle(11) = "width:3px;border-style:none" ' 2010/09/17 T.Watabe add
                ExcelC.pCellStyle(12) = "width:3px;border-style:none" ' 2010/09/17 T.Watabe add
                ExcelC.pCellVal(1) = ""
                ExcelC.pCellVal(2) = ""
                ExcelC.pCellVal(3) = ""
                ExcelC.pCellVal(4) = ""
                ExcelC.pCellVal(5) = ""
                ExcelC.pCellVal(6) = ""
                ExcelC.pCellVal(7) = ""
                ExcelC.pCellVal(8) = ""
                ExcelC.pCellVal(9) = ""
                ExcelC.pCellVal(10) = ""
                ExcelC.pCellVal(11) = "" ' 2010/09/17 T.Watabe add
                ExcelC.pCellVal(12) = "" ' 2010/09/17 T.Watabe add
                ExcelC.mWriteLine("")   '行をファイルに書き込む

                '--------------------------------------------------
                'データの出力
                '--------------------------------------------------
                Dim intRowCount As Integer
                Dim strSTD_CD As String
                Dim strTFKICD As String         '--- 2005/07/12 ADD Falcon ---

                'ループする
                For Each dr In ds.Tables(0).Rows
                    faxUser = ""
                    faxUser = pstrFAXNO & "_" & Convert.ToString(dr.Item("ACBNM")) & "_" & Convert.ToString(dr.Item("KENNM")) & "_" & Convert.ToString(dr.Item("ACBCD")) & Convert.ToString(dr.Item("USER_CD"))


                    sZipFilePass = Convert.ToString(dr.Item("YOBI5")) ' 2008/12/12 T.Watabe add

                    If cliCd4FileHead.Length <= 0 Then '空？→最初？
                        cliCd4FileHead = Convert.ToString(dr.Item("CLI_CD")) '2011/03/10 T.Watabe add
                    End If
                    If jaName4FileHead.Length <= 0 Then '空？→最初？
                        jaName4FileHead = Convert.ToString(dr.Item("JANM")).Replace(" ", "") 'ＪＡ名 2011/05/19 T.Watabe add
                    End If
                    If centerName.Length <= 0 Then '空？→最初？
                        centerName = Convert.ToString(dr.Item("CLI_NAME")).Replace(" ", "") 'クライアント名'2011/05/27 T.Watabe add
                    End If


                    '2006/06/14 NEC UPDATE START
                    '1行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    If pstrAUTO = "1" Then
                        'ExcelC.pCellVal(1, "colspan=6") = "送信先FAX番号：" & Convert.ToString(dr.Item("AUTO_FAX")) ' 2010/05/24 T.Watabe edit
                        ExcelC.pCellVal(1, "colspan=6") = "送信先FAX番号：" & pstrFAXNO
                    Else
                        ExcelC.pCellVal(1, "colspan=6") = ""
                    End If
                    ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '2006/06/14 NEC UPDATE END

                    '2行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellStyle(2) = "border-style:none"
                    'JA名が長いとき改行される
                    'ExcelC.pCellVal(1, "colspan=3") = "ＪＡ名：" & Convert.ToString(dr.Item("JANM")) '2005/12/22 NEC UPDATE
                    'ExcelC.pCellVal(2, "colspan=7") = "支所名：" & Convert.ToString(dr.Item("ACBNM")) & "　御中" '2005/12/22 NEC UPDATE
                    'ExcelC.pCellVal(1, "colspan=4") = "ＪＡ名：" & Convert.ToString(dr.Item("JANM"))  '2006/06/14 NEC UPDATE 
                    'ExcelC.pCellVal(2, "colspan=6") = "支所名：" & Convert.ToString(dr.Item("ACBNM")) & "　御中"  '2006/06/14 NEC UPDATE 
                    ExcelC.pCellVal(1, "colspan=10") = "ＪＡ支所名　 ：" & Convert.ToString(dr.Item("ACBNM")) & "　御中"
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '3行目
                    '2020/11/01 T.Ono mod 2020監視改善 Start
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    ''ExcelC.pCellStyle(2) = "border-style:none"
                    ''ExcelC.pCellStyle(3) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=10") = "県　　　名　 ：" & Convert.ToString(dr.Item("KENNM"))
                    ''ExcelC.pCellVal(2, "colspan=4") = "供給センター名：" & Convert.ToString(dr.Item("NAME"))
                    ''2006/06/14 NEC UPDATE START
                    ''If pstrAUTO = "1" Then
                    ''    ExcelC.pCellVal(3, "colspan=4 align=right") = "FAX番号：" & Convert.ToString(dr.Item("AUTO_FAX"))
                    ''Else
                    ''    '''''ExcelC.pCellVal(3, "colspan=4 align=right") = "ﾒｰﾙｱﾄﾞﾚｽ：" & Convert.ToString(dr.Item("AUTO_MAIL"))
                    ''    ExcelC.pCellVal(3, "colspan=4 align=right") = ""
                    ''End If
                    ''2006/06/14 NEC UPDATE END
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=6") = "県　　　名　 ：" & Convert.ToString(dr.Item("KENNM"))
                    ExcelC.pCellVal(2, "colspan=4 align=right") = "発行者TEL：" & Convert.ToString(drKansimas.Item("TEL"))
                    '2020/11/01 T.Ono mod 2020監視改善 End
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '4行目
                    '2006/06/14 NEC UPDATE START
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=6") = "供給ｾﾝﾀｰ名　 ：" & Convert.ToString(dr.Item("NAME"))
                    'ExcelC.pCellVal(2, "colspan=4 align=right") = "発行者TEL：" & Convert.ToString(drKansimas.Item("TEL"))  '2020/11/01 T.Ono mod 2020監視改善
                    ExcelC.pCellVal(2, "colspan=4 align=right") = "発行者：" & pstrSEND_JALP_NAME   '//�開A-LPｶﾞｽ情報ｾﾝﾀｰ
                    '2006/06/14 NEC UPDATE END
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '2020/11/01 T.Ono del 2020監視改善 Start
                    ''5行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    ''2006/06/14 NEC UPDATE START
                    ''ExcelC.pCellVal(1, "colspan=10 align=right") = pstrSEND_JALP_NAME   '//�開A-LPｶﾞｽ情報ｾﾝﾀｰ
                    'ExcelC.pCellVal(1, "colspan=10 align=right") = "発行者：" & pstrSEND_JALP_NAME   '//�開A-LPｶﾞｽ情報ｾﾝﾀｰ
                    ''2006/06/14 NEC UPDATE END
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '2020/11/01 T.Ono del 2020監視改善 End

                    '6行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=10 align=right") = pstrSEND_CENT_NAME   '//ＬＰガス集中センター
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '2020/11/01 T.Ono del 2020監視改善 Start
                    ''7行目
                    'ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none"
                    'ExcelC.pCellVal(1, "colspan=10") = ""
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '2020/11/01 T.Ono del 2020監視改善 End

                    '8行目
                    'ExcelC.pCellStyle(1) = "border-style:none"    '2020/11/01 T.Ono mod 2020監視改善
                    ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none"
                    ExcelC.pCellVal(1, "colspan=10") = "<<受信情報>>"
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '9行目
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む    '--- 2005/05/19 DEL Falcon ---

                    '10行目
                    ExcelC.pCellStyle(1) = "border-style:none;height:36px;vertical-align:top"
                    ExcelC.pCellStyle(2) = "border-style:none;height:36px;vertical-align:top"
                    'ExcelC.pCellVal(1, "colspan=6") = "お客様氏名：" & Convert.ToString(dr.Item("JUSYONM")) '2015/03/06 T.Ono mod 2014改善開発 No16
                    ExcelC.pCellVal(1, "colspan=6") = "お客様名　：" & Convert.ToString(dr.Item("JUSYONM"))
                    ExcelC.pCellVal(2, "colspan=4") = "お客様ｺｰﾄﾞ：" & Convert.ToString(dr.Item("ACBCD")) & Convert.ToString(dr.Item("USER_CD"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '11行目
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '12行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    '2006/06/14 NEC UPDATE START
                    'ExcelC.pCellVal(1, "colspan=4") = "電話番号：" & Convert.ToString(dr.Item("KTELNO"))
                    If Convert.ToString(dr.Item("JUTEL1")) = "" Or Convert.ToString(dr.Item("JUTEL2")) = "" Then
                        'ExcelC.pCellVal(1, "colspan=6") = "電話番号　：" & Convert.ToString(dr.Item("JUTEL1")) & Convert.ToString(dr.Item("JUTEL2")) '2015/03/06 T.Ono mod 2014改善開発 No16
                        ExcelC.pCellVal(1, "colspan=6") = "結線番号　：" & Convert.ToString(dr.Item("JUTEL1")) & Convert.ToString(dr.Item("JUTEL2"))
                    Else
                        'ExcelC.pCellVal(1, "colspan=6") = "電話番号　：" & Convert.ToString(dr.Item("JUTEL1")) & "-" & Convert.ToString(dr.Item("JUTEL2"))
                        ExcelC.pCellVal(1, "colspan=6") = "結線番号　：" & Convert.ToString(dr.Item("JUTEL1")) & "-" & Convert.ToString(dr.Item("JUTEL2")) '2015/03/06 T.Ono mod 2014改善開発 No16
                    End If
                    '2006/06/14 NEC UPDATE END
                    'ExcelC.pCellVal(2, "colspan=4") = "連絡電話番号：" & Convert.ToString(dr.Item("RENTEL")) '2015/03/06 T.Ono mod 2014改善開発 No16
                    ExcelC.pCellVal(2, "colspan=4") = "連絡先　　：" & Convert.ToString(dr.Item("RENTEL"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '13行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=10") = "住所　　　：" & Convert.ToString(dr.Item("ADDR"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '2020/11/01 T.Ono del 2020監視改善
                    ''14行目
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '15行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    ExcelC.pCellStyle(3) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=3") = "取引中止日：" & fncDateSet(Convert.ToString(dr.Item("GAS_START")))
                    ExcelC.pCellVal(2, "colspan=3") = "取引廃止日　　：" & fncDateSet(Convert.ToString(dr.Item("GAS_DELE")))
                    ExcelC.pCellVal(3, "colspan=4") = "地図番号　：" & Convert.ToString(dr.Item("TIZUNO"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '16行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=3") = "集合区分　：" & Convert.ToString(dr.Item("SHUGOUNM"))
                    '2006/06/14 NEC UPDATE START
                    'If Convert.ToString(dr.Item("NCU_SET")) = "3" Then      '3:未接続
                    '    ExcelC.pCellVal(2, "colspan=7") = "ＮＣＵ設置区分：" & "無"
                    'Else
                    '    ExcelC.pCellVal(2, "colspan=7") = "ＮＣＵ設置区分：" & "有"
                    'End If
                    If Convert.ToString(dr.Item("NCU_SET")) = "3" Then      '3:未接続
                        ExcelC.pCellVal(2, "colspan=3") = "ＮＣＵ設置区分：" & "無"
                    Else
                        ExcelC.pCellVal(2, "colspan=3") = "ＮＣＵ設置区分：" & "有"
                    End If
                    ExcelC.pCellStyle(3) = "border-style:none"
                    'Select Case Convert.ToString(dr.Item("USER_FLG")) 2017/11/17 H.Mori mod 2017改善開発 No9-1 
                    Select Case Convert.ToString(dr.Item("UNYO"))
                        Case "0"
                            '0:未開通
                            ExcelC.pCellVal(3, "colspan=4") = "お客様状態：" & "未開通"
                        Case "1"
                            '1:運用中
                            ExcelC.pCellVal(3, "colspan=4") = "お客様状態：" & "運用中"
                        Case "2"
                            '2:休止中
                            ExcelC.pCellVal(3, "colspan=4") = "お客様状態：" & "休止中"
                        Case Else
                            'その他
                            ExcelC.pCellVal(3, "colspan=4") = "お客様状態："
                    End Select
                    '2006/06/14 NEC UPDATE END
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '2020/11/01 T.Ono del 2020監視改善
                    ''17行目
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '18行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=10") = "【警報内容】"
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '19行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    ExcelC.pCellStyle(3) = "border-style:none"
                    'ExcelC.pCellStyle(4) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=3") = "発生日：" & fncDateSet(Convert.ToString(dr.Item("HATYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("HATTIME"))) '2015/03/06 T.Ono mod 2014改善開発 No16
                    ExcelC.pCellVal(1, "colspan=3") = "受信日時：" & fncDateSet(Convert.ToString(dr.Item("HATYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("HATTIME")))
                    ExcelC.pCellVal(2, "colspan=2") = "メータ値：" & Convert.ToString(dr.Item("KENSIN"))
                    'ExcelC.pCellVal(3, "colspan=1") = "流量区分：" & Convert.ToString(dr.Item("RYURYO"))    '名称ではない
                    'ExcelC.pCellVal(4, "colspan=4") = "メータ種別：" & Convert.ToString(dr.Item("METASYU"))
                    ExcelC.pCellVal(3, "colspan=5") = "流量区分：" & Convert.ToString(dr.Item("RYURYO")) & "　メータ種別：" & Convert.ToString(dr.Item("METASYU"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '20行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=5") = "1：" & Convert.ToString(dr.Item("KMNM1"))
                    '2006/06/14 NEC UPDATE START
                    'ExcelC.pCellVal(2, "colspan=5") = "2：" & Convert.ToString(dr.Item("KMNM2"))
                    ExcelC.pCellVal(2, "colspan=5") = "4：" & Convert.ToString(dr.Item("KMNM4"))
                    '2006/06/14 NEC UPDATE END
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '21行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    '2006/06/14 NEC UPDATE START
                    'ExcelC.pCellVal(1, "colspan=5") = "3：" & Convert.ToString(dr.Item("KMNM3"))
                    'ExcelC.pCellVal(2, "colspan=5") = "4：" & Convert.ToString(dr.Item("KMNM4"))
                    ExcelC.pCellVal(1, "colspan=5") = "2：" & Convert.ToString(dr.Item("KMNM2"))
                    ExcelC.pCellVal(2, "colspan=5") = "5：" & Convert.ToString(dr.Item("KMNM5"))
                    '2006/06/14 NEC UPDATE END
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '22行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    '2006/06/14 NEC UPDATE START
                    'ExcelC.pCellVal(1, "colspan=5") = "5：" & Convert.ToString(dr.Item("KMNM5"))
                    ExcelC.pCellVal(1, "colspan=5") = "3：" & Convert.ToString(dr.Item("KMNM3"))
                    '2006/06/14 NEC UPDATE END
                    ExcelC.pCellVal(2, "colspan=5") = "6：" & Convert.ToString(dr.Item("KMNM6"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '23行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    '2005/10/03 NEC UPDATE START
                    '共有マスタに顧客がいなかったら表示する
                    '                If Convert.ToString(dr.Item("MITOKBN")) = "1" Then
                    If Convert.ToString(dr.Item("SH_USER")) = "" Then
                        '2005/10/03 NEC UPDATE END
                        ExcelC.pCellVal(1, "colspan=10") = "お客様マスター　氏名、住所、電話番号をご確認の上、ご連絡ください。"
                    Else
                        ExcelC.pCellVal(1, "colspan=10") = ""
                    End If
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '2020/11/01 T.Ono del 2020監視改善 Start
                    ''2013/11/26 T.Ono dell
                    ''24行目
                    ''ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none"
                    'ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none;height:6px"
                    'ExcelC.pCellVal(1, "colspan=10") = ""
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '2020/11/01 T.Ono del 2020監視改善 End

                    '25行目
                    '2020/11/01 T.Ono mod 2020監視改善 Start
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    ''ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none"
                    ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none"
                    '2020/11/01 T.Ono mod 2020監視改善 End
                    ExcelC.pCellVal(1, "colspan=10") = "<<監視センター対応内容>>"
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '26行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    ExcelC.pCellStyle(3) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=3") = "対応区分　　：" & Convert.ToString(dr.Item("TAIOKBN_NAI"))
                    ExcelC.pCellVal(2, "colspan=2") = "処理区分：" & Convert.ToString(dr.Item("TMSKB_NAI"))
                    ExcelC.pCellVal(3, "colspan=5") = "処理番号(照会用)：" & Convert.ToString(dr.Item("SYONO"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '27行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellStyle(2) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=5") = "監視ｾﾝﾀｰ担当：" & Convert.ToString(dr.Item("TKTANCD_NM"))
                    'ExcelC.pCellVal(2, "colspan=5") = "対応日：" & fncDateSet(Convert.ToString(dr.Item("SYOYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SYOTIME"))) ' 2008/10/14 T.Watabe edit
                    'ExcelC.pCellVal(2, "colspan=5") = "完了日時　　　　：" & fncDateSet(Convert.ToString(dr.Item("SYOYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SYOTIME"))) '2015/03/06 T.Ono mod 2014改善開発 No16
                    ExcelC.pCellVal(2, "colspan=5") = "対応完了日時　　：" & fncDateSet(Convert.ToString(dr.Item("SYOYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SYOTIME")))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '28行目
                    '2015/03/06 T.Ono mod 2014改善開発 No16 START
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=5") = "依頼日時　　：" & fncDateSet(Convert.ToString(dr.Item("SIJIYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SIJITIME")))
                    '2015/03/06 T.Ono mod 2014改善開発 No16 END
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '29行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=10") = "連絡相手　　：" & Convert.ToString(dr.Item("TAITNM"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '30行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=10") = "電話連絡内容：" & Convert.ToString(dr.Item("TELRNM"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '--- ↓2005/05/17 MOD Falcon↓ ---
                    '出力順を電話メモ１⇒電話メモ２⇒復帰操作メモに修正
                    ''31行目
                    ''ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellStyle(1) = "border-style:none;height:36px"  '--- 2005/05/19 MOD Falcon ---
                    'ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("TEL_MEMO1"))
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''32行目
                    'If Convert.ToString(dr.Item("TEL_MEMO2")).Trim.Length > 0 Then ' 2010/10/06 T.Watabe edit ﾃﾞｰﾀが無ければ詰める
                    '    'ExcelC.pCellStyle(1) = "border-style:none"
                    '    ExcelC.pCellStyle(1) = "border-style:none;height:36px"  '--- 2005/05/19 MOD Falcon ---
                    '    ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("TEL_MEMO2"))
                    '    ExcelC.mWriteLine("")   '行をファイルに書き込む
                    'End If

                    ''33行目
                    'If Convert.ToString(dr.Item("FUK_MEMO")).Trim.Length > 0 Then ' 2010/10/06 T.Watabe edit ﾃﾞｰﾀが無ければ詰める
                    '    'ExcelC.pCellStyle(1) = "border-style:none"
                    '    ExcelC.pCellStyle(1) = "border-style:none;height:36px"  '--- 2005/05/19 MOD Falcon ---
                    '    ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("FUK_MEMO"))
                    '    ExcelC.mWriteLine("")   '行をファイルに書き込む
                    'End If
                    '31行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellStyle(2) = "border-style:none;height:64px;vertical-align:top" '2013/11/26 T.Ono mod
                    'ExcelC.pCellStyle(2) = "border-style:none;height:74px;vertical-align:top" '2020/11/01 T.Ono mod 2020監視改善
                    ExcelC.pCellStyle(2) = "border-style:none;height:148px;vertical-align:top"
                    ExcelC.pCellVal(1) = ""
                    'ExcelC.pCellVal(2, "colspan=9") = Convert.ToString(dr.Item("TEL_MEMO1")) & Convert.ToString(dr.Item("TEL_MEMO2")) & Convert.ToString(dr.Item("FUK_MEMO"))    '2020/11/01 T.Ono mod 2020監視改善
                    ExcelC.pCellVal(2, "colspan=9") = Convert.ToString(dr.Item("TEL_MEMO1")) & Convert.ToString(dr.Item("TEL_MEMO2")) & Convert.ToString(dr.Item("FUK_MEMO")) &
                                                      Convert.ToString(dr.Item("TEL_MEMO4")) & Convert.ToString(dr.Item("TEL_MEMO5")) & Convert.ToString(dr.Item("TEL_MEMO6"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '--- ↑2005/05/17 MOD Falcon↑ ---

                    '34行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=10") = "原因器具名　：" & Convert.ToString(dr.Item("TKIGNM")) '2015/03/06 T.Ono mod 2014改善開発 No16
                    ExcelC.pCellVal(1, "colspan=10") = "原因器具　　：" & Convert.ToString(dr.Item("TKIGNM"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '35行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    ExcelC.pCellVal(1, "colspan=10") = "作動原因　　：" & Convert.ToString(dr.Item("TSADNM"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '36行目
                    '2006/06/14 NEC UPDATE START
                    ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=10") = "出動指示　　：" & Convert.ToString(dr.Item("SDNM")) '2015/03/06 T.Ono mod 2014改善開発 No16
                    '2020/11/01 T.Ono mod 2020監視改善 Start
                    'ExcelC.pCellVal(1, "colspan=10") = "出動依頼内容：" & Convert.ToString(dr.Item("SDNM"))
                    If pstrSD_PRT_FLG = "1" Then
                        ExcelC.pCellVal(1, "colspan=10") = "出動依頼内容：" & Convert.ToString(dr.Item("SDNM"))
                    Else
                        ExcelC.pCellVal(1, "colspan=10") = "出動依頼内容："
                    End If
                    '2020/11/01 T.Ono mod 2020監視改善 End
                    '2006/06/14 NEC UPDATE END
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '37行目
                    ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=10") = "出動指示備考：" & Convert.ToString(dr.Item("SIJI_BIKO1")) '2015/03/06 T.Ono mod 2014改善開発 No16
                    '2020/11/01 T.Ono mod 2020監視改善 Start
                    'ExcelC.pCellVal(1, "colspan=10") = "出動依頼備考：" & Convert.ToString(dr.Item("SIJI_BIKO1"))
                    If pstrSD_PRT_FLG = "1" Then
                        ExcelC.pCellVal(1, "colspan=10") = "出動依頼備考：" & Convert.ToString(dr.Item("SIJI_BIKO1"))
                    Else
                        ExcelC.pCellVal(1, "colspan=10") = "出動依頼備考："
                    End If
                    '2020/11/01 T.Ono mod 2020監視改善 End
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '38行目
                    'ExcelC.pCellStyle(1) = "border-style:none"    '2021/01/05 T.Ono mod 2020監視改善 siji_biko2自体、使用していなさそう
                    ExcelC.pCellStyle(1) = "border-top-style:none;border-left-style:none;border-right-style:none"
                    ExcelC.pCellVal(1, "colspan=10") = "　　　　　　　" & Convert.ToString(dr.Item("siji_biko2"))
                    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '39行目
                    '2006/06/15 NEC UPDATE START
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '2006/06/15 NEC UPDATE END

                    '2020/11/01 T.Ono del 2020監視改善 Start
                    ''2013/11/26 T.Ono del
                    ''40行目
                    ''ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none"
                    'ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none;height:6px"
                    'ExcelC.pCellVal(1, "colspan=10") = ""
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '2020/11/01 T.Ono del 2020監視改善 End

                    '--- ↓2005/05/25 ADD Falcon↓ ---
                    '--- ↓2005/05/23 ADD Falcon↓ ---
                    strSTD_CD = Convert.ToString(dr.Item("STD_CD"))
                    '--- ↑2005/05/23 ADD Falcon↑ ---

                    '--- ↓2005/07/13 ADD Falcon↓ ---
                    strTFKICD = Convert.ToString(dr.Item("TFKICD"))         '復帰対応状況
                    '--- ↑2005/07/13 ADD Falcon↑ ---

                    '--- ↓2005/05/31 MOD Falcon↓ ---
                    'If strSTD_CD.Length <> 0 And Convert.ToString(dr.Item("TAIOKBN")) = "2" Then                       '--- 2005/07/13 DEL Falcon
                    If strSTD_CD.Length <> 0 And Convert.ToString(dr.Item("TAIOKBN")) = "2" And strTFKICD = "8" Then    '--- 2005/07/13 MOD Falcon
                        '---------- 出動指示の場合のみデータ出力する ---------------------------
                        '41行目
                        '2020/11/01 T.Ono mod 2020監視改善 Start
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellStyle(2) = "border-style:none" ' 2008/10/21 T.Watabe add
                        ExcelC.pCellStyle(1) = "border-bottom-style:none;border-left-style:none;border-right-style:none"
                        ExcelC.pCellStyle(2) = "border-bottom-style:none;border-left-style:none;border-right-style:none"
                        '2020/11/01 T.Ono mod 2020監視改善 End
                        'ExcelC.pCellVal(1, "colspan=10") = "<<出動会社対応内容>>"
                        ExcelC.pCellVal(1, "colspan=6") = "<<出動会社対応内容>>"
                        ExcelC.pCellVal(2, "colspan=4 align=right") = "処理区分：" & Convert.ToString(dr.Item("SDSKBN_NAI")) ' 2008/10/21 T.Watabe add
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '42行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellVal(1, "colspan=10") = "出動委託先（指定保安機関）：" & Convert.ToString(dr.Item("STD"))
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '43行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellVal(1, "colspan=10") = "支所･拠点名：" & Convert.ToString(dr.Item("STD_KYOTEN"))
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '44行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        ExcelC.pCellStyle(3) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=3") = "対応者　　 ：" & Convert.ToString(dr.Item("SYUTDTNM")) '2015/03/06 T.Ono mod 2014改善開発 No16
                        ExcelC.pCellVal(1, "colspan=3") = "出動対応者 ：" & Convert.ToString(dr.Item("SYUTDTNM"))
                        'ExcelC.pCellVal(2, "colspan=3") = "到着時間：" & fncDateSet(Convert.ToString(dr.Item("TYAKYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("TYAKTIME"))) ' 2008/10/14 T.Watabe edit
                        'ExcelC.pCellVal(3, "colspan=4") = "措置完了時間：" & fncDateSet(Convert.ToString(dr.Item("SYOKANYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SYOKANTIME"))) ' 2008/10/14 T.Watabe edit
                        ExcelC.pCellVal(2, "colspan=3") = "受信日時：" & fncDateSet(Convert.ToString(dr.Item("SIJIYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SIJITIME"))) '受信日時(出動指示日＆時刻)
                        ExcelC.pCellVal(3, "colspan=4") = "出動日時　　：" & fncDateSet(Convert.ToString(dr.Item("SDYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SDTIME")))
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '45行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none" ' 2008/10/14 T.Watabe add
                        ExcelC.pCellStyle(3) = "border-style:none" ' 2008/10/14 T.Watabe add
                        'ExcelC.pCellVal(1, "colspan=10") = "対応相手：" & Convert.ToString(dr.Item("AITNM")) ' 2008/10/14 T.Watabe edit
                        ExcelC.pCellVal(1, "colspan=3") = "対応相手　 ：" & Convert.ToString(dr.Item("AITNM"))
                        ExcelC.pCellVal(2, "colspan=3") = "到着日時：" & fncDateSet(Convert.ToString(dr.Item("TYAKYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("TYAKTIME"))) ' 2008/10/14 T.Watabe add
                        'ExcelC.pCellVal(3, "colspan=4") = "措置完了日時：" & fncDateSet(Convert.ToString(dr.Item("SYOKANYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SYOKANTIME"))) ' 2008/10/14 T.Watabe add '2015/03/06 T.Ono mod 2014改善開発 No16
                        ExcelC.pCellVal(3, "colspan=4") = "処理完了日時：" & fncDateSet(Convert.ToString(dr.Item("SYOKANYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SYOKANTIME")))
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ' 2008/11/04 T.Watabe del 
                        ''46行目
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '47行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        '2006/06/14 NEC UPDATE START
                        'If Convert.ToString(dr.Item("CSNTNGAS")) = "1" Then
                        '    ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & Convert.ToString(dr.Item("SDTBIK1"))
                        'Else
                        '    If Convert.ToString(dr.Item("METFUKKI")) = "1" Then
                        '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "メータ復帰"
                        '    ElseIf Convert.ToString(dr.Item("HOAN")) = "1" Then
                        '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "保安"
                        '    ElseIf Convert.ToString(dr.Item("GASGIRE")) = "1" Then
                        '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "ガス切れ"
                        '    ElseIf Convert.ToString(dr.Item("KIGKOSYO")) = "1" Then
                        '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "器具故障"
                        '    ElseIf Convert.ToString(dr.Item("CSNTGEN")) = "1" Then
                        '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "その他"
                        '    Else
                        '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & ""
                        '    End If
                        'End If

                        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】"
                        ExcelC.mWriteLine("")   '行をファイルに書き込む
                        '48行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        ExcelC.pCellStyle(3) = "border-style:none"
                        ExcelC.pCellVal(1) = ""
                        If Convert.ToString(dr.Item("METFUKKI")) = "1" Then
                            ExcelC.pCellVal(2, "colspan=3") = "ガス関連：" & "メータ復帰"
                        ElseIf Convert.ToString(dr.Item("HOAN")) = "1" Then
                            ExcelC.pCellVal(2, "colspan=3") = "ガス関連：" & "保安"
                        ElseIf Convert.ToString(dr.Item("GASGIRE")) = "1" Then
                            ExcelC.pCellVal(2, "colspan=3") = "ガス関連：" & "ガス切れ"
                        ElseIf Convert.ToString(dr.Item("KIGKOSYO")) = "1" Then
                            ExcelC.pCellVal(2, "colspan=3") = "ガス関連：" & "器具故障"
                        ElseIf Convert.ToString(dr.Item("CSNTGEN")) = "1" Then
                            ExcelC.pCellVal(2, "colspan=3") = "ガス関連：" & "その他"
                        Else
                            ExcelC.pCellVal(2, "colspan=3") = "ガス関連：" & ""
                        End If
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '49行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        ExcelC.pCellVal(1) = ""
                        ExcelC.pCellVal(2, "colspan=9") = "お話内容：" & Convert.ToString(dr.Item("SDTBIK1"))
                        ExcelC.mWriteLine("")   '行をファイルに書き込む
                        '2006/06/14 NEC UPDATE END

                        '50行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        ExcelC.pCellVal(1, "colspan=5") = "復帰対応：" & Convert.ToString(dr.Item("FKINM"))
                        'If Convert.ToString(dr.Item("KIGTAIYO")) = "1" Then  '1:有 ' 2008/10/21 T.Watabe edit
                        '    ExcelC.pCellVal(2, "colspan=5") = "簡易ガス器具の貸与：" & "有"
                        'Else
                        '    ExcelC.pCellVal(2, "colspan=5") = "簡易ガス器具の貸与：" & "無"
                        'End If
                        ExcelC.pCellVal(2, "colspan=5") = "メータ作動原因１　：" & Convert.ToString(dr.Item("SADNM")) '1:有 ' 2008/10/21 T.Watabe add
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ' ▼ 2008/10/14 T.Watabe add
                        '50行目+1
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=5") = "ガス器具：" & Convert.ToString(dr.Item("KIGNM")) '2015/03/06 T.Ono mod 2014改善開発 No16
                        ExcelC.pCellVal(1, "colspan=5") = "原因器具：" & Convert.ToString(dr.Item("KIGNM"))
                        If Convert.ToString(dr.Item("KIGTAIYO")) = "1" Then  '1:有
                            ExcelC.pCellVal(2, "colspan=5") = "簡易ガス器具の貸与：" & "有"
                        Else
                            ExcelC.pCellVal(2, "colspan=5") = "簡易ガス器具の貸与：" & "無"
                        End If
                        ExcelC.mWriteLine("")   '行をファイルに書き込む
                        ' ▲ 2008/10/14 T.Watabe add

                        '51行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        ExcelC.pCellVal(1, "colspan=5") = "連絡状況 連絡相手：" & Convert.ToString(dr.Item("JAKENREN")) & "　様"
                        ExcelC.pCellVal(2, "colspan=5") = "連絡時間　　　　　：" & fncTimeSet(Convert.ToString(dr.Item("RENTIME")))
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '52行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        If Convert.ToString(dr.Item("GASMUMU")) = "0" Then  ' 0：有　1：無
                            ExcelC.pCellVal(1, "colspan=2") = "ガス漏れ点検：" & "有"
                        Else
                            ExcelC.pCellVal(1, "colspan=2") = "ガス漏れ点検：" & "無"
                        End If
                        If Convert.ToString(dr.Item("ORGENIN")) = "1" Then  'ガス器具　1:有
                            ExcelC.pCellVal(2, "colspan=8") = "原因：" & "ガス器具"
                        Else
                            ExcelC.pCellVal(2, "colspan=8") = "原因：" & ""
                        End If
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '53行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        ExcelC.pCellStyle(3) = "border-style:none"
                        If Convert.ToString(dr.Item("GASGUMU")) = "0" Then      '0：有　1：無
                            ExcelC.pCellVal(1, "colspan=2") = "ガス切れ点検：" & "有"
                        Else
                            ExcelC.pCellVal(1, "colspan=2") = "ガス切れ点検：" & "無"
                        End If
                        If Convert.ToString(dr.Item("HOSKOKAN")) = "0" Then     '0：実施　1：未実施
                            ExcelC.pCellVal(2, "colspan=5") = "ｺﾞﾑﾎｰｽ交換：" & "実施"
                        Else
                            ExcelC.pCellVal(2, "colspan=5") = "ｺﾞﾑﾎｰｽ交換：" & "未実施"
                        End If
                        If Convert.ToString(dr.Item("COYOINA")) = "0" Then      '0：良　1：否
                            ExcelC.pCellVal(3, "colspan=3") = "ＣＯ濃度：" & "良"
                        Else
                            ExcelC.pCellVal(3, "colspan=3") = "ＣＯ濃度：" & "否"
                        End If
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '54行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellStyle(2) = "border-style:none"
                        ExcelC.pCellStyle(3) = "border-style:none"
                        ExcelC.pCellStyle(4) = "border-style:none"
                        If Convert.ToString(dr.Item("METYOINA")) = "0" Then      '0：良　1：否
                            ExcelC.pCellVal(1, "colspan=2") = "メータ点検　：" & "良"
                        Else
                            ExcelC.pCellVal(1, "colspan=2") = "メータ点検　：" & "否"
                        End If
                        '2017/02/17 W.GANEKO 2016監視改善 ��7 START
                        'If Convert.ToString(dr.Item("TYOUYOINA")) = "0" Then      '0：良　1：否
                        '    ExcelC.pCellVal(2, "colspan=3") = "調整期点検：" & "良"
                        'Else
                        '    ExcelC.pCellVal(2, "colspan=3") = "調整期点検：" & "否"
                        'End If
                        If Convert.ToString(dr.Item("TYOUYOINA")) = "0" Then      '0：良　1：否
                            ExcelC.pCellVal(2, "colspan=3") = "調整器点検：" & "良"
                        Else
                            ExcelC.pCellVal(2, "colspan=3") = "調整器点検：" & "否"
                        End If
                        '2017/02/17 W.GANEKO 2016監視改善 ��7 END
                        If Convert.ToString(dr.Item("VALYOINA")) = "0" Then      '0：良　1：否
                            ExcelC.pCellVal(3, "colspan=2") = "容器･中間バルブ：" & "良"
                        Else
                            ExcelC.pCellVal(3, "colspan=2") = "容器･中間バルブ：" & "否"
                        End If
                        '2017/02/17 W.GANEKO 2016監視改善 ��7 START
                        'If Convert.ToString(dr.Item("KYUHAIUMU")) = "0" Then      '0：良　1：否
                        '    ExcelC.pCellVal(4, "colspan=3") = "吸排気口：" & "良"
                        'Else
                        '    ExcelC.pCellVal(4, "colspan=3") = "吸排気口：" & "否"
                        'End If
                        If Convert.ToString(dr.Item("KYUHAIUMU")) = "0" Then      '0：良　1：否
                            ExcelC.pCellVal(4, "colspan=3") = "給排気口：" & "良"
                        Else
                            ExcelC.pCellVal(4, "colspan=3") = "給排気口：" & "否"
                        End If
                        '2017/02/17 W.GANEKO 2016監視改善 ��7 END
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '--- ↓2005/09/09 MOD Falcon↓ ---
                        '2006/06/14 NEC UPDATE START
                        ''55行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "特記事項："
                        ''ExcelC.pCellVal(1, "colspan=10") = "特記事項：" & Convert.ToString(dr.Item("SDTBIK2"))
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''56行目
                        'ExcelC.pCellStyle(1) = "border-style:none;font-size:10pt;height:36px"
                        'ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("SDTBIK2"))
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''57行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "ＪＡ／県連様への依頼事項等："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''58行目
                        'ExcelC.pCellStyle(1) = "border-style:none;font-size:10pt;height:36px"
                        ''ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("SNTTOKKI"))
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        '55行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        ExcelC.pCellVal(1, "colspan=10") = "出動結果内容/報告："
                        ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''56行目
                        'ExcelC.pCellStyle(1) = "border-style:none;height:36px"
                        'ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("SDTBIK2"))
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''57行目
                        'If Convert.ToString(dr.Item("SNTTOKKI")).Trim.Length > 0 Then ' 2010/10/06 T.Watabe edit ﾃﾞｰﾀが無ければ詰める
                        '    ExcelC.pCellStyle(1) = "border-style:none;height:36px"
                        '    ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("SNTTOKKI"))
                        '    ExcelC.mWriteLine("")   '行をファイルに書き込む
                        'End If

                        ''58行目
                        'If Convert.ToString(dr.Item("SDTBIK3")).Trim.Length > 0 Then ' 2010/10/06 T.Watabe edit ﾃﾞｰﾀが無ければ詰める
                        '    ExcelC.pCellStyle(1) = "border-style:none;height:36px"
                        '    ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("SDTBIK3"))
                        '    ExcelC.mWriteLine("")   '行をファイルに書き込む
                        'End If
                        '56行目
                        ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellStyle(2) = "border-style:none;height:64px;vertical-align:top" 2013/11/26 T.Ono mod
                        ExcelC.pCellStyle(2) = "border-style:none;height:74px;vertical-align:top"
                        ExcelC.pCellVal(1) = ""
                        ExcelC.pCellVal(2, "colspan=9") = Convert.ToString(dr.Item("SDTBIK2")) & Convert.ToString(dr.Item("SNTTOKKI")) & Convert.ToString(dr.Item("SDTBIK3"))
                        ExcelC.mWriteLine("")   '行をファイルに書き込む
                        '2006/06/14 NEC UPDATE END
                        '--- ↑2005/09/09 MOD Falcon↑ ---

                    Else
                        '--- ↓2005/09/10 DEL Falcon↓ ---
                        '出動会社が出動した内容ではない場合、「出動情報」欄すべて表示しないようにする

                        ''---------- 出動指示ではない場合、データ出力は行わない ---------------------------
                        ''41行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "<<出動会社対応内容>>"
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''42行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "出動委託先（指定保安機関）："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''43行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "支所・拠点名："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''44行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellStyle(2) = "border-style:none"
                        'ExcelC.pCellStyle(3) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=3") = "対応者："
                        'ExcelC.pCellVal(2, "colspan=3") = "到着時間："
                        'ExcelC.pCellVal(3, "colspan=4") = "措置完了時間："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''45行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "対応相手："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''46行目
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''47行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 "
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''49行目
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''50行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellStyle(2) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=5") = "復帰対応："
                        'ExcelC.pCellVal(2, "colspan=5") = "簡易ガス器具の貸与："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''51行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellStyle(2) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=5") = "連絡状況　連絡相手："
                        'ExcelC.pCellVal(2, "colspan=5") = "連絡時間："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''52行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellStyle(2) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=2") = "ガス漏れ点検："
                        'ExcelC.pCellVal(2, "colspan=8") = "原因："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''53行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellStyle(2) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=2") = "ガス切れ点検："
                        'ExcelC.pCellVal(2, "colspan=8") = "ゴムホース交換："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''54行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellStyle(2) = "border-style:none"
                        'ExcelC.pCellStyle(3) = "border-style:none"
                        'ExcelC.pCellStyle(4) = "border-style:none"
                        'ExcelC.pCellStyle(5) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=2") = "メータ点検："
                        'ExcelC.pCellVal(2, "colspan=2") = "調整期点検："
                        'ExcelC.pCellVal(3, "colspan=2") = "容器・中間バルブ："
                        'ExcelC.pCellVal(4, "colspan=2") = "吸排気口："
                        'ExcelC.pCellVal(5, "colspan=2") = "ＣＯ濃度："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''55行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "特記事項："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''56行目
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''57行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = "ＪＡ／県連様への依頼事項等："
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む

                        ''58行目
                        'ExcelC.pCellStyle(1) = "border-style:none"
                        'ExcelC.pCellVal(1, "colspan=10") = ""
                        'ExcelC.mWriteLine("")   '行をファイルに書き込む
                        '--- ↑2005/09/10 DEL Falcon↑ ---

                    End If

                    '出動対応時のみ出動情報を出力するように修正
                    'If strSTD_CD.Length <> 0 And Convert.ToString(dr.Item("TAIOKBN")) = "2" Then
                    '    '41行目
                    '    ExcelC.pCellStyle(1) = "border-style:none"
                    '    ExcelC.pCellVal(1, "colspan=10") = "<<出動会社対応内容>>"
                    '    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '    '42行目
                    '    '--- ↓2005/05/23 MOD Falcon↓ ---
                    '    ExcelC.pCellStyle(1) = "border-style:none"
                    '    'ExcelC.pCellVal(1, "colspan=10") = "出動委託先（指定保安機関）：" & Convert.ToString(dr.Item("SYUTDTNM"))
                    '    'ExcelC.pCellVal(1, "colspan=10") = "出動委託先（指定保安機関）：" & Convert.ToString(dr.Item("TSTANNM"))
                    '    ExcelC.pCellVal(1, "colspan=10") = "出動委託先（指定保安機関）：" & Convert.ToString(dr.Item("STD"))
                    '    ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '    '--- ↑2005/05/23 MOD Falcon↑ ---

                    '    '43行目
                    '    ExcelC.pCellStyle(1) = "border-style:none"
                    '    ExcelC.pCellVal(1, "colspan=10") = "支所・拠点名：" & Convert.ToString(dr.Item("STD_KYOTEN"))
                    '    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '    '44行目
                    '    ExcelC.pCellStyle(1) = "border-style:none"
                    '    ExcelC.pCellStyle(2) = "border-style:none"
                    '    ExcelC.pCellStyle(3) = "border-style:none"
                    '    '--- ↓2005/05/21 MOD Falcon↓ ---
                    '    'ExcelC.pCellVal(1, "colspan=3") = "対応者：" & Convert.ToString(dr.Item("TSTANNM"))
                    '    ExcelC.pCellVal(1, "colspan=3") = "対応者：" & Convert.ToString(dr.Item("SYUTDTNM"))
                    '    '--- ↑2005/05/21 MOD Falcon↑ ---
                    '    ExcelC.pCellVal(2, "colspan=3") = "到着時間：" & fncDateSet(Convert.ToString(dr.Item("TYAKYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("TYAKTIME")))
                    '    ExcelC.pCellVal(3, "colspan=4") = "措置完了時間：" & fncDateSet(Convert.ToString(dr.Item("SYOKANYMD"))) & " " & fncTimeSet(Convert.ToString(dr.Item("SYOKANTIME")))
                    '    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '    '45行目
                    '    ExcelC.pCellStyle(1) = "border-style:none"
                    '    ExcelC.pCellVal(1, "colspan=10") = "対応相手：" & Convert.ToString(dr.Item("AITNM"))
                    '    ExcelC.mWriteLine("")   '行をファイルに書き込む

                    '    '46行目
                    '    ExcelC.mWriteLine("")   '行をファイルに書き込む
                    'End If

                    ''47行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    ''''''ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】"
                    'If Convert.ToString(dr.Item("CSNTNGAS")) = "1" Then
                    '    ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & Convert.ToString(dr.Item("SDTBIK1"))
                    'Else
                    '    If Convert.ToString(dr.Item("METFUKKI")) = "1" Then
                    '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "メータ復帰"
                    '    ElseIf Convert.ToString(dr.Item("HOAN")) = "1" Then
                    '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "保安"
                    '    ElseIf Convert.ToString(dr.Item("GASGIRE")) = "1" Then
                    '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "ガス切れ"
                    '    ElseIf Convert.ToString(dr.Item("KIGKOSYO")) = "1" Then
                    '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "器具故障"
                    '    ElseIf Convert.ToString(dr.Item("CSNTGEN")) = "1" Then
                    '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & "その他"
                    '    Else
                    '        ExcelC.pCellVal(1, "colspan=10") = "【お客様のお話内容】 " & ""
                    '    End If
                    'End If
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''49行目
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''50行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellStyle(2) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=5") = "復帰対応：" & Convert.ToString(dr.Item("FKINM"))
                    ''--- ↓2005/05/23 MOD Falcon↓ ---
                    'If strSTD_CD.Length = 0 Then
                    '    ExcelC.pCellVal(2, "colspan=5") = "簡易ガス器具の貸与："
                    'Else
                    '    If Convert.ToString(dr.Item("KIGTAIYO")) = "1" Then  '1:有
                    '        ExcelC.pCellVal(2, "colspan=5") = "簡易ガス器具の貸与：" & "有"
                    '    Else
                    '        ExcelC.pCellVal(2, "colspan=5") = "簡易ガス器具の貸与：" & "無"
                    '    End If
                    'End If
                    ''--- ↑2005/05/23 MOD Falcon↑ ---
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''51行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellStyle(2) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=5") = "連絡状況　連絡相手：" & Convert.ToString(dr.Item("JAKENREN")) & "　様"
                    'ExcelC.pCellVal(2, "colspan=5") = "連絡時間：" & fncTimeSet(Convert.ToString(dr.Item("RENTIME")))
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''52行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellStyle(2) = "border-style:none"
                    ''--- ↓2005/05/23 MOD Falcon↓ ---
                    'If strSTD_CD.Length = 0 Then
                    '    ExcelC.pCellVal(1, "colspan=2") = "ガス漏れ点検："
                    '    ExcelC.pCellVal(2, "colspan=8") = "原因："
                    'Else
                    '    If Convert.ToString(dr.Item("GASMUMU")) = "0" Then  ' 0：有　1：無
                    '        ExcelC.pCellVal(1, "colspan=2") = "ガス漏れ点検：" & "有"
                    '    Else
                    '        ExcelC.pCellVal(1, "colspan=2") = "ガス漏れ点検：" & "無"
                    '    End If
                    '    If Convert.ToString(dr.Item("ORGENIN")) = "1" Then  'ガス器具　1:有
                    '        ExcelC.pCellVal(2, "colspan=8") = "原因：" & "ガス器具"
                    '    Else
                    '        ExcelC.pCellVal(2, "colspan=8") = "原因：" & ""
                    '    End If
                    'End If
                    ''--- ↑2005/05/23 MOD Falcon↑ ---
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''53行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellStyle(2) = "border-style:none"
                    ''--- ↓2005/05/23 MOD Falcon↓ ---
                    'If strSTD_CD.Length = 0 Then
                    '    ExcelC.pCellVal(1, "colspan=2") = "ガス切れ点検："
                    '    ExcelC.pCellVal(2, "colspan=8") = "ゴムホース交換："
                    'Else
                    '    If Convert.ToString(dr.Item("GASGUMU")) = "0" Then      '0：有　1：無
                    '        ExcelC.pCellVal(1, "colspan=2") = "ガス切れ点検：" & "有"
                    '    Else
                    '        ExcelC.pCellVal(1, "colspan=2") = "ガス切れ点検：" & "無"
                    '    End If
                    '    If Convert.ToString(dr.Item("HOSKOKAN")) = "0" Then     '0：実施　1：未実施
                    '        'ExcelC.pCellVal(2, "colspan=8") = "ゴムホース交換：" & "未実施"    '--- 2005/05/23 DEL Falcon ---
                    '        ExcelC.pCellVal(2, "colspan=8") = "ゴムホース交換：" & "実施"
                    '    Else
                    '        ExcelC.pCellVal(2, "colspan=8") = "ゴムホース交換：" & "未実施"
                    '    End If
                    'End If
                    ''--- ↑2005/05/23 MOD Falcon↑ ---
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''54行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellStyle(2) = "border-style:none"
                    'ExcelC.pCellStyle(3) = "border-style:none"
                    'ExcelC.pCellStyle(4) = "border-style:none"
                    'ExcelC.pCellStyle(5) = "border-style:none"
                    ''--- ↓2005/05/23 MOD Falcon↓ ---
                    'If strSTD_CD.Length = 0 Then
                    '    ExcelC.pCellVal(1, "colspan=2") = "メータ点検："
                    '    ExcelC.pCellVal(2, "colspan=2") = "調整期点検："
                    '    ExcelC.pCellVal(3, "colspan=2") = "容器・中間バルブ："
                    '    ExcelC.pCellVal(4, "colspan=2") = "吸排気口："
                    '    ExcelC.pCellVal(5, "colspan=2") = "ＣＯ濃度："
                    'Else
                    '    If Convert.ToString(dr.Item("METYOINA")) = "0" Then      '0：良　1：否
                    '        ExcelC.pCellVal(1, "colspan=2") = "メータ点検：" & "良"
                    '    Else
                    '        ExcelC.pCellVal(1, "colspan=2") = "メータ点検：" & "否"
                    '    End If
                    '    If Convert.ToString(dr.Item("TYOUYOINA")) = "0" Then      '0：良　1：否
                    '        ExcelC.pCellVal(2, "colspan=2") = "調整期点検：" & "良"
                    '    Else
                    '        ExcelC.pCellVal(2, "colspan=2") = "調整期点検：" & "否"
                    '    End If
                    '    If Convert.ToString(dr.Item("VALYOINA")) = "0" Then      '0：良　1：否
                    '        ExcelC.pCellVal(3, "colspan=2") = "容器・中間バルブ：" & "良"
                    '    Else
                    '        ExcelC.pCellVal(3, "colspan=2") = "容器・中間バルブ：" & "否"
                    '    End If
                    '    If Convert.ToString(dr.Item("KYUHAIUMU")) = "0" Then      '0：良　1：否
                    '        ExcelC.pCellVal(4, "colspan=2") = "吸排気口：" & "良"
                    '    Else
                    '        ExcelC.pCellVal(4, "colspan=2") = "吸排気口：" & "否"
                    '    End If
                    '    If Convert.ToString(dr.Item("COYOINA")) = "0" Then      '0：良　1：否
                    '        ExcelC.pCellVal(5, "colspan=2") = "ＣＯ濃度：" & "良"
                    '    Else
                    '        ExcelC.pCellVal(5, "colspan=2") = "ＣＯ濃度：" & "否"
                    '    End If
                    'End If
                    ''--- ↑2005/05/23 MOD Falcon↑ ---
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''55行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=10") = "特記事項：" & Convert.ToString(dr.Item("SDTBIK2"))
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''56行目
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''57行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=10") = "ＪＡ／県連様への依頼事項等："
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む

                    ''58行目
                    'ExcelC.pCellStyle(1) = "border-style:none"
                    'ExcelC.pCellVal(1, "colspan=10") = Convert.ToString(dr.Item("SNTTOKKI"))
                    'ExcelC.mWriteLine("")   '行をファイルに書き込む
                    '--- ↑2005/05/31 MOD Falcon↑ ---

                    intRowCount = intRowCount + 1

                    If intRowCount <> ds.Tables(0).Rows.Count Then
                        '改ページ
                        ExcelC.mWriteLine("", True)
                    End If

                    '自動FAX専用ログ出力用にデータセット　2013/09/25 T.Watabe add
                    wkstrTAIOU_SYONO = Convert.ToString(dr.Item("SYONO"))
                    wkstrTAIOU_KURACD = Convert.ToString(dr.Item("CLI_CD"))
                    wkstrTAIOU_JACD = Convert.ToString(dr.Item("JACD"))
                    wkstrTAIOU_ACBCD = Convert.ToString(dr.Item("ACBCD"))
                    wkstrTAIOU_USER_CD = Convert.ToString(dr.Item("ACBCD")) & Convert.ToString(dr.Item("USER_CD"))
                    wkstrAUTO_ZERO_FLG = "0" ' 【ゼロ件送信フラグ】 0:データあり/1:ゼロ件！(データなし)
                    intSEQNO = intSEQNO + 1

                    mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：6（報告あり）SYONO：" & wkstrTAIOU_SYONO) '2014/04/11 T.Ono add ログ強化
                    '自動FAX専用ログ出力　2013/09/25 T.Watabe add
                    Call mInsertS05AutofaxLog(cdb,
                                            strEXEC_YMD,
                                            strEXEC_TIME,
                                            pstrSEND_KBN,
                                            strGUID,
                                            intSEQNO,
                                            pstrTAISYOUBI,
                                            "1",
                                            pstrKANSI_CODE,
                                            wkstrTAIOU_SYONO,
                                            wkstrTAIOU_KURACD,
                                            wkstrTAIOU_JACD,
                                            wkstrTAIOU_ACBCD,
                                            wkstrTAIOU_USER_CD,
                                            pstrAUTO,
                                            pstrFAXNO,
                                            pstrMAILAD,
                                            wkstrAUTO_ZERO_FLG,
                                            pstrWHERE_TAIOU,
                                            "" & pintDebugSQLNo,
                                            ""
                                            )
                Next
            End If

            'ファイルクローズ
            ExcelC.mClose()
            If pintDebugSQLNo = 109 Then ' DEBUG
                Return "DEBUG:(" & pintDebugSQLNo & ")[ExcelC.pDirName=" & ExcelC.pDirName & "]"
            End If
            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：7") '2014/04/11 T.Ono add ログ強化

            '2018/02/19 T.Ono mod START --------------------
            '圧縮方法を変更。UNLHA32.DLLは使わない。
            ''2016/11/29 T.Ono mod 2016改善開発 ��12
            ''If pintDebugSQLNo = 0 Then ' 通常
            'If pintDebugSQLNo = 0 OrElse pintDebugSQLNo = 1 Then ' 通常またはデバッグ1

            '    '圧縮先ファイルのあるフォルダ
            '    compressC.p_Dir = ExcelC.pDirName
            '    '日本語ファイル名の指定(パラメータ[セッション] + 電話番号)
            '    If pstrAUTO = "1" Then '1:fax
            '        'ＦＡＸファイル作成
            '        'compressC.p_NihongoFileName = pstrSESSION & pstrFAXNO & ".xls" '20050506 edit Falcon
            '        'compressC.p_NihongoFileName = pstrSESSION & pstrAUTO & pstrFAXNO & ".xls"
            '        compressC.p_NihongoFileName = pstrSESSION & "[" & pstrAUTO & "][" & pstrFAXNO & "].xls"
            '        '圧縮元ファイル名      (上記作成したEXCELファイル)(LZHに追加する)
            '        compressC.p_FileName = ExcelC.pDirName & ExcelC.pFileName & ".xls"
            '    Else
            '        'メール送信ファイル作成

            '        ' 2008/12/12 T.Watabe add
            '        'ZIPに圧縮
            '        'fncZipTest(ExcelC.pDirName & ExcelC.pFileName & ".xls", ExcelC.pDirName & ExcelC.pFileName & ".zip")
            '        fncMakeZipWithPass(ExcelC.pDirName & ExcelC.pFileName & ".xls", ExcelC.pDirName & ExcelC.pFileName & ".zip", sZipFilePass) ' 2014/06/17 T.Watabe edit
            '        'fncMakeZipWithPass2(ExcelC.pDirName & ExcelC.pFileName & ".xls", ExcelC.pDirName & ExcelC.pFileName & ".zip", sZipFilePass)

            '        'compressC.p_NihongoFileName = pstrSESSION & pstrMAILAD & ".xls" '20050506 edit Falcon
            '        'compressC.p_NihongoFileName = pstrSESSION & pstrAUTO & pstrMAILAD & ".xls" '2008/12/12 T.Watabe edit
            '        'compressC.p_NihongoFileName = pstrSESSION & pstrAUTO & pstrMAILAD & ".xls.zip" '2011/03/10 T.Watabe edit
            '        'compressC.p_NihongoFileName = pstrSESSION & pstrAUTO & "(" & cliCd4FileHead & ")" & pstrMAILAD & ".xls.zip"
            '        'compressC.p_NihongoFileName = pstrSESSION & "[" & pstrAUTO & "][" & pstrMAILAD & "][" & cliCd4FileHead & "][" & jaName4FileHead & "].xls.zip"

            '        If pstrSEND_KBN = "1" Then '1:販売所？
            '            compressC.p_NihongoFileName = pstrSESSION & "[" & pstrAUTO & "][" & pstrMAILAD & "][" & cliCd4FileHead & "][" & jaName4FileHead & "].xls.zip"
            '        Else
            '            compressC.p_NihongoFileName = pstrSESSION & "[" & pstrAUTO & "][" & pstrMAILAD & "][" & cliCd4FileHead & "][" & centerName & "].xls.zip"
            '        End If


            '        '圧縮元ファイル名      (上記作成したEXCELファイルをこの名前でLZHに追加する)
            '        compressC.p_FileName = ExcelC.pDirName & ExcelC.pFileName & ".zip"
            '    End If

            '    '圧縮先ファイル名      (パラメータ[セッション])※２回目以降の時は追加される
            '    compressC.p_madeFilename = ExcelC.pDirName & pstrSESSION & ".lzh"
            '    '解凍先のパスを指定
            '    If pstrFlg = "1" Then
            '        compressC.p_ToDir = pstrCreateFilePath & "\"
            '    End If
            '    '解凍後自動実行無し
            '    compressC.p_Exec = False
            '    '圧縮実行
            '    compressC.mCompress()
            '    mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：8") '2014/04/11 T.Ono add ログ強化

            '    Try
            '        ' 2013/10/01 T.Watabe ファイル名を、指定日＋ＦＡＸ番号としてコピー作成するロジック。トラブル対応時に使用。
            '        'Dim wk As String
            '        'wk = pstrTAISYOUBI & "_" & pstrFAXNO & pstrMAILAD
            '        'System.IO.File.Copy(ExcelC.pDirName & ExcelC.pFileName & ".xls", ExcelC.pDirName & wk & ".xls", True)
            '    Catch ex As Exception
            '    End Try


            '    fncFileKill(ExcelC.pDirName & ExcelC.pFileName & ".xls") 'エクセルファイル削除！
            '    fncFileKill(ExcelC.pDirName & ExcelC.pFileName & ".zip") 'zipファイル削除！

            '    '圧縮したファイルをBase64エンコードして戻す
            '    Return FileToStrC.mFileToStr(compressC.p_madeFilename.Replace(".lzh", ".exe"))
            'End If

            If pintDebugSQLNo = 0 OrElse pintDebugSQLNo = 1 Then ' 通常またはデバッグ1
                '変数宣言
                Dim strToDir As String = ""           '圧縮先ファイルのあるフォルダ
                Dim strToZipDir As String = ""        '圧縮するディレクトリ
                Dim strNihongoFileName As String = "" '日本語ファイル名の指定(パラメータ[セッション] + 電話番号)
                Dim strFileName As String = ""        '圧縮元ファイル名(FAX用EXCEL、メール用ZIPファイル名)
                Dim strmadeFilename As String = ""    '圧縮先ファイル名(パラメータ[セッション])

                '圧縮先ファイルのあるフォルダ
                strToDir = ExcelC.pDirName
                '日本語ファイル名の指定(パラメータ[セッション] + 電話番号)
                If pstrAUTO = "1" Then '1:fax
                    'ＦＡＸファイル作成
                    strNihongoFileName = pstrSESSION & "[" & pstrAUTO & "][" & pstrFAXNO & "].xls"
                    '圧縮元ファイル名      (上記作成したEXCELファイル)(ZIP書庫に追加する)
                    strFileName = ExcelC.pDirName & ExcelC.pFileName & ".xls"
                Else
                    'メール送信ファイル作成
                    fncMakeZipWithPass(ExcelC.pDirName & ExcelC.pFileName & ".xls", ExcelC.pDirName & ExcelC.pFileName & ".zip", sZipFilePass)
                    If pstrSEND_KBN = "1" Then '1:販売所？
                        strNihongoFileName = pstrSESSION & "[" & pstrAUTO & "][" & pstrMAILAD & "][" & cliCd4FileHead & "][" & jaName4FileHead & "].xls.zip"
                    Else
                        strNihongoFileName = pstrSESSION & "[" & pstrAUTO & "][" & pstrMAILAD & "][" & cliCd4FileHead & "][" & centerName & "].xls.zip"
                    End If

                    '圧縮元ファイル名      (上記作成したEXCELファイルをこの名前でZIP書庫に追加する)
                    strFileName = ExcelC.pDirName & ExcelC.pFileName & ".zip"
                End If

                '圧縮するディレクトリの作成
                strToZipDir = ExcelC.pDirName & pstrSESSION
                If File.Exists(strToZipDir) = False Then
                    System.IO.Directory.CreateDirectory(strToZipDir)
                End If

                '圧縮先ファイル名      (パラメータ[セッション])※２回目以降の時は追加される
                strmadeFilename = strToZipDir & ".zip"
                '解凍先のパスを指定
                If pstrFlg = "1" Then
                    strToDir = pstrCreateFilePath & "\"
                End If

                'ディレクトリを圧縮（ZIP書庫作成）
                If File.Exists(strmadeFilename) = False Then
                    ZipFile.CreateFromDirectory(strToZipDir, strmadeFilename, System.IO.Compression.CompressionLevel.Optimal,
                                    False, System.Text.Encoding.GetEncoding("shift_jis"))
                End If

                'ZIP書庫をオープンし、ファイルを追加
                Using fo As ZipArchive = ZipFile.Open(strmadeFilename, ZipArchiveMode.Update)
                    Dim e As ZipArchiveEntry = fo.CreateEntryFromFile(strFileName, strNihongoFileName)
                End Using

                fncFileKill(ExcelC.pDirName & ExcelC.pFileName & ".xls") 'エクセルファイル削除！
                fncFileKill(ExcelC.pDirName & ExcelC.pFileName & ".zip") 'zipファイル削除！

                '圧縮したファイルをBase64エンコードして戻す
                Return FileToStrC.mFileToStr(strmadeFilename)
                mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T, "BTFAXJAX00:mExcelOut：8")
            End If
            '2018/02/19 T.Ono mod END ----------------------

        Catch ex As Exception
            mlog(pstrSESSION & "\" & pstrKANSI_CODE & "\" & pstrKURACD_F & "\" & pstrKURACD_T,
                 "[" & faxUser & "]" & "[" & ex.ToString & "][" & ex.Source & "][" & ex.Message & "]" & Environment.StackTrace)
            'エラーの内容とＳＱＬ文を返す
            Return "ERROR:" & ex.ToString & vbCrLf & vbCrLf & Environment.StackTrace & vbCrLf & " 直前のSQL[" & strSQL.ToString & "]"

        Finally

        End Try

        Return "THROW_DEBUG"
    End Function

    '****************************************************************
    'YYYYMMDD → YYYY年MM月DD日
    '****************************************************************
    Private Function fncEdit_Date(ByVal strDate As String) As String
        If strDate.Length = 8 Then
            Return strDate.Substring(0, 4) & "年" & strDate.Substring(4, 2) & "月" & strDate.Substring(6, 2) & "日"
        Else
            Return ""
        End If
    End Function

    '****************************************************************
    'YYYYMMDD → YYYY/MM/DD
    '****************************************************************
    Private Function fncDateSet(ByVal strDate As String) As String
        If strDate.Length = 8 Then
            Return strDate.Substring(0, 4) & "/" & strDate.Substring(4, 2) & "/" & strDate.Substring(6, 2)
        Else
            Return ""
        End If
    End Function

    '****************************************************************
    'HHMISS → HH時MI分
    '****************************************************************
    Private Function fncEdit_Time(ByVal strTime As String) As String
        If strTime.Length = 4 Then
            Return strTime.Substring(0, 2) & "時" & strTime.Substring(2, 2) & "分"
        Else
            Return ""
        End If
    End Function

    '****************************************************************
    'HHMISS → HH:MI
    '****************************************************************
    Private Function fncTimeSet(ByVal strTime As String) As String
        If strTime.Length = 4 Or strTime.Length = 6 Then
            Return strTime.Substring(0, 2) & ":" & strTime.Substring(2, 2)
        Else
            Return ""
        End If
    End Function

    '************************************************
    '指定された日数加算します
    '************************************************
    Private Function fncAdd_Date(ByVal pstrDate As String, ByVal pintDate As Integer) As String
        Dim strRec As String
        Try
            strRec = Format(DateSerial(CInt(pstrDate.Substring(0, 4)), CInt(pstrDate.Substring(4, 2)), CInt(pstrDate.Substring(6, 2)) + pintDate), "yyyyMMdd")
        Catch ex As Exception
            strRec = ""
        End Try
        Return strRec
    End Function

    '******************************************************************************
    '*　概　要：文字列バイト指定文字取得
    '*　備　考：
    '******************************************************************************
    Private Function MidB(ByVal pstrText As String, ByVal pintSt As Integer, ByVal pintLenb As Integer) As String
        Dim i As Integer
        Dim intCd As Integer
        Dim intBt As Integer
        Dim strTmp As String
        Dim strRec As String
        strRec = ""
        If pintSt < 1 Or pintLenb < 1 Then Return strRec
        For i = 1 To Len(pstrText)
            If intBt + 1 >= pintSt Then Exit For
            strTmp = Mid(pstrText, i, 1)
            intBt = intBt + LenB(strTmp)
        Next
        If i > Len(pstrText) Then Return strRec
        intBt = 0
        For i = i To Len(pstrText)
            strTmp = Mid(pstrText, i, 1)
            intBt = intBt + LenB(strTmp)
            If intBt > pintLenb Then Exit For
            strRec = strRec + strTmp
        Next
        Return strRec
    End Function

    '******************************************************************************
    '*　概　要：文字列バイト取得
    '*　備　考：
    '******************************************************************************
    Private Function LenB(ByVal pstrString As String) As Integer
        Return System.Text.Encoding.GetEncoding(932).GetByteCount(pstrString)

    End Function

    '2011/11/14 ADD H.Uema
    '******************************************************************************
    '*　概　要：ハイフン取り除く関数を付与したSQL文構築
    '*　備　考：引数の長さが0の場合, "'X'"を返却する
    '******************************************************************************
    Private Function ReplaceHyphen(ByVal pstrString As String) As String
        If (pstrString.Length > 0) Then
            Return "REPLACE(" & pstrString & ", '-')"
        Else
            Return "'X'"
        End If
    End Function

    '******************************************************************************
    '*　概　要：ファイルの圧縮(zip) vjslib.dll使用(.Netフレームワーク J#)(要参照設定)
    '*　備　考：
    '******************************************************************************
    'Private Sub fncZipTest(ByVal sXlsFilePath As String, ByVal sZipFilePath As String)
    '    Dim outStream As New java.util.zip.ZipOutputStream(New java.io.FileOutputStream(sZipFilePath))
    '    putFileToZip(outStream, sXlsFilePath)
    '    outStream.close()
    'End Sub
    'Private Sub putFileToZip(ByVal outStream As java.util.zip.ZipOutputStream, ByVal Path As String)
    '    Dim size As Integer = CInt(FileLen(Path))
    '    Dim inStream As New java.io.BufferedInputStream(New java.io.FileInputStream(Path))
    '    Dim crc As New java.util.zip.CRC32
    '    Dim buf(size - 1) As SByte
    '    If inStream.read(buf, 0, size) <> -1 Then
    '        crc.update(buf, 0, size)
    '        outStream.write(buf, 0, size)
    '    End If
    '    Dim entry As New java.util.zip.ZipEntry(System.IO.Path.GetFileName(Path))
    '    entry.setMethod(java.util.zip.ZipEntry.DEFLATED)
    '    entry.setSize(size)
    '    entry.setCrc(crc.getValue())
    '    outStream.putNextEntry(entry)
    '    inStream.close()
    '    outStream.closeEntry()
    '    outStream.flush()
    'End Sub

    '******************************************************************************
    '*　概　要：ファイルの圧縮(zip) ICSharpCode.SharpZipLib.dll使用(要参照設定)
    '*　備　考：
    '******************************************************************************
    Private Sub fncMakeZipWithPass(ByVal sXlsFilePath As String, ByVal sZipFilePath As String, ByVal sPass As String)

        '圧縮するファイルの設定 
        'Dim filePaths(100) As String
        Dim crc As ICSharpCode.SharpZipLib.Checksums.Crc32

        Dim writer As System.IO.FileStream
        Dim zos As ICSharpCode.SharpZipLib.Zip.ZipOutputStream
        Dim f As String
        Dim fs As System.IO.FileStream
        Dim buffer() As Byte
        Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry

        Dim file As String
        'filePaths(0) = sXlsFilePath

        If Len(sPass) <= 0 Then
            sPass = "jalp" 'パスワードのデフォルトはjalp
        End If

        crc = New ICSharpCode.SharpZipLib.Checksums.Crc32
        writer = New System.IO.FileStream( _
                        sZipFilePath, System.IO.FileMode.Create, _
                        System.IO.FileAccess.Write, _
                        System.IO.FileShare.Write)
        zos = New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(writer)

        ' 圧縮レベルを設定する 
        zos.SetLevel(6)
        ' パスワードを設定する 
        zos.Password = sPass

        ' Zipにファイルを追加する 
        If True Then
            'For Each file As String In filePaths '(複数ファイルを１つのzipに圧縮することもできる。)
            file = sXlsFilePath

            ' ZIPに追加するときのファイル名を決定する 
            f = System.IO.Path.GetFileName(file)
            ze = New ICSharpCode.SharpZipLib.Zip.ZipEntry(f)
            ze.CompressionMethod = ICSharpCode.SharpZipLib.Zip.CompressionMethod.Stored 'この1行でWindows標準でのPASS解凍問題が解決！？

            ' ヘッダを設定する 
            ' ファイルを読み込む 
            fs = New System.IO.FileStream( _
                        file, _
                        System.IO.FileMode.Open, _
                        System.IO.FileAccess.Read, _
                        System.IO.FileShare.Read)
            ReDim buffer(CInt(fs.Length))

            fs.Read(buffer, 0, buffer.Length)
            fs.Close()
            ' CRCを設定する 
            crc.Reset()
            crc.Update(buffer)
            ze.Crc = crc.Value
            ' サイズを設定する 
            ze.Size = buffer.Length

            ' 時間を設定する 
            ze.DateTime = DateTime.Now

            ' 新しいエントリの追加を開始 
            zos.PutNextEntry(ze)
            ' 書き込む 
            zos.Write(buffer, 0, buffer.Length)

            'Next
        End If

        zos.Close()
        writer.Close()
    End Sub

    ' 2015/01/22 T.Watabe del
    ''******************************************************************************
    ''*　概　要：ファイルの圧縮(zip) Ionic.Zip.dll使用(要参照設定) 2014/06/17 T.Watabe add
    ''*　備　考：
    ''******************************************************************************
    'Private Sub fncMakeZipWithPass2(ByVal sXlsFilePath As String, ByVal sZipFilePath As String, ByVal sPass As String)

    '    Dim zip As Ionic.Zip.ZipFile
    '    zip = New Ionic.Zip.ZipFile()
    '    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
    '    zip.AlternateEncoding = System.Text.Encoding.GetEncoding("shift_jis")
    '    zip.AlternateEncodingUsage = Ionic.Zip.ZipOption.Always

    '    If Len(sPass) <= 0 Then
    '        sPass = "jalp" 'パスワードのデフォルトはjalp
    '    End If
    '    zip.Password = sPass
    '    '// zip.Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes256 //AES 256ビット暗号化

    '    If (Directory.Exists(sXlsFilePath)) Then
    '        '//フォルダ
    '        zip.AddDirectory(sXlsFilePath, "") '(source,"dir")
    '    Else
    '        '//ファイル
    '        zip.AddFile(sXlsFilePath, "") '//(source,"dir")
    '    End If
    '    zip.Save(sZipFilePath)
    'End Sub

    '2008/12/15 T.Watabe add
    Function fncFileKill(ByVal sFilePath As String) As Boolean
        Dim bErr As Boolean
        bErr = False
        Try
            Kill(sFilePath)
        Catch ex As Exception
            bErr = True
        End Try
        Return bErr
    End Function



    '*****************************************************************
    '*　概　要：バッチ実行ログを取得
    '*****************************************************************
    <WebMethod()> Public Function dispBatchLog(ByRef batchLog As String) As Boolean

        Dim ds As New DataSet
        Dim cdb As New CDB
        Dim dr As DataRow

        batchLog = ""

        'batchLog = "test"
        'Return True
        'Exit Function

        '---------------------------------------------
        '接続文字列の設定
        '---------------------------------------------
        'cdb.pJPUID = ConfigurationSettings.AppSettings("DB_USER_ID")
        'cdb.pJPPWD = ConfigurationSettings.AppSettings("DB_PASSWORD")
        'cdb.pJPDB = ConfigurationSettings.AppSettings("DB_SID")

        '---------------------------------------------
        'プールの最小値設定
        '---------------------------------------------
        cdb.pConnectPoolSize = 1

        '接続OPEN----------------------------------------
        Try
            cdb.mOpen()
        Catch ex As Exception
            Return False
        Finally

        End Try

        Try
            'トランザクション開始
            cdb.mBeginTrans()

            '対応ＤＢ　D20_TAIOU
            Dim sql As New StringBuilder
            sql.Append("SELECT ")
            sql.Append("    TO_CHAR(TO_DATE(ST_YMD || ST_TIME,'YYYYMMDDHH24MISS'), 'YYYY/MM/DD HH24:MI') AS ST, ")
            sql.Append("    LPAD(TRUNC(EXEC_SEC / 1000,0), 5, ' ') || '秒' AS MIN, ")
            sql.Append("    SUBSTRB(MSG, 1,80) AS MSG ")
            sql.Append("FROM S02_BACHDB  ")
            sql.Append("WHERE PROC_ID = 'BTFAXJEE00' ")
            sql.Append("ORDER BY ST_YMD || ST_TIME DESC ")
            cdb.pSQL = sql.ToString
            cdb.mExecQuery() 'SQL実行！

            '結果をデータセットに格納
            ds = cdb.pResult

            'データが存在しない場合
            If ds.Tables(0).Rows.Count = 0 Then

                batchLog = "0件"
            Else
                Dim i As Integer = 0
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    If i >= 50 Then Exit For '50件まで

                    dr = ds.Tables(0).Rows(i)
                    'arrBatchLog.Add(Convert.ToString(dr.Item("ST")) & Convert.ToString(dr.Item("MIN")) & " " & Convert.ToString(dr.Item("MSG")))
                    If batchLog.Length > 0 Then batchLog = batchLog & "$$" '$$は関数から取得した改行付き文字列の改行がうまくされない為。
                    batchLog = batchLog & Convert.ToString(dr.Item("ST")) & Convert.ToString(dr.Item("MIN")) & " " & Convert.ToString(dr.Item("MSG"))
                Next
            End If

        Catch ex As Exception
            batchLog = ex.ToString
            Return False
        Finally
            '接続クローズ
            cdb.mClose()
        End Try
        cdb = Nothing

        Return True

    End Function

    ' 2010/09/15 T.Watabe add
    '*****************************************************************
    '*　概　要：DB SIDを戻す
    '*****************************************************************
    <WebMethod()> Public Function getDBSID() As String
        Dim res As String = ""
        Try
            res = ConfigurationSettings.AppSettings("JPDB")
        Catch ex As Exception
            res = "JPDB 参照エラー:" & ex.ToString
        End Try
        Return res
    End Function

    '******************************************************************************
    '*　概　要：S05_AUTOFAXLOGDB 自動FAXのログを記録する。
    '*　備　考：
    '*  作  成：2013/09/25 T.Watabe
    '******************************************************************************
    Private Function mInsertS05AutofaxLog(ByVal cdb As CDB,
                                        ByVal pstrEXEC_YMD As String,
                                        ByVal pstrEXEC_TIME As String,
                                        ByVal pstrEXEC_KBN As String,
                                        ByVal pstrGUID As String,
                                        ByVal pintSEQNO As Integer,
                                        ByVal pstrINPUT_YMD As String,
                                        ByVal pstrLATEST_OF_DAY_FLG As String,
                                        ByVal pstrTAIOU_KANSCD As String,
                                        ByVal pstrTAIOU_SYONO As String,
                                        ByVal pstrTAIOU_KURACD As String,
                                        ByVal pstrTAIOU_JACD As String,
                                        ByVal pstrTAIOU_ACBCD As String,
                                        ByVal pstrTAIOU_USER_CD As String,
                                        ByVal pstrAUTO_KBN As String,
                                        ByVal pstrAUTO_FAXNO As String,
                                        ByVal pstrAUTO_MAIL As String,
                                        ByVal pstrAUTO_ZERO_FLG As String,
                                        ByVal pstrWHERE_TAIOU As String,
                                        ByVal pstrDEBUGFLG As String,
                                        ByVal pstrBIKO As String
                                        ) As String

        Dim sql As New StringBuilder("")

        Try
            '文字編集
            pstrWHERE_TAIOU = Replace(pstrWHERE_TAIOU, "'", "''") 'カンマを２重化
            pstrWHERE_TAIOU = Replace(pstrWHERE_TAIOU, vbCrLf, "") '改行をはずす
            pstrWHERE_TAIOU = Replace(pstrWHERE_TAIOU, "  ", " ") 'スペースを詰める

            '/* 自動FAX専用ログテーブルへ出力 */
            sql = New StringBuilder("")
            sql.Append("INSERT INTO S05_AUTOFAXLOGDB ")
            sql.Append("(")
            sql.Append("    EXEC_YMD     ,")
            sql.Append("    EXEC_TIME    ,")
            sql.Append("    EXEC_KBN    ,")
            sql.Append("    GUID         ,")
            sql.Append("    SEQNO        ,")
            sql.Append("    INPUT_YMD       ,")
            sql.Append("    LATEST_OF_DAY_FLG       ,")
            sql.Append("    TAIOU_KANSCD       ,")
            sql.Append("    TAIOU_SYONO        ,")
            sql.Append("    TAIOU_KURACD       ,")
            sql.Append("    TAIOU_JACD         ,")
            sql.Append("    TAIOU_ACBCD        ,")
            sql.Append("    TAIOU_USER_CD      ,")
            sql.Append("    AUTO_KBN     ,")
            sql.Append("    AUTO_FAXNO   ,")
            sql.Append("    AUTO_MAIL    ,")
            sql.Append("    AUTO_ZERO_FLG,")
            sql.Append("    SQLWHERE,")
            sql.Append("    DEBUGFLG,")
            sql.Append("    BIKO,")
            sql.Append("    ADD_DATE      ")
            sql.Append(")VALUES( ")
            sql.Append("    '" & pstrEXEC_YMD & "', ")
            sql.Append("    '" & pstrEXEC_TIME & "', ")
            sql.Append("    '" & pstrEXEC_KBN & "', ")
            sql.Append("    '" & pstrGUID & "', ")
            sql.Append("     " & pintSEQNO & ", ")
            sql.Append("    '" & pstrINPUT_YMD & "', ")
            sql.Append("    '" & pstrLATEST_OF_DAY_FLG & "', ")
            sql.Append("    '" & pstrTAIOU_KANSCD & "', ")
            sql.Append("    '" & pstrTAIOU_SYONO & "', ")
            sql.Append("    '" & pstrTAIOU_KURACD & "', ")
            sql.Append("    '" & pstrTAIOU_JACD & "', ")
            sql.Append("    '" & pstrTAIOU_ACBCD & "', ")
            sql.Append("    '" & pstrTAIOU_USER_CD & "', ")
            sql.Append("    '" & pstrAUTO_KBN & "', ")
            sql.Append("    '" & pstrAUTO_FAXNO & "', ")
            sql.Append("    '" & pstrAUTO_MAIL & "', ")
            sql.Append("    '" & pstrAUTO_ZERO_FLG & "', ")
            sql.Append("    '" & pstrWHERE_TAIOU & "', ")
            sql.Append("    '" & pstrDEBUGFLG & "', ")
            sql.Append("    '" & pstrBIKO & "', ")
            sql.Append("    SYSDATE ")
            sql.Append(") ")

            cdb.mBeginTrans() 'トランザクション開始
            cdb.pSQL = sql.ToString '//SQLセット
            cdb.mExecNonQuery() '//SQL実行
            cdb.mCommit() 'トランザクション終了(コミット)
        Catch ex As Exception
            cdb.mRollback() 'トランザクション終了(ロールバック)
            'エラーの内容とＳＱＬ文を返す
            Return "ERROR:" & ex.ToString & vbCrLf & vbCrLf & Environment.StackTrace & vbCrLf & " 直前のSQL[" & sql.ToString & "]"
        Finally
        End Try

        Return "OK"

    End Function
    '******************************************************************************
    '*　概　要：S05_AUTOFAXLOGDB 自動FAXのログの前回分をクリアする。
    '*　備　考：
    '*  作  成：2013/09/25 T.Watabe
    '******************************************************************************
    Private Function mUpdateFlgS05AutofaxLog(ByVal cdb As CDB,
                                        ByVal pstrEXEC_KBN As String,
                                        ByVal pstrINPUT_YMD As String,
                                        ByVal pstrAUTO_KBN As String,
                                        ByVal pstrAUTO_FAXNO As String,
                                        ByVal pstrAUTO_MAIL As String
                                        ) As String

        Dim sql As New StringBuilder("")

        Try
            '/* 自動FAX専用ログテーブルを更新 */
            sql = New StringBuilder("")
            sql.Append("UPDATE S05_AUTOFAXLOGDB ")
            sql.Append("SET LATEST_OF_DAY_FLG = '0' ") '　前回作成データを古いと判別できるようにフラグ変える
            sql.Append("WHERE 1=1 ")
            sql.Append("    AND EXEC_KBN  = '" & pstrEXEC_KBN & "' ")
            sql.Append("    AND INPUT_YMD = '" & pstrINPUT_YMD & "' ")
            sql.Append("    AND LATEST_OF_DAY_FLG = '1' ")
            If pstrAUTO_KBN = "1" Then
                sql.Append("    AND AUTO_FAXNO = '" & pstrAUTO_FAXNO & "' ")
            Else
                sql.Append("    AND AUTO_MAIL = '" & pstrAUTO_MAIL & "' ")
            End If

            cdb.mBeginTrans() 'トランザクション開始
            cdb.pSQL = sql.ToString '//SQLセット
            cdb.mExecNonQuery() '//SQL実行
            cdb.mCommit() 'トランザクション終了(コミット)
        Catch ex As Exception
            cdb.mRollback() 'トランザクション終了(ロールバック)
            'エラーの内容とＳＱＬ文を返す
            Return "ERROR:" & ex.ToString & vbCrLf & vbCrLf & Environment.StackTrace & vbCrLf & " 直前のSQL[" & sql.ToString & "]"
        Finally
        End Try

        Return "OK"

    End Function
    '**********************************************************
    ' 2012/06/28 ADD W.GANEKO
    'ログ吐き出し
    '戻り値：書き込んだファイルへのフルパス
    '**********************************************************
    Public Sub mlog(ByVal pstrSESSION As String, ByVal pstrString As String)
        'Dim strFilnm As String = "log" & System.DateTime.Today.ToString("yyyyMMdd") '20140417 T.Ono mod
        Dim strKanscd() As String = Split(pstrSESSION, "\")
        Dim strFilnm As String = "log" & strKanscd(1) & "_" & System.DateTime.Today.ToString("yyyyMMdd")
        Dim strLogPath As String = ConfigurationSettings.AppSettings("LOG_OUT_PATH")
        Dim strPath As String = strLogPath & strFilnm & ".txt"
        Dim strLogFlg As String = ConfigurationSettings.AppSettings("LOG_OUT")
        If strLogFlg = "1" Then
            '書き込みファイルへのストリーム
            Dim outFile As New StreamWriter(strPath, True, System.Text.Encoding.Default)

            '引数の文字列をストリームに書き込み
            outFile.Write(System.DateTime.Now & ":" & pstrSESSION & ":[" & pstrString + "]" & vbCrLf)

            'メモリフラッシュ（ファイル書き込み）
            outFile.Flush()

            'ファイルクローズ
            outFile.Close()
        End If
    End Sub

End Class
