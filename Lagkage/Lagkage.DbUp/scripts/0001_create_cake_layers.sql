CREATE TABLE IF NOT EXISTS cakelayers(
    name varchar primary key,
    description varchar not null,
    recipe_url varchar,
    possible_layers varchar[] not null);