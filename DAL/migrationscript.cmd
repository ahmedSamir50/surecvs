dotnet ef migrations add InitialCreate --context SureCvDbSqliteContext --output-dir Migrations/SqliteMigrations --project DAL

dotnet ef database update --context SureCvDbSqliteContext --project DAL -- --Provider SQLLite