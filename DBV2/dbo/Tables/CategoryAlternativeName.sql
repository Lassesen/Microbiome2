CREATE TABLE [dbo].[CategoryAlternativeName] (
    [CategoryId]      INT           NOT NULL,
    [AlternativeName] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_CategoryAlternativeName] PRIMARY KEY CLUSTERED ([AlternativeName] ASC),
    CONSTRAINT [FK_CategoryAlternativeName_CategoryReference] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[CategoryReference] ([CategoryId])
);

