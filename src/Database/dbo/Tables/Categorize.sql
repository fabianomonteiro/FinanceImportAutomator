CREATE TABLE [dbo].[Categorize] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Category]    VARCHAR (50) NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Categorize] PRIMARY KEY CLUSTERED ([Id] ASC)
);

