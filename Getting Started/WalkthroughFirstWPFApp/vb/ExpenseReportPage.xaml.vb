Imports System.Windows.Controls

Namespace ExpenseIt
    ''' <summary>
    ''' Interaction logic for ExpenseReportPage.xaml
    ''' </summary>

    Partial Public Class ExpenseReportPage
        Inherits Page
        Public Sub New()
            InitializeComponent()
        End Sub

        ' Custom constructor to pass expense report data
        Public Sub New(ByVal data As Object)
            Me.New()
            ' Bind to expense report data.
            Me.DataContext = data
        End Sub

    End Class

End Namespace
