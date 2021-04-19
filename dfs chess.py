class board:
    def __init__(self, size):       
        self.beginning_position = [1, 1]
        self.board_size = size
        self.possible_moves = [[-2, 1], [-2, -1], [-1, 2], [1, 2], [2, 1], [2, -1], [1, -2], [-1, -2]]
    
class grid:
    def __init__(self, board, parent=None, move=None, previous_moves=None):
        self.grid = None    
        self.board = board
        self.parent = parent
        self.move = move

        # if its the root node
        if self.parent is None:
            self.previous_moves = []
            self.previous_moves.append(self.board.beginning_position)
            self.grid = self.board.beginning_position
        else:
            self.previous_moves = self.parent.previous_moves[:]
            new_x = self.parent.grid[0] + self.move[0]
            new_y = self.parent.grid[1] + self.move[1]
            self.previous_moves.append([new_x, new_y])
            self.grid = [new_x, new_y]

def has_been_explored(explored, next_move):
    for grid in explored:
        if grid.previous_moves == next_move.previous_moves:
            return True
    return False

def dfs(board):
    current_grid = grid(board)

    if current_grid.previous_moves == ((board.board_size)*(board.board_size)):
        print("Found a solution! " + current_grid.previous_moves)
        return

    grids_been_to = []
    grids_been_to.append(current_grid)
    explored = []
    goal_found = False

    while not goal_found and grids_been_to:
        current_grid = grids_been_to.pop()
        explored.append(current_grid.previous_moves)

        for move in board.possible_moves:
            temp_x = current_grid.grid[0] + move[0]
            temp_y = current_grid.grid[1] + move[1]

            if temp_x > 0 and temp_x <= board.board_size and temp_y > 0 and temp_y <= board.board_size:
                next_move = grid(board, current_grid, move)
                if next_move.grid not in current_grid.previous_moves and next_move.previous_moves not in explored and not has_been_explored(grids_been_to, next_move):
                    if len(next_move.previous_moves) == ((board.board_size)*(board.board_size)):
                        goal_found = True
                        break
                    grids_been_to.append(next_move)

    if goal_found:
        print("Found a solution! Here is a list of the moves.")
        print(next_move.previous_moves)
    else:
        print("Failed")

board_size = int(input("Enter size of the board: "))
board = board(board_size)
dfs(board)
