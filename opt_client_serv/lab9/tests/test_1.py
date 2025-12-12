from playwright.sync_api import sync_playwright
from pages import LoginPage

def test_server_availability():
    with sync_playwright() as p:
        browser = p.chromium.launch(headless=False)
        context = browser.new_context()
        page = context.new_page()
        login_page = LoginPage(page)
        login_page.check_response_code()
        browser.close()
