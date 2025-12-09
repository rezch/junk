import numpy as np

arr = np.array([1, 5, 1, 4, 3, 1, 5, 1, 5, 4, 3, 2])
indices_5 = np.where(arr == 5)[0]
print(indices_5)
print()

vector = np.linspace(1, 12, num=12, endpoint=False)[1:-1]
print(vector)
print()

a = np.array([3, 9, 7, 12, 4, 15])
a[a > 8] *= -1
print(a)
print()

mat = np.random.randint(1, 51, (3, 4))
mat_deleted = np.delete(mat, [1, 2], axis=1)
print(mat)
print(mat_deleted)
print()

m0 = np.loadtxt('m0.txt', dtype=int)
m1 = m0[::2]
m2 = m0[1::2]
print("M0: ", m0)
print("M1: ", m1)
print("M2: ", m2)
print()

n = 5
matrix = np.zeros((n, n), dtype=int)
matrix[0, :] = matrix[-1, :] = 1
matrix[:, 0] = matrix[:, -1] = 1
print(matrix)
