Imports System.IO

Public Class Form2

    Public filename As String
    Dim userName As String
    Dim input() As String
    Dim totalQuestions As Integer = 0
    Dim questionNum As Integer = 1
    Dim fontName As String = "Times New Roman"
    Dim fontSize As Integer = 12
    Dim questionOrder() As Integer

    Dim Questions As New List(Of Form1.Question)

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim results As DialogResult = OpenFileDialog1.ShowDialog
        Dim loadQuestion As New Form1.Question
        Dim currentrow As String
        If results <> Windows.Forms.DialogResult.Cancel Then
            filename = OpenFileDialog1.FileName

            Dim reader2 As StreamReader = New StreamReader(filename)

            Do While reader2.Peek() >= 0
                currentrow = reader2.ReadLine()
                loadQuestion.questionName = currentrow
                currentrow = reader2.ReadLine()
                loadQuestion.answer1 = currentrow
                currentrow = reader2.ReadLine()
                loadQuestion.answer2 = currentrow
                currentrow = reader2.ReadLine()
                loadQuestion.answer3 = currentrow
                currentrow = reader2.ReadLine()
                loadQuestion.answer4 = currentrow
                currentrow = reader2.ReadLine()
                loadQuestion.correct = CInt(Int(currentrow))
                Questions.Add(loadQuestion)
            Loop
            reader2.Close()
            resultsButton.Enabled = True
            Label3.Text = "(" & System.IO.Path.GetFileNameWithoutExtension(filename) & " has " & Questions.Count & " possible questions)"
            resultsButton.Enabled = True
        End If
    End Sub

    Private Sub beginButton_Click(sender As Object, e As EventArgs) Handles beginButton.Click
        Dim objWriter As New System.IO.StreamWriter("Settings.dat")
        objWriter.WriteLine(filename)
        objWriter.WriteLine(TextBox1.Text)
        objWriter.Close()
        Form1.Show()
        Me.Hide()
    End Sub


    Private Sub resultsButton_Click(sender As Object, e As EventArgs) Handles resultsButton.Click
        Form3.Show()

        If Not File.Exists(System.IO.Path.GetFileNameWithoutExtension(filename) & "results.dat") Then
            MessageBox.Show("No results available.")
        Else
            Form3.Show()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If IsNumeric(TextBox1.Text) Then
            If TextBox1.Text <= Questions.Count Then
                beginButton.Enabled = True
            End If
        Else
            beginButton.Enabled = False
        End If
    End Sub
End Class