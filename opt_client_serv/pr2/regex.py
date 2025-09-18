import re

text = "<div xmlns=\"http://www.w3.org/1999/xhtml\"><p><em>Текстовая версия видео по протоколу HTTP из обновленного</em><a href=\"https://www.youtube.com/playlist?list=PLtPJ9lKvJ4ojPWFLuUz6g8c73Ta45bUN8\" rel=\"noopener noreferrer nofollow\" target=\"_blank\"><em> <u>курса по компьютерным сетям</u></em></a><em> для начинающих.</em></p><p>Рассматриваем основы работы HTTP, применяем HTTP на практике в терминале, используем Wireshark для анализа пакетов HTTP.&nbsp;</p><h3>Основы HTTP</h3><p>HTTP расшифровывается как <strong>Hypertext Transfer Protocol</strong>, протокол передачи гипертекста. Сейчас это один из самых популярных протоколов интернет, основа Web.</p><p>HTTP находится на <strong>прикладном уровне</strong> в моделях OSI и TCP/IP.&nbsp;</p><p>Концепцию Web предложил в 1989 году Тим Бернерс-Ли из Европейского центра ядерных исследований (ЦЕРН). Основные компоненты Web по предложению Тима Бернерс-Ли:</p><ul><li><p>Язык гипертекстовой разметки страниц HTML.</p></li><li><p>Протокол передачи гипертекстовых страниц HTTP.</p></li><li><p>Web-сервер.</p></li><li><p>Текстовый web-браузер.</p></li></ul><p>Сейчас Web устроен более сложно, браузеры поддерживают не только текст, но и изображения, видео, могут запускать код на JavaScript и многое другое.&nbsp;</p><p>Протокол HTTP используется браузером для того, чтобы загрузить с Web-сервера HTML страницы и другие ресурсы, которые нужны для показа страниц. Также HTTP сейчас активно применяется в API.</p><h3>Uniform Resource Locator</h3><p>Важную роль в работе HTTP играет Uniform Resource Locator, сокращенно URL – единообразный определитель местонахождения ресурса. Именно URL используется для того, чтобы указать, к какой странице мы хотим получить доступ.</p><figure class=\"full-width \"><img src=\"https://habrastorage.org/r/w1560/getpro/habr/upload_files/4d5/9bc/479/4d59bc479477778fbfa5552a7a085e74.png\" alt=\"Структура URL\" title=\"Структура URL\" width=\"903\" height=\"157\" data-src=\"https://habrastorage.org/getpro/habr/upload_files/4d5/9bc/479/4d59bc479477778fbfa5552a7a085e74.png\"><div><figcaption>Структура URL</figcaption></div></figure><p>&nbsp;URL состоит из трех основных частей:</p><ul><li><p><strong>Название протокола</strong>, в примере на рисунке протокол HTTP.</p></li><li><p><strong>Адрес сервера</strong>, на котором размещен ресурс. Можно использовать IP-адрес или доменное имя. Адрес сервера отделяется от названия протокола двоеточием и двумя слешами.&nbsp;</p></li><li><p><strong>Адрес ресурса на сервере</strong>. Это может быть HTML-страница, изображение, видео или ресурс другого типа. В примере на рисунке адрес страницы: /courses/networks.</p></li></ul><p>В URL не обязательно использовать только протокол HTTP, вот примеры с другими протоколами:</p><ul><li><p>https://ya.ru</p></li><li><p>ftp://example.com</p>"


# print(re.findall(r'(?<!\d)\d{2,3}(?!\d)', text, re.MULTILINE))
# print()
#
#
# print(re.findall(r'(?<!\d)([14]\d*)(?!\d)', text, re.MULTILINE))
# print()
#
#
# print(re.findall(r'\b([А-Я]{2,})\b', text, re.MULTILINE))
# print(re.findall(r'\b([A-Z]{2,})\b', text, re.MULTILINE))
# print(re.findall(r'\b([А-ЯA-Z]{2,})\b', text, re.MULTILINE))
# print()
#
# print(re.findall(r'\w+', text, re.MULTILINE))
# print()


# print(re.findall(r'(\<[^\/>]+>)', text, re.MULTILINE))
# print(re.findall(r'(\<\/[^>]+>)', text, re.MULTILINE))
# print(re.findall(r'(\<\/?[^>]+>)', text, re.MULTILINE))
# print()

# print(re.findall(r'(http[s]?:\/\/(?!w{,2}\.)[\w\.-]+)', text, re.MULTILINE))

# x = re.findall(r'((http[s]?:\/\/(?!w{,2}\.)[\w\.-]+)([\/\w\.-]+)?)', text, re.MULTILINE)
# print([x[0] for x in x])

text += "https://www.some/path?x=123&&y=vAlue|"
text += "https://www.other/path/123/?&&&"
x = re.findall(r'((http[s]?:\/\/(?!w{,2}\.)[\w\.-]+)([\/\w\.-]+)?(\?((\w+=)?(\w+)?&*)*)?)', text, re.MULTILINE)
print([x[0] for x in x])

# text = "Каждый охотник желает знать, где сидит фазан. Ехал Грека через реку, видит Грека в реке рак. Сунул Грека руку в реку, а там функция Split."
# print(re.split(r'[^\w]+', text))
#
# text = "212; 356; 213, 213: 14, 12,., 124$145%15"
# print(re.split(r'[^\d]+', text))
