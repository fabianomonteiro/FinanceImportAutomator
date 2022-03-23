CREATE TABLE [dbo].[Transaction] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [Date]            DATE         NOT NULL,
    [Description]     VARCHAR (50) NOT NULL,
    [Amount]          MONEY        NOT NULL,
    [TransactionType] VARCHAR (50) NOT NULL,
    [AccountName]     VARCHAR (50) NOT NULL,
    [Category]        VARCHAR (50) NULL,
    CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED ([Id] ASC)
);

