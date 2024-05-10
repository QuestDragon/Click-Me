Imports System.Windows.Forms

Public Class Dialog1

    Private Sub Dialog1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim canvas As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim g As Graphics = Graphics.FromImage(canvas)
        g.DrawIcon(SystemIcons.Question, 0, 0)
        PictureBox1.Image = canvas
    End Sub

    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        My.Settings.残り = Form1.push
        My.Settings.コース = Form1.Text
        My.Settings.Save()
        End
    End Sub

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub
End Class
