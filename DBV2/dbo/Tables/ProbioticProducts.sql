CREATE TABLE [dbo].[ProbioticProducts] (
    [ProbioticProductId]      INT            IDENTITY (1, 1) NOT NULL,
    [ProbioticManufacturerId] INT            NOT NULL,
    [ProbioticName]           NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_ProbioticProduct] PRIMARY KEY CLUSTERED ([ProbioticProductId] ASC),
    CONSTRAINT [FK_ProbioticProduct_ProbioticManufacturer] FOREIGN KEY ([ProbioticManufacturerId]) REFERENCES [dbo].[ProbioticManufacturer] ([ProbioticManufacturerId])
);

