CREATE TABLE IF NOT EXISTS company_info (
    symbol VARCHAR NOT NULL,
    name VARCHAR NOT NULL,
    description VARCHAR,
    create_date DATE NOT NULL,
    PRIMARY KEY (symbol)
);

CREATE TABLE IF NOT EXISTS price (
    symbol VARCHAR NOT NULL,
    close_date DATE NOT NULL,
    price NUMERIC NOT NULL,
    PRIMARY KEY (symbol, close_date)
);

CREATE TABLE IF NOT EXISTS batch_run (
    run_id INTEGER GENERATED ALWAYS AS IDENTITY,
    run_type VARCHAR NOT NULL,
    start_time TIMESTAMP NOT NULL,
    end_time TIMESTAMP NOT NULL,
    error_count INTEGER NOT NULL,
    success_count INTEGER NOT NULL,
    PRIMARY KEY (run_id)
);

CREATE TABLE IF NOT EXISTS batch_detail (
    msg_id INTEGER GENERATED ALWAYS AS IDENTITY,
    run_id INTEGER NOT NULL,
    message VARCHAR NOT NULL,
    PRIMARY KEY (msg_id),
    CONSTRAINT fk_runid
        FOREIGN KEY(run_id)
            REFERENCES batch_run(run_id)
            ON DELETE CASCADE
);
