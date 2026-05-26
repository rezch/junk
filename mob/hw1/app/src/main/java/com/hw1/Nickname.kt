package com.hw1

import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.google.android.material.textfield.TextInputEditText

class Nickname : AppCompatActivity() {
    private lateinit var nameText: TextView
    private lateinit var nameEnterBox: TextInputEditText

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_nickname)

        nameText = findViewById(R.id.nick_name)
        nameEnterBox = findViewById(R.id.nick_hint)
        showNameHint()

        findViewById<Button>(R.id.nick_save).setOnClickListener {
            onNameSave()
        }

        findViewById<Button>(R.id.nick_button_back).setOnClickListener {
            finish()
        }
    }

    private fun showNameHint() {
        Log.e("TAG", "has: ${NicknameManager.hasName()}, name: ${NicknameManager.name()}")
        if (NicknameManager.hasName())
            nameText.text = "Ваше имя: ${NicknameManager.name()}"
        else
            nameText.text = "Ваше имя: "
    }

    private fun onNameSave() {
        val name = nameEnterBox.text?.toString()?.trim() ?: ""
        Log.e("TAG", "box: $name")
        NicknameManager.change(name)
        showNameHint()
    }
}