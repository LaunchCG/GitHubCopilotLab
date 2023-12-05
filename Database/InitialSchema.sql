CREATE TABLE IF NOT EXISTS profile (
    profile_id INT GENERATED ALWAYS AS IDENTITY,
    email VARCHAR NOT NULL UNIQUE,
    password VARCHAR,
    name VARCHAR NOT NULL,
    account_type VARCHAR NOT NULL,
    description VARCHAR NOT NULL,
    PRIMARY KEY (profile_id)
);

CREATE TABLE IF NOT EXISTS account (
    account_number INT GENERATED ALWAYS AS IDENTITY,
    profile_id INT NOT NULL UNIQUE,
    description VARCHAR NOT NULL,
    balance NUMERIC NOT NULL,
    PRIMARY KEY (account_number),
    CONSTRAINT fk_account_profile
        FOREIGN KEY(profile_id)
            REFERENCES profile(profile_id)
            ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS holding (
    account_number INT NOT NULL,
    symbol VARCHAR NOT NULL,
    shares NUMERIC NOT NULL,
    PRIMARY KEY (account_number, symbol),
    CONSTRAINT fk_holding_account
        FOREIGN KEY(account_number)
            REFERENCES account(account_number)
            ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS operation (
    operation_id INT GENERATED ALWAYS AS IDENTITY,
    operation_type INT NOT NULL,
    account_number INT NOT NULL,
    symbol VARCHAR,
    shares NUMERIC,
    amount NUMERIC,
    transact_date DATE NOT NULL DEFAULT now(),
    PRIMARY KEY (operation_id),
    CONSTRAINT fk_holding_account
        FOREIGN KEY(account_number)
            REFERENCES account(account_number)
            ON DELETE CASCADE
);

