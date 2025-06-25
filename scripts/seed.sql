-- Insert test URL
INSERT INTO Urls (LongUrl, ShortCode, SecretCode, CreatedAt)
VALUES ('https://progress.com', 'uqzsre', '1ccd40a7f6be5c1da48d', GETDATE());

-- Insert test visits
INSERT INTO Visits (UrlId, IpAddress, VisitedAt)
VALUES
    (1, '1.2.3.4', GETDATE()),
    (1, '1.2.3.4', GETDATE()),
    (1, '5.6.7.8', GETDATE()),
    (1, '9.9.9.9', GETDATE()),
    (1, '1.2.3.4', GETDATE()),
    (1, '5.6.7.8', GETDATE()),
    (1, '1.2.3.4', GETDATE()),
    (1, '9.9.9.9', GETDATE()),
    (1, '1.2.3.4', GETDATE()),
    (1, '9.9.9.9', GETDATE());

-- Insert daily uniques
INSERT INTO DailyUniques (UrlId, IpAddress, Date)
VALUES
    (1, '1.2.3.4', CAST(GETDATE() AS DATE)),
    (1, '5.6.7.8', CAST(GETDATE() AS DATE)),
    (1, '9.9.9.9', CAST(GETDATE() AS DATE)),
    (1, '1.2.3.4', CAST(GETDATE()-1 AS DATE)),
    (1, '5.6.7.8', CAST(GETDATE()-1 AS DATE)),
    (1, '1.2.3.4', CAST(GETDATE()-2 AS DATE));