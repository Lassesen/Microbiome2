CREATE TABLE [dbo].[ModifierClassification] (
    [ModifierId]     INT NOT NULL,
    [ModifierTypeId] INT NOT NULL,
    CONSTRAINT [PK_ModifierClassification] PRIMARY KEY CLUSTERED ([ModifierId] ASC, [ModifierTypeId] ASC),
    CONSTRAINT [FK_ModifierClassification_Modifiers] FOREIGN KEY ([ModifierId]) REFERENCES [dbo].[Modifiers] ([ModifierId]),
    CONSTRAINT [FK_ModifierClassification_ModifierType] FOREIGN KEY ([ModifierTypeId]) REFERENCES [dbo].[ModifierType] ([ModifierTypeId])
);

