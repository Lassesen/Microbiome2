CREATE TABLE [dbo].[ModifierComposition] (
    [ModifierId]          INT        NOT NULL,
    [ComponentModifierId] INT        NOT NULL,
    [UomId]               INT        NOT NULL,
    [Componentamount]     FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ModifierComposition] PRIMARY KEY CLUSTERED ([ModifierId] ASC, [ComponentModifierId] ASC, [UomId] ASC),
    CONSTRAINT [FK_ModifierComposition_Modifiers] FOREIGN KEY ([ModifierId]) REFERENCES [dbo].[Modifiers] ([ModifierId]),
    CONSTRAINT [FK_ModifierComposition_Modifiers1] FOREIGN KEY ([ComponentModifierId]) REFERENCES [dbo].[Modifiers] ([ModifierId]),
    CONSTRAINT [FK_ModifierComposition_UnitsOfMeasure] FOREIGN KEY ([UomId]) REFERENCES [dbo].[UnitsOfMeasure] ([UOMId])
);

