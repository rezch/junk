from playwright.sync_api import Page, expect
import time

class LoginPage:
    def __init__(self, page: Page):
        self.page = page
        # Локаторы (уточните при необходимости через инспектор)
        self.login_input = page.locator('input[name="login"]')
        self.password_input = page.locator('input[name="password"]')
        self.submit_button = page.locator('button.MTSButton[type="submit"]')

    # Проверка успешного HTTP-ответа (код 2xx)
    def check_response_code(self):
        response = self.page.goto("https://dev.maintest.ru", wait_until="domcontentloaded")
        expect(response).to_be_ok()

    # Открытие страницы логина c повтором при ошибке
    def navigate(self):
        for attempt in range(3):
            try:
                self.page.goto("https://dev.maintest.ru", wait_until="networkidle", timeout=60000)
                return
            except Exception:
                if attempt == 2:
                    raise
                time.sleep(2)

    # Проверка видимости полей ввода
    def should_have_login_and_password_fields(self):
        expect(self.login_input).to_be_visible()
        expect(self.password_input).to_be_visible()

    # Ввод логина и пароля
    def login(self, username: str, password: str):
        self.login_input.fill(username)
        self.password_input.fill(password)
        self.submit_button.click()
