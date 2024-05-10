Public Class Form1
    Public push As Integer
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.残り = 0 Then
            Me.Show()
            Form2.ShowDialog()
        Else
            Label1.Text = My.Settings.残り
            Text = My.Settings.コース
            push = My.Settings.残り
        End If
    End Sub

    Private randomize As Integer

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        push = push - 1
        Label1.Text = push
        Dim rand As New Random
        Dim items(20) As String
        randomize = rand.Next(1, 20)

        items(1) = "1UPキノコ"
        items(2) = "折れたクレヨン"
        items(3) = "ホットケーキ"
        items(4) = "ゴミ"
        items(5) = "ボタン"
        items(6) = "段ボール"
        items(7) = "インク"
        items(8) = "ティッシュ"
        items(9) = "ハンカチ"
        items(10) = "便座"
        items(11) = "涙"
        items(12) = "漢字ドリル"
        items(13) = "ガソリン"
        items(14) = "パン"
        items(15) = "カップラーメン"
        items(16) = "シール"
        items(17) = "1万回のタマゴ"
        items(18) = "100回のタマゴ"
        items(19) = "10万回のタマゴ"
        items(20) = "100万回のタマゴ"

        If push = 0 Then
            If randomize <= 16 Then
                MsgBox("おめでとう！タマゴが開いたよ！" & vbCrLf & "中には" & items(randomize) & "が入ってたよ！", vbInformation, "クリア")
                My.Settings.残り = push
                Form2.ShowDialog()
            Else
                MsgBox("おめでとう！タマゴが開いたよ！" & vbCrLf & "中には" & items(randomize) & "が入ってたよ！", vbInformation, "クリア?")
                If randomize = 17 Then
                    Text = "1万回のタマゴ"
                    push = 10000
                    Label1.Text = push
                    My.Settings.コース = "1万回のタマゴ"
                    My.Settings.残り = 10000
                ElseIf randomize = 18 Then
                    Text = "100回のタマゴ"
                    push = 100
                    Label1.Text = push
                    My.Settings.コース = "100回のタマゴ"
                    My.Settings.残り = 100
                ElseIf randomize = 19 Then
                    Text = "10万回のタマゴ"
                    push = 100000
                    Label1.Text = push
                    My.Settings.コース = "10万回のタマゴ"
                    My.Settings.残り = 100000
                ElseIf randomize = 20 Then
                    Text = "100万回のタマゴ"
                    push = 1000000
                    Label1.Text = push
                    My.Settings.コース = "100万回のタマゴ"
                    My.Settings.残り = 1000000
                End If
            End If
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dialog1.ShowDialog()
        e.Cancel = True
    End Sub
End Class
