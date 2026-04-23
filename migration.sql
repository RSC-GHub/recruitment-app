BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Applicants]') AND [c].[name] = N'ExpectedSalary');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Applicants] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Applicants] ALTER COLUMN [ExpectedSalary] decimal(18,2) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260423080654_makeEcpectedSalaryOptional', N'8.0.21');
GO

COMMIT;
GO

