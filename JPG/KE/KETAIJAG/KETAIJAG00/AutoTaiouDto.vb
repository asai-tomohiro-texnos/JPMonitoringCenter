Namespace KETAIJAG00DTO
    Public Class AutoTaiouDto

        Private strGROUPCD As String 'O[vR[h
        Private kmDto As KmDto
        Private strPROCKBN As String 'Î/³æª
        Private strTAIOKBN As String 'Îæª
        Private strTMSKB As String 'æª
        Private strTKTANCD As String 'ÄSÒR[h
        Private strTKTANNM As String 'ÄSÒ¼
        Private strTAITCD As String 'AèCD
        Private strTFKICD As String 'AÎóµ
        Private strTKIGCD As String 'KXíï
        Private strTSADCD As String 'ì®´ö
        Private strTELRCD As String 'dbAàe
        Private strTEL_MEMO1 As String 'dbÎP
        Private strUSE_FLG As String 'gptO

        Public Sub New()
        End Sub

        'O[vR[h Setter/Getter
        Public Property groupcd()
            Get
                Return strGROUPCD
            End Get
            Set(ByVal Value)
                strGROUPCD = Value
            End Set
        End Property

        'xñDTO Setter/Getter
        Public Property pkmDto()
            Get
                Return kmDto
            End Get
            Set(ByVal Value)
                kmDto = Value
            End Set
        End Property

        'Î/³æª Setter/Getter
        Public Property prockbn()
            Get
                Return strPROCKBN
            End Get
            Set(ByVal Value)
                strPROCKBN = Value
            End Set
        End Property

        'Îæª Setter/Getter
        Public Property taiokbn()
            Get
                Return strTAIOKBN
            End Get
            Set(ByVal Value)
                strTAIOKBN = Value
            End Set
        End Property

        'æª Setter/Getter
        Public Property tmskb()
            Get
                Return strTMSKB
            End Get
            Set(ByVal Value)
                strTMSKB = Value
            End Set
        End Property

        'ÄSÒR[h Setter/Getter
        Public Property tktancd()
            Get
                Return strTKTANCD
            End Get
            Set(ByVal Value)
                strTKTANCD = Value
            End Set
        End Property

        'ÄSÒ¼ Setter/Getter
        Public Property tktannm()
            Get
                Return strTKTANNM
            End Get
            Set(ByVal Value)
                strTKTANNM = Value
            End Set
        End Property

        'AèCD Setter/Getter
        Public Property taitcd()
            Get
                Return strTAITCD
            End Get
            Set(ByVal Value)
                strTAITCD = Value
            End Set
        End Property

        'AÎóµ Setter/Getter
        Public Property tfkicd()
            Get
                Return strTFKICD
            End Get
            Set(ByVal Value)
                strTFKICD = Value
            End Set
        End Property

        'KXíï Setter/Getter
        Public Property tkigcd()
            Get
                Return strTKIGCD
            End Get
            Set(ByVal Value)
                strTKIGCD = Value
            End Set
        End Property

        'ì®´ö Setter/Getter
        Public Property tsadcd()
            Get
                Return strTSADCD
            End Get
            Set(ByVal Value)
                strTSADCD = Value
            End Set
        End Property

        'dbAàe Setter/Getter
        Public Property telrcd()
            Get
                Return strTELRCD
            End Get
            Set(ByVal Value)
                strTELRCD = Value
            End Set
        End Property

        'dbÎP Setter/Getter
        Public Property tel_memo1()
            Get
                Return strTEL_MEMO1
            End Get
            Set(ByVal Value)
                strTEL_MEMO1 = Value
            End Set
        End Property

        'gptO Setter/Getter
        Public Property use_flg()
            Get
                Return strUSE_FLG
            End Get
            Set(ByVal Value)
                strUSE_FLG = Value
            End Set
        End Property

    End Class
End Namespace
