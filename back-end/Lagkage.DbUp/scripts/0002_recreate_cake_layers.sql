DROP TABLE cakelayers;

CREATE TABLE IF NOT EXISTS cake_layers(
    id varchar primary key,
    name varchar not null,
    description varchar not null,
    recipe_url varchar,
    possible_layers varchar[] not null);