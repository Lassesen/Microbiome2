CREATE TABLE [dbo].[ModifierAlternativeName] (
    [ModifierId]      INT           NOT NULL,
    [AlternativeName] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ModifierAlternativeName] PRIMARY KEY CLUSTERED ([AlternativeName] ASC),
    CONSTRAINT [FK_ModifierAlternativeName_Modifiers] FOREIGN KEY ([ModifierId]) REFERENCES [dbo].[Modifiers] ([ModifierId])
);

