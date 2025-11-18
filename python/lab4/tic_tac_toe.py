EMPTY = 0
CROSS = 1
ZERO = 2

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

    for line in field:
        print(''.join([cellToStr(x) for x in line]))


def checkField3(field):
    # rows
    for line in field:
        if line[0] != EMPTY and len(set(line)):
            return line[0]

    # diag
    if field[0][0] != EMPTY and len(set([field[i][i] for i in range(3)])):
        return field[0][0]
    if field[2][0] != EMPTY and len(set([field[3 - i - 1][i] for i in range(3)])):
        return field[2][0]

    # cols
    for j in range(3):
        if field[0][j] != EMPTY and len(set([field[i][j] for i in range(3)])):
            return field[0][j]

    return EMPTY

def checkField10(field):
    def check_up(i, j):
        nonlocal field
        if i < 4: return False
        return field[i][j] == field[i - 1][j] \
            and field[i][j] == field[i - 2][j] \
            and field[i][j] == field[i - 3][j] \
            and field[i][j] == field[i - 4][j]

    def check_left(i, j):
        nonlocal field
        if j < 4: return False
        return field[i][j] == field[i][j - 1] \
            and field[i][j] == field[i][j - 2] \
            and field[i][j] == field[i][j - 3] \
            and field[i][j] == field[i][j - 4]

    def check_diag(i, j):
        if i < 4 or j < 4: return False
        return field[i][j] == field[i - 1][j - 1] \
            and field[i][j] == field[i - 2][j - 2] \
            and field[i][j] == field[i - 3][j - 3] \
            and field[i][j] == field[i - 4][j - 4]

    def check(i, j):
        return check_up(i, j) or check_left(i, j) or check_diag(i, j)

    for i, line in enumerate(field):
        for j, x in enumerate(line):
            if x != EMPTY and check(i, j):
                return x

    return EMPTY

def checkField(field):
    if len(field) == 3:
        return checkField3(field)
    return checkField10(field)


field = getField(4)
field[2][2] = CROSS
field[1][3] = ZERO
drawField(field)

