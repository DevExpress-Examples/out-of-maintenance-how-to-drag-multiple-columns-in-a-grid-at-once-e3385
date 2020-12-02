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

Namespace DXSample
	Friend Module Program
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		<STAThread>
		Sub Main()
			SkinManager.EnableFormSkins()
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			Application.Run(New Main())
		End Sub
	End Module
End Namespace