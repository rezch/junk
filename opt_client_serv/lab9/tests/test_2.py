from playwright.sync_api import sync_playwright
from pages import LoginPage, MainPage

def test_login():
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=False)
        context = browser.new_context()
        page = context.new_page()
        login_page = LoginPage(page)

        login_page.navigate()
        login_page.should_have_login_and_password_fields()
        login_page.login('test', 'Te$t1234')

        main_page = MainPage(page)
        main_page.login_is_visible()

        browser.close()
