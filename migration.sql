BEGIN TRANSACTION;
GO

ALTER TABLE [Applicants] ADD [LeavingReason] nvarchar(max) NULL;
GO

ALTER TABLE [Applicants] ADD [Relatives] nvarchar(max) NULL;
GO

ALTER TABLE [Applicants] ADD [TotalExperience] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260423130304_UpdateOnApplicantForm', N'8.0.21');
GO

COMMIT;
GO

