// Developer Express Code Central Example:
// How to drag multiple columns in a grid at once
// 
// This example illustrates how to move or hide some columns in a grid at once. For
// this you should select a desirable region where particular column headers are
// located by holding the left mouse button. After this, columns that reside within
// this area will be selected and you can drag them.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E3385

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.Utils.Drawing;

namespace DXSample {
    public class MulticolumnDragProvider
    {
        GridView view;
        List<GridColumn> dragColumns;
        Rectangle dragRectangle;
        Point startPoint, endPoint;
        int visibleIndex = -1;

        public MulticolumnDragProvider(GridView view)
        {
            this.view = view;
            dragColumns = new List<GridColumn>();
            dragRectangle = Rectangle.Empty;
            startPoint = Point.Empty;
            endPoint = Point.Empty;
        }

        public void EnableMultiColumnDragging()
        {
            view.CustomDrawColumnHeader += OnViewCustomDrawColumnHeader;
            view.GridControl.Paint += OnGridControlPaint;
            view.MouseDown += OnViewMouseDown;
            view.MouseMove += OnViewMouseMove;
            view.MouseUp += OnViewMouseUp;
            view.DragObjectStart += OnDragObjectStart;
            view.DragObjectOver += OnViewDragObjectOver;
            view.DragObjectDrop += OnDragObjectDrop;
        }

        void OnViewCustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (dragColumns != null && dragColumns.Contains(e.Column))
            {
                e.Painter.DrawObject(e.Info);
                using (SolidBrush br = new SolidBrush(Color.FromArgb(170, DevExpress.LookAndFeel.LookAndFeelHelper.GetSystemColor(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveLookAndFeel,
                   SystemColors.Control))))
                    e.Graphics.FillRectangle(br, e.Bounds);
                e.Handled = true;
            }
        }

        void OnGridControlPaint(object sender, PaintEventArgs e)
        {
            if (!dragRectangle.IsEmpty)
            {
                using(SolidBrush br = new SolidBrush(Color.FromArgb(170, DevExpress.LookAndFeel.LookAndFeelHelper.GetSystemColor(DevExpress.LookAndFeel.UserLookAndFeel.Default.ActiveLookAndFeel,
                    SystemColors.Control))))
                e.Graphics.FillRectangle(br, dragRectangle);
            }
        }

        void OnViewMouseDown(object sender, MouseEventArgs e)
        {
            startPoint = e.Location;
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);
            if (hitInfo.InRowCell)
                UpdateDragColumns(true);
        }

        void OnViewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                endPoint = e.Location;
                UpdateDragRectangle(false);
            }
        }
        void OnViewMouseUp(object sender, MouseEventArgs e)
        {
            FindDragColumns();
            UpdateDragRectangle(true);
        }

        private void UpdateDragRectangle(bool isEmpty)
        {
            if (!isEmpty)
                dragRectangle = new Rectangle(Math.Min(startPoint.X, endPoint.X), Math.Min(startPoint.Y, endPoint.Y),
                    Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y));
            else
                dragRectangle = Rectangle.Empty;
            view.Invalidate();
        }


        bool CanDrag(GridColumn dragObject, bool valid)
        {
            return dragObject != null && valid && !dragRectangle.IsEmpty && dragColumns.Count > 1;
        }

        void OnDragObjectStart(object sender, DevExpress.XtraGrid.Views.Base.DragObjectStartEventArgs e)
        {
            GridColumn column = e.DragObject as GridColumn;
            if (!CanDrag(column, e.Allow)) return;
            visibleIndex = column.VisibleIndex;
        }

        void OnViewDragObjectOver(object sender, DevExpress.XtraGrid.Views.Base.DragObjectOverEventArgs e)
        {
            GridColumn column = e.DragObject as GridColumn;
            if (!CanDrag(column, e.DropInfo.Valid)) return;
            GridColumn firstColumn = dragColumns[0];
            GridColumn lastColumn = dragColumns[dragColumns.Count -1];
            int startIndex = firstColumn.VisibleIndex;
            int endIndex = lastColumn.VisibleIndex;
            e.DropInfo.Valid = e.DropInfo.Index < startIndex || e.DropInfo.Index > endIndex + 1;
        }

        void OnDragObjectDrop(object sender, DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs e)
        {
            GridColumn column = e.DragObject as GridColumn;
            if (!CanDrag(column, e.DropInfo.Valid)) return;
            if (e.DropInfo.Index < 0)
                HideDragColumns();
            else
                MoveDragColumns(column.VisibleIndex - visibleIndex, column);
            UpdateDragRectangle(true);
            UpdateDragColumns(true);
        }

        private void FindDragColumns()
        {
            dragColumns.Clear();
            GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
            foreach (GridColumnInfoArgs args in viewInfo.ColumnsInfo)
                if (args.Column != null && dragRectangle.Contains(args.Bounds))
                    dragColumns.Add(args.Column);
            UpdateDragColumns(false);
        }

        private void UpdateDragColumns(bool clearDragColumns)
        {
            if (clearDragColumns)
                dragColumns.Clear();
            GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
            view.InvalidateRect(viewInfo.ViewRects.ColumnPanel);
        }

        private void MoveDragColumns(int delta, GridColumn dragColumn)
        {
               view.BeginUpdate();
               foreach (GridColumn column in dragColumns)
                   if (dragColumn != column)
                   {
                       int visibleIndex = column.VisibleIndex;
                       column.VisibleIndex = -1;
                       column.VisibleIndex = visibleIndex + delta;
                   }
               view.EndUpdate();
        }

        private void HideDragColumns()
        {
            foreach (GridColumn column in dragColumns)
                column.Visible = false;
        }

        public void DisableMultiColumnDragging()
        {
            view.CustomDrawColumnHeader -= OnViewCustomDrawColumnHeader;
            view.GridControl.Paint -= OnGridControlPaint;
            view.MouseDown -= OnViewMouseDown;
            view.MouseMove -= OnViewMouseMove;
            view.MouseUp -= OnViewMouseUp;
            view.DragObjectStart -= OnDragObjectStart;
            view.DragObjectOver -= OnViewDragObjectOver;
            view.DragObjectDrop -= OnDragObjectDrop;
            dragColumns.Clear();
            dragRectangle = Rectangle.Empty;
            startPoint = Point.Empty;
            endPoint = Point.Empty;
        }
    }
}