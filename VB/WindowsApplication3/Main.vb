' Developer Express Code Central Example:
' How to drag multiple columns in a grid at once
' 
' This example illustrates how to move or hide some columns in a grid at once. For
' this you should select a desirable region where particular column headers are
' located by holding the left mouse button. After this, columns that reside within
' this area will be selected and you can drag them.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E3385

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors


Namespace DXSample
	Partial Public Class Main
		Inherits XtraForm

		Public Sub New()
			InitializeComponent()
		End Sub
		Public Sub InitData()
			For i As Integer = 0 To 4
				dataSet11.Tables(0).Rows.Add(New Object() { i, String.Format("FirstName {0}", i), i, imageList1.Images(i), DateTime.Today.AddDays(i), True })
				dataSet11.Tables(1).Rows.Add(New Object() { i, i, i })
			Next i
		End Sub
		Private provider As MulticolumnDragProvider
		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
			InitData()
			provider = New MulticolumnDragProvider(gridView1)
			provider.EnableMultiColumnDragging()
		End Sub

		Protected Overrides Sub OnFormClosing(ByVal e As FormClosingEventArgs)
			provider.DisableMultiColumnDragging()
			MyBase.OnFormClosing(e)
		End Sub

	End Class
End Namespace
