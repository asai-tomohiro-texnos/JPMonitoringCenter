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
Namespace KETAISYG00KETAISYW00
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="KETAISYW00Soap", [Namespace]:="http://tempuri.org/KETAISYW00/Service1")>  _
    Partial Public Class KETAISYW00
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private mExcelOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.JPG.KESAIJAG00.Settings.Default.KESAIJAG00_KETAISYG00KETAISYW00_KETAISYW00
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
        Public Event mExcelCompleted As mExcelCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/KETAISYW00/Service1/mExcel", RequestNamespace:="http://tempuri.org/KETAISYW00/Service1", ResponseNamespace:="http://tempuri.org/KETAISYW00/Service1", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function mExcel( _
                    ByVal pstrSessionID As String,  _
                    ByVal pstrKANSCD As String,  _
                    ByVal pstrTFKICD As String,  _
                    ByVal pstrStkbn1 As String,  _
                    ByVal pstrStkbn2 As String,  _
                    ByVal pstrPgkbn1 As String,  _
                    ByVal pstrPgkbn2 As String,  _
                    ByVal pstrPgkbn3 As String,  _
                    ByVal pstrTrgFrom As String,  _
                    ByVal pstrTrgTo As String,  _
                    ByVal pstrKURACDFrom As String,  _
                    ByVal pstrKURACDTo As String,  _
                    ByVal pstrJACDFrom As String,  _
                    ByVal pstrJACDFrom_CLI As String,  _
                    ByVal pstrJACDTo As String,  _
                    ByVal pstrJACDTo_CLI As String,  _
                    ByVal pstrHANGRPFrom As String,  _
                    ByVal pstrHANGRPTo As String,  _
                    ByVal pstrOUTKBN As String,  _
                    ByVal pstrKIKANKBN As String,  _
                    ByVal pstrTrgTimeFrom As String,  _
                    ByVal pstrTrgTimeTo As String,  _
                    ByVal pstrHOKBN As String,  _
                    ByVal pstrOUTLIST As String,  _
                    ByVal pstrTFKINM As String,  _
                    ByVal pstrTSADCD As String,  _
                    ByVal pstrTSADNM As String) As String
            Dim results() As Object = Me.Invoke("mExcel", New Object() {pstrSessionID, pstrKANSCD, pstrTFKICD, pstrStkbn1, pstrStkbn2, pstrPgkbn1, pstrPgkbn2, pstrPgkbn3, pstrTrgFrom, pstrTrgTo, pstrKURACDFrom, pstrKURACDTo, pstrJACDFrom, pstrJACDFrom_CLI, pstrJACDTo, pstrJACDTo_CLI, pstrHANGRPFrom, pstrHANGRPTo, pstrOUTKBN, pstrKIKANKBN, pstrTrgTimeFrom, pstrTrgTimeTo, pstrHOKBN, pstrOUTLIST, pstrTFKINM, pstrTSADCD, pstrTSADNM})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Function BeginmExcel( _
                    ByVal pstrSessionID As String,  _
                    ByVal pstrKANSCD As String,  _
                    ByVal pstrTFKICD As String,  _
                    ByVal pstrStkbn1 As String,  _
                    ByVal pstrStkbn2 As String,  _
                    ByVal pstrPgkbn1 As String,  _
                    ByVal pstrPgkbn2 As String,  _
                    ByVal pstrPgkbn3 As String,  _
                    ByVal pstrTrgFrom As String,  _
                    ByVal pstrTrgTo As String,  _
                    ByVal pstrKURACDFrom As String,  _
                    ByVal pstrKURACDTo As String,  _
                    ByVal pstrJACDFrom As String,  _
                    ByVal pstrJACDFrom_CLI As String,  _
                    ByVal pstrJACDTo As String,  _
                    ByVal pstrJACDTo_CLI As String,  _
                    ByVal pstrHANGRPFrom As String,  _
                    ByVal pstrHANGRPTo As String,  _
                    ByVal pstrOUTKBN As String,  _
                    ByVal pstrKIKANKBN As String,  _
                    ByVal pstrTrgTimeFrom As String,  _
                    ByVal pstrTrgTimeTo As String,  _
                    ByVal pstrHOKBN As String,  _
                    ByVal pstrOUTLIST As String,  _
                    ByVal pstrTFKINM As String,  _
                    ByVal pstrTSADCD As String,  _
                    ByVal pstrTSADNM As String,  _
                    ByVal callback As System.AsyncCallback,  _
                    ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("mExcel", New Object() {pstrSessionID, pstrKANSCD, pstrTFKICD, pstrStkbn1, pstrStkbn2, pstrPgkbn1, pstrPgkbn2, pstrPgkbn3, pstrTrgFrom, pstrTrgTo, pstrKURACDFrom, pstrKURACDTo, pstrJACDFrom, pstrJACDFrom_CLI, pstrJACDTo, pstrJACDTo_CLI, pstrHANGRPFrom, pstrHANGRPTo, pstrOUTKBN, pstrKIKANKBN, pstrTrgTimeFrom, pstrTrgTimeTo, pstrHOKBN, pstrOUTLIST, pstrTFKINM, pstrTSADCD, pstrTSADNM}, callback, asyncState)
        End Function
        
        '''<remarks/>
        Public Function EndmExcel(ByVal asyncResult As System.IAsyncResult) As String
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub mExcelAsync( _
                    ByVal pstrSessionID As String,  _
                    ByVal pstrKANSCD As String,  _
                    ByVal pstrTFKICD As String,  _
                    ByVal pstrStkbn1 As String,  _
                    ByVal pstrStkbn2 As String,  _
                    ByVal pstrPgkbn1 As String,  _
                    ByVal pstrPgkbn2 As String,  _
                    ByVal pstrPgkbn3 As String,  _
                    ByVal pstrTrgFrom As String,  _
                    ByVal pstrTrgTo As String,  _
                    ByVal pstrKURACDFrom As String,  _
                    ByVal pstrKURACDTo As String,  _
                    ByVal pstrJACDFrom As String,  _
                    ByVal pstrJACDFrom_CLI As String,  _
                    ByVal pstrJACDTo As String,  _
                    ByVal pstrJACDTo_CLI As String,  _
                    ByVal pstrHANGRPFrom As String,  _
                    ByVal pstrHANGRPTo As String,  _
                    ByVal pstrOUTKBN As String,  _
                    ByVal pstrKIKANKBN As String,  _
                    ByVal pstrTrgTimeFrom As String,  _
                    ByVal pstrTrgTimeTo As String,  _
                    ByVal pstrHOKBN As String,  _
                    ByVal pstrOUTLIST As String,  _
                    ByVal pstrTFKINM As String,  _
                    ByVal pstrTSADCD As String,  _
                    ByVal pstrTSADNM As String)
            Me.mExcelAsync(pstrSessionID, pstrKANSCD, pstrTFKICD, pstrStkbn1, pstrStkbn2, pstrPgkbn1, pstrPgkbn2, pstrPgkbn3, pstrTrgFrom, pstrTrgTo, pstrKURACDFrom, pstrKURACDTo, pstrJACDFrom, pstrJACDFrom_CLI, pstrJACDTo, pstrJACDTo_CLI, pstrHANGRPFrom, pstrHANGRPTo, pstrOUTKBN, pstrKIKANKBN, pstrTrgTimeFrom, pstrTrgTimeTo, pstrHOKBN, pstrOUTLIST, pstrTFKINM, pstrTSADCD, pstrTSADNM, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub mExcelAsync( _
                    ByVal pstrSessionID As String,  _
                    ByVal pstrKANSCD As String,  _
                    ByVal pstrTFKICD As String,  _
                    ByVal pstrStkbn1 As String,  _
                    ByVal pstrStkbn2 As String,  _
                    ByVal pstrPgkbn1 As String,  _
                    ByVal pstrPgkbn2 As String,  _
                    ByVal pstrPgkbn3 As String,  _
                    ByVal pstrTrgFrom As String,  _
                    ByVal pstrTrgTo As String,  _
                    ByVal pstrKURACDFrom As String,  _
                    ByVal pstrKURACDTo As String,  _
                    ByVal pstrJACDFrom As String,  _
                    ByVal pstrJACDFrom_CLI As String,  _
                    ByVal pstrJACDTo As String,  _
                    ByVal pstrJACDTo_CLI As String,  _
                    ByVal pstrHANGRPFrom As String,  _
                    ByVal pstrHANGRPTo As String,  _
                    ByVal pstrOUTKBN As String,  _
                    ByVal pstrKIKANKBN As String,  _
                    ByVal pstrTrgTimeFrom As String,  _
                    ByVal pstrTrgTimeTo As String,  _
                    ByVal pstrHOKBN As String,  _
                    ByVal pstrOUTLIST As String,  _
                    ByVal pstrTFKINM As String,  _
                    ByVal pstrTSADCD As String,  _
                    ByVal pstrTSADNM As String,  _
                    ByVal userState As Object)
            If (Me.mExcelOperationCompleted Is Nothing) Then
                Me.mExcelOperationCompleted = AddressOf Me.OnmExcelOperationCompleted
            End If
            Me.InvokeAsync("mExcel", New Object() {pstrSessionID, pstrKANSCD, pstrTFKICD, pstrStkbn1, pstrStkbn2, pstrPgkbn1, pstrPgkbn2, pstrPgkbn3, pstrTrgFrom, pstrTrgTo, pstrKURACDFrom, pstrKURACDTo, pstrJACDFrom, pstrJACDFrom_CLI, pstrJACDTo, pstrJACDTo_CLI, pstrHANGRPFrom, pstrHANGRPTo, pstrOUTKBN, pstrKIKANKBN, pstrTrgTimeFrom, pstrTrgTimeTo, pstrHOKBN, pstrOUTLIST, pstrTFKINM, pstrTSADCD, pstrTSADNM}, Me.mExcelOperationCompleted, userState)
        End Sub
        
        Private Sub OnmExcelOperationCompleted(ByVal arg As Object)
            If (Not (Me.mExcelCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent mExcelCompleted(Me, New mExcelCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
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
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub mExcelCompletedEventHandler(ByVal sender As Object, ByVal e As mExcelCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class mExcelCompletedEventArgs
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
End Namespace
