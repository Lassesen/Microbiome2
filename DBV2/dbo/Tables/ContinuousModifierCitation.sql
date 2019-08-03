CREATE TABLE [dbo].[ContinuousModifierCitation] (
    [ContinuousId]      INT           NOT NULL,
    [CitationId]        INT           NOT NULL,
    [ModifierId]        INT           NOT NULL,
    [DirectionOfImpact] INT           CONSTRAINT [DF_ContinuousModifierCitation_DirectionOfImpact] DEFAULT ((0)) NOT NULL,
    [AmountOfImpact]    FLOAT (53)    NOT NULL,
    [UsageInformation]  VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ContinuousModifierCitation] PRIMARY KEY CLUSTERED ([ContinuousId] ASC, [CitationId] ASC, [ModifierId] ASC),
    CONSTRAINT [FK_ContinuousModifierCitation_Citation] FOREIGN KEY ([CitationId]) REFERENCES [dbo].[Citation] ([CitationId]),
    CONSTRAINT [FK_ContinuousModifierCitation_ContinuousReference] FOREIGN KEY ([ContinuousId]) REFERENCES [dbo].[ContinuousReference] ([ContinuousId]),
    CONSTRAINT [FK_ContinuousModifierCitation_Modifiers] FOREIGN KEY ([ModifierId]) REFERENCES [dbo].[Modifiers] ([ModifierId])
);

