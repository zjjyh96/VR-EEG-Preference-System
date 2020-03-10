import pandas as pd
import csv
import re


def get_view(string):
    if re.search(r"Description", string):
        return "Description"
    if re.search(r"Rating", string):
        return "Rating"
    if re.search(r"Distribution", string):
        return "Distribution"
    if re.search(r"Review", string):
        return "Review"


def get_condition(string):
    if re.search(r"NoDisplay", string):
        return "NoDisplay"
    if re.search(r"SomeDisplay", string):
        return "SomeDisplay"
    if re.search(r"FullDisplay", string):
        return "FullDisplay"
    return 0


def parse_file(f_path, user_id):
    data = []
    cur_product = []

    with open(f_path) as file:
        lines = file.readlines()
        index = 0
        while not re.search(r'Phase 1 Start', lines[index]):
            index += 1
        while not re.search(r'End Phase 1', lines[index]):
            index += 1
            print(lines[index])
            # cur_product example: [user_id, product_id, phase, condition, reaction_time, rating]
            reaction_start = 0
            reaction_end = 0
            while not re.search(r'Rate for product', lines[index]):
                cur_product = [user_id, "Phase1"]  # user_id, phase

                if re.search(r'End Phase 1', lines[index]):
                    break

                if re.search(r'Description', lines[index]):
                    breaks = re.split(r'[\]\[]', lines[index])
                    reaction_start = float(breaks[3])
                    index += 1

                if re.search(r'Rating', lines[index]):
                    breaks = re.split(r'[\]\[]', lines[index])
                    reaction_end = float(breaks[3])
                    cur_product.append("NoDisplay")
                    index += 1

                cur_product.append(reaction_end - reaction_start)  # reaction_time

                if re.search(r'Rate for product', lines[index]):
                    digits = [int(s) for s in lines[index].split() if s.isdigit()]
                    cur_product.append(digits[0])  # product_id
                    cur_product.append(digits[1])  # rating
                    index += 1

                data.append(cur_product)
                cur_product = []

        while not re.search(r'Phase 2 Start', lines[index]):
            index += 1

        print("start processing phase 2")
        while not re.search(r'End Phase 2', lines[index]):
            index += 1
            # cur_product example: [user_id, product_id, phase, condition, reaction_time, rating]
            reaction_start = 0
            reaction_end = 0
            while not re.search(r'Rate for product', lines[index]):
                print(lines[index])
                cur_product = [user_id, "Phase2"]  # user_id, phase

                if re.search(r'End Phase 2', lines[index]):
                    break

                condition = get_condition(lines[index])
                if condition == "NoDisplay":
                    if re.search(r'Description', lines[index]):
                        breaks = re.split(r'[\]\[]', lines[index])
                        reaction_start = float(breaks[3])
                        index += 1
                    if re.search(r'Rating', lines[index]):
                        breaks = re.split(r'[\]\[]', lines[index])
                        reaction_end = float(breaks[3])
                        cur_product.append(condition)
                        index += 1

                elif condition == "SomeDisplay":
                    if re.search(r'Description', lines[index]):
                        index += 1
                    if re.search(r'Distribution', lines[index]):
                        breaks = re.split(r'[\]\[]', lines[index])
                        reaction_start = float(breaks[3])
                        index += 1
                    if re.search(r'Rating', lines[index]):
                        breaks = re.split(r'[\]\[]', lines[index])
                        reaction_end = float(breaks[3])
                        cur_product.append(condition)
                        index += 1

                elif condition == "FullDisplay":
                    if re.search(r'Description', lines[index]):
                        index += 1
                    if re.search(r'Distribution', lines[index]):
                        index += 1
                    if re.search(r'Review', lines[index]):
                        breaks = re.split(r'[\]\[]', lines[index])
                        reaction_start = float(breaks[3])
                        index += 1
                    if re.search(r'Rating', lines[index]):
                        breaks = re.split(r'[\]\[]', lines[index])
                        reaction_end = float(breaks[3])
                        cur_product.append(condition)
                        index += 1

                cur_product.append(reaction_end - reaction_start)  # reaction_time

                if re.search(r'Rate for product', lines[index]):
                    digits = [int(s) for s in lines[index].split() if s.isdigit()]
                    cur_product.append(digits[0])  # product_id
                    cur_product.append(digits[1])  # rating
                    index += 1

                data.append(cur_product)
                cur_product = []

    return data


data = []
for user_id in range(1, 31):
    file_path = str(user_id) + ".txt"
    data.extend(parse_file(file_path, user_id))

df = pd.DataFrame(data, columns=['user_id', 'phase', 'condition', 'reaction_time', 'product_id', 'rating'])
df.to_csv(r'output.csv', index=False)
