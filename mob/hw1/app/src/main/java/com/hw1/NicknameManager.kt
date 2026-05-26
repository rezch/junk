package com.hw1

object NicknameManager {
    private var name: String? = null

    fun name(): String? {
        return this.name
    }

    fun change(nameRaw: String) {
        val name = nameRaw.trim()

        if (name.isEmpty())
            this.name = null
        else
            this.name = name
    }

    fun hasName(): Boolean = name != null
}