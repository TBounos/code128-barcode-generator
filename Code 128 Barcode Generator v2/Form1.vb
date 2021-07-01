Imports System.ComponentModel
Imports System.Drawing.Printing

Public Class Form1
    Dim out_str As String = "11010010000"       'The final string with the start B symbol
    Dim tableB As Boolean = True
    Dim let_cnter As Integer = 1
    Dim check_digit As Integer = 104         'Starting with the tableB value

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If MsgBox("Are you sure you want to exit?", MsgBoxStyle.YesNo, "Confirm exit") = MsgBoxResult.Yes Then
            Application.Exit()
            End
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem1.Click
        AboutBox1.Show()
    End Sub

    Friend WithEvents prntDoc As New PrintDocument()

    Private Sub prntDoc_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles prntDoc.PrintPage
        e.Graphics.DrawImage(PictureBox4.Image, 0, 0)
    End Sub

    Private Sub print_sub()
        If PictureBox4.Image IsNot Nothing Then
            Try
                Dim prnDialog As New PrintDialog()
                prnDialog.Document = prntDoc

                If prnDialog.ShowDialog = DialogResult.OK Then
                    prntDoc.Print()
                End If
            Catch
                MsgBox("An error occured while trying to print..." & vbNewLine & vbNewLine + "Check your connection with the printer and try again later...", MsgBoxStyle.Critical, "Cannot complete action")
            End Try
        Else
            MsgBox("Error: no image to print...", MsgBoxStyle.Critical, "Cannot complete action")
        End If
    End Sub

    Private Sub print_prv()
        If PictureBox4.Image IsNot Nothing Then
            Try
                Dim prnPreview As New PrintPreviewDialog()
                prnPreview.Document = prntDoc
                prnPreview.ShowDialog()
            Catch
                MsgBox("An error occured while trying to print..." & vbNewLine & vbNewLine + "Check your connection with the printer and try again later...", MsgBoxStyle.Critical, "Cannot complete action")
            End Try
        Else
            MsgBox("Error: no image to preview...", MsgBoxStyle.Critical, "Cannot complete action")
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        print_sub()
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        print_prv()
    End Sub

    Private Sub paste_text()
        If TextBox1.SelectionLength > 0 Then
            TextBox1.SelectedText = Clipboard.GetText()
        Else
            TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart, Clipboard.GetText())
            TextBox1.SelectionStart = TextBox1.SelectionStart + Clipboard.GetText().Length
        End If
    End Sub

    Private Sub cut_text()
        If TextBox1.Text.Length > 0 Then
            Clipboard.Clear()
            If TextBox1.SelectionLength > 0 Then
                Clipboard.SetText(TextBox1.SelectedText)
                TextBox1.SelectedText = ""
            Else
                Clipboard.SetText(TextBox1.Text)
                TextBox1.Text = ""
            End If
        End If
    End Sub

    Private Sub copy_text()
        If TextBox1.Text.Length > 0 Then
            Clipboard.Clear()
            If TextBox1.SelectionLength > 0 Then
                Clipboard.SetText(TextBox1.SelectedText)
            Else
                Clipboard.SetText(TextBox1.Text)
            End If
        End If
    End Sub

    Private Sub copy()
        If Not PictureBox1.Image Is Nothing Then
            Clipboard.SetImage(PictureBox1.Image)
        Else
            MsgBox("Error: no image to copy...", MsgBoxStyle.Critical, "Cannot complete action")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        copy()
    End Sub

    Private Sub save_as()
        If Not PictureBox1.Image Is Nothing Then
            'Create a new SaveFileDialog
            Dim selection As New SaveFileDialog
            selection.Filter = "Png Image|*.png|Bmp Image|*.bmp|Jpg Image|*.jpg"
            selection.FileName = TextBox1.Text
            selection.AddExtension = True 'Make sure the extension is added to the FileName

            If selection.ShowDialog = DialogResult.OK Then 'If the user presses "Ok" button on the SaveFileDialog
                If selection.FilterIndex = 1 Then 'If user picks the "Jpg Image" format
                    PictureBox1.Image.Save(selection.FileName, Imaging.ImageFormat.Jpeg)
                ElseIf selection.FilterIndex = 2 Then 'If the user picks the "Bmp Image" format
                    PictureBox1.Image.Save(selection.FileName, Imaging.ImageFormat.Bmp)
                Else
                    PictureBox1.Image.Save(selection.FileName, Imaging.ImageFormat.Png)
                End If
            End If
        Else
            MsgBox("Error: no image to save...", MsgBoxStyle.Critical, "Cannot complete action")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        save_as()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        If MsgBox("Are you sure you want to exit?", MsgBoxStyle.YesNo, "Confirm exit") = MsgBoxResult.Yes Then
            Application.Exit()
            End
        End If
    End Sub

    Private Function num_cnter(x As Integer, str As String)      'Returns the number of integers in the string, following the first digit
        Dim cnter As Integer = 0
        Dim temp As Integer = x

        While temp <= TextBox1.Text.Length ' temp <= x + 3 AndAlso
            If Asc(Mid(str, temp, 1)) > 47 AndAlso Asc(Mid(str, temp, 1)) < 58 Then     'Is number or not
                cnter = cnter + 1                                                           'Number counter
            Else
                Exit While
            End If

            temp = temp + 1
        End While

        Return cnter
    End Function

    Private Sub smbl(str As String)                              'Adds values to generate the final string
        out_str = out_str & My.Resources.ResourceManager.GetObject("a" & Mid(str, 1, 2))
        check_digit = check_digit + (Mid(str, 1, 2) * let_cnter)
        let_cnter = let_cnter + 1

        If str.Length > 2 Then
            smbl(Mid(str, 3))
        End If
    End Sub

    Private Sub draw_barcode()
        Dim width As Integer = 739
        Dim height As Integer = 200

        While (out_str.Length * My.Settings.point_size) + 90 > width
            width = width + 50
            height = 183
        End While

        Dim barcode As New Bitmap(width, height)                        'Creating image for the barcode
        Dim barcodeGraphics As Graphics = Graphics.FromImage(barcode)   'Creating graphics variable to draw the barcode
        ' Dim barcode_print As New Bitmap(width, height)

        Dim x As Integer = (width / 2) - ((out_str.Length * My.Settings.point_size) / 2)    'Calculating the starting position of the barcode
        Dim y As Integer = 85 - (My.Settings.barcode_height / 2)

        barcodeGraphics.FillRectangle(Brushes.White, 0, 0, width, height)        'Make the background white

        For i = 1 To out_str.Length
            If Mid(out_str, i, 1) = 1 Then                                  'A bar is an 1
                barcodeGraphics.FillRectangle(Brushes.Black, x, y, My.Settings.point_size, My.Settings.barcode_height)
            End If

            x = x + My.Settings.point_size
        Next

        Dim textSize As Size                'Calculating the starting position of the barcode
        Dim temp_font_size As Integer = 19
        Do
            temp_font_size = temp_font_size - 1
            textSize = TextRenderer.MeasureText(TextBox1.Text, New Font("Microsoft Sans Serif", temp_font_size))
        Loop While (textSize.Width > (out_str.Length * My.Settings.point_size) + 90) AndAlso (temp_font_size > 1)
        barcodeGraphics.DrawString(TextBox1.Text, New Font("Microsoft Sans Serif", temp_font_size), Brushes.Black, ((width / 2) - ((textSize.Width - 10) / 2)), y + My.Settings.barcode_height)

        If (Screen.PrimaryScreen.Bounds.Width - 80) < width Then
            PictureBox1.Height = 183
        Else
            PictureBox1.Height = 200
        End If
        PictureBox1.Width = width
        PictureBox1.Image = barcode

        Dim barcode_print As New Bitmap(barcode)
        Dim barcodeGraphicsPrint As Graphics = Graphics.FromImage(barcode_print)
        barcodeGraphicsPrint.DrawString("Courtesy of Loukas Chaloftis", New Font("Microsoft Sans Serif", 14, FontStyle.Bold), Brushes.Black, ((width / 2) - 136), 0)
        barcodeGraphicsPrint.DrawString("Powered by Thanasis Bounos", New Font("Microsoft Sans Serif", 14, FontStyle.Bold), Brushes.Black, ((width / 2) - 136), 158)
        PictureBox4.Image = barcode_print
    End Sub

    Public Sub Barcode_main()
        Dim in_str As String = TextBox1.Text
        Dim i As Integer = 1
        Dim temp As Integer = 0

        For temp_counter = 1 To in_str.Length
            If Asc(Mid(in_str, temp_counter, 1)) < 32 OrElse Asc(Mid(in_str, temp_counter, 1)) > 126 Then
                MsgBox("Error: unacceptable character(s)..." & vbNewLine & vbNewLine + "For a list of the acceptable characters, refer to help(Help->Help).", MsgBoxStyle.Critical, "Error...")
                Return
            End If
        Next

        While i <= in_str.Length
            temp = num_cnter(i, in_str)

            If tableB Then                      'We are on table B, and deciding whether to switch or not(We always start from here, because B is default)
                If (temp >= 4) OrElse (temp = 2 AndAlso in_str.Length = 2) Then      'Check to see if it's worth going to C
                    tableB = False
                    If i = 1 Then                   'Start C symbol
                        out_str = "11010011100"
                        check_digit = 105
                    Else
                        out_str = out_str & "10111011110"   'Switch to C symbol
                        check_digit = check_digit + (99 * let_cnter)
                        let_cnter = let_cnter + 1
                    End If

                    If (temp Mod 2) = 1 Then
                        smbl(Mid(in_str, i, temp - 1))
                        i = i + temp - 1
                    Else
                        smbl(Mid(in_str, i, temp))
                        i = i + temp
                    End If
                Else                                'Else, continue B
                    out_str = out_str & My.Resources.ResourceManager.GetObject("a" & Asc(Mid(in_str, i, 1)) - 32)
                    check_digit = check_digit + ((Asc(Mid(in_str, i, 1)) - 32) * let_cnter)
                    let_cnter = let_cnter + 1
                    i = i + 1
                End If
            ElseIf temp >= 2 Then               'We are on table C, and staying
                If (temp Mod 2) = 1 Then
                    smbl(Mid(in_str, i, temp - 1))
                    i = i + temp - 1
                Else
                    smbl(Mid(in_str, i, temp))
                    i = i + temp
                End If
            Else                                'We are on table C, and switching to B
                tableB = True
                out_str = out_str & "10111101110"       'Switch to B symbol
                out_str = out_str & My.Resources.ResourceManager.GetObject("a" & Asc(Mid(in_str, i, 1)) - 32)
                check_digit = check_digit + (100 * let_cnter) + ((Asc(Mid(in_str, i, 1)) - 32) * (let_cnter + 1))
                let_cnter = let_cnter + 2
                i = i + 1
            End If
        End While

        'Calculating check digit & adding it to the string
        check_digit = check_digit Mod 103
        Dim check_digit_str As String = My.Resources.ResourceManager.GetObject("a" & check_digit)
        out_str = out_str & check_digit_str

        out_str = out_str & "1100011101011"     'Stop symbol

        draw_barcode()                          'Drawing barcode function

        'Reset variables
        out_str = "11010010000"
        tableB = True
        let_cnter = 1
        check_digit = 104
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text.Length < 1 Then
            MsgBox("Error: no value, field is empty...", MsgBoxStyle.Critical, "Invalid value")
        Else
            Me.Cursor = Cursors.WaitCursor
            Form2.Enabled = False
            AboutBox1.Enabled = False
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            TextBox1.Enabled = False
            Label5.Visible = False
            PictureBox3.Visible = False
            Label6.Visible = True
            PictureBox2.Visible = True
            Timer1.Stop()
            Timer2.Stop()

            Barcode_main()

            Form2.Enabled = True
            AboutBox1.Enabled = True
            Button1.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
            TextBox1.Enabled = True
            Resize_scr()
            Me.Cursor = Cursors.Default
            Label6.Visible = False
            PictureBox2.Visible = False
            Button3.Select()
            Timer1.Interval = 8000
            Timer1.Start()
            Label5.Visible = True
            PictureBox3.Visible = True
        End If
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        Form2.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form2.Show()
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Button1_Click(sender, e)
            e.Handled = True
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Class1.color_update()
        TextBox1.Select()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Timer1.Stop()
        Label5.Visible = False
        PictureBox3.Visible = False
        Timer2.Stop()

        If My.Settings.instant Then
            If TextBox1.Text.Length > 0 Then
                Barcode_main()
                Timer1.Interval = 9500
                Timer1.Start()
                Timer2.Start()
            End If
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label5.Visible = False
        PictureBox3.Visible = False
        Timer1.Stop()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Label5.Visible = True
        PictureBox3.Visible = True
        Resize_scr()
        Timer2.Stop()
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        save_as()
    End Sub

    Private Sub PrintToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles PrintToolStripMenuItem.Click
        print_sub()
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        print_prv()
    End Sub

    Private Sub CopyImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyImageToolStripMenuItem.Click
        copy()
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        Form3.Show()
    End Sub

    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
        cut_text()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        copy_text()
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        paste_text()
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        TextBox1.SelectAll()
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        copy()
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        print_sub()
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        print_prv()
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        save_as()
    End Sub

    Private Sub Resize_scr()
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width - 80

        If screenWidth > PictureBox1.Width Then     'if screen is bigger than the picturebox
            Me.Width = 779 + (PictureBox1.Width - 739)
            Panel1.Width = PictureBox1.Width
        Else                                        'if screen is smaller than the picturebox
            Me.Width = screenWidth
            Panel1.Width = Me.Width - 40
        End If

        Dim y As New Point(Me.Location)
        If y.X + Me.Width > screenWidth Then
            Dim temp As Integer = y.X - ((y.X + Me.Width) - screenWidth) - 40
            If temp < 40 Then
                temp = 40
            End If

            Me.Location = New Point(temp, y.Y)
        End If
    End Sub
End Class