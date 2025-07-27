-- Todosテーブルの作成
CREATE TABLE Todos (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'pending',
    Priority NVARCHAR(50) NOT NULL DEFAULT 'medium',
    DueDate DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- インデックスの作成（パフォーマンス向上のため）
CREATE INDEX IX_Todos_Status ON Todos(Status);
CREATE INDEX IX_Todos_CreatedAt ON Todos(CreatedAt);
CREATE INDEX IX_Todos_Priority ON Todos(Priority); 