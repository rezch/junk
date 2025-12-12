from playwright.sync_api import Page, expect

class MainPage:
    def __init__(self, page: Page):
        self.page = page
        self.profile = page.locator('MTSText:has-text("Текущий пользователь") b')
        self.maintest_5i = page.locator('span.MTSText:has-text("Maintest-5i")')
        self.filter_input = page.locator('input[placeholder="Введите название теста"]')
        self.apply_button = page.locator('button.MTSButton:has-text("Применить")')
        self.test_row = page.locator('div.MTSTableRow')
        # Кнопка перехода на страницу тестирования
        self.row_action = 'a.MTSLink[title="Страница проведения тестирования"]'

    # Проверяет, что профиль с логином виден
    def login_is_visible(self):
        expect(self.profile).to_be_visible(timeout=15000)

    # Открыть раздел "Maintest-5i"
    def open_maintest_5i(self):
        expect(self.maintest_5i).to_be_visible(timeout=10000)
        self.maintest_5i.click()

    # Заполняет фильтр по названию теста и применяет
    def fill_filter_and_apply(self, filter_selector, filter_value, apply_button_locator):
        filter_ = self.page.locator(filter_selector)
        expect(filter_).to_be_visible(timeout=15000)
        filter_.fill(filter_value)
        self.page.locator(apply_button_locator).click()

    # Ищет строку теста и кликает по нужной кнопке-действию
    def find_and_click_row_action(self, action_selector):
        expect(self.test_row).to_be_visible(timeout=15000)
        row = self.test_row.first
        expect(row).to_be_visible()
        row.locator(action_selector).click()

    # Открывает тест (например "Критическое мышление") в Maintest-5i
    def open_test_in_5i(self, test_name: str):
        self.open_maintest_5i()
        self.fill_filter_and_apply('input[placeholder="Введите название теста"]', test_name, 'button.MTSButton:has-text("Применить")')
        self.page.on("dialog", lambda dialog: dialog.accept())
        self.find_and_click_row_action(self.row_action)
        new_page = self.page.context.wait_for_event("page")
        new_page.wait_for_load_state()
        # Можно переключиться на new_page, если нужно дальше с ним работать
        # return new_page
