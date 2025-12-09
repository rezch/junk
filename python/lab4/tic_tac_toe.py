EMPTY = 0
CROSS = 1
ZERO = 2
WINNER = 3


def getField(n):
    return [
        [EMPTY for _ in range(n)]
        for _ in range(n)
    ]


def drawField(field):
    def cellToStr(cell):
        if cell == EMPTY:
            return '.'
        if cell == CROSS:
            return 'X'
        if cell == ZERO:
            return 'O'
        if cell == WINNER:
            return '+'

    for line in field:
        print(''.join([cellToStr(x) for x in line]))


def checkField(field):
    n = len(field)
    to_win = 3 if n == 3 else 5

    dirs = [
        (0, 1),
        (1, 0),
        (1, 1),
        (1, -1),
    ]

    for i in range(n):
        for j in range(n):
            player = field[i][j]
            if player == EMPTY or player == WINNER:
                continue

            for dx, dy in dirs:
                win_cells = []
                for k in range(to_win):
                    x = i + dx * k
                    y = j + dy * k
                    if 0 <= x < n and 0 <= y < n and field[x][y] == player:
                        win_cells.append((x, y))
                    else:
                        break

                if len(win_cells) == to_win:
                    for x, y in win_cells:
                        field[x][y] = WINNER
                    return True

    return False


def nextTurn(turn):
    return 1 + turn % 2


def getInput(size):
    try:
        x, y = list(map(int, input().split()))
    except:
        return None, None

    if 0 > x or x >= size \
            or 0 > y or y >= size:
        return None, None

    return x, y


def main():
    turn = CROSS

    print('Введите размер поля, 3 или 10')
    size = input()
    try:
        size = int(size)
    except ValueError:
        print('Не верно введен размер поля')
        return

    if size != 3 and size != 10:
        print('Не верно введен размер поля')
        return

    field = getField(size)

    print(f'Координаты вводятся двумя числами от 0 до {size - 1}')

    while True:
        drawField(field)

        turnStr = 'X' if turn == CROSS else 'O'
        print(f'Сейчас ход {turnStr}')
        print('Введите координаты клетки')
        x, y = getInput(size)
        if x is None or y is None:
            print('Не верные координаты')
            continue

        if field[x][y] != EMPTY:
            print('Клетка уже занята')
            continue

        field[x][y] = turn

        if checkField(field):
            print(f'Выиграли {turnStr}')
            drawField(field)
            break

        turn = nextTurn(turn)


if __name__ == "__main__":
    main()
