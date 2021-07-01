Public Class Form2
    Dim apply_called As Boolean = False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MsgBox("Point size determines the size of each bar of the barcode." & vbNewLine + "Change only if the scanner isn't picking the barcode or because the printer can't print the barcode." & vbNewLine & vbNewLine + "The default is " & Chr(34) + "1" & Chr(34) + "(1 pixel wide).", MsgBoxStyle.Information, "Help")
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Class1.color_update()
        NumericUpDown1.Value = My.Settings.point_size
        NumericUpDown2.Value = My.Settings.barcode_height

        If My.Settings.dark_theme = True Then
            RadioButton4.Checked = True
        Else
            RadioButton3.Checked = True
        End If

        If My.Settings.instant = True Then
            RadioButton1.Checked = True
        Else
            RadioButton2.Checked = True
        End If

        Button2.Select()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        apply_called = True

        Me.Close()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton2.Checked Then
            RadioButton1.Checked = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        MsgBox("If enabled the barcode will be generated instantly, without the need of pressing " & Chr(34) + "Generate" & Chr(34) + "." & vbNewLine + "Its use will increase the CPU usage." & vbNewLine & vbNewLine + "The default Is " & Chr(34) + "Disabled" & Chr(34) + ".", MsgBoxStyle.Information, "Help")
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton3.Checked Then
            RadioButton4.Checked = False
        End If
    End Sub

    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (Not (My.Settings.point_size = NumericUpDown1.Value)) OrElse (Not (My.Settings.barcode_height = NumericUpDown2.Value)) OrElse (RadioButton2.Checked = My.Settings.instant) OrElse (RadioButton3.Checked = My.Settings.dark_theme) Then
            If apply_called = True Then
                If MsgBox("Are you sure you want to change the settings?", MsgBoxStyle.YesNo, "Confirm exit") = MsgBoxResult.Yes Then
                    My.Settings.instant = RadioButton1.Checked
                    My.Settings.point_size = NumericUpDown1.Value
                    My.Settings.barcode_height = NumericUpDown2.Value

                    If RadioButton3.Checked = My.Settings.dark_theme Then
                        My.Settings.dark_theme = RadioButton4.Checked
                        Class1.color_update()
                    End If

                    My.Settings.Save()
                End If
            Else
                If MsgBox("Are you sure you want to exit the settings without saving?" & vbNewLine & vbNewLine + "All pending changes will be lost...", MsgBoxStyle.YesNo, "Confirm exit") = MsgBoxResult.No Then
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        MsgBox("Barcode height determines the height of each bar of the barcode." & vbNewLine + "Changing it will not affect the barcode in any way other than aesthetically." & vbNewLine & vbNewLine + "The default is " & Chr(34) + "110" & Chr(34) + "(110 pixel wide).", MsgBoxStyle.Information, "Help")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        NumericUpDown1.Value = 1
        NumericUpDown2.Value = 110
        RadioButton2.Checked = True
        RadioButton3.Checked = True
    End Sub
End Class