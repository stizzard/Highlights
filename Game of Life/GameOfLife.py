__author__ = 'Sarah'

import math
import os.path
import re

grid = []
max_col_index = 19
max_row_index = 9
row_range = range(0, max_row_index + 1)
status_dead = ' D '
status_alive = ' A '
status_dying = ' - '
status_birth = ' + '

def is_int(x):
    if x.isnumeric():
        return True
    else:
        return False

def is_coord(x):
    if re.match('^[0-9][0-9]?,[0-9][0-9]?$', x):
        return True
    else:
        return False

def make_grid():
    for i in row_range:
        i = [status_dead] * (max_col_index + 1)
        grid.append(i)

def game():
    print("Welcome to J.H.Conway's Game of Life!")

    file = input(print("Please specify a coordinate file"))
    while not os.path.isfile(file):
        file = input(print("File not found. Please enter a valid file"))

    make_grid()
    coord_intake(file)
    print_grid()

    num_gen = input(print("How many generations?"))
    while is_int(num_gen) is False:
        num_gen = input(print("Please enter a number "))

    for i in range(0, int(num_gen)):
        generation_cycle()
    print_grid()
    cmrq(num_gen)

    return
#continue, modify num_gen, reset grid, or quit
def cmrq(n_g):
    # Alternate Setup: a. (Alternate generation counts): Specify a count of the generations to
    # run at the keyboard in addition to the simply ‘y’ or ‘n’.
    while int(n_g) > 0:
        response = input(print("Do you wish to CONTINUE, MODIFY number of generations, RESET the grid, or QUIT?"))
        # If continue is chosen, the generation cycle will loop the same number of times as initially requested

        while response.upper() != "CONTINUE" and response.upper() != "MODIFY" and response.upper() != \
                "RESET" and response.upper() != "QUIT":
            response = input(print("Please enter either CONTINUE, MODIFY, RESET, or QUIT"))

        if response.upper() == "CONTINUE":
            for i in range(0, int(n_g)):
                generation_cycle()
            print_grid()

        if response.upper() == "MODIFY":
            x = input(print("How many generations?"))
            while is_int(x) is False:
                x = input(print("Please enter a number greater than 0"))
            num_gen = x
            for i in range(0, int(num_gen)):
                generation_cycle()
            print_grid()

        if response.upper() == "RESET":
            grid.clear()
            game()

        if response.upper() == "QUIT":
            end_game()
    return

def print_grid():
    for i in grid:
        print(" ".join(i))
    return grid

def change_delete(line_num, content_in):
    solution = input(print("Would you like to CHANGE or DELETE the problem coordinate set?"))

    if solution.upper() != "CHANGE":
        if solution.upper() != "DELETE":
            solution = input(print("Please enter either CHANGE or DELETE"))

    if solution.upper() == "CHANGE":
        new_coordinates = input(print("Please enter the revised coordinates"))

        while is_coord(new_coordinates) is False:
            new_coordinates = input(print("Please enter two numbers separated by a comma. "
                                          "New row first, new column second."))
        content_in[line_num - 1] = new_coordinates
        return False

    else:  # solution.upper() == "DELETE":
        content_in[line_num - 1] = "\n"
        return True

def coord_intake(f):
    # http://stackoverflow.com/questions/4719438/editing-specific-line-in-text-file-in-python
    with open(f) as fl:
        content_in = fl.readlines()
    line_num = 0

    for line in content_in:
        line_num += 1
        coords = line.split(",")

        #length of coordinates must = 2
        while len(coords) != 2:
            print("Invalid coordinate length on line " + str(line_num))
            if change_delete(line_num, content_in) is True:
                continue
            else:
                coords = content_in[line_num - 1].split(",")

        f_row = int(coords[0])
        f_col = int(coords[1])

        #shorten 4 whiles
        while f_row < 0:
            print("Row is out of range on line " + str(line_num))
            if change_delete(line_num, content_in) is True:
                continue
            else:
                coords = content_in[line_num - 1].split(",")
                f_row = int(coords[0])
                f_col = int(coords[1])

        while f_row > max_row_index + 1:
            print("Row is out of range on line " + str(line_num))
            if change_delete(line_num, content_in) is True:
                continue
            else:
                coords = content_in[line_num - 1].split(",")
                f_row = int(coords[0])
                f_col = int(coords[1])

        while f_col < 0:
            print("Column is out of range on line " + str(line_num))
            if change_delete(line_num, content_in) is True:
                continue
            else:
                coords = content_in[line_num - 1].split(",")
                f_row = int(coords[0])
                f_col = int(coords[1])

        if f_col > max_col_index + 1:
            print("Column is out of range on line " + str(line_num))
            if change_delete(line_num, content_in) is True:
                continue
            else:
                coords = content_in[line_num - 1].split(",")
                f_row = int(coords[0])
                f_col = int(coords[1])

        grid[int(f_row)][int(f_col)] = status_alive
    return grid

def neighbor_count(row, col):
    # 1 The neighbours of given cell are the eight cells that touch it vertically, horizontally, or diagonally.
    ncount = 0
    if row > 0:
        if row < max_row_index:
            if col > 0:
                if col < max_col_index:  # row 1 - max-1; col 1 - max-1
                    if grid[row - 1][col - 1] == status_alive or grid[row - 1][col - 1] == status_dying:
                        ncount += 1
                    if grid[row][col - 1] == status_alive or grid[row][col - 1] == status_dying:
                        ncount += 1
                    if grid[row + 1][col - 1] == status_alive or grid[row + 1][col - 1] == status_dying:
                        ncount += 1
                    if grid[row - 1][col] == status_alive or grid[row - 1][col] == status_dying:
                        ncount += 1
                    if grid[row + 1][col] == status_alive or grid[row + 1][col] == status_dying:
                        ncount += 1
                    if grid[row - 1][col + 1] == status_alive or grid[row - 1][col + 1] == status_dying:
                        ncount += 1
                    if grid[row][col + 1] == status_alive or grid[row][col + 1] == status_dying:
                        ncount += 1
                    if grid[row + 1][col + 1] == status_alive or grid[row + 1][col + 1] == status_dying:
                        ncount += 1
                elif col == max_col_index:  # row 1 - max-1; col max
                    if grid[row - 1][col - 1] == status_alive or grid[row - 1][col - 1] == status_dying:
                        ncount += 1
                    if grid[row][col - 1] == status_alive or grid[row][col - 1] == status_dying:
                        ncount += 1
                    if grid[row + 1][col - 1] == status_alive or grid[row + 1][col - 1] == status_dying:
                        ncount += 1
                    if grid[row - 1][col] == status_alive or grid[row - 1][col] == status_dying:
                        ncount += 1
                    if grid[row + 1][col] == status_alive or grid[row + 1][col] == status_dying:
                        ncount += 1
            else:  # row 1 - max-1; col 0
                if grid[row - 1][col + 1] == status_alive or grid[row - 1][col + 1] == status_dying:
                    ncount += 1
                if grid[row][col + 1] == status_alive or grid[row][col + 1] == status_dying:
                    ncount += 1
                if grid[row + 1][col + 1] == status_alive or grid[row + 1][col + 1] == status_dying:
                    ncount += 1
                if grid[row - 1][col] == status_alive or grid[row - 1][col] == status_dying:
                    ncount += 1
                if grid[row + 1][col] == status_alive or grid[row + 1][col] == status_dying:
                    ncount += 1
        else:  # row == max_rows:
            if col > 0:
                if col < max_col_index:  # row max; col 1 - max-1
                    if grid[row - 1][col - 1] == status_alive or grid[row - 1][col - 1] == status_dying:
                        ncount += 1
                    if grid[row][col - 1] == status_alive or grid[row][col - 1] == status_dying:
                        ncount += 1
                    if grid[row - 1][col] == status_alive or grid[row - 1][col] == status_dying:
                        ncount += 1
                    if grid[row - 1][col + 1] == status_alive or grid[row - 1][col + 1] == status_dying:
                        ncount += 1
                    if grid[row][col + 1] == status_alive or grid[row][col + 1] == status_dying:
                        ncount += 1
                elif col == max_col_index:  # row max; col max
                    if grid[row - 1][col - 1] == status_alive or grid[row - 1][col - 1] == status_dying:
                        ncount += 1
                    if grid[row][col - 1] == status_alive or grid[row][col - 1] == status_dying:
                        ncount += 1
                    if grid[row - 1][col] == status_alive or grid[row - 1][col] == status_dying:
                        ncount += 1
            else:  # row max; col 0
                if grid[row - 1][col + 1] == status_alive or grid[row - 1][col + 1] == status_dying:
                    ncount += 1
                if grid[row][col + 1] == status_alive or grid[row][col + 1] == status_dying:
                    ncount += 1
                if grid[row - 1][col] == status_alive or grid[row - 1][col] == status_dying:
                    ncount += 1
    else:  # row 0
        if col > 0:
            if col < max_col_index:  #row 0; col 1-max
                if grid[row][col - 1] == status_alive or grid[row][col - 1] == status_dying:
                    ncount += 1
                if grid[row + 1][col - 1] == status_alive or grid[row + 1][col - 1] == status_dying:
                    ncount += 1
                if grid[row + 1][col] == status_alive or grid[row + 1][col] == status_dying:
                    ncount += 1
                if grid[row][col + 1] == status_alive or grid[row][col + 1] == status_dying:
                    ncount += 1
                if grid[row + 1][col + 1] == status_alive or grid[row + 1][col + 1] == status_dying:
                    ncount += 1
            elif col == max_col_index:  #row 0; col max
                if grid[row][col - 1] == status_alive or grid[row][col - 1] == status_dying:
                    ncount += 1
                if grid[row + 1][col - 1] == status_alive or grid[row + 1][col - 1] == status_dying:
                    ncount += 1
                if grid[row + 1][col] == status_alive or grid[row + 1][col] == status_dying:
                    ncount += 1
        else:  # row 0; col 0
            if grid[row][col + 1] == status_alive or grid[row][col + 1] == status_dying:
                ncount += 1
            if grid[row + 1][col + 1] == status_alive or grid[row + 1][col + 1] == status_dying:
                ncount += 1
            if grid[row + 1][col] == status_alive or grid[row + 1][col] == status_dying:
                ncount += 1
    return ncount

def generation_cycle():
    for row in range(0, len(grid)):
        for col in range(0, len(grid[row])):
            ncount = neighbor_count(row, col)

            # 2 If a cell is alive but has no neighboring cells alive, or only one alive, then the cell dies in the
            # next generation due to loneliness.
            if grid[row][col] == status_alive:
                if ncount <= 1:
                    grid[row][col] = status_dying
                    #3 If a cell is alive and has two or three living neighbours then it remains alive
                    # in the next generation.
                    #REDUNDANT?
                if ncount == 2 or ncount == 3:
                    grid[row][col] = status_alive
                    #4 If a cell is alive and has four or more neighbouring cells alive then it dies
                    # in the next generation due to overcrowding
                if ncount >= 4:
                    grid[row][col] = status_dying
            #5 If a cell is dead, then in the next generation it can become alive if it has
            #exactly three neighboring cells, no more and no fewer, that are already
            #alive. All other dead cells remain dead in the next generation.
            else:  #status_dead
                if ncount == 3:
                    grid[row][col] = status_birth
                else:
                    grid[row][col] = status_dead

    for row in range(0, len(grid)):
        for col in range(0, len(grid[row])):
            if grid[row][col] == status_birth:
                grid[row][col] = status_alive

            if grid[row][col] == status_dying:
                grid[row][col] = status_dead

    return grid

def end_game():
    print("Thanks for playing!")
    exit(0)

game()
end_game()