CREATE TABLE Users(
 Id INT PRIMARY KEY IDENTITY NOT NULL,
 FirstName NVARCHAR(50) NOT NULL,
 LastName NVARCHAR(50) NOT NULL,
 Email NVARCHAR(80) UNIQUE NOT NULL ,
 PasswordHash VARCHAR(250) NOT NULL,
 PhonNumber VARCHAR(20),
 AvatarUrl VARCHAR(200),
 Roles INT NOT NULL,
 CreatedAt DATE,
 IsActive BIT
 );

 CREATE TABLE RefreshToken(
 Id INT NOT NULL PRIMARY KEY IDENTITY,
 UserId INT NOT NULL,
 IsRevoked BIT NOT NULL,
 Value VARCHAR(100),
 ExpiryDate DATE

 FOREIGN KEY(UserId) REFERENCES  Users(Id)
);

EXEC sp_addextendedproperty 
    'MS_Description', 
    'Integer representing user role: 1 = Customer, 2 = Staff, 3 = Manager, 4 = Admin', 
    'SCHEMA', 'dbo', 
    'TABLE', 'Users', 
    'COLUMN', 'Roles';

-- Insert sample users
INSERT INTO Users (FirstName, LastName, Email, PasswordHash, PhonNumber, AvatarUrl, Roles, CreatedAt, IsActive)
VALUES 
('John', 'Doe', 'admin@email.com', 'hashedpassword1', '123456', 'https://example.com/avatar1.png', 4, '2025-08-30', 1),
('Jane', 'Smith', 'jane.smith@example.com', 'hashedpassword2', '0987654321', 'https://example.com/avatar2.png', 2, '2025-08-29', 1),
('Alice', 'Johnson', 'alice.johnson@example.com', 'hashedpassword3', NULL, NULL, 1, '2025-08-28', 1);

-- Insert sample refresh tokens
INSERT INTO RefreshToken (UserId, IsRevoked, Value, ExpiryDate)
VALUES
(1, 0, 'tokenvalue1', '2025-09-30'),
(1, 1, 'tokenvalue2', '2025-08-31'),
(2, 0, 'tokenvalue3', '2025-09-15'),
(3, 0, 'tokenvalue4', '2025-10-01');
