-- 6 haneli kod ve ek güvenlik için PasswordResetTokens tablosuna kolonlar
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PasswordResetTokens') AND name = 'CreatedAt')
    ALTER TABLE PasswordResetTokens ADD CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE();

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PasswordResetTokens') AND name = 'FailedAttempts')
    ALTER TABLE PasswordResetTokens ADD FailedAttempts int NOT NULL DEFAULT 0;
