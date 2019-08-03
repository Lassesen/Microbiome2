CREATE TABLE [dbo].[UnitOfMeasureConversion] (
    [FromUomId]    INT        NOT NULL,
    [ToUomId]      INT        NOT NULL,
    [ConversionNo] FLOAT (53) NOT NULL,
    CONSTRAINT [PK_UnitOfMeasureConversion] PRIMARY KEY CLUSTERED ([FromUomId] ASC, [ToUomId] ASC),
    CONSTRAINT [FK_UnitOfMeasureConversion_UnitsOfMeasure] FOREIGN KEY ([FromUomId]) REFERENCES [dbo].[UnitsOfMeasure] ([UOMId]),
    CONSTRAINT [FK_UnitOfMeasureConversion_UnitsOfMeasure1] FOREIGN KEY ([ToUomId]) REFERENCES [dbo].[UnitsOfMeasure] ([UOMId])
);

