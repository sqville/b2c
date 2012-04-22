Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Text

Namespace BitmapToCode
    ''' <summary>
    ''' Summary description for Form1.
    ''' </summary>
    Public Class frmBitmapToCode
        Inherits System.Windows.Forms.Form
        Private pictureBox1 As System.Windows.Forms.PictureBox
        Private openFileDialog1 As System.Windows.Forms.OpenFileDialog
        Private WithEvents btnGetFilePath As System.Windows.Forms.Button
        Private lblBitmapFile As System.Windows.Forms.Label
        Private txtHTML As System.Windows.Forms.TextBox
        Private lblHTML As System.Windows.Forms.Label
        Private txtBitmapFilePath As System.Windows.Forms.TextBox
        Private WithEvents btnLoad As System.Windows.Forms.Button
        Private WithEvents txtCopyToClipboard As System.Windows.Forms.Button
        Friend WithEvents lblCharCount As System.Windows.Forms.Label
        Friend WithEvents lblLineCount As System.Windows.Forms.Label
        Friend WithEvents nudTop As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblAdjustTop As System.Windows.Forms.Label
        Friend WithEvents lblAdjustLeft As System.Windows.Forms.Label
        Friend WithEvents nudLeft As System.Windows.Forms.NumericUpDown
        Friend WithEvents chkRelWidth As System.Windows.Forms.CheckBox
        Friend WithEvents chkRelHeight As System.Windows.Forms.CheckBox
        Friend WithEvents ckbLoadArray As System.Windows.Forms.CheckBox
        Friend WithEvents nudRight As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblAdjustRight As System.Windows.Forms.Label
        Friend WithEvents nudBottom As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblAdjustBottom As System.Windows.Forms.Label
        Friend WithEvents txtAdjustLeftVarName As System.Windows.Forms.TextBox
        Friend WithEvents txtAdjustRightVarName As System.Windows.Forms.TextBox
        Friend WithEvents txtAdjustTopVarName As System.Windows.Forms.TextBox
        Friend WithEvents txtAdjustBottomVarName As System.Windows.Forms.TextBox
        Friend WithEvents gbPaintFunctions As System.Windows.Forms.GroupBox
        Friend WithEvents rbwrtb As System.Windows.Forms.RadioButton
        Friend WithEvents rbwltb As System.Windows.Forms.RadioButton
        Friend WithEvents rblrtb As System.Windows.Forms.RadioButton
        Friend WithEvents rbhlrb As System.Windows.Forms.RadioButton
        Friend WithEvents rbwhrb As System.Windows.Forms.RadioButton
        Friend WithEvents rbwhlb As System.Windows.Forms.RadioButton
        Friend WithEvents rbwhrt As System.Windows.Forms.RadioButton
        Friend WithEvents rbhlrt As System.Windows.Forms.RadioButton
        Friend WithEvents rbwhlt As System.Windows.Forms.RadioButton
        Friend WithEvents lblFunctionDesc As System.Windows.Forms.Label
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Sub New()
            '
            ' Required for Windows Form Designer support
            '

            '
            ' TODO: Add any constructor code after InitializeComponent call
            '
            InitializeComponent()
        End Sub

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub
        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread()> _
        Private Shared Sub Main()
            Application.Run(New frmBitmapToCode())
        End Sub

#Region "Form Control Event Code"

        ''' <summary>
        '''   Get the location of the desired bitmap
        ''' </summary>
        ''' <param name="sender">the button</param>
        ''' <param name="e">empty</param>
        Private Sub btnGetFilePath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetFilePath.Click
            If openFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtBitmapFilePath.Text = openFileDialog1.FileName
            End If

        End Sub

        ''' <summary>
        '''   Load the image pointed at by the path in txtBitmapFilePath into the picturebox
        ''' </summary>
        ''' <param name="sender">button</param>
        ''' <param name="e">empty</param>
        Private Sub btnLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoad.Click
 ''testing github again
			Dim bitmap As Bitmap
            Dim strbHTML As New StringBuilder()
            'Dim intWidth As Integer
            Dim strPreviousPixelName As String = ""
            Dim strCurrentPixelName As String = ""
            Dim strNextPixelName As String = ""
            Dim iColumnCount As Integer = 1
            Dim blnRecording As Boolean = False
            Dim intLineStart As Integer = 0
            Dim strvWSign As String = ""
            Dim intvLeft As Integer = 0
            Dim blnLoadArray As Boolean = Me.ckbLoadArray.Checked
            Dim strPaintFunction As String = ""

            ' try to load the file using the path
            Try
                pictureBox1.Image = Image.FromFile(txtBitmapFilePath.Text)
                bitmap = New Bitmap(pictureBox1.Image)

            Catch ex As Exception
                MessageBox.Show(Me, "Loading of image failed: " + ex.ToString())
                Return
            End Try

            'Determine which Paint Function they are creating code for
            'Loop through all the radio buttons to see which is Checked
            For Each rbObj as RadioButton In Me.gbPaintFunctions.Controls
                If rbObj.Checked Then
                    strPaintFunction = rbObj.Text
                    Exit For
                End If
            Next rbObj


            strbHTML.Append("//<START>")
            strbHTML.AppendLine()

            For i As Integer = 0 To bitmap.Height - 1

                For j As Integer = 0 To bitmap.Width - 1

                    'SQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQ
                    'Example code
                    ' //**(Width, Height, Left, Top, vRGB1, vRGB2, vRGB3)  
                    ' <div style='width:" + vA[0] + "px;height:" + vA[1] + "px;position:absolute;overflow:hidden;left:" + vA[2] + "px;top:" + vA[3] + "px;background-color:" + "rgb(" + vA[4] + "," + vA[5] + "," + vA[6] + ")'></div>";

                    ' <Line Break> + "h.push(this._bdt(1, 1," + j + ", " + i + ", " + R + ", " + G + ", " + B + ");"
                    'SQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQSQ

                    ''set current pixel
                    strCurrentPixelName = Me.getPixelName(bitmap.GetPixel(j, i))

                    ''set next pixel
                    If j = bitmap.Width - 1 And i <> bitmap.Height - 1 Then
                        strNextPixelName = Me.getPixelName(bitmap.GetPixel(0, i + 1))
                    ElseIf j <> bitmap.Width - 1 And i <> bitmap.Height - 1 Then
                        strNextPixelName = Me.getPixelName(bitmap.GetPixel(j + 1, i))
                    ElseIf j <> bitmap.Width - 1 And i = bitmap.Height - 1 Then
                        strNextPixelName = Me.getPixelName(bitmap.GetPixel(j + 1, i))
                    Else
                        strNextPixelName = "END"
                    End If

                    ''append code string
                    If strNextPixelName <> "END" And j <> bitmap.Width - 1 Then
                        If strCurrentPixelName.Equals(strNextPixelName) Then
                            iColumnCount += 1
                        Else
                            strbHTML.Append("[" + iColumnCount.ToString() + ",1," + (((j + 1) + nudLeft.Value) - iColumnCount).ToString() + "," + (i + nudTop.Value).ToString() + "," + strCurrentPixelName + "],")
                            'strbHTML.Append(getArrayCodeLine(iColumnCount, strCurrentPixelName, j, i, strPaintFunction))
                            strbHTML.AppendLine()
                            iColumnCount = 1
                        End If

                    ElseIf strNextPixelName <> "END" And j = bitmap.Width - 1 Then
                        strbHTML.Append("[" + iColumnCount.ToString() + ",1," + (((j + 1) + nudLeft.Value) - iColumnCount).ToString() + "," + (i + nudTop.Value).ToString() + "," + strCurrentPixelName + "],")
                        strbHTML.AppendLine()
                        iColumnCount = 1
                    Else
                        strbHTML.Append("[" + iColumnCount.ToString() + ",1," + (((j + 1) + nudLeft.Value) - iColumnCount).ToString() + "," + (i + nudTop.Value).ToString() + "," + strCurrentPixelName + "],")
                        strbHTML.AppendLine()

                    End If

                Next

            Next

            strbHTML.Append("//<END>")

            txtHTML.Text = strbHTML.ToString()
            lblCharCount.Text = "Chars: " + txtHTML.TextLength.ToString()
            lblLineCount.Text = "Lines: " + txtHTML.Lines.Length.ToString()
        End Sub


        ''' <summary>
        '''   Copy the HTML to the clipboard
        ''' </summary>
        ''' <param name="sender">button</param>
        ''' <param name="e">empty</param>
        Private Sub txtCopyToClipboard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCopyToClipboard.Click
            ' copy all the txtHTML text to the clipboard,
            ' true says to persist the data after the app closes
            Clipboard.SetDataObject(txtHTML.Text, True)
        End Sub

#End Region

#Region "Support Functions"
        ''' <summary>
        ''' Description goes here
        ''' </summary>
        ''' <returns></returns>
        Public Function getPixelName(ByVal c As Color) As String

            Dim strName As String

            If Me.ckbLoadArray.Checked Then
                'If c.R.Equals(c.G) And c.R.Equals(c.B) Then
                'strName = c.R.ToString(). Adding text here just to test updateing the file
                'Else
                strName = c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString()
                'End If
            Else
                strName = c.R.ToString() + ", " + c.G.ToString() + ", " + c.B.ToString()
            End If
            ' return the name since it either already existed or was just created
            Return strName

        End Function

        ''' <summary>
        ''' Description goes here
        ''' </summary>
        ''' <returns></returns>
        Public Function getArrayCodeLine(ByVal intColCount As Integer, ByVal strCurPixalName As String, ByVal intj As Integer, ByVal inti As Integer, ByVal strFuncName As String) As String

            Dim strRetString As String = ""

            Select Case strFuncName
                Case "hlrt"
                    strRetString = "[1," + nudLeft.Value.ToString() + "," + nudRight.Value.ToString() + "," + (inti + nudTop.Value).ToString() + "," + strCurPixalName + "],"

                Case "whrt"
                    strRetString = "[" + intColCount.ToString() + ",1," + (((intj + 1) + nudRight.Value) - intColCount).ToString() + "," + (inti + nudTop.Value).ToString() + "," + strFuncName + "],"

                Case "whlb"
                    strRetString = "[" + intColCount.ToString() + ",1," + (((intj + 1) + nudLeft.Value) - intColCount).ToString() + "," + (inti + nudBottom.Value).ToString() + "," + strFuncName + "],"


                Case ""

                Case ""

                Case ""

                Case ""

                Case ""

                Case Else
                    'whlt
                    strRetString = "[" + intColCount.ToString() + ",1," + (((intj + 1) + nudLeft.Value) - intColCount).ToString() + "," + (inti + nudTop.Value).ToString() + "," + strFuncName + "],"


            End Select
            ' return the name since it either already existed or was just created
            Return strRetString

        End Function
#End Region


#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.pictureBox1 = New System.Windows.Forms.PictureBox
            Me.btnGetFilePath = New System.Windows.Forms.Button
            Me.txtBitmapFilePath = New System.Windows.Forms.TextBox
            Me.lblBitmapFile = New System.Windows.Forms.Label
            Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
            Me.txtHTML = New System.Windows.Forms.TextBox
            Me.lblHTML = New System.Windows.Forms.Label
            Me.btnLoad = New System.Windows.Forms.Button
            Me.txtCopyToClipboard = New System.Windows.Forms.Button
            Me.lblCharCount = New System.Windows.Forms.Label
            Me.lblLineCount = New System.Windows.Forms.Label
            Me.nudTop = New System.Windows.Forms.NumericUpDown
            Me.lblAdjustTop = New System.Windows.Forms.Label
            Me.lblAdjustLeft = New System.Windows.Forms.Label
            Me.nudLeft = New System.Windows.Forms.NumericUpDown
            Me.chkRelWidth = New System.Windows.Forms.CheckBox
            Me.chkRelHeight = New System.Windows.Forms.CheckBox
            Me.ckbLoadArray = New System.Windows.Forms.CheckBox
            Me.nudRight = New System.Windows.Forms.NumericUpDown
            Me.lblAdjustRight = New System.Windows.Forms.Label
            Me.nudBottom = New System.Windows.Forms.NumericUpDown
            Me.lblAdjustBottom = New System.Windows.Forms.Label
            Me.txtAdjustLeftVarName = New System.Windows.Forms.TextBox
            Me.txtAdjustRightVarName = New System.Windows.Forms.TextBox
            Me.txtAdjustTopVarName = New System.Windows.Forms.TextBox
            Me.txtAdjustBottomVarName = New System.Windows.Forms.TextBox
            Me.gbPaintFunctions = New System.Windows.Forms.GroupBox
            Me.rbwrtb = New System.Windows.Forms.RadioButton
            Me.rbwltb = New System.Windows.Forms.RadioButton
            Me.rblrtb = New System.Windows.Forms.RadioButton
            Me.rbhlrb = New System.Windows.Forms.RadioButton
            Me.rbwhrb = New System.Windows.Forms.RadioButton
            Me.rbwhlb = New System.Windows.Forms.RadioButton
            Me.rbwhrt = New System.Windows.Forms.RadioButton
            Me.rbhlrt = New System.Windows.Forms.RadioButton
            Me.rbwhlt = New System.Windows.Forms.RadioButton
            Me.lblFunctionDesc = New System.Windows.Forms.Label
            CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudTop, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudLeft, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudRight, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudBottom, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbPaintFunctions.SuspendLayout()
            Me.SuspendLayout()
            '
            'pictureBox1
            '
            Me.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pictureBox1.Location = New System.Drawing.Point(120, 73)
            Me.pictureBox1.Name = "pictureBox1"
            Me.pictureBox1.Size = New System.Drawing.Size(368, 216)
            Me.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
            Me.pictureBox1.TabIndex = 0
            Me.pictureBox1.TabStop = False
            '
            'btnGetFilePath
            '
            Me.btnGetFilePath.Location = New System.Drawing.Point(336, 8)
            Me.btnGetFilePath.Name = "btnGetFilePath"
            Me.btnGetFilePath.Size = New System.Drawing.Size(24, 23)
            Me.btnGetFilePath.TabIndex = 1
            Me.btnGetFilePath.Text = "..."
            '
            'txtBitmapFilePath
            '
            Me.txtBitmapFilePath.Location = New System.Drawing.Point(56, 8)
            Me.txtBitmapFilePath.Name = "txtBitmapFilePath"
            Me.txtBitmapFilePath.Size = New System.Drawing.Size(272, 20)
            Me.txtBitmapFilePath.TabIndex = 2
            '
            'lblBitmapFile
            '
            Me.lblBitmapFile.Location = New System.Drawing.Point(8, 8)
            Me.lblBitmapFile.Name = "lblBitmapFile"
            Me.lblBitmapFile.Size = New System.Drawing.Size(48, 23)
            Me.lblBitmapFile.TabIndex = 3
            Me.lblBitmapFile.Text = "Bitmap:"
            '
            'openFileDialog1
            '
            Me.openFileDialog1.Filter = "Bitmap Images|*.bmp"
            '
            'txtHTML
            '
            Me.txtHTML.AcceptsReturn = True
            Me.txtHTML.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtHTML.Location = New System.Drawing.Point(4, 354)
            Me.txtHTML.MaxLength = 300000
            Me.txtHTML.Multiline = True
            Me.txtHTML.Name = "txtHTML"
            Me.txtHTML.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtHTML.Size = New System.Drawing.Size(845, 291)
            Me.txtHTML.TabIndex = 2
            '
            'lblHTML
            '
            Me.lblHTML.Location = New System.Drawing.Point(1, 331)
            Me.lblHTML.Name = "lblHTML"
            Me.lblHTML.Size = New System.Drawing.Size(48, 16)
            Me.lblHTML.TabIndex = 3
            Me.lblHTML.Text = "Code:"
            '
            'btnLoad
            '
            Me.btnLoad.Location = New System.Drawing.Point(368, 8)
            Me.btnLoad.Name = "btnLoad"
            Me.btnLoad.Size = New System.Drawing.Size(64, 23)
            Me.btnLoad.TabIndex = 1
            Me.btnLoad.Text = "Load"
            '
            'txtCopyToClipboard
            '
            Me.txtCopyToClipboard.Location = New System.Drawing.Point(754, 321)
            Me.txtCopyToClipboard.Name = "txtCopyToClipboard"
            Me.txtCopyToClipboard.Size = New System.Drawing.Size(64, 23)
            Me.txtCopyToClipboard.TabIndex = 1
            Me.txtCopyToClipboard.Text = "Copy"
            '
            'lblCharCount
            '
            Me.lblCharCount.AutoSize = True
            Me.lblCharCount.Location = New System.Drawing.Point(117, 331)
            Me.lblCharCount.Name = "lblCharCount"
            Me.lblCharCount.Size = New System.Drawing.Size(46, 13)
            Me.lblCharCount.TabIndex = 4
            Me.lblCharCount.Text = "Chars: 0"
            '
            'lblLineCount
            '
            Me.lblLineCount.AutoSize = True
            Me.lblLineCount.Location = New System.Drawing.Point(266, 331)
            Me.lblLineCount.Name = "lblLineCount"
            Me.lblLineCount.Size = New System.Drawing.Size(44, 13)
            Me.lblLineCount.TabIndex = 5
            Me.lblLineCount.Text = "Lines: 0"
            '
            'nudTop
            '
            Me.nudTop.Location = New System.Drawing.Point(232, 50)
            Me.nudTop.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
            Me.nudTop.Minimum = New Decimal(New Integer() {20, 0, 0, -2147483648})
            Me.nudTop.Name = "nudTop"
            Me.nudTop.Size = New System.Drawing.Size(47, 20)
            Me.nudTop.TabIndex = 6
            '
            'lblAdjustTop
            '
            Me.lblAdjustTop.AutoSize = True
            Me.lblAdjustTop.Location = New System.Drawing.Point(165, 52)
            Me.lblAdjustTop.Name = "lblAdjustTop"
            Me.lblAdjustTop.Size = New System.Drawing.Size(61, 13)
            Me.lblAdjustTop.TabIndex = 7
            Me.lblAdjustTop.Text = "Adjust Top:"
            '
            'lblAdjustLeft
            '
            Me.lblAdjustLeft.AutoSize = True
            Me.lblAdjustLeft.Location = New System.Drawing.Point(54, 182)
            Me.lblAdjustLeft.Name = "lblAdjustLeft"
            Me.lblAdjustLeft.Size = New System.Drawing.Size(60, 13)
            Me.lblAdjustLeft.TabIndex = 9
            Me.lblAdjustLeft.Text = "Adjust Left:"
            '
            'nudLeft
            '
            Me.nudLeft.Location = New System.Drawing.Point(66, 198)
            Me.nudLeft.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
            Me.nudLeft.Minimum = New Decimal(New Integer() {20, 0, 0, -2147483648})
            Me.nudLeft.Name = "nudLeft"
            Me.nudLeft.Size = New System.Drawing.Size(48, 20)
            Me.nudLeft.TabIndex = 8
            '
            'chkRelWidth
            '
            Me.chkRelWidth.AutoSize = True
            Me.chkRelWidth.Enabled = False
            Me.chkRelWidth.Location = New System.Drawing.Point(696, 14)
            Me.chkRelWidth.Name = "chkRelWidth"
            Me.chkRelWidth.Size = New System.Drawing.Size(122, 17)
            Me.chkRelWidth.TabIndex = 10
            Me.chkRelWidth.Text = "Relative Width (vW)"
            Me.chkRelWidth.UseVisualStyleBackColor = True
            '
            'chkRelHeight
            '
            Me.chkRelHeight.AutoSize = True
            Me.chkRelHeight.Enabled = False
            Me.chkRelHeight.Location = New System.Drawing.Point(696, 37)
            Me.chkRelHeight.Name = "chkRelHeight"
            Me.chkRelHeight.Size = New System.Drawing.Size(122, 17)
            Me.chkRelHeight.TabIndex = 11
            Me.chkRelHeight.Text = "Relative Height (vH)"
            Me.chkRelHeight.UseVisualStyleBackColor = True
            '
            'ckbLoadArray
            '
            Me.ckbLoadArray.AutoSize = True
            Me.ckbLoadArray.Checked = True
            Me.ckbLoadArray.CheckState = System.Windows.Forms.CheckState.Checked
            Me.ckbLoadArray.Location = New System.Drawing.Point(448, 12)
            Me.ckbLoadArray.Name = "ckbLoadArray"
            Me.ckbLoadArray.Size = New System.Drawing.Size(77, 17)
            Me.ckbLoadArray.TabIndex = 12
            Me.ckbLoadArray.Text = "Load Array"
            Me.ckbLoadArray.UseVisualStyleBackColor = True
            '
            'nudRight
            '
            Me.nudRight.Location = New System.Drawing.Point(494, 144)
            Me.nudRight.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
            Me.nudRight.Minimum = New Decimal(New Integer() {20, 0, 0, -2147483648})
            Me.nudRight.Name = "nudRight"
            Me.nudRight.Size = New System.Drawing.Size(48, 20)
            Me.nudRight.TabIndex = 13
            '
            'lblAdjustRight
            '
            Me.lblAdjustRight.AutoSize = True
            Me.lblAdjustRight.Location = New System.Drawing.Point(494, 128)
            Me.lblAdjustRight.Name = "lblAdjustRight"
            Me.lblAdjustRight.Size = New System.Drawing.Size(67, 13)
            Me.lblAdjustRight.TabIndex = 14
            Me.lblAdjustRight.Text = "Adjust Right:"
            '
            'nudBottom
            '
            Me.nudBottom.Location = New System.Drawing.Point(384, 295)
            Me.nudBottom.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
            Me.nudBottom.Minimum = New Decimal(New Integer() {20, 0, 0, -2147483648})
            Me.nudBottom.Name = "nudBottom"
            Me.nudBottom.Size = New System.Drawing.Size(48, 20)
            Me.nudBottom.TabIndex = 15
            '
            'lblAdjustBottom
            '
            Me.lblAdjustBottom.AutoSize = True
            Me.lblAdjustBottom.Location = New System.Drawing.Point(311, 297)
            Me.lblAdjustBottom.Name = "lblAdjustBottom"
            Me.lblAdjustBottom.Size = New System.Drawing.Size(75, 13)
            Me.lblAdjustBottom.TabIndex = 16
            Me.lblAdjustBottom.Text = "Adjust Bottom:"
            '
            'txtAdjustLeftVarName
            '
            Me.txtAdjustLeftVarName.Location = New System.Drawing.Point(66, 225)
            Me.txtAdjustLeftVarName.Name = "txtAdjustLeftVarName"
            Me.txtAdjustLeftVarName.Size = New System.Drawing.Size(48, 20)
            Me.txtAdjustLeftVarName.TabIndex = 26
            '
            'txtAdjustRightVarName
            '
            Me.txtAdjustRightVarName.Location = New System.Drawing.Point(494, 170)
            Me.txtAdjustRightVarName.Name = "txtAdjustRightVarName"
            Me.txtAdjustRightVarName.Size = New System.Drawing.Size(48, 20)
            Me.txtAdjustRightVarName.TabIndex = 27
            '
            'txtAdjustTopVarName
            '
            Me.txtAdjustTopVarName.Location = New System.Drawing.Point(285, 50)
            Me.txtAdjustTopVarName.Name = "txtAdjustTopVarName"
            Me.txtAdjustTopVarName.Size = New System.Drawing.Size(48, 20)
            Me.txtAdjustTopVarName.TabIndex = 28
            '
            'txtAdjustBottomVarName
            '
            Me.txtAdjustBottomVarName.Location = New System.Drawing.Point(438, 295)
            Me.txtAdjustBottomVarName.Name = "txtAdjustBottomVarName"
            Me.txtAdjustBottomVarName.Size = New System.Drawing.Size(48, 20)
            Me.txtAdjustBottomVarName.TabIndex = 29
            '
            'gbPaintFunctions
            '
            Me.gbPaintFunctions.Controls.Add(Me.rbwrtb)
            Me.gbPaintFunctions.Controls.Add(Me.rbwltb)
            Me.gbPaintFunctions.Controls.Add(Me.rblrtb)
            Me.gbPaintFunctions.Controls.Add(Me.rbhlrb)
            Me.gbPaintFunctions.Controls.Add(Me.rbwhrb)
            Me.gbPaintFunctions.Controls.Add(Me.rbwhlb)
            Me.gbPaintFunctions.Controls.Add(Me.rbwhrt)
            Me.gbPaintFunctions.Controls.Add(Me.rbhlrt)
            Me.gbPaintFunctions.Controls.Add(Me.rbwhlt)
            Me.gbPaintFunctions.Location = New System.Drawing.Point(591, 73)
            Me.gbPaintFunctions.Name = "gbPaintFunctions"
            Me.gbPaintFunctions.Size = New System.Drawing.Size(200, 165)
            Me.gbPaintFunctions.TabIndex = 39
            Me.gbPaintFunctions.TabStop = False
            Me.gbPaintFunctions.Text = "Paint Function"
            '
            'rbwrtb
            '
            Me.rbwrtb.AutoSize = True
            Me.rbwrtb.Location = New System.Drawing.Point(113, 97)
            Me.rbwrtb.Name = "rbwrtb"
            Me.rbwrtb.Size = New System.Drawing.Size(45, 17)
            Me.rbwrtb.TabIndex = 47
            Me.rbwrtb.Text = "wrtb"
            Me.rbwrtb.UseVisualStyleBackColor = True
            '
            'rbwltb
            '
            Me.rbwltb.AutoSize = True
            Me.rbwltb.Location = New System.Drawing.Point(113, 75)
            Me.rbwltb.Name = "rbwltb"
            Me.rbwltb.Size = New System.Drawing.Size(44, 17)
            Me.rbwltb.TabIndex = 46
            Me.rbwltb.Text = "wltb"
            Me.rbwltb.UseVisualStyleBackColor = True
            '
            'rblrtb
            '
            Me.rblrtb.AutoSize = True
            Me.rblrtb.Location = New System.Drawing.Point(113, 52)
            Me.rblrtb.Name = "rblrtb"
            Me.rblrtb.Size = New System.Drawing.Size(39, 17)
            Me.rblrtb.TabIndex = 45
            Me.rblrtb.Text = "lrtb"
            Me.rblrtb.UseVisualStyleBackColor = True
            '
            'rbhlrb
            '
            Me.rbhlrb.AutoSize = True
            Me.rbhlrb.Location = New System.Drawing.Point(113, 28)
            Me.rbhlrb.Name = "rbhlrb"
            Me.rbhlrb.Size = New System.Drawing.Size(42, 17)
            Me.rbhlrb.TabIndex = 44
            Me.rbhlrb.Text = "hlrb"
            Me.rbhlrb.UseVisualStyleBackColor = True
            '
            'rbwhrb
            '
            Me.rbwhrb.AutoSize = True
            Me.rbwhrb.Location = New System.Drawing.Point(43, 120)
            Me.rbwhrb.Name = "rbwhrb"
            Me.rbwhrb.Size = New System.Drawing.Size(48, 17)
            Me.rbwhrb.TabIndex = 43
            Me.rbwhrb.Text = "whrb"
            Me.rbwhrb.UseVisualStyleBackColor = True
            '
            'rbwhlb
            '
            Me.rbwhlb.AutoSize = True
            Me.rbwhlb.Location = New System.Drawing.Point(43, 97)
            Me.rbwhlb.Name = "rbwhlb"
            Me.rbwhlb.Size = New System.Drawing.Size(47, 17)
            Me.rbwhlb.TabIndex = 42
            Me.rbwhlb.Text = "whlb"
            Me.rbwhlb.UseVisualStyleBackColor = True
            '
            'rbwhrt
            '
            Me.rbwhrt.AutoSize = True
            Me.rbwhrt.Location = New System.Drawing.Point(43, 74)
            Me.rbwhrt.Name = "rbwhrt"
            Me.rbwhrt.Size = New System.Drawing.Size(45, 17)
            Me.rbwhrt.TabIndex = 41
            Me.rbwhrt.Text = "whrt"
            Me.rbwhrt.UseVisualStyleBackColor = True
            '
            'rbhlrt
            '
            Me.rbhlrt.AutoSize = True
            Me.rbhlrt.Location = New System.Drawing.Point(43, 51)
            Me.rbhlrt.Name = "rbhlrt"
            Me.rbhlrt.Size = New System.Drawing.Size(39, 17)
            Me.rbhlrt.TabIndex = 40
            Me.rbhlrt.Text = "hlrt"
            Me.rbhlrt.UseVisualStyleBackColor = True
            '
            'rbwhlt
            '
            Me.rbwhlt.AutoSize = True
            Me.rbwhlt.Checked = True
            Me.rbwhlt.Location = New System.Drawing.Point(43, 28)
            Me.rbwhlt.Name = "rbwhlt"
            Me.rbwhlt.Size = New System.Drawing.Size(44, 17)
            Me.rbwhlt.TabIndex = 39
            Me.rbwhlt.TabStop = True
            Me.rbwhlt.Text = "whlt"
            Me.rbwhlt.UseVisualStyleBackColor = True
            '
            'lblFunctionDesc
            '
            Me.lblFunctionDesc.AutoSize = True
            Me.lblFunctionDesc.Location = New System.Drawing.Point(588, 241)
            Me.lblFunctionDesc.MaximumSize = New System.Drawing.Size(360, 60)
            Me.lblFunctionDesc.Name = "lblFunctionDesc"
            Me.lblFunctionDesc.Size = New System.Drawing.Size(121, 13)
            Me.lblFunctionDesc.TabIndex = 40
            Me.lblFunctionDesc.Text = "Width, Height, Left, Top"
            '
            'frmBitmapToCode
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(861, 647)
            Me.Controls.Add(Me.lblFunctionDesc)
            Me.Controls.Add(Me.gbPaintFunctions)
            Me.Controls.Add(Me.txtAdjustBottomVarName)
            Me.Controls.Add(Me.txtAdjustTopVarName)
            Me.Controls.Add(Me.txtAdjustRightVarName)
            Me.Controls.Add(Me.txtAdjustLeftVarName)
            Me.Controls.Add(Me.lblAdjustBottom)
            Me.Controls.Add(Me.nudBottom)
            Me.Controls.Add(Me.lblAdjustRight)
            Me.Controls.Add(Me.nudRight)
            Me.Controls.Add(Me.ckbLoadArray)
            Me.Controls.Add(Me.chkRelHeight)
            Me.Controls.Add(Me.chkRelWidth)
            Me.Controls.Add(Me.lblAdjustLeft)
            Me.Controls.Add(Me.nudLeft)
            Me.Controls.Add(Me.lblAdjustTop)
            Me.Controls.Add(Me.nudTop)
            Me.Controls.Add(Me.lblLineCount)
            Me.Controls.Add(Me.lblCharCount)
            Me.Controls.Add(Me.lblBitmapFile)
            Me.Controls.Add(Me.txtBitmapFilePath)
            Me.Controls.Add(Me.txtHTML)
            Me.Controls.Add(Me.btnGetFilePath)
            Me.Controls.Add(Me.pictureBox1)
            Me.Controls.Add(Me.lblHTML)
            Me.Controls.Add(Me.btnLoad)
            Me.Controls.Add(Me.txtCopyToClipboard)
            Me.Name = "frmBitmapToCode"
            Me.Text = "Bitmap to Code"
            CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudTop, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudLeft, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudRight, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudBottom, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbPaintFunctions.ResumeLayout(False)
            Me.gbPaintFunctions.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
#End Region



        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAdjustRight.Click

        End Sub

        Private Sub rbhlrb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbhlrb.Click
            Me.lblFunctionDesc.Text = "Height, Left, Right, Bottom"
        End Sub

        Private Sub rbhlrt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbhlrt.Click
            Me.lblFunctionDesc.Text = "Height, Left, Right, Top"
        End Sub

        Private Sub rblrtb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblrtb.Click
            Me.lblFunctionDesc.Text = "Left, Right, Top, Bottom"
        End Sub

        Private Sub rbwhlb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbwhlb.Click
            Me.lblFunctionDesc.Text = "Width, Height, Left, Bottom"
        End Sub

        Private Sub rbwhlt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbwhlt.Click
            Me.lblFunctionDesc.Text = "Width, Height, Left, Top"
        End Sub

        Private Sub rbwhrb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbwhrb.Click
            Me.lblFunctionDesc.Text = "Width, Height, Right, Bottom"
        End Sub

        Private Sub rbwhrt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbwhrt.Click
            Me.lblFunctionDesc.Text = "Width, Height, Right, Top"
        End Sub

        Private Sub rbwltb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbwltb.Click
            Me.lblFunctionDesc.Text = "Width, Left, Top, Bottom"
        End Sub

        Private Sub rbwrtb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbwrtb.Click
            Me.lblFunctionDesc.Text = "Width, Right, Top, Bottom"
        End Sub
    End Class
End Namespace
