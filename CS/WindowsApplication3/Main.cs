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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;


namespace DXSample {
    public partial class Main: XtraForm {
        public Main() {
            InitializeComponent();
        }
        public void InitData() {
            for(int i = 0;i < 5;i++) {
                dataSet11.Tables[0].Rows.Add(new object[] { i, string.Format("FirstName {0}", i), i, imageList1.Images[i], DateTime.Today.AddDays(i), true });
                dataSet11.Tables[1].Rows.Add(new object[] { i, i, i });
            }
        }
        MulticolumnDragProvider provider;
        private void Form1_Load(object sender, EventArgs e) {
            InitData();
            provider = new MulticolumnDragProvider(gridView1);
            provider.EnableMultiColumnDragging();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            provider.DisableMultiColumnDragging();
            base.OnFormClosing(e);
        }
      
    }
}
