'====================================================================================================
'The Free Edition of Instant VB limits conversion output to 100 lines per file.

'To purchase the Premium Edition, visit our website:
'https://www.tangiblesoftwaresolutions.com/order/order-instant-vb.html
'====================================================================================================

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
Imports System.Windows.Forms
Imports DevExpress.Skins
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports System.Drawing
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.XtraGrid.Views.Grid.Drawing
Imports DevExpress.Utils.Drawing

Namespace DXSample
	Public Class MulticolumnDragProvider
		Private view As GridView
		Private dragColumns As List(Of GridColumn)
		Private dragRectangle As Rectangle
		Private startPoint, endPoint As Point
		Private visibleIndex As Integer = -1

		Public Sub New(ByVal view As GridView)
			Me.view = view
			dragColumns = New List(Of GridColumn)()
			dragRectangle = Rectangle.Empty
			startPoint = Point.Empty
			endPoint = Point.Empty
		End Sub

		Public Sub EnableMultiColumnDragging()
			AddHandler view.CustomDrawColumnHeader, AddressOf OnViewCustomDrawColumnHeader
			AddHandler view.GridControl.Paint, AddressOf OnGridControlPaint
			AddHandler view.MouseDown, AddressOf OnViewMouseDown
			AddHandler view.MouseMove, AddressOf OnViewMouseMove
			AddHandler view.MouseUp, AddressOf OnViewMouseUp
			AddHandler view.DragObjectStart, AddressOf OnDragObjectStart
			AddHandler view.DragObjectOver, AddressOf OnViewDragObjectOver
			AddHandler view.DragObjectDrop, AddressOf OnDragObjectDrop
		End Sub

		Private Sub OnViewCustomDrawColumnHeader(ByVal sender As Object, ByVal e As ColumnHeaderCustomDrawEventArgs)
			If dragColumns IsNot Nothing AndAlso dragColumns.Contains(e.Column) Then
				e.Painter.DrawObject(e.Info)
				Using br As New SolidBrush(Color.FromArgb(170, DevExpress.LookAndFeel.LookAndFeelHelper.GetSystemColor(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveLookAndFeel, SystemColors.Control)))
					e.Graphics.FillRectangle(br, e.Bounds)
				End Using
				e.Handled = True
			End If
		End Sub

		Private Sub OnGridControlPaint(ByVal sender As Object, ByVal e As PaintEventArgs)
			If Not dragRectangle.IsEmpty Then
				Using br As New SolidBrush(Color.FromArgb(170, DevExpress.LookAndFeel.LookAndFeelHelper.GetSystemColor(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveLookAndFeel, SystemColors.Control)))
				e.Graphics.FillRectangle(br, dragRectangle)
				End Using
			End If
		End Sub

		Private Sub OnViewMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
			startPoint = e.Location
			Dim hitInfo As GridHitInfo = view.CalcHitInfo(e.Location)
			If hitInfo.InRowCell Then
				UpdateDragColumns(True)
			End If
		End Sub

		Private Sub OnViewMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			If e.Button = MouseButtons.Left Then
				endPoint = e.Location
				UpdateDragRectangle(False)
			End If
		End Sub
		Private Sub OnViewMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
			FindDragColumns()
			UpdateDragRectangle(True)
		End Sub

		Private Sub UpdateDragRectangle(ByVal isEmpty As Boolean)
			If Not isEmpty Then
				dragRectangle = New Rectangle(Math.Min(startPoint.X, endPoint.X), Math.Min(startPoint.Y, endPoint.Y), Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y))
			Else
				dragRectangle = Rectangle.Empty
			End If
			view.Invalidate()
		End Sub


		Private Function CanDrag(ByVal dragObject As GridColumn, ByVal valid As Boolean) As Boolean
			Return dragObject IsNot Nothing AndAlso valid AndAlso Not dragRectangle.IsEmpty AndAlso dragColumns.Count > 1
		End Function

		Private Sub OnDragObjectStart(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.DragObjectStartEventArgs)
			Dim column As GridColumn = TryCast(e.DragObject, GridColumn)
			If Not CanDrag(column, e.Allow) Then
				Return
			End If
			visibleIndex = column.VisibleIndex
		End Sub

		Private Sub OnViewDragObjectOver(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.DragObjectOverEventArgs)
			Dim column As GridColumn = TryCast(e.DragObject, GridColumn)
			If Not CanDrag(column, e.DropInfo.Valid) Then
				Return
			End If
			Dim firstColumn As GridColumn = dragColumns(0)
			Dim lastColumn As GridColumn = dragColumns(dragColumns.Count -1)
			Dim startIndex As Integer = firstColumn.VisibleIndex
			Dim endIndex As Integer = lastColumn.VisibleIndex
			e.DropInfo.Valid = e.DropInfo.Index < startIndex OrElse e.DropInfo.Index > endIndex + 1
		End Sub

		Private Sub OnDragObjectDrop(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs)
			Dim column As GridColumn = TryCast(e.DragObject, GridColumn)
			If (Not CanDrag(column, e.DropInfo.Valid)) Then
				Return
			End If
			If e.DropInfo.Index < 0 Then
				HideDragColumns()
			Else
				MoveDragColumns(column.VisibleIndex - visibleIndex, column)
			End If
			UpdateDragRectangle(True)
			UpdateDragColumns(True)
		End Sub

		Private Sub FindDragColumns()
			dragColumns.Clear()
			Dim viewInfo As GridViewInfo = TryCast(view.GetViewInfo(), GridViewInfo)
			For Each args As GridColumnInfoArgs In viewInfo.ColumnsInfo
				If args.Column IsNot Nothing AndAlso dragRectangle.Contains(args.Bounds) Then
					dragColumns.Add(args.Column)
				End If
			Next args
			UpdateDragColumns(False)
		End Sub

		Private Sub UpdateDragColumns(ByVal clearDragColumns As Boolean)
			If clearDragColumns Then
				dragColumns.Clear()
			End If
			Dim viewInfo As GridViewInfo = TryCast(view.GetViewInfo(), GridViewInfo)
			view.InvalidateRect(viewInfo.ViewRects.ColumnPanel)
		End Sub

		Private Sub MoveDragColumns(ByVal delta As Integer, ByVal dragColumn As GridColumn)
			view.BeginUpdate()
			For Each column As GridColumn In dragColumns
				If dragColumn IsNot column Then
					Dim visibleIndex As Integer = column.VisibleIndex
					column.VisibleIndex = -1
					column.VisibleIndex = visibleIndex + delta
				End If
			Next column
			view.EndUpdate()
		End Sub

		Private Sub HideDragColumns()
			For Each column As GridColumn In dragColumns
				column.Visible = False
			Next column
		End Sub

		Public Sub DisableMultiColumnDragging()
			RemoveHandler view.CustomDrawColumnHeader, AddressOf OnViewCustomDrawColumnHeader
			RemoveHandler view.GridControl.Paint, AddressOf OnGridControlPaint
			RemoveHandler view.MouseDown, AddressOf OnViewMouseDown
			RemoveHandler view.MouseMove, AddressOf OnViewMouseMove
			RemoveHandler view.MouseUp, AddressOf OnViewMouseUp
			RemoveHandler view.DragObjectStart, AddressOf OnDragObjectStart
			RemoveHandler view.DragObjectOver, AddressOf OnViewDragObjectOver
			RemoveHandler view.DragObjectDrop, AddressOf OnDragObjectDrop
			dragColumns.Clear()
			dragRectangle = Rectangle.Empty
			startPoint = Point.Empty
			endPoint = Point.Empty
		End Sub
	End Class
End Namespace