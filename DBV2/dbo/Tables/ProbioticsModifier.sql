CREATE TABLE [dbo].[ProbioticsModifier] (
    [ModifierId]         INT NOT NULL,
    [ProbioticProductId] INT NOT NULL,
    CONSTRAINT [PK_ProbioticsModifier] PRIMARY KEY CLUSTERED ([ModifierId] ASC),
    CONSTRAINT [FK_ProbioticsModifier_ProbioticProducts] FOREIGN KEY ([ProbioticProductId]) REFERENCES [dbo].[ProbioticProducts] ([ProbioticProductId])
);

