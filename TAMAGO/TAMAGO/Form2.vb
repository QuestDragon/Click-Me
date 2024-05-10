Public Class Form2
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ComboBox1.SelectedIndex = 0 Then
            Form1.Text = "100回のタマゴ"
            My.Settings.コース = "100回のタマゴ"
            My.Settings.残り = 100
            Form1.Label1.Text = My.Settings.残り
            Form1.push = My.Settings.残り
            Close()
        ElseIf ComboBox1.SelectedIndex = 1 Then
            Form1.Text = "1000回のタマゴ"
            My.Settings.コース = "1000回のタマゴ"
            My.Settings.残り = 1000
            Form1.Label1.Text = My.Settings.残り
            Form1.push = My.Settings.残り
            Close()
        ElseIf ComboBox1.SelectedIndex = 2 Then
            Form1.Text = "1万回のタマゴ"
            My.Settings.コース = "1万回のタマゴ"
            My.Settings.残り = 10000
            Form1.Label1.Text = My.Settings.残り
            Form1.push = My.Settings.残り
            Close()
        ElseIf ComboBox1.SelectedIndex = 3 Then
            Form1.Text = "10万回のタマゴ"
            My.Settings.コース = "10万回のタマゴ"
            My.Settings.残り = 100000
            Form1.Label1.Text = My.Settings.残り
            Form1.push = My.Settings.残り
            Close()
        ElseIf ComboBox1.SelectedIndex = 4 Then
            Form1.Text = "100万回のタマゴ"
            My.Settings.コース = "100万回のタマゴ"
            My.Settings.残り = 1000000
            Form1.Label1.Text = My.Settings.残り
            Form1.push = My.Settings.残り
            Close()
        ElseIf ComboBox1.SelectedIndex = 5 Then
            Form1.Text = "1000万回のタマゴ"
            My.Settings.コース = "1000万回のタマゴ"
            My.Settings.残り = 10000000
            Form1.Label1.Text = My.Settings.残り
            Form1.push = My.Settings.残り
            Close()
        ElseIf ComboBox1.SelectedIndex = 6 Then
            Form1.Text = "1億回のタマゴ"
            My.Settings.コース = "1億回のタマゴ"
            My.Settings.残り = 100000000
            Form1.Label1.Text = My.Settings.残り
            Form1.push = My.Settings.残り
            Close()
        ElseIf ComboBox1.SelectedIndex = 7 Then
            Form1.Text = TextBox1.Text & "回のタマゴ"
            My.Settings.コース = TextBox1.Text & "回のタマゴ"
            My.Settings.残り = Val(TextBox1.Text)
            Form1.Label1.Text = My.Settings.残り
            Form1.push = My.Settings.残り
            Close()
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex = 7 Then
            TextBox1.Enabled = True
            Label1.Visible = True
        Else
            TextBox1.Text = ""
            TextBox1.Enabled = False
            Label1.Visible = False
        End If
    End Sub

    'TextBox1のKeyPressイベントハンドラ
    Private Sub txt_Keypress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        '0～9と、バックスペース以外の時は、イベントをキャンセルする
        If (e.KeyChar < "0"c OrElse "9"c < e.KeyChar) AndAlso
            e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        TextBox1.Focus()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If Len(TextBox1.Text) >= 1 Then
            Label1.Visible = False
        Else
            Label1.Visible = True
        End If
    End Sub

    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If My.Settings.残り = 0 Then
            Dialog1.ShowDialog()
            e.Cancel = True
        End If
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
    End Sub
End Class