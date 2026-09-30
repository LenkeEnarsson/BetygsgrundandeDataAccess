USE [sql-attractions];
GO

--create schemas
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'gstusr')
    EXEC('CREATE SCHEMA gstusr');
GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'usr')
    EXEC('CREATE SCHEMA usr');
GO

--View for database info shown to guest
CREATE OR ALTER VIEW gstusr.vwInfoDb AS
    SELECT (SELECT COUNT(*) FROM dbo.Users) as nrUsers, 
        (SELECT COUNT(*) FROM suprusr.Attractions a JOIN usr.Reviews r ON a.AttractionId = r.AttractionId) as nrAttractionsWithReviews,
        (SELECT COUNT(*) FROM suprusr.Attractions a FULL OUTER JOIN usr.Reviews r ON a.AttractionId = r.AttractionId WHERE r.AttractionId IS NULL) as nrAttractionsWithoutReviews,
        (SELECT COUNT(*) FROM suprusr.Attractions) as nrTotalAttractions,
        (SELECT COUNT(*) FROM suprusr.Categories) as nrCategories, 
        (SELECT COUNT(*) FROM suprusr.Countries) as nrCountries,
        (SELECT COUNT(*) FROM suprusr.Cities) as nrCities,
        (SELECT COUNT(*) FROM usr.Reviews) as nrReviews;
GO

-- SP Delete Seed
CREATE OR ALTER PROCEDURE dbo.spDeleteSeeded
    @seededParam BIT = 1, --true

    @nrAttractionsAffected INT OUTPUT,
    @nrCitiesAffected INT OUTPUT,
    @nrCountriesAffected INT OUTPUT,
    @nrUsersAffected INT OUTPUT,
    @nrReviewsAffected INT OUTPUT
AS
BEGIN TRY

    SET NOCOUNT ON;

    SELECT  @nrAttractionsAffected = COUNT(*) FROM suprusr.Attractions WHERE Seeded = @seededParam;
    SELECT  @nrCitiesAffected = COUNT(*) FROM suprusr.Cities WHERE Seeded = @seededParam;
    SELECT  @nrCountriesAffected = COUNT(*) FROM suprusr.Countries WHERE Seeded = @seededParam;
    SELECT  @nrUsersAffected = COUNT(*) FROM dbo.Users WHERE Seeded = @seededParam;
    SELECT  @nrReviewsAffected = COUNT(*) FROM usr.Reviews WHERE Seeded = @seededParam;

    DELETE FROM suprusr.Attractions WHERE Seeded = @seededParam;
    DELETE FROM suprusr.Cities WHERE Seeded = @seededParam;
    DELETE FROM suprusr.Countries WHERE Seeded = @seededParam;
    DELETE FROM dbo.Users WHERE Seeded = @seededParam;
    DELETE FROM usr.Reviews WHERE Seeded = @seededParam;

    SELECT * FROM gstusr.VwInfoDb;
END TRY

    BEGIN CATCH
    THROW 99999, 'Error occurred while deleting seeded data.', 1;
    END CATCH
GO
