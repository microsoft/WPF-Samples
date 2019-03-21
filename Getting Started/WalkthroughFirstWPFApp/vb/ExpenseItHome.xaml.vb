Namespace ExpenseIt
	''' <summary>
	''' Interaction logic for ExpenseItHome.xaml
	''' </summary>
	Partial Public Class ExpenseItHome
		Inherits Page
		Public Sub New()
			InitializeComponent()
		End Sub


		Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			' View Expense Report
			Dim expenseReportPage As New ExpenseReportPage(Me.peopleListBox.SelectedItem)
			Me.NavigationService.Navigate(expenseReportPage)

		End Sub

	End Class
End Namespace
