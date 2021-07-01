Public Class Class1
    Public Shared Sub color_update()
        If My.Settings.dark_theme Then
            Form1.BackColor = SystemColors.ControlDarkDark
            Form1.MenuStrip1.BackColor = SystemColors.ControlDarkDark
            Form2.BackColor = SystemColors.ControlDarkDark
            AboutBox1.BackColor = SystemColors.ControlDarkDark
            Form3.BackColor = SystemColors.ControlDarkDark
            Form1.PictureBox2.BackColor = SystemColors.ControlDarkDark
            Form1.PictureBox3.BackColor = SystemColors.ControlDarkDark
        Else
            Form1.BackColor = SystemColors.Control
            Form1.MenuStrip1.BackColor = SystemColors.Control
            Form2.BackColor = SystemColors.Control
            AboutBox1.BackColor = SystemColors.Control
            Form3.BackColor = SystemColors.Control
            Form1.PictureBox2.BackColor = SystemColors.Control
            Form1.PictureBox3.BackColor = SystemColors.Control
        End If
    End Sub
End Class
