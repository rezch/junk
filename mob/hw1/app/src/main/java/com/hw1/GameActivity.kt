package com.hw1

import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat

class GameActivity : AppCompatActivity() {
    private lateinit var choiceButtons: List<Button>
    private lateinit var resultText: TextView
    private lateinit var playerChoiceText: TextView
    private lateinit var computerChoiceText: TextView
    private lateinit var playerNameText: TextView

    private val gameManager = GameManager()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_game)

        initializeViews()
        setupClickListeners()
        updatePlayerName()

        gameManager.prepareGame()
    }

    private fun initializeViews() {
        choiceButtons = listOf(
            findViewById(R.id.btn_rock),
            findViewById(R.id.btn_paper),
            findViewById(R.id.btn_scissors),
            findViewById(R.id.btn_lizard),
            findViewById(R.id.btn_spock)
        )

        resultText = findViewById(R.id.result_text)
        playerChoiceText = findViewById(R.id.player_choice)
        computerChoiceText = findViewById(R.id.computer_choice)
        playerNameText = findViewById(R.id.player_name)
    }

    private fun setupClickListeners() {
        choiceButtons[0].setOnClickListener { onChoiceSelected(Choice.Rock) }
        choiceButtons[1].setOnClickListener { onChoiceSelected(Choice.Paper) }
        choiceButtons[2].setOnClickListener { onChoiceSelected(Choice.Scissors) }
        choiceButtons[3].setOnClickListener { onChoiceSelected(Choice.Lizard) }
        choiceButtons[4].setOnClickListener { onChoiceSelected(Choice.Spock) }

        findViewById<Button>(R.id.btn_back).setOnClickListener {
            finish()
        }

        findViewById<Button>(R.id.btn_new_round).setOnClickListener {
            startNewRound()
        }
    }

    private fun updatePlayerName() {
        val name = NicknameManager.name() ?: "Игрок"
        playerNameText.text = "Игрок: $name"
    }

    private fun onChoiceSelected(choice: Choice) {
        gameManager.setPlayerChoice(choice)
        val result = gameManager.gameResult()

        displayResults(choice, gameManager.getComputerChoice()!!, result)
    }

    private fun displayResults(playerChoice: Choice, computerChoice: Choice, result: GameResult) {
        playerChoiceText.text = "Ваш выбор: ${choiceToText(playerChoice)}"
        computerChoiceText.text = "Выбор компьютера: ${choiceToText(computerChoice)}"

        resultText.text = when (result) {
            GameResult.PLAYER_WIN -> "Поздравляем! Вы победили!"
            GameResult.COMPUTER_WIN -> "Компьютер победил. Попробуйте еще раз!"
            GameResult.DRAW -> "Ничья!"
        }

        resultText.setTextColor(when (result) {
            GameResult.PLAYER_WIN -> ContextCompat.getColor(this, android.R.color.holo_green_dark)
            GameResult.COMPUTER_WIN -> ContextCompat.getColor(this, android.R.color.holo_red_dark)
            GameResult.DRAW -> ContextCompat.getColor(this, android.R.color.holo_orange_dark)
        })
    }

    private fun choiceToText(choice: Choice): String {
        return when (choice) {
            Choice.Rock -> "Камень"
            Choice.Paper -> "Бумага"
            Choice.Scissors -> "Ножницы"
            Choice.Lizard -> "Ящерица"
            Choice.Spock -> "Спок"
        }
    }

    private fun startNewRound() {
        gameManager.prepareGame()
        resultText.text = "Сделайте ваш выбор!"
        resultText.setTextColor(ContextCompat.getColor(this, android.R.color.black))
        playerChoiceText.text = "Ваш выбор: "
        computerChoiceText.text = "Выбор компьютера: "
    }
}
