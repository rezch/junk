package com.hw1

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity

class MainActivity : AppCompatActivity() {
    private lateinit var currentNicknameText: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        currentNicknameText = findViewById(R.id.current_nickname)
        updateNicknameDisplay()

        findViewById<Button>(R.id.main_nick_button).setOnClickListener {
            changeNickname()
        }

        findViewById<Button>(R.id.main_game_button).setOnClickListener {
            startNewGame()
        }
    }

    override fun onResume() {
        super.onResume()
        updateNicknameDisplay()
    }

    private fun updateNicknameDisplay() {
        val name = NicknameManager.name()
        currentNicknameText.text = if (NicknameManager.hasName()) {
            "Текущее имя: $name"
        } else {
            "Имя не установлено"
        }
    }

    private fun changeNickname() {
        val intent = Intent(this, Nickname::class.java)
        startActivity(intent)
    }

    private fun startNewGame() {
        if (!NicknameManager.hasName()) {
            Toast.makeText(this, "Сначала установите имя", Toast.LENGTH_SHORT).show()
            changeNickname()
            return
        }

        val intent = Intent(this, GameActivity::class.java)
        startActivity(intent)
    }
}