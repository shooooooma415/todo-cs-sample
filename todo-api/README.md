# Todo API

Azure SQL Database を使用した Todo アプリケーションの API です。

## セットアップ

### 1. 設定ファイルの準備

`appsettings.Example.json`をコピーして`appsettings.json`を作成し、実際の接続文字列を設定してください：

```bash
cp appsettings.Example.json appsettings.json
```

### 2. データベースの準備

Azure SQL Database で以下の SQL スクリプトを実行してください：

```sql
-- Scripts/CreateTable.sql の内容を実行
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

-- インデックスの作成
CREATE INDEX IX_Todos_Status ON Todos(Status);
CREATE INDEX IX_Todos_CreatedAt ON Todos(CreatedAt);
CREATE INDEX IX_Todos_Priority ON Todos(Priority);
```

### 3. アプリケーションの起動

```bash
# Dockerで起動
docker compose up --build

# またはローカルで起動
dotnet run
```

## API エンドポイント

### POST /api/todo

新しい Todo を作成します。

**リクエスト例：**

```json
{
  "title": "新しいタスク",
  "description": "タスクの詳細説明",
  "due_date": "2025-08-01T12:00:00Z",
  "priority": "medium"
}
```

**レスポンス例：**

```json
{
  "id": "uuid-3",
  "title": "新しいタスク",
  "description": "タスクの詳細説明",
  "status": "pending",
  "priority": "medium",
  "dueDate": "2025-08-01T12:00:00Z",
  "createdAt": "2025-07-27T09:30:00Z",
  "updatedAt": "2025-07-27T09:30:00Z"
}
```

### GET /api/todo/{id}

指定された ID の Todo を取得します。

### GET /api/todo

すべての Todo を取得します。

### GET /api/todo?status=pending

指定されたステータスの Todo を取得します。

## アクセス

- アプリケーション: http://localhost:8080
- Swagger UI: http://localhost:8080/swagger

## セキュリティ

- `appsettings.json`は`.gitignore`に含まれているため、接続文字列が Git にコミットされることはありません
- 本番環境では、環境変数や Azure Key Vault を使用して接続文字列を管理することを推奨します
