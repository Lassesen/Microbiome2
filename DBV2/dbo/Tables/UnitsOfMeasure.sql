CREATE TABLE [dbo].[UnitsOfMeasure] (
    [UOMId]         INT          IDENTITY (1, 1) NOT NULL,
    [UnitOfMeasure] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_UnitsOfMeasure] PRIMARY KEY CLUSTERED ([UOMId] ASC)
);

