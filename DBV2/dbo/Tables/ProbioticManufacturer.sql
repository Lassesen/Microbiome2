CREATE TABLE [dbo].[ProbioticManufacturer] (
    [ProbioticManufacturerId] INT          IDENTITY (1, 1) NOT NULL,
    [ProbioticManufacturer]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ProbioticManufacturer] PRIMARY KEY CLUSTERED ([ProbioticManufacturerId] ASC)
);

