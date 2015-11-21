Imports System.IO

Public Class Form1

    Dim filename As String
    Dim userName As String
    Dim input() As String
    Dim totalQuestions As Integer = 0
    Dim questionNum As Integer = 1
    Dim fontName As String = "Times New Roman"
    Dim fontSize As Integer = 12
    Dim questionOrder() As Integer

    Dim Questions As New List(Of Question)

    Structure Question
        Dim questionName As String
        Dim answer1 As String
        Dim answer2 As String
        Dim answer3 As String
        Dim answer4 As String
        Dim correct As Integer
    End Structure


    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click

        If File.Exists("Settings.dat") Then
            Dim read As StreamReader = New StreamReader("Settings.dat")
            Dim result = read.ReadLine()
            totalQuestions = CInt(read.ReadLine())
            Dim loadQuestion As New Question
            Dim currentrow As String
            If result <> "" Then
                filename = result

                Dim reader As StreamReader = New StreamReader(filename)

                Do While reader.Peek() >= 0
                    currentrow = reader.ReadLine()
                    loadQuestion.questionName = currentrow
                    currentrow = reader.ReadLine()
                    loadQuestion.answer1 = currentrow
                    currentrow = reader.ReadLine()
                    loadQuestion.answer2 = currentrow
                    currentrow = reader.ReadLine()
                    loadQuestion.answer3 = currentrow
                    currentrow = reader.ReadLine()
                    loadQuestion.answer4 = currentrow
                    currentrow = reader.ReadLine()
                    loadQuestion.correct = CInt(Int(currentrow))
                    Questions.Add(loadQuestion)
                    ListBox1.Items.Add(Questions(0).questionName)
                Loop
                reader.Close()
                title.Text = System.IO.Path.GetFileNameWithoutExtension(filename)
                nameTextBox.Visible = True
                numLabel.Visible = False
                qLabel.Text = "Please enter your name."
                beginButton.Visible = True
                beginButton.Text = "Start Quiz"
            End If
        Else
            qLabel.Text = "No quiz available. Contact your teacher for help."
        End If

    End Sub
    Private Sub nameTextBox_TextChanged(sender As Object, e As EventArgs) Handles nameTextBox.TextChanged
        If nameTextBox.Text.Length > 0 Then
            beginButton.Enabled = True
        Else
            beginButton.Enabled = False
        End If
    End Sub

    Private Sub beginButton_Click(sender As Object, e As EventArgs) Handles beginButton.Click
        numLabel.Visible = True
        qLabel.Visible = True
        userName = nameTextBox.Text
        nameLabel.Visible = False
        nameTextBox.Visible = False
        nameTextBox.Clear()
        RadioButton1.Visible = True
        RadioButton2.Visible = True
        RadioButton3.Visible = True
        RadioButton4.Visible = True
        beginButton.Visible = False
        previousButton.Visible = True
        nextButton.Visible = True
        previousButton.Enabled = False
        nextButton.Enabled = False
        RadioButton.PerformClick()
        RandomizeQuestions()
        qLabel.Text = Questions(0).questionName
        RadioButton1.Text = Questions(questionNum - 1).answer1
        RadioButton2.Text = Questions(questionNum - 1).answer2
        RadioButton3.Text = Questions(questionNum - 1).answer3
        RadioButton4.Text = Questions(questionNum - 1).answer4
        beginButton.Visible = False
        ReDim input(Questions.Count)
        ReDim questionOrder(Questions.Count)
    End Sub

    Public Sub RandomizeQuestions()
        Dim counter As Integer
        Dim newPosition As Integer
        Dim rand As New Random
        Dim tempObject As Object

        For counter = 0 To Questions.Count - 1
            newPosition = rand.Next(0, Questions.Count - 1)

            tempObject = Questions(counter)
            Questions(counter) = Questions(newPosition)
            Questions(newPosition) = tempObject
        Next counter

    End Sub

    Public Sub saveSelection()
        If RadioButton1.Checked = True Then
            input(questionNum - 1) = 1
            nextButton.Enabled = True
        ElseIf RadioButton2.Checked = True Then
            input(questionNum - 1) = 2
            nextButton.Enabled = True
        ElseIf RadioButton3.Checked = True Then
            input(questionNum - 1) = 3
            nextButton.Enabled = True
        ElseIf RadioButton4.Checked = True Then
            input(questionNum - 1) = 4
            nextButton.Enabled = True
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        saveSelection()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        saveSelection()
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        saveSelection()
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        saveSelection()
    End Sub

    Private Sub nextButton_Click(sender As Object, e As EventArgs) Handles nextButton.Click
        If nextButton.Text = "Finish" Then
            RandomizeQuestions()
            questionNum = 1
            resultsLabel.Visible = False
            percentLabel.Visible = False
            fractionLabel.Visible = False
            nextButton.Text = "Next"
            nextButton.Enabled = False
            beginButton.Text = "Begin"
            qLabel.Text = "Please enter your name."
            RadioButton1.Visible = False
            RadioButton2.Visible = False
            RadioButton3.Visible = False
            RadioButton4.Visible = False
            beginButton.Visible = True
            Label3.Visible = False
            numLabel.Visible = False
            nameTextBox.Visible = True
            nameLabel.Visible = True

        ElseIf questionNum = totalQuestions - 1 Then
            nextButton.Text = "View Results"
            '...
            nextButton.Enabled = False
            previousButton.Enabled = True
            RadioButton.PerformClick()
            questionNum += 1
            numLabel.Text = CStr(questionNum) & "."
            qLabel.Text = Questions(questionNum - 1).questionName
            RadioButton1.Text = Questions(questionNum - 1).answer1
            RadioButton2.Text = Questions(questionNum - 1).answer2
            RadioButton3.Text = Questions(questionNum - 1).answer3
            RadioButton4.Text = Questions(questionNum - 1).answer4
            '...
        ElseIf questionNum >= totalQuestions Then
            'gradeQuiz()
            '
            'qLabel.Visible = False
            RadioButton1.Visible = False
            RadioButton2.Visible = False
            RadioButton3.Visible = False
            RadioButton4.Visible = False
            qLabel.Text = "COMPLETE. Your scores have been saved."
            resultsLabel.Visible = True
            percentLabel.Visible = True
            fractionLabel.Visible = True
            Dim Corrects As Integer = 0
            numLabel.Visible = False

            Dim counter As Integer = 0
            For Each q In Questions
                If q.correct = input(counter) Then
                    ListBox1.Items.Add("correct")
                    Corrects += 1
                Else
                    ListBox1.Items.Add("incorrect")
                End If
                counter += 1
            Next

            percentLabel.Text = CStr((Corrects / totalQuestions) * 100) & "%"
            fractionLabel.Text = Corrects & " / " & totalQuestions
            nextButton.Text = "Finish"
            previousButton.Enabled = False

            If Not File.Exists(title.Text & "results.dat") Then
                ' Create a file to write to. 
                Using sw As StreamWriter = File.CreateText(title.Text & "results.dat")
                    sw.WriteLine(userName)
                    sw.WriteLine(percentLabel.Text)
                    sw.WriteLine(fractionLabel.Text)
                End Using
            Else
                Using sw As StreamWriter = File.AppendText(title.Text & "results.dat")
                    sw.WriteLine(userName)
                    sw.WriteLine(percentLabel.Text)
                    sw.WriteLine(fractionLabel.Text)
                End Using
            End If
        Else
            nextButton.Enabled = False
            previousButton.Enabled = True
            RadioButton.PerformClick()
            questionNum += 1
            numLabel.Text = CStr(questionNum) & "."
            qLabel.Text = Questions(questionNum - 1).questionName
            RadioButton1.Text = Questions(questionNum - 1).answer1
            RadioButton2.Text = Questions(questionNum - 1).answer2
            RadioButton3.Text = Questions(questionNum - 1).answer3
            RadioButton4.Text = Questions(questionNum - 1).answer4
        End If
    End Sub

    Private Sub prevButton_Click(sender As Object, e As EventArgs) Handles previousButton.Click
        If questionNum > 1 Then
            nextButton.Text = "Next"
            nextButton.Enabled = False
            RadioButton.PerformClick()
            questionNum -= 1
            numLabel.Text = ToString(questionNum) & "."
            qLabel.Text = Questions(questionNum - 1).questionName
            RadioButton1.Text = Questions(questionNum - 1).answer1
            RadioButton2.Text = Questions(questionNum - 1).answer2
            RadioButton3.Text = Questions(questionNum - 1).answer3
            RadioButton4.Text = Questions(questionNum - 1).answer4
            If questionNum = 1 Then
                previousButton.Enabled = False
            End If
        End If
    End Sub

    Public Sub setfonts()
        numLabel.Font = New Font(fontName, fontSize, FontStyle.Regular)
        qLabel.Font = New Font(fontName, fontSize, FontStyle.Regular)
        RadioButton1.Font = New Font(fontName, fontSize, FontStyle.Regular)
        RadioButton2.Font = New Font(fontName, fontSize, FontStyle.Regular)
        RadioButton3.Font = New Font(fontName, fontSize, FontStyle.Regular)
        RadioButton4.Font = New Font(fontName, fontSize, FontStyle.Regular)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SegoeUIToolStripMenuItem.Checked = True
        MediumToolStripMenuItem.Checked = True
        Me.Hide()
        Form2.Show()
        OpenToolStripMenuItem.PerformClick()
        OpenToolStripMenuItem.Visible = False
        ' Add any initialization after the InitializeComponent() call.
        ' This allows for key presses to be recognized while the form is in focus.
        Me.KeyPreview = True
        Form2.Hide()
        Me.AllowTransparency = True
        TransparencyKey = Color.Red

        For Each element In Rectangles
            element.free = 0
        Next
    End Sub

    Private Sub SegoeUIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SegoeUIToolStripMenuItem.Click
        fontName = "Segoe UI"
        SegoeUIToolStripMenuItem.Checked = True
        TimesNewRomanToolStripMenuItem.Checked = False
        ComicSansToolStripMenuItem.Checked = False
        setfonts()
    End Sub

    Private Sub TimesNewRomanToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles TimesNewRomanToolStripMenuItem.Click
        fontName = "Times New Roman"
        TimesNewRomanToolStripMenuItem.Checked = True
        SegoeUIToolStripMenuItem.Checked = False
        ComicSansToolStripMenuItem.Checked = False
        setfonts()
    End Sub

    Private Sub ComicSansToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ComicSansToolStripMenuItem.Click
        fontName = "Comic Sans MS"
        ComicSansToolStripMenuItem.Checked = True
        TimesNewRomanToolStripMenuItem.Checked = False
        SegoeUIToolStripMenuItem.Checked = False
        setfonts()
    End Sub

    Private Sub DarkToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub LightToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RandomToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub LargeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LargeToolStripMenuItem.Click
        fontSize = 13
        MediumToolStripMenuItem.Checked = False
        SmallToolStripMenuItem.Checked = False
        setfonts()
    End Sub

    Private Sub MediumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MediumToolStripMenuItem.Click
        fontSize = 11
        LargeToolStripMenuItem.Checked = False
        SmallToolStripMenuItem.Checked = False
        setfonts()
    End Sub

    Private Sub SmallToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SmallToolStripMenuItem.Click
        fontSize = 9
        MediumToolStripMenuItem.Checked = False
        LargeToolStripMenuItem.Checked = False
        setfonts()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Application.Exit()
    End Sub



    Dim keyorder() As Object = {System.Windows.Forms.Keys.Up, System.Windows.Forms.Keys.Up, _
                                        System.Windows.Forms.Keys.Down, System.Windows.Forms.Keys.Down, _
                                        System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.Right, _
                                        System.Windows.Forms.Keys.Left, System.Windows.Forms.Keys.Right, _
                                        System.Windows.Forms.Keys.B, System.Windows.Forms.Keys.A}
    Dim index As Integer = 0
    Dim sequence() As Boolean = {False, False, False, False, False, False, False, False, False, False}

    Public Sub New()
        InitializeComponent()
        Me.KeyPreview = True
    End Sub

    Private Sub Form_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyUp
        If secretLabel.Text = "Konami Code!" Then
            If e.KeyCode = System.Windows.Forms.Keys.Left Then
                Pdirection = 1
            End If
            If e.KeyCode = System.Windows.Forms.Keys.Right Then
                Pdirection = 2
            End If
            If e.KeyCode = System.Windows.Forms.Keys.Up Then
                Pdirection = 3
            End If
            If e.KeyCode = System.Windows.Forms.Keys.Down Then
                Pdirection = 4
            End If
        End If
        If index < 9 And sequence(index) = False And e.KeyCode = keyorder(index) Then
            sequence(index) = True
            index += 1
        ElseIf index = 9 And e.KeyCode = keyorder(index) Then
            secretLabel.Visible = True
            secretLabel.Text = "Konami Code!"
            Button1.Visible = True
            Button1.PerformClick()
        Else
            index = 0
            For i As Integer = 0 To sequence.Length - 1
                sequence(i) = False
            Next
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
    Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
    Dim random As New Random()
    Dim start As Integer
    Dim myBrush As New SolidBrush(Color.ForestGreen)
    Dim clearBrush As New SolidBrush(Color.Red)
    Dim Counter As Integer

    Public Structure Rect
        Dim colors As Color
        Dim speed As Integer
        Dim free As Integer
        Dim xpos As Integer
        Dim ypos As Integer
        Dim width As Integer
        Dim height As Integer
    End Structure

    Dim Rectangles(100) As Rect
    Dim Player As Rect
    Dim Pdirection As Integer = 0
    Dim isHorizontalCollision As Boolean = False
    Dim isVerticalCollision As Boolean = False
    Dim score = 0

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button3.Visible = True
        Player.colors = Color.Black
        Player.speed = 1
        Player.xpos = (screenWidth / 2) - 15
        Player.ypos = screenHeight - 50
        Player.height = 30
        Player.width = 30

        Button1.Visible = False
        Button2.Visible = False
        MenuStrip1.Visible = False
        Panel1.Visible = False
        Panel2.Visible = False
        Label1.Visible = False
        title.Visible = False
        Me.BackColor = Color.Red
        Timer1.Enabled = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.WindowState = FormWindowState.Maximized
        For Each element In Rectangles
            element.free = True
        Next

    End Sub
    Dim e As EventArgs
    Private Sub moveRect(rectangle As Integer)
        score += 1
        Button3.Text = score
        Me.CreateGraphics.FillRectangle(clearBrush, Player.xpos, Player.ypos, Player.width, Player.height)
        If Pdirection = 1 And Player.xpos > 0 Then
            Player.xpos = Player.xpos - Player.speed
        ElseIf Pdirection = 2 And Player.xpos < (screenWidth - 30) Then
            Player.xpos = Player.xpos + Player.speed
        ElseIf Pdirection = 3 And Player.ypos > 0 Then
            Player.ypos = Player.ypos - Player.speed
        ElseIf Pdirection = 4 And Player.ypos < (screenHeight - 30) Then
            Player.ypos = Player.ypos + Player.speed
        End If

        Me.CreateGraphics.FillRectangle(Brushes.Black, Player.xpos, Player.ypos, Player.width, Player.height)

        Me.CreateGraphics.FillRectangle(clearBrush, Rectangles(Counter).xpos, Rectangles(Counter).ypos, Rectangles(Counter).width, Rectangles(Counter).speed)
        Rectangles(Counter).ypos += Rectangles(Counter).speed
        myBrush.Color = Color.FromArgb(50, Rectangles(Counter).colors.R, Rectangles(Counter).colors.G, Rectangles(Counter).colors.B)
        Me.CreateGraphics().FillRectangle(myBrush, Rectangles(Counter).xpos, Rectangles(Counter).ypos, Rectangles(Counter).width, Rectangles(Counter).height)
        If Rectangles(Counter).ypos > screenHeight Then
            Rectangles(Counter).free = 0
        End If

        If Player.xpos > Rectangles(Counter).xpos And Player.xpos < (Rectangles(Counter).xpos + Rectangles(Counter).width) Then
            If Player.ypos > Rectangles(Counter).ypos And Player.ypos < (Rectangles(Counter).ypos + Rectangles(Counter).height) Then
                stopScreensaver(e)
            ElseIf (Player.ypos + 30) > Rectangles(Counter).ypos And (Player.ypos + 30) < (Rectangles(Counter).ypos + Rectangles(Counter).height) Then
                stopScreensaver(e)
            End If
        ElseIf (Player.xpos + 30) > Rectangles(Counter).xpos And (Player.xpos + 30) < (Rectangles(Counter).xpos + Rectangles(Counter).width) Then
            If Player.ypos > Rectangles(Counter).ypos And Player.ypos < (Rectangles(Counter).ypos + Rectangles(Counter).height) Then
                stopScreensaver(e)
            ElseIf (Player.ypos + 30) > Rectangles(Counter).ypos And (Player.ypos + 30) < (Rectangles(Counter).ypos + Rectangles(Counter).height) Then
                stopScreensaver(e)
            End If
        End If

    End Sub


    Dim difficulty As Integer = 1000000

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Counter = 0
        For Each element In Rectangles

            If element.free = 1 Then
                moveRect(Counter)
            Else
                start = random.Next(-10, difficulty)
                If start < screenWidth Then
                    UpdateElement(Counter, start)
                End If

            End If
            Counter += 1
        Next
        If difficulty > 10000 Then
            difficulty -= 1000
        End If

        start = random.Next(0, 100)
    End Sub

    Public Sub UpdateElement(rect As Integer, position As Integer)
        Rectangles(Counter).xpos = start
        Rectangles(Counter).height = random.Next(30, 300)
        Rectangles(Counter).width = random.Next(30, 300)
        Rectangles(Counter).ypos = 0 - Rectangles(Counter).height
        Rectangles(Counter).free = 1
        Rectangles(Counter).speed = random.Next(1, 10)
        start = random.Next(10, 120)
        Rectangles(Counter).colors = Color.FromArgb(CInt(Int((254 * Rnd()) + 0)), CInt(Int((254 * Rnd()) + 0)), CInt(Int((254 * Rnd()) + 0)))
        'Rectangles(Counter).colors = Color.FromArgb(start, start * 0.1, start * 0.1)
    End Sub

    Public Sub stopScreensaver(e As EventArgs)
        Pdirection = 0
        Timer1.Enabled = False
        Panel1.Visible = True
        Panel2.Visible = True
        Label1.Visible = True
        title.Visible = True
        MenuStrip1.Visible = True
        Button3.Visible = False
        Button1.Visible = False
        Button2.Visible = True
        Me.WindowState = FormWindowState.Normal
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Fixed3D
        Me.BackColor = Color.WhiteSmoke
    End Sub

    Private Sub Form1_dblclick(sender As Object, e As EventArgs) Handles Me.DoubleClick
        stopScreensaver(e)
    End Sub




End Class
