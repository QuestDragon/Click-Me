Option Explicit On
Public Class Form1
    Dim A As Integer
    Dim B As Integer
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        A = 0
        ComboBox1.Items.Add("10秒")
        ComboBox1.Items.Add("30秒")
        ComboBox1.Items.Add("60秒")
        ComboBox1.Items.Add("未設定")
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        ComboBox1.SelectedIndex = 3
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        A = A + 1
        Label2.Text = A
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim Ended As Integer
        Ended = MsgBox("終了しますか？", 32 + 4 + vbDefaultButton2, "Click Me!!!")
        Select Case Ended '押されたボタンの確認
            Case vbYes
                End
        End Select
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label2.Text = 0
        A = 0
        If ComboBox1.SelectedIndex <> 3 Then 'タイマーを開始した場合、Startボタンを無効化
            Button1.Enabled = False
        End If
        Button2.Enabled = True 'Attackボタンの有効化
        Button2.Focus()
        If ComboBox1.SelectedIndex = 0 Then
            Call count3()
        ElseIf Combobox1.SelectedIndex = 1 Then
            Call count2()
        ElseIf ComboBox1.SelectedIndex = 2 Then
            Call count()
        Else
            Button2.Enabled = True
        End If
    End Sub
    Sub count()
        Dim t As Long
        Dim endTime As Date
        Dim rtn As Integer
        t = 60
        endTime = DateAdd("s", t, Now)
        Do Until t <= 0
            t = DateDiff("s", Now, endTime)
            Label3.Text = " left: " & t
            My.Application.DoEvents()
        Loop
        rtn = MsgBox("時間切れ！！", 16, "Click Me!!!")
        Select Case rtn '押されたボタンの確認
            Case vbOK
                Button2.Enabled = False
                Label3.Text = "left: -"
                Button1.Enabled = True
        End Select
    End Sub
    Sub count2()
        Dim t As Long
        Dim endTime As Date
        Dim rtn As Integer
        t = 30
        endTime = DateAdd("s", t, Now)
        Do Until t <= 0
            t = DateDiff("s", Now, endTime)
            Label3.Text = " left: " & t
            My.Application.DoEvents()
        Loop
        rtn = MsgBox("時間切れ！！", 16, "Click Me!!!")
        Select Case rtn '押されたボタンの確認
            Case vbOK
                Button2.Enabled = False
                Label3.Text = "left: -"
                Button1.Enabled = True
        End Select
    End Sub
    Sub count3()
        Dim t As Long
        Dim endTime As Date
        Dim rtn As Integer
        t = 10
        endTime = DateAdd("s", t, Now)
        Do Until t <= 0
            t = DateDiff("s", Now, endTime)
            Label3.Text = " left: " & t
            My.Application.DoEvents()
        Loop
        rtn = MsgBox("時間切れ！！", 16, "Click Me!!!")
        Select Case rtn '押されたボタンの確認
            Case vbOK
                Button2.Enabled = False
                Label3.Text = "left: -"
                Button1.Enabled = True
        End Select
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 3 Then
            Button1.Text = "Reset"
        Else
            Button1.Text = "Start"
        End If
    End Sub
    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim confirm As Integer = MsgBox("終了しますか？", 32 + 4 + vbDefaultButton2, "Click Me!!!")
        If confirm = vbNo Then
            e.Cancel = True
        End If
    End Sub
End Class
