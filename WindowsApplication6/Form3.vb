Imports System.IO

Public Class Form3
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = System.IO.Path.GetFileNameWithoutExtension(Form2.filename)
        Dim reader3 As StreamReader = New StreamReader(Label1.Text & "results.dat")

        Do While reader3.Peek() >= 0
            ListBox1.Items.Add(reader3.ReadLine() & "   " & reader3.ReadLine() & "   " & reader3.ReadLine())
        Loop
    End Sub
End Class