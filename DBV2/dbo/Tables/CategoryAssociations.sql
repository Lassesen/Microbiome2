CREATE TABLE [dbo].[CategoryAssociations] (
    [CategoriIdA] INT NOT NULL,
    [CategoryIdB] INT NOT NULL,
    CONSTRAINT [PK_CategoryAssociations] PRIMARY KEY CLUSTERED ([CategoriIdA] ASC, [CategoryIdB] ASC),
    CONSTRAINT [FK_CategoryAssociations_CategoryReference] FOREIGN KEY ([CategoriIdA]) REFERENCES [dbo].[CategoryReference] ([CategoryId]),
    CONSTRAINT [FK_CategoryAssociations_CategoryReference1] FOREIGN KEY ([CategoryIdB]) REFERENCES [dbo].[CategoryReference] ([CategoryId])
);

