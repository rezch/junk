package com.hw1

enum class Choice {
    Rock,
    Paper,
    Scissors,
    Lizard,
    Spock,
}

class GameManager {
    private var playerChoice: Choice? = null
    private var computerChoice: Choice? = null

    fun prepareGame() {
        playerChoice = null
        computerChoice = Choice.entries.random()
    }

    fun prepareGameWithChoice(choice: Choice) {
        playerChoice = null
        computerChoice = choice
    }

    fun setPlayerChoice(choice: Choice) {
        playerChoice = choice
    }

    fun gameResult(): GameResult {
        if (playerChoice == null || computerChoice == null) {
            throw Exception("Choices are not set.")
        }

        return when {
            playerChoice == computerChoice -> GameResult.DRAW
            playerWins() -> GameResult.PLAYER_WIN
            else -> GameResult.COMPUTER_WIN
        }
    }

    private fun playerWins(): Boolean {
        return when (playerChoice) {
            Choice.Rock -> computerChoice == Choice.Scissors || computerChoice == Choice.Lizard
            Choice.Paper -> computerChoice == Choice.Rock || computerChoice == Choice.Spock
            Choice.Scissors -> computerChoice == Choice.Paper || computerChoice == Choice.Lizard
            Choice.Lizard -> computerChoice == Choice.Paper || computerChoice == Choice.Spock
            Choice.Spock -> computerChoice == Choice.Rock || computerChoice == Choice.Scissors
            null -> false
        }
    }

    fun getPlayerChoice(): Choice? = playerChoice
    fun getComputerChoice(): Choice? = computerChoice
}

enum class GameResult {
    PLAYER_WIN,
    COMPUTER_WIN,
    DRAW
}
