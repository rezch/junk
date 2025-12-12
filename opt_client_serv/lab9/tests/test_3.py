from playwright.sync_api import sync_playwright
from pages import LoginPage, MainPage

def test_open_critical_thinking_in_maintest_5i():
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=False)
        context = browser.new_context()
        page = context.new_page()
        login_page = LoginPage(page)

        # Логин
        login_page.navigate()
        login_page.should_have_login_and_password_fields()
        login_page.login('test', 'Te$t1234')

        # Открытие теста
        main_page = MainPage(page)
        main_page.login_is_visible()
        main_page.open_test_in_5i('Критическое мышление')

        # Визуальная пауза (например, на 2 секунды)
        page.wait_for_timeout(2000)

        browser.close()
