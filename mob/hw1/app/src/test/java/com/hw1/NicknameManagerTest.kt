package com.hw1

import org.junit.After
import org.junit.Before
import org.junit.Test
import org.junit.Assert.*

class NicknameManagerTest {
    @Before
    fun setUp() {
        NicknameManager.change("")
    }

    @After
    fun tearDown() {
        NicknameManager.change("")
    }

    @Test
    fun `test initial state has no name`() {
        assertFalse(NicknameManager.hasName())
        assertNull(NicknameManager.name())
    }

    @Test
    fun `test setting and getting name`() {
        val testName = "TestUser"
        NicknameManager.change(testName)

        assertTrue(NicknameManager.hasName())
        assertEquals(testName, NicknameManager.name())
    }

    @Test
    fun `test changing name`() {
        val firstName = "FirstUser"
        val secondName = "SecondUser"

        NicknameManager.change(firstName)
        assertEquals(firstName, NicknameManager.name())

        NicknameManager.change(secondName)
        assertEquals(secondName, NicknameManager.name())
    }

    @Test
    fun `test empty name is considered as not set`() {
        NicknameManager.change("")
        assertFalse(NicknameManager.hasName())
        assertEquals(null, NicknameManager.name())

        NicknameManager.change("   ")
        assertFalse(NicknameManager.hasName())
    }

    @Test
    fun `test trimming whitespace in name`() {
        val nameWithSpaces = "  Test User  "
        val nameTrimmed = "Test User"
        NicknameManager.change(nameWithSpaces)

        assertEquals(nameTrimmed, NicknameManager.name())
    }
}
