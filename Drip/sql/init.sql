-- Create tables for Drip database

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT NOW()
);

CREATE TABLE expenses (
    id SERIAL PRIMARY KEY,
    user_id INT REFERENCES users(id),
    amount DECIMAL(10,2) NOT NULL,
    merchant VARCHAR(200) NOT NULL,
    category_id INT REFERENCES categories(id),
    payment_method VARCHAR(50),
    description TEXT,
    expense_date DATE NOT NULL DEFAULT CURRENT_DATE,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

-- Seed categories
INSERT INTO categories (name) VALUES 
    ('Food Delivery'),
    ('Cafeteria'),
    ('Grocery'),
    ('Shopping'),
    ('Transport'),
    ('Entertainment'),
    ('Bills');
