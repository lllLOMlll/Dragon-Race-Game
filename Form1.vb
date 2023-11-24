Imports System.Net.NetworkInformation
Imports System.Windows.Forms

Public Class Form1
    Dim counter As Integer = 0
    Dim rand As New Random()
    Dim dragonFinishOrder As New List(Of String)
    Dim raceInProgress As Boolean = False
    Dim isPlaceYourBetValid = True
    Dim roundNumber As Integer = 1
    Dim maxNumOfRound = 3
    Dim startMoney As Integer
    Dim moneyLeft As Integer
    Dim yourBet As Integer
    Dim dragonChoosen As String



    'The Window
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Display the number of round when the app load
        Label_Round.Text = "Round " & roundNumber

        'Disable stuff
        Button_Start_Race.Enabled = False
        Button_Start_Race.BackColor = Color.Gray
        Button_Next.Enabled = False
        Button_Start_Race.BackColor = Color.Gray
        Button_Next.Enabled = False
        Button_Next.BackColor = Color.Gray
        ' Make my dummy button (invisible) checked by default
        RadioButton_Dummy.Checked = True
    End Sub

    Private Sub Button_Place_Your_Bet_Click(sender As Object, e As EventArgs) Handles Button_Place_Your_Bet.Click
        'Resetting the isYourPlaceBetValid (after each click of the Button Place Your Bet
        isPlaceYourBetValid = True

        'VALIDATION
        ' DRAGON
        If RadioButton_Dummy.Checked Then
            MsgBox("Please choose a dragon", MsgBoxStyle.Exclamation, "Wrong Input")
            isYourBetValidFalseAndReturn()
            ' STARTING MONEY
        ElseIf String.IsNullOrEmpty(TextBox_Start_Money.Text) Then
            MsgBox("Please enter a number between 100$ and 1 0000 000$. Cannot be empy.", MsgBoxStyle.Exclamation, "Wrong Input")
            isYourBetValidFalseAndReturn()
        ElseIf Not Integer.TryParse(TextBox_Start_Money.Text, startMoney) Then
            MsgBox("Please enter a number between 100$ and 1 0000 000$. Must be digits", MsgBoxStyle.Exclamation, "Wrong Input")
            isYourBetValidFalseAndReturn()
        ElseIf startMoney < 100 OrElse startMoney > 1000000 Then
            MsgBox("Please enter a number between 100$ and 1 0000 000$.", MsgBoxStyle.Exclamation, "Wrong Input")
            isYourBetValidFalseAndReturn()
            ' YOUR BET
        ElseIf String.IsNullOrEmpty(TextBox_Your_Bet.Text) Then
            MsgBox("You bet cannot be empty", MsgBoxStyle.Exclamation, "Wrong Input")
            isYourBetValidFalseAndReturn()
        ElseIf Not Integer.TryParse(TextBox_Your_Bet.Text, yourBet) Then
            MsgBox("Your bet must be an integer number", MsgBoxStyle.Exclamation, "Wrong Input")
            isYourBetValidFalseAndReturn()
        ElseIf yourBet = 0 Then
            MsgBox("You cannot bet 0$.", MsgBoxStyle.Exclamation, "Wrong Input")
            isYourBetValidFalseAndReturn()
            ' MONEY LEFT
            ' Firt time betting
        ElseIf String.IsNullOrEmpty(TextBox_Money_Left.Text) Then
            Integer.TryParse(TextBox_Money_Left.Text, moneyLeft)
            moneyLeft = startMoney
        ElseIf yourBet > moneyLeft Then
            MsgBox("You cannot bet more than what you've got", MsgBoxStyle.Exclamation, "Wrong Input")
            isYourBetValidFalseAndReturn()
        Else
            isPlaceYourBetValid = True
        End If


        If isPlaceYourBetValid = True Then
            If RadioButtonDragon1.Checked = True Then
                dragonChoosen = RadioButtonDragon1.Text
            ElseIf RadioButtonDragon2.Checked = True Then
                dragonChoosen = RadioButtonDragon2.Text
            ElseIf RadioButtonDragon3.Checked = True Then
                dragonChoosen = RadioButtonDragon3.Text
            ElseIf RadioButtonDragon4.Checked = True Then
                dragonChoosen = RadioButtonDragon4.Text
            ElseIf RadioButtonDragon5.Checked = True Then
                dragonChoosen = RadioButtonDragon5.Text
            End If

            'Disable money TextBox
            TextBox_Start_Money.Enabled = False
            TextBox_Money_Left.Enabled = False
            TextBox_Your_Bet.Enabled = False
            ' Deduct the bet amount from money left
            moneyLeft -= yourBet
            ' Change the background color of TextBox Start Money
            TextBox_Start_Money.BackColor = Color.Gray
            ' Update the money left TextBox with the new value
            TextBox_Money_Left.Text = moneyLeft.ToString()
            ' Update buttons behavior
            Button_Start_Race.Enabled = True
            Button_Start_Race.BackColor = Color.White
            Button_Place_Your_Bet.Enabled = False
            Button_Place_Your_Bet.BackColor = Color.Gray
        End If

    End Sub


    Private Sub Button_Start_Race_Click(sender As Object, e As EventArgs) Handles Button_Start_Race.Click
        If raceInProgress = False Then
            Timer_Dragon_Race.Enabled = True
            'Disable buttons "Start Race" and "Next"
            Button_Start_Race.Enabled = False
            Button_Start_Race.BackColor = Color.Gray
            Button_Next.Enabled = False
            Button_Next.BackColor = Color.Gray
        End If

    End Sub

    Private Sub Button_Next_Click(sender As Object, e As EventArgs) Handles Button_Next.Click
        ' Checking if the user wins
        If GetStanding(dragonChoosen) = "1st" Then
            Dim moneyWon As Integer = yourBet * 2
            moneyLeft += moneyWon
            TextBox_Money_Left.Text = moneyLeft
            MsgBox($"BIG BIG WINNER!!!! You've won {moneyWon}$. You now have {moneyLeft}$", MsgBoxStyle.Exclamation, "WIN")
        End If
        ' Increasing the number of rounds by 1
        roundNumber += 1
        ' Dont update the Label Round if the round number = 3
        If roundNumber <= 3 Then
            Label_Round.Text = "Round " & roundNumber
        End If

        ResetRace()
        'Buttons
        Button_Next.Enabled = False
        Button_Next.BackColor = Color.Gray
        Button_Place_Your_Bet.Enabled = True
        Button_Place_Your_Bet.BackColor = Color.White
        'Textbox
        TextBox_Your_Bet.Enabled = True

        ' Check if the roundNumber is greater than 3 to signify game over
        If roundNumber > 3 Then
            ' Store the response of the message box in a variable
            Dim response As MsgBoxResult
            response = MsgBox("Game Over. Want to play again?", MsgBoxStyle.YesNo, "Game Over")
            'YES
            If response = MsgBoxResult.Yes Then
                roundNumber = 1
                Label_Round.Text = "Round " & roundNumber
                ResetGame()
                ResetMoneyBoxes()
                'NO
            ElseIf response = MsgBoxResult.No Then
                MsgBox("Thank you for using a Major Sofware product!", MsgBoxStyle.Critical, "Quit")
                Application.Exit()
            End If
        End If

        If moneyLeft = 0 Then
            ' Store the response of the message box in a variable
            Dim response As MsgBoxResult
            response = MsgBox("No more money left. Game Over! Want to play again?", MsgBoxStyle.YesNo, "Game Over")
            'YES
            If response = MsgBoxResult.Yes Then
                roundNumber = 1
                Label_Round.Text = "Round " & roundNumber
                ResetGame()
                ResetMoneyBoxes()
                'NO
            ElseIf response = MsgBoxResult.No Then
                MsgBox("Thank you for using a Major Sofware product!", MsgBoxStyle.Critical, "Quit")
                Application.Exit()
            End If
        End If
    End Sub

    Private Sub Timer_Dragon_Race_Tick(sender As Object, e As EventArgs) Handles Timer_Dragon_Race.Tick
        counter += 50

        ' Random advancement for each dragon.
        UpdateDragonsProgressBar(rand.Next(0, 50), ProgressBar_Dragon1, "Dragon 1")
        UpdateDragonsProgressBar(rand.Next(0, 50), ProgressBar_Dragon2, "Dragon 2")
        UpdateDragonsProgressBar(rand.Next(0, 50), ProgressBar_Dragon3, "Dragon 3")
        UpdateDragonsProgressBar(rand.Next(0, 50), ProgressBar_Dragon4, "Dragon 4")
        UpdateDragonsProgressBar(rand.Next(0, 50), ProgressBar_Dragon5, "Dragon 5")

        If dragonFinishOrder.Count = 5 Then
            Timer_Dragon_Race.Stop()
            raceInProgress = False
            DisplayRaceResults()
            ' Enabling the "Next" buttons
            Button_Next.Enabled = True
            Button_Next.BackColor = Color.White

        End If

    End Sub

    Private Sub UpdateDragonsProgressBar(dragonRandomIncrement As Integer, dragonProgressBar As ProgressBar, dragonName As String)
        Dim newValue As Integer = dragonProgressBar.Value + dragonRandomIncrement
        If newValue > dragonProgressBar.Maximum Then
            newValue = dragonProgressBar.Maximum
        End If
        dragonProgressBar.Value = newValue
        CheckForRaceCompletion(dragonProgressBar, dragonName)
    End Sub

    Private Sub CheckForRaceCompletion(dragonProgressBar As ProgressBar, dragonName As String)
        If dragonProgressBar.Value >= dragonProgressBar.Maximum AndAlso Not dragonFinishOrder.Contains(dragonName) Then
            dragonFinishOrder.Add(dragonName)
        End If
    End Sub

    Private Sub ResetRace()
        counter = 0
        dragonFinishOrder.Clear()

        'Reset progress bar to 0
        resetProgressBar()
        'Reset Standing labels
        ResetLabelStandingDragon()
    End Sub

    Private Sub ResetGame()
        counter = 0
        roundNumber = 1
        dragonFinishOrder.Clear()
        'Reset background color of TextBox Start Money
        TextBox_Start_Money.BackColor = Color.White
        'Reset progress bar to 0
        resetProgressBar()
        'Reset Standing labels
        ResetLabelStandingDragon()
        ResetMoneyBoxes()
    End Sub

    Private Sub resetProgressBar()
        ProgressBar_Dragon1.Value = 0
        ProgressBar_Dragon2.Value = 0
        ProgressBar_Dragon3.Value = 0
        ProgressBar_Dragon4.Value = 0
        ProgressBar_Dragon5.Value = 0
    End Sub

    Private Sub ResetLabelStandingDragon()
        Label_Standing_Dragon1.Text = "?"
        Label_Standing_Dragon1.ForeColor = Color.White
        Label_Standing_Dragon2.Text = "?"
        Label_Standing_Dragon2.ForeColor = Color.White
        Label_Standing_Dragon3.Text = "?"
        Label_Standing_Dragon3.ForeColor = Color.White
        Label_Standing_Dragon4.Text = "?"
        Label_Standing_Dragon4.ForeColor = Color.White
        Label_Standing_Dragon5.Text = "?"
        Label_Standing_Dragon5.ForeColor = Color.White
    End Sub

    Private Sub ResetMoneyBoxes()
        TextBox_Start_Money.Enabled = True
        TextBox_Start_Money.Text = ""
        TextBox_Money_Left.Text = ""
        TextBox_Your_Bet.Text = ""
    End Sub

    Private Sub DisplayRaceResults()
        UpdateStandingLabel(Label_Standing_Dragon1, "Dragon 1")
        UpdateStandingLabel(Label_Standing_Dragon2, "Dragon 2")
        UpdateStandingLabel(Label_Standing_Dragon3, "Dragon 3")
        UpdateStandingLabel(Label_Standing_Dragon4, "Dragon 4")
        UpdateStandingLabel(Label_Standing_Dragon5, "Dragon 5")
    End Sub

    Private Function GetStanding(dragonName As String) As String
        'Index 0 = first (1st) 0 + 1 = 1 
        Dim position As Integer = dragonFinishOrder.IndexOf(dragonName) + 1
        Select Case position
            Case 1
                Return "1st"
            Case 2
                Return "2nd"
            Case 3
                Return "3rd"
            Case Else
                Return position.ToString() & "th"
        End Select
    End Function

    Private Sub TextBox_Start_Money_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox_Start_Money.KeyPress
        ' Check if the key pressed is not a digit and also not a control key (like backspace)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' Suppress the key press event
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox_Your_Bet_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox_Your_Bet.KeyPress
        ' Check if the key pressed is not a digit and also not a control key (like backspace)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            ' Suppress the key press event
            e.Handled = True
        End If
    End Sub

    Private Sub isYourBetValidFalseAndReturn()
        isPlaceYourBetValid = False
        Return
    End Sub

    Private Sub UpdateStandingLabel(standingLabel As Label, dragonName As String)
        Dim standing As String = GetStanding(dragonName)
        standingLabel.Text = standing
        If standing = "1st" Then
            standingLabel.ForeColor = Color.Green
        Else
            standingLabel.ForeColor = Color.Red
        End If
    End Sub
End Class
