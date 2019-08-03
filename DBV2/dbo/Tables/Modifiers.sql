CREATE TABLE [dbo].[Modifiers] (
    [ModifierId]   INT           IDENTITY (1, 1) NOT NULL,
    [ModifierName] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Modifiers] PRIMARY KEY CLUSTERED ([ModifierId] ASC),
    CONSTRAINT [FK_Modifiers_ProbioticsModifier] FOREIGN KEY ([ModifierId]) REFERENCES [dbo].[ProbioticsModifier] ([ModifierId])
);

