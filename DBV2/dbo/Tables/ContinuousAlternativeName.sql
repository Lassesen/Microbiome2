CREATE TABLE [dbo].[ContinuousAlternativeName] (
    [ContinuousId]    INT           NOT NULL,
    [AlternativeName] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ContinuousAlternativeName] PRIMARY KEY CLUSTERED ([AlternativeName] ASC),
    CONSTRAINT [FK_ContinuousAlternativeName_ContinuousReference] FOREIGN KEY ([ContinuousId]) REFERENCES [dbo].[ContinuousReference] ([ContinuousId])
);

