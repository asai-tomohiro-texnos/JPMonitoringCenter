﻿'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:4.0.30319.42000
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'このソース コードは Microsoft.VSDesigner、バージョン 4.0.30319.42000 によって自動生成されました。
'
Namespace MSTAGJAG00MSTAGJAW00
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="MSTAGJAW00Soap", [Namespace]:="http://tempuri.org/MSTAGJAW00/Service1")>  _
    Partial Public Class MSTAGJAW00
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private mSetOperationCompleted As System.Threading.SendOrPostCallback
        
        Private mSetExOperationCompleted As System.Threading.SendOrPostCallback
        
        Private mSetTantoOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.MSTAGJAG00.My.MySettings.Default.MSTAGJAG00_MSTAGJAG00MSTAGJAW00_MSTAGJAW00
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event mSetCompleted As mSetCompletedEventHandler
        
        '''<remarks/>
        Public Event mSetExCompleted As mSetExCompletedEventHandler
        
        '''<remarks/>
        Public Event mSetTantoCompleted As mSetTantoCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/MSTAGJAW00/Service1/mSet", RequestNamespace:="http://tempuri.org/MSTAGJAW00/Service1", ResponseNamespace:="http://tempuri.org/MSTAGJAW00/Service1", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function mSet( _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrKBN As String,  _
                    ByVal pstrKURACD As String,  _
                    ByVal pstrCODE As String,  _
                    ByVal pstrUSER_CD_FROM As String,  _
                    ByVal pstrUSER_CD_TO As String,  _
                    ByVal pstrTANCD() As String,  _
                    ByVal pstrTANNM() As String,  _
                    ByVal pstrRENTEL1() As String,  _
                    ByVal pstrRENTEL2() As String,  _
                    ByVal pstrRENTEL3() As String,  _
                    ByVal pstrFAXNO() As String,  _
                    ByVal pstrDISP_NO() As String,  _
                    ByVal pstrBIKO() As String,  _
                    ByVal pstrADD_DATE As String,  _
                    ByVal pstrEDT_DATE As String,  _
                    ByVal pstrTIME As String,  _
                    ByVal pstrEDT_DT() As String) As String
            Dim results() As Object = Me.Invoke("mSet", New Object() {pintMODE, pstrKBN, pstrKURACD, pstrCODE, pstrUSER_CD_FROM, pstrUSER_CD_TO, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrDISP_NO, pstrBIKO, pstrADD_DATE, pstrEDT_DATE, pstrTIME, pstrEDT_DT})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Function BeginmSet( _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrKBN As String,  _
                    ByVal pstrKURACD As String,  _
                    ByVal pstrCODE As String,  _
                    ByVal pstrUSER_CD_FROM As String,  _
                    ByVal pstrUSER_CD_TO As String,  _
                    ByVal pstrTANCD() As String,  _
                    ByVal pstrTANNM() As String,  _
                    ByVal pstrRENTEL1() As String,  _
                    ByVal pstrRENTEL2() As String,  _
                    ByVal pstrRENTEL3() As String,  _
                    ByVal pstrFAXNO() As String,  _
                    ByVal pstrDISP_NO() As String,  _
                    ByVal pstrBIKO() As String,  _
                    ByVal pstrADD_DATE As String,  _
                    ByVal pstrEDT_DATE As String,  _
                    ByVal pstrTIME As String,  _
                    ByVal pstrEDT_DT() As String,  _
                    ByVal callback As System.AsyncCallback,  _
                    ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("mSet", New Object() {pintMODE, pstrKBN, pstrKURACD, pstrCODE, pstrUSER_CD_FROM, pstrUSER_CD_TO, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrDISP_NO, pstrBIKO, pstrADD_DATE, pstrEDT_DATE, pstrTIME, pstrEDT_DT}, callback, asyncState)
        End Function
        
        '''<remarks/>
        Public Function EndmSet(ByVal asyncResult As System.IAsyncResult) As String
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub mSetAsync( _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrKBN As String,  _
                    ByVal pstrKURACD As String,  _
                    ByVal pstrCODE As String,  _
                    ByVal pstrUSER_CD_FROM As String,  _
                    ByVal pstrUSER_CD_TO As String,  _
                    ByVal pstrTANCD() As String,  _
                    ByVal pstrTANNM() As String,  _
                    ByVal pstrRENTEL1() As String,  _
                    ByVal pstrRENTEL2() As String,  _
                    ByVal pstrRENTEL3() As String,  _
                    ByVal pstrFAXNO() As String,  _
                    ByVal pstrDISP_NO() As String,  _
                    ByVal pstrBIKO() As String,  _
                    ByVal pstrADD_DATE As String,  _
                    ByVal pstrEDT_DATE As String,  _
                    ByVal pstrTIME As String,  _
                    ByVal pstrEDT_DT() As String)
            Me.mSetAsync(pintMODE, pstrKBN, pstrKURACD, pstrCODE, pstrUSER_CD_FROM, pstrUSER_CD_TO, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrDISP_NO, pstrBIKO, pstrADD_DATE, pstrEDT_DATE, pstrTIME, pstrEDT_DT, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub mSetAsync( _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrKBN As String,  _
                    ByVal pstrKURACD As String,  _
                    ByVal pstrCODE As String,  _
                    ByVal pstrUSER_CD_FROM As String,  _
                    ByVal pstrUSER_CD_TO As String,  _
                    ByVal pstrTANCD() As String,  _
                    ByVal pstrTANNM() As String,  _
                    ByVal pstrRENTEL1() As String,  _
                    ByVal pstrRENTEL2() As String,  _
                    ByVal pstrRENTEL3() As String,  _
                    ByVal pstrFAXNO() As String,  _
                    ByVal pstrDISP_NO() As String,  _
                    ByVal pstrBIKO() As String,  _
                    ByVal pstrADD_DATE As String,  _
                    ByVal pstrEDT_DATE As String,  _
                    ByVal pstrTIME As String,  _
                    ByVal pstrEDT_DT() As String,  _
                    ByVal userState As Object)
            If (Me.mSetOperationCompleted Is Nothing) Then
                Me.mSetOperationCompleted = AddressOf Me.OnmSetOperationCompleted
            End If
            Me.InvokeAsync("mSet", New Object() {pintMODE, pstrKBN, pstrKURACD, pstrCODE, pstrUSER_CD_FROM, pstrUSER_CD_TO, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrDISP_NO, pstrBIKO, pstrADD_DATE, pstrEDT_DATE, pstrTIME, pstrEDT_DT}, Me.mSetOperationCompleted, userState)
        End Sub
        
        Private Sub OnmSetOperationCompleted(ByVal arg As Object)
            If (Not (Me.mSetCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent mSetCompleted(Me, New mSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/MSTAGJAW00/Service1/mSetEx", RequestNamespace:="http://tempuri.org/MSTAGJAW00/Service1", ResponseNamespace:="http://tempuri.org/MSTAGJAW00/Service1", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function mSetEx( _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrDBKBN As String,  _
                    ByVal pstrGROUPCD As String,  _
                    ByVal pstrGROUPNM() As String,  _
                    ByVal pstrTANCD() As String,  _
                    ByVal pstrTANNM() As String,  _
                    ByVal pstrRENTEL1() As String,  _
                    ByVal pstrRENTEL2() As String,  _
                    ByVal pstrRENTEL3() As String,  _
                    ByVal pstrFAXNO() As String,  _
                    ByVal pstrBIKO() As String,  _
                    ByVal pstrSPOT_MAIL() As String,  _
                    ByVal pstrMAIL_PASS() As String,  _
                    ByVal pstrAUTO_FAXNM() As String,  _
                    ByVal pstrAUTO_MAIL() As String,  _
                    ByVal pstrAUTO_MAIL_PASS() As String,  _
                    ByVal pstrAUTO_FAXNO() As String,  _
                    ByVal pstrAUTO_KBN() As String,  _
                    ByVal pstrAUTO_ZERO_FLG() As String,  _
                    ByVal pstrSD_PRT_FLG() As String,  _
                    ByVal pstrGUIDELINE() As String,  _
                    ByVal pstrGUIDELINE2() As String,  _
                    ByVal pstrGUIDELINE3() As String,  _
                    ByVal pstrGUIDELINENM1() As String,  _
                    ByVal pstrGUIDELINENM2() As String,  _
                    ByVal pstrGUIDELINENM3() As String,  _
                    ByVal pstrFAXKURAKBN() As String,  _
                    ByVal pstrFAXJAKBN() As String,  _
                    ByVal pstrINS_DATE() As String,  _
                    ByVal pstrINS_USER() As String,  _
                    ByVal pstrUPD_DATE() As String,  _
                    ByVal pstrUPD_USER() As String,  _
                    ByVal pstrUSERNM As String) As String
            Dim results() As Object = Me.Invoke("mSetEx", New Object() {pintMODE, pstrDBKBN, pstrGROUPCD, pstrGROUPNM, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrBIKO, pstrSPOT_MAIL, pstrMAIL_PASS, pstrAUTO_FAXNM, pstrAUTO_MAIL, pstrAUTO_MAIL_PASS, pstrAUTO_FAXNO, pstrAUTO_KBN, pstrAUTO_ZERO_FLG, pstrSD_PRT_FLG, pstrGUIDELINE, pstrGUIDELINE2, pstrGUIDELINE3, pstrGUIDELINENM1, pstrGUIDELINENM2, pstrGUIDELINENM3, pstrFAXKURAKBN, pstrFAXJAKBN, pstrINS_DATE, pstrINS_USER, pstrUPD_DATE, pstrUPD_USER, pstrUSERNM})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Function BeginmSetEx( _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrDBKBN As String,  _
                    ByVal pstrGROUPCD As String,  _
                    ByVal pstrGROUPNM() As String,  _
                    ByVal pstrTANCD() As String,  _
                    ByVal pstrTANNM() As String,  _
                    ByVal pstrRENTEL1() As String,  _
                    ByVal pstrRENTEL2() As String,  _
                    ByVal pstrRENTEL3() As String,  _
                    ByVal pstrFAXNO() As String,  _
                    ByVal pstrBIKO() As String,  _
                    ByVal pstrSPOT_MAIL() As String,  _
                    ByVal pstrMAIL_PASS() As String,  _
                    ByVal pstrAUTO_FAXNM() As String,  _
                    ByVal pstrAUTO_MAIL() As String,  _
                    ByVal pstrAUTO_MAIL_PASS() As String,  _
                    ByVal pstrAUTO_FAXNO() As String,  _
                    ByVal pstrAUTO_KBN() As String,  _
                    ByVal pstrAUTO_ZERO_FLG() As String,  _
                    ByVal pstrSD_PRT_FLG() As String,  _
                    ByVal pstrGUIDELINE() As String,  _
                    ByVal pstrGUIDELINE2() As String,  _
                    ByVal pstrGUIDELINE3() As String,  _
                    ByVal pstrGUIDELINENM1() As String,  _
                    ByVal pstrGUIDELINENM2() As String,  _
                    ByVal pstrGUIDELINENM3() As String,  _
                    ByVal pstrFAXKURAKBN() As String,  _
                    ByVal pstrFAXJAKBN() As String,  _
                    ByVal pstrINS_DATE() As String,  _
                    ByVal pstrINS_USER() As String,  _
                    ByVal pstrUPD_DATE() As String,  _
                    ByVal pstrUPD_USER() As String,  _
                    ByVal pstrUSERNM As String,  _
                    ByVal callback As System.AsyncCallback,  _
                    ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("mSetEx", New Object() {pintMODE, pstrDBKBN, pstrGROUPCD, pstrGROUPNM, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrBIKO, pstrSPOT_MAIL, pstrMAIL_PASS, pstrAUTO_FAXNM, pstrAUTO_MAIL, pstrAUTO_MAIL_PASS, pstrAUTO_FAXNO, pstrAUTO_KBN, pstrAUTO_ZERO_FLG, pstrSD_PRT_FLG, pstrGUIDELINE, pstrGUIDELINE2, pstrGUIDELINE3, pstrGUIDELINENM1, pstrGUIDELINENM2, pstrGUIDELINENM3, pstrFAXKURAKBN, pstrFAXJAKBN, pstrINS_DATE, pstrINS_USER, pstrUPD_DATE, pstrUPD_USER, pstrUSERNM}, callback, asyncState)
        End Function
        
        '''<remarks/>
        Public Function EndmSetEx(ByVal asyncResult As System.IAsyncResult) As String
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub mSetExAsync( _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrDBKBN As String,  _
                    ByVal pstrGROUPCD As String,  _
                    ByVal pstrGROUPNM() As String,  _
                    ByVal pstrTANCD() As String,  _
                    ByVal pstrTANNM() As String,  _
                    ByVal pstrRENTEL1() As String,  _
                    ByVal pstrRENTEL2() As String,  _
                    ByVal pstrRENTEL3() As String,  _
                    ByVal pstrFAXNO() As String,  _
                    ByVal pstrBIKO() As String,  _
                    ByVal pstrSPOT_MAIL() As String,  _
                    ByVal pstrMAIL_PASS() As String,  _
                    ByVal pstrAUTO_FAXNM() As String,  _
                    ByVal pstrAUTO_MAIL() As String,  _
                    ByVal pstrAUTO_MAIL_PASS() As String,  _
                    ByVal pstrAUTO_FAXNO() As String,  _
                    ByVal pstrAUTO_KBN() As String,  _
                    ByVal pstrAUTO_ZERO_FLG() As String,  _
                    ByVal pstrSD_PRT_FLG() As String,  _
                    ByVal pstrGUIDELINE() As String,  _
                    ByVal pstrGUIDELINE2() As String,  _
                    ByVal pstrGUIDELINE3() As String,  _
                    ByVal pstrGUIDELINENM1() As String,  _
                    ByVal pstrGUIDELINENM2() As String,  _
                    ByVal pstrGUIDELINENM3() As String,  _
                    ByVal pstrFAXKURAKBN() As String,  _
                    ByVal pstrFAXJAKBN() As String,  _
                    ByVal pstrINS_DATE() As String,  _
                    ByVal pstrINS_USER() As String,  _
                    ByVal pstrUPD_DATE() As String,  _
                    ByVal pstrUPD_USER() As String,  _
                    ByVal pstrUSERNM As String)
            Me.mSetExAsync(pintMODE, pstrDBKBN, pstrGROUPCD, pstrGROUPNM, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrBIKO, pstrSPOT_MAIL, pstrMAIL_PASS, pstrAUTO_FAXNM, pstrAUTO_MAIL, pstrAUTO_MAIL_PASS, pstrAUTO_FAXNO, pstrAUTO_KBN, pstrAUTO_ZERO_FLG, pstrSD_PRT_FLG, pstrGUIDELINE, pstrGUIDELINE2, pstrGUIDELINE3, pstrGUIDELINENM1, pstrGUIDELINENM2, pstrGUIDELINENM3, pstrFAXKURAKBN, pstrFAXJAKBN, pstrINS_DATE, pstrINS_USER, pstrUPD_DATE, pstrUPD_USER, pstrUSERNM, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub mSetExAsync( _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrDBKBN As String,  _
                    ByVal pstrGROUPCD As String,  _
                    ByVal pstrGROUPNM() As String,  _
                    ByVal pstrTANCD() As String,  _
                    ByVal pstrTANNM() As String,  _
                    ByVal pstrRENTEL1() As String,  _
                    ByVal pstrRENTEL2() As String,  _
                    ByVal pstrRENTEL3() As String,  _
                    ByVal pstrFAXNO() As String,  _
                    ByVal pstrBIKO() As String,  _
                    ByVal pstrSPOT_MAIL() As String,  _
                    ByVal pstrMAIL_PASS() As String,  _
                    ByVal pstrAUTO_FAXNM() As String,  _
                    ByVal pstrAUTO_MAIL() As String,  _
                    ByVal pstrAUTO_MAIL_PASS() As String,  _
                    ByVal pstrAUTO_FAXNO() As String,  _
                    ByVal pstrAUTO_KBN() As String,  _
                    ByVal pstrAUTO_ZERO_FLG() As String,  _
                    ByVal pstrSD_PRT_FLG() As String,  _
                    ByVal pstrGUIDELINE() As String,  _
                    ByVal pstrGUIDELINE2() As String,  _
                    ByVal pstrGUIDELINE3() As String,  _
                    ByVal pstrGUIDELINENM1() As String,  _
                    ByVal pstrGUIDELINENM2() As String,  _
                    ByVal pstrGUIDELINENM3() As String,  _
                    ByVal pstrFAXKURAKBN() As String,  _
                    ByVal pstrFAXJAKBN() As String,  _
                    ByVal pstrINS_DATE() As String,  _
                    ByVal pstrINS_USER() As String,  _
                    ByVal pstrUPD_DATE() As String,  _
                    ByVal pstrUPD_USER() As String,  _
                    ByVal pstrUSERNM As String,  _
                    ByVal userState As Object)
            If (Me.mSetExOperationCompleted Is Nothing) Then
                Me.mSetExOperationCompleted = AddressOf Me.OnmSetExOperationCompleted
            End If
            Me.InvokeAsync("mSetEx", New Object() {pintMODE, pstrDBKBN, pstrGROUPCD, pstrGROUPNM, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrBIKO, pstrSPOT_MAIL, pstrMAIL_PASS, pstrAUTO_FAXNM, pstrAUTO_MAIL, pstrAUTO_MAIL_PASS, pstrAUTO_FAXNO, pstrAUTO_KBN, pstrAUTO_ZERO_FLG, pstrSD_PRT_FLG, pstrGUIDELINE, pstrGUIDELINE2, pstrGUIDELINE3, pstrGUIDELINENM1, pstrGUIDELINENM2, pstrGUIDELINENM3, pstrFAXKURAKBN, pstrFAXJAKBN, pstrINS_DATE, pstrINS_USER, pstrUPD_DATE, pstrUPD_USER, pstrUSERNM}, Me.mSetExOperationCompleted, userState)
        End Sub
        
        Private Sub OnmSetExOperationCompleted(ByVal arg As Object)
            If (Not (Me.mSetExCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent mSetExCompleted(Me, New mSetExCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/MSTAGJAW00/Service1/mSetTanto", RequestNamespace:="http://tempuri.org/MSTAGJAW00/Service1", ResponseNamespace:="http://tempuri.org/MSTAGJAW00/Service1", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function mSetTanto( _
                    ByRef cdb As CDB,  _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrDBKBN As String,  _
                    ByVal pstrGROUPCD As String,  _
                    ByVal pstrGROUPNM As String,  _
                    ByVal pstrTANCD As String,  _
                    ByVal pstrTANNM As String,  _
                    ByVal pstrRENTEL1 As String,  _
                    ByVal pstrRENTEL2 As String,  _
                    ByVal pstrRENTEL3 As String,  _
                    ByVal pstrFAXNO As String,  _
                    ByVal pstrBIKO As String,  _
                    ByVal pstrSPOT_MAIL As String,  _
                    ByVal pstrMAIL_PASS As String,  _
                    ByVal pstrAUTO_FAXNM As String,  _
                    ByVal pstrAUTO_MAIL As String,  _
                    ByVal pstrAUTO_MAIL_PASS As String,  _
                    ByVal pstrAUTO_FAXNO As String,  _
                    ByVal pstrAUTO_KBN As String,  _
                    ByVal pstrAUTO_ZERO_FLG As String,  _
                    ByVal pstrSD_PRT_FLG As String,  _
                    ByVal pstrGUIDELINE As String,  _
                    ByVal pstrGUIDELINE2 As String,  _
                    ByVal pstrGUIDELINE3 As String,  _
                    ByVal pstrGUIDELINENM1 As String,  _
                    ByVal pstrGUIDELINENM2 As String,  _
                    ByVal pstrGUIDELINENM3 As String,  _
                    ByVal pstrFAXKURAKBN As String,  _
                    ByVal pstrFAXJAKBN As String,  _
                    ByVal pstrINS_DATE As String,  _
                    ByVal pstrINS_USER As String,  _
                    ByVal pstrUPD_DATE As String,  _
                    ByVal pstrUPD_USER As String,  _
                    ByVal pstrUSERNM As String) As String
            Dim results() As Object = Me.Invoke("mSetTanto", New Object() {cdb, pintMODE, pstrDBKBN, pstrGROUPCD, pstrGROUPNM, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrBIKO, pstrSPOT_MAIL, pstrMAIL_PASS, pstrAUTO_FAXNM, pstrAUTO_MAIL, pstrAUTO_MAIL_PASS, pstrAUTO_FAXNO, pstrAUTO_KBN, pstrAUTO_ZERO_FLG, pstrSD_PRT_FLG, pstrGUIDELINE, pstrGUIDELINE2, pstrGUIDELINE3, pstrGUIDELINENM1, pstrGUIDELINENM2, pstrGUIDELINENM3, pstrFAXKURAKBN, pstrFAXJAKBN, pstrINS_DATE, pstrINS_USER, pstrUPD_DATE, pstrUPD_USER, pstrUSERNM})
            cdb = CType(results(1),CDB)
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Function BeginmSetTanto( _
                    ByVal cdb As CDB,  _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrDBKBN As String,  _
                    ByVal pstrGROUPCD As String,  _
                    ByVal pstrGROUPNM As String,  _
                    ByVal pstrTANCD As String,  _
                    ByVal pstrTANNM As String,  _
                    ByVal pstrRENTEL1 As String,  _
                    ByVal pstrRENTEL2 As String,  _
                    ByVal pstrRENTEL3 As String,  _
                    ByVal pstrFAXNO As String,  _
                    ByVal pstrBIKO As String,  _
                    ByVal pstrSPOT_MAIL As String,  _
                    ByVal pstrMAIL_PASS As String,  _
                    ByVal pstrAUTO_FAXNM As String,  _
                    ByVal pstrAUTO_MAIL As String,  _
                    ByVal pstrAUTO_MAIL_PASS As String,  _
                    ByVal pstrAUTO_FAXNO As String,  _
                    ByVal pstrAUTO_KBN As String,  _
                    ByVal pstrAUTO_ZERO_FLG As String,  _
                    ByVal pstrSD_PRT_FLG As String,  _
                    ByVal pstrGUIDELINE As String,  _
                    ByVal pstrGUIDELINE2 As String,  _
                    ByVal pstrGUIDELINE3 As String,  _
                    ByVal pstrGUIDELINENM1 As String,  _
                    ByVal pstrGUIDELINENM2 As String,  _
                    ByVal pstrGUIDELINENM3 As String,  _
                    ByVal pstrFAXKURAKBN As String,  _
                    ByVal pstrFAXJAKBN As String,  _
                    ByVal pstrINS_DATE As String,  _
                    ByVal pstrINS_USER As String,  _
                    ByVal pstrUPD_DATE As String,  _
                    ByVal pstrUPD_USER As String,  _
                    ByVal pstrUSERNM As String,  _
                    ByVal callback As System.AsyncCallback,  _
                    ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("mSetTanto", New Object() {cdb, pintMODE, pstrDBKBN, pstrGROUPCD, pstrGROUPNM, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrBIKO, pstrSPOT_MAIL, pstrMAIL_PASS, pstrAUTO_FAXNM, pstrAUTO_MAIL, pstrAUTO_MAIL_PASS, pstrAUTO_FAXNO, pstrAUTO_KBN, pstrAUTO_ZERO_FLG, pstrSD_PRT_FLG, pstrGUIDELINE, pstrGUIDELINE2, pstrGUIDELINE3, pstrGUIDELINENM1, pstrGUIDELINENM2, pstrGUIDELINENM3, pstrFAXKURAKBN, pstrFAXJAKBN, pstrINS_DATE, pstrINS_USER, pstrUPD_DATE, pstrUPD_USER, pstrUSERNM}, callback, asyncState)
        End Function
        
        '''<remarks/>
        Public Function EndmSetTanto(ByVal asyncResult As System.IAsyncResult, ByRef cdb As CDB) As String
            Dim results() As Object = Me.EndInvoke(asyncResult)
            cdb = CType(results(1),CDB)
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub mSetTantoAsync( _
                    ByVal cdb As CDB,  _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrDBKBN As String,  _
                    ByVal pstrGROUPCD As String,  _
                    ByVal pstrGROUPNM As String,  _
                    ByVal pstrTANCD As String,  _
                    ByVal pstrTANNM As String,  _
                    ByVal pstrRENTEL1 As String,  _
                    ByVal pstrRENTEL2 As String,  _
                    ByVal pstrRENTEL3 As String,  _
                    ByVal pstrFAXNO As String,  _
                    ByVal pstrBIKO As String,  _
                    ByVal pstrSPOT_MAIL As String,  _
                    ByVal pstrMAIL_PASS As String,  _
                    ByVal pstrAUTO_FAXNM As String,  _
                    ByVal pstrAUTO_MAIL As String,  _
                    ByVal pstrAUTO_MAIL_PASS As String,  _
                    ByVal pstrAUTO_FAXNO As String,  _
                    ByVal pstrAUTO_KBN As String,  _
                    ByVal pstrAUTO_ZERO_FLG As String,  _
                    ByVal pstrSD_PRT_FLG As String,  _
                    ByVal pstrGUIDELINE As String,  _
                    ByVal pstrGUIDELINE2 As String,  _
                    ByVal pstrGUIDELINE3 As String,  _
                    ByVal pstrGUIDELINENM1 As String,  _
                    ByVal pstrGUIDELINENM2 As String,  _
                    ByVal pstrGUIDELINENM3 As String,  _
                    ByVal pstrFAXKURAKBN As String,  _
                    ByVal pstrFAXJAKBN As String,  _
                    ByVal pstrINS_DATE As String,  _
                    ByVal pstrINS_USER As String,  _
                    ByVal pstrUPD_DATE As String,  _
                    ByVal pstrUPD_USER As String,  _
                    ByVal pstrUSERNM As String)
            Me.mSetTantoAsync(cdb, pintMODE, pstrDBKBN, pstrGROUPCD, pstrGROUPNM, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrBIKO, pstrSPOT_MAIL, pstrMAIL_PASS, pstrAUTO_FAXNM, pstrAUTO_MAIL, pstrAUTO_MAIL_PASS, pstrAUTO_FAXNO, pstrAUTO_KBN, pstrAUTO_ZERO_FLG, pstrSD_PRT_FLG, pstrGUIDELINE, pstrGUIDELINE2, pstrGUIDELINE3, pstrGUIDELINENM1, pstrGUIDELINENM2, pstrGUIDELINENM3, pstrFAXKURAKBN, pstrFAXJAKBN, pstrINS_DATE, pstrINS_USER, pstrUPD_DATE, pstrUPD_USER, pstrUSERNM, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub mSetTantoAsync( _
                    ByVal cdb As CDB,  _
                    ByVal pintMODE As Integer,  _
                    ByVal pstrDBKBN As String,  _
                    ByVal pstrGROUPCD As String,  _
                    ByVal pstrGROUPNM As String,  _
                    ByVal pstrTANCD As String,  _
                    ByVal pstrTANNM As String,  _
                    ByVal pstrRENTEL1 As String,  _
                    ByVal pstrRENTEL2 As String,  _
                    ByVal pstrRENTEL3 As String,  _
                    ByVal pstrFAXNO As String,  _
                    ByVal pstrBIKO As String,  _
                    ByVal pstrSPOT_MAIL As String,  _
                    ByVal pstrMAIL_PASS As String,  _
                    ByVal pstrAUTO_FAXNM As String,  _
                    ByVal pstrAUTO_MAIL As String,  _
                    ByVal pstrAUTO_MAIL_PASS As String,  _
                    ByVal pstrAUTO_FAXNO As String,  _
                    ByVal pstrAUTO_KBN As String,  _
                    ByVal pstrAUTO_ZERO_FLG As String,  _
                    ByVal pstrSD_PRT_FLG As String,  _
                    ByVal pstrGUIDELINE As String,  _
                    ByVal pstrGUIDELINE2 As String,  _
                    ByVal pstrGUIDELINE3 As String,  _
                    ByVal pstrGUIDELINENM1 As String,  _
                    ByVal pstrGUIDELINENM2 As String,  _
                    ByVal pstrGUIDELINENM3 As String,  _
                    ByVal pstrFAXKURAKBN As String,  _
                    ByVal pstrFAXJAKBN As String,  _
                    ByVal pstrINS_DATE As String,  _
                    ByVal pstrINS_USER As String,  _
                    ByVal pstrUPD_DATE As String,  _
                    ByVal pstrUPD_USER As String,  _
                    ByVal pstrUSERNM As String,  _
                    ByVal userState As Object)
            If (Me.mSetTantoOperationCompleted Is Nothing) Then
                Me.mSetTantoOperationCompleted = AddressOf Me.OnmSetTantoOperationCompleted
            End If
            Me.InvokeAsync("mSetTanto", New Object() {cdb, pintMODE, pstrDBKBN, pstrGROUPCD, pstrGROUPNM, pstrTANCD, pstrTANNM, pstrRENTEL1, pstrRENTEL2, pstrRENTEL3, pstrFAXNO, pstrBIKO, pstrSPOT_MAIL, pstrMAIL_PASS, pstrAUTO_FAXNM, pstrAUTO_MAIL, pstrAUTO_MAIL_PASS, pstrAUTO_FAXNO, pstrAUTO_KBN, pstrAUTO_ZERO_FLG, pstrSD_PRT_FLG, pstrGUIDELINE, pstrGUIDELINE2, pstrGUIDELINE3, pstrGUIDELINENM1, pstrGUIDELINENM2, pstrGUIDELINENM3, pstrFAXKURAKBN, pstrFAXJAKBN, pstrINS_DATE, pstrINS_USER, pstrUPD_DATE, pstrUPD_USER, pstrUSERNM}, Me.mSetTantoOperationCompleted, userState)
        End Sub
        
        Private Sub OnmSetTantoOperationCompleted(ByVal arg As Object)
            If (Not (Me.mSetTantoCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent mSetTantoCompleted(Me, New mSetTantoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/MSTAGJAW00/Service1")>  _
    Partial Public Class CDB
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub mSetCompletedEventHandler(ByVal sender As Object, ByVal e As mSetCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class mSetCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub mSetExCompletedEventHandler(ByVal sender As Object, ByVal e As mSetExCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class mSetExCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub mSetTantoCompletedEventHandler(ByVal sender As Object, ByVal e As mSetTantoCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class mSetTantoCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property cdb() As CDB
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(1),CDB)
            End Get
        End Property
    End Class
End Namespace
