/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [city_id]
      ,[country_id]
      ,[name]
      ,[created_at]
      ,[updated_at]
      ,[deleted_at]
  FROM [CI_Platform].[dbo].[city] where country_id=25

  INSERT INTO city (country_id, name, created_at)
VALUES 
    (24, 'Mexico City', GETDATE()),
    (24, 'Guadalajara', GETDATE()),
    (24, 'Monterrey', GETDATE()),
    (24, 'Puebla', GETDATE()),
    (24, 'Tijuana', GETDATE()),
    (24, 'Ciudad Juarez', GETDATE()),
    (24, 'Leon', GETDATE()),
    (24, 'Toluca', GETDATE()),
    (24, 'Cancun', GETDATE()),
    (24, 'Merida', GETDATE()),
    (24, 'Acapulco', GETDATE()),
    (24, 'Veracruz', GETDATE()),
    (24, 'Villahermosa', GETDATE()),
    (24, 'Morelia', GETDATE()),
    (24, 'Tampico', GETDATE()),
    (24, 'Queretaro', GETDATE()),
    (24, 'Culiacan', GETDATE()),
    (24, 'Chihuahua', GETDATE()),
    (24, 'Aguascalientes', GETDATE()),
    (24, 'Durango', GETDATE());
