package com.hw1

import org.junit.Before
import org.junit.Test
import org.junit.Assert.*

class GameManagerImprovedTest {

    private lateinit var gameManager: GameManager

    @Before
    fun setUp() {
        gameManager = GameManager()
    }

    @Test
    fun `test draw when choices are same`() {
        val choice = Choice.Spock
        gameManager.prepareGameWithChoice(choice)
        gameManager.setPlayerChoice(choice)

        assertEquals(GameResult.DRAW, gameManager.gameResult())
    }

    @Test
    fun `test all win conditions`() {
        val winConditions = mapOf(
            Choice.Rock to listOf(Choice.Scissors, Choice.Lizard),
            Choice.Paper to listOf(Choice.Rock, Choice.Spock),
            Choice.Scissors to listOf(Choice.Paper, Choice.Lizard),
            Choice.Lizard to listOf(Choice.Paper, Choice.Spock),
            Choice.Spock to listOf(Choice.Rock, Choice.Scissors)
        )

        for ((playerChoice, losingComputerChoices) in winConditions) {
            for (computerChoice in losingComputerChoices) {
                gameManager.prepareGameWithChoice(computerChoice)
                gameManager.setPlayerChoice(playerChoice)
                assertEquals(
                    "Player with $playerChoice should win against $computerChoice",
                    GameResult.PLAYER_WIN,
                    gameManager.gameResult()
                )
            }
        }
    }

    @Test
    fun `test all lose conditions`() {
        val loseConditions = mapOf(
            Choice.Rock to listOf(Choice.Paper, Choice.Spock),
            Choice.Paper to listOf(Choice.Scissors, Choice.Lizard),
            Choice.Scissors to listOf(Choice.Rock, Choice.Spock),
            Choice.Lizard to listOf(Choice.Rock, Choice.Scissors),
            Choice.Spock to listOf(Choice.Paper, Choice.Lizard)
        )

        for ((playerChoice, winningComputerChoices) in loseConditions) {
            for (computerChoice in winningComputerChoices) {
                gameManager.prepareGameWithChoice(computerChoice)
                gameManager.setPlayerChoice(playerChoice)
                assertEquals(
                    "Player with $playerChoice should lose against $computerChoice",
                    GameResult.COMPUTER_WIN,
                    gameManager.gameResult()
                )
            }
        }
    }

    @Test
    fun `test computer choice is random after prepareGame`() {
        val choices = mutableSetOf<Choice>()

        repeat(100) {
            gameManager.prepareGame()
            val computerChoice = gameManager.getComputerChoice()
            assertNotNull(computerChoice)
            choices.add(computerChoice!!)
        }

        assertTrue("Should generate different choices", choices.size > 1)
    }
}
